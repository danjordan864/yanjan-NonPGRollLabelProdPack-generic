using System.Text;

namespace RollLabelProdPack.Library.Entities
{
    /// <summary>
    /// Represents an inventory issue detail entity.
    /// </summary>
    public class InventoryIssueDetail : InventoryDetail
    {
        /// <summary>
        /// Gets or sets the planned issue quantity.
        /// </summary>
        public double PlannedIssueQty { get; set; }

        /// <summary>
        /// Gets or sets the base entry.
        /// </summary>
        public int BaseEntry { get; set; }

        /// <summary>
        /// Gets or sets the base line.
        /// </summary>
        public int BaseLine { get; set; }

        /// <summary>
        /// Gets or sets the short quantity.
        /// </summary>
        public double ShortQty { get; set; }

        /// <summary>
        /// Returns a string representation of the inventory issue detail.
        /// </summary>
        /// <returns>The string representation of the inventory issue detail.</returns>
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
