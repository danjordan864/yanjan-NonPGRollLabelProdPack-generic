using System;
using System.Data.SqlClient;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;

namespace RollLabelProdPack.Library.Services
{
    /// <summary>
    /// Service for handling unit of measure detection and quantity conversions
    /// Supports conversion between Linear Meters (LM), Square Meters (SM), and Square Yards (SY)
    /// </summary>
    public static class UnitConversionService
    {
        #region Constants

        /// <summary>
        /// Conversion factor from Square Meters to Square Yards
        /// 1 square meter = 1.19599 square yards
        /// </summary>
        private const decimal SM_TO_SY_FACTOR = 1.19599m;

        #endregion

        #region Public Methods

        /// <summary>
        /// Detects the unit of measure for a given item
        /// Priority: Item Master UOM → Customer Default → Default to SM
        /// </summary>
        /// <param name="itemCode">SAP item code</param>
        /// <param name="customerID">Optional customer ID for fallback</param>
        /// <returns>Unit of measure code (e.g., "SM", "SY", "KGS")</returns>
        public static string DetectUOM(string itemCode, string customerID = null)
        {
            try
            {
                // Strategy 1: Check item master UOM
                string itemUOM = GetItemMasterUOM(itemCode);
                if (IsValidUOM(itemUOM))
                {
                    return itemUOM.ToUpper();
                }

                // Strategy 2: Detect from customer
                if (!string.IsNullOrEmpty(customerID))
                {
                    string customerUOM = GetCustomerDefaultUOM(customerID);
                    if (!string.IsNullOrEmpty(customerUOM))
                    {
                        return customerUOM;
                    }
                }

                // Strategy 3: Default to Square Meters
                return "SM";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DetectUOM: {ex.Message}");
                return "SM"; // Safe default
            }
        }

        /// <summary>
        /// Calculates roll quantities based on linear meters input and detected UOM
        /// Updates the Roll entity with SquareMeters, Quantity, and UOM properties
        /// </summary>
        /// <param name="roll">Roll entity to update</param>
        /// <param name="uom">Unit of measure (SM, SY, KGS, etc.)</param>
        /// <param name="linearMeters">Length input in linear meters</param>
        /// <param name="widthMM">Width in millimeters</param>
        public static void CalculateRollQuantity(Roll roll, string uom, decimal linearMeters, decimal widthMM)
        {
            if (roll == null)
            {
                throw new ArgumentNullException(nameof(roll));
            }

            // Base calculation: Convert width from MM to meters and multiply by length
            // Formula: SquareMeters = (WidthMM × 0.001) × LinearMeters
            decimal baseSquareMeters = widthMM * 0.001m * linearMeters;

            // Always set SquareMeters property
            roll.SquareMeters = baseSquareMeters;
            roll.UOM = uom?.ToUpper() ?? "SM";

            // Calculate Quantity based on UOM
            switch (roll.UOM)
            {
                case "SM":
                    // Square Meters: Quantity = SquareMeters
                    roll.Quantity = baseSquareMeters;
                    break;

                case "SY":
                    // Square Yards: Convert from Square Meters
                    // Formula: SquareYards = SquareMeters × 1.19599
                    roll.Quantity = Math.Round(baseSquareMeters * SM_TO_SY_FACTOR, 2);
                    break;

                case "KGS":
                case "KG":
                    // Weight-based: Use weight (for future P&G support)
                    roll.Quantity = roll.Kgs;
                    break;

                case "M":
                case "LM":
                    // Linear Meters: Just use the linear meters value
                    roll.Quantity = linearMeters;
                    break;

                default:
                    // Unknown UOM: Default to Square Meters
                    roll.Quantity = baseSquareMeters;
                    System.Diagnostics.Debug.WriteLine($"Unknown UOM '{uom}', defaulting to SM");
                    break;
            }
        }

        /// <summary>
        /// Converts a quantity from one UOM to another
        /// </summary>
        /// <param name="quantity">Quantity to convert</param>
        /// <param name="fromUOM">Source unit of measure</param>
        /// <param name="toUOM">Target unit of measure</param>
        /// <returns>Converted quantity</returns>
        public static decimal ConvertQuantity(decimal quantity, string fromUOM, string toUOM)
        {
            if (string.IsNullOrEmpty(fromUOM) || string.IsNullOrEmpty(toUOM))
            {
                return quantity;
            }

            string from = fromUOM.ToUpper();
            string to = toUOM.ToUpper();

            // Same UOM, no conversion needed
            if (from == to)
            {
                return quantity;
            }

            // Convert SM to SY
            if (from == "SM" && to == "SY")
            {
                return Math.Round(quantity * SM_TO_SY_FACTOR, 2);
            }

            // Convert SY to SM
            if (from == "SY" && to == "SM")
            {
                return Math.Round(quantity / SM_TO_SY_FACTOR, 2);
            }

            // No conversion available
            System.Diagnostics.Debug.WriteLine($"No conversion available from {fromUOM} to {toUOM}");
            return quantity;
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Retrieves the inventory UOM from item master (OITM table)
        /// </summary>
        /// <param name="itemCode">SAP item code</param>
        /// <returns>Unit of measure code or null</returns>
        private static string GetItemMasterUOM(string itemCode)
        {
            try
            {
                string connectionString = AppUtility.GetSAPConnectionString();
                int timeout = AppUtility.GetSqlCommandTimeOut();

                string sql = "SELECT InvntryUom FROM OITM WHERE ItemCode = @ItemCode";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = timeout;
                    cmd.Parameters.AddWithValue("@ItemCode", itemCode);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        string value = result.ToString().Trim();
                        return string.IsNullOrEmpty(value) ? null : value;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading item master UOM for {itemCode}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gets default UOM based on customer ID
        /// Medline (C1007) → SM, Rockline (C1020) → SY
        /// </summary>
        /// <param name="customerID">Customer ID</param>
        /// <returns>Default UOM for customer or null</returns>
        private static string GetCustomerDefaultUOM(string customerID)
        {
            try
            {
                string medlineID = AppUtility.GetMedlineCustomerID();
                string rocklineID = AppUtility.GetRocklineCustomerID();

                if (customerID == medlineID)
                {
                    return "SM";
                }

                if (customerID == rocklineID)
                {
                    return "SY";
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting customer default UOM: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Validates if a UOM code is recognized
        /// </summary>
        /// <param name="uom">Unit of measure code</param>
        /// <returns>True if valid, false otherwise</returns>
        private static bool IsValidUOM(string uom)
        {
            if (string.IsNullOrWhiteSpace(uom))
            {
                return false;
            }

            string uomUpper = uom.ToUpper();

            // List of recognized UOMs
            string[] validUOMs = { "SM", "SY", "KGS", "KG", "M", "LM" };

            foreach (string validUOM in validUOMs)
            {
                if (uomUpper == validUOM)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
