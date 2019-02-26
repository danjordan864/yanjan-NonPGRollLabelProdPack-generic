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


        public int BaseEntry { get { return _line.BaseEntry; } }


        public int BaseLine { get { return _line.BaseLine; } }


        /// <summary>
        /// Line item code (item Primary key)
        /// </summary>
        //public string ItemCode { get { return _line.ItemCode; } set { _line.ItemCode = value; } }



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
        public InventoryReceiptLine(IDocuments receipt, int baseEntry, string itemCode,
            double quantity, int prodBatchNo, string storageLoc,
            string qualityStatus, string batchNo, int luid, string sscc, string uom,
            string lotNumber, bool isScrap, int scrapLine, string shift, string user,
            string scrapReason = null, string scrapGLOffset = null)
        {
            int index = -1;
            receipt.Lines.Quantity = quantity;
            if (scrapGLOffset == null)
            {
                receipt.Lines.BaseEntry = baseEntry;
                receipt.Lines.BaseType = 202;

                //base entry
                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_PMX_BAEN");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = baseEntry; }

                //base type
                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_PMX_BATY");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = "202"; }
            }
            else
            {
                receipt.Lines.BaseType = -1;
                receipt.Lines.AccountCode = scrapGLOffset;
                receipt.Lines.ItemCode = itemCode;
            }

            if (isScrap)
            {
                receipt.Lines.BaseLine = scrapLine;
                //Scrap  Reason
                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_SII_ScrapReason");
                if (index != -1 && scrapReason != null) { receipt.Lines.UserFields.Fields.Item(index).Value = scrapReason; }
            }


            if (!string.IsNullOrEmpty(batchNo) && !isScrap)
            {
                receipt.Lines.BatchNumbers.BatchNumber = batchNo;
                receipt.Lines.BatchNumbers.Quantity = quantity;
                receipt.Lines.BatchNumbers.Notes = lotNumber;
                //Batch
                index = UDFIndexLocation(receipt.Lines, "U_PMX_BATC");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = batchNo; }

                if (!string.IsNullOrEmpty(lotNumber))
                {
                    index = UDFIndexLocation(receipt.Lines, "U_PMX_BAT2");
                    if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = lotNumber; }
                }


            }

            //production batch
            index = -1;
            index = UDFIndexLocation(receipt.Lines, "U_PMX_PRDB");
            if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = prodBatchNo; }
            //Storage Location
            index = -1;
            index = UDFIndexLocation(receipt.Lines, "U_PMX_LOCO");
            if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = storageLoc; }

            //Quality Status
            index = -1;
            index = UDFIndexLocation(receipt.Lines, "U_PMX_QYSC");
            if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = qualityStatus; }

            //Quantity
            index = -1;
            index = UDFIndexLocation(receipt.Lines, "U_PMX_QUAN");
            if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = quantity.ToString(); }


            //luid
            index = -1;
            index = UDFIndexLocation(receipt.Lines, "U_PMX_LUID");
            if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = luid.ToString(); }

            //sscc
            index = -1;
            index = UDFIndexLocation(receipt.Lines, "U_PMX_SSCC");
            if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = sscc; }


            //RollNo
            index = -1;
            index = UDFIndexLocation(receipt.Lines, "U_SII_RollNo");
            if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = batchNo; }

            //Shift
            index = -1;
            index = UDFIndexLocation(receipt.Lines, "U_SII_Shift");
            if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = shift; }

            //User
            index = -1;
            index = UDFIndexLocation(receipt.Lines, "U_SII_User");
            if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = user; }

            //Lot No
            index = -1;
            index = UDFIndexLocation(receipt.Lines, "U_SII_LotNo");
            if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = lotNumber; }

            receipt.Lines.Add();
            _line = receipt.Lines;
        }

        #endregion


        #region methods

        /// <summary></summary>
        private static int UDFIndexLocation(IDocument_Lines receiptLines, string udfName)
        {
            int index = -1;
            for (int i = 0; i < receiptLines.UserFields.Fields.Count; i++)
            {
                if (receiptLines.UserFields.Fields.Item(i).Name == udfName)
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