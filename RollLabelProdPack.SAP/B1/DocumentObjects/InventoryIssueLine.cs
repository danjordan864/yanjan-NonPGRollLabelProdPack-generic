using log4net;
using RollLabelProdPack.Library.Data;
using SAPbobsCOM;

namespace RollLabelProdPack.SAP.B1.DocumentObjects
{
    /// <summary>
    /// Wrapper class for the line items on a given document.
    /// </summary>
    /// <remarks>
    /// This code defines a wrapper class called InventoryIssueLine for the line items on a given document in SAP Business One (SAP B1). 
    /// It includes properties to access various attributes of the line item, such as DocEntry, LineNumber, BaseEntry, and ItemCode. 
    /// The class provides constructors to initialize instances of InventoryIssueLine and link them to SAP B1 document 
    /// line objects.
    /// 
    /// The constructors allow you to set different properties of the InventoryIssueLine, such as base entry, 
    /// base line, item code, quantity, storage location, quality status, batch number, LUID (Logistic Unit Identifier), 
    /// SSCC (Serial Shipping Container Code), unit of measure, lot number, scrap offset code, scrap reason, and 
    /// shift. These properties are set using the IDocuments and Document_Lines objects from the SAP B1 API. 
    /// 
    /// The class also includes a method called IsCommitted() to check whether the line item has been 
    /// committed to the database.
    /// </remarks>
    public class InventoryIssueLine
    {

        #region variables

        /// <summary></summary>
        Document_Lines _line = null;

        ILog _log = null;

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
        /// Gets the base entry of the line item.
        /// </summary>
        /// <remarks>
        /// The base entry is the document entry of the document on which the line item is based. For example, 
        /// in the case of a goods receipt, the base entry would be the purchase order or production order from 
        /// which the goods were received.
        /// </remarks>
        /// <value>The base entry of the line item.</value>
        public int BaseEntry { get { return _line.BaseEntry; } }

        /// <summary>
        /// Gets the base line number of the line item.
        /// </summary>
        /// <remarks>
        /// The base line number is used to link the line item to its corresponding base document line in 
        /// SAP Business One.
        /// </remarks>
        public int BaseLine { get { return _line.BaseLine; } }

        /// <summary>
        /// Line item code (item Primary key)
        /// </summary>
        public string ItemCode { get { return _line.ItemCode; } set { _line.ItemCode = value; } }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryIssueLine"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is used to create a default instance of the <see cref="InventoryIssueLine"/> class.
        /// </remarks>
        public InventoryIssueLine()
        {
            _log = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Links the wrapper class to a given SAP B1 document line object
        /// </summary>
        /// <param name="line"></param>
        public InventoryIssueLine(Document_Lines line) : this()
        {
            _line = line;
        }

        /// <summary>
        /// Links the wrapper class to a given SAP B1 document line object
        /// </summary>
        /// <param name="productionOrder">The SAP B1 document object that owns the line item.</param>
        /// <param name="itemCode">Line item code (item Primary key)</param>
        public InventoryIssueLine(IDocuments issue, int baseEntry, int baseLine, string itemCode, double quantity, string storageLoc, string qualityStatus, string batchNo, int luid, string sscc, string uom, string lotNumber)
            : this()
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug("In InventoryIssueLine - first constructor");
                _log.Debug($"baseEntry = {baseEntry}");
                _log.Debug($"baseLine = {baseLine}");
                _log.Debug($"itemCode = {itemCode}");
                _log.Debug($"quantity = {quantity}");
                _log.Debug($"storageLoc = {storageLoc}");
                _log.Debug($"qualityStatus = {qualityStatus}");
                _log.Debug($"batchNo = {batchNo}");
                _log.Debug($"luid = {luid}");
                _log.Debug($"sscc = {sscc}");
                _log.Debug($"uom = {uom}");
                _log.Debug($"lotNumber = {lotNumber}");
            }

            int index = -1;
            issue.Lines.BaseEntry = baseEntry;
            issue.Lines.BaseLine = baseLine;
            issue.Lines.Quantity = quantity;
            issue.Lines.BaseType = 202;

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


            //luid and sscc
            if (luid != 0)
            {
                index = -1;
                index = UDFIndexLocation(issue.Lines, "U_PMX_LUID");
                if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = luid.ToString(); }

                //sscc
                index = -1;
                index = UDFIndexLocation(issue.Lines, "U_PMX_SSCC");
                if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = sscc; }
            }

            //"PMX_INVT"."ItemTransactionalInfoKey"
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

            //Lot No
            index = -1;
            index = UDFIndexLocation(issue.Lines, "U_SII_LotNo");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = lotNumber; }

            // Check to see whether the item is expecting a second batch number. If it is true,
            // then try to populate it; otherwise don't.
            var so = AppData.GetItem(itemCode);
            if (so.SuccessFlag)
            {
                var item = (RollLabelProdPack.Library.Entities.Item)so.ReturnValue;
                if (item.HasSecondBatchNumber)
                {
                    if (!string.IsNullOrEmpty(batchNo))
                    {
                        issue.Lines.BatchNumbers.BatchNumber = batchNo;
                        issue.Lines.BatchNumbers.Quantity = quantity;
                        issue.Lines.BatchNumbers.Notes = lotNumber;
                        //Batch
                        index = UDFIndexLocation(issue.Lines, "U_PMX_BATC");
                        if (_log.IsDebugEnabled) { _log.Debug($"UDFIndexLocation for U_PMX_BATC = {index}"); }
                        if (index != -1)
                        {
                            issue.Lines.UserFields.Fields.Item(index).Value = batchNo;
                            if (_log.IsDebugEnabled) { _log.Debug($"U_PMX_BATC set to {issue.Lines.UserFields.Fields.Item(index).Value}"); }
                        }
                    }
                }


                //Batch id

            }

            issue.Lines.Add();
            _line = issue.Lines;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryIssueLine"/> class with the specified parameters.
        /// </summary>
        /// <param name="issue">The SAP B1 document object that owns the line item.</param>
        /// <param name="itemCode">The line item code (item Primary key).</param>
        /// <param name="quantity">The quantity of the line item.</param>
        /// <param name="storageLoc">The storage location for the line item.</param>
        /// <param name="qualityStatus">The quality status of the line item.</param>
        /// <param name="batchNo">The batch number for the line item (optional).</param>
        /// <param name="luid">The LUID (optional).</param>
        /// <param name="sscc">The SSCC (optional).</param>
        /// <param name="uom">The unit of measure for the line item.</param>
        /// <param name="lotNumber">The lot number for the line item (optional).</param>
        /// <param name="scrapOffsetCode">The scrap offset code.</param>
        /// <param name="scrapReason">The scrap reason for the line item.</param>
        /// <param name="shift">The shift for the line item.</param>
        /// <remarks>
        /// This constructor initializes a new instance of the <see cref="InventoryIssueLine"/> class with the 
        /// specified parameters. It sets the values for various properties of the SAP B1 document line object, 
        /// such as quantity, item code, storage location, quality status, batch number, LUID, SSCC, unit of measure, 
        /// lot number, scrap offset code, scrap reason, and shift.
        /// </remarks>
        public InventoryIssueLine(IDocuments issue, string itemCode, double quantity, string storageLoc, string qualityStatus,
            string batchNo, int luid, string sscc, string uom, string lotNumber, string scrapOffsetCode, string scrapReason, string shift)
            : this()
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug("In InventoryIssueLine - second constructor");
                _log.Debug($"itemCode = {itemCode}");
                _log.Debug($"quantity = {quantity}");
                _log.Debug($"storageLoc = {storageLoc}");
                _log.Debug($"qualityStatus = {qualityStatus}");
                _log.Debug($"batchNo = {batchNo}");
                _log.Debug($"luid = {luid}");
                _log.Debug($"sscc = {sscc}");
                _log.Debug($"uom = {uom}");
                _log.Debug($"lotNumber = {lotNumber}");
                _log.Debug($"scrapOffsetCode = {scrapOffsetCode}");
                _log.Debug($"scrapReason = {scrapReason}");
                _log.Debug($"shift = {shift}");
            }

            int index = -1;
            issue.Lines.Quantity = quantity;
            issue.Lines.BaseType = -1;
            issue.Lines.ItemCode = itemCode;

            if (!string.IsNullOrEmpty(batchNo))
            {
                issue.Lines.BatchNumbers.BatchNumber = batchNo;
                issue.Lines.BatchNumbers.Quantity = quantity;
                issue.Lines.BatchNumbers.Notes = lotNumber;
                //Batch
                index = UDFIndexLocation(issue.Lines, "U_PMX_BATC");
                if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = batchNo; }

                index = UDFIndexLocation(issue.Lines, "U_PMX_BAT2");
                if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = lotNumber; }
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


            //luid and sscc
            if (luid != 0)
            {
                index = -1;
                index = UDFIndexLocation(issue.Lines, "U_PMX_LUID");
                if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = luid.ToString(); }

                //sscc
                index = -1;
                index = UDFIndexLocation(issue.Lines, "U_PMX_SSCC");
                if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = sscc; }
            }

            //Shift
            index = -1;
            index = UDFIndexLocation(issue.Lines, "U_SII_Shift");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = shift; }

            //scrap reason
            index = -1;
            index = UDFIndexLocation(issue.Lines, "U_SII_ScrapReason");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = scrapReason; }

            //Lot No
            index = -1;
            index = UDFIndexLocation(issue.Lines, "U_SII_LotNo");
            if (index != -1) { issue.Lines.UserFields.Fields.Item(index).Value = lotNumber; }

            issue.Lines.Add();
            _line = issue.Lines;
        }
        #endregion


        #region methods

        /// <summary>
        /// Retrieves the index location of a user-defined field (UDF) within the document lines.
        /// </summary>
        /// <param name="issueLines">The SAP B1 document lines object.</param>
        /// <param name="udfName">The name of the user-defined field.</param>
        /// <returns>
        /// The index location of the specified user-defined field within the document lines. Returns -1 
        /// if the user-defined field is not found.
        /// </returns>
        /// <remarks>
        /// This method searches for a user-defined field with the specified name within the SAP B1 document 
        /// lines object. It returns the index location of the user-defined field if found, or -1 if the 
        /// user-defined field is not found.
        /// </remarks>
        public static int UDFIndexLocation(IDocument_Lines issueLines, string udfName)
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
