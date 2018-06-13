using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Utility
{
    public class AppUtility
    {
        #region get connections

        /// <summary></summary>
        public static string GetSQLDatabaseConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;
        }

        /// <summary></summary>

        #endregion

        #region event logs

        /// <summary></summary>
        public static void WriteToEventLog(string logMessage, EventLogEntryType eType, bool sendEmail)
        {
            var logSource = GetLoggingSource();
            var logging = GetLogging();
            var logName = "Application";
            var logMachine = ".";
            var eventID = 1000;
            try
            {
                if (eType != EventLogEntryType.Error) { if (!logging) { return; } } else { eventID = 9000; }
                EventLog logEntry = new EventLog(logName, logMachine, logSource);
                logEntry.WriteEntry(logMessage, eType, eventID);
                //if (sendEmail) { SendEmail(logMessage, (eType == EventLogEntryType.Error)); }
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }


        public static bool GetLogging()
        {
            try { return bool.Parse(ConfigurationManager.AppSettings["Logging"]); }
            catch { return false; }
        }

        /// <summary></summary>
        public static string GetLoggingSource()
        {
            try { return ConfigurationManager.AppSettings["LoggingSource"]; }
            catch { return "Roll Label Prod Pack"; }
        }

        /// <summary></summary>
        public static string GetLoggingText()
        {
            try { return ConfigurationManager.AppSettings["LoggingText"]; }
            catch { return "Roll Label Prod Pack"; }
        }

        public static int LoggingEventIDForTrigger()
        {
            try { return int.Parse(ConfigurationManager.AppSettings["LoggingEventIDForTrigger"].ToString()); }
            catch { return 9999; }
        }

        /// <summary></summary>
        public static string GetLoggingTriggerAppName()
        {
            try { return ConfigurationManager.AppSettings["LoggingTriggerAppName"]; }
            catch { return ""; }
        }
        #endregion

        #region b1 settings

        /// <summary></summary>
        public static string GetSAPServerName()
        {
            try { return ConfigurationManager.AppSettings["SAPSERVERNAME"]; }
            catch { return ""; }
        }

        /// <summary></summary>
        public static string GetSAPCompany()
        {
            try { return ConfigurationManager.AppSettings["SAPCOMPANY"]; }
            catch { return ""; }
        }

        /// <summary></summary>
        public static string GetSAPUser()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER"]; }
            catch { return ""; }
        }

        /// <summary></summary>
        public static string GetSAPPassword()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS"]; }
            catch { return ""; }
        }

        /// <summary></summary>
        public static string GetSAPLicenseServer()
        {
            try { return ConfigurationManager.AppSettings["SAPLICENSESERVER"]; }
            catch { return ""; }
        }

        /// <summary></summary>
        public static bool GetSSAPUseTransactions()
        {
            try { return bool.Parse(ConfigurationManager.AppSettings["UseTransactions"]); }
            catch { return false; }
        }

        /// <summary></summary>
        public static bool GetSAPRollbackForTesting()
        {
            try { return bool.Parse(ConfigurationManager.AppSettings["RollBackForTesting"]); }
            catch { return false; }
        }

        public static string GetSAPDBType()
        {
            return ConfigurationManager.AppSettings["SAPDBTYPE"];
        }
        #endregion

        #region app config Settings
        public static string GetFactoryCode()
        {
            return ConfigurationManager.AppSettings["FactoryCode"];
        }
        public static string GetDefaultItemCode()
        {
            return ConfigurationManager.AppSettings["DefaultItemCode"];
        }

        public static string GetDefaultItemDesc()
        {
            return ConfigurationManager.AppSettings["DefaultItemDesc"];
        }
        public static string GetLastBatch()
        {
            return ConfigurationManager.AppSettings["LastBatch"];
        }
        public static string GetLastRoll()
        {
            return ConfigurationManager.AppSettings["LastRoll"];
        }

        public static string GetDefaultIMRS()
        {
            return ConfigurationManager.AppSettings["DefaultIMRS"];
        }

        public static string GetDefaultMacineNo()
        {
            return ConfigurationManager.AppSettings["DefaultMachineNo"];
        }
        public static string GetDefaultMaterialCode()
        {
            return ConfigurationManager.AppSettings["DefaultMaterialCode"];
        }

        public static string GetDefaultNoOfSlitPositions()
        {
            return ConfigurationManager.AppSettings["DefaultNoOfSlitPositions"];
        }
        public static string GetDefaultShift()
        {
            return ConfigurationManager.AppSettings["DefaultShift"];
        }

        public static string GetDefaultDie()
        {
            return ConfigurationManager.AppSettings["DefaultDie"];
        }
        public static string GetDefaultProdName()
        {
            return ConfigurationManager.AppSettings["DefaultProdName"];
        }
        public static string GetPrintLocRollLabel4by6()
        {
            return ConfigurationManager.AppSettings["PrintLocRollLabel4by6"];
        }
        public static string GetPrintLocRollLabel1by6()
        {
            return ConfigurationManager.AppSettings["PrintLocRollLabel1by6"];
        }
        public static string GetPrintLocPack()
        {
            return ConfigurationManager.AppSettings["PrintLocPack"];
        }

        public static string GetLabelPrintExtension()
        {
            return ConfigurationManager.AppSettings["LabelPrintExtension"];
        }

        public static string GetSupplierId()
        {
            return ConfigurationManager.AppSettings["SupplierId"];
        }
        public static string GetYanJanProdMo()
        {
            string mo = DateTime.Now.Month.ToString();
            string yjMoCode = null;
            switch (mo)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    yjMoCode = mo;
                    break;
                case "10":
                    yjMoCode = "0";
                    break;
                case "11":
                    yjMoCode = "N";
                    break;
                case "12":
                    yjMoCode = "D";
                    break;
            }
            return yjMoCode;
        }

        #endregion


    }
}
