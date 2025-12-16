using System;
using System.Collections.Generic;
using System.Text;
using RollLabelProdPack.Library.Entities;

namespace RollLabelProdPack.Library.Strategies
{
    /// <summary>
    /// Label format strategy for Medline customer (C1007)
    /// CSV Format: Item, ItemName, IRMS, LotNo, RollNo, SSCC, Qty, PONumber
    /// </summary>
    public class MedlineLabelFormatStrategy : ILabelFormatStrategy
    {
        /// <summary>
        /// Generates Medline-specific CSV content for label printing
        /// </summary>
        /// <param name="rolls">List of rolls</param>
        /// <param name="order">Production order data</param>
        /// <returns>CSV content with headers and data</returns>
        public string GenerateCSV(List<Roll> rolls, RollLabelData order)
        {
            if (rolls == null || rolls.Count == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            // Add CSV headers
            sb.AppendLine(GetCSVHeaders());

            // Add data rows
            foreach (var roll in rolls)
            {
                // Format: Item, ItemName, IRMS, LotNo, RollNo, SSCC, Qty, PONumber
                sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7}",
                    roll.Scrap ? order.ScrapItem : roll.ItemCode,     // Item
                    roll.Scrap ? "Scrap" : roll.ItemName,             // ItemName
                    roll.IRMS,                                         // IRMS
                    roll.YJNOrder,                                     // LotNo (YJN Order)
                    roll.RollNo,                                       // RollNo
                    roll.SSCC,                                         // SSCC
                    roll.SquareMeters,                                 // Qty (in Square Meters)
                    roll.PONumber                                      // PONumber
                );
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets Medline CSV header format
        /// </summary>
        /// <returns>CSV header string</returns>
        public string GetCSVHeaders()
        {
            return "Item, ItemName, IRMS, LotNo, RollNo, SSCC, Qty, PONumber";
        }
    }
}
