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
        private BoObjectTypes _objectType = BoObjectTypes.oInventoryGenExit;
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

        /// <summary>
        /// Creates an _receipt of the wrapper class for the SAP B1 Production Order Object (oItems)
        /// </summary>
        /// <param name="sapCompany">The _receipt of the SAP Connection Object used by the wrapper.</param>
        /// <param name="docEntry">The primary key of the payment to be loaded.</param>
        public InventoryReceipt(Company sapCompany, int docEntry) : this(sapCompany)
        {

            if (docEntry > 0)
            {
                if (_receipt.GetByKey(docEntry) == false)
                    throw new B1Exception(sapCompany, $"Unable to retrieve Production Order {docEntry} from {sapCompany.CompanyName}");
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
        public InventoryReceipt(Company sapCompany, int baseEntry, int baseLine, string whCode, string batch, double qty)
        {
            _sapCompany = sapCompany;
            _receipt = (SAPbobsCOM.IDocuments)_sapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
            _receipt.Lines.BaseEntry = baseEntry;
            _receipt.Lines.BaseLine = baseLine;
            _receipt.Lines.BaseType = 202;
            _receipt.Lines.Quantity = qty;
        }

        public Dictionary<string, int> UserDefinedFieldsDictionary
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public UserFields UserDefinedFields
        {
            get
            {
                throw new NotImplementedException();
            }
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
            throw new NotImplementedException();
        }

        public void SetUDFValue(string key, dynamic value)
        {
            _receipt.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value = value;
        }

        public void SetUDFValue(int key, dynamic value)
        {
            throw new NotImplementedException();
        }

        public dynamic GetUDFValue(string key)
        {
            throw new NotImplementedException();
        }

        public dynamic GetUDFValue(int key)
        {
            throw new NotImplementedException();
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
