using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.SAP.B1.DocumentObjects
{
    public class InventoryReceiptLine
    {

        #region variables

        /// <summary></summary>
        Document_Lines _line = null;

        #endregion

        #region properties

        /// <summary>
        /// production order number of the associated SAP B1 document object.
        /// </summary>
        public int DocEntry { get { return _line.DocEntry; } }

        /// <summary>
        /// The line number of the given document line as known by the associated SAP B1 document object.
        /// </summary>
        public int LineNumber { get { return _line.LineNum; } }

        /// <summary>
        /// Line item code (item Primary key)
        /// </summary>
        public string ItemCode { get { return _line.ItemCode; } set { _line.ItemCode = value; } }

        #endregion

        #region constructors

        /// <summary>
        /// Links the wrapper class to a given SAP B1 document line object
        /// </summary>
        /// <param name="line"></param>
        public InventoryReceiptLine(Document_Lines line)
        {
            _line = line;
        }

        /// <summary>
        /// Links the wrapper class to a given SAP B1 document line object
        /// </summary>
        /// <param name="productionOrder">The SAP B1 document object that owns the line item.</param>
        /// <param name="itemCode">Line item code (item Primary key)</param>
        public InventoryReceiptLine(IDocuments inventoryIssue, string itemCode, double quantity, string issueMethod, string warehouse, int opSeq, string medFile, double scrapPct, string wrkInstr, double qNeed)
        {


            //if (itemCode.Substring(0, 2) == "R_") { productionOrder.Lines.ItemType = ProductionItemType.pit_Resource; }
            //else { productionOrder.Lines.ItemType = ProductionItemType.pit_Item; }

            //productionOrder.Lines.ItemNo = itemCode;
            //productionOrder.Lines.BaseQuantity = quantity;
            //productionOrder.Lines.ProductionOrderIssueType = BoIssueMethod.im_Manual;
            //productionOrder.Lines.Warehouse = warehouse;


            //var index = -1;
            //index = UDFIndexLocation(productionOrder.Lines, "U_NBS_OpSeq");
            //if (index != -1) { productionOrder.Lines.UserFields.Fields.Item(index).Value = opSeq; }

            //index = -1;
            //index = UDFIndexLocation(productionOrder.Lines, "U_NBS_MedFile");
            //if (index != -1 && medFile.Trim().Length > 0) { productionOrder.Lines.UserFields.Fields.Item(index).Value = medFile; }

            //index = -1;
            //index = UDFIndexLocation(productionOrder.Lines, "U_NBS_ScrapPct");
            //if (index != -1) { productionOrder.Lines.UserFields.Fields.Item(index).Value = scrapPct; }

            //index = -1;
            //index = UDFIndexLocation(productionOrder.Lines, "U_NBS_WrkInstr");
            //if (index != -1 && wrkInstr.Trim().Length > 0) { productionOrder.Lines.UserFields.Fields.Item(index).Value = wrkInstr; }

            //index = -1;
            //index = UDFIndexLocation(productionOrder.Lines, "U_NB_QNeed");
            //if (index != -1) { productionOrder.Lines.UserFields.Fields.Item(index).Value = qNeed; }

            //productionOrder.Lines.Add();
            //_line = productionOrder.Lines;
        }

        #endregion


        #region methods

        /// <summary></summary>
        private static int UDFIndexLocation(IProductionOrders_Lines sapOrder, string udfName)
        {
            int index = -1;
            for (int i = 0; i < sapOrder.UserFields.Fields.Count; i++)
            {
                if (sapOrder.UserFields.Fields.Item(i).Name == udfName)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }


        #endregion


    }
}
