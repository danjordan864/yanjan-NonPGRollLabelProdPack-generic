using SAPbobsCOM;
using System;
using System.Collections.Generic;
using log4net;
using RollLabelProdPack.Library.Data;

namespace RollLabelProdPack.SAP.B1.DocumentObjects
{
    /// <summary>
    /// Represents an Inventory Issue document in SAP Business One.
    /// </summary>
    public class InventoryIssue : IDisposable, IB1Object
    {

        #region Variables
        private BoObjectTypes _objectType = BoObjectTypes.oInventoryGenExit;
        private Company _sapCompany;
        private ILog _log;
        private Dictionary<int, string> _issueItems;
        private int _currentIssueLine = 0;

        /// <summary></summary>
        private Dictionary<string, int> _userDefinedFieldsDictionary = new Dictionary<string, int>();

        private IDocuments _issue = null;
        private List<InventoryIssueLine> _issueLines = new List<InventoryIssueLine>();
        /// <summary></summary>
        //private bool _isNew = true;

        /// <summary>
        /// Initializes a new instance of the InventoryIssue class with the specified SAP Company.
        /// </summary>
        /// <param name="sapCompany">The SAP Company object used for SAP Business One integration.</param>
        /// <remarks>
        /// The constructor initializes a new instance of the InventoryIssue class by assigning the provided SAP 
        /// Company object to the _sapCompany field. It also retrieves the SAP Business One inventory issue object 
        /// using the SAP DI API and assigns it to the _issue field.
        /// </remarks>
        public InventoryIssue(Company sapCompany)
        {
            _log = LogManager.GetLogger(this.GetType());
            _issueItems = new Dictionary<int, string>();
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
        /// Initializes a new instance of the InventoryIssue class with the specified SAP Company and document entry.
        /// </summary>
        /// <param name="sapCompany">The SAP Company object used for SAP Business One integration.</param>
        /// <param name="docEntry">The document entry of the inventory issue to be loaded.</param>
        /// <remarks>
        /// The constructor initializes a new instance of the InventoryIssue class with the provided SAP Company 
        /// object and document entry. If the docEntry parameter is greater than 0, it retrieves the inventory 
        /// issue from SAP Business One using the document entry. It also initializes the _isNew field accordingly. 
        /// Additionally, it retrieves and adds the line items associated with the inventory issue to the _issueLines 
        /// list.
        /// </remarks>
        public InventoryIssue(Company sapCompany, int docEntry) : this(sapCompany)
        {

            if (docEntry > 0)
            {
                if (_issue.GetByKey(docEntry) == false)
                    throw new B1Exception(sapCompany, $"Unable to retrieve Issue {docEntry} from {sapCompany.CompanyName}");
                //_isNew = false;

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

        /// <summary>
        /// Adds a line item to the inventory issue for an order.
        /// </summary>
        /// <param name="baseEntry">The base entry of the document from which the item is issued.</param>
        /// <param name="baseLine">The base line number of the item in the document from which it is issued.</param>
        /// <param name="itemCode">The item code of the issued item.</param>
        /// <param name="quantity">The quantity of the item to be issued.</param>
        /// <param name="storageLoc">The storage location of the issued item.</param>
        /// <param name="qualityStatus">The quality status of the issued item.</param>
        /// <param name="batchNo">The batch number of the issued item.</param>
        /// <param name="luid">The logistical unit ID of the line item in the source document.</param>
        /// <param name="sscc">The SSCC (Serial Shipping Container Code) of the issued item.</param>
        /// <param name="uom">The unit of measure of the issued item.</param>
        /// <param name="lotNumber">The lot number of the issued item.</param>
        public void AddOrderIssueLine(int baseEntry, int baseLine, string itemCode, double quantity, string storageLoc, string qualityStatus, string batchNo, int luid, string sscc, string uom, string lotNumber)
        {
            _issueItems.Add(_currentIssueLine++, itemCode);
            _issueLines.Add(new InventoryIssueLine(_issue, baseEntry,baseLine,itemCode,quantity,storageLoc,qualityStatus,batchNo,luid,sscc,uom,lotNumber));
        }

        /// <summary>
        /// Adds a line item to the inventory issue for scrap.
        /// </summary>
        /// <param name="itemCode">The item code of the scrapped item.</param>
        /// <param name="quantity">The quantity of the item to be scrapped.</param>
        /// <param name="storageLoc">The storage location of the scrapped item.</param>
        /// <param name="qualityStatus">The quality status of the scrapped item.</param>
        /// <param name="batchNo">The batch number of the scrapped item.</param>
        /// <param name="luid">The unique ID of the line item in the source document.</param>
        /// <param name="sscc">The SSCC (Serial Shipping Container Code) of the scrapped item.</param>
        /// <param name="uom">The unit of measure of the scrapped item.</param>
        /// <param name="lotNumber">The lot number of the scrapped item.</param>
        /// <param name="scrapOffsetCode">The offset code associated with the scrap issue.</param>
        /// <param name="scrapReason">The reason for the scrap issue.</param>
        /// <param name="shift">The shift associated with the scrap issue.</param>
        public void AddScrapIssueLine(string itemCode, double quantity, string storageLoc, string qualityStatus, string batchNo, int luid, string sscc, string uom, string lotNumber, string scrapOffsetCode,string scrapReason,string shift)
        {
            _currentIssueLine++;
            _issueLines.Add(new InventoryIssueLine(_issue, itemCode, quantity, storageLoc, qualityStatus, batchNo, luid, sscc, uom, lotNumber,scrapOffsetCode,scrapReason,shift));
        }

        /// <summary>
        /// Gets the SAP Business One object type of the inventory issue.
        /// </summary>
        /// <remarks>
        /// The SAPObjectType property retrieves the SAP Business One object type associated with the inventory issue. 
        /// It returns a value of type BoObjectTypes, which represents the specific object type (in this case, 
        /// oInventoryGenExit).
        /// </remarks>
        public BoObjectTypes SAPObjectType
        {
            get
            {
                return _objectType;
            }
        }

        /// <summary>
        /// Releases the resources used by the inventory issue object.
        /// </summary>
        /// <remarks>
        /// The Dispose method is responsible for releasing the resources used by the inventory issue object. 
        /// It releases the COM object _issue, sets it to null, and performs garbage collection. This method 
        /// should be called when you're finished working with the inventory issue object to free up system 
        /// resources.
        /// </remarks>
        public void Dispose()
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_issue);
            _issue = null;
            GC.Collect();
        }

        /// <summary>
        /// Loads the User Defined Fields (UDF) dictionary for the inventory issue object.
        /// </summary>
        /// <remarks>
        /// The LoadUDFDictionary method is used to populate the User Defined Fields (UDF) dictionary for the 
        /// inventory issue object. It iterates over the UDF fields of the _issue object and adds them to the 
        /// _userDefinedFieldsDictionary, where the key is the UDF name and the value is the index position of 
        /// the UDF as known by the UserFields property. This dictionary can be used to conveniently access and 
        /// set UDF values for the inventory issue object.
        /// </remarks>
        public void LoadUDFDictionary()
        {
            for (int i = 0; i < _issue.UserFields.Fields.Count; i++)
                _userDefinedFieldsDictionary.Add(_issue.UserFields.Fields.Item(i).Name, i);
        }

        /// <summary>
        /// Sets the value of a User Defined Field (UDF) for the inventory issue object.
        /// </summary>
        /// <param name="key">The name of the UDF.</param>
        /// <param name="value">The value to be set for the UDF.</param>
        /// <remarks>
        /// The SetUDFValue method is used to set the value of a User Defined Field (UDF) for the inventory issue 
        /// object. It takes two parameters: key, which is the name of the UDF, and value, which is the value to be 
        /// set for the UDF. The method accesses the UDF field using the key and assigns the provided value to it. 
        /// This allows you to conveniently set UDF values for the inventory issue object using the UDF name as the 
        /// key.
        /// </remarks>
        public void SetUDFValue(string key, dynamic value)
        {
            _issue.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value = value;
        }

        /// <summary>
        /// Sets the value of a User Defined Field (UDF) for the inventory issue object.
        /// </summary>
        /// <param name="key">The index position of the UDF.</param>
        /// <param name="value">The value to be set for the UDF.</param>
        /// <remarks>
        /// The SetUDFValue method is used to set the value of a User Defined Field (UDF) for the inventory issue 
        /// object. It takes two parameters: key, which is the index position of the UDF, and value, which is the 
        /// value to be set for the UDF. The method accesses the UDF field using the provided index and assigns the 
        /// value to it. This allows you to set UDF values for the inventory issue object using the UDF index 
        /// position as the key.
        /// </remarks>
        public void SetUDFValue(int key, dynamic value)
        {
            _issue.UserFields.Fields.Item(key).Value = value;
        }

        /// <summary>
        /// Gets the value of a User Defined Field (UDF) for the inventory issue object.
        /// </summary>
        /// <param name="key">The name of the UDF.</param>
        /// <returns>The value of the UDF.</returns>
        /// <remarks>
        /// The GetUDFValue method is used to retrieve the value of a User Defined Field (UDF) for the inventory 
        /// issue object. It takes a key parameter, which is the name of the UDF. The method accesses the UDF 
        /// field using the provided key (name) and returns its value. This allows you to retrieve the value of a 
        /// UDF for the inventory issue object based on its name. The return type is dynamic, indicating that the 
        /// method can return values of different types depending on the UDF's data type.
        /// </remarks>
        public dynamic GetUDFValue(string key)
        {
            return _issue.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value;
        }

        /// <summary>
        /// Gets the value of a User Defined Field (UDF) for the inventory issue object.
        /// </summary>
        /// <param name="key">The index of the UDF.</param>
        /// <returns>The value of the UDF.</returns>
        /// <remarks>
        /// The GetUDFValue method is used to retrieve the value of a User Defined Field (UDF) for the inventory 
        /// issue object. It takes an int key parameter, which represents the index of the UDF. The method accesses 
        /// the UDF field using the provided key (index) and returns its value. This allows you to retrieve the 
        /// value of a UDF for the inventory issue object based on its index. The return type is dynamic, indicating 
        /// that the method can return values of different types depending on the UDF's data type.
        /// </remarks>
        public dynamic GetUDFValue(int key)
        {
            return _issue.UserFields.Fields.Item(key).Value;
        }

        /// <summary>
        /// Saves the inventory issue object to the database.
        /// </summary>
        /// <returns>True if the save operation is successful, otherwise False.</returns>
        /// <remarks>
        /// The Save method is responsible for saving the inventory issue object to the database. It returns a bool 
        /// value indicating whether the save operation was successful or not.
        /// 
        /// Inside the method, it checks if the DocNum property of the _issue object is zero. If it is zero, it means 
        /// that the inventory issue object is new and needs to be added to the database using the Add method.
        /// Otherwise, if DocNum is not zero, it means the inventory issue object already exists in the database and 
        /// needs to be updated using the Update method.
        /// 
        /// If the return code of the add/update operation is not zero, indicating an error occurred, the method 
        /// retrieves the error code and error message using the _sapCompany.GetLastError method. It then throws an 
        /// exception with the error information.
        /// 
        /// Finally, if the save operation is successful, the method returns true.
        /// </remarks>
        public bool Save()
        {
            int returnCode;

            if (_log.IsDebugEnabled)
            {
                var luidIndex = InventoryIssueLine.UDFIndexLocation(_issue.Lines, "U_PMX_LUID");
                _log.Debug($"luidIndex = {luidIndex}");
                _log.Debug($"Issue lines:\n");
                _log.Debug(string.Format("{0,5} {1,-10} {2,8} {3,20} {4,9} {5,-12} {6,8}",
                    "Line", "Item", "Quantity", "Has 2nd Batch Number", "UDF Index", "Batch Number", "LUID"));
                _log.Debug(string.Format("{0,5} {1,-10} {2,8} {3,-20} {4,9} {5,-12} {6,8}",
                    new String('-', 5), new String('-', 10), new String('-', 8), new String('-', 20), new String('-', 9),
                    new String('-', 12), new String('-',8)));
                foreach (var lineNumber in _issueItems.Keys)
                {
                    var itemCode = _issueItems[lineNumber];
                    var so = AppData.GetItem(itemCode);
                    if (so.SuccessFlag)
                    {
                        var hasBatchNumber2 = ((RollLabelProdPack.Library.Entities.Item)so.ReturnValue).HasSecondBatchNumber;
                        _issue.Lines.SetCurrentLine(lineNumber);
                        var udfIndex = InventoryIssueLine.UDFIndexLocation(_issue.Lines, "U_PMX_BATC");
                        _log.Debug(string.Format("  {0,5} {1,-10} {2,8} {3,-20} {4,9} {5,-12} {6,8}", lineNumber, itemCode, _issue.Lines.Quantity, hasBatchNumber2, udfIndex,
                            _issue.Lines.UserFields.Fields.Item(udfIndex).Value, _issue.Lines.UserFields.Fields.Item(luidIndex).Value));
                    }
                }
            }

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

