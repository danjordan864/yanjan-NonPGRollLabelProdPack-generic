using System;
using RollLabelProdPack.Library.Utility;

namespace RollLabelProdPack.Library.Strategies
{
    /// <summary>
    /// Factory for creating appropriate label format strategy based on customer
    /// </summary>
    public static class LabelFormatStrategyFactory
    {
        /// <summary>
        /// Gets the appropriate label format strategy for the given customer ID
        /// </summary>
        /// <param name="customerID">Customer ID (e.g., "C1007", "C1020")</param>
        /// <returns>Label format strategy instance</returns>
        public static ILabelFormatStrategy GetStrategy(string customerID)
        {
            if (string.IsNullOrEmpty(customerID))
            {
                return new GenericLabelFormatStrategy();
            }

            try
            {
                // Get customer IDs from configuration
                string medlineID = AppUtility.GetMedlineCustomerID();
                string rocklineID = AppUtility.GetRocklineCustomerID();

                // Match customer ID to strategy
                if (customerID.Equals(medlineID, StringComparison.OrdinalIgnoreCase))
                {
                    return new MedlineLabelFormatStrategy();
                }

                if (customerID.Equals(rocklineID, StringComparison.OrdinalIgnoreCase))
                {
                    return new RocklineLabelFormatStrategy();
                }

                // Default to generic strategy for unknown customers
                return new GenericLabelFormatStrategy();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetStrategy: {ex.Message}");
                return new GenericLabelFormatStrategy();
            }
        }
    }
}
