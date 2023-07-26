using System;
using System.Collections.Generic;
using SAPbobsCOM;
using RollLabelProdPack.SAP.B1.DocumentObjects;

namespace RollLabelProdPack.SAP.B1
{

    /// <summary>
    /// SAP Exception object.  The object will return the error that caused the exception along with an instance of the SAP Compnay object.
    /// </summary>
    public class B1Exception : Exception
    {

        #region variables

        /// <summary></summary>
        private string _exceptionMessage = null;
        /// <summary></summary>
        private Company _sapCompany = null;
        /// <summary></summary>
        private Exception _innerException = null;

        #endregion

        #region properties

        /// <summary>
        /// Error message thrown by the calling application.
        /// </summary>
        public string ExceptionMessage { get { return _exceptionMessage; } set { _exceptionMessage = value; } }

        /// <summary>
        /// The SAP Company (connection) object associated with the exception.
        /// </summary>
        public Company SAPB1Company { get { return _sapCompany; } set { _sapCompany = value; } }

        /// <summary>
        /// Possible inter exception thrown by the calling program.  Please note, there may be several nested messages.
        /// </summary>
        public Exception InnerException1 { get { return _innerException; } set { _innerException = value; } }
        #endregion

        #region constructors

        /// <summary>
        /// Creates an instance of the B1 Exception Object.
        /// </summary>
        /// <param name="sapCompany">The SAP Company (connection) object associated with the exception.</param>
        /// <param name="message">Error message thrown by the calling application.</param>
        public B1Exception(Company sapCompany, string message) : this(sapCompany, message, null) { }

        /// <summary>
        /// Creates an instance of the B1 Exception Object.
        /// </summary>
        /// <param name="sapCompany">The SAP Company (connection) object associated with the exception.</param>
        /// <param name="innerException">Possible inter exception thrown by the calling program.  Please note, there may be several nested messages.</param>
        public B1Exception(Company sapCompany, Exception innerException) : this(sapCompany, innerException.Message, innerException) { }

        /// <summary>
        /// Creates an instance of the B1 Exception Object.
        /// </summary>
        /// <param name="sapCompany">The SAP Company (connection) object associated with the exception.</param>
        /// <param name="message">Error message thrown by the calling application.</param>
        /// <param name="innerException">Possible inter exception thrown by the calling program.  Please note, there may be several nested messages.</param>
        public B1Exception(Company sapCompany, string message, Exception innerException) : base(message)
        {
            _sapCompany = sapCompany;
            _exceptionMessage = message;
            _innerException = innerException;
        }
        #endregion

        #region methods

        /// <summary>
        /// Returns the exception as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("SAP B1 Exception: {0}", _exceptionMessage);
        }
        #endregion

    }




    /// <summary>
    /// The SAPB1 is the master wrapping class for use with the SAP B1 DI API.  The class holds the instance of an SAP connection (SAP Company Object) and can
    /// be used to generate .Net wrapper classes for the SAP B1 COM objects.
    /// </summary>
    public class SAPB1 : IDisposable
    {

        #region enum

        /// <summary>
        /// Enum of the types of databases used by SAP B1.
        /// </summary>
        public enum SAPDatabaseTypes
        {
            /// <summary>SQL Server 2005</summary>
            SQL2005,
            /// <summary>SQL Server 2008</summary>
            SQL2008,
            /// <summary>SQL Server 2012</summary>
            SQL2012,
            /// <summary>SAP Properitary database</summary>
            Hana
        };

        #endregion

        #region variables

        /// <summary></summary>
        private Company _sapCompany = null;
        /// <summary></summary>
        private Dictionary<BoObjectTypes, Dictionary<string, int>> _udfMaps = new Dictionary<BoObjectTypes, Dictionary<string, int>>();
        /// <summary></summary>
        private bool _useTransactions = false;

        #endregion

        #region properties

        /// <summary>
        /// The instance of the SAP Connection Object used by the wrapper.
        /// </summary>
        public Company SapCompany { get { return _sapCompany; } }

        /// <summary>
        /// Flag used to determine if database actions should be wrapped in a transaction statement.
        /// </summary>
        public bool UseTransactions { get { return _useTransactions; } set { _useTransactions = value; } }

        #endregion

        #region constructors

        /// <summary>
        /// Creates an instance of the SAPB1 wrapper class.  The class with connect to the SAP database after creation.
        /// </summary>
        /// <param name="serverName">The name of the server used by SAP B1</param>
        /// <param name="initialCatalog">Name of the database used by the given instance of SAP B1</param>
        /// <param name="user">The SAP B1 user name as entered on the login form.</param>
        /// <param name="password">The SAP B1 user password as entered on the login form.</param>
        /// <param name="dbVersion">The type of database used by SAP B1</param>
        /// <param name="licenseServer">The name of the license server used by SAP B1</param>
        public SAPB1(string serverName, string initialCatalog, string user, string password, string dbVersion, string licenseServer)
        {
            _sapCompany = new Company();
            _sapCompany.Server = serverName;
            _sapCompany.CompanyDB = initialCatalog;
            _sapCompany.UserName = user;
            _sapCompany.Password = password;
            _sapCompany.LicenseServer = licenseServer;
            _sapCompany.language = BoSuppLangs.ln_English;
            _sapCompany.DbServerType = (BoDataServerTypes)Enum.Parse(typeof(SAPbobsCOM.BoDataServerTypes), dbVersion);
            var returnCode = _sapCompany.Connect();

            if (returnCode != 0)
            {
                BuildSAPB1Error("Creating Company Connection");
            }
        }

        /// <summary>
        /// Creates an instance of the SAPB1 wrapper class.  The class with connect to the SAP database after creation.
        /// </summary>
        /// <remarks>
        /// This constructor uses the values in the cnxInfo object as generated by the SAPCnxInfo default constructor.
        /// </remarks>
        public SAPB1()
        {
            SAPCnxInfo cnxInfo = new SAPCnxInfo();
            _sapCompany = new Company();
            _sapCompany.Server = cnxInfo.ServerName;
            _sapCompany.CompanyDB = cnxInfo.CatalogName;
            _sapCompany.UserName = cnxInfo.SAPUserName;
            _sapCompany.Password = cnxInfo.SAPUserPassword;
            _sapCompany.LicenseServer = cnxInfo.LicenseServer;
            _sapCompany.language = BoSuppLangs.ln_English;
            _sapCompany.DbServerType = (BoDataServerTypes)Enum.Parse(typeof(SAPbobsCOM.BoDataServerTypes), cnxInfo.DatabaseVersion);
            var returnCode = _sapCompany.Connect();

            if (returnCode != 0)
            {
                BuildSAPB1Error("Creating Company Connection");
            }
        }

        /// <summary>
        /// Creates an instance of the SAPB1 wrapper class.  This constructor assumes the company is already connected.
        /// </summary>
        public SAPB1(Company company)
        {
            _sapCompany = company;
        }

        /// <summary>
        /// Creates an instance of the SAPB1 wrapper class.  The class with connect to the SAP database after creation.
        /// </summary>
        /// <param name="user">The SAP B1 user name as entered on the login form.</param>
        /// <param name="password">The SAP B1 user password as entered on the login form.</param>
        /// <remarks>
        /// This constructor uses an SAPCnxInfo object created with the default constructor to get
        /// the server name, catalog name, license server, and DB server type.
        /// </remarks>
        public SAPB1(string userName, string password)
        {
            SAPCnxInfo cnxInfo = new SAPCnxInfo();
            _sapCompany = new Company();
            _sapCompany.Server = cnxInfo.ServerName;
            _sapCompany.CompanyDB = cnxInfo.CatalogName;
            _sapCompany.UserName = userName;
            _sapCompany.Password = password;
            _sapCompany.LicenseServer = cnxInfo.LicenseServer;
            _sapCompany.language = BoSuppLangs.ln_English;
            _sapCompany.DbServerType = (BoDataServerTypes)Enum.Parse(typeof(SAPbobsCOM.BoDataServerTypes), cnxInfo.DatabaseVersion);
            var returnCode = _sapCompany.Connect();

            if (returnCode != 0)
            {
                BuildSAPB1Error("Creating Company Connection");
            }
        }
        #endregion

        #region dispose methods

        /// <summary>
        /// Ends the SAP connection and sets the instance to null.
        /// </summary>
        public void Dispose()
        {
            if (_sapCompany != null)
            {
                if (_sapCompany.Connected)
                {
                    _sapCompany.Disconnect();
                    _sapCompany = null;
                }
            }
        }

        #endregion

        #region Transaction Methods

        /// <summary>
        /// Starts an instance of the SAP B1 DI API Transaction set.
        /// </summary>
        public void StartTransaction()
        {
            if (_useTransactions)
                if (!_sapCompany.InTransaction)
                    _sapCompany.StartTransaction();

        }

        /// <summary>
        /// Commits an instance of the SAP B1 DI API Transaction set.
        /// </summary>
        public void CommitTransaction()
        {
            if (_useTransactions)
                if (_sapCompany.InTransaction)
                    _sapCompany.EndTransaction(BoWfTransOpt.wf_Commit);
        }

        /// <summary>
        /// Rolls back an instance of the SAP B1 DI API Transaction set.
        /// </summary>
        public void RollbackTransaction()
        {
            if (_useTransactions)
                if (_sapCompany.InTransaction)
                    _sapCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
        }

        #endregion

        #region error methods

        /// <summary>
        /// Builds the SAP B1 exception error message based on the last known error from the DI API.
        /// </summary>
        /// <param name="callingProcedure">The name of the calling procedure or method.</param>
        /// <remarks>
        /// This method retrieves the error code and error message from the SAP B1 DI API using the <c>GetLastError</c> method of the SAP Company object.
        /// It constructs the exception message by combining the error code, error message, and information about the SAP Company connection.
        /// The resulting exception is thrown as a <c>B1Exception</c> with the SAP Company object and the constructed message.
        /// </remarks>
        private void BuildSAPB1Error(string callingProcedure)
        {
            var errorMessage = GetLastExceptionMessage();
            var message = string.Format("{0} in {1} - {2}\n[{3}]", callingProcedure, _sapCompany.Server, _sapCompany.CompanyDB, errorMessage);
            throw new B1Exception(_sapCompany, message);
        }

        /// <summary>
        /// Builds the exception message from SAP for failed DI API actions.
        /// </summary>
        /// <returns>Returns the last known error message from SAP.</returns>
        public string GetLastExceptionMessage()
        {
            var errorCode = 0;
            var errorMessage = string.Empty;
            _sapCompany.GetLastError(out errorCode, out errorMessage);
            return string.Format("[{0}] {1}", errorCode, errorMessage);
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Creates an instance of a wrapper class for a given SAP B1 Object.  The factory preloads the User Defined Fields dictionary used
        /// by the underlining SAP Object.
        /// </summary>
        /// <param name="b1Object">The type of SAP Object to be wrapped.</param>
        /// <param name="keyValue">The primary key of a SAP Object to be created.  A null value may be passed to create a new instance.</param>
        /// <returns></returns>
        public IB1Object B1Factory(BoObjectTypes b1Object, object keyValue)
        {
            IB1Object returnObject = null;
            if (keyValue != null)
                if (!(keyValue is int))
                    throw new InvalidCastException($"{b1Object} [a document object] must use a key of type int.");
            if (keyValue == null)
                keyValue = 0;
            switch (b1Object)
            {
                case BoObjectTypes.oProductionOrders:
                    if (keyValue != null)
                        if (!(keyValue is int))
                            throw new InvalidCastException($"{b1Object} [a document object] must use a key of type int.");
                    if (keyValue == null)
                        keyValue = 0;
                    returnObject = new ProductionOrder(_sapCompany, (int)keyValue);
                    break;
                case BoObjectTypes.oInventoryGenExit: //Issue
                    if (keyValue != null)
                        if (!(keyValue is int))
                            throw new InvalidCastException($"{b1Object} [a document object] must use a key of type int.");
                    if (keyValue == null)
                        keyValue = 0;
                    returnObject = new InventoryIssue(_sapCompany, (int)keyValue);
                    break;
                case BoObjectTypes.oInventoryGenEntry: //Receipt
                    if (keyValue != null)
                        if (!(keyValue is int))
                            throw new InvalidCastException($"{b1Object} [a document object] must use a key of type int.");
                    if (keyValue == null)
                        keyValue = 0;
                    returnObject = new InventoryReceipt(_sapCompany, (int)keyValue);
                    break;
                default:
                    returnObject = null;
                    break;
            }
            if (returnObject == null)
                throw new ArgumentException($"B1 Object type of {b1Object} does not have a factory match.");

            returnObject.UserDefinedFieldsDictionary = LocateUDFMap(returnObject.SAPObjectType, returnObject.UserDefinedFields);
            return returnObject;
        }

        /// <summary>
        /// Maps the SAP B1 Object's User Defined Fields into a dictionary.
        /// </summary>
        /// <param name="b1Object">The type of SAP Object to be wrapped.</param>
        /// <param name="userFields">The calling SAP Object's userFields Object</param>
        /// <returns></returns>
        public Dictionary<string, int> LocateUDFMap(BoObjectTypes b1Object, UserFields userFields)
        {
            Dictionary<string, int> udfMap = new Dictionary<string, int>();
            if (_udfMaps.ContainsKey(b1Object))
                udfMap = _udfMaps[b1Object];
            else
            {
                if (userFields != null)
                {
                    for (int i = 0; i < userFields.Fields.Count; i++)
                    {
                        udfMap.Add(userFields.Fields.Item(i).Name, i);
                    }
                    _udfMaps.Add(b1Object, udfMap);
                }
            }
            return udfMap;
        }

        #endregion


    }


}

