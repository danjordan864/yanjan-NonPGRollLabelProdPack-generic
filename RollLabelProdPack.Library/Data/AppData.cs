using log4net;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.Library.Data
{
    public class AppData
    {
        //_sii_rpr_sps_getProdOrder
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
                    RollLabelData prodOrder = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                   new RollLabelData
                   {
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
            }
            return serviceOutput;
        }
        public static ServiceOutput GetOpenProdOrders(string itemGroupFilter)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection =AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getFGProdOrders", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@itemGroupFilter", itemGroupFilter);
                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<RollLabelData> openProdOrders = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                   new RollLabelData
                   {
                       StartDate = row.Field<DateTime>("StartDate"),
                       DueDate = row.Field<DateTime>("DueDate"),
                       NoOfSlits = row.Field<int>("NoOfSlits"),
                       ItemCode = row.Field<string>("ItemCode"),
                       ItemDescription = row.Field<string>("ItemDescription"),
                       IRMS = row.Field<string>("IRMS"),
                       FactoryCode = row.Field<string>("FactoryCode"),
                       ProductionLine= row.Field<string>("ProductionLine"),
                       ProductionMachineNo = row.Field<string>("ProductionMachineNo"),
                       MaterialCode= row.Field<string>("MaterialCode"),
                       ProductName = row.Field<string>("ProductName"),
                       BatchNo = row.Field<string>("BatchNo"),
                       SAPOrderNo = row.Field<int>("SAPOrderNo"),
                       YJNOrder = row.Field<string>("YJNOrder"),
                       OrderDisplay = string.IsNullOrEmpty(row.Field<string>("YJNOrder"))? row.Field<int>("SAPOrderNo").ToString():$"{row.Field<int>("SAPOrderNo").ToString()} - {row.Field<string>("YJNOrder")}",
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
        public static ServiceOutput GetPackLabels(bool isReprint, string order = null)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var noOfCopies = Convert.ToInt32(AppUtility.GetDefaultPackCopies());
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getPackLabels", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@rePrint", isReprint?1:0);
                    cmd.Parameters.AddWithValue("@order", order);
                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<PackLabel> packLabels = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                   new PackLabel
                   {
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
                       Printed = row.Field<string>("Printed") == "Y"?true:false,
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

        public static ServiceOutput GetPackLabelRolls(int packLabelId)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getPackLabelRolls", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@packLabelId", packLabelId);
                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    IList<Roll> rolls = serviceOutput.ResultSet.Tables[0].AsEnumerable().Select(row =>
                   new Roll
                   {
                       RollNo = row.Field<string>("RollNo"),
                       ItemCode = row.Field<string>("ItemCode"),
                       ItemName = row.Field<string>("ItemName"),
                       IRMS = row.Field<string>("IRMS"),
                       SSCC = row.Field<string>("SSCC"),
                       YJNOrder = row.Field<string>("YJNOrderNo"),
                       NetKg = row.Field<decimal>("Kgs"),
                       JumboRoll = string.IsNullOrEmpty(row.Field<string>("RollNo"))?"":row.Field<string>("RollNo").Substring(8, 2),
                       TareKg = row.Field<decimal>("TareKg") /*,
                       Kgs = row.Field<decimal>("Kgs") + row.Field<decimal>("TareKg")*/
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
        public static ServiceOutput GetRollsForOrder(string yjnOrderNo)
        {
            var serviceOutput = new ServiceOutput();
            var databaseConnection = AppUtility.GetSAPConnectionString();
            var commandTimeOut = AppUtility.GetSqlCommandTimeOut();
            try
            {
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand("_sii_rpr_sps_getRollsForProdOrder", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = commandTimeOut;
                    cmd.Parameters.AddWithValue("@yjnOrderNo", yjnOrderNo);
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
                       Kgs = row.Field<decimal>("Kgs"),
                       JumboRoll = row.Field<string>("RollNo").Substring(8,2),
                       StorLocCode = row.Field<string>("StorLocCode"),
                       QualityStatus = row.Field<string>("QualityStatus"),
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
        public static ServiceOutput GetProdLineInputMaterial(string prodLine,string packingMtlLoc)
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
                       InDate = row.Field < DateTime ?>("InDate"),
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
                       BatchControlled = row.Field<int>("BatchControlled") == 1?true:false,
                       PackagingMtl = row.Field<int>("PackagingMtl") ==1?true:false,
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
                    cmd.Parameters.AddWithValue("@includeMixLines", includeMixed?"Y":"N");
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
                    cmd.Parameters.AddWithValue("@docNum",sapOrderNo);
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
    }
}
