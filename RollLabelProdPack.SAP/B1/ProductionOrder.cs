using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace RollLabelProdPack.SAP.B1
{
    public class ProductionOrder : IDisposable, IB1Object
    {

        #region Variables

        /// <summary></summary>
        private ProductionOrders _prodOrder = null;
        /// <summary></summary>
        private BoObjectTypes _objectType = BoObjectTypes.oProductionOrders;
        /// <summary></summary>
        private List<ProductionOrderLine> _productionOrderLines = new List<ProductionOrderLine>();
        /// <summary></summary>
        private Dictionary<string, int> _userDefinedFieldsDictionary = new Dictionary<string, int>();
        /// <summary></summary>
        private bool _isNew = true;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the SAP Object Type used by the class.
        /// </summary>
        public BoObjectTypes SAPObjectType { get { return BoObjectTypes.oProductionOrders; } }

        /// <summary>
        /// Returns the identification key of the production order as assigned by SAP Business One when adding one.  Field name: DocEntry.  Length: 11 characters.
        /// </summary>
        public int DocEntry { get { return _prodOrder.AbsoluteEntry; } }

        /// <summary>
        /// Sets or returns the customer code, which is the card code in the business partner master data. Mandatory field in SAP Business One. Field name: CardCode. Length: 15 characters. This is a foreign key to the BusinessPartners object.
        /// </summary>
        public string CardCode { get { return _prodOrder.CustomerCode; } set { _prodOrder.CustomerCode = value; } }

        /// <summary>
        /// Sets or returns the planned quantity of the completed product.  Field name: PlannedQty.
        /// </summary>
        public double PlannedQuantity { get { return _prodOrder.PlannedQuantity; } set { _prodOrder.PlannedQuantity = value; } }

        /// <summary>
        /// Sets or returns the key of the product.  Mandatory for creating a new production order.  Field name: ItemCode. Length: 20 characters. This is a foreign key to the ProductTrees object. 
        /// </summary>
        public string ItemCode { get { return _prodOrder.ItemNo; } set { _prodOrder.ItemNo = value; } }

        /// <summary>
        /// Sets or returns the planned completion date of the production order. The DueDate can be modified (read-write) while the ProductionOrderStatus is either Planned or Released (read-only when the status is Closed).  Mandatory for creating a new production order. Field name: DueDate. 
        /// </summary>
        public DateTime DueDate { get { return _prodOrder.DueDate; } set { _prodOrder.DueDate = value; } }

        /// <summary>
        /// Sets or returns the order date of the product.  Field name: PostDate. 
        /// </summary>
        public DateTime PostingDate { get { return _prodOrder.PostingDate; } set { _prodOrder.PostingDate = value; } }

        /// <summary>
        /// Sets or returns the order start date of the product.  Field name: StartDate. 
        /// </summary>
        public DateTime StartDate { get { return _prodOrder.StartDate; } set { _prodOrder.StartDate = value; } }

        /// <summary>
        /// Sets or returns the status of the production order(Planned, Released, Closed, or Cancelled).  Field name: Sets or returns the status of the production order(Planned, Released, Closed, or Cancelled)..
        /// </summary>
        public BoProductionOrderStatusEnum ProductionOrderStatus { get { return _prodOrder.ProductionOrderStatus; } set { _prodOrder.ProductionOrderStatus = value; } }

        /// <summary>
        /// Sets or returns the origin type of the production order (Manual, MRP, or Sales Order).  Field name: OriginType.
        /// </summary>
        public BoProductionOrderOriginEnum ProductionOrderOrigin { get { return _prodOrder.ProductionOrderOrigin; } set { _prodOrder.ProductionOrderOrigin = value; } }

        /// <summary>
        /// Sets or returns the key number of sales order that is linked to the production order.  Field name: OriginAbs.
        /// </summary>
        public int ProductionOrderOriginEntry { get { return _prodOrder.ProductionOrderOriginEntry; } set { _prodOrder.ProductionOrderOriginEntry = value; } }


        /// <summary>
        /// List of line items associated with the given SAP B1 document object
        /// </summary>
        public List<ProductionOrderLine> ProductionOrderLines { get { return _productionOrderLines; } set { _productionOrderLines = value; } }

        /// <summary>
        /// Returns the root SAP B1 object's User Defined Fields object based on the SAP DI API UserFields interface.
        /// </summary>
        public UserFields UserDefinedFields { get { return _prodOrder.UserFields; } }

        /// <summary>
        /// Returns a dictionary of the User Defined Fields (UDF) used by the root SAP B1 object in the class.  The dictionary is based on the 
        /// string name of the UDF and returns the given UDF's index position as known by the UserDefinedFields property.
        /// </summary>
        public Dictionary<string, int> UserDefinedFieldsDictionary { get { return _userDefinedFieldsDictionary; } set { _userDefinedFieldsDictionary = value; } }

        /// <summary>
        /// Returns the underlying SAP B1 Item object.
        /// </summary>
        public ProductionOrders ProdOrder { get { return _prodOrder; } set { _prodOrder = value; } }

        #endregion

        #region constructors


        /// <summary>
        /// Creates an instance of the wrapper class for the SAP B1 Production Order Object (oItems)
        /// </summary>
        /// <param name="sapCompany">The instance of the SAP Connection Object used by the wrapper.</param>
        public ProductionOrder(Company sapCompany)
        {
            ProdOrder = sapCompany.GetBusinessObject(BoObjectTypes.oProductionOrders);
        }

        /// <summary>
        /// Creates an instance of the wrapper class for the SAP B1 Production Order Object (oItems)
        /// </summary>
        /// <param name="sapCompany">The instance of the SAP Connection Object used by the wrapper.</param>
        /// <param name="docEntry">The primary key of the payment to be loaded.</param>
        public ProductionOrder(Company sapCompany, int docEntry) : this(sapCompany)
        {

            if (docEntry > 0)
            {
                if (ProdOrder.GetByKey(docEntry) == false)
                    throw new B1Exception(sapCompany, $"Unable to retrieve Production Order {docEntry} from {sapCompany.CompanyName}");
                _isNew = false;

                // lines
                // -------------------------------------------------------------------------------------

                if (_prodOrder.Lines.Count > 0)
                    for (var i = 0; i < _prodOrder.Lines.Count; i++)
                    {
                        _prodOrder.Lines.SetCurrentLine(i);
                        ProductionOrderLine line = new ProductionOrderLine(_prodOrder.Lines);
                        _productionOrderLines.Add(line);
                    }
            }
        }

        #endregion

        #region UDFs

        /// <summary>
        /// Loads the UserDefinedFieldsDictionary variable based on the given SAP B1 object type
        /// </summary>
        public void LoadUDFDictionary()
        {
            for (int i = 0; i < ProdOrder.UserFields.Fields.Count; i++)
                _userDefinedFieldsDictionary.Add(ProdOrder.UserFields.Fields.Item(i).Name, i);
        }

        /// <summary>
        /// Sets the value of the User Defined Field
        /// </summary>
        /// <param name="key">The name of the User Defined Field as known by the database.</param>
        /// <param name="value">The new value of the UDF.  Please note, this accepts a dynamic value that requires checking before assignement.</param>
        public void SetUDFValue(string key, dynamic value)
        {
            ProdOrder.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value = value;
        }

        /// <summary>
        /// Sets the value of the User Defined Field
        /// </summary>
        /// <param name="key">The INDEX of the User Defined Field as known by the returning array.</param>
        /// <param name="value">The new value of the UDF.  Please note, this accepts a dynamic value that requires checking before assignement.</param>
        public void SetUDFValue(int key, dynamic value)
        {
            ProdOrder.UserFields.Fields.Item(key).Value = value;
        }

        /// <summary>
        /// Returns the value of a given user defined field.
        /// </summary>
        /// <param name="key">The name of the User Defined Field as known by the database.</param>
        /// <returns>Returns the value of the User Defined Field.  Based on the dynamic type, further casting may be required.</returns>
        public dynamic GetUDFValue(string key)
        {
            return ProdOrder.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value;
        }

        /// <summary>
        /// Returns the value of a given user defined field.
        /// </summary>
        /// <param name="key">The INDEX of the User Defined Field as known by the returning array.</param>
        /// <returns>Returns the value of the User Defined Field.  Based on the dynamic type, further casting may be required.</returns>
        public dynamic GetUDFValue(int key)
        {
            return ProdOrder.UserFields.Fields.Item(key).Value;
        }

        #endregion

        #region methods

        /// <summary>
        /// Adds a line to the SAP Document with base information included
        /// </summary>
        /// <param name="itemCode">The SAP Item Primary key to be used</param>
        public void AddLine(string itemCode, double quantity, string issueMethod, string warehouse, int opSeq, string medFile, double scrapPct, string wrkInstr, double qNeed)
        {
            _productionOrderLines.Add(new ProductionOrderLine(_prodOrder, itemCode, quantity, issueMethod, warehouse, opSeq, medFile, scrapPct, wrkInstr, qNeed));
        }


        /// <summary>
        /// Saves the instance of the B1 Object.
        /// </summary>
        /// <returns>A boolean flag that reports success or failure.  Please call the GetLastError method from SAPB1.</returns>
        public bool Save()
        {
            var returnCode = 0;
            if (_isNew)
            {
                returnCode = _prodOrder.Add();
                _isNew = (returnCode == 0);
            }
            else
                returnCode = _prodOrder.Update();
            return (returnCode == 0);
        }


        public string GetNewObjectCode(Company sapCompany)
        {
            string newObjectCode = null;
            sapCompany.GetNewObjectCode(out newObjectCode);
            return newObjectCode;
        }


        #endregion

        #region Dispose

        /// <summary>
        /// Releases the associated B1 COM Object, sets it's instance to null, and calls for garbage collection.
        /// </summary>
        public void Dispose()
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ProdOrder);
            ProdOrder = null;
            GC.Collect();
        }

        #endregion


    }
}
