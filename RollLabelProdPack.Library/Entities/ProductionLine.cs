using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Entities
{
    public class ProductionLine
    {
        public string Code { get; set; }
        public string LineNo { get; set; }
        public string InputLocationCode { get; set; }
        public string OutputLocationCode { get; set; }
        public string Printer { get; set; }
    }
}
