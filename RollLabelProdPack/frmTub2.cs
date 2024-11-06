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
    /// Represents a form for tub production
    /// </summary>
    public partial class frmTub2 : Form
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
        //private int luid;
        //private string sscc;
        private int _prodRun;
        private ILog _log;

        /// <summary>
        /// Initializes a new instance of the frmTub2 class
        /// </summary>
        public frmTub2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler triggered when the btnSelect button is clicked.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            // Call the ChangeOrder method to initiate the order selection process
            ChangeOrder();
        }

        /// <summary>
        /// Method called to initiate the order change process.
        /// </summary>
        private void ChangeOrder()
        {
            try
            {
                // Create a SelectOrderDialog instance
                using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
                {
                    // Set the data source for the dialog
                    frmSignInDialog.SetDataSource("TUB");

                    // Show the dialog and capture the result
                    DialogResult dr = frmSignInDialog.ShowDialog();

                    // If the result is OK, update the selected order
                    if (dr == DialogResult.OK)
                    {
                        _selectOrder = frmSignInDialog.SelectOrder;

                        // Log the selected order if debug mode is enabled
                        if (_log.IsDebugEnabled)
                        {
                            _log.Debug("In ChangeOrder, using selected order from frmSignInDialog");
                            _log.Debug(_selectOrder);
                        }

                        // Update the current shift, employee, scrap item, YJN order, and scrap line
                        _currentShift = _selectOrder.Shift;
                        _currentEmployee = _selectOrder.Employee;
                        _currentScrapItem = _selectOrder.ScrapItem;
                        _currentYJNOrder = _selectOrder.YJNOrder;
                        _currentScrapLine = _selectOrder.ScrapLine;

                        // Update the order information in the user control
                        tubProductionUserControl1.Order = _selectOrder;

                        // Enable the "toLineComboBox" and set the selected line
                        toLineComboBox.Enabled = true;
                        if (_prodLines.Any(t => t.ProductionLine == _selectOrder.ProductionLine))
                            toLineComboBox.SelectedItem = _prodLines.First(t => t.ProductionLine == _selectOrder.ProductionLine);
                        else
                            toLineComboBox.SelectedItem = _prodLines[0];

                        // Update the selected line
                        _selectedLine = (ProductionLineMachineNo)toLineComboBox.SelectedItem;
                    }
                }

                // Refresh the order information if an order is selected
                if (_selectOrder != null)
                    RefreshOrderInfo();
            }
            catch (Exception ex)
            {
                // Log the error
                _log.Error(ex.Message, ex);

                // Display an error toast notification
                DisplayToastNotification(ToastNotificationType.Error, "ChangeOrder", ex.Message, 10000);
            }
        }


        /// <summary>
        /// Refreshes the order information based on the selected order.
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

                // Throw an exception if the retrieval is unsuccessful
                if (!so.SuccessFlag) throw new ApplicationException($"Error refreshing order information. Error:{so.ServiceException}");

                // Update the _selectOrder variable with the retrieved data
                _selectOrder = (RollLabelData)so.ReturnValue;

                if (_log.IsDebugEnabled)
                {
                    _log.Debug("RefreshOrderInfo, after calling GetProdOrder");
                    _log.Debug(_selectOrder);
                }

                // Update specific fields in the _selectOrder variable if corresponding values are set
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
                    // RDJ 20230818 - remove dash from batch number
                    _selectOrder.BatchNo = $"{_selectOrder.SAPOrderNo.ToString()}{_selectOrder.ProductionLine.Replace("TUB", "T")}";
                }

                // Update the order information in the user controls
                tubProductionUserControl1.Order = _selectOrder;
                tubScrapUserControl1.Order = _selectOrder;

                // Get the next production run for the order
                so = AppData.GetLastProductionRun(_selectOrder.SAPOrderNo);

                // Throw an exception if the retrieval is unsuccessful
                if (!so.SuccessFlag) throw new ApplicationException($"Error getting next production run. Error:{so.ServiceException}");

                // Update the _prodRun variable with the next production run
                _prodRun = (int)so.ReturnValue;
                _prodRun += 1;
            }
            catch (Exception ex)
            {
                // Log the error
                _log.Error(ex.Message, ex);

                // Display an error toast notification
                DisplayToastNotification(ToastNotificationType.Error, "RefreshOrderInfo", ex.Message, 10000);
            }
        }


        /// <summary>
        /// Handles the load event of the frmTub2 form.
        /// </summary>
        private void frmTub2_Load(object sender, EventArgs e)
        {
            try
            {
                _log = LogManager.GetLogger(typeof(frmTub2));

                // Retrieve the production lines from the data source
                ServiceOutput so = AppData.GetProdLines(false);

                // Throw an exception if the retrieval is unsuccessful
                if (!so.SuccessFlag) throw new ApplicationException($"Error getting production lines: {so.ServiceException}");

                // Filter and extract the TUB production lines
                List<ProductionLine> tubLines = ((List<ProductionLine>)so.ReturnValue).Where(t => t.Code.StartsWith("TUB")).ToList();

                // Create a list of ProductionLineMachineNo objects from the TUB lines
                _prodLines = tubLines.Select(t => new ProductionLineMachineNo
                {
                    ProductionLine = t.Code,
                    ProductionMachineNo = t.LineNo,
                    InputLocationCode = t.InputLocationCode,
                    OutputLocationCode = t.OutputLocationCode,
                    Printer = t.Printer
                }).OrderBy(t => t.ProductionLine).ToList();

                // Insert a placeholder item at the beginning of the list
                _prodLines.Insert(0, new ProductionLineMachineNo { ProductionLine = "<Select Line>", ProductionMachineNo = "0" });

                // Set the data source and display member for the toLineComboBox control
                toLineComboBox.DataSource = _prodLines;
                toLineComboBox.DisplayMember = "ProductionLine";

                // Subscribe to events and set properties for the tubProductionUserControl1 control
                tubProductionUserControl1.ValidationFailed += TubProductionUserControl1_ValidationFailed;
                tubProductionUserControl1.IssuesRefreshRequested += TubProductionUserControl1_IssuesRefreshRequested;

                // Uncomment the following lines if scrap functionality is implemented
                //tubScrapUserControl1.ScrapRequested += TubScrapUserControl1_ScrapRequested;
                //tubScrapUserControl1.ValidationFailed += TubScrapUserControl1_ValidationFailed;

                // Retrieve the scrap reasons from the data source
                //so = AppData.GetScrapReasons("TUB");
                //if (!so.SuccessFlag) throw new ApplicationException($"Error getting scrap reasons: {so.ServiceException}");
                //tubScrapUserControl1.ScrapReasons = (List<string>)so.ReturnValue;
            }
            catch (Exception ex)
            {
                // Log the error
                _log.Error(ex.Message, ex);

                // Display an error toast notification
                DisplayToastNotification(ToastNotificationType.Error, "frmTub2_Load", ex.Message, 10000);
            }
        }

        /// <summary>
        /// Handles the IssuesRefreshRequested event of the TubProductionUserControl1 control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The TubProductionRefreshIssuesEventArgs instance containing the event data.</param>
        private void TubProductionUserControl1_IssuesRefreshRequested(object source, TubProductionRefreshIssuesEventArgs e)
        {
            try
            {
                if (_log.IsDebugEnabled)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("About to call AppUtility.RefreshIssueQty");
                    sb.AppendLine("----------------------------------------");
                    sb.AppendLine($"_selectOrder.SAPOrderNo = {_selectOrder.SAPOrderNo}");
                    sb.AppendLine($"_selectOrder.ProductionLine = {_selectOrder.ProductionLine}");
                    sb.AppendLine($"(decimal)(e.QtyPerCase * e.NumberOfCases) = {(decimal)(e.QtyPerCase * e.NumberOfCases)}");
                    _log.Debug(sb.ToString());
                }

                // Refresh the planned issue quantities
                _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, (decimal)(e.QtyPerCase * e.NumberOfCases), 1);

                if (_log.IsDebugEnabled)
                {
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

                // Check if there are shortages and the produce is not requested
                var hasShortage = _plannedIssue.Where(i => i.ShortQty > 0 && i.BatchControlled);

                if (hasShortage.Count() > 0 || !e.ProduceRequested)
                {
                    // Open the PlannedIssueDialog to display planned issues
                    using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
                    {
                        frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                        DialogResult dr = frmPlannedIssues.ShowDialog();
                    }
                }

                // Check if produce is requested
                if (e.ProduceRequested)
                {
                    Produce(e.QtyPerCase, e.NumberOfCases);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _log.Error(ex.Message, ex);

                // Display an error toast notification
                DisplayToastNotification(ToastNotificationType.Error, "Issues Refresh", ex.Message, 10000);
            }
        }

        /// <summary>
        /// Handles the production process for tubs.
        /// </summary>
        /// <param name="qtyPerCase">The quantity per case.</param>
        /// <param name="numberOfCases">The number of cases.</param>
        private void Produce(int qtyPerCase, int numberOfCases)
        {
            try
            {
                // Get the username and password for SAP
                var userNamePW = AppUtility.GetUserNameAndPasswordMix(_selectOrder.ProductionMachineNo);
                ServiceOutput so;

                // Create a SAPB1 instance using the obtained username and password
                using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
                {
                    // Create an InventoryIssue object
                    using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                    {
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

                                // Add an order issue line to the InventoryIssue object
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

                                // Add an issue shortage using AppData.AddIssueShortage method
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

                        // Save the InventoryIssue object if there are planned issue quantities greater than 0
                        if (_plannedIssue.Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false)
                        {
                            throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                        }
                    }

                    List<int> luids = new List<int>();
                    List<string> ssccs = new List<string>();

                    // Create an InventoryReceipt object
                    using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                    {
                        for (int caseNumber = 0; caseNumber < numberOfCases; ++caseNumber)
                        {
                            // Create an SSCC using AppData.CreateSSCC method
                            so = AppData.CreateSSCC();
                            if (!so.SuccessFlag) throw new ApplicationException($"Error Creating SSCC. Error:{so.ServiceException}");
                            var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
                            var luid = luid_sscc.Key;
                            var sscc = luid_sscc.Value;
                            luids.Add(luid);
                            ssccs.Add(sscc);
                            var defaultStatus = AppUtility.GetDefaultStatus();
                            var defaultUom = AppUtility.GetDefaultUom();

                            if (_log.IsDebugEnabled) _log.Debug($"_selectOrder.OutputLoc = {_selectOrder.OutputLoc}");

                            // Add a line to the InventoryReceipt object
                            invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ItemCode, Convert.ToDouble(qtyPerCase), _prodRun, _selectOrder.OutputLoc, defaultStatus, _selectOrder.BatchNo, luid, sscc, defaultUom, _selectOrder.SAPOrderNo.ToString(), false, 0, _selectOrder.Shift, _selectOrder.Employee);
                        }

                        // Save the InventoryReceipt object
                        if (invReceipt.Save() == false)
                        {
                            throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                        }

                        // Increment the production run using AppData.IncrementProductionRun method
                        so = AppData.IncrementProductionRun(_selectOrder.SAPOrderNo);
                        if (!so.SuccessFlag) throw new ApplicationException($"Error getting next batch. Error:{so.ServiceException}");
                        _prodRun = (int)so.ReturnValue;
                        _prodRun += 1;

                        // Print tub labels using the PrintTubLabel method
                        PrintTubLabel(ssccs, qtyPerCase);
                    }
                }

                // Display a success toast notification
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Tubs Produced", $"{numberOfCases} cases produced. Order: {_selectOrder.SAPOrderNo}");
            }
            catch (Exception ex)
            {
                // Log the error
                _log.Error(ex.Message, ex);

                // Display an error toast notification
                DisplayToastNotification(ToastNotificationType.Error, "Produce", ex.Message, 10000);
            }
        }

        /// <summary>
        /// Handles the process of scrapping tubs.
        /// </summary>
        /// <param name="qty">The quantity of tubs to be scrapped.</param>
        /// <param name="scrapReason">The reason for scrapping the tubs.</param>
        private void Scrap(int qty, string scrapReason)
        {
            try
            {
                ServiceOutput so;

                // Create SSCC (Serial Shipping Container Code)
                so = AppData.CreateSSCC();
                if (!so.SuccessFlag)
                    throw new ApplicationException($"Error Creating SSCC. Error: {so.ServiceException}");
                var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
                int luid = luid_sscc.Key;
                string sscc = luid_sscc.Value;

                // Retrieve username and password for SAP
                // and production batch number based on the first character of the order's batch number
                var userNamePW = AppUtility.GetUserNameAndPasswordMix(_selectOrder.ProductionMachineNo);
                var prodBatchNo = AppUtility.GetOrderBatchNoFromChar(_selectOrder.BatchNo[0]);

                // Refresh planned issue quantities
                _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(qty), 1);

                using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
                {
                    // Create InventoryIssue object
                    using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                    {
                        // Process planned issue quantities for scrap
                        foreach (var plIssue in _plannedIssue.Where(i => i.BatchControlled))
                        {
                            if (plIssue.ShortQty == 0)
                            {
                                // Add order issue line
                                invIssue.AddOrderIssueLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.YJNOrder);
                            }
                            else
                            {
                                // Add issue shortage
                                so = AppData.AddIssueShortage(_selectOrder.SAPOrderNo, plIssue.ItemCode, Convert.ToDecimal(plIssue.ShortQty));
                                if (!so.SuccessFlag)
                                    throw new ApplicationException($"Error adding shortage. Error: {so.ServiceException}");
                            }
                        }

                        // Save inventory issues
                        if (_plannedIssue.Where(i => i.BatchControlled).Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false)
                        {
                            throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                        }
                    }

                    // Create InventoryReceipt object
                    using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                    {
                        // Add scrap line to inventory receipt
                        invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ScrapItem, Convert.ToDouble(qty), prodBatchNo, AppUtility.GetScrapLocCode(), "RELEASED", "", luid, sscc, "Kgs", _selectOrder.BatchNo + "SCRAP", true, _selectOrder.ScrapLine, _selectOrder.Shift, _selectOrder.Employee, scrapReason);

                        // Save inventory receipt
                        if (invReceipt.Save() == false)
                        {
                            throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                        }
                    }
                }

                // Display success toast notification
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Scrap Produced", $"{qty} produced. Order: {_selectOrder.SAPOrderNo}");
            }
            catch (Exception ex)
            {
                // Log the error
                _log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //DisplayToastNotification(ToastNotificationType.Error, "Scrap", ex.Message, 10000);
            }
        }

        /// <summary>
        /// Prints tub case labels for the given SSCCs and quantity per case.
        /// </summary>
        /// <param name="ssccs">The list of SSCCs (Serial Shipping Container Codes) for the tubs.</param>
        /// <param name="qtyPerCase">The quantity of tubs per case.</param>
        private void PrintTubLabel(List<string> ssccs, int qtyPerCase)
        {
            try
            {
                // Get label print location, extension, and file paths
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();
                var fileNameTubCaseLabels = Path.Combine(labelPrintLoc, $"TubCaseLabel{Guid.NewGuid().ToString()}" + labelPrintExtension);
                var formatFilePathTubCaseLabel = AppUtility.GetPGDefaultTubCaseLabelFormat(); // Shouldn't be AppUtility.GetPGDefaultResmixLabelFormat();

                // Create a StringBuilder to store the label content
                var sbMixLabel = new StringBuilder(5000);

                if (_log.IsDebugEnabled)
                {
                    _log.Debug($"_selectOrder.Printer = {_selectOrder.Printer}");
                }

                // Append label print command to the StringBuilder
                sbMixLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathTubCaseLabel, _selectOrder.Printer);
                sbMixLabel.AppendLine();
                sbMixLabel.Append(@"%END%");
                sbMixLabel.AppendLine();
                sbMixLabel.Append("Item, ItemName, IRMS, LotNo, SSCC, Qty");
                sbMixLabel.AppendLine();

                // Add tub case label data to the StringBuilder
                for (int i = 0; i < ssccs.Count; i++)
                {
                    sbMixLabel.AppendFormat("{0},{1},{2},{3},{4},{5}", _selectOrder.ItemCode, _selectOrder.ItemDescription, "", _selectOrder.BatchNo, ssccs[i], Convert.ToDecimal(qtyPerCase));
                    sbMixLabel.AppendLine();
                }

                // Write the label content to a file
                using (StreamWriter sw = File.CreateText(fileNameTubCaseLabels))
                {
                    sw.Write(sbMixLabel.ToString());
                }

                // Display success toast notification and refresh order information
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Tub case label printed. Please check printer.");
                RefreshOrderInfo();
            }
            catch (Exception ex)
            {
                // Log the error
                _log.Error(ex.Message, ex);

                // Display error toast notification and write the exception details to the event log
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }

        /// <summary>
        /// Event handler for the ValidationFailed event of the TubScrapUserControl1 control.
        /// Displays an error toast notification with the validation message.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The TubScrapValidationEventArgs containing the validation message.</param>
        private void TubScrapUserControl1_ValidationFailed(object source, TubScrapValidationEventArgs e)
        {
            DisplayToastNotification(ToastNotificationType.Error, "Scrap", e.ValidationMessage);
        }


        /// <summary>
        /// Event handler for the ScrapRequested event of the TubScrapUserControl1 control.
        /// Initiates the scrap process by calling the Scrap method with the specified scrap quantity and reason.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The TubScrapEventArgs containing the scrap quantity and reason.</param>
        private void TubScrapUserControl1_ScrapRequested(object source, TubScrapEventArgs e)
        {
            Scrap(e.ScrapQty, e.ScrapReason);
        }


        /// <summary>
        /// Event handler for the ValidationFailed event of the TubProductionUserControl1 control.
        /// Displays an error toast notification with the provided validation message.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The TubProductionValidationEventArgs containing the validation message.</param>
        private void TubProductionUserControl1_ValidationFailed(object source, TubProductionValidationEventArgs e)
        {
            DisplayToastNotification(ToastNotificationType.Error, "Production", e.ValidationMessage);
        }

        /// <summary>
        /// Displays a toast notification with the specified type, title, and text.
        /// </summary>
        /// <param name="type">The type of the toast notification.</param>
        /// <param name="title">The title of the toast notification.</param>
        /// <param name="text">The text content of the toast notification.</param>
        /// <param name="timeOut">The timeout duration for displaying the toast notification in milliseconds. Default is 4000ms.</param>
        public void DisplayToastNotification(ToastNotificationType type, string title, string text, int timeOut = 4000)
        {
            // Close any existing toast notifications
            m_htmlToast.Close();

            int offset = 15;

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

            // Calculate the dimensions of the toast notification based on the content
            int imgHeight = m_htmlToast.GetHTMLHeight(html) + 5;
            int imgWidth = this.Width - 25;

            // Set the dimensions and HTML content of the toast notification
            m_htmlToast.SetImgSize(imgWidth, imgHeight);
            m_htmlToast.SetHTML(html);

            Rectangle rect = this.Bounds;
            Point px = new Point(rect.Left, rect.Bottom);
            Point screenLocation = PointToScreen(px);

            // Set the location of the toast notification
            m_htmlToast.SetImgLocation(px.X + offset, px.Y - imgHeight - offset);

            // Show the toast notification for the specified duration
            m_htmlToast.Show(timeOut);
        }


        /// <summary>
        /// Event handler for the selection change in the "toLineComboBox".
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void toLineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_orderChangeCausedComboSelectionChange)
                {
                    // Retrieve the selected production line and machine number
                    ProductionLineMachineNo selectedLineMachineNo = (ProductionLineMachineNo)toLineComboBox.SelectedItem;

                    // Create a StringBuilder to log debug messages
                    StringBuilder sbDebugMsg = new StringBuilder();
                    sbDebugMsg.AppendLine("selectedLineMachineNo = ");
                    sbDebugMsg.AppendLine(selectedLineMachineNo.ToString());

                    // Log the selected production line and machine number if debug logging is enabled
                    if (_log.IsDebugEnabled)
                        _log.Debug(sbDebugMsg.ToString());

                    if (selectedLineMachineNo.ProductionMachineNo != "0")
                    {
                        // Update the selected order's production line, machine number, input location, output location, and printer
                        _selectOrder.ProductionLine = selectedLineMachineNo.ProductionLine;
                        _selectOrder.ProductionMachineNo = selectedLineMachineNo.ProductionMachineNo;
                        _selectOrder.InputLoc = selectedLineMachineNo.InputLocationCode;
                        _selectOrder.OutputLoc = selectedLineMachineNo.OutputLocationCode;
                        _selectOrder.Printer = selectedLineMachineNo.Printer;
                        _selectedLine = selectedLineMachineNo;

                        // Log the output location and printer if debug logging is enabled
                        if (_log.IsDebugEnabled)
                            _log.Debug($"_selectOrder.OutputLoc = {_selectOrder.OutputLoc}");
                        if (_log.IsDebugEnabled)
                            _log.Debug($"_selectOrder.Printer = {_selectOrder.Printer}");

                        // Refresh the order information on the user interface
                        RefreshOrderInfo();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                _log.Error(ex.Message, ex);

                // Display a toast notification with the error message
                DisplayToastNotification(ToastNotificationType.Error, "Line Combo SelectedIndexChanged", ex.Message, 10000);
            }
        }

    }
}
