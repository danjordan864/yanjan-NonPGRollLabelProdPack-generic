using log4net;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.SAP.B1.DocumentObjects
{
    public class InventoryReceipt : IDisposable, IB1Object
    {
        #region Variables
        private BoObjectTypes _objectType = BoObjectTypes.oInventoryGenEntry;
        private Company _sapCompany;
        /// <summary></summary>
        private Dictionary<string, int> _userDefinedFieldsDictionary = new Dictionary<string, int>();

        private IDocuments _receipt = null;
        private List<InventoryReceiptLine> _receiptLines = new List<InventoryReceiptLine>();
        /// <summary></summary>
        private bool _isNew = true;

        private ILog _log;

        /// <summary>
        /// Initializes a new instance of the InventoryReceipt class with the specified SAP company.
        /// </summary>
        /// <param name="sapCompany">The SAP company object.</param>
        /// <remarks>
        /// The constructor of the InventoryReceipt class initializes a new instance of the class with the 
        /// provided SAP company object. The constructor assigns the SAP company object, creates a new SAP 
        /// Business One document object of type oInventoryGenEntry (inventory general entry), and initializes 
        /// the logger using log4net.
        /// </remarks>
        public InventoryReceipt(Company sapCompany)
        {
            _sapCompany = sapCompany;
            _receipt = (SAPbobsCOM.IDocuments)_sapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
            _log = LogManager.GetLogger(this.GetType());
        }

        #endregion Variables

        #region Properties
        /// <summary>
        /// Returns the root SAP B1 object's User Defined Fields object based on the SAP DI API UserFields interface.
        /// </summary>
        public UserFields UserDefinedFields { get { return _receipt.UserFields; } }

        /// <summary>
        /// Returns a dictionary of the User Defined Fields (UDF) used by the root SAP B1 object in the class.  The dictionary is based on the 
        /// string name of the UDF and returns the given UDF's index position as known by the UserDefinedFields property.
        /// </summary>
        public Dictionary<string, int> UserDefinedFieldsDictionary { get { return _userDefinedFieldsDictionary; } set { _userDefinedFieldsDictionary = value; } }

        /// <summary>
        /// Returns the underlying SAP B1 Item object.
        /// </summary>
        public IDocuments Receipt { get { return _receipt; } set { _receipt = value; } }

        /// <summary>
        /// List of line items associated with the given SAP B1 document object
        /// </summary>
        public List<InventoryReceiptLine> ReceiptLines { get { return _receiptLines; } set { _receiptLines = value; } }
        #endregion Properties
        /// <summary>
        /// Creates an _issue of the wrapper class for the SAP B1 Production Order Object (oItems)
        /// </summary>
        /// <param name="sapCompany">The _issue of the SAP Connection Object used by the wrapper.</param>
        /// <param name="docEntry">The primary key of the payment to be loaded.</param>
        public InventoryReceipt(Company sapCompany, int docEntry) : this(sapCompany)
        {

            if (docEntry > 0)
            {
                if (_receipt.GetByKey(docEntry) == false)
                    throw new B1Exception(sapCompany, $"Unable to retrieve Issue {docEntry} from {sapCompany.CompanyName}");
                _isNew = false;

                // lines
                // -------------------------------------------------------------------------------------

                if (_receipt.Lines.Count > 0)
                    for (var i = 0; i < _receipt.Lines.Count; i++)
                    {
                        _receipt.Lines.SetCurrentLine(i);
                        InventoryReceiptLine line = new InventoryReceiptLine(_receipt.Lines);
                        _receiptLines.Add(line);
                    }
            }
        }

        /// <summary>
        /// Adds a line item to the inventory receipt.
        /// </summary>
        /// <param name="baseEntry">The base entry of the line item.</param>
        /// <param name="itemCode">The item code of the line item.</param>
        /// <param name="quantity">The quantity of the line item.</param>
        /// <param name="prodBatchNo">The production batch number of the line item.</param>
        /// <param name="storageLoc">The storage location of the line item.</param>
        /// <param name="qualityStatus">The quality status of the line item.</param>
        /// <param name="batchNo">The batch number of the line item.</param>
        /// <param name="luid">The LUID of the line item.</param>
        /// <param name="sscc">The SSCC of the line item.</param>
        /// <param name="uom">The unit of measure of the line item.</param>
        /// <param name="lotNumber">The lot number of the line item.</param>
        /// <param name="isScrap">Specifies if the line item is a scrap.</param>
        /// <param name="scrapLine">The scrap line number.</param>
        /// <param name="shift">The shift of the line item.</param>
        /// <param name="user">The user associated with the line item.</param>
        /// <param name="scrapReason">The scrap reason of the line item. (Optional)</param>
        /// <param name="scrapGLOffset">The scrap GL offset of the line item. (Optional)</param>
        /// <param name="isAdjustment">Specifies if the line item is an adjustment. (Default: false)</param>
        /// <remarks>
        /// The method adds a line item to the inventory receipt with various parameters such as base entry, 
        /// item code, quantity, production batch number, storage location, quality status, batch number, LUID, 
        /// SSCC, unit of measure, lot number, scrap information, shift, user, scrap reason, scrap GL offset, 
        /// and adjustment flag.
        /// </remarks>
        public void AddLine(int baseEntry, string itemCode, double quantity, int prodBatchNo, string storageLoc, string qualityStatus, string batchNo, int luid, string sscc, string uom, string lotNumber, bool isScrap, int scrapLine,
            string shift, string user, string scrapReason = null, string scrapGLOffset = null, bool isAdjustment = false)
        {
            if (_log.IsDebugEnabled)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("In InventoryReceipt.AddLine:");
                sb.AppendLine($"baseEntry: {baseEntry}");
                sb.AppendLine($"itemCode: {itemCode}");
                sb.AppendLine($"quantity: {quantity}");
                sb.AppendLine($"prodBatchNo: {prodBatchNo}");
                sb.AppendLine($"storageLoc: {storageLoc}");
                sb.AppendLine($"qualityStatus: {qualityStatus}");
                sb.AppendLine($"batchNo: {batchNo}");
                sb.AppendLine($"luid: {luid}");
                sb.AppendLine($"sscc: {sscc}");
                sb.AppendLine($"uom: {uom}");
                sb.AppendLine($"lotNumber: {lotNumber}");
                sb.AppendLine($"isScrap: {isScrap}");
                sb.AppendLine($"scrapLine: {scrapLine}");
                sb.AppendLine($"shift: {shift}");
                sb.AppendLine($"user: {user}");
                sb.AppendLine($"scrapReason: {scrapReason}");
                sb.AppendLine($"scrapGLOffset: {scrapGLOffset}");
                sb.Append($"isAdjustment: {isAdjustment}");
                _log.Debug(sb.ToString());
            }
            _receiptLines.Add(new InventoryReceiptLine(_receipt, baseEntry, itemCode, quantity, prodBatchNo, storageLoc, qualityStatus, batchNo, luid, sscc, uom, lotNumber, isScrap, scrapLine, shift, user, scrapReason, scrapGLOffset, isAdjustment));
        }

        /// <summary>
        /// Gets the SAP B1 object type of the inventory receipt.
        /// </summary>
        /// <remarks>
        /// The property retrieves the SAP B1 object type of the inventory receipt using the BoObjectTypes 
        /// enumeration.
        /// </remarks>
        public BoObjectTypes SAPObjectType
        {
            get
            {
                return _objectType;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>
        /// This method is used to perform cleanup tasks and release unmanaged resources. It releases the reference 
        /// to the _receipt object by calling ReleaseComObject, sets it to null, and triggers garbage collection with 
        /// GC.Collect().
        /// </remarks>
        public void Dispose()
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_receipt);
            _receipt = null;
            GC.Collect();
        }

        /// <summary>
        /// Loads the User Defined Fields (UDF) dictionary for the root SAP B1 object.
        /// </summary>
        /// <remarks>
        /// This method is used to load the User Defined Fields (UDF) dictionary for the root SAP B1 object. 
        /// It iterates over the UDF fields of the _receipt object and adds them to the _userDefinedFieldsDictionary, 
        /// using the UDF name as the key and the index position as the value.
        /// </remarks>
        public void LoadUDFDictionary()
        {
            for (int i = 0; i < _receipt.UserFields.Fields.Count; i++)
                _userDefinedFieldsDictionary.Add(_receipt.UserFields.Fields.Item(i).Name, i);
        }

        /// <summary>
        /// Sets the value of a User Defined Field (UDF) based on the specified key.
        /// </summary>
        /// <param name="key">The key of the UDF.</param>
        /// <param name="value">The value to be set.</param>
        /// <remarks>
        /// This method is used to set the value of a User Defined Field (UDF) based on the specified key. 
        /// It takes two parameters: key, which represents the key of the UDF, and value, which is the value 
        /// to be set. The method accesses the UDF field in the _receipt object using the key and assigns the 
        /// provided value to it.
        /// </remarks>
        public void SetUDFValue(string key, dynamic value)
        {
            _receipt.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value = value;
        }

        /// <summary>
        /// Sets the value of a User Defined Field (UDF) based on the specified position.
        /// </summary>
        /// <param name="pos">The position of the UDF.</param>
        /// <param name="value">The value to be set.</param>
        /// <remarks>
        /// This method is used to set the value of a User Defined Field (UDF) based on the specified 
        /// integer position. It takes two parameters: pos, which represents the position of the UDF, 
        /// and value, which is the value to be set. The method accesses the UDF field in the _receipt object 
        /// using the position and assigns the provided value to it.
        /// </remarks>
        public void SetUDFValue(int pos, dynamic value)
        {
            _receipt.UserFields.Fields.Item(pos).Value = value;
        }

        /// <summary>
        /// Gets the value of a User Defined Field (UDF) based on the specified key.
        /// </summary>
        /// <param name="key">The key of the UDF.</param>
        /// <returns>The value of the UDF.</returns>
        /// <remarks>
        /// This method is used to retrieve the value of a User Defined Field (UDF) based on the specified key. 
        /// It takes a single parameter key, which represents the key of the UDF. The method accesses the UDF 
        /// field in the _receipt object using the key and returns its value.
        /// </remarks>
        public dynamic GetUDFValue(string key)
        {
            return _receipt.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value;
        }

        /// <summary>
        /// Gets the value of a User Defined Field (UDF) based on the specified position.
        /// </summary>
        /// <param name="pos">The position of the UDF.</param>
        /// <returns>The value of the UDF.</returns>
        /// <remarks>
        /// This method is used to retrieve the value of a User Defined Field (UDF) based on the specified 
        /// position. It takes a single parameter pos, which represents the position of the UDF. The method 
        /// accesses the UDF field in the _receipt object using the position and returns its value.
        /// </remarks>
        public dynamic GetUDFValue(int pos)
        {
            return _receipt.UserFields.Fields.Item(pos).Value;
        }

        /// <summary>
        /// Saves the inventory receipt document.
        /// </summary>
        /// <returns>True if the document is successfully saved; otherwise, false.</returns>
        /// <remarks>
        /// The method is used to save the inventory receipt document. The method doesn't take any parameters 
        /// and returns a boolean value indicating whether the document was successfully saved or not.
        /// 
        /// If the DocNum property of the _receipt object is zero, it means a new document should be added. 
        /// Otherwise, an existing document should be updated. After calling the appropriate method (Add or 
        /// Update), the return code is checked. If it is non-zero, an exception is thrown with the error 
        /// details retrieved from the SAP company. Finally, the method returns true to indicate a successful 
        /// save operation.
        /// </remarks>
        public bool Save()
        {
            int returnCode;
            _log.Debug($"In Save: _receipt.DocNum = {_receipt.DocNum}");

            if (_receipt.DocNum == 0)
            {
                // Add new document
                returnCode = _receipt.Add();
            }
            else
            {
                // Update existing document
                returnCode = _receipt.Update();
            }

            if (returnCode != 0)
            {
                int errorCode = 0;
                string errorMessage = string.Empty;

                _sapCompany.GetLastError(out errorCode, out errorMessage);

                throw new Exception(string.Format("(APIs.InventoryReceipt)ERROR: {0}-{1}", errorCode, errorMessage));
            }

            return true;
        }

    }
}
