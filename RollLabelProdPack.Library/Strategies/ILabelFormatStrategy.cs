using System.Collections.Generic;
using RollLabelProdPack.Library.Entities;

namespace RollLabelProdPack.Library.Strategies
{
    /// <summary>
    /// Strategy interface for generating label CSV content
    /// Allows different customers to have different CSV formats
    /// </summary>
    public interface ILabelFormatStrategy
    {
        /// <summary>
        /// Generates CSV content for BarTender label printing
        /// </summary>
        /// <param name="rolls">List of rolls to include in the CSV</param>
        /// <param name="order">Production order data</param>
        /// <returns>CSV content string including headers and data rows</returns>
        string GenerateCSV(List<Roll> rolls, RollLabelData order);

        /// <summary>
        /// Gets the CSV header row for this format
        /// </summary>
        /// <returns>CSV header string</returns>
        string GetCSVHeaders();
    }
}
