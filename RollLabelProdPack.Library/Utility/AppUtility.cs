using log4net;
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

namespace RollLabelProdPack.Library.Utility
{
    /// <summary>
    /// Contains production common routines.
    /// </summary>
    public class AppUtility
    {
        #region production common routines

        /// <summary>
        /// Refreshes the issue quantity for a production order.
        /// </summary>
        /// <param name="prodOrder">The production order ID.</param>
        /// <param name="prodLine">The production line.</param>
        /// <param name="productionQty">The production quantity.</param>
        /// <param name="decimalPlaces">The number of decimal places to round the quantity (optional, default is -1).</param>
        /// <returns>A list of <see cref="InventoryIssueDetail"/> representing the planned issue details.</returns>
        /// <exception cref="ApplicationException">Thrown when there is an error getting the production line input material or issue material.</exception>
        /// <remarks>
        /// This method retrieves the necessary inventory details and calculates the planned issue quantity for a production order.
        /// It takes into account the production line, production quantity, and decimal places for rounding the quantity.
        /// The result is a list of planned issue details that can be used for further processing.
        /// </remarks>
        public static List<InventoryIssueDetail> RefreshIssueQty(int prodOrder, string prodLine, decimal productionQty, int decimalPlaces = -1)
        {
            // Initialize logger
            ILog log = LogManager.GetLogger(typeof(AppUtility));

            // Initialize planned issue list
            var plannedIssue = new List<InventoryIssueDetail>();

            // Get the packing material location
            var packingMtlLoc = GetPackingMtlLocation();

            // Log debug information
            if (log.IsDebugEnabled)
            {
                log.Debug("About to call GetProdLineInputMaterial");
                log.Debug("--------------------------------------");
                log.Debug($"prodLine = {prodLine}");
                log.Debug($"packingMtlLoc = {packingMtlLoc}");
            }

            // Retrieve input location material details
            var so = AppData.GetProdLineInputMaterial(prodLine, packingMtlLoc);
            if (!so.SuccessFlag)
                throw new ApplicationException($"Error getting Prod. Line Input Material. Error:{so.ServiceException}");
            var inputLocMaterial = so.ReturnValue as List<InventoryDetail>;

            // Log input location material details
            if (log.IsDebugEnabled)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("inputLocMaterial");
                sb.AppendLine("----------------");
                if (inputLocMaterial.Count == 0)
                    sb.AppendLine("None");
                else
                {
                    foreach (var material in inputLocMaterial)
                    {
                        sb.AppendLine(material.ToString());
                    }
                }
                log.Debug(sb.ToString());
            }

            // Retrieve production order issue material details
            so = AppData.GetProdOrderIssueMaterial(prodOrder, productionQty);
            if (!so.SuccessFlag)
                throw new ApplicationException($"Error getting Issue Material. Error:{so.ServiceException}");
            var prodOrderLines = so.ReturnValue as List<InventoryIssueDetail>;

            // Calculate planned issue details
            foreach (var line in prodOrderLines)
            {
                var qtyReq = line.PlannedIssueQty;

                // Handle rounding of quantity based on decimal places
                if (decimalPlaces != -1)
                {
                    qtyReq = Math.Round(qtyReq, decimalPlaces);
                }
                else
                {
                    qtyReq = Math.Round(qtyReq, 2);
                }

                if (line.PlannedIssueQty > 0) // Scrap is set up as by-product negative line quantity
                {
                    var itemAvail = inputLocMaterial
                        .Where(i => i.ItemCode == line.ItemCode)
                        .OrderBy(i => i.InDate)
                        .ThenBy(i => i.Batch)
                        .ToList();

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
                                    // Handle rounding of item quantity based on decimal places
                                    var itemQty = item.Quantity;
                                    if (decimalPlaces != -1)
                                    {
                                        itemQty = Math.Round(itemQty, decimalPlaces);
                                    }
                                    else
                                    {
                                        itemQty = Math.Round(itemQty, 2);
                                    }

                                    plannedIssue.Add(CreatePlannedIssueDetail(line.ItemCode, line.UOM, line.BaseEntry, line.BaseLine, item.StorageLocation, item.QualityStatus,
                                        item.LUID, item.SSCC, qtyReq, itemQty, item.Batch, 0, line.BatchControlled, line.PackagingMtl));
                                    qtyReq = qtyReq - itemQty;
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


        /// <summary>
        /// Creates a planned issue detail for inventory.
        /// </summary>
        /// <param name="itemCode">The item code.</param>
        /// <param name="uom">The unit of measure.</param>
        /// <param name="baseEntry">The base entry.</param>
        /// <param name="baseLine">The base line.</param>
        /// <param name="storageLocation">The storage location.</param>
        /// <param name="qualityStatus">The quality status.</param>
        /// <param name="luid">The unique identifier.</param>
        /// <param name="sscc">The SSCC.</param>
        /// <param name="availableQty">The available quantity.</param>
        /// <param name="plannedIssueqty">The planned issue quantity.</param>
        /// <param name="batch">The batch.</param>
        /// <param name="shortQty">The short quantity.</param>
        /// <param name="batchControlled">Specifies if the item is batch controlled.</param>
        /// <param name="packagingMtl">Specifies if the item is packaging material.</param>
        /// <returns>An instance of <see cref="InventoryIssueDetail"/> representing the planned issue detail.</returns>
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

        /// <summary>
        /// Retrieves the SAP connection string from the configuration.
        /// </summary>
        /// <returns>The SAP connection string.</returns>
        public static string GetSAPConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SAPConnection"].ConnectionString;
        }

        /// <summary>
        /// Retrieves the PMX connection string from the configuration.
        /// </summary>
        /// <returns>The PMX connection string.</returns>
        public static string GetPMXConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["PMXConnection"].ConnectionString;
        }

        /// <summary>
        /// Retrieves the SQL command timeout value from the configuration.
        /// </summary>
        /// <returns>The SQL command timeout value.</returns>
        public static int GetSqlCommandTimeOut()
        {
            var commandTimeout = 30;
            try
            {
                commandTimeout = int.Parse(ConfigurationManager.AppSettings["SQLCommandTimeout"]);
            }
            catch { }
            return commandTimeout;
        }

        /// <summary>
        /// Tests the SQL connection by attempting to open a connection using the SAP database connection string.
        /// </summary>
        /// <returns>An empty string if the connection is successful; otherwise, returns the error message.</returns>
        public static string TestSQLConnection()
        {
            try
            {
                var databaseConnection = GetSAPConnectionString();
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                {
                    cnx.Open();
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        #endregion

        #region datasets

        /// <summary>
        /// Populates a DataSet using the provided SqlCommand object.
        /// </summary>
        /// <param name="MyCmd">The SqlCommand object used to populate the DataSet.</param>
        /// <returns>A DataSet filled with data from the SqlCommand.</returns>
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
        /// <summary>
        /// Writes a log message to the Event Log.
        /// </summary>
        /// <param name="logMessage">The message to be written to the Event Log.</param>
        /// <param name="eType">The type of the log entry.</param>
        /// <param name="sendEmail">A flag indicating whether to send an email notification.</param>
        public static void WriteToEventLog(string logMessage, EventLogEntryType eType, bool sendEmail)
        {
            // Retrieve logging source and logging settings
            var logSource = GetLoggingSource();
            var logging = GetLogging();

            // Default log name and machine
            var logName = "Application";
            var logMachine = ".";

            // Default event ID
            var eventID = 1000;

            try
            {
                // Check if log entry type is not an error
                if (eType != EventLogEntryType.Error)
                {
                    // If logging is disabled, return early
                    if (!logging)
                    {
                        return;
                    }
                }
                else
                {
                    // Set specific event ID for error log entry
                    eventID = 9000;
                }

                // Create an instance of the EventLog class
                EventLog logEntry = new EventLog(logName, logMachine, logSource);

                // Write the log message with the specified entry type and event ID
                logEntry.WriteEntry(logMessage, eType, eventID);

                // Send email notification if required
                if (sendEmail)
                {
                    SendEmail(logMessage, (eType == EventLogEntryType.Error));
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the execution of the method
                var test = ex.Message;
            }
        }


        /// <summary>
        /// Gets the logging setting from the configuration.
        /// </summary>
        /// <returns>The logging setting value.</returns>
        public static bool GetLogging()
        {
            try
            {
                return bool.Parse(ConfigurationManager.AppSettings["Logging"]);
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Gets the logging source from the configuration.
        /// </summary>
        /// <returns>The logging source value.</returns>
        public static string GetLoggingSource()
        {
            try
            {
                return ConfigurationManager.AppSettings["LoggingSource"];
            }
            catch
            {
                return "Roll Label Prod Pack";
            }
        }


        /// <summary>
        /// Gets the logging text from the configuration.
        /// </summary>
        /// <returns>The logging text value.</returns>
        public static string GetLoggingText()
        {
            try
            {
                return ConfigurationManager.AppSettings["LoggingText"];
            }
            catch
            {
                return "Roll Label Prod Pack";
            }
        }


        /// <summary>
        /// Gets the event ID for triggering logging from the configuration.
        /// </summary>
        /// <returns>The event ID for triggering logging.</returns>
        public static int LoggingEventIDForTrigger()
        {
            try
            {
                return int.Parse(ConfigurationManager.AppSettings["LoggingEventIDForTrigger"].ToString());
            }
            catch
            {
                return 9999;
            }
        }

        /// <summary>
        /// Retrieves the value of the co-pack pallet printer name from the configuration settings.
        /// </summary>
        /// <returns>The value of the co-pack pallet printer name.</returns>
        public static string GetCoPackPalletPrinterName()
        {
            return ConfigurationManager.AppSettings["CoPackPalletPrinterName"];
        }


        /// <summary></summary>
        /// <summary>
        /// Gets the application name for triggering logging from the configuration.
        /// </summary>
        /// <returns>The application name for triggering logging.</returns>
        public static string GetLoggingTriggerAppName()
        {
            try
            {
                return ConfigurationManager.AppSettings["LoggingTriggerAppName"];
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region b1 settings

        /// <summary>
        /// Gets the SAP server name from the configuration.
        /// </summary>
        /// <returns>The SAP server name.</returns>
        public static string GetSAPServerName()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPSERVERNAME"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP company name from the configuration.
        /// </summary>
        /// <returns>The SAP company name.</returns>
        public static string GetSAPCompany()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPCOMPANY"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP user name from the configuration.
        /// </summary>
        /// <returns>The SAP user name.</returns>
        public static string GetSAPUser()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP password from the configuration.
        /// </summary>
        /// <returns>The SAP password.</returns>
        public static string GetSAPPassword()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP user associated with Line 1 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Line 1.</returns>
        public static string GetSAPUserLine1()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_LINE1"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Line 1 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Line 1.</returns>
        public static string GetSAPPassLine1()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_LINE1"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP user associated with Line 2 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Line 2.</returns>
        public static string GetSAPUserLine2()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_LINE2"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Line 2 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Line 2.</returns>
        public static string GetSAPPassLine2()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_LINE2"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP user associated with Line 3 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Line 3.</returns>
        public static string GetSAPUserLine3()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_LINE3"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Line 3 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Line 3.</returns>
        public static string GetSAPPassLine3()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_LINE3"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP user associated with Line 4 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Line 4.</returns>
        public static string GetSAPUserLine4()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_LINE4"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Line 4 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Line 4.</returns>
        public static string GetSAPPassLine4()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_LINE4"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP user associated with Line 5 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Line 5.</returns>
        public static string GetSAPUserLine5()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_LINE5"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Line 5 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Line 5.</returns>
        public static string GetSAPPassLine5()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_LINE5"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP user associated with Line 6 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Line 6.</returns>
        public static string GetSAPUserLine6()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_LINE6"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Line 6 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Line 6.</returns>
        public static string GetSAPPassLine6()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_LINE6"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP user associated with Line 7 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Line 7.</returns>
        public static string GetSAPUserLine7()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_LINE7"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Line 7 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Line 7.</returns>
        public static string GetSAPPassLine7()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_LINE7"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP user associated with Mix Line 1 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Mix Line 1.</returns>
        public static string GetSAPUserMixLine1()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_MIXLINE1"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Mix Line 1 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Mix Line 1.</returns>
        public static string GetSAPPassMixLine1()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_MIXLINE1"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP user associated with Mix Line 2 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Mix Line 2.</returns>
        public static string GetSAPUserMixLine2()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_MIXLINE2"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Mix Line 2 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Mix Line 2.</returns>
        public static string GetSAPPassMixLine2()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_MIXLINE2"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP user associated with Mix Line 3 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Mix Line 3.</returns>
        public static string GetSAPUserMixLine3()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_MIXLINE3"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Mix Line 3 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Mix Line 3.</returns>
        public static string GetSAPPassMixLine3()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_MIXLINE3"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP user associated with Mask Line 1 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Mask Line 1.</returns>
        public static string GetSAPUserMaskLine1()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_MASKLINE1"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Mask Line 1 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Mask Line 1.</returns>
        public static string GetSAPPassMaskLine1()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_MASKLINE1"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP user associated with Mask Line 2 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Mask Line 2.</returns>
        public static string GetSAPUserMaskLine2()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_MASKLINE2"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Mask Line 2 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Mask Line 2.</returns>
        public static string GetSAPPassMaskLine2()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_MASKLINE2"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP user associated with Mask Line 3 from the configuration.
        /// </summary>
        /// <returns>The SAP user associated with Mask Line 3.</returns>
        public static string GetSAPUserMaskLine3()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPUSER_MASKLINE3"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Gets the SAP password associated with Mask Line 3 from the configuration.
        /// </summary>
        /// <returns>The SAP password associated with Mask Line 3.</returns>
        public static string GetSAPPassMaskLine3()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPPASS_MASKLINE3"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the SAP license server from the configuration.
        /// </summary>
        /// <returns>The SAP license server.</returns>
        public static string GetSAPLicenseServer()
        {
            try
            {
                return ConfigurationManager.AppSettings["SAPLICENSESERVER"];
            }
            catch
            {
                return "";
            }
        }


        /// <summary></summary>
        /// <summary>
        /// Gets the flag indicating whether to use transactions for SAP operations from the configuration.
        /// </summary>
        /// <returns><c>true</c> if transactions should be used; otherwise, <c>false</c>.</returns>
        public static bool GetSAPUseTransactions()
        {
            try
            {
                return bool.Parse(ConfigurationManager.AppSettings["UseTransactions"]);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the flag indicating whether to rollback SAP operations for testing purposes from the configuration.
        /// </summary>
        /// <returns><c>true</c> if rollback should be enabled for testing; otherwise, <c>false</c>.</returns>
        public static bool GetSAPRollbackForTesting()
        {
            try
            {
                return bool.Parse(ConfigurationManager.AppSettings["RollBackForTesting"]);
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Retrieves the SAP database type from the configuration.
        /// </summary>
        /// <returns>The SAP database type.</returns>
        public static string GetSAPDBType()
        {
            return ConfigurationManager.AppSettings["SAPDBTYPE"];
        }


        /// <summary>
        /// Retrieves the SAP username and password based on the specified film machine number.
        /// </summary>
        /// <param name="machineNo">The film machine number.</param>
        /// <returns>A KeyValuePair containing the SAP username and password.</returns>
        /// <remarks>
        /// The GetUserNameAndPasswordFilm method retrieves the SAP username and password based on the 
        /// specified film machine number. It takes in a machineNo parameter representing the film machine number 
        /// and returns a KeyValuePair containing the corresponding SAP username and password.
        /// By default, it uses the SAP username and password obtained from the GetSAPUser and 
        /// GetSAPPassword methods. However, if a specific machine number is provided, it retrieves 
        /// the corresponding SAP username and password using the corresponding GetSAPUserLineX 
        /// and GetSAPPassLineX methods.
        /// </remarks>
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


        /// <summary>
        /// Retrieves the SAP username and password based on the specified machine number for mixing.
        /// </summary>
        /// <param name="machineNo">The machine number.</param>
        /// <returns>A KeyValuePair containing the SAP username and password for mixing.</returns>
        /// <remarks>
        /// The GetUserNameAndPasswordMix method in the AppUtility class retrieves the SAP username and password 
        /// based on the specified resmix machine number.
        /// It takes in a machineNo parameter representing the resmix machine number and returns a KeyValuePair 
        /// containing the corresponding SAP username and password for the resmix machine. 
        /// By default, it uses the SAP username and password obtained from the GetSAPUser and GetSAPPassword 
        /// methods. However, if a specific machine number is provided, it retrieves the corresponding SAP 
        /// username and password for the resmix machine using the corresponding GetSAPUserMixLineX 
        /// and GetSAPPassMixLineX methods.
        /// </remarks>
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


        /// <summary>
        /// Retrieves the SAP username and password based on the specified mask machine number.
        /// </summary>
        /// <param name="machineNo">The mask machine number.</param>
        /// <returns>A KeyValuePair containing the SAP username and password for the specified mask machine.</returns>
        /// <remarks>
        /// The GetUserNameAndPasswordMask method in the AppUtility class retrieves the SAP username and password based on the specified mask machine number.
        /// It takes in a machineNo parameter representing the mask machine number and returns a KeyValuePair 
        /// containing the corresponding SAP username and password for that mask machine. By default, it uses the 
        /// SAP username and password obtained from the GetSAPUser and GetSAPPassword methods.
        /// However, if a specific mask machine number is provided, it retrieves the corresponding SAP username 
        /// and password for that mask machine using the corresponding GetSAPUserMaskLineX and GetSAPPassMaskLineX methods.
        /// </remarks>
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

        /// <summary>
        /// Retrieves the factory code from the configuration settings.
        /// </summary>
        /// <returns>The factory code.</returns>
        public static string GetFactoryCode()
        {
            return ConfigurationManager.AppSettings["FactoryCode"];
        }

        /// <summary>
        /// Retrieves the default item code from the configuration settings.
        /// </summary>
        /// <returns>The default item code.</returns>
        public static string GetDefaultItemCode()
        {
            return ConfigurationManager.AppSettings["DefaultItemCode"];
        }

        /// <summary>
        /// Retrieves the default item description from the configuration settings.
        /// </summary>
        /// <returns>The default item description.</returns>
        public static string GetDefaultItemDesc()
        {
            return ConfigurationManager.AppSettings["DefaultItemDesc"];
        }

        /// <summary>
        /// Retrieves the value of the last batch from the configuration settings.
        /// </summary>
        /// <returns>The value of the last batch.</returns>
        public static string GetLastBatch()
        {
            return ConfigurationManager.AppSettings["LastBatch"];
        }

        /// <summary>
        /// Retrieves the value of the last roll from the configuration settings.
        /// </summary>
        /// <returns>The value of the last roll.</returns>
        public static string GetLastRoll()
        {
            return ConfigurationManager.AppSettings["LastRoll"];
        }

        /// <summary>
        /// Retrieves the value of the last pallet number from the configuration settings.
        /// </summary>
        /// <returns>The value of the last pallet number.</returns>
        public static string GetLastPalletNo()
        {
            return ConfigurationManager.AppSettings["LastPalletNo"];
        }

        /// <summary>
        /// Retrieves the value of the default IRMS from the configuration settings.
        /// </summary>
        /// <returns>The value of the default IRMS.</returns>
        public static string GetDefaultIRMS()
        {
            return ConfigurationManager.AppSettings["DefaultIRMS"];
        }

        /// <summary>
        /// Retrieves the value of the default machine number from the configuration settings.
        /// </summary>
        /// <returns>The value of the default machine number.</returns>
        public static string GetDefaultMachineNo()
        {
            return ConfigurationManager.AppSettings["DefaultMachineNo"];
        }

        /// <summary>
        /// Retrieves the value of the default material code from the configuration settings.
        /// </summary>
        /// <returns>The value of the default material code.</returns>
        public static string GetDefaultMaterialCode()
        {
            return ConfigurationManager.AppSettings["DefaultMaterialCode"];
        }

        /// <summary>
        /// Retrieves the value of the default number of slit positions from the configuration settings.
        /// </summary>
        /// <returns>The value of the default number of slit positions.</returns>
        public static string GetDefaultNoOfSlitPositions()
        {
            return ConfigurationManager.AppSettings["DefaultNoOfSlitPositions"];
        }

        /// <summary>
        /// Retrieves the value of the default shift from the configuration settings.
        /// </summary>
        /// <returns>The value of the default shift.</returns>
        public static string GetDefaultShift()
        {
            return ConfigurationManager.AppSettings["DefaultShift"];
        }

        /// <summary>
        /// Retrieves the value of the default die from the configuration settings.
        /// </summary>
        /// <returns>The value of the default die.</returns>
        public static string GetDefaultDie()
        {
            return ConfigurationManager.AppSettings["DefaultDie"];
        }

        /// <summary>
        /// Retrieves the value of the default product name from the configuration settings.
        /// </summary>
        /// <returns>The value of the default product name.</returns>
        public static string GetDefaultProdName()
        {
            return ConfigurationManager.AppSettings["DefaultProdName"];
        }

        /// <summary>
        /// Retrieves the value of the print location for the 4x6 roll label from the configuration settings.
        /// </summary>
        /// <returns>The value of the print location for the 4x6 roll label.</returns>
        public static string GetPrintLocRollLabel4by6()
        {
            return ConfigurationManager.AppSettings["PrintLocRollLabel4by6"];
        }

        /// <summary>
        /// Retrieves the value of the print location for the 1x6 roll label from the configuration settings.
        /// </summary>
        /// <returns>The value of the print location for the 1x6 roll label.</returns>
        public static string GetPrintLocRollLabel1by6()
        {
            return ConfigurationManager.AppSettings["PrintLocRollLabel1by6"];
        }

        /// <summary>
        /// Retrieves the value of the BT trigger location from the configuration settings.
        /// </summary>
        /// <returns>The value of the BT trigger location.</returns>
        public static string GetBTTriggerLoc()
        {
            return ConfigurationManager.AppSettings["BTTriggerLoc"];
        }

        /// <summary>
        /// Retrieves the value of the pack print location from the configuration settings.
        /// </summary>
        /// <returns>The value of the pack print location.</returns>
        public static string GetPrintLocPack()
        {
            return ConfigurationManager.AppSettings["PrintLocPack"];
        }

        /// <summary>
        /// Retrieves the value of the pack printer name from the configuration settings.
        /// </summary>
        /// <returns>The value of the pack printer name.</returns>
        public static string GetPackPrinterName()
        {
            return ConfigurationManager.AppSettings["PackPrinterName"];
        }

        /// <summary>
        /// Retrieves the value of the tub pallet printer name from the configuration settings.
        /// </summary>
        /// <returns>The value of the tub pallet printer name.</returns>
        public static string GetTubPalletPrinterName()
        {
            return ConfigurationManager.AppSettings["TubPalletPrinterName"];
        }

        /// <summary>
        /// Retrieves the value of the label print extension from the configuration settings.
        /// </summary>
        /// <returns>The value of the label print extension.</returns>
        public static string GetLabelPrintExtension()
        {
            return ConfigurationManager.AppSettings["LabelPrintExtension"];
        }

        /// <summary>
        /// Retrieves the default label file for the P&G 4-inch label format from the configuration settings.
        /// </summary>
        /// <returns>The default label file for the P&G 4-inch label format.</returns>
        public static string GetPGDefault4InchLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefault4InchLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default label file for the P&G 1-inch label format from the configuration settings.
        /// </summary>
        /// <returns>The default label file for the P&G 1-inch label format.</returns>
        public static string GetPGDefault1InchLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefault1InchLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default label file for the combined P&G label format from the configuration settings.
        /// </summary>
        /// <returns>The default label file for the combined P&G label format.</returns>
        public static string GetPGDefaultCombLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultCombLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default roll label format file name for the Medline roll label from the configuration settings.
        /// </summary>
        /// <returns>The default roll label format file name for the Medline roll label</returns>
        public static string GetMedlineDefaultRollLabelFormat()
        {
            return ConfigurationManager.AppSettings["MedlineDefaultRollLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default roll label format file name for the Rockline roll label from the configuration settings.
        /// </summary>
        /// <returns>The default roll label format file name for the Rockline roll label.</returns>
        public static string GetRocklineDefaultRollLabelFormat()
        {
            return ConfigurationManager.AppSettings["RocklineDefaultRollLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default label file for the P&G scrap label format from the configuration settings.
        /// </summary>
        /// <returns>The default label file for the P&G scrap label format.</returns>
        public static string GetPGDefaultScrapLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultScrapLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default label file for the P&G Resmix label format from the configuration settings.
        /// </summary>
        /// <returns>The default label file for the P&G Resmix label format.</returns>
        public static string GetPGDefaultResmixLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultResmixLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default label file for the P&G pack label format from the configuration settings.
        /// </summary>
        /// <returns>The default label file for the P&G pack label format.</returns>
        public static string GetPGDefaultPackLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultPackLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default label file for the Medline pack label format from the configuration settings.
        /// </summary>
        /// <returns>The default label file for the Medline pack label format</returns>
        public static string GetMedlineDefaultPackLabelFormat()
        {
            return ConfigurationManager.AppSettings["MedlineDefaultPackLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default label file for the Rockline pack label format from the configuration settings.
        /// </summary>
        /// <returns>The default label file for the Rockline pack label format.</returns>
        public static string GetRocklineDefaultPackLabelFormat()
        {
            return ConfigurationManager.AppSettings["RocklineDefaultPackLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default label file for the P&G tub/case label format from the configuration settings.
        /// </summary>
        /// <returns>The default label file for the P&G tub/case label format.</returns>
        public static string GetPGDefaultTubCaseLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultTubCaseLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default label file for the P&G co-pack label format from the configuration settings.
        /// </summary>
        /// <returns>The default label file for the P&G co-pack label format.</returns>
        public static string GetPGDefaultCoPackLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultCoPackLabelFormat"];
        }

        /// <summary>
        /// Retrieves the default label file for the P&G tub pallet label format from the configuration settings.
        /// </summary>
        /// <returns>The default label file for the P&G tub pallet label format.</returns>
        public static string GetPGDefaultTubPalletLabelFormat()
        {
            return ConfigurationManager.AppSettings["PGDefaultTubPalletLabelFormat"];
        }

        /// <summary>
        /// Retrieves the old/default label file for the P&G pack label format from the configuration settings.
        /// </summary>
        /// <returns>The old/default label file for the P&G pack label format.</returns>
        public static string GetPGDefaultPackLabelFormatOld()
        {
            return ConfigurationManager.AppSettings["PGDefaultPackLabelFormatOld"];
        }

        /// <summary>
        /// Retrieves the value of the "SupplierId" configuration setting.
        /// </summary>
        /// <returns>The SupplierId value.</returns>
        public static string GetSupplierId()
        {
            return ConfigurationManager.AppSettings["SupplierId"];
        }

        /// <summary>
        /// Retrieves the value of the "MaskUOMLabel" configuration setting.
        /// </summary>
        /// <returns>The MaskUOMLabel value.</returns>
        public static string GetMaskUOMLabel()
        {
            return ConfigurationManager.AppSettings["MaskUOMLabel"];
        }

        public static string GetMedlineCustomerID()
        {
            return ConfigurationManager.AppSettings["MedlineCustomerID"];
        }

        public static string GetRocklineCustomerID()
        {
            return ConfigurationManager.AppSettings["RocklineCustomerID"];
        }

        /// <summary>
        /// Gets the Yanjan production month code based on the order start date.
        /// </summary>
        /// <param name="orderStartDate">The order start date.</param>
        /// <returns>The Yanjan production month code.</returns>
        /// <remarks>
        /// The method converts the month value of the orderStartDate parameter to a string, 
        /// and then uses a switch statement to determine the corresponding Yanjan production month code. 
        /// Months 1 to 9 have the same code as their numeric value, while months 10, 11, and 12 have 
        /// specific codes ("O", "N", and "D" respectively). The method returns the determined Yanjan 
        /// production month code.
        /// </remarks>
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


        /// <summary>
        /// Gets the order batch number from a character.
        /// </summary>
        /// <param name="batchNoChar">The character representing the batch number.</param>
        /// <returns>The order batch number.</returns>
        /// <remarks>
        /// The method first checks if the batchNoChar character is a digit by using a regular expression pattern 
        /// ("^[0-9]*$") and the IsMatch method. If it is a digit, it converts the character to a string, then 
        /// to an integer using Convert.ToInt32.
        /// 
        /// If the batchNoChar character is not a digit, it assumes it is a letter. It converts the letter to 
        /// uppercase using char.ToUpper, subtracts the ASCII value of 'A' (64) from the uppercase letter, 
        /// adds 10 to the result, and assigns it to the batchNo variable.
        /// 
        /// Finally, the method returns the calculated batchNo.
        /// 
        /// Please note that the method assumes that batchNoChar is a valid character representing a batch number.
        /// </remarks>
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

        /// <summary>
        /// Gets the name of the current method.
        /// </summary>
        /// <returns>The name of the current method.</returns>
        /// <remarks>
        /// The method uses the StackTrace class to obtain the current stack trace, and then it retrieves 
        /// the second stack frame using the GetFrame(1) method. The index 1 is used to skip the GetCurrentMethod 
        /// frame and get the calling frame.
        /// 
        /// From the obtained stack frame, the GetMethod().Name is used to retrieve the name of the method.
        /// 
        /// The [MethodImpl(MethodImplOptions.NoInlining)] attribute is used to ensure that the method itself 
        /// is not included in the stack trace. This attribute prevents the GetCurrentMethod method from being 
        /// included in the stack trace, so the name of the calling method is accurately retrieved.
        /// 
        /// Please note that the method may not work correctly in certain scenarios, such as when the code is 
        /// optimized or when using asynchronous or dynamic features.
        /// </remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        /// <summary>
        /// Retrieves the value of the "DefaultPackCopies" setting from the application configuration file.
        /// </summary>
        /// <returns>The value of the "DefaultPackCopies" setting.</returns>
        public static string GetDefaultPackCopies()
        {
            return ConfigurationManager.AppSettings["DefaultPackCopies"];
        }

        /// <summary>
        /// Retrieves the default number of co-pack pallet label copies from the application configuration file.
        /// </summary>
        /// <returns>A default value of 2 if DefaultCoPackCopies isn't specified or &lt;= 0; otherwise the
        /// value of DefaultCoPackCopies from the configuration file as an integer.</returns>
        public static int GetDefaultCoPackCopies()
        {
            var defaultCoPackCopies = 2;
            var defaultCoPackCopiesStr = ConfigurationManager.AppSettings["DefaultCoPackCopies"];
            if (!string.IsNullOrEmpty(defaultCoPackCopiesStr))
            {
                var defaultCoPackCopiesConfigValue = -1;
                if (int.TryParse(defaultCoPackCopiesStr, out defaultCoPackCopiesConfigValue))
                {
                    if (defaultCoPackCopiesConfigValue > 0)
                    {
                        defaultCoPackCopies = defaultCoPackCopiesConfigValue;
                    }
                }
            }
            return defaultCoPackCopies;
        }

        /// <summary>
        /// Retrieves the value of the "PackingMtlLocation" setting from the application configuration file.
        /// </summary>
        /// <returns>The value of the "PackingMtlLocation" setting.</returns>
        public static string GetPackingMtlLocation()
        {
            return ConfigurationManager.AppSettings["PackingMtlLocation"];
        }

        /// <summary>
        /// Retrieves the value of the "ScrapOffsetCode" setting from the application configuration file.
        /// </summary>
        /// <returns>The value of the "ScrapOffsetCode" setting.</returns>
        public static string GetScrapOffsetCode()
        {
            return ConfigurationManager.AppSettings["ScrapOffsetCode"];
        }

        /// <summary>
        /// Retrieves the value of the "ScrapLocCode" setting from the application configuration file.
        /// </summary>
        /// <returns>The value of the "ScrapLocCode" setting.</returns>
        public static string GetScrapLocCode()
        {
            return ConfigurationManager.AppSettings["ScrapLocCode"];
        }

        /// <summary>
        /// Retrieves the value of the "HoldStatus" setting from the application configuration file.
        /// </summary>
        /// <returns>The value of the "HoldStatus" setting.</returns>
        public static string GetHoldStatus()
        {
            return ConfigurationManager.AppSettings["HoldStatus"];
        }

        /// <summary>
        /// Retrieves the value of the "HoldLocation" setting from the application configuration file.
        /// </summary>
        /// <returns>The value of the "HoldLocation" setting.</returns>
        public static string GetHoldLocation()
        {
            return ConfigurationManager.AppSettings["HoldLocation"];
        }

        /// <summary>
        /// Retrieves the value of the "ScrapStatus" setting from the application configuration file.
        /// </summary>
        /// <returns>The value of the "ScrapStatus" setting.</returns>
        public static string GetScrapStatus()
        {
            return ConfigurationManager.AppSettings["ScrapStatus"];
        }

        /// <summary>
        /// Retrieves the value of the "DefaultStatus" setting from the application configuration file.
        /// </summary>
        /// <returns>The value of the "DefaultStatus" setting.</returns>
        public static string GetDefaultStatus()
        {
            return ConfigurationManager.AppSettings["DefaultStatus"];
        }

        /// <summary>
        /// Retrieves the value of the "DefaultUom" setting from the application configuration file.
        /// </summary>
        /// <returns>The value of the "DefaultUom" setting.</returns>
        public static string GetDefaultUom()
        {
            return ConfigurationManager.AppSettings["DefaultUom"];
        }

        /// <summary>
        /// Retrieves the value of the "Resmix001ToLines" setting from the application configuration file.
        /// </summary>
        /// <returns>The value of the "Resmix001ToLines" setting.</returns>
        public static string GetResmix001ToLines()
        {
            return ConfigurationManager.AppSettings["Resmix001ToLines"];
        }

        /// <summary>
        /// Retrieves the value of the "Resmix002ToLines" setting from the application configuration file.
        /// </summary>
        /// <returns>The value of the "Resmix002ToLines" setting.</returns>
        public static string GetResmix002ToLines()
        {
            return ConfigurationManager.AppSettings["Resmix002ToLines"];
        }

        /// <summary>
        /// Retrieves the value of the "GenericTubLineCode" setting from the application configuration file.
        /// If the setting is not found or is null, the default value "TUB" is returned.
        /// </summary>
        /// <returns>The value of the "GenericTubLineCode" setting, or "TUB" if the setting is not found or is null.</returns>
        public static string GetGenericTubLineCode()
        {
            var genericTubLineCode = ConfigurationManager.AppSettings["GenericTubLineCode"];
            return genericTubLineCode == null ? "TUB" : genericTubLineCode;
        }

        /// <summary>
        /// Retrieves the value of the "GenericTubLineMachineNo" setting from the application configuration file.
        /// If the setting is not found or is null, the default value "T0" is returned.
        /// </summary>
        /// <returns>The value of the "GenericTubLineMachineNo" setting, or "T0" if the setting is not found or is null.</returns>
        public static string GetGenericTubLineMachineNo()
        {
            var genericTubLineMachineNo = ConfigurationManager.AppSettings["GenericTubLineMachineNo"];
            return genericTubLineMachineNo == null ? "T0" : genericTubLineMachineNo;
        }

        /// <summary>
        /// Retrieves the value of the "GenericTubInputLocationCode" setting from the application configuration file.
        /// If the setting is not found or is null, the default value "TUBRAW1" is returned.
        /// </summary>
        /// <returns>The value of the "GenericTubInputLocationCode" setting, or "TUBRAW1" if the setting is not found or is null.</returns>
        public static string GetGenericTubInputLocationCode()
        {
            var genericTubInputLocationCode = ConfigurationManager.AppSettings["GenericTubInputLocationCode"];
            return genericTubInputLocationCode == null ? "TUBRAW1" : genericTubInputLocationCode;
        }

        /// <summary>
        /// Retrieves the value of the "GenericTubOutputLocationCode" setting from the application configuration file.
        /// If the setting is not found or is null, the default value "TUBFIN1" is returned.
        /// </summary>
        /// <returns>The value of the "GenericTubOutputLocationCode" setting, or "TUBFIN1" if the setting is not found or is null.</returns>
        public static string GetGenericTubOutputLocationCode()
        {
            var genericTubOutputLocationCode = ConfigurationManager.AppSettings["GenericTubOutputLocationCode"];
            return genericTubOutputLocationCode == null ? "TUBFIN1" : genericTubOutputLocationCode;
        }

        /// <summary>
        /// Retrieves the value of the "GenericTubPrinter" setting from the application configuration file.
        /// If the setting is not found or is null, the default value "DT-026_Zebra" is returned.
        /// </summary>
        /// <returns>The value of the "GenericTubPrinter" setting, or "DT-026_Zebra" if the setting is not found or is null.</returns>
        public static string GetGenericTubPrinter()
        {
            var genericTubPrinter = ConfigurationManager.AppSettings["GenericTubPrinter"];
            return genericTubPrinter == null ? "DT-026_Zebra" : genericTubPrinter;
        }

        #endregion

        #region html toast

        /// <summary>
        /// Generates an HTML toast message with the specified title, text, and type.
        /// The default type is "w3-warning".
        /// </summary>
        /// <param name="title">The title of the toast message.</param>
        /// <param name="text">The text content of the toast message.</param>
        /// <param name="type">The type of the toast message. Defaults to "w3-warning".</param>
        /// <returns>An HTML string representing the toast message.</returns>
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

        /// <summary>
        /// Retrieves the SMTP port number from the application configuration file.
        /// If the port number is not specified or cannot be parsed as an integer, the default port number 25 is returned.
        /// </summary>
        /// <returns>The SMTP port number.</returns>
        public static int GetSMTPPort()
        {
            try
            {
                return int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
            }
            catch
            {
                return 25;
            }
        }

        /// <summary>
        /// Retrieves the SMTP host name from the application configuration file.
        /// If the host name is not specified, the default host name "SII-EXS" is returned.
        /// </summary>
        /// <returns>The SMTP host name.</returns>
        public static string GetSMTPHost()
        {
            try
            {
                return ConfigurationManager.AppSettings["SMTPHost"];
            }
            catch
            {
                return "SII-EXS";
            }
        }

        /// <summary>
        /// Retrieves the SMTP credentials usage flag from the application configuration file.
        /// If the flag is not specified, the default value of false is returned.
        /// </summary>
        /// <returns>A boolean value indicating whether SMTP credentials should be used.</returns>
        public static bool GetSMTPUseCredentials()
        {
            try
            {
                return bool.Parse(ConfigurationManager.AppSettings["SMTPUseCredentials"]);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves the SMTP user from the application configuration file.
        /// If the user is not specified, an empty string is returned.
        /// </summary>
        /// <returns>The SMTP user.</returns>
        public static string GetSMTPUser()
        {
            try
            {
                return ConfigurationManager.AppSettings["SMTPUser"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Retrieves the SMTP password from the application configuration file.
        /// If the password is not specified, an empty string is returned.
        /// </summary>
        /// <returns>The SMTP password.</returns>
        public static string GetSMTPPass()
        {
            try
            {
                return ConfigurationManager.AppSettings["SMTPPass"];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Retrieves the flag indicating whether to use TLS (Transport Layer Security) for SMTP communication from the application configuration file.
        /// If the flag is not specified or parsing fails, false is returned.
        /// </summary>
        /// <returns>True if TLS should be used for SMTP communication, otherwise false.</returns>
        public static bool GetSMTPUseTSL()
        {
            try
            {
                return bool.Parse(ConfigurationManager.AppSettings["SMTPUseTSL"]);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves the flag indicating whether email alerts should be enabled from the application configuration file.
        /// If the flag is not specified or parsing fails, false is returned.
        /// </summary>
        /// <returns>True if email alerts should be enabled, otherwise false.</returns>
        public static bool GetEmailAlerts()
        {
            try
            {
                return bool.Parse(ConfigurationManager.AppSettings["EmailAlerts"]);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves the flag indicating whether email notifications for errors should be enabled from the application configuration file.
        /// If the flag is not specified or parsing fails, false is returned.
        /// </summary>
        /// <returns>True if email notifications for errors should be enabled, otherwise false.</returns>
        public static bool GetEmailErrors()
        {
            try
            {
                return bool.Parse(ConfigurationManager.AppSettings["EmailErrors"]);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves the email address to be used as the "From" address for outgoing emails from the application configuration file.
        /// If the email address is not specified, the default address "sap@synesisintl.com" is returned.
        /// </summary>
        /// <returns>The "From" email address.</returns>
        public static string GetFromAddress()
        {
            try
            {
                return ConfigurationManager.AppSettings["FromAddress"];
            }
            catch
            {
                return "sap@synesisintl.com";
            }
        }

        /// <summary>
        /// Retrieves the email address to which the outgoing emails should be sent, from the application configuration file.
        /// If the email address is not specified, the default address "jsnyder@synesisintl.com" is returned.
        /// </summary>
        /// <returns>The email address where the emails should be sent.</returns>
        public static string GetToAddress()
        {
            try
            {
                return ConfigurationManager.AppSettings["ToAddress"];
            }
            catch
            {
                return "jsnyder@synesisintl.com";
            }
        }

        /// <summary>
        /// Retrieves the email address to which the outgoing emails should be CC'd, from the application configuration file.
        /// If the email address is not specified, the default address "jsnyder@synesisintl.com" is returned.
        /// </summary>
        /// <returns>The email address to which the emails should be CC'd.</returns>
        public static string GetCCAddress()
        {
            try
            {
                return ConfigurationManager.AppSettings["CCAddress"];
            }
            catch
            {
                return "jsnyder@synesisintl.com";
            }
        }

        /// <summary>
        /// Retrieves the subject line for outgoing emails, from the application configuration file.
        /// If the subject line is not specified, the default subject "Alert" is returned.
        /// </summary>
        /// <returns>The subject line for outgoing emails.</returns>
        public static string GetSubject()
        {
            try
            {
                return ConfigurationManager.AppSettings["Subject"];
            }
            catch
            {
                return "Alert";
            }
        }


        /// <summary>
        /// Retrieves the email address where error notifications should be sent, from the application configuration file.
        /// If the email address is not specified, the default address "jsnyder@synesisintl.com" is returned.
        /// </summary>
        /// <returns>The email address where error notifications should be sent.</returns>
        public static string GetErrorToAddress()
        {
            try
            {
                return ConfigurationManager.AppSettings["ErrorToAddress"];
            }
            catch
            {
                return "jsnyder@synesisintl.com";
            }
        }

        /// <summary>
        /// Retrieves the email address where error notifications should be carbon-copied (CC), from the application configuration file.
        /// If the email address is not specified, the default address "jsnyder@synesisintl.com" is returned.
        /// </summary>
        /// <returns>The email address where error notifications should be carbon-copied (CC).</returns>
        public static string GetErrorCCAddress()
        {
            try
            {
                return ConfigurationManager.AppSettings["ErrorCCAddress"];
            }
            catch
            {
                return "jsnyder@synesisintl.com";
            }
        }

        /// <summary>
        /// Retrieves the subject line for error notifications, from the application configuration file.
        /// If the subject line is not specified, the default value "Error" is returned.
        /// </summary>
        /// <returns>The subject line for error notifications.</returns>
        public static string GetErrorSubject()
        {
            try
            {
                return ConfigurationManager.AppSettings["ErrorSubject"];
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// Sends an email notification with the specified message and exception flag.
        /// If the exception flag is set to true, the email is sent only if email errors are enabled.
        /// If the exception flag is set to false, the email is sent only if email alerts are enabled.
        /// </summary>
        /// <param name="message">The message to be included in the email.</param>
        /// <param name="exception">A flag indicating whether the email is for an exception or an alert.</param>
        /// <exception cref="Exception">Thrown when an error occurs while sending the email.</exception>
        public static void SendEmail(string message, bool exception)
        {
            try
            {
                if (exception)
                {
                    // Check if email errors are enabled
                    if (!GetEmailErrors())
                    {
                        return;
                    }
                }
                else
                {
                    // Check if email alerts are enabled
                    if (!GetEmailAlerts())
                    {
                        return;
                    }
                }

                EmailItem email = new EmailItem();
                email.FromAddress = GetFromAddress();

                if (exception)
                {
                    // Set email properties for exception
                    email.ToAddress = GetErrorToAddress();
                    email.CCAddress = GetErrorCCAddress();
                    email.Subject = GetErrorSubject();
                    email.Body = message;
                }
                else
                {
                    // Set email properties for alert
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

