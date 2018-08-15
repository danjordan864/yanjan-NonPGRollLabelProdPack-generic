using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Entities
{
    public class RollLabelData
    {
        public string ProductionYear { get; set; }
        public string ProductionMonth { get; set; }
        public string ProductionDate { get; set; }
        public string AperatureDieNo { get; set; }
        public string ProductionShift { get; set; }
        public int JumboRollNo { get; set; }
        public int SlitPosition { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string IRMS { get; set; }
        public string FactoryCode { get; set; }
        public string ProductionMachineNo { get; set; }
        public string ProductionLine { get; set; }
        public string MaterialCode { get; set; }
        public string ProductName { get; set; }
        public string BatchNo { get; set; }
        public int SAPOrderNo { get; set; }
        public int SAPDocEntry { get; set; }
        public string YJNOrder { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int NoOfSlits { get; set; }
        public string Employee { get; set; }
        public string Shift { get; set; }
        public string RollNo { get; set; }
        public int LUID { get; set; }
        public string SSCC { get; set; }
    }
}
