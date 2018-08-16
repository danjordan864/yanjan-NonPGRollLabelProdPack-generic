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
        public static ServiceOutput GetOpenProdOrders(string prodLine)
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
                       ProductionLine= row.Field<string>("ProductionLine"),
                       ProductionMachineNo = row.Field<string>("ProductionMachineNo"),
                       MaterialCode= row.Field<string>("MaterialCode"),
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
                       YJNOrder = row.Field<string>("YJNOrderNo"),
                       Kgs = row.Field<decimal>("Kgs")
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

      
        public static ServiceOutput GetProdLineInputMaterial(string prodLine)
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
                       InDate = row.Field < DateTime ?>("InDate")
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
        public static ServiceOutput CreateSSCC()
        {
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
                }
                using (SqlConnection cnx = new SqlConnection(databaseConnection))
                using (SqlCommand cmd = new SqlCommand($"SELECT SSCC FROM PMX_LUID WHERE InternalKey = {luid}", cnx))
                {
                    cnx.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = commandTimeOut;
                    serviceOutput.ResultSet = AppUtility.PopulateDataSet(cmd);
                    sscc = serviceOutput.ResultSet.Tables[0].Rows[0]["SSCC"].ToString();
                }
                serviceOutput.SuccessFlag = true;
                serviceOutput.ReturnValue = new KeyValuePair<int, string>(luid, sscc);
            }
            catch (Exception ex)
            {
                serviceOutput.CallStack = ex.StackTrace;
                serviceOutput.MethodName = AppUtility.GetCurrentMethod();
                serviceOutput.ServiceException = $"Method:{serviceOutput.MethodName}. Error:{ex.Message}";
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
    }
}
