using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Entities
{
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

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string PMXSSCC
        {
            get
            {
                return _pmxsscc;
            }
            set
            {
                if (value != _pmxsscc)
                {
                    _pmxsscc = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string ItemCode
        {
            get
            {
                return _itemCode;
            }
            set
            {
                if (value != _itemCode)
                {
                    _itemCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string ItemName
        {
            get
            {
                return _itemName;
            }
            set
            {
                if (value != _itemName)
                {
                    _itemName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string IRMS
        {
            get
            {
                return _irms;
            }
            set
            {
                if (value != _irms)
                {
                    _irms = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string YJNOrder
        {
            get
            {
                return _yjnOrder;
            }
            set
            {
                if (value != _yjnOrder)
                {
                    _yjnOrder = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string SSCC
        {
            get
            {
                return _sscc;
            }
            set
            {
                if (value != _sscc)
                {
                    _sscc = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int SAPOrder
        {
            get
            {
                return _sapOrder;
            }

            set
            {
                if (value != _sapOrder)
                {
                    _sapOrder = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                if (value != _created)
                {
                    _created = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime ProductionDate
        {
            get
            {
                return _productionDate;
            }
            set
            {
                if (value != _productionDate)
                {
                    _productionDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string LotNo
        {
            get
            {
                return _lotNo;
            }
            set
            {
                if (value != _lotNo)
                {
                    _lotNo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string PalletType
        {
            get
            {
                return _palletType;
            }
            set
            {
                if (value != _palletType)
                {
                    _palletType = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool Printed
        {
            get
            {
                return _printed;
            }
            set
            {
                if (value != _printed)
                {
                    _printed = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool Valid
        {
            get
            {
                return _valid;
            }
            set
            {
                if (value != _valid)
                {
                    _valid = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string ValidMessage
        {
            get
            {
                return _validMessage;
            }
            set
            {
                if (value != _validMessage)
                {
                    _validMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal Qty
        {
            get
            {
                return _qty;
            }
            set
            {
                if (value != _qty)
                {
                    _qty = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<Roll> Rolls
        {
            get
            {
                return _rolls;
            }
            set
            {
                if (value != _rolls)
                {
                    _rolls = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int MaxRollsPerPack
        {
            get
            {
                return _maxRollsPerPack;
            }
            set
            {
                if (value != _maxRollsPerPack)
                {
                    _maxRollsPerPack = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Copies
        {
            get
            {
                return _copies;
            }
            set
            {
                if (value != _copies)
                {
                    _copies = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Employee
        {
            get
            {
                return _employee;
            }
            set
            {
                if (value != _employee)
                {
                    _employee = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal TotalWeight
        {
            get
            {
                return _totalWeight;
            }
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

        public decimal TotalTareKg {
            get
            {
                return _totalTareKg;
            }
            set
            {
                if (value != _totalTareKg)
                {
                    _totalTareKg = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal TotalNetKg
        {
            get
            {
                return _totalNetKg;
            }
            set
            {
                if (value != _totalNetKg)
                {
                    _totalNetKg = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool TotalWeightEntered
        {
            get
            {
                return _totalWeightEntered;
            }
            set
            {
                if (value != _totalWeightEntered)
                {
                    _totalWeightEntered = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string PrintButtonText { get { return "Print"; } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
