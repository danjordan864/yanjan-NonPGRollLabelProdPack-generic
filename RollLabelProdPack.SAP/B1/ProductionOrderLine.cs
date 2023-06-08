using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.SAP.B1
{
    /// <summary>
    /// Wrapper class for the line items on a given document.
    /// </summary>
    public class ProductionOrderLine
    {

        #region variables

        /// <summary></summary>
        ProductionOrders_Lines _line = null;

        #endregion

        #region properties

        /// <summary>
        /// production order number of the associated SAP B1 document object.
        /// </summary>
        public int DocEntry { get { return _line.DocumentAbsoluteEntry; } }

        /// <summary>
        /// The line number of the given document line as known by the associated SAP B1 document object.
        /// </summary>
        public int LineNumber { get { return _line.LineNumber; } }

        /// <summary>
        /// Gets the issue type of the production order line.
        /// </summary>
        public BoIssueMethod IssueType { get { return _line.ProductionOrderIssueType; } }


        /// <summary>
        /// Line item code (item Primary key)
        /// </summary>
        public string ItemCode { get { return _line.ItemNo; } set { _line.ItemNo = value; } }

        /// <summary>
        /// Gets the base quantity of the production order line.
        /// </summary>
        public double BaseQty { get { return _line.BaseQuantity; }}

        /// <summary>
        /// Gets the unit of measure code for the production order line.
        /// </summary>
        public string UomCode { get { return _line.UoMCode; } }



        #endregion

        #region constructors

        /// <summary>
        /// Links the wrapper class to a given SAP B1 document line object
        /// </summary>
        /// <param name="line"></param>
        public ProductionOrderLine(ProductionOrders_Lines line)
        {
            _line = line;
        }

        /// <summary>
        /// Links the wrapper class to a given SAP B1 document line object
        /// </summary>
        /// <param name="productionOrder">The SAP B1 document object that owns the line item.</param>
        /// <param name="itemCode">Line item code (item Primary key)</param>
        public ProductionOrderLine(ProductionOrders productionOrder, string itemCode, double quantity, string issueMethod, string warehouse, int opSeq, string medFile, double scrapPct, string wrkInstr, double qNeed)
        {


            if (itemCode.Substring(0, 2) == "R_") { productionOrder.Lines.ItemType = ProductionItemType.pit_Resource; }
            else { productionOrder.Lines.ItemType = ProductionItemType.pit_Item; }

            productionOrder.Lines.ItemNo = itemCode;
            productionOrder.Lines.BaseQuantity = quantity;
            productionOrder.Lines.ProductionOrderIssueType = BoIssueMethod.im_Manual;
            productionOrder.Lines.Warehouse = warehouse;


            var index = -1;
            index = UDFIndexLocation(productionOrder.Lines, "U_NBS_OpSeq");
            if (index != -1) { productionOrder.Lines.UserFields.Fields.Item(index).Value = opSeq; }

            index = -1;
            index = UDFIndexLocation(productionOrder.Lines, "U_NBS_MedFile");
            if (index != -1 && medFile.Trim().Length > 0) { productionOrder.Lines.UserFields.Fields.Item(index).Value = medFile; }

            index = -1;
            index = UDFIndexLocation(productionOrder.Lines, "U_NBS_ScrapPct");
            if (index != -1) { productionOrder.Lines.UserFields.Fields.Item(index).Value = scrapPct; }

            index = -1;
            index = UDFIndexLocation(productionOrder.Lines, "U_NBS_WrkInstr");
            if (index != -1 && wrkInstr.Trim().Length > 0) { productionOrder.Lines.UserFields.Fields.Item(index).Value = wrkInstr; }

            index = -1;
            index = UDFIndexLocation(productionOrder.Lines, "U_NB_QNeed");
            if (index != -1) { productionOrder.Lines.UserFields.Fields.Item(index).Value = qNeed; }

            productionOrder.Lines.Add();
            _line = productionOrder.Lines;
        }

        #endregion


        #region methods

        /// <summary>
        /// Finds the index of a user-defined field (UDF) in the production order line.
        /// </summary>
        /// <param name="sapOrder">The production order line object.</param>
        /// <param name="udfName">The name of the user-defined field.</param>
        /// <returns>The index of the user-defined field if found, or -1 if the field is not found.</returns>
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
