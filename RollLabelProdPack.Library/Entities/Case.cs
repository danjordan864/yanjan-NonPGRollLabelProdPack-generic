namespace RollLabelProdPack.Library.Entities
{
    /// <summary>
    /// Represents a case entity.
    /// </summary>
    public class Case
    {
        /// <summary>
        /// Gets or sets the case number.
        /// </summary>
        public string CaseNo { get; set; }

        /// <summary>
        /// Gets or sets the item code.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the item name.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the IRMS (Integrated Retail Management System) value.
        /// </summary>
        public string IRMS { get; set; }

        /// <summary>
        /// Gets or sets the Yanjan order value.
        /// </summary>
        public string YJNOrder { get; set; }

        /// <summary>
        /// Gets or sets the SSCC (Serial Shipping Container Code) value.
        /// </summary>
        public string SSCC { get; set; }

        /// <summary>
        /// Gets or sets the PG_SSCC (P&G Serial Shipping Container Code) value.
        /// </summary>
        public string PG_SSCC { get; set; }

        /// <summary>
        /// Gets or sets the number of units in the case.
        /// </summary>
        public decimal Units { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the case is marked as scrap.
        /// </summary>
        public bool Scrap { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the case is on hold.
        /// </summary>
        public bool Hold { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the case is marked for printing.
        /// </summary>
        public bool Print { get; set; }

        /// <summary>
        /// Gets or sets the Logistic Unit ID.
        /// </summary>
        public int LUID { get; set; }

        /// <summary>
        /// Gets or sets the reason for marking the case as scrap.
        /// </summary>
        public string ScrapReason { get; set; }

        /// <summary>
        /// Gets or sets the storage location code.
        /// </summary>
        public string StorLocCode { get; set; }

        /// <summary>
        /// Gets or sets the quality status of the case.
        /// </summary>
        public string QualityStatus { get; set; }

        /// <summary>
        /// Gets or sets the unit of measure.
        /// </summary>
        public string UOM { get; set; }
    }
}
