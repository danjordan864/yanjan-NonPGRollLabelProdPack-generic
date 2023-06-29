namespace RollLabelProdPack.Library.Entities
{
    /// <summary>
    /// Represents a production line.
    /// </summary>
    public class ProductionLine
    {
        // Internal comment: The code of the production line.
        /// <summary>
        /// Gets or sets the code of the production line.
        /// </summary>
        public string Code { get; set; }

        // Internal comment: The line number of the production line.
        /// <summary>
        /// Gets or sets the line number of the production line.
        /// </summary>
        public string LineNo { get; set; }

        // Internal comment: The input location code of the production line.
        /// <summary>
        /// Gets or sets the input location code of the production line.
        /// </summary>
        public string InputLocationCode { get; set; }

        // Internal comment: The output location code of the production line.
        /// <summary>
        /// Gets or sets the output location code of the production line.
        /// </summary>
        public string OutputLocationCode { get; set; }

        // Internal comment: The printer associated with the production line.
        /// <summary>
        /// Gets or sets the printer associated with the production line.
        /// </summary>
        public string Printer { get; set; }
    }
}
