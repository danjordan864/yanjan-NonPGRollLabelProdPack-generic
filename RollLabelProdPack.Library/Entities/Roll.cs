using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Entities
{
    public class Roll
    {
        public string RollNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string IRMS { get; set; }
        public string YJNOrder { get; set; }
        public string SSCC { get; set; }

        public string PG_SSCC { get; set; }
        public decimal Kgs { get; set; }
        public bool Scrap { get; set; }
        public bool Print { get; set; }
        public int LUID { get; set; }
        public string JumboRoll { get; set; }
        public string ScrapReason { get; set; }
        public string StorLocCode { get; set; }
        public string QualityStatus { get; set; }
        public string UOM { get; set; }

    }
}
