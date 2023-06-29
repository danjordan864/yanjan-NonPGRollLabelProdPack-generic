using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Entities
{
    public class PackLabel
    {
        public int ID { get; set; }
        public string PMXSSCC { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string IRMS { get; set; }
        public string YJNOrder { get; set; }
        public string SSCC { get; set; }
        public int SAPOrder { get; set; }
        public DateTime Created { get; set; }
        public DateTime ProductionDate { get; set; }
        public string LotNo { get; set; }
        public string PalletType { get; set; }
        public bool Printed { get; set; }
        public bool Valid { get; set; }
        public string ValidMessage { get; set; }
        public decimal Qty { get; set; }
        public List<Roll> Rolls { get; set; }
        public int MaxRollsPerPack { get; set; }
        public int Copies { get; set; }

    }
}
