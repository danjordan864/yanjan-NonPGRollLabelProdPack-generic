using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.SAP.B1.DocumentObjects
{
    /// <summary>
    /// Wrapper class for the line items on a given document.
    /// </summary>
    public class InventoryIssueLine
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


        public int BaseEntry { get { return _line.BaseEntry; } }

       
        public int BaseLine { get { return _line.BaseLine; } }

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
        public InventoryIssueLine(Document_Lines line)
        {
            _line = line;
        }

        /// <summary>
        /// Links the wrapper class to a given SAP B1 document line object
        /// </summary>
        /// <param name="productionOrder">The SAP B1 document object that owns the line item.</param>
        /// <param name="itemCode">Line item code (item Primary key)</param>
        public InventoryIssueLine(IDocuments issue, int baseEntry, int baseLine, string itemCode, double quantity,  string storageLoc, string qualityStatus, string batchNo, int luid, string sscc, string uom, string lotNumber)
        {
            int index = -1;
            issue.Lines.BaseEntry = baseEntry;
            issue.Lines.BaseLine = baseLine;
            issue.Lines.Quantity = quantity;
            issue.Lines.BaseType = 202;

            if (!string.IsNullOrEmpty(batchNo))
            {
                issue.Lines.BatchNumbers.BatchNumber = batchNo;
                issue.Lines.BatchNumbers.Quantity = quantity;
                issue.Lines.BatchNumbers.Notes = lotNumber;
               //Batch
                index = UDFIndexLocation(issue.Lines, "U_PMX_BATC");
                if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = batchNo; }

            }
            //Storage Location
           index = -1;
            index = UDFIndexLocation(issue.Lines, "U_PMX_LOCO");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = storageLoc; }

            //Quality Status
            index = -1;
            index = UDFIndexLocation(issue.Lines, "U_PMX_QYSC");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = qualityStatus; }

            //Quantity
            index = -1;
            index = UDFIndexLocation(issue.Lines, "U_PMX_QUAN");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = quantity.ToString(); }


            //luid
            index = -1;
            index = UDFIndexLocation(issue.Lines, "U_PMX_LUID");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = luid.ToString(); }

            //base entry
            index = -1;
            index = UDFIndexLocation(issue.Lines, "U_PMX_BAEN");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = baseEntry; }

            //base type
            index = -1;
            index = UDFIndexLocation(issue.Lines, "U_PMX_BATY");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = "202"; }

            //production batch
            index = -1;
            index = UDFIndexLocation(issue.Lines, "U_PMX_PRDB");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = 1; }

            //sscc
            index = -1;
            index = UDFIndexLocation(issue.Lines, "U_PMX_SSCC");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = sscc; }

            issue.Lines.Add();
            _line = issue.Lines;
        }

        #endregion


        #region methods

        /// <summary></summary>
        private static int UDFIndexLocation(IDocument_Lines issueLines, string udfName)
        {
            int index = -1;
            for (int i = 0; i < issueLines.UserFields.Fields.Count; i++)
            {
                if (issueLines.UserFields.Fields.Item(i).Name == udfName)
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
