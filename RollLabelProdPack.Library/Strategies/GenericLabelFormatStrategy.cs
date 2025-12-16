using System;
using System.Collections.Generic;
using System.Text;
using RollLabelProdPack.Library.Entities;

namespace RollLabelProdPack.Library.Strategies
{
    /// <summary>
    /// Generic label format strategy for customers without specific format requirements
    /// Comprehensive CSV format that includes most common fields
    /// </summary>
    public class GenericLabelFormatStrategy : ILabelFormatStrategy
    {
        /// <summary>
        /// Generates generic CSV content for label printing
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
                // Format: ItemCode,ItemName,RollNo,LotNo,Quantity,UOM,NetWeight,SSCC,PONumber
                sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                    roll.Scrap ? order.ScrapItem : roll.ItemCode,     // ItemCode
                    roll.Scrap ? "Scrap" : roll.ItemName,             // ItemName
                    roll.RollNo,                                       // RollNo
                    roll.YJNOrder,                                     // LotNo (YJN Order)
                    roll.Quantity,                                     // Quantity (in primary UOM)
                    roll.UOM,                                          // UOM (SM, SY, KG, etc.)
                    roll.NetKg,                                        // NetWeight (in kg)
                    roll.SSCC,                                         // SSCC
                    roll.PONumber                                      // PONumber
                );
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets generic CSV header format
        /// </summary>
        /// <returns>CSV header string</returns>
        public string GetCSVHeaders()
        {
            return "ItemCode,ItemName,RollNo,LotNo,Quantity,UOM,NetWeight,SSCC,PONumber";
        }
    }
}
