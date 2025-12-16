using System;
using System.Collections.Generic;
using System.Text;
using RollLabelProdPack.Library.Entities;

namespace RollLabelProdPack.Library.Strategies
{
    /// <summary>
    /// Label format strategy for Rockline customer (C1020)
    /// CSV Format: PurchaseOrder,CustomerPartNumber,ItemNumber,RollNumber,Width,LotNumber,Quantity,UOM,SSCC
    /// </summary>
    public class RocklineLabelFormatStrategy : ILabelFormatStrategy
    {
        /// <summary>
        /// Generates Rockline-specific CSV content for label printing
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
                // Convert width from MM to inches (25.4 MM per inch)
                decimal widthInInches = Math.Round(order.WidthInMM / 25.4m, 0);

                // Format: PurchaseOrder,CustomerPartNumber,ItemNumber,RollNumber,Width,LotNumber,Quantity,UOM,SSCC
                sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                    order.PONumber,                                    // PurchaseOrder
                    order.IRMS,                                        // CustomerPartNumber
                    roll.Scrap ? order.ScrapItem : roll.ItemCode,     // ItemNumber
                    roll.RollNo,                                       // RollNumber
                    widthInInches,                                     // Width (in inches)
                    order.YJNOrder,                                    // LotNumber (YJN Order)
                    roll.Quantity,                                     // Quantity (in SY for Rockline)
                    roll.UOM,                                          // UOM (typically "SY")
                    roll.SSCC                                          // SSCC
                );
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets Rockline CSV header format
        /// </summary>
        /// <returns>CSV header string</returns>
        public string GetCSVHeaders()
        {
            return "PurchaseOrder,CustomerPartNumber,ItemNumber,RollNumber,Width,LotNumber,Quantity,UOM,SSCC";
        }
    }
}
