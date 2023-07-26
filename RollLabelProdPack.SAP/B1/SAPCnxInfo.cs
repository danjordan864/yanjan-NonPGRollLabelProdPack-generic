using System.Configuration;

namespace RollLabelProdPack.SAP.B1
{
    /// <summary>
    /// Represents the connection information for SAP Business One (SAP B1) integration.
    /// </summary>
    public class SAPCnxInfo
    {
        /// <summary>
        /// Gets or sets the name of the SAP server.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the database catalog.
        /// </summary>
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets or sets the username for SAP B1.
        /// </summary>
        public string SAPUserName { get; set; }

        /// <summary>
        /// Gets or sets the password for SAP B1 user.
        /// </summary>
        public string SAPUserPassword { get; set; }

        /// <summary>
        /// Gets or sets the version of the SAP database.
        /// </summary>
        public string DatabaseVersion { get; set; }

        /// <summary>
        /// Gets or sets the license server for SAP B1.
        /// </summary>
        public string LicenseServer { get; set; }

        /// <summary>
        /// Initializes a new instance of the SAPCnxInfo class.
        /// Reads the SAP B1 connection information from the configuration file.
        /// </summary>
        public SAPCnxInfo()
        {
            // Read the SAP B1 connection information from the configuration file
            ServerName = ConfigurationManager.AppSettings["SAPSERVERNAME"];
            DatabaseVersion = ConfigurationManager.AppSettings["SAPDBTYPE"];
            LicenseServer = ConfigurationManager.AppSettings["SAPLICENSESERVER"];
            SAPUserName = ConfigurationManager.AppSettings["SAPUSER"];
            SAPUserPassword = ConfigurationManager.AppSettings["SAPPASS"];
            CatalogName = ConfigurationManager.AppSettings["SAPCOMPANY"];
        }
    }
}
