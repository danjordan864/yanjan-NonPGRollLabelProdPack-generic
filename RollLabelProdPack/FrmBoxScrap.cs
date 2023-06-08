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
    /// Partial definition of form class to handle box scrap production
    /// </summary>
    public partial class FrmBoxScrap : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        private RollLabelData _selectOrder = new RollLabelData();
        private BindingSource bindingSource1;
        private List<InventoryIssueDetail> _plannedIssue;
        private int luid;
        private string sscc;
        private ILog _log;

        /// <summary>
        /// Initializes a new instance of the FrmBoxScrap class.
        /// </summary>
        public FrmBoxScrap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the FrmBoxScrap form.
        /// Initializes the form and sets up data bindings.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void FrmBoxScrap_Load(object sender, EventArgs e)
        {
            _log = LogManager.GetLogger(typeof(FrmBoxScrap));

            // Set up data bindings
            bindingSource1 = new BindingSource();
            bindingSource1.DataSource = _selectOrder;
            txtEmployee.DataBindings.Add("Text", bindingSource1, "Employee");
            txtOrderNo.DataBindings.Add("Text", bindingSource1, "SAPOrderNo");
            txtShift.DataBindings.Add("Text", bindingSource1, "Shift");
            txtProductionLine.DataBindings.Add("Text", bindingSource1, "ProductionLine");
            txtItemCode.DataBindings.Add("Text", bindingSource1, "ItemCode");
            txtItemName.DataBindings.Add("Text", bindingSource1, "ItemDescription");

            // Set up scrap reason combo
            var so = AppData.GetScrapReasons();
            if (!so.SuccessFlag) throw new ApplicationException($"Failed to get scrap reasons. Error:{so.ServiceException}.");
            var scrapReasons = so.ReturnValue as List<string>;
            cboScrapReason.DataSource = scrapReasons;
        }

        /// <summary>
        /// Handles the Click event of the btnSelect button.
        /// Changes the order to the selected order from the dialog.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }

        /// <summary>
        /// Changes the order to the selected order from the dialog.
        /// </summary>
        private void ChangeOrder()
        {
            using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
            {
                frmSignInDialog.SetDataSource("FF"); //pass in item itemGroup fileter = FF, MIX, MASK
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


        /// <summary>
        /// Event handler for the Validated event of the txtWeightKgs TextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The EventArgs object that contains the event data.</param>
        private void txtWeightKgs_Validated(object sender, EventArgs e)
        {
            // Call the CheckReadyToProduce method to check if the system is ready for production
            CheckReadyToProduce();
        }


        /// <summary>
        /// Checks if the system is ready for production based on the selected order and input values.
        /// </summary>
        private void CheckReadyToProduce()
        {
            if (string.IsNullOrEmpty(_selectOrder.ScrapItem))
            {
                btnScrap.Enabled = false;
                DisplayToastNotification(ToastNotificationType.Error, "No Scrap Item", "Production Order must have scrap item.");
                return;
            }
            else if (string.IsNullOrEmpty(txtWeightKgs.Text) || txtWeightKgs.Text == "0")
            {
                btnScrap.Enabled = false;
                DisplayToastNotification(ToastNotificationType.Error, "Complete Form", "Please input scrap weight.");
                return;
            }
            else
            {
                // Refresh the issue quantity based on the selected order, production line, and scrap weight
                _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(txtWeightKgs.Text), 1);

                var hasShortage = _plannedIssue.Where(i => i.ShortQty > 0 && i.BatchControlled);
                if (hasShortage.Count() > 0)
                {
                    // Show the planned issue dialog for items with shortage
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


        /// <summary>
        /// Produces the box scrap based on the selected order and input values.
        /// </summary>
        private void Produce()
        {
            _log.Debug("In Produce");
            ServiceOutput so;

            // Create SSCC
            so = AppData.CreateSSCC();
            if (!so.SuccessFlag)
                throw new ApplicationException($"Error Creating SSCC. Error: {so.ServiceException}");
            var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
            luid = luid_sscc.Key;
            sscc = luid_sscc.Value;

            // Get user name and password for the production machine
            var userNamePW = AppUtility.GetUserNameAndPasswordFilm(_selectOrder.ProductionMachineNo);
            var prodBatchNo = AppUtility.GetOrderBatchNoFromChar(_selectOrder.BatchNo[0]);

            // Refresh the issue quantity based on the selected order, production line, and scrap weight
            _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(txtWeightKgs.Text), 1);

            using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
            {
                using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                {
                    foreach (var plIssue in _plannedIssue.Where(i => i.BatchControlled))
                    {
                        // Add order issue line for items without shortage
                        if (plIssue.ShortQty == 0)
                        {
                            invIssue.AddOrderIssueLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.YJNOrder);
                        }
                        else
                        {
                            // Add shortage for items with shortage
                            so = AppData.AddIssueShortage(_selectOrder.SAPOrderNo, plIssue.ItemCode, Convert.ToDecimal(plIssue.ShortQty));
                            if (!so.SuccessFlag)
                                throw new ApplicationException($"Error adding shortage. Error: {so.ServiceException}");
                        }
                    }

                    if (_plannedIssue.Where(i => i.BatchControlled).Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false)
                    {
                        throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                    }
                }

                using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                {
                    // Add line for the box scrap in inventory receipt
                    invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ScrapItem, Convert.ToDouble(txtWeightKgs.Text), prodBatchNo, AppUtility.GetScrapLocCode(), "RELEASED", "", luid, sscc, "Kgs", _selectOrder.YJNOrder, true, _selectOrder.ScrapLine, _selectOrder.Shift, _selectOrder.Employee, cboScrapReason.Text);

                    if (invReceipt.Save() == false)
                    {
                        throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                    }
                }
            }

            DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Box Scrap Produced", $"#{txtWeightKgs.Text} kgs. produced. Order: {txtOrderNo.Text}");
        }


        /// <summary>
        /// Event handler for the planned issues link label clicked event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="LinkLabelLinkClickedEventArgs"/> that contains the event data.</param>
        private void lnkPlannedIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
            {
                // Set the data source of the planned issue dialog to the filtered list of planned issues
                frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());

                // Show the planned issue dialog and get the dialog result
                DialogResult dr = frmPlannedIssues.ShowDialog();
            }
        }


        /// <summary>
        /// Event handler for the Produce button click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        private void btnProduce_Click(object sender, EventArgs e)
        {
            // Produce the box scrap
            Produce();

            // Print the box scrap label
            PrintBoxScrapLabel();

            // Reset the weight input to zero
            txtWeightKgs.Text = "0";

            // Disable the planned issues link
            lnkPlannedIssues.Enabled = false;

            // Disable the scrap button
            btnScrap.Enabled = false;

            // Reset the luid and sscc variables
            luid = 0;
            sscc = null;
        }

        /// <summary>
        /// Prints the box scrap label.
        /// </summary>
        private void PrintBoxScrapLabel()
        {
            try
            {
                // Get the label print location and extension
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();

                // Construct the file path for the label file
                var fileNameRollLabels = Path.Combine(labelPrintLoc, "BoxScrapLabel" + labelPrintExtension);

                // Get the format file path for the label
                var formatFilePathLabel = AppUtility.GetPGDefaultScrapLabelFormat();

                // Create a StringBuilder to build the label content
                var sbMixLabel = new StringBuilder(5000);

                // Add the BTW header and parameters
                sbMixLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathLabel, _selectOrder.Printer);
                sbMixLabel.AppendLine();
                sbMixLabel.Append(@"%END%");
                sbMixLabel.AppendLine();

                // Add the label fields header
                sbMixLabel.Append("Item, ItemName, IRMS, LotNo, RollNo, SSCC, Qty");
                sbMixLabel.AppendLine();

                // Add the label field values
                sbMixLabel.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", _selectOrder.ScrapItem, _selectOrder.ScrapItemName, "", _selectOrder.YJNOrder, "", sscc, Convert.ToDecimal(txtWeightKgs.Text));

                // Create the label file and write the label content
                using (StreamWriter sw = File.CreateText(fileNameRollLabels))
                {
                    sw.Write(sbMixLabel.ToString());
                }

                // Display success notification
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Scrap label printed. Please check printer.");

                // Reset controls and variables
                txtWeightKgs.Text = "0";
                lnkPlannedIssues.Enabled = false;
                btnScrap.Enabled = false;
                luid = 0;
                sscc = null;
            }
            catch (Exception ex)
            {
                // Display error notification and log exception
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Test SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }

        /// <summary>
        /// Displays a toast notification with the specified type, title, and text.
        /// </summary>
        /// <param name="type">The type of the toast notification.</param>
        /// <param name="title">The title of the toast notification.</param>
        /// <param name="text">The text of the toast notification.</param>
        /// <param name="timeOut">The timeout duration for the toast notification in milliseconds. Default is 4000ms.</param>
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

            // Generate HTML content for the toast notification
            string html = AppUtility.GenerateHTMLToast(title, text, cssClass);

            // Calculate the size of the toast notification image based on the HTML content
            int imgHeight = m_htmlToast.GetHTMLHeight(html) + 5;
            int imgWidth = this.Width - 25;

            // Set the size and HTML content of the toast notification
            m_htmlToast.SetImgSize(imgWidth, imgHeight);
            m_htmlToast.SetHTML(html);

            // Calculate the location of the toast notification
            Rectangle rect = this.Bounds;
            Point px = new Point(rect.Left, rect.Bottom);
            Point screenLocation = PointToScreen(px);

            // Set the location of the toast notification image
            m_htmlToast.SetImgLocation(px.X + offset, px.Y - imgHeight - offset);

            // Show the toast notification with the specified timeout
            m_htmlToast.Show(timeOut);
        }


        /// <summary>
        /// Event handler for the Validated event of the txtBatch TextBox control.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void txtBatch_Validated(object sender, EventArgs e)
        {
            CheckReadyToProduce();
        }

    }
}
