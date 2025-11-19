using BrightIdeasSoftware;
using log4net;
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
    /// Represents the form for film production
    /// </summary>
    public partial class FrmMainRockline : Form
    {
        private static ILog log = LogManager.GetLogger(typeof(FrmMainRockline));

        FloatingHTML m_htmlToast = new FloatingHTML();
        private RollLabelData _selectOrder = new RollLabelData();
        private List<Roll> _rolls = null;
        private BindingSource bindingSource1;
        private List<InventoryIssueDetail> _plannedIssue;

        /// <summary>
        /// Initializes a new instance of the FrmMain class.
        /// </summary>
        public FrmMainRockline()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the form load event.
        /// </summary>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Create a new BindingSource and set it as the data source for the form
            bindingSource1 = new BindingSource();
            bindingSource1.DataSource = _selectOrder;

            // Disable the create and generate rolls buttons initially
            btnCreate.Enabled = false;
            btnGenerateRolls.Enabled = false;

            // Bind the text boxes to the appropriate properties of the binding source
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
            txtTargetRolls.DataBindings.Add("Text", bindingSource1, "TargetRolls");
            txtInvRolls.DataBindings.Add("Text", bindingSource1, "InvRolls");
            txtRollsLeft.DataBindings.Add("Text", bindingSource1, "LeftToProduce");
            var widthMMBinding = txtWidthMM.DataBindings.Add("Text", bindingSource1, "WidthInMM");
            widthMMBinding.Format += WidthMMBinding_Format;
        }

        private void WidthMMBinding_Format(object sender, ConvertEventArgs e)
        {
            // The value comes in as a decimal; format it like an integer
            var valueType = e.Value.GetType();
            if (valueType == typeof(decimal))
            {
                var strValue = ((decimal)e.Value).ToString("N0");
                e.Value = strValue;
            }
        }



        /// <summary>
        /// Shows or hides the slit position checkboxes based on the number of slit positions.
        /// </summary>
        /// <param name="noSlitPos">The number of slit positions.</param>
        private void ShowSlitPos(int noSlitPos)
        {
            // Set the visibility of each slit position checkbox based on the number of slit positions
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
            chkSP16.Visible = noSlitPos > 15;
            chkSP17.Visible = noSlitPos > 16;
            chkSP18.Visible = noSlitPos > 17;
            chkSP19.Visible = noSlitPos > 18;
            chkSP20.Visible = noSlitPos > 19;
            chkSP21.Visible = noSlitPos > 20;
            chkSP22.Visible = noSlitPos > 21;
            chkSP23.Visible = noSlitPos > 22;
            chkSP24.Visible = noSlitPos > 23;
            chkSP25.Visible = noSlitPos > 24;
            chkSP26.Visible = noSlitPos > 25;
            chkSP27.Visible = noSlitPos > 26;
            chkSP28.Visible = noSlitPos > 27;
            chkSP29.Visible = noSlitPos > 28;

            // Enable the txtDie and txtLengthLM text boxes
            txtDie.Enabled = true;
            txtLengthLM.Enabled = true;
        }


        /// <summary>
        /// Checks if the form is ready to start the production process.
        /// </summary>
        private void CheckReadyToProduce()
        {
            try
            {
                // Reset rolls and roll objects
                olvRolls.Objects = null;
                _rolls = null;

                // Retrieve lenth in linear meters (LM) from txtLengthWM text box
                var weightKgs = Convert.ToDecimal(txtLengthLM.Text);

                // Check if all required fields are filled and valid
                if (txtNoOfSlits.Text == "0" || string.IsNullOrEmpty(txtNoOfSlits.Text) ||
                    string.IsNullOrEmpty(txtDie.Text) || string.IsNullOrEmpty(txtLengthLM.Text) ||
                    txtLengthLM.Text == "0")
                {
                    // Disable the btnGenerateRolls button and display an error notification
                    btnGenerateRolls.Enabled = false;
                    DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Complete Form", "Please input values No. Of Slits, Die No., and Linear meters.");

                    // Reset rolls and return
                    _rolls = null;
                    return;
                }
                //else if (weightKgs < _selectOrder.MinRollKgs || weightKgs > _selectOrder.MaxRollKgs)
                //{
                //    // Display an error notification for an invalid roll weight
                //    DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Invalid Roll Weight", $"Weight (kgs) must be greater than Min. Roll Weight: {_selectOrder.MinRollKgs.ToString("0.00")} and less than Max Roll Weight: {_selectOrder.MaxRollKgs.ToString("0.00")}.");
                //}
                else
                {
                    // Calculate the planned issue quantity
                    _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(txtLengthLM.Text) * _selectOrder.WidthInMM * 0.001m * Convert.ToDecimal(txtNoOfSlits.Text), 2);

                    // Check if there are shortages in the planned issue
                    var hasShortage = _plannedIssue.Where(i => i.ShortQty > 0 && i.BatchControlled);

                    // TODO: Add back when resmix is moved to line
                    // Display planned issue dialog if there are shortages
                    //if (hasShortage.Count() > 0)
                    //{
                    //    using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
                    //    {
                    //        frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                    //        DialogResult dr = frmPlannedIssues.ShowDialog();
                    //    }
                    //}

                    // Enable the btnGenerateRolls button and lnkPlannedIssues link
                    btnGenerateRolls.Enabled = true;
                    lnkPlannedIssues.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                // Display an error notification for an exception in the CheckReadyToProduce method
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, $"Check Ready to Produce Error", $"Error Check Ready to Produce: {ex.Message}");
            }
        }


        /// <summary>
        /// Sets the enabled state of the create button based on certain conditions.
        /// </summary>
        private void SetCreateButtonEnabled()
        {
            var invRollPlusRollsThisDoff = _selectOrder.InvRolls + _rolls.Where(r => !r.Scrap).Count();

            // Check if the target rolls exceeded
            if (_selectOrder.TargetRolls - invRollPlusRollsThisDoff < 0)
            {
                // Display a warning notification for exceeding the target rolls
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, $"Target Rolls Exceeded", $"Target Rolls exceeded. Target: {txtTargetRolls.Text} Inventory plus rolls this Doff: {invRollPlusRollsThisDoff.ToString()}");
                btnCreate.Enabled = false;
            }
            // Check if there are scrap rolls without a scrap reason
            else if (_rolls.Where(r => r.Scrap && string.IsNullOrEmpty(r.ScrapReason)).Count() > 0)
            {
                // Display a warning notification to enter a scrap reason for all scrap rolls
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Enter Scrap Reason", "Please enter a scrap reason for all scrap rolls.");
                btnCreate.Enabled = false;
            }
            else
            {
                btnCreate.Enabled = true;
            }
        }

        /// <summary>
        /// Generates rolls based on the specified number of slits and other input values.
        /// </summary>
        private void GenerateRolls()
        {
            RefreshProdDate();

            // Check if the number of slits is 0 or empty
            if (txtNoOfSlits.Text == "0" || string.IsNullOrEmpty(txtNoOfSlits.Text))
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Complete Form", "Please input values No. Of Slits, Die No., and first roll weight.");
                return;
            }

            _rolls = new List<Roll>();
            try
            {
                for (int i = 1; i <= Convert.ToInt32(txtNoOfSlits.Text); i++)
                {
                    // Get the corresponding slit position checkbox
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
                            //Kgs = Convert.ToDecimal(txtLengthLM.Text)
                            SquareMeters = _selectOrder.WidthInMM * 0.001m * Convert.ToDecimal(txtLengthLM.Text),
                            // Quantity in square yards
                            Quantity = Math.Round(_selectOrder.WidthInMM * 0.001m * Convert.ToDecimal(txtLengthLM.Text) * 1.19599m, 2),
                            PONumber = _selectOrder.PONumber,
                            UOM = "SY"
                        };

                        // Check if the roll already exists
                        var so = AppData.CheckRoll(roll.RollNo);
                        if (!so.SuccessFlag) throw new ApplicationException($"Error on Check Roll: {roll.RollNo}");
                        var count = (Int32)so.ReturnValue;
                        if (count > 0) throw new ApplicationException($"Roll {roll.RollNo} has already been created.");

                        _rolls.Add(roll);
                    }
                }

                olvRolls.SetObjects(_rolls);
                SetCreateButtonEnabled();
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, $"Roll No. Error", $"Error creating rolls: {ex.Message}");
                AppUtility.WriteToEventLog($"Error creating rolls: {ex.Message}", System.Diagnostics.EventLogEntryType.Error, AppUtility.GetEmailErrors());
            }
        }



        /// <summary>
        /// Changes the selected order by displaying a dialog to select a new order.
        /// </summary>
        private void ChangeOrder()
        {
            using (RocklineSelectOrderDialog frmSignInDialog = new RocklineSelectOrderDialog())
            {
                frmSignInDialog.SetDataSource("OTHER");
                DialogResult dr = frmSignInDialog.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    _selectOrder = frmSignInDialog.SelectOrder;
                    txtLengthLM.Text = "0";
                    txtNoOfSlits.Enabled = true;
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();

                    if (_selectOrder.NoOfSlits > 0)
                        ShowSlitPos(_selectOrder.NoOfSlits);

                    bindingSource1.DataSource = _selectOrder;

                    if (_selectOrder.ScrapItem == null)
                        DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Scrap setup issue", "A scrap item needs to be set on the order to record scrap.");

                    reprintToolStripMenuItem.Enabled = true;
                    scrapRollsToolStripMenuItem.Enabled = true;
                }
            }
        }


        /// <summary>
        /// Refreshes the information of the selected order.
        /// </summary>
        private void RefreshOrderInfo()
        {
            olvRolls.Objects = null;
            _rolls = null;
            txtLengthLM.Text = "0";
            lnkPlannedIssues.Enabled = false;
            btnGenerateRolls.Enabled = false;

            var so = AppData.GetProdOrder(_selectOrder.SAPOrderNo);

            if (!so.SuccessFlag)
                throw new ApplicationException("Error getting Production Orders. " + so.ServiceException);

            var order = so.ReturnValue as RollLabelData;
            _selectOrder.InvRolls = order.InvRolls;
            _selectOrder.TargetRolls = order.TargetRolls;
            bindingSource1.ResetBindings(false);
        }

        /// <summary>
        /// Handles the click event of the "Test SQL Connection" menu item.
        /// </summary>
        private void testSQLConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var connection = AppUtility.TestSQLConnection();
            Cursor.Current = Cursors.Default;

            if (connection.Length == 0)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Successful SQL Connection");
            }
            else
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Error", connection);
            }
        }


        /// <summary>
        /// Displays a toast notification with the specified type, title, and text.
        /// </summary>
        /// <param name="type">The type of the toast notification (Success, Warning, Error).</param>
        /// <param name="title">The title of the toast notification.</param>
        /// <param name="text">The text of the toast notification.</param>
        /// <param name="timeOut">The timeout duration in milliseconds (default is 4000ms).</param>
        public void DisplayToastNotification(ToastNotificationType type, string title, string text, int timeOut = 4000)
        {
            // Close any existing toast notification
            m_htmlToast.Close();

            // Set the offset value for positioning the toast notification
            int offset = 15;

            // Determine the CSS class based on the toast notification type
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
                    cssClass = "w3-error";
                    break;
            }

            // Generate the HTML content for the toast notification
            string html = AppUtility.GenerateHTMLToast(title, text, cssClass);

            // Calculate the height and width of the toast notification image based on the HTML content
            int imgHeight = m_htmlToast.GetHTMLHeight(html) + 5;
            int imgWidth = this.Width - 25;

            // Set the image size and HTML content of the toast notification
            m_htmlToast.SetImgSize(imgWidth, imgHeight);
            m_htmlToast.SetHTML(html);

            // Calculate the screen location for positioning the toast notification
            Rectangle rect = this.Bounds;
            Point px = new Point(rect.Left, rect.Bottom);
            Point screenLocation = PointToScreen(px);

            // Set the image location of the toast notification based on the screen location and offset
            m_htmlToast.SetImgLocation(px.X + offset, px.Y - imgHeight - offset);

            // Show the toast notification with the specified timeout duration
            m_htmlToast.Show(timeOut);
        }


        /// <summary>
        /// Handles the click event of the "Select" button.
        /// Changes the selected order and updates the UI accordingly.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            // Set the cursor to the wait cursor
            Cursor.Current = Cursors.WaitCursor;

            // Change the selected order
            ChangeOrder();

            // Set the cursor back to the default cursor
            Cursor.Current = Cursors.Default;
        }


        /// <summary>
        /// Handles the click event of the "SAP B1 Connection" menu item.
        /// Tests the connection to SAP B1 and displays a notification with the result.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void testSAPB1ConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set the cursor to the wait cursor
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // Create a new instance of the SAPB1 class to test the connection
                SAPB1 sap = new SAPB1();

                // Display a success notification
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "SAP B1 Connection", "Test Completed Successfully!");
            }
            catch (Exception ex)
            {
                // Display an error notification with the exception message
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "SAP B1 Connection", $"Test Failed!\n\n[Exception Message]\n{ex.Message}");
            }

            // Set the cursor back to the default cursor
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Handles the click event of the "Create" button.
        /// Initiates the production process and updates the order information.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Set the cursor to the wait cursor
            Cursor.Current = Cursors.WaitCursor;

            // Disable the "Create" button to prevent multiple clicks
            btnCreate.Enabled = false;

            try
            {
                // Call the Produce method to initiate the production process
                Produce();
            }
            catch (Exception ex)
            {
                // Display an error notification with the exception message and log the error
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
                log.Error("Exception occurred in btnCreate_Click", ex);
            }
            finally
            {
                // Refresh the order information after the production process
                RefreshOrderInfo();

                // Set the cursor back to the default cursor
                Cursor.Current = Cursors.Default;
            }
        }


        /// <summary>
        /// Initiates the production process by creating SSCC codes, updating inventory, printing roll labels, and performing inventory issues.
        /// </summary>
        private void Produce()
        {
            ServiceOutput so;

            // Get hold area, hold status, scrap status, and default status from AppUtility
            var holdArea = AppUtility.GetHoldLocation();
            var holdStatus = AppUtility.GetHoldStatus();
            var scrapStatus = AppUtility.GetScrapStatus();
            var defaultStatus = AppUtility.GetDefaultStatus();
            var defaultUom = AppUtility.GetDefaultUom();

            // Create SSCC codes for each roll
            foreach (var roll in _rolls)
            {
                so = AppData.CreateSSCC();
                if (!so.SuccessFlag)
                    throw new ApplicationException($"Error Creating SSCC. Error:{so.ServiceException}");

                var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
                roll.LUID = luid_sscc.Key;
                roll.SSCC = luid_sscc.Value;
            }

            // Get SAP credentials for the production machine
            var userNamePW = AppUtility.GetUserNameAndPasswordFilm(_selectOrder.ProductionMachineNo);
            var prodBatchNo = _selectOrder.SAPOrderNo; // AppUtility.GetOrderBatchNoFromChar(_selectOrder.BatchNo[0]);

            // Initialize SAPB1 instance with the obtained credentials
            using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
            {
                // Initialize InventoryReceipt object
                using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                {
                    var stdProduction = _rolls.Where(r => !r.Scrap);

                    // Add inventory receipt lines for standard production rolls
                    foreach (var roll in stdProduction)
                    {
                        invReceipt.AddLine(_selectOrder.SAPDocEntry, roll.ItemCode, Convert.ToDouble(roll.SquareMeters), prodBatchNo, roll.Hold ? holdStatus : _selectOrder.OutputLoc,
                            roll.Hold ? holdStatus : defaultStatus, roll.RollNo, roll.LUID, roll.SSCC, defaultUom, _selectOrder.YJNOrder, false, 0, _selectOrder.Shift, _selectOrder.Employee);
                    }

                    var scrapProd = _rolls.Where(r => r.Scrap && _selectOrder.ScrapItem != null);
                    var scrapLoc = AppUtility.GetScrapLocCode();

                    // Add inventory receipt lines for scrap production rolls
                    foreach (var roll in scrapProd)
                    {
                        invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ScrapItem, Convert.ToDouble(roll.SquareMeters), prodBatchNo, _selectOrder.OutputLoc, scrapStatus,
                            roll.RollNo, roll.LUID, roll.SSCC, defaultUom, _selectOrder.YJNOrder, true, _selectOrder.ScrapLine, _selectOrder.Shift, _selectOrder.Employee, roll.ScrapReason);
                    }

                    // Save the inventory receipt
                    if (invReceipt.Save() == false)
                    {
                        throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                    }
                }

                // Print roll labels
                PrintRollLabels();

                // Update jumbo roll in the database
                so = AppData.UpdateJumboRoll(_selectOrder.SAPOrderNo);
                if (!so.SuccessFlag)
                    throw new ApplicationException($"Error updating jumbo roll. Error:{so.ServiceException}");

                _selectOrder.JumboRollNo = (int)so.ReturnValue;
                txtJumboRoll.Text = _selectOrder.JumboRollNo.ToString();

                // Display success notification for the produced rolls
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Rolls Produced",
                    $"#{_rolls.Count.ToString()} rolls produced. Order: {txtYJNProdOrder.Text}, Roll No. {txtJumboRoll.Text}");

                // Update issues right before production
                _plannedIssue = AppUtility.RefreshIssueQty(_selectOrder.SAPOrderNo, _selectOrder.ProductionLine, Convert.ToDecimal(txtLengthLM.Text) * .001m * _selectOrder.WidthInMM * Convert.ToDecimal(txtNoOfSlits.Text), 2);

                // Initialize InventoryIssue object
                using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                {
                    foreach (var plIssue in _plannedIssue)
                    {
                        if (plIssue.ShortQty == 0)
                        {
                            // Add inventory issue line for planned issues with no shortage
                            invIssue.AddOrderIssueLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation,
                                plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.YJNOrder);
                        }
                        else
                        {
                            // Add shortage for planned issues with shortage
                            so = AppData.AddIssueShortage(_selectOrder.SAPOrderNo, plIssue.ItemCode, Convert.ToDecimal(plIssue.ShortQty));
                            if (!so.SuccessFlag)
                                throw new ApplicationException($"Error adding shortage. Error:{so.ServiceException}");
                        }
                    }

                    // Save the inventory issue
                    if (_plannedIssue.Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false)
                    {
                        throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                    }
                }
            }
        }


        /// <summary>
        /// Handles the KeyPress event for the txtNoOfSlits TextBox to allow only numeric input with a single decimal point.
        /// </summary>
        private void txtNoOfSlits_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }



        /// <summary>
        /// Handles the KeyPress event for the txtLengthLM TextBox to allow only numeric input with a single decimal point.
        /// </summary>
        private void txtLengthLM_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }


        /// <summary>
        /// Handles the Click event for the btnGenerateRolls button to generate rolls.
        /// </summary>
        private void btnGenerateRolls_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateRolls();
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Generate Rolls", $"An exception occurred in {AppUtility.GetLoggingText()} GenerateRolls Click.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"An exception occurred in {AppUtility.GetLoggingText()} GenerateRolls Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
            RefreshProdDate();
        }


        /// <summary>
        /// Refreshes the production date and updates the related properties in the select order.
        /// </summary>
        private void RefreshProdDate()
        {
            var currentDateTime = DateTime.Now;
            txtProductionDateFull.Text = currentDateTime.ToShortDateString();
            _selectOrder.ProductionYear = currentDateTime.Year.ToString().Last().ToString();
            _selectOrder.ProductionMonth = AppUtility.GetYanJanProdMo(currentDateTime);
            _selectOrder.ProductionDate = currentDateTime.Day.ToString("00");
        }

        /// <summary>
        /// Event handler that is triggered when the content of the txtNoOfSlits TextBox is validated.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void txtNoOfSlits_Validated(object sender, EventArgs e)
        {
            // Convert the text value of txtNoOfSlits to an integer
            var noOfSlits = Convert.ToInt32(txtNoOfSlits.Text);

            // Check if the number of slits is greater than zero
            if (noOfSlits > 0)
            {
                // Call the ShowSlitPos method to display the slit positions
                ShowSlitPos(noOfSlits);
            }
        }

        /// <summary>
        /// Generate and print roll labels based on the information stored in the _rolls collection.
        /// </summary>
        private void PrintRollLabels()
        {
            try
            {
                // Retrieve the necessary information for label printing
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();
                var fileNameRollLabels = Path.Combine(labelPrintLoc, "RocklineRollLabels" + labelPrintExtension);
                var rocklineFormatFilePath = AppUtility.GetRocklineDefaultRollLabelFormat();

                // Create a StringBuilder to store the content of the roll labels
                var sbRollLabel = new StringBuilder(5000);

                // Append the BTW command and its parameters to the StringBuilder
                sbRollLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", rocklineFormatFilePath, _selectOrder.Printer);
                sbRollLabel.AppendLine();
                sbRollLabel.Append(@"%END%");
                sbRollLabel.AppendLine();
                sbRollLabel.Append("PurchaseOrder,CustomerPartNumber,ItemNumber,RollNumber,Width,LotNumber,Quantity,UOM");
                sbRollLabel.AppendLine();

                // Append the roll information to the StringBuilder
                foreach (var roll in _rolls)
                {
                    sbRollLabel.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7}", 
                        _selectOrder.PONumber,
                        _selectOrder.IRMS,
                        roll.Scrap ? _selectOrder.ScrapItem : roll.ItemCode, 
                        roll.RollNo,
                        Math.Round(_selectOrder.WidthInMM / 25.4m, 0),
                        _selectOrder.YJNOrder,
                        roll.Quantity,
                        roll.UOM);
                    sbRollLabel.AppendLine();
                }

                // Write the roll labels to a file
                using (StreamWriter sw = File.CreateText(fileNameRollLabels))
                {
                    sw.Write(sbRollLabel.ToString());
                }

                // Display a success notification
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Roll labels printed. Please check printer.");
            }
            catch (Exception ex)
            {
                // Display an error notification and write the exception details to the event log
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }


        /// <summary>
        /// Event handler that handles the click event for the "Reprint" menu item.
        /// Allows the user to reprint rolls labels for a selected order.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        private void reprintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Create an instance of the reprint dialog form
                using (FrmReprintOrderRollsDialogRockline frmReprintDialog = new FrmReprintOrderRollsDialogRockline())
                {
                    // Set the YJNOrderNo property of the dialog form to the current order number
                    frmReprintDialog.YJNOrderNo = _selectOrder.YJNOrder;

                    // Show the dialog form and wait for the user's response
                    DialogResult dr = frmReprintDialog.ShowDialog();

                    // If the user clicks OK, proceed with reprinting the selected rolls
                    if (dr == DialogResult.OK)
                    {
                        // Get the selected rolls from the dialog form
                        _rolls = frmReprintDialog.SelectedRolls;

                        // Print the roll labels
                        PrintRollLabels();

                        // Reset the _rolls collection to null
                        _rolls = null;

                        // Display a success notification to inform the user
                        DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Check Printer");
                    }
                }
            }
            catch (Exception ex)
            {
                // Display an error notification and write the exception details to the event log
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} RePrintRollLabels.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }


        /// <summary>
        /// Event handler triggered when the txtWeightKgs TextBox is validated.
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The event arguments</param>
        private void txtWeightKgs_Validated(object sender, EventArgs e)
        {
            // Check if the weight is zero
            bool enable = txtLengthLM.Text == "0";

            // Get all the CheckBox controls in the Panel1 of the split container
            var slitPosCheckBoxes = this.splitContainer1.Panel1.Controls.OfType<CheckBox>();

            // Enable or disable the CheckBox controls based on the weight value
            foreach (var checkbox in slitPosCheckBoxes)
            {
                checkbox.Enabled = enable;
            }

            // Call the CheckReadyToProduce method to perform additional checks
            CheckReadyToProduce();
        }

        /// <summary>
        /// Event handler triggered when the contents of the txtDie textbox are validated.
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The event arguments</param>
        private void txtDie_Validated(object sender, EventArgs e)
        {
            // Call the CheckReadyToProduce method
            CheckReadyToProduce();
        }


        /// <summary>
        /// Event handler for the LinkClicked event of the lnkPlannedIssues LinkLabel.
        /// Opens a dialog to display planned issues.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void lnkPlannedIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
            {
                // Set the data source of the dialog
                frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
                // Show the dialog and wait for the user response
                DialogResult dr = frmPlannedIssues.ShowDialog();
            }
        }


        /// <summary>
        /// Event handler for the Click event of the boxScrapToolStripMenuItem menu item.
        /// Opens the FrmBoxScrap form to manage box scrap.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void boxScrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBoxScrapRockline frmBoxScrap = new FrmBoxScrapRockline();
            // Show the FrmBoxScrap form
            frmBoxScrap.Show();
        }


        /// <summary>
        /// Event handler for the Click event of the adjustResmixToolStripMenuItem menu item.
        /// Opens the FrmAdjustResmix form to adjust the resmix.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void adjustResmixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAdjustResmix frmAdjustResmix = new FrmAdjustResmix();
            // Show the FrmAdjustResmix form
            frmAdjustResmix.Show();
        }


        /// <summary>
        /// Event handler for the CellEditStarting event of the olvRolls object ListView.
        /// Handles the cell editing starting event for the olvRolls ListView.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The CellEditEventArgs containing event data.</param>
        private void olvRolls_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            var roll = (Roll)e.RowObject;
            // Ignore edit events for other columns
            if (e.Column != this.olvColScrapReason || !roll.Scrap) return;

            // Retrieve scrap reasons
            var so = AppData.GetScrapReasons();
            if (!so.SuccessFlag) throw new ApplicationException($"Failed to get scrap reasons. Error:{so.ServiceException}.");
            var scrapReasons = so.ReturnValue as List<string>;

            // Create and configure the ComboBox control
            ComboBox cb = new ComboBox();
            cb.Bounds = e.CellBounds;
            cb.Font = ((ObjectListView)sender).Font;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.DataSource = scrapReasons;

            // Set the ComboBox control as the editing control
            e.Control = cb;
        }

        /// <summary>
        /// Event handler for the CellEditFinishing event of the olvRolls object ListView.
        /// Handles the cell editing finishing event for the olvRolls ListView.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The CellEditEventArgs containing event data.</param>
        private void olvRolls_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            // Check if the editing control is a ComboBox
            if (e.Control is ComboBox)
            {
                // Check if the edited column is olvColScrapReason
                if (e.Column == this.olvColScrapReason)
                {
                    // Get the selected value from the ComboBox
                    string value = ((ComboBox)e.Control).SelectedItem.ToString();

                    // Update the ScrapReason property of the corresponding Roll object
                    ((Roll)e.RowObject).ScrapReason = value;

                    // Refresh the object in the olvRolls ListView to reflect the changes
                    this.olvRolls.RefreshObject((Roll)e.RowObject);

                    // Cancel the editing to prevent further processing
                    e.Cancel = true;

                    // Update the enabled state of the Create button
                    SetCreateButtonEnabled();
                }
            }
        }

        /// <summary>
        /// Event handler for the ItemChecked event of the olvRolls object ListView.
        /// Handles the item checked event for the olvRolls ListView.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The ItemCheckedEventArgs containing event data.</param>
        private void olvRolls_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // Enable editing for the olvColScrapReason column
            this.olvColScrapReason.IsEditable = true;

            // Update the enabled state of the Create button
            SetCreateButtonEnabled();
        }


        /// <summary>
        /// Event handler for the Click event of the scrapRollsToolStripMenuItem.
        /// Handles the click event for the scrapRollsToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The EventArgs containing event data.</param>
        private void scrapRollsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set the cursor to a wait cursor to indicate processing
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                using (FrmScrapRollsDialog frmScrapRollsDialog = new FrmScrapRollsDialog())
                {
                    // Set the YJNOrderNo property of the scrap rolls dialog form
                    frmScrapRollsDialog.YJNOrderNo = _selectOrder.YJNOrder;
                    frmScrapRollsDialog.CustomerId = AppUtility.GetRocklineCustomerID();

                    // Show the scrap rolls dialog form
                    DialogResult dr = frmScrapRollsDialog.ShowDialog();

                    if (dr == DialogResult.OK)
                    {
                        // Retrieve the selected rolls from the scrap rolls dialog form
                        _rolls = frmScrapRollsDialog.SelectedRolls;

                        // Scrap the selected rolls
                        ScrapRolls();

                        // Refresh the order information
                        RefreshOrderInfo();

                        // Display a success notification
                        DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Check Printer for Scrap Labels");
                    }
                }
            }
            catch (Exception ex)
            {
                // Display an error notification and log the exception
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "SAP B1 Connection", $"Exception has occurred in {AppUtility.GetLoggingText()} ScrapRolls.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} ScrapRolls.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }

            // Set the cursor back to the default cursor
            Cursor.Current = Cursors.Default;
        }


        /// <summary>
        /// Scrap the selected rolls.
        /// </summary>
        private void ScrapRolls()
        {
            // Get the username and password for the production machine from the configuration
            var userNamePW = AppUtility.GetUserNameAndPasswordFilm(_selectOrder.ProductionMachineNo);

            // Connect to SAP Business One
            using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
            {
                // Get the scrap location code and offset code from the configuration
                var scrapLocCode = AppUtility.GetScrapLocCode();
                var scrapGLOffset = AppUtility.GetScrapOffsetCode();

                // Create an inventory issue document for scrap
                using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                {
                    foreach (var roll in _rolls)
                    {
                        // Add a scrap issue line to the inventory issue document for each roll
                        invIssue.AddScrapIssueLine(roll.ItemCode, Convert.ToDouble(roll.SquareMeters), roll.StorLocCode, roll.QualityStatus, roll.RollNo, roll.LUID, roll.SSCC, roll.UOM, _selectOrder.YJNOrder, scrapGLOffset, roll.ScrapReason, _selectOrder.Shift);
                    }

                    // Save the inventory issue document
                    if (invIssue.Save() == false)
                    {
                        throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                    }
                }

                // Create an inventory receipt document to release the scrap
                using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                {
                    foreach (var roll in _rolls)
                    {
                        // Create a new LUID/SSCC if the roll has no SSCC
                        if (roll.LUID == 0)
                        {
                            var so = AppData.CreateSSCC();
                            if (!so.SuccessFlag)
                            {
                                throw new ApplicationException($"Error Creating SSCC. Error:{so.ServiceException}");
                            }
                            var luid_sscc = (KeyValuePair<int, string>)so.ReturnValue;
                            roll.LUID = luid_sscc.Key;
                            roll.SSCC = luid_sscc.Value;
                        }

                        // Add a line to the inventory receipt document for each roll
                        invReceipt.AddLine(_selectOrder.SAPDocEntry, _selectOrder.ScrapItem, Convert.ToDouble(roll.SquareMeters), roll.RollNo.Last(), scrapLocCode, "RELEASED", roll.RollNo, roll.LUID, roll.SSCC, "Kgs", _selectOrder.YJNOrder, true, _selectOrder.ScrapLine, _selectOrder.Shift, _selectOrder.Employee, roll.ScrapReason, scrapGLOffset);

                        // Save the inventory receipt document
                        if (invReceipt.Save() == false)
                        {
                            throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                        }
                    }
                }

                // Print the roll labels
                PrintRollLabels();
            }

            // Display a success notification
            DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Rolls converted to scrap", "Rolls converted to scrap");

            // Refresh the order information
            RefreshOrderInfo();
        }

    }
}