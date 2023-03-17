using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Entities
{
    public class InventoryDetail
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Warehouse { get; set; }
        public string StorageLocation { get; set; }
        public string QualityStatus { get; set; }
        public string Batch { get; set; }
        public string Lot { get; set; }
        public string UOM { get; set; }
        public double Quantity { get; set; }
        public int LUID { get; set; }
        public string SSCC { get; set; }
        public DateTime? InDate { get; set; }
        public bool BatchControlled { get; set; }
        public bool PackagingMtl { get; set; }
        public int InvGrpCode { get; set; }

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
