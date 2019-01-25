using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.SAP.B1
{
    public class SAPCnxInfo
    {
        public string ServerName { get; set; }
        public string CatalogName { get; set; }
        public string SAPUserName { get; set; }
        public string SAPUserPassword { get; set; }
        public string DatabaseVersion { get; set; }
        public string LicenseServer { get; set; }

        public SAPCnxInfo()
        {
            ServerName = ConfigurationManager.AppSettings["SAPSERVERNAME"];
            DatabaseVersion = ConfigurationManager.AppSettings["SAPDBTYPE"];
            LicenseServer = ConfigurationManager.AppSettings["SAPLICENSESERVER"];
            SAPUserName = ConfigurationManager.AppSettings["SAPUSER"];
            SAPUserPassword = ConfigurationManager.AppSettings["SAPPASS"];
            CatalogName = ConfigurationManager.AppSettings["SAPCOMPANY"];
        }
    }
}
