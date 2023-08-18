using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Entities
{
    /// <summary>
    /// Represents an item entity
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Item Code
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Has Second Batch Number
        /// </summary>
        public bool HasSecondBatchNumber { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Item()
        {
            ItemCode = string.Empty;
            HasSecondBatchNumber = false;
        }

        /// <summary>
        /// Constructor with all fields
        /// </summary>
        /// <param name="itemCode">Item Code</param>
        /// <param name="hasSecondBatchNumber">True if item has a second batch number</param>
        public Item(string itemCode, bool hasSecondBatchNumber)
        {
            ItemCode = itemCode;
            HasSecondBatchNumber = hasSecondBatchNumber;
        }
    }
}
