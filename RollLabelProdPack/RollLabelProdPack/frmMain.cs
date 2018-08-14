using RollLabelProdPack.Library.Data;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;
using RollLabelProdPack.SAP.B1;
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
        private RollLabelData _selectOrder = null;
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
            btnCreate.Enabled = false;
            btnGenerateRolls.Enabled = false;
            ChangeOrder();
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

        }

        private bool EnableGenerate()
        {
            if (string.IsNullOrEmpty(txtDie.Text) || string.IsNullOrEmpty(txtWeightKgs.Text) || txtWeightKgs.Text == "0")
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Missing Fields", "Must enter Die No. and Quantity");
                btnGenerateRolls.Enabled = false;
                return false;
            }
            else
            {
                btnGenerateRolls.Enabled = true;
                return true;
            }
        }

        private bool EnableProduce()
        {
            var hasShortage = _plannedIssue.Where(i => i.ShortQty > 0).Count() > 0;
            if (hasShortage)
            {
                using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
                {
                    frmPlannedIssues.SetDataSource(_plannedIssue);
                    DialogResult dr = frmPlannedIssues.ShowDialog();
                }
                btnCreate.Enabled = false;
                return false;
            }
            else if (_rolls.Count == 0)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Missing Fields", "Must enter Die No. and Quantity");
                btnCreate.Enabled = false;
                return false;
            }
            else
            {
                btnCreate.Enabled = true;
                return true;
            }
        }
        private void GenerateRolls()
        {
            _rolls = new List<Roll>();

            for (int i = 1; i <= Convert.ToInt32(txtNoOfSlits.Text); i++)
            {
                var slitPosCheckBox = this.splitContainer1.Panel1.Controls.OfType<CheckBox>().Where(c => c.Tag.ToString() == i.ToString()).FirstOrDefault();
                if (slitPosCheckBox.Visible && slitPosCheckBox.Checked)
                {
                    var roll = new Roll
                    {
                        RollNo = $"{_selectOrder.ProductionYear}{_selectOrder.ProductionMonth}{_selectOrder.ProductionDate}{_selectOrder.AperatureDieNo}" +
                        $"{_selectOrder.Shift}{_selectOrder.JumboRollNo.ToString("00")}{(i).ToString("00")}",
                        YJNOrder = _selectOrder.YJNOrder,
                        ItemCode = _selectOrder.ItemCode,
                        ItemName = _selectOrder.ItemDescription,
                        IRMS = _selectOrder.IRMS,
                        SSCC = "",
                        Kgs = Convert.ToDecimal(txtWeightKgs.Text)
                    };
                    _rolls.Add(roll);
                }
            }
            olvRolls.SetObjects(_rolls);
        }

        private void ChangeOrder()
        {
            using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
            {
                DialogResult dr = frmSignInDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    _selectOrder = frmSignInDialog.SelectOrder;
                    txtWeightKgs.Text = "0";
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                    ShowSlitPos(_selectOrder.NoOfSlits);
                    bindingSource1.DataSource = _selectOrder;
                    DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Order Selected");
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

        private string GenerateHTMLToast(string title, string text, string type = "w3-warning")
        {
            string str = @"
                    <html>
                    <head>
                    <style>
	                    .w3-warning {
		                    background-color:#ffffcc;
		                    border-left:8px solid #ffeb3b;
		                    margin:0;
	                    }

                        .w3-error  {
	                        background-color:#ffdddd;
	                        border-left:6px solid #f44336;
	                        margin:0;
                        }

                        .w3-success {
	                        background-color:#dff0d8;
	                        border-left:6px solid #4bae4f;
	                        margin:0;
                        }
                    </style>
                    </head>
                    <body style='margin: 0; border-style: solid;'>
	                    <div class='{000}'>
		                    <p><strong>{111}: </strong>{222}
	                    </div>
                    </body>
                    </html>";

            str = str.Replace("{000}", type);
            str = str.Replace("{111}", title);
            str = str.Replace("{222}", text);
            return str;
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

            string html = GenerateHTMLToast(title, text, cssClass);

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


                //if (EnableProduce())
                //{

                //}
                //if (IssueQtyAvailableInInputLoc(_selectOrder.ProductionLine, Convert.ToDouble(txtWeightKgs.Text) * Convert.ToDouble(txtNoOfSlits.Text)))
                //{
                //    Produce();
                //    //PrintRollLabels();
                //    so = AppData.UpdateJumboRoll(_selectOrder.SAPOrderNo);
                //    if (!so.SuccessFlag) throw new ApplicationException($"Error updating jumbo roll. Error:{so.ServiceException}");
                //    _selectOrder.JumboRollNo = (int)so.ReturnValue;
                //    txtJumboRoll.Text = _selectOrder.JumboRollNo.ToString();
                //    olvRolls.Objects = null;
                //    _rolls = null;
                //}
                //else
                //{
                //    DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Inventory Check", $"Insufficient material in {_selectOrder.ProductionLine} input location.  {invCheckMsg}");
                //}


            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }




            //    SIIProductionOrder order = null;
            //    DataView productionOrders = new DataView(ds.Tables[0]);
            //    var processed = 0;
            //    var processMessage = "";

            //    try
            //    {
            //        Utilities.WriteToEventLog($"[ {productionOrders.Count} ] production orders are ready to be updated", EventLogEntryType.Information, false);
            //        using (PostedXMLDoc _postedOrders = new PostedXMLDoc("UpdatedProdcutionOrders"))
            //        {
            //            foreach (DataRowView rv in productionOrders)
            //            {
            //                try
            //                {
            //                    DataRow row = rv.Row;
            //                    order = new SIIProductionOrder(row);

            //                    // create production order
            //                    // ------------------------------------------------------------------------------------------------------------------------
            //                    using (ProductionOrder productionOrder = (ProductionOrder)_sap.B1Factory(SAPbobsCOM.BoObjectTypes.oProductionOrders, order.DocEntry))
            //                    {
            //                        try
            //                        {
            //                            if (order.UpdatePriority == 1) { productionOrder.SetUDFValue("U_NBS_Priority", order.Priority); }
            //                            if (order.UpdateDocDueDate == 1) { productionOrder.DueDate = order.DueDate; }
            //                            if (order.UpdateStartDate == 1) { productionOrder.StartDate = order.DueDate; }
            //                            if (productionOrder.Save() == false) { throw new B1Exception(_sap.SapCompany, _sap.GetLastExceptionMessage()); }
            //                            processed = 1;
            //                            processMessage = $"Production Order: {order.DocNum} was updated in SAP B1.";
            //                            if (order.UpdatePriority == 1) { processMessage = $"{processMessage} | Priority was updated"; }
            //                            if (order.UpdateDocDueDate == 1) { processMessage = $"{processMessage} | Due Date was updated"; }
            //                            if (order.UpdateStartDate == 1) { processMessage = $"{processMessage} | Start Date was updated"; }
            //                        }
            //                        catch (B1Exception ex) { processed = -1; processMessage = ex.Message.ToString(); }
            //                        catch (Exception ex) { processed = -1; processMessage = ex.Message.ToString(); }
            //                        _postedOrders.AppendUpdateProductionOrder(order.DocEntry, order.DocNum, order.Priority, order.DueDate, processed, processMessage);
            //                        order = null;
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    _postedOrders.AppendPostedSalesOrder(order.DocEntry, order.DocNum, order.Id, order.LineNum, order.ItemCode, order.Country, order.Quantity, order.QuantityNeeded, order.MaxOrder, order.Commited, order.OnHand, order.OnOrder, order.Priority, order.DocDate, order.DueDate, order.ProductionOrderDocEntry, -1, ex.Message.ToString(), order.OpenQty, order.CalcQty);
            //                    _postedOrders.AppendUpdateProductionOrder(order.DocEntry, order.DocNum, order.Priority, order.DueDate, -1, ex.Message.ToString());
            //                }
            //            }
            //            _postedOrders.SaveUpdateProductionOrdersXML();
            //        }


            //    }
            //    catch (Exception ex) { Utilities.WriteToEventLog($"{Utilities.GetLoggingText()} failed to Update Prodcution Orders.\n\n{ex.Message}", EventLogEntryType.Error, true); }
            //}

        }
        /// <summary>
        /// Calculates the planned issue based on available inventory
        /// </summary>
        /// <param name="prodLine"></param>
        /// <param name="productionQty"></param>
        private void RefreshIssueQty(string prodLine, double productionQty)
        {
            _plannedIssue = new List<InventoryIssueDetail>();
            var so = AppData.GetProdLineInputMaterial(prodLine);
            if (!so.SuccessFlag) throw new ApplicationException($"Error getting Prod. Line Input Material. Error:{so.ServiceException}");
            var inputLocMaterial = so.ReturnValue as List<InventoryDetail>;
            var userName = Convert.ToInt32(_selectOrder.ProductionMachineNo) == 1 ? AppUtility.GetSAPUserLine1() : AppUtility.GetSAPUserLine2();
            var password = Convert.ToInt32(_selectOrder.ProductionMachineNo) == 1 ? AppUtility.GetSAPPassLine1() : AppUtility.GetSAPPassLine2();
            using (SAPB1 sapB1 = new SAPB1(userName, password))
            {
                using (ProductionOrder productionOrder = (ProductionOrder)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oProductionOrders, _selectOrder.SAPDocEntry))
                {
                    foreach (var line in productionOrder.ProductionOrderLines)
                    {
                        var qtyReq = line.BaseQty * productionQty;
                        if (line.IssueType == SAPbobsCOM.BoIssueMethod.im_Manual)
                        {
                            var itemAvail = inputLocMaterial.Where(i => i.ItemCode == line.ItemCode).ToList();
                            if (itemAvail.Count > 0)
                            {
                                foreach (var item in itemAvail)
                                {
                                    if (qtyReq > 0)
                                    {
                                        if (item.Quantity > qtyReq)
                                        {
                                            AddPlannedIssueDetail(item.ItemCode, line.LineNumber, item.StorageLocation, item.LUID, item.SSCC, qtyReq, qtyReq, item.Batch, 0);
                                            qtyReq = 0;
                                        }
                                        else
                                        {
                                            AddPlannedIssueDetail(item.ItemCode, line.LineNumber, item.StorageLocation, item.LUID, item.SSCC, qtyReq, item.Quantity, item.Batch, 0);
                                            qtyReq = qtyReq - item.Quantity;
                                        }
                                    }
                                    
                                }
                            }
                            if (qtyReq > 0)
                            {
                                AddPlannedIssueDetail(line.ItemCode, line.LineNumber, string.Empty, 0, string.Empty, qtyReq, 0, string.Empty, qtyReq);
                            }
                        }
                    }
                }
            }
        }

        private void AddPlannedIssueDetail(string itemCode, int baseLine, string storageLocation, int luid, string sscc, double availableQty, double plannedIssueqty, string batch, double shortQty)
        {
            var issueDetail = new InventoryIssueDetail
            {
                ItemCode = itemCode,
                BaseLine = baseLine,
                StorageLocation = storageLocation,
                LUID = luid,
                SSCC = sscc,
                Quantity = availableQty,
                PlannedIssueQty = plannedIssueqty,
                Batch = batch,
                ShortQty = shortQty
            };
            _plannedIssue.Add(issueDetail);
        }

        private void Produce()
        {
            ServiceOutput so;
            try
            {
                var userName = Convert.ToInt32(_selectOrder.ProductionMachineNo) == 1 ? AppUtility.GetSAPUserLine1() : AppUtility.GetSAPUserLine2();
                var password = Convert.ToInt32(_selectOrder.ProductionMachineNo) == 1 ? AppUtility.GetSAPPassLine1() : AppUtility.GetSAPPassLine2();
                using (SAPB1 sapB1 = new SAPB1(userName, password))
                {
                    foreach (var roll in _rolls)
                    {
                        using (ProductionOrder productionOrder = (ProductionOrder)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oProductionOrders, _selectOrder.SAPOrderNo))
                        {
                            //productionOrder.
                            if (productionOrder.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                        }
                        so = AppData.AddRoll(roll.RollNo, _selectOrder.SAPOrderNo, roll.YJNOrder, roll.SSCC, 0, _selectOrder.Employee);

                        if (!so.SuccessFlag) throw new ApplicationException($"Error producing roll#: {roll.RollNo}. Error:{so.ServiceException}");
                    }
                }
                so = AppData.UpdateJumboRoll(_selectOrder.SAPOrderNo);
                if (!so.SuccessFlag) throw new ApplicationException($"Error updating jumbo roll. Error:{so.ServiceException}");
                _selectOrder.JumboRollNo = (int)so.ReturnValue;
                txtJumboRoll.Text = _selectOrder.JumboRollNo.ToString();
                olvRolls.Objects = null;
                _rolls = null;
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Rolls Produced", $"#{_rolls.Count.ToString()} rolls produced. Order: {txtYJNProdOrder.Text}, Roll No. {txtJumboRoll.Text}");
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }

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
            RefreshProdDate();
            if (!string.IsNullOrEmpty(txtWeightKgs.Text) && !string.IsNullOrEmpty(txtNoOfSlits.Text) && !string.IsNullOrEmpty(txtDie.Text))
            {
                GenerateRolls();
                EnableProduce();
            }
            else
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Invalied Values", "Input values for weight, No. of Slits and Die No.");
            }
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
            ShowSlitPos(Convert.ToInt32(txtNoOfSlits.Text));
        }

        private void PrintRollLabels()
        {
            try
            {
                var so = AppData.GetProdLine(_selectOrder.ProductionLine);
                if (!so.SuccessFlag) throw new ApplicationException($"Error getting Prod. Line Printers. Error:{so.ServiceException}");
                var prodLine = so.ReturnValue as ProdLine;
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();
                var fileNameRollLabels = Path.Combine(labelPrintLoc, "RollLabels" + labelPrintExtension);
                //var fileNameRollLabels1x6 = Path.Combine(labelPrintLoc, "RollLabels_1by6" + labelPrintExtension);
                //var formatFilePath4x6RollLabel = AppUtility.GetPGDefault4InchLabelFormat();
                //var formatFilePath1x6RollLabel = AppUtility.GetPGDefault1InchLabelFormat();
                var formatFilePathCombRollLabel = AppUtility.GetPGDefaultCombLabelFormat();

                var sbRollLabel = new StringBuilder(5000);
                sbRollLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathCombRollLabel, prodLine.Printer1);
                sbRollLabel.AppendLine();
                sbRollLabel.Append(@"%END%");
                sbRollLabel.AppendLine();
                sbRollLabel.Append("Item, ItemName, IRMS, LotNo, RollNo, SSCC, Qty");
                sbRollLabel.AppendLine();
                for (int i = 0; i < nudCopies.Value; i++)
                {
                    foreach (var roll in _rolls)
                    {
                        sbRollLabel.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", roll.ItemCode, roll.ItemName, roll.IRMS, roll.YJNOrder, roll.RollNo, roll.SSCC, roll.Kgs);
                        sbRollLabel.AppendLine();
                    }
                }
                using (StreamWriter sw = File.CreateText(fileNameRollLabels))
                {
                    sw.Write(sbRollLabel.ToString());
                }
                //var sb1x6 = new StringBuilder(5000);
                //sb1x6.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePath1x6RollLabel, prodLine.Printer2);
                //sb1x6.AppendLine();
                //sb1x6.Append(@"%END%");
                //sb1x6.Append("Item, ItemName, IRMS, LotNo, RollNo, SSCC, Qty");
                //for (int i = 0; i < nudCopies1x6.Value; i++)
                //{
                //    foreach (var roll in _rolls)
                //    {
                //        sb1x6.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", roll.ItemCode, roll.ItemName, roll.IRMS, roll.YJNOrder, roll.RollNo, roll.SSCC,roll.Kgs);
                //        sb1x6.AppendLine();
                //    }
                //}
                //using (StreamWriter sw = File.CreateText(fileNameRollLabels1x6))
                //{
                //    sw.Write(sb1x6.ToString());
                //}
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "4x6 and 1 inch roll labels printed. Please check printer.");

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
            var so = AppData.GetProdLine(_selectOrder.ProductionLine);
            if (!so.SuccessFlag) throw new ApplicationException($"Error getting Prod. Line Printers. Error:{so.ServiceException}");
            var prodLine = so.ReturnValue as ProdLine;
            RefreshIssueQty(_selectOrder.ProductionLine, Convert.ToDouble(txtWeightKgs.Text) * Convert.ToDouble(txtNoOfSlits.Text));
            EnableGenerate();
        }

        private void txtDie_Validated(object sender, EventArgs e)
        {
            EnableGenerate();
        }
    }
}