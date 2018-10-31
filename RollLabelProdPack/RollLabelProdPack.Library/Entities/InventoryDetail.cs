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
    }
}
