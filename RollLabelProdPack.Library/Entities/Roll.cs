using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Entities
{
    public class Roll : INotifyPropertyChanged
    {
        private decimal _kgs;
        private decimal _tareKg;
        private decimal _adjustKgs;
        private decimal _netKg;

        public Roll()
        {
            PropertyChanged += Roll_PropertyChanged;
        }

        private void Roll_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NetKg" || e.PropertyName == "AdjustKgs" || e.PropertyName == "TareKg")
            {
                Kgs = NetKg + TareKg + AdjustKgs;
            }
        }

        public string RollNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string IRMS { get; set; }
        public string YJNOrder { get; set; }
        public string SSCC { get; set; }

        public string PG_SSCC { get; set; }
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

        public bool Scrap { get; set; }
        public bool Hold { get; set; }
        public bool Print { get; set; }
        public int LUID { get; set; }
        public string JumboRoll { get; set; }
        public string ScrapReason { get; set; }
        public string StorLocCode { get; set; }
        public string QualityStatus { get; set; }
        public string UOM { get; set; }

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
