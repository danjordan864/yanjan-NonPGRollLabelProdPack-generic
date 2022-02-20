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
    public partial class FrmMix : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        private RollLabelData _selectOrder = new RollLabelData();
        private BindingSource bindingSource1;
        private List<InventoryIssueDetail> _plannedIssue;
        private List<ProductionLine> _prodLines;
        private int luid;
        private string sscc;
        private int _prodRun;
        private bool _loading;
        public FrmMix()
        {
            InitializeComponent();
        }

        private void FrmMix_Load(object sender, EventArgs e)
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
            //set up scrap reason combo
            var so = AppData.GetProdLines(false);
            if (!so.SuccessFlag) throw new ApplicationException($"Failed to get prod lines. Error:{so.ServiceException}.");
            _prodLines = so.ReturnValue as List<ProductionLine>;
            var pls = _prodLines.Select(p => p.Code).ToList();
            pls.Insert(0, "Select To Line");
            cboToLine.DataSource = pls;
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
                frmSignInDialog.SetDataSource("MIX");
                DialogResult dr = frmSignInDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    _selectOrder = frmSignInDialog.SelectOrder;
                    txtWeightKgs.Text = "0";
                    //txtWeightKgs.Enabled = true;
                    cboToLine.Enabled = true;
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                    bindingSource1.DataSource = _selectOrder;
                }
            }
            var so = AppData.GetLastProductionRun(_selectOrder.SAPOrderNo);
            if (!so.SuccessFlag) throw new ApplicationException($"Error getting next batch. Error:{so.ServiceException}");
            _prodRun = (int)so.ReturnValue;
            _prodRun += 1;
        }


        private void txtWeightKgs_Validated(object sender, EventArgs e)
        {
            CheckReadyToProduce();
        }

        private void CheckReadyToProduce()
        {
            if (string.IsNullOrEmpty(txtWeightKgs.Text) || txtWeightKgs.Text == "0" || string.IsNullOrEmpty(txtBatch.Text) || cboToLine.Text == "Select To Line")
            {
                btnProduce.Enabled = false;
                cboToLine.Focus();
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Complete Form", "Please input Mix weight");
                return;
            }
            else
            {
                _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(txtWeightKgs.Text));
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
            }
        }

        private void Produce()
        {
            ServiceOutput so;
            so = AppData.CreateSSCC();
            if (!so.SuccessFlag) throw new ApplicationException($"Error Creating SSCC. Error:{so.ServiceException}");
            var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
            luid = luid_sscc.Key;
            sscc = luid_sscc.Value;
            var defaultStatus = AppUtility.GetDefaultStatus();
            var defaultUom = AppUtility.GetDefaultUom();

            var userNamePW = AppUtility.GetUserNameAndPasswordMix(_selectOrder.ProductionMachineNo);
            var prodBatchNo = Convert.ToInt32(txtBatch.Text.Substring(txtBatch.Text.LastIndexOf("-") + 1));
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
                var toProductionLineInputLoc = _prodLines.Where(p => p.Code == cboToLine.Text).Select(p => p.InputLocationCode).FirstOrDefault();

                using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                {
                    invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ItemCode, Convert.ToDouble(txtWeightKgs.Text), prodBatchNo, toProductionLineInputLoc, defaultStatus, txtBatch.Text, luid, sscc, defaultUom, "", false, 0, _selectOrder.Shift, _selectOrder.Employee);
                    if (invReceipt.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                    so = AppData.IncrementProductionRun(_selectOrder.SAPOrderNo);
                    if (!so.SuccessFlag) throw new ApplicationException($"Error getting next batch. Error:{so.ServiceException}");
                    _prodRun = (int)so.ReturnValue;
                    _prodRun += 1;
                }
            }
            DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Resmix Produced", $"#{txtWeightKgs.Text} kgs. produced. Order: {txtOrderNo.Text}");
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
                PrintResMixLabel();
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
            txtWeightKgs.Text = "0";
            txtWeightKgs.Enabled = false;
            cboToLine.Text = "Select To Line";
            lnkPlannedIssues.Enabled = false;
            btnProduce.Enabled = false;
            luid = 0;
            sscc = null;
        }

        private void PrintResMixLabel()
        {
            try
            {
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();
                var fileNameRollLabels = Path.Combine(labelPrintLoc, "ResMixLabel" + labelPrintExtension);
                var formatFilePathResmixLabel = AppUtility.GetPGDefaultResmixLabelFormat();

                var sbMixLabel = new StringBuilder(5000);
                sbMixLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathResmixLabel, _selectOrder.Printer);
                sbMixLabel.AppendLine();
                sbMixLabel.Append(@"%END%");
                sbMixLabel.AppendLine();
                sbMixLabel.Append("Item, ItemName, IRMS, LotNo, RollNo, SSCC, Qty");
                sbMixLabel.AppendLine();

                sbMixLabel.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", _selectOrder.ItemCode, _selectOrder.ItemDescription, "", "", txtBatch.Text, sscc, Convert.ToDecimal(txtWeightKgs.Text));

                using (StreamWriter sw = File.CreateText(fileNameRollLabels))
                {
                    sw.Write(sbMixLabel.ToString());
                }
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "ResMix label printed. Please check printer.");
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

        private void cboToLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboToLine.Text != "Select To Line")
            {
                var resmixTo001 = AppUtility.GetResmix001ToLines();
                var resmixTo002 = AppUtility.GetResmix002ToLines();
                if (_selectOrder.ItemCode.ToUpper() == "RESMIX-001" && resmixTo001.IndexOf(cboToLine.Text) == -1)
                {
                    DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Select Line", $"{_selectOrder.ItemCode} cannot be selected for line {cboToLine.Text}");
                    cboToLine.Text = "Select To Line";
                    return;
                }
                if (_selectOrder.ItemCode.ToUpper() == "RESMIX-002" && resmixTo002.IndexOf(cboToLine.Text) == -1)
                {
                    DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Select Line", $"{_selectOrder.ItemCode} cannot be selected for line {cboToLine.Text}");
                    cboToLine.Text = "Select To Line";
                    return;
                }
                var lineNo = _prodLines.Where(p => p.Code == cboToLine.Text).Select(p => p.LineNo).FirstOrDefault();
                txtBatch.Text = $"{_selectOrder.SAPOrderNo.ToString()}-{lineNo}{_prodRun.ToString("000")}";
                txtWeightKgs.Enabled = true;
            }
            else
            {
                txtWeightKgs.Enabled = false;
            }

        }

    }
}
