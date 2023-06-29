using RollLabelProdPack.Library.Data;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;
using RollLabelProdPack.SAP.B1;
using RollLabelProdPack.SAP.B1.DocumentObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormUtils;

namespace RollLabelProdPack
{
    /// <summary>
    /// Partial class representing a tub production form.
    /// </summary>
    /// <remarks>
    /// Deprecated; <see cref="frmTub2"/>
    /// </remarks>
    public partial class FrmTub : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        private RollLabelData _selectOrder = new RollLabelData();
        private BindingSource bindingSource1;
        private List<InventoryIssueDetail> _plannedIssue;
        //private List<ProductionLine> _prodLines;
        private int luid;
        private string sscc;
        private int _prodRun;
        private bool _loading;
        private List<ProductionLineMachineNo> _prodLines;
        private BindingSource prodLinesBindingSource;
        private bool _orderChangeCausedComboSelectionChange = false;
        private string _currentShift;
        private string _currentEmployee;
        private ProductionLineMachineNo _selectedLine;
        private string _scrapItem = "N/A";
        private BindingSource _frmTubBindingSource;
        private int _scrapQty;

        public FrmTub()
        {
            InitializeComponent();
        }

        public string ScrapItem {
            get {
                if (string.IsNullOrEmpty(_scrapItem))
                    return "N/A";
                else
                    return _scrapItem;
            }
            set
            {
                _scrapItem = value;
            }
        }

        private void FrmTub_Load(object sender, EventArgs e)
        {
            _loading = true;
            bindingSource1 = new BindingSource();
            bindingSource1.DataSource = _selectOrder;
            txtEmployee.DataBindings.Add("Text", bindingSource1, "Employee");
            txtOrderNo.DataBindings.Add("Text", bindingSource1, "SAPOrderNo");
            txtShift.DataBindings.Add("Text", bindingSource1, "Shift");
            txtProductionLine.DataBindings.Add("Text", bindingSource1, "ProductionLine");
            txtItemCode.DataBindings.Add("Text", bindingSource1, "ItemCode");
            txtItemName.DataBindings.Add("Text", bindingSource1, "ItemDescription");
            casesProducedLabel.DataBindings.Add("Text", bindingSource1, "InvRolls");
            maxCasesLabel.DataBindings.Add("Text", bindingSource1, "TargetRolls");

            _frmTubBindingSource = new BindingSource();
            _frmTubBindingSource.DataSource = this;
            scrapItemLabel.DataBindings.Add("Text", _frmTubBindingSource, "ScrapItem");

            ServiceOutput so = AppData.GetProdLines(false);
            if (!so.SuccessFlag) throw new ApplicationException($"Error getting production lines: {so.ServiceException}");
            List<ProductionLine> tubLines = ((List<ProductionLine>)so.ReturnValue).Where(t => t.Code.StartsWith("TUB")).ToList();
            _prodLines = tubLines.Select(t => new ProductionLineMachineNo { ProductionLine = t.Code, ProductionMachineNo = t.LineNo, InputLocationCode = t.InputLocationCode, OutputLocationCode = t.OutputLocationCode, Printer = t.Printer }).OrderBy(t => t.ProductionLine).ToList();
            _prodLines.Insert(0, new ProductionLineMachineNo { ProductionLine = "<Select Line>", ProductionMachineNo = "0" });
            toLineComboBox.DataSource = _prodLines;
            toLineComboBox.DisplayMember = "ProductionLine";
            _loading = false;
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }

        private void ChangeOrder()
        {
            using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
            {
                frmSignInDialog.SetDataSource("TUB");
                DialogResult dr = frmSignInDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    _selectOrder = frmSignInDialog.SelectOrder;
                    _currentShift = _selectOrder.Shift;
                    _currentEmployee = _selectOrder.Employee;
                    txtQty.Text = "0";
                    //txtWeightKgs.Enabled = true;
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                    bindingSource1.DataSource = _selectOrder;
                    _orderChangeCausedComboSelectionChange = true;
                    toLineComboBox.Enabled = true;
                    toLineComboBox.SelectedItem = _prodLines.First(t => t.ProductionLine == _selectOrder.ProductionLine);
                    _selectedLine = (ProductionLineMachineNo)toLineComboBox.SelectedItem;
                    _orderChangeCausedComboSelectionChange = false;
                }
            }
            RefreshOrderInfo();

        }


        private void txtQty_Validated(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNumberOfCases.Text))
            {
                CheckReadyToProduce();
            }
        }

        private void CheckReadyToProduce()
        {
            string validationMessage = string.Empty;
            btnProduce.Enabled = false;
            lnkPlannedIssues.Enabled = false;
            int qty = 0;
            int numberOfCases = 0;
            bool invalidQty = false;
            bool invalidNumberOfCases = false;

            if (string.IsNullOrEmpty(txtQty.Text))
            {
                invalidQty = true;
                validationMessage = "Quantity is required";
            }

            if (!invalidQty && !int.TryParse(txtQty.Text, out qty))
            {
                invalidQty = true;
                validationMessage = "Quantity must be a number";
            }

            if (!invalidQty && qty <= 0)
            {
                invalidQty = true;
                validationMessage = "Quantity must be greater than 0";
            }

            if (invalidQty)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Invalid quantity", validationMessage);
                txtQty.Focus();
                txtQty.SelectAll();
                return;
            }

            if (string.IsNullOrEmpty(txtNumberOfCases.Text))
            {
                invalidNumberOfCases = true;
                validationMessage = "Number of cases is required";
            }

            if (!invalidNumberOfCases && !int.TryParse(txtNumberOfCases.Text, out numberOfCases))
            {
                invalidNumberOfCases = true;
                validationMessage = "Number of cases must be a number";
            }

            if (!invalidNumberOfCases && numberOfCases <= 0)
            {
                invalidNumberOfCases = true;
                validationMessage = "Number of cases must be greater than 0";
            }

            if (invalidNumberOfCases)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Invalid number of cases", validationMessage);
                txtNumberOfCases.Focus();
                txtNumberOfCases.SelectAll();
                return;
            }

            //if (string.IsNullOrEmpty(txtQty.Text) || txtQty.Text == "0" || string.IsNullOrEmpty(txtBatch.Text) || Convert.ToDecimal(txtQty.Text)%200 !=0)
            //{
            //    btnProduce.Enabled = false;
            //    //cboToLine.Focus();
            //    DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Complete Form", "Please input Quantity (Increments of 200)");
            //    return;
            //}
            //else
            //{
            _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, (decimal)(qty * numberOfCases));
            var hasShortage = _plannedIssue.Where(i => i.ShortQty > 0 && i.BatchControlled);
            if (hasShortage.Count() > 0)
            {
                using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
                {
                    frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                    DialogResult dr = frmPlannedIssues.ShowDialog();
                }
            }
            btnProduce.Enabled = true;
            lnkPlannedIssues.Enabled = true;
            //}
        }

        private void Produce()
        {
            int qty = 0;
            int numberOfCases = 0;
            var userNamePW = AppUtility.GetUserNameAndPasswordMix(_selectOrder.ProductionMachineNo);
            var prodBatchNo = _prodRun;
            //var prodBatchNo =  Convert.ToInt32(txtBatch.Text.Substring(txtBatch.Text.LastIndexOf("-") + 1));
            ServiceOutput so;
            if (int.TryParse(txtQty.Text, out qty) && int.TryParse(txtNumberOfCases.Text, out numberOfCases))
            {
                using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
                {
                    using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                    {
                        foreach (var plIssue in _plannedIssue)
                        {
                            if (plIssue.ShortQty == 0)
                            {
                                invIssue.AddOrderIssueLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.SAPOrderNo.ToString());
                            }
                            else
                            {
                                so = AppData.AddIssueShortage(_selectOrder.SAPOrderNo, plIssue.ItemCode, Convert.ToDecimal(plIssue.ShortQty));
                                if (!so.SuccessFlag) throw new ApplicationException($"Error adding shortage. Error:{so.ServiceException}");
                            }
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

                            //var userNamePW = AppUtility.GetUserNameAndPasswordMix(_selectOrder.ProductionMachineNo);
                            //var prodBatchNo = Convert.ToInt32(txtBatch.Text.Substring(txtBatch.Text.LastIndexOf("-") + 1));
                            //using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
                            //{
                            //var toProductionLineInputLoc = "TUBFIN1";//_prodLines.Where(p => p.Code == cboToLine.Text).Select(p => p.InputLocationCode).FirstOrDefault();

                            invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ItemCode, Convert.ToDouble(txtQty.Text), prodBatchNo, _selectOrder.OutputLoc, defaultStatus, txtBatch.Text, luid, sscc, defaultUom, _selectOrder.SAPOrderNo.ToString(), false, 0, _selectOrder.Shift, _selectOrder.Employee);

                            //}

                        }

                        if (invReceipt.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                        so = AppData.IncrementProductionRun(_selectOrder.SAPOrderNo);
                        if (!so.SuccessFlag) throw new ApplicationException($"Error getting next batch. Error:{so.ServiceException}");
                        _prodRun = (int)so.ReturnValue;
                        _prodRun += 1;
                        PrintTubLabel(ssccs);

                    }
                }

            }
            DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Tubs Produced", $"{txtNumberOfCases.Text} cases produced. Order: {txtOrderNo.Text}");
        }

        private void lnkPlannedIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
            {
                frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                DialogResult dr = frmPlannedIssues.ShowDialog();
            }
        }

        private void btnProduce_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Produce();
                //PrintTubLabel();
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Produce Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
            finally
            {
                RefreshOrderInfo();
                Cursor.Current = Cursors.Default;
            }
        }

        private void RefreshOrderInfo()
        {
            var so = AppData.GetProdOrder(_selectOrder.SAPOrderNo);
            if (!so.SuccessFlag) throw new ApplicationException($"Error refreshing order information. Error:{so.ServiceException}");
            _selectOrder = (RollLabelData)so.ReturnValue;
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
            }
            bindingSource1.DataSource = _selectOrder;
            ScrapItem = _selectOrder.ScrapItem;
            bool enableScrapControls = ScrapItem != "N/A";
            scrapQtyTextBox.Enabled = enableScrapControls;
            scrapButton.Enabled = enableScrapControls;
            _frmTubBindingSource.ResetBindings(false);

            txtQty.Text = "0";
            txtQty.Enabled = true;
            txtNumberOfCases.Enabled = true;
            lnkPlannedIssues.Enabled = false;
            btnProduce.Enabled = false;
            luid = 0;
            sscc = null;
            so = AppData.GetLastProductionRun(_selectOrder.SAPOrderNo);
            if (!so.SuccessFlag) throw new ApplicationException($"Error getting next batch. Error:{so.ServiceException}");
            _prodRun = (int)so.ReturnValue;
            _prodRun += 1;
            txtBatch.Text = $"{_selectOrder.SAPOrderNo.ToString()}{_selectOrder.ProductionLine.Replace("TUB","T")}";
        }

        private void PrintTubLabel(List<string> ssccs)
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
                    sbMixLabel.AppendFormat("{0},{1},{2},{3},{4},{5}", _selectOrder.ItemCode, _selectOrder.ItemDescription, "", txtBatch.Text, ssccs[i], Convert.ToDecimal(txtQty.Text));
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
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
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

        private void txtBatch_Validated(object sender, EventArgs e)
        {
            CheckReadyToProduce();
        }

        private void txtNumberOfCases_Validated(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtQty.Text))
            {
                CheckReadyToProduce();
            }
        }

        private void toLineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_orderChangeCausedComboSelectionChange)
            {
                ProductionLineMachineNo selectedLineMachineNo = (ProductionLineMachineNo)toLineComboBox.SelectedItem;
                if (selectedLineMachineNo.ProductionMachineNo != "0")
                {
                    _selectOrder.ProductionLine = selectedLineMachineNo.ProductionLine;
                    _selectOrder.ProductionMachineNo = selectedLineMachineNo.ProductionMachineNo;
                    _selectOrder.InputLoc = selectedLineMachineNo.InputLocationCode;
                    _selectOrder.OutputLoc = selectedLineMachineNo.OutputLocationCode;
                    _selectOrder.Printer = selectedLineMachineNo.Printer;
                    bindingSource1.ResetBindings(false);
                    _selectedLine = selectedLineMachineNo;
                    RefreshOrderInfo();
                }
            }
        }

        private void scrapQtyTextBox_Validating(object sender, CancelEventArgs e)
        {
            //scrapButton.Enabled = false;
            if (!string.IsNullOrEmpty(scrapQtyTextBox.Text))
            {
                if (int.TryParse(scrapQtyTextBox.Text, out _scrapQty))
                {
                    if (_scrapQty > 0)
                    {
                        //scrapButton.Enabled = true;
                    }
                    else
                    {
                        DisplayToastNotification(ToastNotificationType.Error, "Invalid scrap qty", "Please enter an integer > 0 for scrap qty");
                        scrapQtyTextBox.Focus();
                        scrapQtyTextBox.SelectAll();
                        e.Cancel = true;
                    }
                }
                else
                {
                    DisplayToastNotification(ToastNotificationType.Error, "Invalid scrap qty", "Please enter an integer for scrap qty");
                    scrapQtyTextBox.Focus();
                    scrapQtyTextBox.SelectAll();
                    e.Cancel = true;
                }
            }
            else
            {
                DisplayToastNotification(ToastNotificationType.Error, "No scrap qty entered", "Please enter a valid scrap qty");
                scrapQtyTextBox.Focus();
                e.Cancel = true;
            }
        }

        private void scrapButton_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                DisplayToastNotification(ToastNotificationType.Success, "Button clicked", $"You clicked the scrap button. Scrap qty is: {_scrapQty}");
            }
        }

        private void FrmTub_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
