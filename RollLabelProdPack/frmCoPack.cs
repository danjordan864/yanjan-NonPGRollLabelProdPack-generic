using log4net;
using RollLabelProdPack.Library.Data;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;
using RollLabelProdPack.SAP.B1;
using RollLabelProdPack.SAP.B1.DocumentObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinFormUtils;

namespace RollLabelProdPack
{
    /// <summary>
    /// Represents a form for co-pack production
    /// </summary>
    public partial class frmCoPack : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        private string _currentEmployee;
        private string _currentShift;
        private string _currentScrapItem;
        private string _currentYJNOrder;
        private int _currentScrapLine;
        private bool _orderChangeCausedComboSelectionChange = false;
        private RollLabelData _selectOrder;
        private List<ProductionLineMachineNo> _prodLines;
        private ProductionLineMachineNo _selectedLine;
        private List<InventoryIssueDetail> _plannedIssue;
        private int luid;
        private string sscc;
        private int _prodRun;
        private ILog _log;
        private string _lotNumber = string.Empty;

        /// <summary>
        /// Initializes a new instance of the frmCoPack class.
        /// </summary>
        public frmCoPack()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the "btnSelect" button click.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }

        /// <summary>
        /// Changes the selected order based on user input.
        /// </summary>
        private void ChangeOrder()
        {
            try
            {
                using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
                {
                    frmSignInDialog.SetDataSource("COPACK");
                    DialogResult dr = frmSignInDialog.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        _selectOrder = frmSignInDialog.SelectOrder;
                        if (_log.IsDebugEnabled)
                        {
                            _log.Debug("In ChangeOrder, using selected order from frmSignInDialog");
                            _log.Debug(_selectOrder);
                        }
                        _currentShift = _selectOrder.Shift;
                        _currentEmployee = _selectOrder.Employee;
                        _currentScrapItem = _selectOrder.ScrapItem;
                        _currentYJNOrder = _selectOrder.YJNOrder;
                        _currentScrapLine = _selectOrder.ScrapLine;

                        coPackProductionUserControl1.Order = _selectOrder;

                        _orderChangeCausedComboSelectionChange = true;
                        _orderChangeCausedComboSelectionChange = false;
                    }
                }
                if (_selectOrder != null)
                    RefreshOrderInfo();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                DisplayToastNotification(ToastNotificationType.Error, "ChangeOrder", ex.Message, 10000);
            }
        }


        /// <summary>
        /// Refreshes the order information based on the selected order, including the production details and last production run.
        /// </summary>
        private void RefreshOrderInfo()
        {
            try
            {
                if (_log.IsDebugEnabled)
                {
                    _log.Debug($"RefreshOrderInfo, before calling GetProdOrder, SAPOrderNo = {_selectOrder.SAPOrderNo}");
                }

                // Retrieve the updated order information from the data source
                var so = AppData.GetProdOrder(_selectOrder.SAPOrderNo);
                if (!so.SuccessFlag) throw new ApplicationException($"Error refreshing order information. Error:{so.ServiceException}");
                _selectOrder = (RollLabelData)so.ReturnValue;

                if (_log.IsDebugEnabled)
                {
                    _log.Debug("RefreshOrderInfo, after calling GetProdOrder");
                    _log.Debug(_selectOrder);
                }

                // Update selected order properties if necessary
                if (_currentShift != null)
                {
                    _selectOrder.Shift = _currentShift;
                }
                if (_currentEmployee != null)
                {
                    _selectOrder.Employee = _currentEmployee;
                }
                if (_selectedLine != null)
                {
                    _selectOrder.ProductionLine = _selectedLine.ProductionLine;
                    _selectOrder.ProductionMachineNo = _selectedLine.ProductionMachineNo;
                    _selectOrder.OutputLoc = _selectedLine.OutputLocationCode;
                    _selectOrder.InputLoc = _selectedLine.InputLocationCode;
                    _selectOrder.Printer = _selectedLine.Printer;
                    _selectOrder.BatchNo = $"{_selectOrder.SAPOrderNo.ToString()}{_selectOrder.ProductionLine.Replace("TUB", "T")}";
                }

                // Update the co-pack production user control with the updated order
                coPackProductionUserControl1.Order = _selectOrder;

                // Get the last production run for the order
                so = AppData.GetLastProductionRun(_selectOrder.SAPOrderNo);
                if (!so.SuccessFlag) throw new ApplicationException($"Error getting next production run. Error:{so.ServiceException}");
                _prodRun = (int)so.ReturnValue;
                _prodRun += 1;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                DisplayToastNotification(ToastNotificationType.Error, "RefreshOrderInfo", ex.Message, 10000);
            }
        }


        /// <summary>
        /// Event handler for the form load event. Initializes the form and sets up event handlers.
        /// </summary>
        private void frmCoPack_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize the logger
                _log = LogManager.GetLogger(GetType());

                // Retrieve the production lines from the data source
                ServiceOutput so = AppData.GetProdLines(false);
                if (!so.SuccessFlag) throw new ApplicationException($"Error getting production lines: {so.ServiceException}");

                // Filter and create a list of production lines specific to tubs
                List<ProductionLine> tubLines = ((List<ProductionLine>)so.ReturnValue).Where(t => t.Code.StartsWith("TUB")).ToList();

                // Create a list of ProductionLineMachineNo objects from the filtered tub lines
                _prodLines = tubLines.Select(t => new ProductionLineMachineNo
                {
                    ProductionLine = t.Code,
                    ProductionMachineNo = t.LineNo,
                    InputLocationCode = t.InputLocationCode,
                    OutputLocationCode = t.OutputLocationCode,
                    Printer = t.Printer
                }).OrderBy(t => t.ProductionLine).ToList();

                // Insert a default "Select Line" option at the beginning of the list
                _prodLines.Insert(0, new ProductionLineMachineNo { ProductionLine = "<Select Line>", ProductionMachineNo = "0" });

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                DisplayToastNotification(ToastNotificationType.Error, "frmCoPack_Load", ex.Message, 10000);
            }
        }

        /// <summary>
        /// Performs the production process for co-pack items.
        /// </summary>
        /// <param name="qtyPerCase">The quantity of items per case.</param>
        /// <param name="numberOfCases">The number of cases to produce.</param>
        private void Produce(int qtyPerCase, int numberOfCases)
        {
            try
            {
                // Obtain the user credentials for SAPB1
                var userNamePW = AppUtility.GetUserNameAndPasswordMix(_selectOrder.ProductionMachineNo);
                ServiceOutput so;
                using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
                {
                    using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                    {
                        // Process planned issues and add them to the inventory issue
                        foreach (var plIssue in _plannedIssue)
                        {
                            if (plIssue.ShortQty == 0)
                            {
                                if (_log.IsDebugEnabled)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine("Calling AddOrderIssueLine:");
                                    sb.AppendLine("--------------------------");
                                    sb.AppendLine($"plIssue.BaseEntry = {plIssue.BaseEntry}");
                                    sb.AppendLine($"plIssue.BaseLine = {plIssue.BaseLine}");
                                    sb.AppendLine($"plIssue.ItemCode = {plIssue.ItemCode}");
                                    sb.AppendLine($"plIssue.PlannedIssueQty = {plIssue.PlannedIssueQty}");
                                    sb.AppendLine($"plIssue.StorageLocation = {plIssue.StorageLocation}");
                                    sb.AppendLine($"plIssue.QualityStatus = {plIssue.QualityStatus}");
                                    sb.AppendLine($"plIssue.Batch = {plIssue.Batch}");
                                    sb.AppendLine($"plIssue.LUID = {plIssue.LUID}");
                                    sb.AppendLine($"plIssue.SSCC = {plIssue.SSCC}");
                                    sb.AppendLine($"plIssue.UOM = {plIssue.UOM}");
                                    sb.AppendLine($"_selectOrder.SAPOrderNo.ToString() = {_selectOrder.SAPOrderNo.ToString()}");
                                    _log.Debug(sb.ToString());
                                }
                                // Add an order issue line to the inventory issue
                                invIssue.AddOrderIssueLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.SAPOrderNo.ToString());
                            }
                            else
                            {
                                if (_log.IsDebugEnabled)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine("Calling AddIssueShortage");
                                    sb.AppendLine("------------------------");
                                    sb.AppendLine($"_selectOrder.SAPOrderNo = {_selectOrder.SAPOrderNo}");
                                    sb.AppendLine($"plIssue.ItemCode = {plIssue.ItemCode}");
                                    sb.AppendLine($"plIssue.ShortQty = {Convert.ToDecimal(plIssue.ShortQty)}");
                                    _log.Debug(sb.ToString());
                                }
                                // Add a shortage for the planned issue
                                so = AppData.AddIssueShortage(_selectOrder.SAPOrderNo, plIssue.ItemCode, Convert.ToDecimal(plIssue.ShortQty));
                                if (!so.SuccessFlag) throw new ApplicationException($"Error adding shortage. Error:{so.ServiceException}");
                            }
                        }
                        if (_log.IsDebugEnabled)
                        {
                            var issueSum = _plannedIssue.Sum(q => q.PlannedIssueQty);
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine($"_plannedIssue.Sum(q => q.PlannedIssueQty) = {issueSum}");
                            _log.Debug(sb.ToString());
                        }
                        // Save the inventory issue
                        if (_plannedIssue.Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                    }

                    // Create SSCCs for each case and an SSCC for the pallet
                    List<int> luids = new List<int>();
                    List<string> ssccs = new List<string>();

                    if (string.IsNullOrEmpty(_selectOrder.MaterialCode))
                    {
                        _selectOrder.MaterialCode = "CP";
                    }

                    // Ensure there is a Yanjan order ID since these aren't generated automatically
                    so = AppData.EnsureYJNOrderNo(_selectOrder.SAPOrderNo, _selectOrder.ProductionMachineNo,
                        _selectOrder.StartDate, _selectOrder.MaterialCode, _selectOrder.ProductName);

                    if (!so.SuccessFlag)
                    {
                        throw new ApplicationException($"Error generating YJN order number. Error: {so.ServiceException}");
                    }

                    string yjnOrderNo = so.ReturnValue.ToString();

                    // RDJ 20230620 - Create one line/SSCC for the whole pallet and not for each case.
                    using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                    {
                        // RDJ 20230620 This was for when we were creating a SSCC for each case. 
                        // Yanjan doesn't want this.
                        //for (int caseNumber = 0; caseNumber < numberOfCases; ++caseNumber)
                        //{
                        //    // Create an SSCC for each case
                        //    so = AppData.CreateSSCC();
                        //    if (!so.SuccessFlag) throw new ApplicationException($"Error Creating SSCC. Error:{so.ServiceException}");
                        //    var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
                        //    luid = luid_sscc.Key;
                        //    sscc = luid_sscc.Value;
                        //    luids.Add(luid);
                        //    ssccs.Add(sscc);
                        //    var defaultStatus = AppUtility.GetDefaultStatus();
                        //    var defaultUom = AppUtility.GetDefaultUom();

                        //    if (_log.IsDebugEnabled) _log.Debug($"_selectOrder.OutputLoc = {_selectOrder.OutputLoc}");
                        //    // Add a line to the inventory receipt for each case
                        //    invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ItemCode, Convert.ToDouble(qtyPerCase), _prodRun, _selectOrder.OutputLoc, defaultStatus, _selectOrder.BatchNo, luid, sscc, defaultUom, _selectOrder.SAPOrderNo.ToString(), false, 0, _selectOrder.Shift, _selectOrder.Employee);
                        //}

                        // Create an SSCC for the pallet 
                        so = AppData.CreateSSCC();
                        if (!so.SuccessFlag) throw new ApplicationException($"Error Creating SSCC. Error:{so.ServiceException}");
                        var palletLuid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
                        var palletLUID = palletLuid_sscc.Key;
                        var palletSSCC = palletLuid_sscc.Value;

                        var defaultStatus = AppUtility.GetDefaultStatus();
                        var defaultUom = AppUtility.GetDefaultUom();
                        if (_log.IsDebugEnabled) _log.Debug($"_selectOrder.OutputLoc = {_selectOrder.OutputLoc}");

                        // Add a line to the inventory receipt for the pallet
                        invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ItemCode, numberOfCases, _prodRun, _selectOrder.OutputLoc, defaultStatus, _selectOrder.BatchNo, palletLUID, palletSSCC, defaultUom, _selectOrder.SAPOrderNo.ToString(), false, 0, _selectOrder.Shift, _selectOrder.Employee);

                        // Save the inventory receipt
                        if (invReceipt.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }

                        // Increment the production run
                        so = AppData.IncrementProductionRun(_selectOrder.SAPOrderNo);
                        if (!so.SuccessFlag) throw new ApplicationException($"Error getting next batch. Error:{so.ServiceException}");
                        _prodRun = (int)so.ReturnValue;
                        _prodRun += 1;


                        // RDJ 20230620 Only creating one entry for the entire pallet.
                        // Add cases to the pallet LUID
                        //foreach (var caseLUID in luids)
                        //{
                        //    so = AppData.AddSubLUIDToMaster(caseLUID, palletLUID, _lotNumber, yjnOrderNo, _selectOrder.ItemCode);
                        //    if (!so.SuccessFlag) throw new ApplicationException($"Error adding case LUID to pallet. Error: {so.ServiceException}");
                        //}

                        // Add a row to @SII_PG_BUNDLE for the new LUID.
                        so = AppData.AddNewBundle(palletLUID);

                        if (!so.SuccessFlag)
                        {
                            throw new ApplicationException($"Error adding to @SII_PG_BUNDLE. Error: {so.ServiceException} ");
                        }

                        // Print the pallet label
                        PrintCoPackPalletLabel(palletSSCC);

                        // Get the ID for the new "bundle" update the printed status
                        SetPalletPrintedStatus(palletSSCC);
                    }
                }
                // Display success notification
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Cases Produced", $"{numberOfCases} cases produced. Order: {_selectOrder.SAPOrderNo}");

            }
            catch (Exception ex)
            {
                // Log and display error notification
                _log.Error(ex.Message, ex);
                DisplayToastNotification(ToastNotificationType.Error, "Produce", ex.Message, 10000);
            }
        }

        /// <summary>
        /// Updates the printed status of the co-pack pallet
        /// </summary>
        /// <param name="palletSSCC"></param>
        private void SetPalletPrintedStatus(string palletSSCC)
        {
            var so = AppData.GetBundlePackLabel(palletSSCC);
            if (so.SuccessFlag)
            {
                var packLabels = (IList<PackLabel>)so.ReturnValue;
                var soUpdate = AppData.UpdatePackLabel(packLabels[0].ID, packLabels[0].Qty, "Y");
                if (!soUpdate.SuccessFlag)
                {
                    throw new ApplicationException($"SetPalletPrintedStatus: {so.ServiceException}");
                }
            }
            else
            {
                throw new ApplicationException($"SetPalletPrintedStatus: {so.ServiceException}");
            }
        }

        /// <summary>
        /// Performs the scrap process for co-pack items.
        /// </summary>
        /// <param name="qty">The quantity of items to scrap.</param>
        /// <param name="scrapReason">The reason for scrapping the items.</param>
        private void Scrap(int qty, string scrapReason)
        {
            try
            {
                ServiceOutput so;

                // Create an SSCC for the scrap items
                so = AppData.CreateSSCC();
                if (!so.SuccessFlag) throw new ApplicationException($"Error Creating SSCC. Error:{so.ServiceException}");
                var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
                luid = luid_sscc.Key;
                sscc = luid_sscc.Value;

                // Obtain the user credentials for SAPB1
                var userNamePW = AppUtility.GetUserNameAndPasswordMix(_selectOrder.ProductionMachineNo);
                var prodBatchNo = AppUtility.GetOrderBatchNoFromChar(_selectOrder.BatchNo[0]);

                // Refresh the planned issue quantity based on the scrap quantity
                _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(qty), 1);

                using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
                {
                    using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                    {
                        // Process planned issues and add them to the inventory issue
                        foreach (var plIssue in _plannedIssue.Where(i => i.BatchControlled)) // don't issue packaging material for scrap
                        {
                            if (plIssue.ShortQty == 0)
                            {
                                // Add an order issue line to the inventory issue
                                invIssue.AddOrderIssueLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.YJNOrder);
                            }
                            else
                            {
                                // Add a shortage for the planned issue
                                so = AppData.AddIssueShortage(_selectOrder.SAPOrderNo, plIssue.ItemCode, Convert.ToDecimal(plIssue.ShortQty));
                                if (!so.SuccessFlag) throw new ApplicationException($"Error adding shortage. Error:{so.ServiceException}");
                            }
                        }

                        // Save the inventory issue
                        if (_plannedIssue.Where(i => i.BatchControlled).Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                    }

                    using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                    {
                        // Add a line to the inventory receipt for the scrap items
                        MessageBox.Show($"qty = {qty}\nscrapLine = {_selectOrder.ScrapLine}");
                        invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ScrapItem, Convert.ToDouble(qty), prodBatchNo, AppUtility.GetScrapLocCode(), "RELEASED", "", luid, sscc, "Kgs", _selectOrder.BatchNo + "SCRAP", true, _selectOrder.ScrapLine, _selectOrder.Shift, _selectOrder.Employee, scrapReason);

                        // Save the inventory receipt
                        if (invReceipt.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                    }
                }

                // Display success notification
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Scrap Produced", $"{qty} produced. Order: {_selectOrder.SAPOrderNo}");
            }
            catch (Exception ex)
            {
                // Log and display error notification
                _log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //DisplayToastNotification(ToastNotificationType.Error, "Scrap", ex.Message, 10000);
            }
        }


        /// <summary>
        /// Prints co-pack labels for the given SSCCs and quantity per case.
        /// </summary>
        /// <param name="ssccs">The list of SSCCs to print labels for.</param>
        /// <param name="qtyPerCase">The quantity per case.</param>
        private void PrintCoPackLabel(List<string> ssccs, int qtyPerCase)
        {
            try
            {
                // Get the label print location and extension
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();

                // Generate a unique file name for the co-pack labels
                var fileNameCoPackLabels = Path.Combine(labelPrintLoc, $"CoPackLabel{Guid.NewGuid().ToString()}" + labelPrintExtension);

                // Get the format file path for co-pack labels
                var formatFilePathCoPackLabel = AppUtility.GetPGDefaultCoPackLabelFormat();

                // Create a StringBuilder to build the label content
                var sbMixLabel = new StringBuilder(5000);

                if (_log.IsDebugEnabled)
                {
                    _log.Debug($"_selectOrder.Printer = {_selectOrder.Printer}");
                }

                // Append the label print command to the StringBuilder
                sbMixLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathCoPackLabel, _selectOrder.Printer);
                sbMixLabel.AppendLine();
                sbMixLabel.Append(@"%END%");
                sbMixLabel.AppendLine();
                sbMixLabel.Append("Item, ItemName, IRMS, LotNo, SSCC, Qty, Order");
                sbMixLabel.AppendLine();

                // Append the label data for each SSCC
                for (int i = 0; i < ssccs.Count; i++)
                {
                    sbMixLabel.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", _selectOrder.ItemCode, _selectOrder.ItemDescription, "", _lotNumber, ssccs[i], Convert.ToDecimal(qtyPerCase), _selectOrder.SAPOrderNo);
                    sbMixLabel.AppendLine();
                }

                // Write the label content to the file
                using (StreamWriter sw = File.CreateText(fileNameCoPackLabels))
                {
                    sw.Write(sbMixLabel.ToString());
                }

                // Display success notification and refresh order information
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Co-pack label printed. Please check printer.");
                RefreshOrderInfo();
            }
            catch (Exception ex)
            {
                // Log and display error notification
                _log.Error(ex.Message, ex);
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }


        /// <summary>
        /// Prints a co-pack pallet label for the given pallet SSCC.
        /// </summary>
        /// <param name="palletSSCC">The SSCC of the pallet.</param>
        private void PrintCoPackPalletLabel(string palletSSCC)
        {
            try
            {
                // Get the label print location and extension
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();

                // Generate a unique file name for the co-pack pallet label
                var fileNameCoPackLabels = Path.Combine(labelPrintLoc, $"CoPackLabel{Guid.NewGuid().ToString()}" + labelPrintExtension);

                // Get the format file path for co-pack labels
                var formatFilePathCoPackLabel = AppUtility.GetPGDefaultCoPackLabelFormat();

                // Get default number of copies
                var copies = AppUtility.GetDefaultCoPackCopies();

                // Create a StringBuilder to build the label content
                var sbMixLabel = new StringBuilder(5000);

                if (_log.IsDebugEnabled)
                {
                    _log.Debug($"_selectOrder.Printer = {_selectOrder.Printer}");
                }

                // Append the label print command to the StringBuilder
                sbMixLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /C={2} /R=3 /P /DD", formatFilePathCoPackLabel, _selectOrder.Printer, copies);
                sbMixLabel.AppendLine();
                sbMixLabel.Append(@"%END%");
                sbMixLabel.AppendLine();
                sbMixLabel.Append("Item, ItemName, IRMS, LotNo, PMXSSCC, SSCC, Qty, Order");
                sbMixLabel.AppendLine();

                // Get the bundle pack label for the pallet SSCC
                ServiceOutput so = AppData.GetBundlePackLabel(palletSSCC);

                if (so.SuccessFlag)
                {
                    IList<PackLabel> packLabels = (IList<PackLabel>)so.ReturnValue;

                    if (packLabels.Count == 0)
                    {
                        throw new ApplicationException($"No Bundle info for SSCC {palletSSCC}");
                    }
                    else
                    {
                        // Append the label data for the pack label
                        var packLabel = packLabels[0];
                        sbMixLabel.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7}", packLabel.ItemCode, packLabel.ItemName, "", _lotNumber, packLabel.PMXSSCC, packLabel.SSCC, packLabel.Qty, packLabel.SAPOrder);
                        sbMixLabel.AppendLine();
                    }
                }
                else
                {
                    throw new ApplicationException(so.ServiceException);
                }

                // Write the label content to the file
                using (StreamWriter sw = File.CreateText(fileNameCoPackLabels))
                {
                    sw.Write(sbMixLabel.ToString());
                }

                // Display success notification and refresh order information
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Co-pack pallet label printed. Please check printer.");
                RefreshOrderInfo();
            }
            catch (Exception ex)
            {
                // Log and display error notification
                _log.Error(ex.Message, ex);
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Printing Co-Pack Pallet Label", $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} PrintCoPackPalletLabel.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }


        /// <summary>
        /// Displays a toast notification with the specified type, title, and text.
        /// </summary>
        /// <param name="type">The type of the toast notification.</param>
        /// <param name="title">The title of the toast notification.</param>
        /// <param name="text">The text of the toast notification.</param>
        /// <param name="timeOut">The duration for which the toast notification is displayed (in milliseconds). Default is 4000 milliseconds.</param>
        public void DisplayToastNotification(ToastNotificationType type, string title, string text, int timeOut = 4000)
        {
            // Close any existing toast notification
            m_htmlToast.Close();

            // Set the offset for the toast notification position
            int offset = 15;

            // Set the CSS class based on the notification type
            string cssClass = "w3-success";
            switch (type)
            {
                case ToastNotificationType.Success:
                    cssClass = "w3-success";
                    break;
                case ToastNotificationType.Warning:
                    cssClass = "w3-warning";
                    break;
                case ToastNotificationType.Error:
                    cssClass = "w3-error ";
                    break;
            }

            // Generate the HTML content for the toast notification
            string html = AppUtility.GenerateHTMLToast(title, text, cssClass);

            // Calculate the dimensions of the toast notification based on the HTML content
            int imgHeight = m_htmlToast.GetHTMLHeight(html) + 5;
            int imgWidth = this.Width - 25;

            // Set the size of the toast notification image
            m_htmlToast.SetImgSize(imgWidth, imgHeight);

            // Set the HTML content for the toast notification
            m_htmlToast.SetHTML(html);

            // Calculate the screen location to display the toast notification
            Rectangle rect = this.Bounds;
            Point px = new Point(rect.Left, rect.Bottom);
            Point screenLocation = PointToScreen(px);

            // Set the location of the toast notification image
            m_htmlToast.SetImgLocation(px.X + offset, px.Y - imgHeight - offset);

            // Show the toast notification for the specified duration
            m_htmlToast.Show(timeOut);
        }


        /// <summary>
        /// Event handler for the validation failed event of the CoPackProductionUserControl.
        /// Displays a toast notification with an error message when the validation fails.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The CoPackProductionValidationEventArgs containing the validation message.</param>
        private void coPackProductionUserControl1_ValidationFailed(object source, CoPackProductionUserControl.CoPackProductionValidationEventArgs e)
        {
            // Display a toast notification with an error message
            DisplayToastNotification(ToastNotificationType.Error, "Production", e.ValidationMessage);
        }

        /// <summary>
        /// Event handler for the issues refresh requested event of the CoPackProductionUserControl.
        /// Refreshes the planned issues based on the specified quantity per case and number of cases.
        /// Displays a dialog to handle shortage issues if any, and triggers the Produce method if production is requested.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The CoPackProductionRefreshIssuesEventArgs containing the refresh details.</param>
        private void coPackProductionUserControl1_IssuesRefreshRequested(object source, CoPackProductionUserControl.CoPackProductionRefreshIssuesEventArgs e)
        {
            try
            {
                if (_log.IsDebugEnabled)
                {
                    // Log debug information before calling AppUtility.RefreshIssueQty
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("About to call AppUtility.RefreshIssueQty");
                    sb.AppendLine("----------------------------------------");
                    sb.AppendLine($"_selectOrder.SAPOrderNo = {_selectOrder.SAPOrderNo}");
                    sb.AppendLine($"_selectOrder.ProductionLine = {_selectOrder.ProductionLine}");
                    sb.AppendLine($"(decimal)(e.QtyPerCase * e.NumberOfCases) = {(decimal)(e.QtyPerCase * e.NumberOfCases)}");
                    _log.Debug(sb.ToString());
                }

                // Refresh the planned issues based on the quantity per case and number of cases
                _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, (decimal)(e.QtyPerCase * e.NumberOfCases));

                if (_log.IsDebugEnabled)
                {
                    // Log the returned planned issues
                    foreach (var issue in _plannedIssue)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Returned from AppUtility.RefreshIssueQty");
                        sb.AppendLine("Planned issues:");
                        sb.AppendLine("---------------");
                        sb.AppendLine(issue.ToString());
                        _log.Debug(sb.ToString());
                    }
                }

                var hasShortage = _plannedIssue.Where(i => i.ShortQty > 0 && i.BatchControlled);

                if (hasShortage.Count() > 0 || !e.ProduceRequested)
                {
                    // Display a dialog to handle shortage issues if any
                    using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
                    {
                        frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                        DialogResult dr = frmPlannedIssues.ShowDialog();
                    }
                }

                if (e.ProduceRequested)
                {
                    // Trigger the Produce method if production is requested
                    Produce(e.QtyPerCase, e.NumberOfCases);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                DisplayToastNotification(ToastNotificationType.Error, "Issues Refresh", ex.Message, 10000);
            }
        }


        /// <summary>
        /// Event handler for the lot number entered event of the CoPackProductionUserControl.
        /// Updates the lot number with the entered value.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The CoPackProductionLotNumberEnteredEventArgs containing the entered lot number.</param>
        private void coPackProductionUserControl1_LotNumberEntered(object source, CoPackProductionUserControl.CoPackProductionLotNumberEnteredEventArgs e)
        {
            // Update the lot number with the entered value
            _lotNumber = e.LotNumber;
        }
    }
}
