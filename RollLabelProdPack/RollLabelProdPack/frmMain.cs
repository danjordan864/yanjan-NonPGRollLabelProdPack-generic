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
    public partial class FrmMain : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        private RollLabelData _selectOrder = new RollLabelData();
        private List<Roll> _rolls = null;
        private BindingSource bindingSource1;
        private List<InventoryIssueDetail> _plannedIssue;
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            bindingSource1 = new BindingSource();
            bindingSource1.DataSource = _selectOrder;
            btnCreate.Enabled = false;
            btnGenerateRolls.Enabled = false;
            txtEmployee.DataBindings.Add("Text", bindingSource1, "Employee");
            txtYJNProdOrder.DataBindings.Add("Text", bindingSource1, "YJNOrder");
            txtOrderNo.DataBindings.Add("Text", bindingSource1, "SAPOrderNo");
            txtShift.DataBindings.Add("Text", bindingSource1, "Shift");
            txtProductionLine.DataBindings.Add("Text", bindingSource1, "ProductionLine");
            txtNoOfSlits.DataBindings.Add("Text", bindingSource1, "NoOfSlits");
            txtDie.DataBindings.Add("Text", bindingSource1, "AperatureDieNo");
            txtItemCode.DataBindings.Add("Text", bindingSource1, "ItemCode");
            txtItemName.DataBindings.Add("Text", bindingSource1, "ItemDescription");
            txtJumboRoll.DataBindings.Add("Text", bindingSource1, "JumboRollNo");
            txtIRMS.DataBindings.Add("Text", bindingSource1, "IRMS");
        }


        private void ShowSlitPos(int noSlitPos)
        {
            chkSP1.Visible = noSlitPos > 0;
            chkSP2.Visible = noSlitPos > 1;
            chkSP3.Visible = noSlitPos > 2;
            chkSP4.Visible = noSlitPos > 3;
            chkSP5.Visible = noSlitPos > 4;
            chkSP6.Visible = noSlitPos > 5;
            chkSP7.Visible = noSlitPos > 6;
            chkSP8.Visible = noSlitPos > 7;
            chkSP9.Visible = noSlitPos > 8;
            chkSP10.Visible = noSlitPos > 9;
            chkSP11.Visible = noSlitPos > 10;
            chkSP12.Visible = noSlitPos > 11;
            chkSP13.Visible = noSlitPos > 12;
            chkSP14.Visible = noSlitPos > 13;
            chkSP15.Visible = noSlitPos > 14;
            txtDie.Enabled = true;
            txtWeightKgs.Enabled = true;
        }

        private void CheckReadyToProduce()
        {
            try
            {
                olvRolls.Objects = null;
                _rolls = null;
                if (txtNoOfSlits.Text == "0" || string.IsNullOrEmpty(txtNoOfSlits.Text) || string.IsNullOrEmpty(txtDie.Text) || string.IsNullOrEmpty(txtWeightKgs.Text) || txtWeightKgs.Text == "0")
                {
                    btnGenerateRolls.Enabled = false;
                    DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Complete Form", "Please input values No. Of Slits,  Die No. and first roll weight.");
                    return;
                }
                else
                {
                    _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(txtWeightKgs.Text) * Convert.ToDecimal(txtNoOfSlits.Text));
                    var hasShortage = _plannedIssue.Where(i => i.ShortQty > 0 && i.BatchControlled);
                    if (hasShortage.Count() > 0)
                    {
                        using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
                        {
                            frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                            DialogResult dr = frmPlannedIssues.ShowDialog();
                        }
                    }
                    btnGenerateRolls.Enabled = true;
                    lnkPlannedIssues.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, $"Check Ready to Produce Error", $"Error Check Ready to Produce: {ex.Message}");
            }
            
        }


        private void GenerateRolls()
        {
            RefreshProdDate();

            if (txtNoOfSlits.Text == "0" || string.IsNullOrEmpty(txtNoOfSlits.Text))
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Complete Form", "Please input values No. Of Slits,  Die No. and first roll weight.");
                return;
            }
            _rolls = new List<Roll>();
            try
            {
                for (int i = 1; i <= Convert.ToInt32(txtNoOfSlits.Text); i++)
                {
                    var slitPosCheckBox = this.splitContainer1.Panel1.Controls.OfType<CheckBox>().Where(c => c.Tag.ToString() == i.ToString()).FirstOrDefault();
                    if (slitPosCheckBox.Visible && slitPosCheckBox.Checked)
                    {
                        var roll = new Roll
                        {
                            RollNo = $"{_selectOrder.ProductionYear}{_selectOrder.ProductionMonth}{_selectOrder.ProductionDate}{_selectOrder.AperatureDieNo}".ToUpper() +
                            $"{_selectOrder.Shift}{_selectOrder.JumboRollNo.ToString("00")}{(i).ToString("00")}".ToUpper(),
                            YJNOrder = _selectOrder.YJNOrder,
                            ItemCode = _selectOrder.ItemCode,
                            ItemName = _selectOrder.ItemDescription,
                            IRMS = _selectOrder.IRMS,
                            Kgs = Convert.ToDecimal(txtWeightKgs.Text)
                        };
                        var so = AppData.CheckRoll(roll.RollNo);
                        if (!so.SuccessFlag) throw new ApplicationException($"Error on Check Roll: {roll.RollNo}");
                        var count = (Int32)so.ReturnValue;
                        if (count > 0) throw new ApplicationException($"Roll {roll.RollNo} has already been created.");
                        _rolls.Add(roll);
                    }
                }
                olvRolls.SetObjects(_rolls);
                btnCreate.Enabled = true;
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, $"Roll No. Error", $"Error creating rolls: {ex.Message}");
                AppUtility.WriteToEventLog($"Error creating rolls: {ex.Message}", System.Diagnostics.EventLogEntryType.Error, AppUtility.GetEmailErrors());
            }

        }

        private void ChangeOrder()
        {
            using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
            {
                frmSignInDialog.SetDataSource(104);
                DialogResult dr = frmSignInDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    _selectOrder = frmSignInDialog.SelectOrder;
                    txtWeightKgs.Text = "0";
                    txtNoOfSlits.Enabled = true;
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                    if (_selectOrder.NoOfSlits > 0) ShowSlitPos(_selectOrder.NoOfSlits);
                    bindingSource1.DataSource = _selectOrder;
                    if (_selectOrder.ScrapItem == null)
                        DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Scrap setup issue", "A scrap item needs to be set on the order to record scrap.");
                }
            }
        }

        private void testSQLConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var connection = AppUtility.TestSQLConnection();
            Cursor = Cursors.Default;
            if (connection.Length == 0) { DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Successful SQL Connection"); }
            else { DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Error", connection); }
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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }

        private void testSAPB1ConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                SAPB1 sap = new SAPB1();
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Test SAP B1 Connection", "Test Completed Successfully!");
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Test Failed!\n\n[Exception Message]\n{ex.Message}");
            }
            Cursor = Cursors.Default;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                Produce();
                PrintRollLabels();
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }

        private void Produce()
        {
            ServiceOutput so;

            foreach (var roll in _rolls)
            {
                so = AppData.CreateSSCC();
                if (!so.SuccessFlag) throw new ApplicationException($"Error Creating SSCC. Error:{so.ServiceException}");
                var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
                roll.LUID = luid_sscc.Key;
                roll.SSCC = luid_sscc.Value;
            }
            var userNamePW = AppUtility.GetUserNameAndPasswordFilm(_selectOrder.ProductionMachineNo);

            using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
            {
                using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                {
                    foreach (var plIssue in _plannedIssue)
                    {
                        if (plIssue.ShortQty == 0)
                        {
                            invIssue.AddLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.YJNOrder);
                        }
                        else
                        {
                            so = AppData.AddIssueShortage(_selectOrder.SAPOrderNo, plIssue.ItemCode, Convert.ToDecimal(plIssue.ShortQty));
                            if (!so.SuccessFlag) throw new ApplicationException($"Error adding shortage. Error:{so.ServiceException}");
                        }
                    }
                    if (_plannedIssue.Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                }
                using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                {
                    foreach (var roll in _rolls)
                    {
                        //receive each roll
                        so = AppData.AddRoll(roll.RollNo, _selectOrder.SAPOrderNo, roll.YJNOrder, roll.SSCC, roll.Kgs, _selectOrder.Employee);
                        if (!so.SuccessFlag) DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Error Adding Roll", $"Error adding roll#: {roll.RollNo} to SAP @SII_ROLLS. Error:{so.ServiceException}");
                    }
                    var stdProduction = _rolls.Where(r => !r.Scrap);
                    foreach (var roll in stdProduction)
                    {
                        invReceipt.AddLine(_selectOrder.SAPDocEntry, roll.ItemCode, Convert.ToDouble(roll.Kgs), _selectOrder.OutputLoc, "RELEASED", roll.RollNo, roll.LUID, roll.SSCC, "Kgs", _selectOrder.YJNOrder, false, 0);
                    }
                    var scrapProd = _rolls.Where(r => r.Scrap && _selectOrder.ScrapItem != null);
                    foreach (var roll in scrapProd)
                    {
                        invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ScrapItem, Convert.ToDouble(roll.Kgs), _selectOrder.OutputLoc, "RELEASED", roll.RollNo, roll.LUID, roll.SSCC, "Kgs", _selectOrder.YJNOrder, true, _selectOrder.ScrapLine);
                    }
                    if (invReceipt.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                }

            }
            so = AppData.UpdateJumboRoll(_selectOrder.SAPOrderNo);
            if (!so.SuccessFlag) throw new ApplicationException($"Error updating jumbo roll. Error:{so.ServiceException}");
            _selectOrder.JumboRollNo = (int)so.ReturnValue;
            txtJumboRoll.Text = _selectOrder.JumboRollNo.ToString();
            DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Rolls Produced", $"#{_rolls.Count.ToString()} rolls produced. Order: {txtYJNProdOrder.Text}, Roll No. {txtJumboRoll.Text}");

        }

        private void txtNoOfSlits_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtWeightKgs_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnGenerateRolls_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateRolls();
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} GenerateRolls Click.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} GenerateRolls Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
            RefreshProdDate();

        }

        private void RefreshProdDate()
        {
            var currentDateTime = DateTime.Now;
            txtProductionDateFull.Text = currentDateTime.ToShortDateString();
            _selectOrder.ProductionYear = currentDateTime.Year.ToString().Last().ToString();
            _selectOrder.ProductionMonth = AppUtility.GetYanJanProdMo(currentDateTime);
            _selectOrder.ProductionDate = currentDateTime.Day.ToString("00");

        }
        private void txtNoOfSlits_Validated(object sender, EventArgs e)
        {
            var noOfSlits = Convert.ToInt32(txtNoOfSlits.Text);
            if (noOfSlits > 0) ShowSlitPos(noOfSlits);
        }

        private void PrintRollLabels()
        {
            try
            {
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();
                var fileNameRollLabels = Path.Combine(labelPrintLoc, "RollLabels" + labelPrintExtension);
                var formatFilePathCombRollLabel = AppUtility.GetPGDefaultCombLabelFormat();

                var sbRollLabel = new StringBuilder(5000);
                sbRollLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathCombRollLabel, _selectOrder.Printer);
                sbRollLabel.AppendLine();
                sbRollLabel.Append(@"%END%");
                sbRollLabel.AppendLine();
                sbRollLabel.Append("Item, ItemName, IRMS, LotNo, RollNo, SSCC, Qty");
                sbRollLabel.AppendLine();
                foreach (var roll in _rolls)
                {
                    sbRollLabel.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", roll.Scrap ? _selectOrder.ScrapItem : roll.ItemCode, roll.Scrap ? "Scrap" : roll.ItemName, roll.IRMS, roll.YJNOrder, roll.RollNo, roll.SSCC, roll.Kgs);
                    sbRollLabel.AppendLine();
                }
                using (StreamWriter sw = File.CreateText(fileNameRollLabels))
                {
                    sw.Write(sbRollLabel.ToString());
                }
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Roll labels printed. Please check printer.");
                olvRolls.Objects = null;
                _rolls = null;
                txtWeightKgs.Text = "0";
                lnkPlannedIssues.Enabled = false;
                btnGenerateRolls.Enabled = false;
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }

        private void reprintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (FrmReprintOrderRollsDialog frmReprintDialog = new FrmReprintOrderRollsDialog())
                {
                    frmReprintDialog.YJNOrderNo = _selectOrder.YJNOrder;
                    DialogResult dr = frmReprintDialog.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        _rolls = frmReprintDialog.SelectedRolls;
                        PrintRollLabels();
                        _rolls = null;
                        DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Check Printer");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} RePrintRollLabels.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }

        }

        private void txtWeightKgs_Validated(object sender, EventArgs e)
        {
            bool enable = txtWeightKgs.Text == "0";
            var slitPosCheckBoxes = this.splitContainer1.Panel1.Controls.OfType<CheckBox>();
            foreach (var checkbox in slitPosCheckBoxes)
            {
                checkbox.Enabled = enable;
            }
            CheckReadyToProduce();
        }

        private void txtDie_Validated(object sender, EventArgs e)
        {
            CheckReadyToProduce();
        }

        private void lnkPlannedIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
            {
                frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                DialogResult dr = frmPlannedIssues.ShowDialog();
            }
        }

        private void boxScrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBoxScrap frmBoxScrap = new FrmBoxScrap();
            frmBoxScrap.Show();
        }

        private void adjustResmixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAdjustResmix frmAdjustResmix = new FrmAdjustResmix();
            frmAdjustResmix.Show();
        }
    }
}