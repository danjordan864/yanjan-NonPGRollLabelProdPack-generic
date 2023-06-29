using System;
using System.Text;

namespace RollLabelProdPack.Library.Entities
{
    /// <summary>
    /// Represents an inventory detail entity.
    /// </summary>
    public class InventoryDetail
    {
        /// <summary>
        /// Gets or sets the item code.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the item name.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the warehouse.
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// Gets or sets the storage location.
        /// </summary>
        public string StorageLocation { get; set; }

        /// <summary>
        /// Gets or sets the quality status.
        /// </summary>
        public string QualityStatus { get; set; }

        /// <summary>
        /// Gets or sets the batch.
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// Gets or sets the lot.
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// Gets or sets the unit of measure.
        /// </summary>
        public string UOM { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Gets or sets the Logistic Unit ID.
        /// </summary>
        public int LUID { get; set; }

        /// <summary>
        /// Gets or sets the Serial Shipping Container Code.
        /// </summary>
        public string SSCC { get; set; }

        /// <summary>
        /// Gets or sets the incoming date.
        /// </summary>
        public DateTime? InDate { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the item is batch controlled.
        /// </summary>
        public bool BatchControlled { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the item is a packaging material.
        /// </summary>
        public bool PackagingMtl { get; set; }

        /// <summary>
        /// Gets or sets the inventory group code.
        /// </summary>
        public int InvGrpCode { get; set; }

        /// <summary>
        /// Returns a string representation of the inventory detail.
        /// </summary>
        /// <returns>The string representation of the inventory detail.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("InventoryDetail");
            sb.AppendLine("---------------");
            sb.AppendLine($"ItemCode = {ItemCode}");
            sb.AppendLine($"ItemName = {ItemName}");
            sb.AppendLine($"Warehouse = {Warehouse}");
            sb.AppendLine($"StorageLocation = {StorageLocation}");
            sb.AppendLine($"QualityStatus = {QualityStatus}");
            sb.AppendLine($"Batch = {Batch}");
            sb.AppendLine($"Lot = {Lot}");
            sb.AppendLine($"UOM = {UOM}");
            sb.AppendLine($"Quantity = {Quantity}");
            sb.AppendLine($"LUID = {LUID}");
            sb.AppendLine($"SSCC = {SSCC}");
            sb.AppendLine($"InDate = {InDate}");
            sb.AppendLine($"BatchControlled = {BatchControlled}");
            sb.AppendLine($"PackagingMtl = {PackagingMtl}");
            sb.AppendLine($"InvGrpCode = {InvGrpCode}");
            return sb.ToString();
        }
    }
}
