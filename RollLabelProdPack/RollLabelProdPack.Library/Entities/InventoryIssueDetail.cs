using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Entities
{
    public class InventoryIssueDetail:InventoryDetail
    {
        public double PlannedIssueQty { get; set; }
        public int BaseEntry { get; set; }
        public int BaseLine { get; set; }
        public double ShortQty { get; set; }
        

    }
}
