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
    /// Represents a form for mask production/labeling
    /// </summary>
    public partial class FrmMask : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        private RollLabelData _selectOrder = new RollLabelData();
        private BindingSource bindingSource1;
        private List<InventoryIssueDetail> _plannedIssue;
        private int luid;
        private string sscc;
        private int _prodRun;

        /// <summary>
        /// Initialize a new instance of the FrmMask class.
        /// </summary>
        public FrmMask()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the load event of the FrmMask form.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void FrmMask_Load(object sender, EventArgs e)
        {
            // Initialize the binding source
            bindingSource1 = new BindingSource();
            bindingSource1.DataSource = _selectOrder;

            // Bind the text properties of controls to the data source
            txtEmployee.DataBindings.Add("Text", bindingSource1, "Employee");
            txtOrderNo.DataBindings.Add("Text", bindingSource1, "SAPOrderNo");
            txtShift.DataBindings.Add("Text", bindingSource1, "Shift");
            txtProductionLine.DataBindings.Add("Text", bindingSource1, "ProductionLine");
            txtItemCode.DataBindings.Add("Text", bindingSource1, "ItemCode");
            txtItemName.DataBindings.Add("Text", bindingSource1, "ItemDescription");

            // Set the UOM label text
            lblUOM.Text = AppUtility.GetMaskUOMLabel();
        }

        /// <summary>
        /// Handles the click event of the btnSelect button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }

        /// <summary>
        /// Changes the selected order and updates the form fields accordingly.
        /// </summary>
        private void ChangeOrder()
        {
            using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
            {
                frmSignInDialog.SetDataSource("MASK");
                DialogResult dr = frmSignInDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    _selectOrder = frmSignInDialog.SelectOrder;
                    txtNoOfMasks.Text = "0";
                    txtNoOfMasks.Enabled = true;
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                    bindingSource1.DataSource = _selectOrder;
                }
            }
            var so = AppData.GetLastProductionRun(_selectOrder.SAPOrderNo);
            if (!so.SuccessFlag) throw new ApplicationException($"Error getting next batch. Error:{so.ServiceException}");
            _prodRun = (int)so.ReturnValue;
            _prodRun += 1;
            txtBatch.Text = $"{_selectOrder.SAPOrderNo.ToString()}";
        }

        /// <summary>
        /// Event handler for the Validated event of the "txtWeightKgs" TextBox control.
        /// It is triggered when the contents of the "txtWeightKgs" TextBox control are validated.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void txtWeightKgs_Validated(object sender, EventArgs e)
        {
            CheckReadyToProduce();
        }

        /// <summary>
        /// Checks if the form is ready for production based on the input values.
        /// </summary>
        private void CheckReadyToProduce()
        {
            if (string.IsNullOrEmpty(txtNoOfMasks.Text) || txtNoOfMasks.Text == "0" || string.IsNullOrEmpty(txtBatch.Text))
            {
                // Disable the "Produce" button and display an error toast notification.
                btnProduce.Enabled = false;
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Complete Form", "Please input Units to Produce.");
                return;
            }
            else
            {
                // Refresh the planned issue quantities based on the input values.
                _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(txtNoOfMasks.Text));

                // Check if there are shortages with batch control.
                var hasShortage = _plannedIssue.Where(i => i.ShortQty > 0 && i.BatchControlled);
                if (hasShortage.Count() > 0)
                {
                    // Display the planned issue dialog to handle shortages.
                    using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
                    {
                        frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                        DialogResult dr = frmPlannedIssues.ShowDialog();
                    }
                }

                // Enable the "Produce" button and the planned issues link.
                btnProduce.Enabled = true;
                lnkPlannedIssues.Enabled = true;
            }
        }


        /// <summary>
        /// Handles the production process.
        /// </summary>
        private void Produce()
        {
            ServiceOutput so;

            // Create SSCC
            so = AppData.CreateSSCC();
            if (!so.SuccessFlag) throw new ApplicationException($"Error Creating SSCC. Error:{so.ServiceException}");
            var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
            luid = luid_sscc.Key;
            sscc = luid_sscc.Value;

            // Get username and password for SAP B1
            var userNamePW = AppUtility.GetUserNameAndPasswordMask(_selectOrder.ProductionMachineNo);

            // Get the production batch number
            var prodBatchNo = Convert.ToInt32(txtBatch.Text.Substring(txtBatch.Text.LastIndexOf("-") + 1));

            // Create SAPB1 and InventoryIssue instances
            using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
            {
                using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                {
                    foreach (var plIssue in _plannedIssue)
                    {
                        if (plIssue.ShortQty == 0)
                        {
                            // Add order issue line for planned issue
                            invIssue.AddOrderIssueLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.SAPOrderNo.ToString());
                        }
                        else
                        {
                            // Add issue shortage for items with shortage quantity
                            so = AppData.AddIssueShortage(_selectOrder.SAPOrderNo, plIssue.ItemCode, Convert.ToDecimal(plIssue.ShortQty));
                            if (!so.SuccessFlag) throw new ApplicationException($"Error adding shortage. Error:{so.ServiceException}");
                        }
                    }

                    // Save the inventory issue
                    if (_plannedIssue.Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false)
                    {
                        throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                    }
                }

                // Create InventoryReceipt instance
                using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                {
                    // Add line for inventory receipt
                    invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ItemCode, Convert.ToDouble(txtNoOfMasks.Text), prodBatchNo, _selectOrder.OutputLoc, "RELEASED", txtBatch.Text, luid, sscc, "Kgs", "", false, 0, _selectOrder.Shift, _selectOrder.Employee);

                    // Save the inventory receipt
                    if (invReceipt.Save() == false)
                    {
                        throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                    }

                    // Increment the production run
                    so = AppData.IncrementProductionRun(_selectOrder.SAPOrderNo);
                    if (!so.SuccessFlag) throw new ApplicationException($"Error getting next batch. Error:{so.ServiceException}");
                    _prodRun = (int)so.ReturnValue;
                    _prodRun += 1;
                    txtBatch.Text = $"{_selectOrder.SAPOrderNo.ToString()}";
                }
            }

            // Display success toast notification
            DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Units Produced", $"#{txtNoOfMasks.Text} produced. Order: {txtOrderNo.Text}");
        }


        /// <summary>
        /// Handles the link clicked event for viewing planned issues.
        /// </summary>
        private void lnkPlannedIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
            {
                frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                DialogResult dr = frmPlannedIssues.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the button click event for producing and printing.
        /// </summary>
        private void btnProduce_Click(object sender, EventArgs e)
        {
            try
            {
                Produce();
                PrintMaskLabel();
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Produce/Print", $"Exception has occurred in {AppUtility.GetLoggingText()} Produce.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Produce/Print.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
            finally
            {
                txtNoOfMasks.Text = "0";
                lnkPlannedIssues.Enabled = false;
                btnProduce.Enabled = false;
                luid = 0;
                sscc = null;
            }
        }

        /// <summary>
        /// Prints the mask label.
        /// </summary>
        private void PrintMaskLabel()
        {
            var labelPrintLoc = AppUtility.GetBTTriggerLoc();
            var labelPrintExtension = AppUtility.GetLabelPrintExtension();
            var fileNameRollLabels = Path.Combine(labelPrintLoc, "MaskLabel" + labelPrintExtension);
            var formatFilePathResmixLabel = AppUtility.GetPGDefaultResmixLabelFormat();

            var sbMixLabel = new StringBuilder(5000);
            sbMixLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathResmixLabel, _selectOrder.Printer);
            sbMixLabel.AppendLine();
            sbMixLabel.Append(@"%END%");
            sbMixLabel.AppendLine();
            sbMixLabel.Append("Item, ItemName, IRMS, LotNo, RollNo, SSCC, Qty");
            sbMixLabel.AppendLine();

            sbMixLabel.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", _selectOrder.ItemCode, _selectOrder.ItemDescription, "", "", txtBatch.Text, sscc, Convert.ToDecimal(txtNoOfMasks.Text));

            using (StreamWriter sw = File.CreateText(fileNameRollLabels))
            {
                sw.Write(sbMixLabel.ToString());
            }

            DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Mask label printed. Please check printer.");
        }

        /// <summary>
        /// Displays a toast notification with the specified type, title, text, and timeout duration.
        /// </summary>
        /// <param name="type">The type of the toast notification.</param>
        /// <param name="title">The title of the toast notification.</param>
        /// <param name="text">The text content of the toast notification.</param>
        /// <param name="timeOut">The timeout duration for the toast notification in milliseconds.</param>
        public void DisplayToastNotification(ToastNotificationType type, string title, string text, int timeOut = 4000)
        {
            // Close any existing toast notification
            m_htmlToast.Close();

            // Set the offset for positioning the notification
            int offset = 15;

            // Determine the CSS class based on the notification type
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

            // Calculate the height and width of the notification image
            int imgHeight = m_htmlToast.GetHTMLHeight(html) + 5;
            int imgWidth = this.Width - 25;

            // Set the size and content of the notification image
            m_htmlToast.SetImgSize(imgWidth, imgHeight);
            m_htmlToast.SetHTML(html);

            // Calculate the screen position for displaying the notification
            Rectangle rect = this.Bounds;
            Point px = new Point(rect.Left, rect.Bottom);
            Point screenLocation = PointToScreen(px);

            // Set the location of the notification image
            m_htmlToast.SetImgLocation(px.X + offset, px.Y - imgHeight - offset);

            // Show the notification with the specified timeout duration
            m_htmlToast.Show(timeOut);
        }


        /// <summary>
        /// Event handler triggered when the contents of the txtBatch TextBox control are validated.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void txtBatch_Validated(object sender, EventArgs e)
        {
            // Call the CheckReadyToProduce method to validate the form and enable/disable the Produce button
            CheckReadyToProduce();
        }

    }
}
