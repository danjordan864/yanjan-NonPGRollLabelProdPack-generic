using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RollLabelProdPack.Library.Entities
{
    /// <summary>
    /// Represents a pack label.
    /// </summary>
    public class PackLabel : INotifyPropertyChanged
    {
        private int _id;
        private string _pmxsscc;
        private string _itemCode;
        private string _itemName;
        private string _description;
        private string _irms;
        private string _yjnOrder;
        private string _sscc;
        private int _sapOrder;
        private DateTime _created;
        private DateTime _productionDate;
        private string _lotNo;
        private string _palletType;
        private bool _printed;
        private bool _valid;
        private string _validMessage;
        private decimal _qty;
        private List<Roll> _rolls;
        private int _maxRollsPerPack;
        private int _copies;
        private string _employee;
        private decimal _totalWeight;
        private decimal _totalTareKg;
        private decimal _totalNetKg;
        private bool _totalWeightEntered;
        private int _numRolls;
        private string _poNumber;

        /// <summary>
        /// Gets or sets the ID of the pack label.
        /// </summary>
        public int ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The PMXSSCC is a unique identifier for the pack label.
        /// <summary>
        /// Gets or sets the PMXSSCC of the pack label.
        /// </summary>
        public string PMXSSCC
        {
            get { return _pmxsscc; }
            set
            {
                if (value != _pmxsscc)
                {
                    _pmxsscc = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The ItemCode represents the code of the item contained in the pack label.
        /// <summary>
        /// Gets or sets the item code of the pack label.
        /// </summary>
        public string ItemCode
        {
            get { return _itemCode; }
            set
            {
                if (value != _itemCode)
                {
                    _itemCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The ItemName represents the name of the item contained in the pack label.
        /// <summary>
        /// Gets or sets the item name of the pack label.
        /// </summary>
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                if (value != _itemName)
                {
                    _itemName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The Description provides additional information about the pack label.
        /// <summary>
        /// Gets or sets the description of the pack label.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The IRMS represents a specific identifier for the pack label.
        /// <summary>
        /// Gets or sets the IRMS of the pack label.
        /// </summary>
        public string IRMS
        {
            get { return _irms; }
            set
            {
                if (value != _irms)
                {
                    _irms = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The YJNOrder represents the order associated with the pack label.
        /// <summary>
        /// Gets or sets the YJN order of the pack label.
        /// </summary>
        public string YJNOrder
        {
            get { return _yjnOrder; }
            set
            {
                if (value != _yjnOrder)
                {
                    _yjnOrder = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The SSCC is a unique identifier for the pack label.
        /// <summary>
        /// Gets or sets the SSCC of the pack label.
        /// </summary>
        public string SSCC
        {
            get { return _sscc; }
            set
            {
                if (value != _sscc)
                {
                    _sscc = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The SAPOrder represents the order associated with the pack label in the SAP system.
        /// <summary>
        /// Gets or sets the SAP order of the pack label.
        /// </summary>
        public int SAPOrder
        {
            get { return _sapOrder; }
            set
            {
                if (value != _sapOrder)
                {
                    _sapOrder = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The Created property represents the date and time when the pack label was created.
        /// <summary>
        /// Gets or sets the creation date and time of the pack label.
        /// </summary>
        public DateTime Created
        {
            get { return _created; }
            set
            {
                if (value != _created)
                {
                    _created = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The ProductionDate represents the date when the pack label is produced.
        /// <summary>
        /// Gets or sets the production date of the pack label.
        /// </summary>
        public DateTime ProductionDate
        {
            get { return _productionDate; }
            set
            {
                if (value != _productionDate)
                {
                    _productionDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The LotNo represents the lot number associated with the pack label.
        /// <summary>
        /// Gets or sets the lot number of the pack label.
        /// </summary>
        public string LotNo
        {
            get { return _lotNo; }
            set
            {
                if (value != _lotNo)
                {
                    _lotNo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The PalletType represents the type of pallet used for the pack label.
        /// <summary>
        /// Gets or sets the pallet type of the pack label.
        /// </summary>
        public string PalletType
        {
            get { return _palletType; }
            set
            {
                if (value != _palletType)
                {
                    _palletType = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The Printed property indicates whether the pack label has been printed.
        /// <summary>
        /// Gets or sets a value indicating whether the pack label has been printed.
        /// </summary>
        public bool Printed
        {
            get { return _printed; }
            set
            {
                if (value != _printed)
                {
                    _printed = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The Valid property indicates whether the pack label is valid.
        /// <summary>
        /// Gets or sets a value indicating whether the pack label is valid.
        /// </summary>
        public bool Valid
        {
            get { return _valid; }
            set
            {
                if (value != _valid)
                {
                    _valid = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The ValidMessage provides information about the validity of the pack label.
        /// <summary>
        /// Gets or sets the valid message of the pack label.
        /// </summary>
        public string ValidMessage
        {
            get { return _validMessage; }
            set
            {
                if (value != _validMessage)
                {
                    _validMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The Qty represents the quantity of the item in the pack label.
        /// <summary>
        /// Gets or sets the quantity of the item in the pack label.
        /// </summary>
        public decimal Qty
        {
            get { return _qty; }
            set
            {
                if (value != _qty)
                {
                    _qty = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The Rolls represent the list of rolls contained in the pack label.
        /// <summary>
        /// Gets or sets the list of rolls in the pack label.
        /// </summary>
        public List<Roll> Rolls
        {
            get { return _rolls; }
            set
            {
                if (value != _rolls)
                {
                    _rolls = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The MaxRollsPerPack represents the maximum number of rolls allowed in the pack label.
        /// <summary>
        /// Gets or sets the maximum number of rolls allowed in the pack label.
        /// </summary>
        public int MaxRollsPerPack
        {
            get { return _maxRollsPerPack; }
            set
            {
                if (value != _maxRollsPerPack)
                {
                    _maxRollsPerPack = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The Copies represent the number of copies to be printed for the pack label.
        /// <summary>
        /// Gets or sets the number of copies to be printed for the pack label.
        /// </summary>
        public int Copies
        {
            get { return _copies; }
            set
            {
                if (value != _copies)
                {
                    _copies = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The Employee represents the employee associated with the pack label.
        /// <summary>
        /// Gets or sets the employee associated with the pack label.
        /// </summary>
        public string Employee
        {
            get { return _employee; }
            set
            {
                if (value != _employee)
                {
                    _employee = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The TotalWeight represents the total weight of the pack label.
        /// <summary>
        /// Gets or sets the total weight of the pack label.
        /// </summary>
        public decimal TotalWeight
        {
            get { return _totalWeight; }
            set
            {
                if (value != _totalWeight)
                {
                    _totalWeight = value;
                    TotalNetKg = _totalWeight - TotalTareKg;
                    //NotifyPropertyChanged();
                }
                NotifyPropertyChanged();
            }
        }

        // Internal comment: The TotalTareKg represents the total tare weight of the pack label.
        /// <summary>
        /// Gets or sets the total tare weight of the pack label.
        /// </summary>
        public decimal TotalTareKg
        {
            get { return _totalTareKg; }
            set
            {
                if (value != _totalTareKg)
                {
                    _totalTareKg = value;
                    TotalNetKg = TotalWeight - _totalTareKg;
                    //NotifyPropertyChanged();
                }
                NotifyPropertyChanged();
            }
        }

        // Internal comment: The TotalNetKg represents the total net weight of the pack label.
        /// <summary>
        /// Gets or sets the total net weight of the pack label.
        /// </summary>
        public decimal TotalNetKg
        {
            get { return _totalNetKg; }
            set
            {
                if (value != _totalNetKg)
                {
                    _totalNetKg = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Internal comment: The TotalWeightEntered indicates whether the total weight has been entered for the pack label.
        /// <summary>
        /// Gets or sets a value indicating whether the total weight has been entered for the pack label.
        /// </summary>
        public bool TotalWeightEntered
        {
            get { return _totalWeightEntered; }
            set
            {
                if (value != _totalWeightEntered)
                {
                    _totalWeightEntered = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of rolls on the pack label.
        /// </summary>
        public int NumRolls
        {
            get { return _numRolls; }
            set
            {
                _numRolls = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the purchase order (PO) number for the pack label
        /// </summary>
        public string PONumber
        {
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

        public string PrintButtonText
        {
            get { return "Print"; }
        }

        // Internal comment: Event raised when a property value changes.
        /// <summary>
        /// Event raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
