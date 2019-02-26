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
    
    public partial class FrmBoxScrap : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        private RollLabelData _selectOrder = new RollLabelData();
        private BindingSource bindingSource1;
        private List<InventoryIssueDetail> _plannedIssue;
        private int luid;
        private string sscc;
        public FrmBoxScrap()
        {
            InitializeComponent();
        }

        private void FrmBoxScrap_Load(object sender, EventArgs e)
        {
            bindingSource1 = new BindingSource();
            bindingSource1.DataSource = _selectOrder;
            txtEmployee.DataBindings.Add("Text", bindingSource1, "Employee");
            txtOrderNo.DataBindings.Add("Text", bindingSource1, "SAPOrderNo");
            txtShift.DataBindings.Add("Text", bindingSource1, "Shift");
            txtProductionLine.DataBindings.Add("Text", bindingSource1, "ProductionLine");
            txtItemCode.DataBindings.Add("Text", bindingSource1, "ItemCode");
            txtItemName.DataBindings.Add("Text", bindingSource1, "ItemDescription");
            //set up scrap reason combo
            var so = AppData.GetScrapReasons();
            if (!so.SuccessFlag) throw new ApplicationException($"Failed to get scrap reasons. Error:{so.ServiceException}.");
            var scrapReasons = so.ReturnValue as List<string>;
            cboScrapReason.DataSource = scrapReasons;
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }

        private void ChangeOrder()
        {
            using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
            {
                frmSignInDialog.SetDataSource(104); //pass in item group 103 = resmix, 104=formedfilm
                DialogResult dr = frmSignInDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    _selectOrder = frmSignInDialog.SelectOrder;
                    txtWeightKgs.Text = "0";
                    txtWeightKgs.Enabled = true;
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                    bindingSource1.DataSource = _selectOrder;
                }
            }
        }


        private void txtWeightKgs_Validated(object sender, EventArgs e)
        {
            CheckReadyToProduce();
        }

        private void CheckReadyToProduce()
        {
            if (string.IsNullOrEmpty(_selectOrder.ScrapItem))
            {
                btnScrap.Enabled = false;
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "No Scrap Item", "Production Order must have scrap item.");
                return;
            }
            else if (string.IsNullOrEmpty(txtWeightKgs.Text) || txtWeightKgs.Text == "0" )
            {
                btnScrap.Enabled = false;
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Complete Form", "Please input scrap weight.");
                return;
            }
            else
            {
                _plannedIssue =  AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(txtWeightKgs.Text));
                var hasShortage = _plannedIssue.Where(i => i.ShortQty > 0 && i.BatchControlled);
                if (hasShortage.Count() >0)
                {
                    using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
                    {
                        frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                        DialogResult dr = frmPlannedIssues.ShowDialog();
                    }
                }
                btnScrap.Enabled = true;
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

            var userNamePW = AppUtility.GetUserNameAndPasswordFilm(_selectOrder.ProductionMachineNo);
            var prodBatchNo = AppUtility.GetOrderBatchNoFromChar(_selectOrder.BatchNo[0]);
            using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
            {
                using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                {
                    foreach (var plIssue in _plannedIssue.Where(i=>i.BatchControlled)) // don't issue packaging material for box scrap
                    {
                        if(plIssue.ShortQty == 0)
                        {
                            invIssue.AddOrderIssueLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.YJNOrder);
                        }
                        else
                        {
                            so = AppData.AddIssueShortage(_selectOrder.SAPOrderNo, plIssue.ItemCode, Convert.ToDecimal(plIssue.ShortQty));
                            if (!so.SuccessFlag) throw new ApplicationException($"Error adding shortage. Error:{so.ServiceException}");
                        }
                    }
                    if (_plannedIssue.Where(i=>i.BatchControlled).Sum(q=>q.PlannedIssueQty) > 0 && invIssue.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                }
                using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                {
                    invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ScrapItem, Convert.ToDouble(txtWeightKgs.Text), prodBatchNo,AppUtility.GetScrapLocCode(), "RELEASED", "", luid, sscc, "Kgs", _selectOrder.YJNOrder, true, _selectOrder.ScrapLine,_selectOrder.Shift,_selectOrder.Employee,cboScrapReason.Text);
                    if (invReceipt.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                }
            }
            DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Box Scrap Produced", $"#{txtWeightKgs.Text} kgs. produced. Order: {txtOrderNo.Text}");
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
            Produce();
            PrintBoxScrapLabel();
            txtWeightKgs.Text = "0";
            lnkPlannedIssues.Enabled = false;
            btnScrap.Enabled = false;
            luid = 0;
            sscc = null;
        }
        private void PrintBoxScrapLabel()
        {
            try
            {
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();
                var fileNameRollLabels = Path.Combine(labelPrintLoc, "BoxScrapLabel" + labelPrintExtension);
                var formatFilePathLabel = AppUtility.GetPGDefaultScrapLabelFormat();

                var sbMixLabel = new StringBuilder(5000);
                sbMixLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathLabel, _selectOrder.Printer);
                sbMixLabel.AppendLine();
                sbMixLabel.Append(@"%END%");
                sbMixLabel.AppendLine();
                sbMixLabel.Append("Item, ItemName, IRMS, LotNo, RollNo, SSCC, Qty");
                sbMixLabel.AppendLine();

                sbMixLabel.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", _selectOrder.ScrapItem, _selectOrder.ScrapItemName, "", _selectOrder.YJNOrder, "", sscc, Convert.ToDecimal(txtWeightKgs.Text));

                using (StreamWriter sw = File.CreateText(fileNameRollLabels))
                {
                    sw.Write(sbMixLabel.ToString());
                }
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Scrap label printed. Please check printer.");
                txtWeightKgs.Text = "0";
                lnkPlannedIssues.Enabled = false;
                btnScrap.Enabled = false;
                luid = 0;
                sscc = null;
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

       

    }
}
