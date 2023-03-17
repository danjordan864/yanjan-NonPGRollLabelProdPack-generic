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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("InventoryIssueDetail");
            sb.AppendLine("--------------------");
            sb.AppendLine($"PlannedIssueQty = {PlannedIssueQty}");
            sb.AppendLine($"BaseEntry = {BaseEntry}");
            sb.AppendLine($"BaseLine = {BaseLine}");
            sb.AppendLine($"ShortQty = {ShortQty}");
            sb.Append(base.ToString());
            return sb.ToString();
        }
    }
}
