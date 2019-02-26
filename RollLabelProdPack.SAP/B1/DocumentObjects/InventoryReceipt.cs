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

        public InventoryReceipt(Company sapCompany)
        {
            _sapCompany = sapCompany;
            _receipt = (SAPbobsCOM.IDocuments)_sapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
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

        public void AddLine(int baseEntry, string itemCode, double quantity, int prodBatchNo, string storageLoc, string qualityStatus, string batchNo, int luid, string sscc, string uom, string lotNumber,bool isScrap, int scrapLine,
            string shift, string user, string scrapReason = null, string scrapGLOffset = null)
        {
            _receiptLines.Add(new InventoryReceiptLine(_receipt, baseEntry, itemCode, quantity, prodBatchNo,  storageLoc, qualityStatus, batchNo, luid, sscc, uom, lotNumber,isScrap,scrapLine,shift,user,scrapReason,scrapGLOffset));
        }

        public BoObjectTypes SAPObjectType
        {
            get
            {
                return _objectType;
            }
        }

        public void Dispose()
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_receipt);
            _receipt = null;
            GC.Collect();
        }

        public void LoadUDFDictionary()
        {
            for (int i = 0; i < _receipt.UserFields.Fields.Count; i++)
                _userDefinedFieldsDictionary.Add(_receipt.UserFields.Fields.Item(i).Name, i);
        }

        public void SetUDFValue(string key, dynamic value)
        {
            _receipt.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value = value;
        }

        public void SetUDFValue(int key, dynamic value)
        {
            _receipt.UserFields.Fields.Item(key).Value = value;
        }

        public dynamic GetUDFValue(string key)
        {
            return _receipt.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value;
        }

        public dynamic GetUDFValue(int key)
        {
            return _receipt.UserFields.Fields.Item(key).Value;
        }

        public bool Save()
        {
            int returnCode;
            if (_receipt.DocNum == 0)
            {
                returnCode = _receipt.Add();

            }
            else
            {
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
