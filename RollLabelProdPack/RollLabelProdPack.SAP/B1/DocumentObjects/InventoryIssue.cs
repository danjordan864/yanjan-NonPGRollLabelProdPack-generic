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
                    throw new B1Exception(sapCompany, $"Unable to retrieve Production Order {docEntry} from {sapCompany.CompanyName}");
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
        public InventoryIssue(Company sapCompany, int baseEntry, int baseLine, string whCode, string batch, double qty)
        {
            _sapCompany = sapCompany;
            _issue = (SAPbobsCOM.IDocuments)_sapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
            _issue.Lines.BaseEntry = baseEntry;
            _issue.Lines.BaseLine = baseLine;
            _issue.Lines.BaseType = 202;
            _issue.Lines.Quantity = qty;
        }
        //public void IssueConvertFrom(Entities.StockTransfer convertToScrap, string offsetAcctCode)
        //{
        //    try
        //    {
        //        _issue = (SAPbobsCOM.IDocuments)_sapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

        //        _issue.DocDate = convertToScrap.DateCreated;
        //        _issue.Comments = "Converted To Scrap Item: " + convertToScrap.ConvertToItem + " Batch: " + convertToScrap.ConvertToBatch;
        //        _issue.Lines.SetCurrentLine(0);
        //        _issue.Lines.ItemCode = convertToScrap.ItemCode;
        //        _issue.Lines.WarehouseCode = convertToScrap.FromWhsCode;
        //        _issue.Lines.AccountCode = offsetAcctCode;
        //        _issue.Lines.Quantity = Convert.ToDouble(convertToScrap.TransferQty);

        //        _issue.Lines.BatchNumbers.BatchNumber = convertToScrap.Batch;
        //        _issue.Lines.BatchNumbers.Quantity = Convert.ToDouble(convertToScrap.TransferQty);
        //        _issue.Lines.BatchNumbers.UserFields.Fields.Item("U_SII_Lot").Value = convertToScrap.Lot == null ? "" : convertToScrap.Lot;
        //        _issue.Lines.BinAllocations.SetCurrentLine(0);
        //        _issue.Lines.BinAllocations.BinAbsEntry = convertToScrap.FromBinAbs;
        //        _issue.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
        //        _issue.Lines.BinAllocations.Quantity = Convert.ToDouble(convertToScrap.TransferQty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(string.Format("(APIs.InventoryTransfer)ERROR: {0}", ex.Message));
        //    }
        //}

        //public void WriteOff(Batch batch, string offsetAcctCode)
        //{
        //    try
        //    {
        //        _issue = (SAPbobsCOM.IDocuments)_sapCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

        //        _issue.DocDate = DateTime.Now;
        //        _issue.Comments = "Write off quantity under .001: " + batch.ItemCode + " Batch: " + batch.BatchNo + " OffsetAcctCode: " + offsetAcctCode;
        //        _issue.Lines.SetCurrentLine(0);
        //        _issue.Lines.ItemCode = batch.ItemCode;
        //        _issue.Lines.WarehouseCode = batch.WhsCode;
        //        _issue.Lines.AccountCode = offsetAcctCode;
        //        _issue.Lines.Quantity = Convert.ToDouble(batch.Qty);

        //        _issue.Lines.BatchNumbers.BatchNumber = batch.BatchNo;
        //        _issue.Lines.BatchNumbers.Quantity = Convert.ToDouble(batch.Qty);
        //        _issue.Lines.BatchNumbers.UserFields.Fields.Item("U_SII_Lot").Value = batch.Lot == null ? "" : batch.Lot;
        //        _issue.Lines.BinAllocations.SetCurrentLine(0);
        //        _issue.Lines.BinAllocations.BinAbsEntry = batch.BinAbs;
        //        _issue.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
        //        _issue.Lines.BinAllocations.Quantity = Convert.ToDouble(batch.Qty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(string.Format("(APIs.WriteOff)ERROR: {0}", ex.Message));
        //    }
        //}
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
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_issue);
            _issue = null;
            GC.Collect();
        }

        public void LoadUDFDictionary()
        {
            throw new NotImplementedException();
        }

        public void SetUDFValue(string key, dynamic value)
        {
            _issue.UserFields.Fields.Item(_userDefinedFieldsDictionary[key]).Value = value;
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

