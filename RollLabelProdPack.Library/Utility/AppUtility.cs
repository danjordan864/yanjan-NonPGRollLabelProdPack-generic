using RollLabelProdPack.Library.Data;
using RollLabelProdPack.Library.Email;
using RollLabelProdPack.Library.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Utility
{
    public class AppUtility
    {
        #region production common routines
        public static List<InventoryIssueDetail> RefreshIssueQty(int prodOrder, string prodLine, decimal productionQty)
        {
            var plannedIssue = new List<InventoryIssueDetail>();
            var packingMtlLoc = GetPackingMtlLocation();
            var so = AppData.GetProdLineInputMaterial(prodLine, packingMtlLoc);
            if (!so.SuccessFlag) throw new ApplicationException($"Error getting Prod. Line Input Material. Error:{so.ServiceException}");
            var inputLocMaterial = so.ReturnValue as List<InventoryDetail>;
            so = AppData.GetProdOrderIssueMaterial(prodOrder, productionQty);
            if (!so.SuccessFlag) throw new ApplicationException($"Error getting Issue Material. Error:{so.ServiceException}");
            var prodOrderLines = so.ReturnValue as List<InventoryIssueDetail>;
            foreach (var line in prodOrderLines)
            {
                var qtyReq = line.PlannedIssueQty;
                if (line.PlannedIssueQty > 0) //scrap is set up as by-product negative line qty
                {
                    var itemAvail = inputLocMaterial.Where(i => i.ItemCode == line.ItemCode).OrderBy(i => i.InDate).ThenBy(i => i.Batch).ToList();
                    if (itemAvail.Count > 0)
                    {
                        foreach (var item in itemAvail)
                        {
                            if (qtyReq > 0)
                            {
                                if (item.Quantity > qtyReq)
                                {
                                    plannedIssue.Add(CreatePlannedIssueDetail(line.ItemCode, line.UOM, line.BaseEntry, line.BaseLine, item.StorageLocation, item.QualityStatus,
                                        item.LUID, item.SSCC, qtyReq, qtyReq, item.Batch, 0, line.BatchControlled, line.PackagingMtl));

                                    qtyReq = 0;
                                }
                                else
                                {
                                    plannedIssue.Add(CreatePlannedIssueDetail(line.ItemCode, line.UOM, line.BaseEntry, line.BaseLine, item.StorageLocation, item.QualityStatus,
                                        item.LUID, item.SSCC, qtyReq, item.Quantity, item.Batch, 0, line.BatchControlled, line.PackagingMtl));
                                    qtyReq = qtyReq - item.Quantity;
                                }
                            }
                        }
                    }
                    if (qtyReq > 0)
                    {
                        plannedIssue.Add(CreatePlannedIssueDetail(line.ItemCode, line.UOM, line.BaseEntry, line.BaseLine, string.Empty, string.Empty, 0,
                            string.Empty, qtyReq, 0, string.Empty, qtyReq, line.BatchControlled, line.PackagingMtl));
                    }
                }
            }
            return plannedIssue;
        }


        public static InventoryIssueDetail CreatePlannedIssueDetail(string itemCode, string uom, int baseEntry, int baseLine, string storageLocation,
           string qualityStatus, int luid, string sscc, double availableQty, double plannedIssueqty,
           string batch, double shortQty, bool batchControlled, bool packagingMtl)
        {
            return new InventoryIssueDetail
            {
                ItemCode = itemCode,
                BaseEntry = baseEntry,
                BaseLine = baseLine,
                StorageLocation = storageLocation,
                LUID = luid,
                SSCC = sscc,
                Quantity = availableQty,
                PlannedIssueQty = plannedIssueqty,
                Batch = batch,
                ShortQty = shortQty,
                UOM = uom,
                QualityStatus = qualityStatus,
                PackagingMtl = packagingMtl,
                BatchControlled = batchControlled
            };
        }
        #endregion
        #region get connections

        /// <summary></summary>
        public static string GetSAPConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SAPConnection"].ConnectionString;
        }
        public static string GetPMXConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["PMXConnection"].ConnectionString;
        }

        public static int GetSqlCommandTimeOut()
        {
            var commandTimeout = 30;
            try { commandTimeout = int.Parse(ConfigurationManager.AppSettings["SQLCommandTimeout"]); }
            catch { }
            return commandTimeout;
        }
        public static string TestSQLConnection()
        {
            try
            {
                var databaseConnection = GetSAPConnectionString();
                using (SqlConnection cnx = new SqlConnection(databaseConnection)) { cnx.Open(); }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        /// <summary></summary>

        #endregion

        #region datasets

        /// <summary></summary>
        public static DataSet PopulateDataSet(SqlCommand MyCmd)
        {
            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(MyCmd))
            {
                DataSet returnSet = new DataSet();
                sqlAdapter.Fill(returnSet);
                return returnSet;
            }
        }

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
                if (sendEmail) { SendEmail(logMessage, (eType == EventLogEntryType.Error)); }
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

        public static string GetSAPUserLine1()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_LINE1"]; }
            catch { return ""; }
        }

        public static string GetSAPPassLine1()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_LINE1"]; }
            catch { return ""; }
        }

        public static string GetSAPUserLine2()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_LINE2"]; }
            catch { return ""; }
        }

        public static string GetSAPPassLine2()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_LINE2"]; }
            catch { return ""; }
        }
        public static string GetSAPUserLine3()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_LINE3"]; }
            catch { return ""; }
        }

        public static string GetSAPPassLine3()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_LINE3"]; }
            catch { return ""; }
        }
        public static string GetSAPUserLine4()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_LINE4"]; }
            catch { return ""; }
        }

        public static string GetSAPPassLine4()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_LINE4"]; }
            catch { return ""; }
        }
        public static string GetSAPUserLine5()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_LINE5"]; }
            catch { return ""; }
        }

        public static string GetSAPPassLine5()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_LINE5"]; }
            catch { return ""; }
        }
        public static string GetSAPUserLine6()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_LINE6"]; }
            catch { return ""; }
        }

        public static string GetSAPPassLine6()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_LINE6"]; }
            catch { return ""; }
        }
        public static string GetSAPUserLine7()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_LINE7"]; }
            catch { return ""; }
        }

        public static string GetSAPPassLine7()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_LINE7"]; }
            catch { return ""; }
        }
        public static string GetSAPUserMixLine1()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_MIXLINE1"]; }
            catch { return ""; }
        }

        public static string GetSAPPassMixLine1()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_MIXLINE1"]; }
            catch { return ""; }
        }
        public static string GetSAPUserMixLine2()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_MIXLINE2"]; }
            catch { return ""; }
        }

        public static string GetSAPPassMixLine2()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_MIXLINE2"]; }
            catch { return ""; }
        }

        public static string GetSAPUserMixLine3()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_MIXLINE3"]; }
            catch { return ""; }
        }

        public static string GetSAPPassMixLine3()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_MIXLINE3"]; }
            catch { return ""; }
        }
        public static string GetSAPUserMaskLine1()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_MASKLINE1"]; }
            catch { return ""; }
        }

        public static string GetSAPPassMaskLine1()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_MASKLINE1"]; }
            catch { return ""; }
        }
        public static string GetSAPUserMaskLine2()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_MASKLINE2"]; }
            catch { return ""; }
        }

        public static string GetSAPPassMaskLine2()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_MASKLINE2"]; }
            catch { return ""; }
        }

        public static string GetSAPUserMaskLine3()
        {
            try { return ConfigurationManager.AppSettings["SAPUSER_MASKLINE3"]; }
            catch { return ""; }
        }

        public static string GetSAPPassMaskLine3()
        {
            try { return ConfigurationManager.AppSettings["SAPPASS_MASKLINE3"]; }
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

        public static KeyValuePair<string, string> GetUserNameAndPasswordFilm(string machineNo)
        {
            KeyValuePair<string, string> userNameAndPW = new KeyValuePair<string, string>(GetSAPUser(), GetSAPPassword());
            switch (machineNo)
            {
                case "1":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserLine1(), GetSAPPassLine1());
                    break;
                case "2":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserLine2(), GetSAPPassLine2());
                    break;
                case "3":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserLine3(), GetSAPPassLine3());
                    break;
                case "4":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserLine4(), GetSAPPassLine4());
                    break;
                case "5":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserLine5(), GetSAPPassLine5());
                    break;
                case "6":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserLine6(), GetSAPPassLine6());
                    break;
                case "7":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserLine6(), GetSAPPassLine7());
                    break;
                default:
                    break;
            }
            return userNameAndPW;
        }

        public static KeyValuePair<string, string> GetUserNameAndPasswordMix(string machineNo)
        {
            KeyValuePair<string, string> userNameAndPW = new KeyValuePair<string, string>(GetSAPUser(), GetSAPPassword());
            switch (machineNo)
            {
                case "B":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserMixLine1(), GetSAPPassMixLine1());
                    break;
                case "C":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserMixLine2(), GetSAPPassMixLine2());
                    break;
                case "D":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserMixLine3(), GetSAPPassMixLine3());
                    break;
                default:
                    break;
            }
            return userNameAndPW;
        }

        public static KeyValuePair<string, string> GetUserNameAndPasswordMask(string machineNo)
        {
            KeyValuePair<string, string> userNameAndPW = new KeyValuePair<string, string>(GetSAPUser(), GetSAPPassword());
            switch (machineNo)
            {
                case "M1":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserMaskLine1(), GetSAPPassMaskLine1());
                    break;
                case "M2":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserMaskLine2(), GetSAPPassMaskLine2());
                    break;
                case "M3":
                    userNameAndPW = new KeyValuePair<string, string>(GetSAPUserMaskLine3(), GetSAPPassMaskLine3());
                    break;
                default:
                    break;
            }
            return userNameAndPW;
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

        public static string GetLastPalletNo()
        {
            return ConfigurationManager.AppSettings["LastPalletNo"];
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
        public static string GetBTTriggerLoc()
        {
            return ConfigurationManager.AppSettings["BTTriggerLoc"];
        }
        public static string GetPrintLocPack()
        {
            return ConfigurationManager.AppSettings["PrintLocPack"];
        }
        public static string GetPackPrinterName()
        {
            return ConfigurationManager.AppSettings["TubPalletPrinterName"];
        }
        public static string GetTubPalletPrinterName()
        {
            return ConfigurationManager.AppSettings["PackPrinterName"];
        }
        public static string GetLabelPrintExtension()
        {
            return ConfigurationManager.AppSettings["LabelPrintExtension"];
        }
        public static string GetPGDefault4InchLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefault4InchLabelFormat"];
        }
        public static string GetPGDefault1InchLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefault1InchLabelFormat"];
        }
        public static string GetPGDefaultCombLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultCombLabelFormat"];
        }
        public static string GetPGDefaultScrapLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultScrapLabelFormat"];
        }
        public static string GetPGDefaultResmixLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultResmixLabelFormat"];
        }
        public static string GetPGDefaultPackLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultPackLabelFormat"];
        }

        public static string GetPGDefaultTubPalletLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultTubPalletLabelFormat"];
        }

        public static string GetPGDefaultPackLabelFormatOld()
        {
            return ConfigurationManager.AppSettings["PGDefaultPackLabelFormatOld"];
        }
        public static string GetSupplierId()
        {
            return ConfigurationManager.AppSettings["SupplierId"];
        }

        public static string GetMaskUOMLabel()
        {
            return ConfigurationManager.AppSettings["MaskUOMLabel"];
        }
        public static string GetYanJanProdMo(DateTime orderStartDate)
        {
            string mo = orderStartDate.Month.ToString();
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
                    yjMoCode = "O";
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

        public static int GetOrderBatchNoFromChar(char batchNoChar)
        {
            int batchNo = 0;
            if (System.Text.RegularExpressions.Regex.IsMatch(batchNoChar.ToString(), "^[0-9]*$"))
            {
                batchNo = Convert.ToInt32(batchNoChar.ToString());
            }
            else
            {
                batchNo = 10 + char.ToUpper(batchNoChar) - 64;
            }
            return batchNo;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
        public static string GetDefaultPackCopies()
        {
            return ConfigurationManager.AppSettings["DefaultPackCopies"];
        }

        //PackingMtlLocation
        public static string GetPackingMtlLocation()
        {
            return ConfigurationManager.AppSettings["PackingMtlLocation"];
        }

        public static string GetScrapOffsetCode()
        {
            return ConfigurationManager.AppSettings["ScrapOffsetCode"];
        }

        public static string GetScrapLocCode()
        {
            return ConfigurationManager.AppSettings["ScrapLocCode"];
        }

        public static string GetHoldStatus()
        {
            return ConfigurationManager.AppSettings["HoldStatus"];
        }
        public static string GetHoldLocation()
        {
            return ConfigurationManager.AppSettings["HoldLocation"];
        }
        public static string GetScrapStatus()
        {
            return ConfigurationManager.AppSettings["ScrapStatus"];
        }
        public static string GetDefaultStatus()
        {
            return ConfigurationManager.AppSettings["DefaultStatus"];
        }
        public static string GetDefaultUom()
        {
            return ConfigurationManager.AppSettings["DefaultUom"];
        }
        //Resmix001ToLines
        public static string GetResmix001ToLines()
        {
            return ConfigurationManager.AppSettings["Resmix001ToLines"];
        }
        public static string GetResmix002ToLines()
        {
            return ConfigurationManager.AppSettings["Resmix002ToLines"];
        }
        #endregion

        #region html toast
        public static string GenerateHTMLToast(string title, string text, string type = "w3-warning")
        {
            string str = @"
                    <html>
                    <head>
                    <style>
	                    .w3-warning {
		                    background-color:#ffffcc;
		                    border-left:8px solid #ffeb3b;
		                    margin:0;
	                    }

                        .w3-error  {
	                        background-color:#ffdddd;
	                        border-left:6px solid #f44336;
	                        margin:0;
                        }

                        .w3-success {
	                        background-color:#dff0d8;
	                        border-left:6px solid #4bae4f;
	                        margin:0;
                        }
                    </style>
                    </head>
                    <body style='margin: 0; border-style: solid;'>
	                    <div class='{000}'>
		                    <p><strong>{111}: </strong>{222}
	                    </div>
                    </body>
                    </html>";

            str = str.Replace("{000}", type);
            str = str.Replace("{111}", title);
            str = str.Replace("{222}", text);
            return str;
        }


        #endregion

        #region email

        /// <summary></summary>
        public static int GetSMTPPort()
        {
            try { return int.Parse(ConfigurationManager.AppSettings["SMTPPort"]); }
            catch { return 25; }
        }

        /// <summary></summary>
        public static string GetSMTPHost()
        {
            try { return ConfigurationManager.AppSettings["SMTPHost"]; }
            catch { return "SII-EXS"; }
        }

        /// <summary></summary>
        public static bool GetSMTPUseCredentials()
        {
            try { return bool.Parse(ConfigurationManager.AppSettings["SMTPUseCredentials"]); }
            catch { return false; }
        }

        /// <summary></summary>
        public static string GetSMTPUser()
        {
            try { return ConfigurationManager.AppSettings["SMTPUser"]; }
            catch { return ""; }
        }

        /// <summary></summary>
        public static string GetSMTPPass()
        {
            try { return ConfigurationManager.AppSettings["SMTPPass"]; }
            catch { return ""; }
        }

        /// <summary></summary>
        public static bool GetSMTPUseTSL()
        {
            try { return bool.Parse(ConfigurationManager.AppSettings["SMTPUseTSL"]); }
            catch { return false; }
        }

        /// <summary></summary>
        public static bool GetEmailAlerts()
        {
            try { return bool.Parse(ConfigurationManager.AppSettings["EmailAlerts"]); }
            catch { return false; }
        }

        /// <summary></summary>
        public static bool GetEmailErrors()
        {
            try { return bool.Parse(ConfigurationManager.AppSettings["EmailErrors"]); }
            catch { return false; }
        }

        /// <summary></summary>
        public static string GetFromAddress()
        {
            try { return ConfigurationManager.AppSettings["FromAddress"]; }
            catch { return "sap@synesisintl.com"; }
        }

        /// <summary></summary>
        public static string GetToAddress()
        {
            try { return ConfigurationManager.AppSettings["ToAddress"]; }
            catch { return "jsnyder@synesisintl.com"; }
        }

        /// <summary></summary>
        public static string GetCCAddress()
        {
            try { return ConfigurationManager.AppSettings["CCAddress"]; }
            catch { return "jsnyder@synesisintl.com"; }
        }

        /// <summary></summary>
        public static string GetSubject()
        {
            try { return ConfigurationManager.AppSettings["Subject"]; }
            catch { return "Alert"; }
        }

        /// <summary></summary>
        public static string GetErrorToAddress()
        {
            try { return ConfigurationManager.AppSettings["ErrorToAddress"]; }
            catch { return "jsnyder@synesisintl.com"; }
        }

        /// <summary></summary>
        public static string GetErrorCCAddress()
        {
            try { return ConfigurationManager.AppSettings["ErrorCCAddress"]; }
            catch { return "jsnyder@synesisintl.com"; }
        }

        /// <summary></summary>
        public static string GetErrrorSubject()
        {
            try { return ConfigurationManager.AppSettings["ErrorSubject"]; }
            catch { return "Error"; }
        }
        /// <summary></summary>
        public static void SendEmail(string message, bool exception)
        {
            try
            {
                if (exception) { if (!GetEmailErrors()) { return; } }
                else { if (!GetEmailAlerts()) { return; } }
                EmailItem email = new EmailItem();
                email.FromAddress = GetFromAddress();
                if (exception)
                {
                    email.ToAddress = GetErrorToAddress();
                    email.CCAddress = GetErrorCCAddress();
                    email.Subject = GetErrrorSubject();
                    email.Body = message;
                }
                else
                {
                    email.ToAddress = GetToAddress();
                    email.CCAddress = GetCCAddress();
                    email.Subject = GetSubject();
                    email.Body = message;
                }
                email.EmailReport();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Email | {ex.Message}");
            }
        }

        #endregion
    }
}

