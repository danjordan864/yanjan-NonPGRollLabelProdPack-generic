using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RollLabelProdPack.Library.Entities
{
    /// <summary>
    /// Represents a roll.
    /// </summary>
    public class Roll : INotifyPropertyChanged
    {
        private decimal _kgs;
        private decimal _tareKg;
        private decimal _adjustKgs;
        private decimal _netKg;
        private decimal _squareMeters;
        private string _poNumber;
        private decimal _quantity;

        /// <summary>
        /// Initializes a new instance of the <see cref="Roll"/> class.
        /// </summary>
        public Roll()
        {
            PropertyChanged += Roll_PropertyChanged;
        }

        /// <summary>
        /// Event handler for the PropertyChanged event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A PropertyChangedEventArgs that contains the event data.</param>
        private void Roll_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NetKg" || e.PropertyName == "AdjustKgs" || e.PropertyName == "TareKg")
            {
                Kgs = NetKg + TareKg + AdjustKgs;
            }
        }

        // Internal comment: The roll number.
        /// <summary>
        /// Gets or sets the roll number.
        /// </summary>
        public string RollNo { get; set; }

        // Internal comment: The item code associated with the roll.
        /// <summary>
        /// Gets or sets the item code associated with the roll.
        /// </summary>
        public string ItemCode { get; set; }

        // Internal comment: The item name associated with the roll.
        /// <summary>
        /// Gets or sets the item name associated with the roll.
        /// </summary>
        public string ItemName { get; set; }

        // Internal comment: The IRMS of the roll.
        /// <summary>
        /// Gets or sets the IRMS of the roll.
        /// </summary>
        public string IRMS { get; set; }

        // Internal comment: The YJN order of the roll.
        /// <summary>
        /// Gets or sets the YJN order of the roll.
        /// </summary>
        public string YJNOrder { get; set; }

        // Internal comment: The SSCC of the roll.
        /// <summary>
        /// Gets or sets the SSCC of the roll.
        /// </summary>
        public string SSCC { get; set; }

        // Internal comment: The P&G SSCC of the roll.
        /// <summary>
        /// Gets or sets the P&G SSCC of the roll.
        /// </summary>
        public string PG_SSCC { get; set; }

        // Internal comment: The weight of the roll in kilograms.
        /// <summary>
        /// Gets or sets the weight of the roll in kilograms.
        /// </summary>
        public decimal Kgs
        {
            get
            {
                return _kgs;
            }
            set
            {
                _kgs = value;
                NotifyPropertyChanged();
            }
        }

        public decimal Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                NotifyPropertyChanged();
            }
        }

        // Internal comment: Indicates if the roll is marked as scrap.
        /// <summary>
        /// Gets or sets a value indicating whether the roll is marked as scrap.
        /// </summary>
        public bool Scrap { get; set; }

        // Internal comment: Indicates if the roll is on hold.
        /// <summary>
        /// Gets or sets a value indicating whether the roll is on hold.
        /// </summary>
        public bool Hold { get; set; }

        // Internal comment: Indicates if the roll should be printed.
        /// <summary>
        /// Gets or sets a value indicating whether the roll should be printed.
        /// </summary>
        public bool Print { get; set; }

        // Internal comment: The Logistic Unit ID (LUID) of the roll.
        /// <summary>
        /// Gets or sets the Logistic Unit ID (LUID) of the roll.
        /// </summary>
        public int LUID { get; set; }

        // Internal comment: The Jumbo Roll associated with the roll.
        /// <summary>
        /// Gets or sets the Jumbo Roll associated with the roll.
        /// </summary>
        public string JumboRoll { get; set; }

        // Internal comment: The reason for marking the roll as scrap.
        /// <summary>
        /// Gets or sets the reason for marking the roll as scrap.
        /// </summary>
        public string ScrapReason { get; set; }

        // Internal comment: The storage location code of the roll.
        /// <summary>
        /// Gets or sets the storage location code of the roll.
        /// </summary>
        public string StorLocCode { get; set; }

        // Internal comment: The quality status of the roll.
        /// <summary>
        /// Gets or sets the quality status of the roll.
        /// </summary>
        public string QualityStatus { get; set; }

        // Internal comment: The unit of measure (UOM) for the roll weight.
        /// <summary>
        /// Gets or sets the unit of measure (UOM) for the roll weight.
        /// </summary>
        public string UOM { get; set; }

        // Internal comment: The adjustment weight in kilograms.
        /// <summary>
        /// Gets or sets the adjustment weight in kilograms.
        /// </summary>
        public decimal AdjustKgs
        {
            get
            {
                return _adjustKgs;
            }
            set
            {
                _adjustKgs = value;
                NotifyPropertyChanged();
            }
        }

        // Internal comment: The tare weight in kilograms.
        /// <summary>
        /// Gets or sets the tare weight in kilograms.
        /// </summary>
        public decimal TareKg
        {
            get
            {
                return _tareKg;
            }
            set
            {
                _tareKg = value;
                NotifyPropertyChanged();
            }
        }

        // Internal comment: The net weight in kilograms.
        /// <summary>
        /// Gets or sets the net weight in kilograms.
        /// </summary>
        public decimal NetKg
        {
            get
            {
                return _netKg;
            }
            set
            {
                _netKg = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the number of Square Meters on the roll
        /// </summary>
        public decimal SquareMeters
        {
            get
            {
                return _squareMeters;
            }
            set
            {
                _squareMeters = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the PO Number on the roll
        /// </summary>
        public string PONumber {
            get
            {
                return _poNumber;
            }
            set
            {
                _poNumber = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies subscribers that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
