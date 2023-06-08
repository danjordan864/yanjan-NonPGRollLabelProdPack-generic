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
    /// Represents a form for resmix labels.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the FrmMix class.
        /// </summary>
        public FrmMix()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the FrmMix form.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
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

            // Set up scrap reason combo
            var so = AppData.GetProdLines(false);
            if (!so.SuccessFlag)
                throw new ApplicationException($"Failed to get prod lines. Error: {so.ServiceException}.");
            _prodLines = so.ReturnValue as List<ProductionLine>;
            var pls = _prodLines.Select(p => p.Code).ToList();
            pls.Insert(0, "Select To Line");
            cboToLine.DataSource = pls;

            _loading = false;
        }

        /// <summary>
        /// Handles the Click event of the btnSelect button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }


        /// <summary>
        /// Changes the order and updates the necessary controls and data.
        /// </summary>
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
            if (!so.SuccessFlag)
                throw new ApplicationException($"Error getting next batch. Error:{so.ServiceException}");

            _prodRun = (int)so.ReturnValue;
            _prodRun += 1;
        }


        /// <summary>
        /// When the entered weight is validated, checks if the system is ready to produce.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void txtWeightKgs_Validated(object sender, EventArgs e)
        {
            CheckReadyToProduce();
        }


        /// <summary>
        /// Checks if the system is ready to produce based on the entered weight and form completion.
        /// </summary>
        private void CheckReadyToProduce()
        {
            if (string.IsNullOrEmpty(txtWeightKgs.Text) || txtWeightKgs.Text == "0" || string.IsNullOrEmpty(txtBatch.Text) || cboToLine.Text == "Select To Line")
            {
                // Disable the "Produce" button and set focus to the "cboToLine" control
                btnProduce.Enabled = false;
                cboToLine.Focus();

                // Display an error toast notification
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Complete Form", "Please input Mix weight");
                return;
            }
            else
            {
                // Refresh the planned issue quantity based on the selected order and entered weight
                _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(txtWeightKgs.Text));

                // Check if there are any shortages in the planned issue
                var hasShortage = _plannedIssue.Where(i => i.ShortQty > 0 && i.BatchControlled);

                if (hasShortage.Count() > 0)
                {
                    // Display a dialog to handle planned issues with shortages
                    using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
                    {
                        frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                        DialogResult dr = frmPlannedIssues.ShowDialog();
                    }
                }

                // Enable the "Produce" button and the "lnkPlannedIssues" link
                btnProduce.Enabled = true;
                lnkPlannedIssues.Enabled = true;
            }
        }


        /// <summary>
        /// Performs the production process for the selected order.
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

            // Get default status and UOM
            var defaultStatus = AppUtility.GetDefaultStatus();
            var defaultUom = AppUtility.GetDefaultUom();

            // Get user name and password for the production machine
            var userNamePW = AppUtility.GetUserNameAndPasswordMix(_selectOrder.ProductionMachineNo);

            // Get production batch number
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
                            // Add order issue line to the inventory issue
                            invIssue.AddOrderIssueLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.SAPOrderNo.ToString());
                        }
                        else
                        {
                            // Add shortage for the planned issue
                            so = AppData.AddIssueShortage(_selectOrder.SAPOrderNo, plIssue.ItemCode, Convert.ToDecimal(plIssue.ShortQty));
                            if (!so.SuccessFlag) throw new ApplicationException($"Error adding shortage. Error:{so.ServiceException}");
                        }
                    }

                    // Save the inventory issue if there are planned issue quantities
                    if (_plannedIssue.Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false)
                    {
                        throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                    }
                }

                // Get the input location for the production line
                var toProductionLineInputLoc = _prodLines.Where(p => p.Code == cboToLine.Text).Select(p => p.InputLocationCode).FirstOrDefault();

                // Create InventoryReceipt instance
                using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                {
                    // Add line to the inventory receipt
                    invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ItemCode, Convert.ToDouble(txtWeightKgs.Text), prodBatchNo, toProductionLineInputLoc, defaultStatus, txtBatch.Text, luid, sscc, defaultUom, "", false, 0, _selectOrder.Shift, _selectOrder.Employee);

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
                }
            }

            // Display a success toast notification
            DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Resmix Produced", $"#{txtWeightKgs.Text} kgs. produced. Order: {txtOrderNo.Text}");
        }

        /// <summary>
        /// Handles the click event of the Planned Issues link label.
        /// Opens a dialog to display planned issues with batch control.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void lnkPlannedIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
            {
                // Set the data source for the planned issues dialog
                frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());

                // Show the planned issues dialog
                DialogResult dr = frmPlannedIssues.ShowDialog();
            }
        }


        /// <summary>
        /// Handles the click event of the Produce button.
        /// Initiates the production process and prints the ResMix label.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void btnProduce_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Produce();  // Initiate the production process
                PrintResMixLabel();  // Print the ResMix label
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Produce Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
            finally
            {
                RefreshOrderInfo();  // Refresh the order information
                Cursor.Current = Cursors.Default;
            }
        }


        /// <summary>
        /// Refreshes the order information on the form.
        /// </summary>
        private void RefreshOrderInfo()
        {
            txtWeightKgs.Text = "0";  // Reset the weight to 0
            txtWeightKgs.Enabled = false;  // Disable the weight input field
            cboToLine.Text = "Select To Line";  // Reset the selected production line
            lnkPlannedIssues.Enabled = false;  // Disable the planned issues link
            btnProduce.Enabled = false;  // Disable the Produce button
            luid = 0;  // Reset the luid variable
            sscc = null;  // Reset the sscc variable
        }


        /// <summary>
        /// Prints the ResMix label.
        /// </summary>
        private void PrintResMixLabel()
        {
            try
            {
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();  // Get the label print location
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();  // Get the label print extension
                var fileNameRollLabels = Path.Combine(labelPrintLoc, "ResMixLabel" + labelPrintExtension);  // Create the file path for the label
                var formatFilePathResmixLabel = AppUtility.GetPGDefaultResmixLabelFormat();  // Get the format file path for the ResMix label

                var sbMixLabel = new StringBuilder(5000);
                sbMixLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathResmixLabel, _selectOrder.Printer);  // Construct the command to print the label
                sbMixLabel.AppendLine();
                sbMixLabel.Append(@"%END%");
                sbMixLabel.AppendLine();
                sbMixLabel.Append("Item, ItemName, IRMS, LotNo, RollNo, SSCC, Qty");
                sbMixLabel.AppendLine();

                sbMixLabel.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", _selectOrder.ItemCode, _selectOrder.ItemDescription, "", "", txtBatch.Text, sscc, Convert.ToDecimal(txtWeightKgs.Text));  // Add the label data

                using (StreamWriter sw = File.CreateText(fileNameRollLabels))
                {
                    sw.Write(sbMixLabel.ToString());  // Write the label data to the file
                }

                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "ResMix label printed. Please check printer.");
                RefreshOrderInfo();  // Refresh the order information

            }
            catch (Exception ex)
            {
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
            m_htmlToast.Close();  // Close any existing toast notification

            int offset = 15;  // Set the offset for positioning the toast notification

            string cssClass = "w3-success";  // Default CSS class for success type
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

            string html = AppUtility.GenerateHTMLToast(title, text, cssClass);  // Generate the HTML content for the toast notification

            int imgHeight = m_htmlToast.GetHTMLHeight(html) + 5;  // Calculate the height of the toast notification
            int imgWidth = this.Width - 25;  // Calculate the width of the toast notification

            m_htmlToast.SetImgSize(imgWidth, imgHeight);  // Set the size of the toast notification
            m_htmlToast.SetHTML(html);  // Set the HTML content of the toast notification

            Rectangle rect = this.Bounds;
            Point px = new Point(rect.Left, rect.Bottom);
            Point screenLocation = PointToScreen(px);

            m_htmlToast.SetImgLocation(px.X + offset, px.Y - imgHeight - offset);  // Set the location of the toast notification

            m_htmlToast.Show(timeOut);  // Show the toast notification for the specified duration
        }


        /// <summary>
        /// Handles the event when the Batch text box is validated.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void txtBatch_Validated(object sender, EventArgs e)
        {
            CheckReadyToProduce();
        }

        /// <summary>
        /// Handles the event when the selected index of the To Line combo box changes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
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
