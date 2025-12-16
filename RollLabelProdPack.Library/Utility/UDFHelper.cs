using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace RollLabelProdPack.Library.Utility
{
    /// <summary>
    /// Label type enumeration for UDF-based label format resolution
    /// </summary>
    public enum LabelType
    {
        Roll,
        Bundle,
        Core
    }

    /// <summary>
    /// Helper class for reading Production Order UDFs with fallback logic
    /// </summary>
    public static class UDFHelper
    {
        #region Public Methods

        /// <summary>
        /// Gets the label format file path with fallback chain:
        /// 1. Try production order UDF (U_SII_RollLabel, U_SII_BunLabel, U_SII_CoreLabel)
        /// 2. If empty/invalid, fall back to customer-specific config
        /// 3. If still empty, fall back to generic config default
        /// </summary>
        /// <param name="docNum">Production order document number</param>
        /// <param name="labelType">Type of label (Roll, Bundle, Core)</param>
        /// <param name="customerID">Customer ID for fallback lookup (e.g., "C1007")</param>
        /// <returns>Full path to .btw label format file</returns>
        public static string GetLabelFormatPath(int docNum, LabelType labelType, string customerID = null)
        {
            try
            {
                // Step 1: Try to read from production order UDF
                string udfFieldName = GetUDFFieldName(labelType);
                string udfValue = GetProductionOrderUDF(docNum, udfFieldName);

                // Step 2: If UDF is populated and file exists, use it
                if (!string.IsNullOrWhiteSpace(udfValue))
                {
                    // Handle both full paths and just filenames
                    string fullPath = udfValue;
                    if (!Path.IsPathRooted(udfValue))
                    {
                        // If just a filename, combine with Produmex BTLabelFormats directory
                        fullPath = Path.Combine(@"C:\Produmex\BTLabelFormats", udfValue);
                    }

                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }

                // Step 3: Fall back to customer-specific config
                if (!string.IsNullOrEmpty(customerID))
                {
                    string customerFormat = GetCustomerDefaultFormat(labelType, customerID);
                    if (!string.IsNullOrEmpty(customerFormat) && File.Exists(customerFormat))
                    {
                        return customerFormat;
                    }
                }

                // Step 4: Fall back to generic config
                string genericFormat = GetGenericDefaultFormat(labelType);
                if (!string.IsNullOrEmpty(genericFormat))
                {
                    return genericFormat;
                }

                // If all fallbacks fail, return empty string (caller should handle)
                return string.Empty;
            }
            catch (Exception ex)
            {
                // Log error but don't crash - return fallback
                System.Diagnostics.Debug.WriteLine($"Error in GetLabelFormatPath: {ex.Message}");
                return GetGenericDefaultFormat(labelType) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the number of label copies to print from UDF
        /// Returns default value of 1 if UDF is not set
        /// </summary>
        /// <param name="docNum">Production order document number</param>
        /// <param name="labelType">Type of label (Roll, Bundle, Core)</param>
        /// <returns>Number of copies to print (default: 1)</returns>
        public static int GetLabelCopies(int docNum, LabelType labelType)
        {
            try
            {
                string udfFieldName = GetLabelCopiesUDFFieldName(labelType);
                int copies = GetProductionOrderUDFInt(docNum, udfFieldName, 1);
                return copies > 0 ? copies : 1; // Ensure at least 1 copy
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetLabelCopies: {ex.Message}");
                return 1; // Default to 1 copy on error
            }
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Gets the UDF field name for label format based on label type
        /// </summary>
        private static string GetUDFFieldName(LabelType labelType)
        {
            switch (labelType)
            {
                case LabelType.Roll:
                    return "U_SII_RollLabel";
                case LabelType.Bundle:
                    return "U_SII_BunLabel";
                case LabelType.Core:
                    return "U_SII_CoreLabel";
                default:
                    return "U_SII_RollLabel";
            }
        }

        /// <summary>
        /// Gets the UDF field name for label copies based on label type
        /// </summary>
        private static string GetLabelCopiesUDFFieldName(LabelType labelType)
        {
            switch (labelType)
            {
                case LabelType.Roll:
                    return "U_SII_RLabelQty";
                case LabelType.Bundle:
                    return "U_SII_BLabel";
                case LabelType.Core:
                    return "U_SII_CLabel";
                default:
                    return "U_SII_RLabelQty";
            }
        }

        /// <summary>
        /// Reads a string UDF value from production order (OWOR table)
        /// </summary>
        /// <param name="docNum">Production order document number</param>
        /// <param name="udfFieldName">UDF field name (e.g., "U_SII_RollLabel")</param>
        /// <returns>UDF value or null if not found/empty</returns>
        private static string GetProductionOrderUDF(int docNum, string udfFieldName)
        {
            try
            {
                string connectionString = AppUtility.GetSAPConnectionString();
                int timeout = AppUtility.GetSqlCommandTimeOut();

                string sql = $"SELECT {udfFieldName} FROM OWOR WHERE DocNum = @DocNum";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = timeout;
                    cmd.Parameters.AddWithValue("@DocNum", docNum);

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
                System.Diagnostics.Debug.WriteLine($"Error reading UDF {udfFieldName}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Reads a numeric UDF value from production order (OWOR table)
        /// </summary>
        /// <param name="docNum">Production order document number</param>
        /// <param name="udfFieldName">UDF field name (e.g., "U_SII_RLabelQty")</param>
        /// <param name="defaultValue">Default value if UDF is null or invalid</param>
        /// <returns>UDF value or default</returns>
        private static int GetProductionOrderUDFInt(int docNum, string udfFieldName, int defaultValue)
        {
            try
            {
                string connectionString = AppUtility.GetSAPConnectionString();
                int timeout = AppUtility.GetSqlCommandTimeOut();

                string sql = $"SELECT ISNULL({udfFieldName}, @Default) FROM OWOR WHERE DocNum = @DocNum";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = timeout;
                    cmd.Parameters.AddWithValue("@DocNum", docNum);
                    cmd.Parameters.AddWithValue("@Default", defaultValue);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        if (int.TryParse(result.ToString(), out int value))
                        {
                            return value;
                        }
                    }

                    return defaultValue;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading UDF {udfFieldName}: {ex.Message}");
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets customer-specific default format from configuration
        /// </summary>
        /// <param name="labelType">Type of label</param>
        /// <param name="customerID">Customer ID (e.g., "C1007", "C1020")</param>
        /// <returns>Label format path from config</returns>
        private static string GetCustomerDefaultFormat(LabelType labelType, string customerID)
        {
            string configKey = string.Empty;

            // Get customer IDs from config for comparison
            string medlineID = AppUtility.GetMedlineCustomerID();
            string rocklineID = AppUtility.GetRocklineCustomerID();

            // Determine customer type
            bool isMedline = customerID == medlineID;
            bool isRockline = customerID == rocklineID;

            // Map to appropriate config key
            if (isMedline)
            {
                configKey = labelType == LabelType.Roll
                    ? "MedlineDefaultRollLabelFormat"
                    : labelType == LabelType.Bundle
                        ? "MedlineDefaultPackLabelFormat"
                        : "MedlineDefaultCoreLabelFormat"; // May not exist yet
            }
            else if (isRockline)
            {
                configKey = labelType == LabelType.Roll
                    ? "RocklineDefaultRollLabelFormat"
                    : labelType == LabelType.Bundle
                        ? "RocklineDefaultPackLabelFormat"
                        : "RocklineDefaultCoreLabelFormat"; // May not exist yet
            }

            if (string.IsNullOrEmpty(configKey))
            {
                return null;
            }

            return ConfigurationManager.AppSettings[configKey];
        }

        /// <summary>
        /// Gets generic default format from configuration
        /// </summary>
        /// <param name="labelType">Type of label</param>
        /// <returns>Generic label format path from config</returns>
        private static string GetGenericDefaultFormat(LabelType labelType)
        {
            string configKey = string.Empty;

            switch (labelType)
            {
                case LabelType.Roll:
                    configKey = "GenericDefaultRollLabelFormat";
                    break;
                case LabelType.Bundle:
                    configKey = "GenericDefaultBundleLabelFormat";
                    break;
                case LabelType.Core:
                    configKey = "GenericDefaultCoreLabelFormat";
                    break;
            }

            return ConfigurationManager.AppSettings[configKey];
        }

        #endregion
    }
}
