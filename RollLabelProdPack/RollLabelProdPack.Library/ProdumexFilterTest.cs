using System;
using System.Reflection;
//using Produmex.Foundation.Data.Sbo;
//using Produmex.Foundation.Diagnostics;
//using Produmex.Sbo.Logex.Data.BusinessObjects;
//using Produmex.Foundation.Data.Sbo.BusinessObjects;
//using Produmex.Foundation.Data.Sbo.Utilities;


namespace RollLabelProdPack.Library
{
    //class ProdumexFilterTest
    //{

    //    private static readonly ILog s_log =
    //            LogProvider.GetLogger(MethodInfo.GetCurrentMethod().DeclaringType);

    //    public static bool PrintReport(PmxPrintReportEventType eventType, int key, PmxDbConnection dbConn)
    //    {
    //        //Create the query you want to use
    //        string query = "SELECT  COUNT(rolls.Code) as NoOfBatches, Coalesce(MIN(OWOR.CardCode),'') as CardCode ";
    //        query = query + "FROM [PMX_INVENTORY_REPORT_DETAIL] inv INNER JOIN [@SII_ROLLS] rolls on inv.BatchNumber1 = rolls.Code ";
    //        query = query + "INNER JOIN OWOR ON OWOR.DocNum = rolls.U_SII_ProdOrder WHERE LUID =" + key.ToString();
    //        query = query + " GROUP BY rolls.U_SII_ProdOrder,OWOR.CardCode";


    //        //Run the query
    //        using (ISboRecordset rs = SboRecordsetHelper.RunQuery(s_log, query, dbConn))
    //        {
    //            if (!rs.EoF)//Check if you get result from the query
    //            {
    //                string cardCode = rs.GetTypedValue<string>("CardCode");//Get a string value
    //                int noOfBatches = rs.GetTypedValue<int>("NoOfBatches");//Get an int value

    //                //Possibility to add a check on the result
    //                //In this case if the value of column with name 'COLUMNAME2' equals to 99, 
    //                //a label should be printed
    //                if (cardCode == "C1001" && noOfBatches > 1)
    //                {
    //                    string query2 = "INSERT INTO [dbo].[@SII_PG_BUNDLE]([Name],[U_SII_InternalSSCC],[U_SII_ItemCode],[U_SII_ItemName]";
    //                    query2 = query2 + ",[U_SII_IRMS],[U_SII_YJNOrder],[U_SII_Lot],[U_SII_SSCC])";
    //                    query2 = query2 + "VALUES ('Test','00','','123' ,'123' ,'123' ,'123','123')";
    //                    SboRecordsetHelper.RunQueryNoReturn(s_log, query2, dbConn);
    //                    return true; //Label will be printed
    //                }
    //                else
    //                {
    //                    return false; //Label will not be printed
    //                }
    //            }
    //        }
    //        return false; //Label will not be printed
    //    }

    //}
}