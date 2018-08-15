using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollLabelProdPack.SAP.B1.DocumentObjects
{
    public class InventoryIssue : IDisposable, IB1Object
    {
        #region Variables
        private BoObjectTypes _objectType = BoObjectTypes.oInventoryGenExit;
        private Company _sapCompany;
        /// <summary></summary>
        private Dictionary<string, int> _userDefinedFieldsDictionary = new Dictionary<string, int>();

        private IDocuments _issue = null;
        private List<InventoryIssueLine> _issueLines = new List<InventoryIssueLine>();
        /// <summary></summary>
        private bool _isNew = true;

        public InventoryIssue(Company sapCompany)
        {
            _sapCompany = sapCompany;
            _issue = (SAPbobsCOM.IDocuments)_sapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
        }
        #endregion Variables

        #region Properties
        /// <summary>
        /// Returns the root SAP B1 object's User Defined Fields object based on the SAP DI API UserFields interface.
        /// </summary>
        public UserFields UserDefinedFields { get { return _issue.UserFields; } }

        /// <summary>
        /// Returns a dictionary of the User Defined Fields (UDF) used by the root SAP B1 object in the class.  The dictionary is based on the 
        /// string name of the UDF and returns the given UDF's index position as known by the UserDefinedFields property.
        /// </summary>
        public Dictionary<string, int> UserDefinedFieldsDictionary { get { return _userDefinedFieldsDictionary; } set { _userDefinedFieldsDictionary = value; } }

        /// <summary>
        /// Returns the underlying SAP B1 Item object.
        /// </summary>
        public IDocuments Issue { get { return _issue; } set { _issue = value; } }

        /// <summary>
        /// List of line items associated with the given SAP B1 document object
        /// </summary>
        public List<InventoryIssueLine> IssueLines { get { return _issueLines; } set { _issueLines = value; } }
        #endregion Properties
        /// <summary>
        /// Creates an _issue of the wrapper class for the SAP B1 Production Order Object (oItems)
        /// </summary>
        /// <param name="sapCompany">The _issue of the SAP Connection Object used by the wrapper.</param>
        /// <param name="docEntry">The primary key of the payment to be loaded.</param>
        public InventoryIssue(Company sapCompany, int docEntry) : this(sapCompany)
        {

            if (docEntry > 0)
            {
                if (_issue.GetByKey(docEntry) == false)
                    throw new B1Exception(sapCompany, $"Unable to retrieve Issue {docEntry} from {sapCompany.CompanyName}");
                _isNew = false;

                // lines
                // -------------------------------------------------------------------------------------

                if (_issue.Lines.Count > 0)
                    for (var i = 0; i < _issue.Lines.Count; i++)
                    {
                        _issue.Lines.SetCurrentLine(i);
                        InventoryIssueLine line = new InventoryIssueLine(_issue.Lines);
                        _issueLines.Add(line);
                    }
            }
        }

        public void AddLine(int baseEntry, int baseLine, string itemCode, double quantity, string storageLoc, string qualityStatus, string batchNo, int luid, string sscc, string uom, string lotNumber)
        {
            _issueLines.Add(new InventoryIssueLine(_issue, baseEntry,baseLine,itemCode,quantity,storageLoc,qualityStatus,batchNo,luid,sscc,uom,lotNumber));
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
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_issue);
            _issue = null;
            GC.Collect();
        }

        public void LoadUDFDictionary()
        {
            for (int i = 0; i < _issue.UserFields.Fields.Count; i++)
                _userDefinedFieldsDictionary.Add(_issue.UserFields.Fields.Item(i).Name, i);
        }

        public void SetUDFValue(string key, dynamic value)
        {
            _issue.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value = value;
        }

        public void SetUDFValue(int key, dynamic value)
        {
            _issue.UserFields.Fields.Item(key).Value = value;
        }

        public dynamic GetUDFValue(string key)
        {
            return _issue.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value;
        }

        public dynamic GetUDFValue(int key)
        {
            return _issue.UserFields.Fields.Item(key).Value;
        }

        public bool Save()
        {
            int returnCode;
            if (_issue.DocNum == 0)
            {
                returnCode = _issue.Add();

            }
            else
            {
                returnCode = _issue.Update();
            }
            if (returnCode != 0)
            {
                int errorCode = 0;
                string errorMessage = string.Empty;

                _sapCompany.GetLastError(out errorCode, out errorMessage);

                throw new Exception(string.Format("(APIs.InventoryIssue)ERROR: {0}-{1}", errorCode, errorMessage));
            }
            return true;
        }

    }
}

