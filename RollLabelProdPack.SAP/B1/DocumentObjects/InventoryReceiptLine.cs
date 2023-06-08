using log4net;
using SAPbobsCOM;
using System;

namespace RollLabelProdPack.SAP.B1.DocumentObjects
{
    /// <summary>
    /// Represents a line item in an inventory receipt document.
    /// </summary>
    public class InventoryReceiptLine
    {

        #region variables

        /// <summary></summary>
        Document_Lines _line = null;

        #endregion

        #region properties

        /// <summary>
        /// Gets the document entry number of the associated SAP B1 document object.
        /// </summary>
        public int DocEntry { get { return _line.DocEntry; } }

        /// <summary>
        /// Gets the line number of the given document line as known by the associated SAP B1 document object.
        /// </summary>
        public int LineNumber { get { return _line.LineNum; } }

        /// <summary>
        /// Gets the line number of the given document line as known by the associated SAP B1 document object.
        /// </summary>
        public int BaseEntry { get { return _line.BaseEntry; } }

        /// <summary>
        /// Gets the base line of the associated SAP B1 document object.
        /// </summary>
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
        /// <param name="line">The SAP B1 document line object to link.</param>
        public InventoryReceiptLine(Document_Lines line)
        {
            _line = line;
        }

        private ILog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryReceiptLine"/> class.
        /// </summary>
        /// <param name="receipt">The SAP B1 document object that owns the line item.</param>
        /// <param name="baseEntry">The base entry.</param>
        /// <param name="itemCode">The line item code (item Primary key).</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="prodBatchNo">The production batch number.</param>
        /// <param name="storageLoc">The storage location.</param>
        /// <param name="qualityStatus">The quality status.</param>
        /// <param name="batchNo">The batch number.</param>
        /// <param name="luid">The LUID.</param>
        /// <param name="sscc">The SSCC.</param>
        /// <param name="uom">The unit of measure.</param>
        /// <param name="lotNumber">The lot number.</param>
        /// <param name="isScrap">Indicates whether it is a scrap item.</param>
        /// <param name="scrapLine">The scrap line number.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="user">The user.</param>
        /// <param name="scrapReason">The scrap reason.</param>
        /// <param name="scrapGLOffset">The scrap G/L offset account.</param>
        /// <param name="isAdjustment">Indicates whether it is an adjustment.</param>
        public InventoryReceiptLine(IDocuments receipt, int baseEntry, string itemCode,
            double quantity, int prodBatchNo, string storageLoc,
            string qualityStatus, string batchNo, int luid, string sscc, string uom,
            string lotNumber, bool isScrap, int scrapLine, string shift, string user,
            string scrapReason = null, string scrapGLOffset = null, bool isAdjustment = false)
        {
            // RDJ 20220228 - Added isAdjustment flag for bundle adjustments. These should not be linked
            // to a production order and use default G/L accounts.
            _log = LogManager.GetLogger(this.GetType());

            try
            {
                if (_log.IsDebugEnabled)
                {
                    // Debug log messages for parameter values
                    _log.Debug($"baseEntry = {baseEntry}");
                    _log.Debug($"itemCode = {itemCode}");
                    _log.Debug($"quantity = {quantity}");
                    _log.Debug($"prodBatchNo = {prodBatchNo}");
                    _log.Debug($"storageLoc = {storageLoc}");
                    _log.Debug($"qualityStatus = {qualityStatus}");
                    _log.Debug($"batchNo = {batchNo}");
                    _log.Debug($"luid = {luid}");
                    _log.Debug($"sscc = {sscc}");
                    _log.Debug($"uom = {uom}");
                    _log.Debug($"lotNumber = {lotNumber}");
                    _log.Debug($"isScrap = {isScrap}");
                    _log.Debug($"scrapLine = {scrapLine}");
                    _log.Debug($"shift = {shift}");
                    _log.Debug($"user = {user}");
                    _log.Debug($"scrapReason = {scrapReason}");
                    _log.Debug($"scrapGLOffset = {scrapGLOffset}");
                    _log.Debug($"isAdjustment = {isAdjustment}");
                }

                int index = -1;
                receipt.Lines.Quantity = quantity;

                if (scrapGLOffset == null)
                {
                    receipt.Lines.BaseEntry = baseEntry;
                    receipt.Lines.BaseType = 202;

                    // Set user-defined fields for base entry and base type
                    index = -1;
                    index = UDFIndexLocation(receipt.Lines, "U_PMX_BAEN");
                    if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = baseEntry; }

                    index = -1;
                    index = UDFIndexLocation(receipt.Lines, "U_PMX_BATY");
                    if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = "202"; }
                }
                else
                {
                    receipt.Lines.BaseType = -1;
                    // RDJ 20220228 - If isAdjustment is true, don't set the scrap G/L offset account
                    if (!isAdjustment)
                    {
                        receipt.Lines.AccountCode = scrapGLOffset;
                    }
                    receipt.Lines.ItemCode = itemCode;
                }

                if (isScrap)
                {
                    receipt.Lines.BaseLine = scrapLine;
                    // Set user-defined field for scrap reason
                    index = -1;
                    index = UDFIndexLocation(receipt.Lines, "U_SII_ScrapReason");
                    if (index != -1 && scrapReason != null) { receipt.Lines.UserFields.Fields.Item(index).Value = scrapReason; }
                }

                if (!string.IsNullOrEmpty(batchNo) && !isScrap)
                {
                    receipt.Lines.BatchNumbers.BatchNumber = batchNo;
                    receipt.Lines.BatchNumbers.Quantity = quantity;
                    receipt.Lines.BatchNumbers.Notes = lotNumber;

                    // Set user-defined field for batch number
                    index = UDFIndexLocation(receipt.Lines, "U_PMX_BATC");
                    if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = batchNo; }

                    if (!string.IsNullOrEmpty(lotNumber))
                    {
                        index = UDFIndexLocation(receipt.Lines, "U_PMX_BAT2");
                        if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = lotNumber; }
                    }
                }

                // Set user-defined fields for production batch, storage location, quality status, quantity, LUID, SSCC
                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_PMX_PRDB");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = prodBatchNo; }

                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_PMX_LOCO");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = storageLoc; }

                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_PMX_QYSC");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = qualityStatus; }

                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_PMX_QUAN");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = quantity.ToString(); }

                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_PMX_LUID");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = luid.ToString(); }

                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_PMX_SSCC");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = sscc; }

                // Set user-defined fields for roll number, shift, user, and lot number
                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_SII_RollNo");
                if (index != -1 && !string.IsNullOrEmpty(batchNo)) { receipt.Lines.UserFields.Fields.Item(index).Value = batchNo; }

                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_SII_Shift");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = shift; }

                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_SII_User");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = user; }

                index = -1;
                index = UDFIndexLocation(receipt.Lines, "U_SII_LotNo");
                if (index != -1) { receipt.Lines.UserFields.Fields.Item(index).Value = lotNumber; }

                receipt.Lines.Add();
                _line = receipt.Lines;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                throw;
            }
        }
        #endregion


        #region methods

        /// <summary>
        /// Finds the index of a user-defined field (UDF) in the UserFields collection.
        /// </summary>
        /// <param name="receiptLines">The document lines object.</param>
        /// <param name="udfName">The name of the UDF.</param>
        /// <returns>The index of the UDF if found; otherwise, -1.</returns>
        /// <remarks>
        /// This method iterates over the UserFields collection and compares the name of each UDF with the specified 
        /// udfName. If a match is found, it returns the index of that UDF. Otherwise, it returns -1 to indicate that 
        /// the UDF was not found.
        /// </remarks>
        private static int UDFIndexLocation(IDocument_Lines receiptLines, string udfName)
        {
            int index = -1;
            for (int i = 0; i < receiptLines.UserFields.Fields.Count; i++)
            {
                // Check if the name of the current UDF matches the specified UDF name
                if (receiptLines.UserFields.Fields.Item(i).Name == udfName)
                {
                    // Store the index of the matching UDF and break out of the loop
                    index = i;
                    break;
                }
            }
            return index;
        }



        #endregion


    }
}