using log4net;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace RollLabelProdPack.Library.Data
{
    /// <summary>
    /// Handles database operations
    /// </summary>
    public class AppData
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

        /// <summary>
        /// Retrieves production order details for a given order number.
        /// </summary>
        /// <param name="orderNo">The order number of the production order.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the retrieved production order details.</returns>
        public static ServiceOutput GetProdOrder(int orderNo)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getProdOrder", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@orderNo", orderNo);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);

                    // Extract the production order details from the result set and create a RollLabelData object
                    RollLabelData prodOrder = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                        new RollLabelData
                        {
                            // Set properties based on retrieved data
                            StartDate = row.Field<DateTime>("StartDate"),
                            DueDate = row.Field<DateTime>("DueDate"),
                            NoOfSlits = row.Field<int>("NoOfSlits"),
                            ItemCode = row.Field<string>("ItemCode"),
                            ItemDescription = row.Field<string>("ItemDescription"),
                            IRMS = row.Field<string>("IRMS"),
                            FactoryCode = row.Field<string>("FactoryCode"),
                            ProductionLine = row.Field<string>("ProductionLine"),
                            ProductionMachineNo = row.Field<string>("ProductionMachineNo"),
                            MaterialCode = row.Field<string>("MaterialCode"),
                            ProductName = row.Field<string>("ProductName"),
                            BatchNo = row.Field<string>("BatchNo"),
                            SAPOrderNo = row.Field<int>("SAPOrderNo"),
                            YJNOrder = row.Field<string>("YJNOrder"),
                            OrderDisplay = string.IsNullOrEmpty(row.Field<string>("YJNOrder")) ? row.Field<int>("SAPOrderNo").ToString() : $"{row.Field<int>("SAPOrderNo").ToString()} - {row.Field<string>("YJNOrder")}",
                            JumboRollNo = row.Field<int>("JumboRoll"),
                            AperatureDieNo = row.Field<string>("AperatureDieNo"),
                            SAPDocEntry = row.Field<int>("DocEntry"),
                            InputLoc = row.Field<string>("InputLoc"),
                            OutputLoc = row.Field<string>("OutputLoc"),
                            Printer = row.Field<string>("Printer"),
                            DefaultQualityStatus = row.Field<string>("DefaultQualityStatus"),
                            MinRollKgs = row.Field<decimal>("MinRollKgs"),
                            MaxRollKgs = row.Field<decimal>("MaxRollKgs"),
                            ScrapItem = row.Field<string>("ScrapItem"),
                            ScrapItemName = row.Field<string>("ScrapItemName"),
                            ScrapLine = row.Field<int>("ScrapLine"),
                            TargetRolls = row.Field<int>("TargetRolls"),
                            InvRolls = row.Field<int>("InvRolls")
                        }).First();

                    serviceOutput.ReturnValue = prodOrder;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";

                // Log the exception
                log.Error(ex.Message, ex);
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves the co-pack pallet labels.
        /// </summary>
        /// <param name="isReprint">Specifies whether to retrieve reprint labels.</param>
        /// <param name="order">The order to filter the labels (optional).</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the retrieved co-pack pallet labels.</returns>
        public static ServiceOutput GetCoPackPalletLabels(bool isReprint, string order)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var noOfCopies = Convert.ToInt32(AppUtility.GetDefaultCoPackCopies());
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getCoPackPalletLabels", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@rePrint", isReprint ? 1 : 0);
                    cmd.Parameters.AddWithValue("@order", order);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<CoPackPalletLabel> coPackPalletLabels = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                        new CoPackPalletLabel
                        {
                            // Set properties based on retrieved data
                            ID = row.Field<int>("ID"),
                            PMXSSCC = row.Field<string>("PMXSSCC"),
                            ItemCode = row.Field<string>("ItemCode"),
                            ItemName = row.Field<string>("ItemName"),
                            IRMS = row.Field<string>("IRMS"),
                            YJNOrder = row.Field<string>("YJNOrder"),
                            SSCC = row.Field<string>("SSCC"),
                            SAPOrder = row.Field<int>("SAPOrder"),
                            LotNo = $"YJWR{row.Field<string>("YJNOrder")}",
                            PalletType = "NONE",
                            Printed = row.Field<string>("Printed") == "Y" ? true : false,
                            Created = ConvertSAPDateAndTime(row["DateCreated"], row["TimeCreated"]),
                            ProductionDate = row.Field<DateTime>("ProductionDate"),
                            Qty = row.Field<decimal>("Qty"),
                            MaxCasesPerPack = row.Field<int>("MaxCasesPerPack"),
                            Copies = noOfCopies,
                            Employee = ""
                        }).ToList();

                    serviceOutput.ReturnValue = coPackPalletLabels;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves a list of open production orders filtered by item group.
        /// </summary>
        /// <param name="itemGroupFilter">The item group filter to apply to the retrieval.</param>
        /// <param name="customerCode">Customer Code</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the list of open production orders.</returns>
        public static ServiceOutput GetOpenProdOrders(string itemGroupFilter, string customerCode = "")
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            //var numberOfAppenders = log.Logger.Repository.GetAppenders().Count();
            //MessageBox.Show($"{numberOfAppenders} appenders defined");
            //MessageBox.Show($"log.IsDebugEnabled = {log.IsDebugEnabled}");
            log.Debug($"itemGroupFilter = {itemGroupFilter}");
            log.Debug($"customerCode = {customerCode}");
            log.Debug($"databaseConnection = {databaseConnection}");
            log.Debug($"commandTimeOut = {commandTimeOut}");


            try
            {
                var storedProcedureName = "_sii_rpr_sps_getFGProdOrders";
                if (customerCode != string.Empty)
                    storedProcedureName = "_sii_rpr_sps_getNonPGProdOrders";

                log.Debug($"storedProcedureName = {storedProcedureName}");

                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@itemGroupFilter", itemGroupFilter);
                    if (customerCode != string.Empty)
                        cmd.Parameters.AddWithValue("@customerCode", customerCode);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<RollLabelData> openProdOrders = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                        new RollLabelData
                        {
                            // Set properties based on retrieved data
                            StartDate = row.Field<DateTime>("StartDate"),
                            DueDate = row.Field<DateTime>("DueDate"),
                            NoOfSlits = row.Field<int>("NoOfSlits"),
                            ItemCode = row.Field<string>("ItemCode"),
                            ItemDescription = row.Field<string>("ItemDescription"),
                            IRMS = row.Field<string>("IRMS"),
                            FactoryCode = row.Field<string>("FactoryCode"),
                            ProductionLine = row.Field<string>("ProductionLine"),
                            ProductionMachineNo = row.Field<string>("ProductionMachineNo"),
                            MaterialCode = row.Field<string>("MaterialCode"),
                            ProductName = row.Field<string>("ProductName"),
                            BatchNo = row.Field<string>("BatchNo"),
                            SAPOrderNo = row.Field<int>("SAPOrderNo"),
                            YJNOrder = row.Field<string>("YJNOrder"),
                            OrderDisplay = string.IsNullOrEmpty(row.Field<string>("YJNOrder")) ? row.Field<int>("SAPOrderNo").ToString() : $"{row.Field<int>("SAPOrderNo").ToString()} - {row.Field<string>("YJNOrder")}",
                            JumboRollNo = row.Field<int>("JumboRoll"),
                            AperatureDieNo = row.Field<string>("AperatureDieNo"),
                            SAPDocEntry = row.Field<int>("DocEntry"),
                            InputLoc = row.Field<string>("InputLoc"),
                            OutputLoc = row.Field<string>("OutputLoc"),
                            Printer = row.Field<string>("Printer"),
                            DefaultQualityStatus = row.Field<string>("DefaultQualityStatus"),
                            MinRollKgs = row.Field<decimal>("MinRollKgs"),
                            MaxRollKgs = row.Field<decimal>("MaxRollKgs"),
                            ScrapItem = row.Field<string>("ScrapItem"),
                            ScrapItemName = row.Field<string>("ScrapItemName"),
                            ScrapLine = row.Field<int>("ScrapLine"),
                            TargetRolls = row.Field<int>("TargetRolls"),
                            InvRolls = row.Field<int>("InvRolls"),
                            WidthInMM = row.Field<decimal>("WidthInMM"),
                            PONumber = row.Field<string>("PONumber")
                        }).ToList();

                    serviceOutput.ReturnValue = openProdOrders;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
                log.Error("Exception thrown in GetOpenProdOrders", ex);
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves a list of open production orders for a specific production line (old version).
        /// </summary>
        /// <param name="prodLine">The production line for which to retrieve open production orders.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the list of open production orders.</returns>
        [Obsolete]
        public static ServiceOutput GetOpenProdOrders_old(string prodLine)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getFGProdOrders_old", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@productionLine", prodLine);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<RollLabelData> openProdOrders = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                        new RollLabelData
                        {
                            // Set properties based on retrieved data
                            StartDate = row.Field<DateTime>("StartDate"),
                            DueDate = row.Field<DateTime>("DueDate"),
                            NoOfSlits = row.Field<int>("NoOfSlits"),
                            ItemCode = row.Field<string>("ItemCode"),
                            ItemDescription = row.Field<string>("ItemDescription"),
                            IRMS = row.Field<string>("IRMS"),
                            FactoryCode = row.Field<string>("FactoryCode"),
                            ProductionLine = row.Field<string>("ProductionLine"),
                            ProductionMachineNo = row.Field<string>("ProductionMachineNo"),
                            MaterialCode = row.Field<string>("MaterialCode"),
                            ProductName = row.Field<string>("ProductName"),
                            BatchNo = row.Field<string>("BatchNo"),
                            SAPOrderNo = row.Field<int>("SAPOrderNo"),
                            YJNOrder = row.Field<string>("YJNOrder"),
                            JumboRollNo = row.Field<int>("JumboRoll"),
                            AperatureDieNo = row.Field<string>("AperatureDieNo"),
                            SAPDocEntry = row.Field<int>("DocEntry"),
                            InputLoc = row.Field<string>("InputLoc"),
                            OutputLoc = row.Field<string>("OutputLoc"),
                            Printer = row.Field<string>("Printer"),
                            DefaultQualityStatus = row.Field<string>("DefaultQualityStatus"),
                            ScrapItem = row.Field<string>("ScrapItem"),
                            ScrapLine = row.Field<int>("ScrapLine")
                        }).ToList();

                    serviceOutput.ReturnValue = openProdOrders;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves bundle pack labels based on the given PMXSSCC.
        /// </summary>
        /// <param name="pmxSSCC">The PMXSSCC for which to retrieve bundle pack labels.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the retrieved bundle pack labels.</returns>
        public static ServiceOutput GetBundlePackLabel(string pmxSSCC)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var noOfCopies = Convert.ToInt32(AppUtility.GetDefaultPackCopies());
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getBundlePackLabel", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@pmxSSCC", pmxSSCC);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<PackLabel> packLabels = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                        new PackLabel
                        {
                            // Set properties based on retrieved data
                            ID = row.Field<int>("ID"),
                            PMXSSCC = row.Field<string>("PMXSSCC"),
                            ItemCode = row.Field<string>("ItemCode"),
                            ItemName = row.Field<string>("ItemName"),
                            IRMS = row.Field<string>("IRMS"),
                            YJNOrder = row.Field<string>("YJNOrder"),
                            SSCC = row.Field<string>("SSCC"),
                            SAPOrder = row.Field<int>("SAPOrder"),
                            LotNo = $"YJWR{row.Field<string>("YJNOrder")}",
                            PalletType = row.Field<string>("PalletType"),
                            Printed = row.Field<string>("Printed") == "Y" ? true : false,
                            Created = ConvertSAPDateAndTime(row["DateCreated"], row["TimeCreated"]),
                            ProductionDate = row.Field<DateTime>("ProductionDate"),
                            Qty = row.Field<decimal>("Qty"),
                            MaxRollsPerPack = row.Field<int>("MaxRollsPerPack"),
                            Copies = noOfCopies,
                            Employee = ""
                        }).ToList();

                    serviceOutput.ReturnValue = packLabels;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves pack labels based on the specified criteria.
        /// </summary>
        /// <param name="isReprint">A flag indicating whether the pack labels are for reprinting.</param>
        /// <param name="order">The order number to filter the pack labels (optional).</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the retrieved pack labels.</returns>
        public static ServiceOutput GetPackLabels(bool isReprint, string order = null)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var noOfCopies = Convert.ToInt32(AppUtility.GetDefaultPackCopies());
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getPackLabelsNonPG", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@rePrint", isReprint ? 1 : 0);
                    cmd.Parameters.AddWithValue("@order", order);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<PackLabel> packLabels = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                        new PackLabel
                        {
                            // Set properties based on retrieved data
                            ID = row.Field<int>("ID"),
                            PMXSSCC = row.Field<string>("PMXSSCC"),
                            ItemCode = row.Field<string>("ItemCode"),
                            ItemName = row.Field<string>("ItemName"),
                            IRMS = row.Field<string>("IRMS"),
                            YJNOrder = row.Field<string>("YJNOrder"),
                            SSCC = row.Field<string>("SSCC"),
                            SAPOrder = row.Field<int>("SAPOrder"),
                            LotNo = $"YJWR{row.Field<string>("YJNOrder")}",
                            PalletType = "NONE",
                            Printed = row.Field<string>("Printed") == "Y" ? true : false,
                            Created = ConvertSAPDateAndTime(row["DateCreated"], row["TimeCreated"]),
                            ProductionDate = row.Field<DateTime>("ProductionDate"),
                            Qty = row.Field<decimal>("Qty"),
                            MaxRollsPerPack = row.Field<int>("MaxRollsPerPack"),
                            Copies = noOfCopies,
                            Employee = "",
                            TotalWeight = row.IsNull("TotalWeight") ? 0 : row.Field<decimal>("TotalWeight"),
                            NumRolls = row.Field<int>("NumRolls"),
                            PONumber = row.Field<string>("PONumber")
                        }).ToList();

                    serviceOutput.ReturnValue = packLabels;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves the rolls associated with a pack label.
        /// </summary>
        /// <param name="packLabelId">The ID of the pack label.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the retrieved rolls.</returns>
        public static ServiceOutput GetPackLabelRolls(int packLabelId)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getPackLabelRollsNonPG", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@packLabelId", packLabelId);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<Roll> rolls = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                        new Roll
                        {
                            // Set properties based on retrieved data
                            RollNo = row.Field<string>("RollNo"),
                            ItemCode = row.Field<string>("ItemCode"),
                            ItemName = row.Field<string>("ItemName"),
                            IRMS = row.Field<string>("IRMS"),
                            SSCC = row.Field<string>("SSCC"),
                            YJNOrder = row.Field<string>("YJNOrderNo"),
                            Quantity = row.Field<decimal>("Quantity"),
                            //NetKg = row.Field<decimal>("Kgs"),
                            JumboRoll = string.IsNullOrEmpty(row.Field<string>("RollNo")) ? "" : row.Field<string>("RollNo").Substring(8, 2),
                            TareKg = row.Field<decimal>("TareKg"),
                            UOM = row.Field<string>("UOM")
                        }).ToList();

                    serviceOutput.ReturnValue = rolls;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves the tub pallet labels.
        /// </summary>
        /// <param name="isReprint">Specifies whether to retrieve reprint labels.</param>
        /// <param name="order">The order to filter the labels (optional).</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the retrieved tub pallet labels.</returns>
        public static ServiceOutput GetTubPalletLabels(bool isReprint, string order = null)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var noOfCopies = Convert.ToInt32(AppUtility.GetDefaultPackCopies());
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getTubPalletLabels", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@rePrint", isReprint ? 1 : 0);
                    cmd.Parameters.AddWithValue("@order", order);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<TubPalletLabel> tubPalletLabels = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                        new TubPalletLabel
                        {
                            // Set properties based on retrieved data
                            ID = row.Field<int>("ID"),
                            PMXSSCC = row.Field<string>("PMXSSCC"),
                            ItemCode = row.Field<string>("ItemCode"),
                            ItemName = row.Field<string>("ItemName"),
                            IRMS = row.Field<string>("IRMS"),
                            YJNOrder = row.Field<string>("YJNOrder"),
                            SSCC = row.Field<string>("SSCC"),
                            SAPOrder = row.Field<int>("SAPOrder"),
                            LotNo = $"YJWR{row.Field<string>("YJNOrder")}",
                            PalletType = "NONE",
                            Printed = row.Field<string>("Printed") == "Y" ? true : false,
                            Created = ConvertSAPDateAndTime(row["DateCreated"], row["TimeCreated"]),
                            ProductionDate = row.Field<DateTime>("ProductionDate"),
                            Qty = row.Field<decimal>("Qty"),
                            MaxCasesPerPack = row.Field<int>("MaxCasesPerPack"),
                            Copies = noOfCopies,
                            Employee = ""
                        }).ToList();

                    serviceOutput.ReturnValue = tubPalletLabels;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves the tub pallet label cases.
        /// </summary>
        /// <param name="packLabelId">The ID of the pack label.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the retrieved cases.</returns>
        public static ServiceOutput GetTubPalletLabelCases(int packLabelId)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getTubPalletLabelCases", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@packLabelId", packLabelId);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<Case> cases = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                       new Case
                       {
                           CaseNo = row.Field<string>("CaseNo"),
                           ItemCode = row.Field<string>("ItemCode"),
                           ItemName = row.Field<string>("ItemName"),
                           IRMS = row.Field<string>("IRMS"),
                           SSCC = row.Field<string>("SSCC"),
                           YJNOrder = row.Field<string>("YJNOrderNo"),
                           Units = row.Field<decimal>("Units")
                       }).ToList();

                    serviceOutput.ReturnValue = cases;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves the rolls for a production order.
        /// </summary>
        /// <param name="yjnOrderNo">The YJN order number.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the retrieved rolls.</returns>
        public static ServiceOutput GetRollsForOrder(string yjnOrderNo, string customerCode = "")
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getRollsForNonPGProdOrder", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@yjnOrderNo", yjnOrderNo);
                    cmd.Parameters.AddWithValue("@customerCode", customerCode);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<Roll> rolls = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                       new Roll
                       {
                           RollNo = row.Field<string>("RollNo"),
                           ItemCode = row.Field<string>("ItemCode"),
                           ItemName = row.Field<string>("ItemName"),
                           IRMS = row.Field<string>("IRMS"),
                           SSCC = row.Field<string>("SSCC"),
                           PG_SSCC = row.Field<string>("PG_SSCC"),
                           LUID = row.Field<int>("LUID"),
                           YJNOrder = row.Field<string>("YJNOrderNo"),
                           //Kgs = row.Field<decimal>("Kgs"),
                           Quantity = row.Table.Columns.Contains("Quantity") ? row.Field<decimal>("Quantity") : 0m,
                           SquareMeters = row.Table.Columns.Contains("SquareMeters") ? row.Field<decimal>("SquareMeters") : 0m,
                           JumboRoll = row.Field<string>("RollNo").Substring(8, 2),
                           StorLocCode = row.Field<string>("StorLocCode"),
                           QualityStatus = row.Field<string>("QualityStatus"),
                           UOM = row.Field<string>("UOM"),
                           PONumber = row.Field<string>("PONumber")
                       })
                       .GroupBy(r => r.RollNo)  // Group by RollNo to remove duplicates
                       .Select(g => g.First())   // Take the first occurrence of each roll
                       .ToList();

                    serviceOutput.ReturnValue = rolls;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }

        /// <summary>
        /// Checks the availability of a roll in the system.
        /// </summary>
        /// <param name="rollNo">The roll number to check.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the count of rolls and the success flag.</returns>
        public static ServiceOutput CheckRoll(string rollNo)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_rollCheck", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;

                    cmd.Parameters.AddWithValue("@rollNo", rollNo);

                    var parm = new SqlParameter("@countRolls", SqlDbType.Int);
                    parm.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parm);

                    cmd.ExecuteNonQuery();

                    serviceOutput.ReturnValue = (int)cmd.Parameters["@countRolls"].Value;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves the input material details for a production line and packing material location.
        /// </summary>
        /// <param name="prodLine">The production line code.</param>
        /// <param name="packingMtlLoc">The packing material location code.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the inventory details and the success flag.</returns>
        public static ServiceOutput GetProdLineInputMaterial(string prodLine, string packingMtlLoc)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getLineInputLocMaterial", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;

                    cmd.Parameters.AddWithValue("@prodLine", prodLine);
                    cmd.Parameters.AddWithValue("@packingMtlLoc", packingMtlLoc);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);

                    IList<InventoryDetail> invDetails = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                    new InventoryDetail
                    {
                        ItemCode = row.Field<string>("ItemCode"),
                        ItemName = row.Field<string>("ItemName"),
                        Warehouse = row.Field<string>("Warehouse"),
                        StorageLocation = row.Field<string>("StorLocCode"),
                        QualityStatus = row.Field<string>("QualityStatus"),
                        Batch = row.Field<string>("BatchNumber1"),
                        Lot = row.Field<string>("LotNumber"),
                        UOM = row.Field<string>("InvntryUom"),
                        Quantity = Convert.ToDouble(row.Field<decimal>("Quantity")),
                        LUID = row.Field<int>("LUID"),
                        SSCC = row.Field<string>("SSCC"),
                        InDate = row.Field<DateTime?>("InDate"),
                        BatchControlled = row.Field<int>("BatchControlled") == 1 ? true : false,
                        PackagingMtl = row.Field<int>("PackagingMtl") == 1 ? true : false
                    }).ToList();

                    serviceOutput.ReturnValue = invDetails;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves the issue material details for a production order and planned quantity.
        /// </summary>
        /// <param name="prodOrder">The production order number.</param>
        /// <param name="plannedQty">The planned quantity.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the inventory issue details and the success flag.</returns>
        public static ServiceOutput GetProdOrderIssueMaterial(int prodOrder, decimal plannedQty)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getOrderIssueLines", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;

                    cmd.Parameters.AddWithValue("@prodOrder", prodOrder);
                    cmd.Parameters.AddWithValue("@plannedQty", plannedQty);

                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);

                    IList<InventoryIssueDetail> invDetails = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                    new InventoryIssueDetail
                    {
                        ItemCode = row.Field<string>("ItemCode"),
                        ItemName = row.Field<string>("ItemName"),
                        Warehouse = row.Field<string>("wareHouse"),
                        UOM = row.Field<string>("InvntryUom"),
                        PlannedIssueQty = Convert.ToDouble(row.Field<decimal>("PlannedQty")),
                        BatchControlled = row.Field<int>("BatchControlled") == 1 ? true : false,
                        PackagingMtl = row.Field<int>("PackagingMtl") == 1 ? true : false,
                        BaseEntry = row.Field<int>("DocEntry"),
                        BaseLine = row.Field<int>("LineNum")
                    }).ToList();

                    serviceOutput.ReturnValue = invDetails;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }


        /// <summary>
        /// Retrieves the scrap reasons from the database.
        /// </summary>
        /// <param name="itemGroupFilter">Optional filter for item group.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the list of scrap reasons and the success flag.</returns>
        public static ServiceOutput GetScrapReasons(string itemGroupFilter = "")
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getScrapReasons", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(itemGroupFilter))
                    {
                        cmd.Parameters.AddWithValue("@itemGroupFilter", itemGroupFilter);
                    }

                    cmd.CommandTimeout = commandTimeOut;
                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);

                    List<string> scrapReasons = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                        row.Field<string>("Code")
                    ).ToList();

                    serviceOutput.ReturnValue = scrapReasons;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }


        /// <summary>
        /// Retrieves the production lines from the database.
        /// </summary>
        /// <param name="includeMixed">Flag indicating whether to include mixed lines.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the list of production lines and the success flag.</returns>
        public static ServiceOutput GetProdLines(bool includeMixed)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getProdLines", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@includeMixLines", includeMixed ? "Y" : "N");
                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);

                    List<ProductionLine> prodLines = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                        new ProductionLine
                        {
                            Code = row.Field<string>("Code"),
                            LineNo = row.Field<string>("MachineNo"),
                            InputLocationCode = row.Field<string>("InputLocationCode"),
                            OutputLocationCode = row.Field<string>("OutputLocationCode"),
                            Printer = row.Field<string>("Printer")
                        }
                    ).ToList();

                    serviceOutput.ReturnValue = prodLines;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }

            return serviceOutput;
        }

        /// <summary>
        /// Retrieves the last production run for a given order number.
        /// </summary>
        /// <param name="orderNo">The order number.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the last production run value and the success flag.</returns>
        public static ServiceOutput GetLastProductionRun(int orderNo)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getLastProductionRun", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@docNum", orderNo);
                    var parm = new SqlParameter("@lastProductionRun", SqlDbType.Int);
                    parm.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parm);
                    cmd.ExecuteNonQuery();

                    serviceOutput.ReturnValue = (int)cmd.Parameters["@lastProductionRun"].Value;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }
            return serviceOutput;
        }

        /// <summary>
        /// Creates a new SSCC (Serial Shipping Container Code).
        /// </summary>
        /// <returns>A <see cref="ServiceOutput"/> object containing the generated SSCC and its associated LUID (Internal Key) if successful.</returns>
        public static ServiceOutput CreateSSCC()
        {
            ILog log = LogManager.GetLogger(typeof(AppData));
            log.Debug("Entered CreateSSCC");

            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetPMXConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            int luid;
            string sscc;

            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("PMX_SP_CreateSSCC", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@supplierPalletNumber", DBNull.Value);
                    cmd.Parameters.AddWithValue("@userSign", 1);
                    var parm = new SqlParameter("@luid", SqlDbType.Int);
                    parm.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parm);
                    cmd.ExecuteNonQuery();
                    luid = (int)cmd.Parameters["@luid"].Value;
                    log.DebugFormat("luid = {0}", luid);
                }

                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand($"SELECT SSCC FROM PMX_LUID WHERE InternalKey = {luid}", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = commandTimeOut;
                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    sscc = serviceOutput.ResultSet.Tables[0].Rows[0]["SSCC"].ToString();
                    log.DebugFormat("sscc = {0}", sscc);
                }

                serviceOutput.SuccessFlag = true;
                serviceOutput.ReturnValue = new KeyValuePair<int, string>(luid, sscc);
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
                log.Error(ex.Message, ex);
            }

            return serviceOutput;
        }


        /// <summary>
        /// Adds a sub LUID (Internal Key) to a master LUID in the system.
        /// </summary>
        /// <param name="LUID">The sub LUID to be added.</param>
        /// <param name="masterLUID">The master LUID to which the sub LUID will be added.</param>
        /// <param name="batchNumber">The batch number associated with the sub LUID.</param>
        /// <param name="yanjanOrderID">The Yanjan Order ID associated with the sub LUID.</param>
        /// <param name="itemCode">The item code associated with the sub LUID.</param>
        /// <returns>A <see cref="ServiceOutput"/> object indicating the success of the operation.</returns>
        public static ServiceOutput AddSubLUIDToMaster(int LUID, int masterLUID, string batchNumber, string yjnOrderID, string itemCode)
        {
            var serviceOutput = new ServiceOutput();
            ILog log = LogManager.GetLogger(typeof(AppData));
            log.Debug("Entered AddSubLUIDToMaster");
            var databaseConnection = AppUtility.GetSAPConnectionString();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_spi_addSubLUIDToMaster", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LUID", LUID);
                    cmd.Parameters.AddWithValue("@masterLUID", masterLUID);
                    cmd.Parameters.AddWithValue("@yjnOrderID", yjnOrderID);
                    cmd.Parameters.AddWithValue("@itemCode", itemCode);
                    cmd.Parameters.AddWithValue("@batchNumber", batchNumber);
                    cmd.ExecuteNonQuery();
                }
                serviceOutput.SuccessFlag = true;
                serviceOutput.ReturnValue = null;
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method: {serviceOutput.MethodName}. Error: {ex.Message}";
                log.Error(ex.Message, ex);
            }

            return serviceOutput;
        }

        /// <summary>
        /// Updates the last batch information for a production machine in the system.
        /// </summary>
        /// <param name="prodMachine">The production machine code.</param>
        /// <param name="prodStartDate">The start date of the production.</param>
        /// <param name="matlCode">The material code of the batch.</param>
        /// <param name="prodName">The name of the production.</param>
        /// <returns>The Yanjan Order ID associated with the updated last batch.</returns>
        /// <remarks>
        /// This method updates the last batch information for a production machine, based on the provided parameters.
        /// The Yanjan Order ID associated with the updated last batch is returned.
        /// </remarks>
        public static ServiceOutput UpdateLastBatch(string prodMachine,
            DateTime prodStartDate, string matlCode, string prodName)
        {
            var serviceOutput = new ServiceOutput();
            ILog log = LogManager.GetLogger(typeof(AppData));
            log.Debug("Entered UpdateLastBatch");
            var databaseConnection = AppUtility.GetSAPConnectionString();
            try
            {
                string yjnOrder = string.Empty;
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_spu_updateLastBatch", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@prodMachine", prodMachine);
                    cmd.Parameters.AddWithValue("@prodStartDate", prodStartDate);
                    cmd.Parameters.AddWithValue("@matlCode", matlCode);
                    cmd.Parameters.AddWithValue("@prodName", prodName);
                    yjnOrder = cmd.ExecuteScalar().ToString();
                }
                serviceOutput.SuccessFlag = true;
                serviceOutput.ReturnValue = yjnOrder;
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method: {serviceOutput.MethodName}. Error: {ex.Message}";
                log.Error(ex.Message, ex);
            }

            return serviceOutput;

        }

        /// <summary>
        /// Ensures that a Yanjan Order Number (YJN Order No) is associated with the provided SAP Order Number (SAPOrderNo).
        /// If the YJN Order No is not already associated with the SAP Order, it is updated in the system.
        /// </summary>
        /// <param name="SAPOrderNo">The SAP Order Number.</param>
        /// <param name="prodMachine">The production machine code.</param>
        /// <param name="prodStartDate">The start date of the production.</param>
        /// <param name="matlCode">The material code of the batch.</param>
        /// <param name="prodName">The name of the production.</param>
        /// <returns>The Yanjan Order ID associated with the SAP Order.</returns>
        /// <remarks>
        /// This method ensures that a Yanjan Order No is associated with the provided SAP Order No.
        /// If the YJN Order No is already associated with the SAP Order, it is returned.
        /// If not, the method updates the last batch information for the specified production machine
        /// and associates the generated Yanjan Order ID with the SAP Order in the system.
        /// The Yanjan Order ID associated with the SAP Order is returned.
        /// </remarks>

        public static ServiceOutput EnsureYJNOrderNo(int SAPOrderNo, string prodMachine, DateTime prodStartDate,
            string matlCode, string prodName)
        {
            var serviceOutput = new ServiceOutput();
            ILog log = LogManager.GetLogger(typeof(AppData));
            log.Debug("Entered EnsureYJNOrderNo");
            var databaseConnection = AppUtility.GetSAPConnectionString();
            try
            {
                string yjnOrder = string.Empty;
                ServiceOutput soUpdateBatch = new ServiceOutput();
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("SELECT U_SII_YanJanOrderId FROM OWOR WHERE DocNum = @docNum", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@docNum", SAPOrderNo);
                    var orderIdResults = cmd.ExecuteScalar();
                    if (orderIdResults == DBNull.Value)
                    {
                        soUpdateBatch = UpdateLastBatch(prodMachine, prodStartDate, matlCode, prodName);
                        if (soUpdateBatch.SuccessFlag)
                        {
                            yjnOrder = soUpdateBatch.ReturnValue.ToString();
                        }
                        else
                        {
                            yjnOrder = SAPOrderNo.ToString();
                        }
                        using (SqlCommand updateCmd = new SqlCommand("UPDATE OWOR SET U_SII_YanJanOrderId = @yjnOrder WHERE DocNum = @docNum", cnx))
                        {
                            updateCmd.CommandType = CommandType.Text;
                            updateCmd.Parameters.AddWithValue("@yjnOrder", yjnOrder);
                            updateCmd.Parameters.AddWithValue("@docNum", SAPOrderNo);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        yjnOrder = orderIdResults.ToString();
                    }
                }
                serviceOutput.SuccessFlag = true;
                serviceOutput.ReturnValue = yjnOrder;
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method: {serviceOutput.MethodName}. Error: {ex.Message}";
                log.Error(ex.Message, ex);
            }

            return serviceOutput;

        }

        /// <summary>
        /// Adds a new bundle for the specified LUID (Logical Unit Identifier).
        /// </summary>
        /// <param name="LUID">The Logical Unit Identifier of the bundle.</param>
        /// <returns>A service output indicating the success of the operation.</returns>
        /// <remarks>
        /// This method adds a new bundle for the specified LUID.
        /// The LUID is a unique identifier that represents a logical unit within the system.
        /// The method executes a stored procedure to add the new bundle to the system.
        /// If the operation is successful, the SuccessFlag property of the service output is set to true.
        /// Otherwise, an exception is caught and logged, and the SuccessFlag property is set to false.
        /// </remarks>
        public static ServiceOutput AddNewBundle(int LUID)
        {
            var serviceOutput = new ServiceOutput();
            ILog log = LogManager.GetLogger(typeof(AppData));
            log.Debug("Entered AddNewBundle");
            var databaseConnection = AppUtility.GetSAPConnectionString();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_spi_addNewBundle", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@luid", LUID);
                    cmd.ExecuteNonQuery();
                }
                serviceOutput.SuccessFlag = true;
                serviceOutput.ReturnValue = null;
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method: {serviceOutput.MethodName}. Error: {ex.Message}";
                log.Error(ex.Message, ex);
            }

            return serviceOutput;
        }

        /// <summary>
        /// Generates a new Procter and Gamble (P&G) pallet number.
        /// </summary>
        /// <returns>A service output containing the generated pallet number.</returns>
        /// <remarks>
        /// This method generates a new P&G pallet number by incrementing the current value.
        /// The generated pallet number is obtained by executing a stored procedure.
        /// If the operation is successful, the SuccessFlag property of the service output is set to true
        /// and the generated pallet number is returned in the ReturnValue property.
        /// Otherwise, an exception is caught and logged, the SuccessFlag is set to false,
        /// and the ServiceException property of the service output is populated with the error details.
        /// </remarks>
        public static ServiceOutput NewPGPalletNo()
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_spi_incrementPGPalletNo", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    var parm = new SqlParameter("RETURN_VALUE", SqlDbType.Int);
                    parm.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(parm);
                    cmd.ExecuteNonQuery();

                    serviceOutput.ReturnValue = (int)cmd.Parameters["RETURN_VALUE"].Value;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }
            return serviceOutput;
        }

        /// <summary>
        /// Generates a new Procter and Gamble (P&G) pallet number (legacy version).
        /// </summary>
        /// <returns>A service output containing the generated pallet number.</returns>
        /// <remarks>
        /// This method generates a new P&G pallet number by incrementing the current value.
        /// The generated pallet number is obtained by executing a stored procedure (legacy version).
        /// If the operation is successful, the SuccessFlag property of the service output is set to true
        /// and the generated pallet number is returned in the ReturnValue property.
        /// Otherwise, an exception is caught and logged, the SuccessFlag is set to false,
        /// and the ServiceException property of the service output is populated with the error details.
        /// </remarks>
        [Obsolete]
        public static ServiceOutput NewPGPalletNo_old()
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_spi_incrementPGPalletNo_old", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    var parm = new SqlParameter("RETURN_VALUE", SqlDbType.Int);
                    parm.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(parm);
                    cmd.ExecuteNonQuery();

                    serviceOutput.ReturnValue = (int)cmd.Parameters["RETURN_VALUE"].Value;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }
            return serviceOutput;
        }

        /// <summary>
        /// Adds a new roll to the system.
        /// </summary>
        /// <param name="rollNo">The roll number of the new roll.</param>
        /// <param name="prodOrder">The production order associated with the roll.</param>
        /// <param name="lot">The lot number of the roll.</param>
        /// <param name="sscc">The SSCC (Serial Shipping Container Code) of the roll.</param>
        /// <param name="qty">The quantity of the roll.</param>
        /// <param name="emp">The employee associated with the roll.</param>
        /// <returns>A service output indicating the success of the operation.</returns>
        /// <remarks>
        /// This method adds a new roll to the system by executing a stored procedure.
        /// The necessary information such as the roll number, production order, lot number,
        /// SSCC, quantity, and employee are provided as parameters.
        /// If the operation is successful, the SuccessFlag property of the service output is set to true.
        /// Otherwise, an exception is caught and logged, the SuccessFlag is set to false,
        /// and the ServiceException property of the service output is populated with the error details.
        /// </remarks>
        public static ServiceOutput AddRoll(string rollNo, int prodOrder, string lot, string sscc, decimal qty, string emp)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_spi_addRoll", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@rollNo", rollNo);
                    cmd.Parameters.AddWithValue("@prodOrder", prodOrder);
                    cmd.Parameters.AddWithValue("@lot", lot);
                    cmd.Parameters.AddWithValue("@sscc", sscc);
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@emp", emp);
                    cmd.ExecuteNonQuery();
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }
            return serviceOutput;
        }

        /// <summary>
        /// Adds an issue shortage for a production order.
        /// </summary>
        /// <param name="prodOrder">The production order associated with the issue shortage.</param>
        /// <param name="itemCode">The item code of the shortage.</param>
        /// <param name="shortQty">The quantity of the shortage.</param>
        /// <returns>A service output indicating the success of the operation.</returns>
        /// <remarks>
        /// This method adds an issue shortage for a production order by executing a stored procedure.
        /// The production order, item code, and shortage quantity are provided as parameters.
        /// If the operation is successful, the SuccessFlag property of the service output is set to true.
        /// Otherwise, an exception is caught and logged, the SuccessFlag is set to false,
        /// and the ServiceException property of the service output is populated with the error details.
        /// </remarks>
        public static ServiceOutput AddIssueShortage(int prodOrder, string itemCode, decimal shortQty)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_spi_addIssueShortage", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@prodOrder", prodOrder);
                    cmd.Parameters.AddWithValue("@itemCode", itemCode);
                    cmd.Parameters.AddWithValue("@shortQty", shortQty);
                    cmd.ExecuteNonQuery();
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }
            return serviceOutput;
        }

        /// <summary>
        /// Updates the jumbo roll for a SAP order.
        /// </summary>
        /// <param name="sapOrderNo">The SAP order number associated with the jumbo roll.</param>
        /// <returns>A service output indicating the success of the operation and the next jumbo roll number.</returns>
        /// <remarks>
        /// This method updates the jumbo roll for a SAP order by executing a stored procedure.
        /// The SAP order number is provided as a parameter.
        /// If the operation is successful, the SuccessFlag property of the service output is set to true,
        /// and the ReturnValue property of the service output contains the next jumbo roll number.
        /// Otherwise, an exception is caught and logged, the SuccessFlag is set to false,
        /// and the ServiceException property of the service output is populated with the error details.
        /// </remarks>
        public static ServiceOutput UpdateJumboRoll(int sapOrderNo)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_spu_incrementJumboRoll", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@docNum", sapOrderNo);
                    var parm = new SqlParameter("@nextJumboRoll", SqlDbType.Int);
                    parm.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parm);
                    cmd.ExecuteNonQuery();

                    serviceOutput.ReturnValue = (int)cmd.Parameters["@nextJumboRoll"].Value;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }
            return serviceOutput;
        }

        /// <summary>
        /// Increments the production run for a specific order.
        /// </summary>
        /// <param name="orderNo">The order number for which the production run needs to be incremented.</param>
        /// <returns>A service output indicating the success of the operation and the next production run number.</returns>
        /// <remarks>
        /// This method increments the production run for a specific order by executing a stored procedure.
        /// The order number is provided as a parameter.
        /// If the operation is successful, the SuccessFlag property of the service output is set to true,
        /// and the ReturnValue property of the service output contains the next production run number.
        /// Otherwise, an exception is caught and logged, the SuccessFlag is set to false,
        /// and the ServiceException property of the service output is populated with the error details.
        /// </remarks>
        public static ServiceOutput IncrementProductionRun(int orderNo)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_spu_incrementProductionRun", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@docNum", orderNo);
                    var parm = new SqlParameter("@nextProductionRun", SqlDbType.Int);
                    parm.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parm);
                    cmd.ExecuteNonQuery();

                    serviceOutput.ReturnValue = (int)cmd.Parameters["@nextProductionRun"].Value;
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }
            return serviceOutput;
        }

        /// <summary>
        /// Updates the pack label with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the pack label to update.</param>
        /// <param name="qty">The quantity to update on the pack label.</param>
        /// <param name="printed">The status of the pack label (printed or not printed).</param>
        /// <returns>A service output indicating the success of the operation.</returns>
        /// <remarks>
        /// This method updates a pack label with the specified ID by executing a stored procedure.
        /// The ID, quantity, and printed status are provided as parameters.
        /// If the operation is successful, the SuccessFlag property of the service output is set to true.
        /// Otherwise, an exception is caught and logged, the SuccessFlag is set to false,
        /// and the ServiceException property of the service output is populated with the error details.
        /// </remarks>
        public static ServiceOutput UpdatePackLabel(int id, decimal qty, string printed)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_spu_updatePackLabel", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@printed", printed);
                    cmd.ExecuteNonQuery();
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }
            return serviceOutput;
        }

        /// <summary>
        /// Update the contents of the @SII_PG_BUNDLE table corresponding to the PackLabel object
        /// </summary>
        /// <param name="packLabel">The PackLabel object for which the @SII_PG_BUNDLE data need to be updated</param>
        /// <param name="printed">"Y" or "N" to indicate whether the label has been printed</param>
        /// <returns></returns>
        public static ServiceOutput UpdatePackLabel(PackLabel packLabel, string printed)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_spu_updatePackLabelNonPG", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@id", packLabel.ID);
                    cmd.Parameters.AddWithValue("@qty", packLabel.Qty);
                    cmd.Parameters.AddWithValue("@weight", packLabel.TotalWeight);
                    cmd.Parameters.AddWithValue("@printed", printed);
                    cmd.ExecuteNonQuery();
                    serviceOutput.SuccessFlag = true;
                }
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
            }
            return serviceOutput;

        }

        /// <summary>
        /// Converts a date and time value from the SAP database format to a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="date">The date value from the SAP database.</param>
        /// <param name="hrsmins">The time value from the SAP database in the format HHMM.</param>
        /// <returns>A <see cref="DateTime"/> object representing the converted date and time.</returns>
        /// <remarks>
        /// This method takes a date value and a time value from the SAP database and converts them into a <see cref="DateTime"/> object.
        /// If the date value is null or DBNull.Value, the method returns DateTime.MinValue.
        /// Otherwise, it extracts the hours and minutes from the time value (HHMM format), converts them into integers,
        /// and adds them to the date value to obtain the final converted date and time.
        /// The converted <see cref="DateTime"/> object is then returned.
        /// </remarks>
        private static DateTime ConvertSAPDateAndTime(object date, object hrsmins)
        {
            if (date == System.DBNull.Value)
            {
                return DateTime.MinValue;
            }
            else
            {
                var hrs = Convert.ToInt32(Convert.ToInt32(hrsmins) / 100);
                var mins = Convert.ToInt32(hrsmins) % 100;
                return Convert.ToDateTime(date).AddHours(hrs).AddMinutes(mins);
            }
        }

        /// <summary>
        /// Retrieves the Logical Unit Identifier (LUID) for a given Serial Shipping Container Code (SSCC) from the PMX_INVENTORY_REPORT_DETAIL view in the SAP database.
        /// </summary>
        /// <param name="sscc">The Serial Shipping Container Code (SSCC) to retrieve the LUID for.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing the LUID value if successful, or an error message if an exception occurs.</returns>
        /// <remarks>
        /// This method queries the PMX_INVENTORY_REPORT_DETAIL view in the SAP database to retrieve the Location Identifier (LUID) for a given Serial Shipping Container Code (SSCC).
        /// It uses the provided SSCC parameter in the SQL query to filter the results and retrieves the TOP 1 LUID value.
        /// If the query is successful and a matching LUID is found, the LUID value is returned in the <see cref="ServiceOutput.ReturnValue"/> property of the <see cref="ServiceOutput"/> object with <see cref="ServiceOutput.SuccessFlag"/> set to true.
        /// If an exception occurs during the database operation, the method sets <see cref="ServiceOutput.SuccessFlag"/> to false and sets the <see cref="ServiceOutput.ServiceException"/> property to the error message.
        /// </remarks>
        public static ServiceOutput GetLUIDForSSCC(string sscc)
        {
            ServiceOutput so = new ServiceOutput();
            try
            {
                var databaseConnection = AppUtility.GetSAPConnectionString();
                var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                {
                    cnx.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 LUID FROM PMX_INVENTORY_REPORT_DETAIL WHERE SSCC = @sscc", cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@sscc", sscc);
                        so.ReturnValue = cmd.ExecuteScalar();
                        so.SuccessFlag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                so.SuccessFlag = false;
                so.ServiceException = ex.Message;
            }

            return so;
        }

        /// <summary>
        /// Retrieves the minimum and maximum roll weights in kilograms (kgs) for a given item code from the OITM table in the SAP database.
        /// </summary>
        /// <param name="itemCode">The item code to retrieve the roll weight information for.</param>
        /// <returns>A <see cref="ServiceOutput"/> object containing a <see cref="RollMinMaxKg"/> object with the minimum and maximum roll weights if successful, or an error message if an exception occurs.</returns>
        /// <remarks>
        /// This method queries the OITM table in the SAP database to retrieve the minimum and maximum roll weights in kilograms (kgs) for a given item code.
        /// It uses the provided itemCode parameter in the SQL query to filter the results and retrieves the U_SII_MinRollKgs and U_SII_MaxRollKgs values.
        /// If the query is successful and a matching item is found, the minimum and maximum roll weights are returned as a <see cref="RollMinMaxKg"/> object in the <see cref="ServiceOutput.ReturnValue"/> property of the <see cref="ServiceOutput"/> object with <see cref="ServiceOutput.SuccessFlag"/> set to true.
        /// If no matching item is found in the database, an <see cref="ApplicationException"/> is thrown with an error message.
        /// If an exception occurs during the database operation, the method sets <see cref="ServiceOutput.SuccessFlag"/> to false and sets the <see cref="ServiceOutput.ServiceException"/> property to the error message.
        /// </remarks>
        public static ServiceOutput GetRollMinMaxKgForItem(string itemCode)
        {
            ServiceOutput so = new ServiceOutput();
            try
            {
                var databaseConnection = AppUtility.GetSAPConnectionString();
                var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                {
                    cnx.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT U_SII_MinRollKgs, U_SII_MaxRollKgs FROM OITM WHERE ItemCode = @itemCode", cnx))
                    {
                        cmd.CommandTimeout = commandTimeOut;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@itemCode", itemCode);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                var minRollKg = rdr.GetDecimal(0);
                                var maxRollKg = rdr.GetDecimal(1);
                                so.ReturnValue = new RollMinMaxKg { MinRollKg = minRollKg, MaxRollKg = maxRollKg };
                                so.SuccessFlag = true;
                            }
                            else
                            {
                                throw new ApplicationException($"Item {itemCode} not found in database");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                so.SuccessFlag = false;
                so.ServiceException = ex.Message;
            }

            return so;
        }

        public static ServiceOutput GetItem(string itemCode)
        {
            ServiceOutput so = new ServiceOutput();
            try
            {
                var databaseConnection = AppUtility.GetSAPConnectionString();
                var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                {
                    cnx.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT ItemCode, U_PMX_HBN2 FROM OITM WHERE ItemCode = @itemCode", cnx))
                    {
                        cmd.CommandTimeout = commandTimeOut;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@itemCode", itemCode);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                var itemCodeStr = rdr.GetString(0);
                                var hasBatchNumberStr = rdr.GetString(1);
                                so.ReturnValue = new Item(itemCodeStr, hasBatchNumberStr == "Y");
                                so.SuccessFlag = true;
                            }
                            else
                            {
                                throw new ApplicationException($"Item {itemCode} not found in database");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                so.SuccessFlag = false;
                so.ServiceException = ex.Message;
            }

            return so;

        }
    }
}
