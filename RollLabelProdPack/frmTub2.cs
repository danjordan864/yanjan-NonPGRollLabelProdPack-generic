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
        private int luid;
        private string sscc;
        private int _prodRun;
        private ILog _log;

        public frmTub2()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }

        private void ChangeOrder()
        {
            try
            {
                using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
                {
                    frmSignInDialog.SetDataSource("TUB");
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
                        //txtQty.Text = "0";
                        //txtWeightKgs.Enabled = true;
                        //txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                        tubProductionUserControl1.Order = _selectOrder;
                        //tubScrapUserControl1.Order = _selectOrder;
                        _orderChangeCausedComboSelectionChange = true;
                        toLineComboBox.Enabled = true;
                        if (_prodLines.Any(t => t.ProductionLine == _selectOrder.ProductionLine))
                            toLineComboBox.SelectedItem = _prodLines.First(t => t.ProductionLine == _selectOrder.ProductionLine);
                        else
                            toLineComboBox.SelectedItem = _prodLines[0];
                        _selectedLine = (ProductionLineMachineNo)toLineComboBox.SelectedItem;
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

        private void RefreshOrderInfo()
        {
            try
            {
                if (_log.IsDebugEnabled)
                {
                    _log.Debug($"RefreshOrderInfo, before calling GetProdOrder, SAPOrderNo = {_selectOrder.SAPOrderNo}");
                }
                var so = AppData.GetProdOrder(_selectOrder.SAPOrderNo);
                if (!so.SuccessFlag) throw new ApplicationException($"Error refreshing order information. Error:{so.ServiceException}");
                _selectOrder = (RollLabelData)so.ReturnValue;
                if (_log.IsDebugEnabled)
                {
                    _log.Debug("RefreshOrderInfo, after calling GetProdOrder");
                    _log.Debug(_selectOrder);
                }
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
                    _selectOrder.OutputLoc = _selectedLine.OutputLocationCode;
                    _selectOrder.InputLoc = _selectedLine.InputLocationCode;
                    _selectOrder.BatchNo = $"{_selectOrder.SAPOrderNo.ToString()}{_selectOrder.ProductionLine.Replace("TUB", "T")}";
                }
                tubProductionUserControl1.Order = _selectOrder;
                tubScrapUserControl1.Order = _selectOrder;
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

        private void frmTub2_Load(object sender, EventArgs e)
        {
            try
            {
                _log = LogManager.GetLogger(typeof(frmTub2));
                ServiceOutput so = AppData.GetProdLines(false);
                if (!so.SuccessFlag) throw new ApplicationException($"Error getting production lines: {so.ServiceException}");
                List<ProductionLine> tubLines = ((List<ProductionLine>)so.ReturnValue).Where(t => t.Code.StartsWith("TUB")).ToList();
                _prodLines = tubLines.Select(t => new ProductionLineMachineNo { ProductionLine = t.Code, ProductionMachineNo = t.LineNo, InputLocationCode = t.InputLocationCode, OutputLocationCode = t.OutputLocationCode, Printer = t.Printer }).OrderBy(t => t.ProductionLine).ToList();
                _prodLines.Insert(0, new ProductionLineMachineNo { ProductionLine = "<Select Line>", ProductionMachineNo = "0" });
                toLineComboBox.DataSource = _prodLines;
                toLineComboBox.DisplayMember = "ProductionLine";
                tubProductionUserControl1.ValidationFailed += TubProductionUserControl1_ValidationFailed;
                tubProductionUserControl1.IssuesRefreshRequested += TubProductionUserControl1_IssuesRefreshRequested;
                // RDJ 20230315 scrap not implemented
                //tubScrapUserControl1.ScrapRequested += TubScrapUserControl1_ScrapRequested;
                //tubScrapUserControl1.ValidationFailed += TubScrapUserControl1_ValidationFailed;
                //so = AppData.GetScrapReasons("TUB");
                //if (!so.SuccessFlag) throw new ApplicationException($"Error getting scrap reasons: {so.ServiceException}");
                //tubScrapUserControl1.ScrapReasons = (List<string>)so.ReturnValue;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                DisplayToastNotification(ToastNotificationType.Error, "frmTub2_Load", ex.Message, 10000);
            }
        }

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
                _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, (decimal)(e.QtyPerCase * e.NumberOfCases));
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
                var hasShortage = _plannedIssue.Where(i => i.ShortQty > 0 && i.BatchControlled);
                if (hasShortage.Count() > 0 || !e.ProduceRequested)
                {
                    using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
                    {
                        frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                        DialogResult dr = frmPlannedIssues.ShowDialog();
                    }
                }
                if (e.ProduceRequested)
                {
                    Produce(e.QtyPerCase, e.NumberOfCases);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                DisplayToastNotification(ToastNotificationType.Error, "Issues Refresh", ex.Message, 10000);
            }
        }

        private void Produce(int qtyPerCase, int numberOfCases)
        {
            try
            {
                var userNamePW = AppUtility.GetUserNameAndPasswordMix(_selectOrder.ProductionMachineNo);
                ServiceOutput so;
                using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
                {
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
                        if (_plannedIssue.Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                    }

                    List<int> luids = new List<int>();
                    List<string> ssccs = new List<string>();

                    using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                    {
                        for (int caseNumber = 0; caseNumber < numberOfCases; ++caseNumber)
                        {
                            so = AppData.CreateSSCC();
                            if (!so.SuccessFlag) throw new ApplicationException($"Error Creating SSCC. Error:{so.ServiceException}");
                            var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
                            luid = luid_sscc.Key;
                            sscc = luid_sscc.Value;
                            luids.Add(luid);
                            ssccs.Add(sscc);
                            var defaultStatus = AppUtility.GetDefaultStatus();
                            var defaultUom = AppUtility.GetDefaultUom();

                            if (_log.IsDebugEnabled) _log.Debug($"_selectOrder.OutputLoc = {_selectOrder.OutputLoc}");
                            invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ItemCode, Convert.ToDouble(qtyPerCase), _prodRun, _selectOrder.OutputLoc, defaultStatus, _selectOrder.BatchNo, luid, sscc, defaultUom, _selectOrder.SAPOrderNo.ToString(), false, 0, _selectOrder.Shift, _selectOrder.Employee);
                        }

                        if (invReceipt.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                        so = AppData.IncrementProductionRun(_selectOrder.SAPOrderNo);
                        if (!so.SuccessFlag) throw new ApplicationException($"Error getting next batch. Error:{so.ServiceException}");
                        _prodRun = (int)so.ReturnValue;
                        _prodRun += 1;
                        PrintTubLabel(ssccs, qtyPerCase);
                    }
                }
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Tubs Produced", $"{numberOfCases} cases produced. Order: {_selectOrder.SAPOrderNo}");

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                DisplayToastNotification(ToastNotificationType.Error, "Produce", ex.Message, 10000);
            }
        }

        private void Scrap(int qty, string scrapReason)
        {
            try
            {
                ServiceOutput so;

                so = AppData.CreateSSCC();
                if (!so.SuccessFlag) throw new ApplicationException($"Error Creating SSCC. Error:{so.ServiceException}");
                var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
                luid = luid_sscc.Key;
                sscc = luid_sscc.Value;

                var userNamePW = AppUtility.GetUserNameAndPasswordMix(_selectOrder.ProductionMachineNo);
                var prodBatchNo = AppUtility.GetOrderBatchNoFromChar(_selectOrder.BatchNo[0]);
                _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(qty), 1);
                using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
                {
                    using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                    {
                        foreach (var plIssue in _plannedIssue.Where(i => i.BatchControlled)) // don't issue packaging material for scrap
                        {
                            if (plIssue.ShortQty == 0)
                            {
                                invIssue.AddOrderIssueLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.YJNOrder);
                            }
                            else
                            {
                                so = AppData.AddIssueShortage(_selectOrder.SAPOrderNo, plIssue.ItemCode, Convert.ToDecimal(plIssue.ShortQty));
                                if (!so.SuccessFlag) throw new ApplicationException($"Error adding shortage. Error:{so.ServiceException}");
                            }
                        }
                        if (_plannedIssue.Where(i => i.BatchControlled).Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                    }
                    using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                    {
                        MessageBox.Show($"qty = {qty}\nscrapLine = {_selectOrder.ScrapLine}");
                        invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ScrapItem, Convert.ToDouble(qty), prodBatchNo, AppUtility.GetScrapLocCode(), "RELEASED", "", luid, sscc, "Kgs", _selectOrder.BatchNo + "SCRAP", true, _selectOrder.ScrapLine, _selectOrder.Shift, _selectOrder.Employee, scrapReason);
                        if (invReceipt.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                    }
                }
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Scrap Produced", $"{qty} produced. Order: {_selectOrder.SAPOrderNo}");
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //DisplayToastNotification(ToastNotificationType.Error, "Scrap", ex.Message, 10000);
            }
        }

        private void PrintTubLabel(List<string> ssccs, int qtyPerCase)
        {
            try
            {
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();
                var fileNameTubCaseLabels = Path.Combine(labelPrintLoc, $"TubCaseLabel{Guid.NewGuid().ToString()}" + labelPrintExtension);
                var formatFilePathTubCaseLabel = AppUtility.GetPGDefaultTubCaseLabelFormat(); // Shouldn't be AppUtility.GetPGDefaultResmixLabelFormat();

                var sbMixLabel = new StringBuilder(5000);
                sbMixLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathTubCaseLabel, _selectOrder.Printer);
                sbMixLabel.AppendLine();
                sbMixLabel.Append(@"%END%");
                sbMixLabel.AppendLine();
                sbMixLabel.Append("Item, ItemName, IRMS, LotNo, SSCC, Qty");
                sbMixLabel.AppendLine();

                for (int i = 0; i < ssccs.Count; i++)
                {
                    sbMixLabel.AppendFormat("{0},{1},{2},{3},{4},{5}", _selectOrder.ItemCode, _selectOrder.ItemDescription, "", _selectOrder.BatchNo, ssccs[i], Convert.ToDecimal(qtyPerCase));
                    sbMixLabel.AppendLine();
                }

                using (StreamWriter sw = File.CreateText(fileNameTubCaseLabels))
                {
                    sw.Write(sbMixLabel.ToString());
                }
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Tub case label printed. Please check printer.");
                RefreshOrderInfo();

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }

        private void TubScrapUserControl1_ValidationFailed(object source, TubScrapValidationEventArgs e)
        {
            DisplayToastNotification(ToastNotificationType.Error, "Scrap", e.ValidationMessage);
        }

        private void TubScrapUserControl1_ScrapRequested(object source, TubScrapEventArgs e)
        {
            Scrap(e.ScrapQty, e.ScrapReason);
        }

        private void TubProductionUserControl1_ValidationFailed(object source, TubProductionValidationEventArgs e)
        {
            DisplayToastNotification(ToastNotificationType.Error, "Production", e.ValidationMessage);
        }


        public void DisplayToastNotification(ToastNotificationType type, string title, string text, int timeOut = 4000)
        {
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

            string html = AppUtility.GenerateHTMLToast(title, text, cssClass);

            int imgHeight = m_htmlToast.GetHTMLHeight(html) + 5;
            int imgWidth = this.Width - 25;

            m_htmlToast.SetImgSize(imgWidth, imgHeight);
            m_htmlToast.SetHTML(html);

            Rectangle rect = this.Bounds;
            Point px = new Point(rect.Left, rect.Bottom);
            Point screenLocation = PointToScreen(px);

            m_htmlToast.SetImgLocation(px.X + offset, px.Y - imgHeight - offset);

            m_htmlToast.Show(timeOut);
        }


        private void toLineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_orderChangeCausedComboSelectionChange)
                {
                    ProductionLineMachineNo selectedLineMachineNo = (ProductionLineMachineNo)toLineComboBox.SelectedItem;
                    StringBuilder sbDebugMsg = new StringBuilder();
                    sbDebugMsg.AppendLine("selectedLineMachineNo = ");
                    sbDebugMsg.AppendLine(selectedLineMachineNo.ToString());
                    if (_log.IsDebugEnabled) _log.Debug(sbDebugMsg.ToString());
                    if (selectedLineMachineNo.ProductionMachineNo != "0")
                    {
                        _selectOrder.ProductionLine = selectedLineMachineNo.ProductionLine;
                        _selectOrder.ProductionMachineNo = selectedLineMachineNo.ProductionMachineNo;
                        _selectOrder.InputLoc = selectedLineMachineNo.InputLocationCode;
                        _selectOrder.OutputLoc = selectedLineMachineNo.OutputLocationCode;
                        _selectOrder.Printer = selectedLineMachineNo.Printer;
                        _selectedLine = selectedLineMachineNo;
                        if (_log.IsDebugEnabled) _log.Debug($"_selectOrder.OutputLoc = {_selectOrder.OutputLoc}");
                        RefreshOrderInfo();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                DisplayToastNotification(ToastNotificationType.Error, "Line Combo SelectedIndexChanged", ex.Message, 10000);
            }
        }
    }
}
