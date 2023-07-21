using System;
using System.Collections.Generic;

namespace RollLabelProdPack.Library.Entities
{
    /// <summary>
    /// Represents a Co-Pack Pallet Label.
    /// </summary>
    public class CoPackPalletLabel
    {
        /// <summary>
        /// Gets or sets the ID of the Co-Pack Pallet Label.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the PMX SSCC of the Co-Pack Pallet Label.
        /// </summary>
        public string PMXSSCC { get; set; }

        /// <summary>
        /// Gets or sets the item code of the Co-Pack Pallet Label.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the item name of the Co-Pack Pallet Label.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the description of the Co-Pack Pallet Label.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the IRMS of the Co-Pack Pallet Label.
        /// </summary>
        public string IRMS { get; set; }

        /// <summary>
        /// Gets or sets the YJN Order of the Co-Pack Pallet Label.
        /// </summary>
        public string YJNOrder { get; set; }

        /// <summary>
        /// Gets or sets the SSCC of the Co-Pack Pallet Label.
        /// </summary>
        public string SSCC { get; set; }

        /// <summary>
        /// Gets or sets the SAP Order of the Co-Pack Pallet Label.
        /// </summary>
        public int SAPOrder { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the Co-Pack Pallet Label.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the production date of the Co-Pack Pallet Label.
        /// </summary>
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// Gets or sets the lot number of the Co-Pack Pallet Label.
        /// </summary>
        public string LotNo { get; set; }

        /// <summary>
        /// Gets or sets the pallet type of the Co-Pack Pallet Label.
        /// </summary>
        public string PalletType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Co-Pack Pallet Label has been printed.
        /// </summary>
        public bool Printed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Co-Pack Pallet Label is valid.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets the validation message of the Co-Pack Pallet Label.
        /// </summary>
        public string ValidMessage { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the Co-Pack Pallet Label.
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// Gets or sets the list of cases associated with the Co-Pack Pallet Label.
        /// </summary>
        public List<Case> Cases { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of cases per pack for the Co-Pack Pallet Label.
        /// </summary>
        public int MaxCasesPerPack { get; set; }

        /// <summary>
        /// Gets or sets the number of copies of the Co-Pack Pallet Label.
        /// </summary>
        public int Copies { get; set; }

        /// <summary>
        /// Gets or sets the employee associated with the Co-Pack Pallet Label.
        /// </summary>
        public string Employee { get; set; }
    }
}
