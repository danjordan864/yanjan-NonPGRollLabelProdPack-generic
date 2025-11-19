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
    public partial class FrmPackPrintRockline : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        private ILog _log;
        private bool _loading;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmPackPrint"/> class.
        /// </summary>
        public FrmPackPrintRockline()
        {
            InitializeComponent();

            // Start the timer (uncomment the line below if needed)
            // timer1.Start();

            // Get the logger instance for logging purposes
            _log = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Handles the event when the "FrmPackPrint" form is loaded.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void FrmPackPrint_Load(object sender, EventArgs e)
        {
            _loading = false;

            // Setup the described task column
            SetupDescribedTaskColumn();

            olvBundles.ShowImagesOnSubItems = true;
            olvBundles.RowHeight = 100;
            olvBundles.EmptyListMsg = "No Unprinted Pack labels";
            olvBundles.UseAlternatingBackColors = false;
            olvBundles.UseHotItem = false;
            olvBundles.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;
            olvBundles.CellEditStarting += OlvBundlesOnCellEditStarting;

            // Find the index of the print column
            int printColumnIndex = 0;
            for (; olvBundles.ColumnsInDisplayOrder[printColumnIndex].AspectName != "PrintButtonText"; printColumnIndex++)
                ;

            // Install the renderer for the print column if found
            if (printColumnIndex < olvBundles.ColumnsInDisplayOrder.Count)
            {
                var printColumn = olvBundles.ColumnsInDisplayOrder[printColumnIndex];
                if (_log.IsDebugEnabled)
                    _log.Debug($"About to install renderer for print column at {printColumn} ");
                printColumn.Renderer = new PrintColumnButtonRenderer(ilPackPrint.Images["Print"]);
                printColumn.IsButton = true;
            }

            olvBundles.ButtonClick += OlvBundles_ButtonClick;
            olvBundles.CellEditFinishing += OlvBundlesOnCellEditFinishing;
            olvBundles.SmallImageList = ilPackPrint;
            olvTotalWeight.AspectPutter = delegate (object obj, object newValue) { ((PackLabel)obj).TotalWeight = decimal.Parse(newValue.ToString()); };

            olvColPrint.IsEditable = true;
            olvTotalWeight.IsEditable = true;

            // Setup the show rolls hyperlink column
            SetupShowRollsHyperLinkColumn();

            // Setup the print button
            SetupPrintButton();

            // Load the pack labels
            LoadPackLabels(false);
        }

        /// <summary>
        /// Handles the button click event in the "olvBundles" ObjectListView control.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OlvBundles_ButtonClick(object sender, CellClickEventArgs e)
        {
            _log.Debug("Entered OlvBundles_ButtonClick");
            _log.Debug($"e.Column.AspectName = {e.Column.AspectName}");
            if (e.Column.AspectName == "PrintButtonText")
            {
                var packLabel = e.Model as PackLabel;
                if (packLabel != null && packLabel.Valid)
                {
                    try
                    {
                        if (packLabel.TotalWeightEntered)
                        {
                            // Get the roll min/max weight for the item
                            var so = AppData.GetRollMinMaxKgForItem(packLabel.ItemCode);
                            if (so.SuccessFlag)
                            {
                                RollMinMaxKg minMax = (RollMinMaxKg)so.ReturnValue;

                                // Check if the total net weight falls within the valid range
                                if (packLabel.TotalNetKg >= minMax.MinRollKg * packLabel.Rolls.Count &&
                                    packLabel.TotalNetKg <= minMax.MaxRollKg * packLabel.Rolls.Count)
                                {
                                    Cursor = Cursors.WaitCursor;

                                    // For Rockline we don't adjust roll quantities
                                    // Adjust roll quantities
                                    //AdjustRollQuantities(packLabel);

                                    // Print the pack label
                                    PrintPackLabel(packLabel);

                                    // Reload pack labels
                                    LoadPackLabels(chkReprint.Checked, txtOrder.Text);
                                }
                                else
                                {
                                    MessageBox.Show("Total net weight is outside range for roll net weight times number of rolls",
                                        "Total net weight out of range", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    int rowIndex = e.RowIndex;
                                    olvBundles.EditSubItem((OLVListItem)olvBundles.Items[rowIndex], 1);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Item master is missing min/max roll weight. Please update in SAP before proceeding",
                                    "No min/max roll weight found", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                int rowIndex = e.RowIndex;
                                olvBundles.EditSubItem((OLVListItem)olvBundles.Items[rowIndex], 1);
                            }
                        }
                        else
                        {
                            MessageBox.Show("You must enter the total weight.", "Total weight entry required", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            int rowIndex = e.RowIndex;
                            olvBundles.EditSubItem((OLVListItem)olvBundles.Items[rowIndex], 1);
                        }
                    }
                    finally
                    {
                        Cursor = Cursors.Arrow;
                    }
                }
            }
        }


        /// <summary>
        /// Handles the event when cell editing is finishing in the "olvBundles" ObjectListView control.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OlvBundlesOnCellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.Column.AspectName == "TotalWeight")
            {
                decimal newTotalWeightValue = 0m;
                if (!decimal.TryParse(e.NewValue.ToString(), out newTotalWeightValue))
                {
                    e.Cancel = true;
                }
                if (newTotalWeightValue < 0)
                {
                    e.Cancel = true;
                }
            }
        }


        /// <summary>
        /// Sets up the print button column in the ObjectListView control.
        /// </summary>
        private void SetupPrintButton()
        {
            // Commented code blocks are removed from the method implementation

            // Set the aspect getter for the print button column
            //olvColPrint.AspectGetter = delegate {
            //    return "Print";
            //};

            // Set the image getter for the print button column
            olvColPrint.ImageGetter = delegate (object rowObject)
            {
                if (rowObject is PackLabel)
                {
                    var packLabel = rowObject as PackLabel;
                    if (packLabel.Valid == true)
                    {
                        return "Print";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            };

            // Commented code blocks are removed from the method implementation

            //olvColPrint.AspectToStringConverter = delegate {
            //    return String.Empty;
            //};
        }


        /// <summary>
        /// Loads the pack labels into the ObjectListView control.
        /// </summary>
        /// <param name="isReprint">A boolean value indicating whether the pack labels are for reprinting.</param>
        /// <param name="order">Optional. The order value to filter the pack labels. Default is null.</param>
        private void LoadPackLabels(bool isReprint, string order = null)
        {
            try
            {
                _loading = true;

                // Get the pack labels from the data source
                var so = AppData.GetPackLabels(isReprint, order);
                if (!so.SuccessFlag)
                {
                    throw new ApplicationException($"Error getting pack labels. Error: {so.ServiceException}");
                }

                var packLabels = so.ReturnValue as List<PackLabel>;

                foreach (var packLabel in packLabels)
                {
                    // Attach event handler for property changes in the pack label
                    packLabel.PropertyChanged += PackLabel_PropertyChanged;

                    // Set the pack label description
                    packLabel.Description = $"Created: {packLabel.Created.ToString("g")}\r\n{packLabel.ItemCode} - {packLabel.Description} SM: {packLabel.Qty.ToString("#.##")}\r\nSSCC: {packLabel.SSCC}";

                    // Get the rolls for the pack label
                    so = AppData.GetPackLabelRolls(packLabel.ID);
                    if (!so.SuccessFlag)
                    {
                        throw new ApplicationException($"Error getting pack label rolls. Error: {so.ServiceException}");
                    }

                    packLabel.Rolls = so.ReturnValue as List<Roll>;

                    // Calculate the total net weight and total tare weight for the pack label
                    packLabel.TotalNetKg = packLabel.Rolls.Sum(t => t.NetKg);
                    packLabel.TotalTareKg = packLabel.Rolls.Sum(t => t.TareKg);
                    packLabel.TotalWeight = packLabel.TotalNetKg + packLabel.TotalTareKg;

                    // Validate the pack label
                    if (packLabel.Rolls.Count() > packLabel.MaxRollsPerPack)
                    {
                        packLabel.ValidMessage = $"Rolls in pack: {packLabel.Rolls.Count().ToString()} exceeded maximum allowed rolls per pack: {packLabel.MaxRollsPerPack.ToString()} for this item.\r\n";
                    }
                    if (packLabel.Rolls.Select(r => r.ItemCode).Distinct().Count() > 1)
                    {
                        packLabel.ValidMessage += "Cannot combine different items in a pack.\r\n";
                    }
                    if (packLabel.Rolls.Select(r => r.YJNOrder).Distinct().Count() > 1)
                    {
                        packLabel.ValidMessage += "Cannot combine different lots in a pack.\r\n";
                    }

                    if (string.IsNullOrEmpty(packLabel.ValidMessage))
                    {
                        packLabel.Valid = true;
                    }
                    else
                    {
                        packLabel.Valid = false;
                    }
                }

                // Clear and set the pack labels in the ObjectListView control
                olvBundles.Objects = null;
                olvBundles.SetObjects(packLabels);
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Load Pack Labels", $"Exception has occurred in {AppUtility.GetLoggingText()} Load Pack Labels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
            finally
            {
                _loading = false;
            }
        }


        /// <summary>
        /// Event handler for the PropertyChanged event of the PackLabel object.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs containing information about the changed property.</param>
        private void PackLabel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PackLabel packLabel = (PackLabel)sender;
            if (e.PropertyName == "TotalWeight")
            {
                // Calculate the total adjustment and distribute it among the rolls
                // RDJ 20220719 - Round total adjustment to 2 places
                var totalAdjustment = Math.Round(packLabel.TotalNetKg - packLabel.Qty, 2);
                foreach (Roll roll in packLabel.Rolls)
                {
                    // RDJ 20220719 - Use 2 decimal places instead of 5
                    var adjustAmt = Math.Round(totalAdjustment / packLabel.Rolls.Count, 2);
                    roll.AdjustKgs = adjustAmt;
                }
                // RDJ 20220719 - add/subtract difference between total of AdjustKgs for all rolls and totalAdjustment
                // to AdjustKgs for the first roll.
                if (packLabel.Rolls.Count > 0)
                {
                    var difference = totalAdjustment - packLabel.Rolls.Sum(t => t.AdjustKgs);
                    packLabel.Rolls[0].AdjustKgs += difference;
                }
                // Set the TotalWeightEntered flag if not loading and TotalWeightEntered is false
                if (!_loading && !packLabel.TotalWeightEntered)
                {
                    packLabel.TotalWeightEntered = true;
                }
            }
        }

        /// <summary>
        /// Adjusts the quantities of rolls based on the pack label's adjustments and updates the inventory in SAP Business One.
        /// </summary>
        /// <param name="packLabel">The pack label containing the rolls to be adjusted.</param>
        /// <exception cref="B1Exception">Thrown when there is an error saving the inventory issue or receipt in SAP Business One.</exception>
        /// <exception cref="Exception">Thrown when an exception occurs during the adjustment process.</exception>
        private void AdjustRollQuantities(PackLabel packLabel)
        {
            try
            {
                // Retrieve SAP user and password
                var user = AppUtility.GetSAPUser();
                var password = AppUtility.GetSAPPassword();

                // Create a new SAPB1 instance
                using (SAPB1 sapB1 = new SAPB1(user, password))
                {
                    // Get scrap location code and offset code
                    var scrapLocCode = AppUtility.GetScrapLocCode();
                    var scrapGLOffset = AppUtility.GetScrapOffsetCode();

                    // Set the shift value
                    var shift = "D";

                    // Initialize flags for scraps added and receipts added
                    var scrapsAdded = false;
                    var receiptsAdded = false;

                    // Create an InventoryIssue instance for scrap issues
                    using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                    {
                        int luid = 0;
                        foreach (var roll in packLabel.Rolls)
                        {
                            // Process rolls with negative adjustments as scraps
                            if (roll.AdjustKgs < 0m)
                            {
                                roll.ScrapReason = "Bundle weight adjustment";

                                // Set the default storage location code if not specified
                                if (string.IsNullOrEmpty(roll.StorLocCode))
                                {
                                    roll.StorLocCode = "BUNDLE";
                                }

                                // Get the LUID for the SSCC
                                var so = AppData.GetLUIDForSSCC(roll.SSCC);
                                if (so.SuccessFlag)
                                {
                                    if (so.ReturnValue is int)
                                    {
                                        luid = (int)so.ReturnValue;
                                    }
                                }

                                roll.LUID = luid;

                                // Add a scrap issue line
                                invIssue.AddScrapIssueLine(roll.ItemCode, Convert.ToDouble(roll.AdjustKgs * -1m), roll.StorLocCode, "RELEASED", roll.RollNo, roll.LUID, roll.SSCC, roll.UOM, roll.YJNOrder, scrapGLOffset, roll.ScrapReason, shift);

                                // Reset adjustments and update roll quantities
                                roll.ScrapReason = string.Empty;
                                roll.Kgs += roll.AdjustKgs;
                                roll.AdjustKgs = 0m;

                                scrapsAdded = true;
                            }
                        }

                        // Save the inventory issue if scraps were added
                        if (scrapsAdded)
                        {
                            if (invIssue.Save() == false)
                            {
                                throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                            }
                        }
                    }

                    // Create an InventoryReceipt instance for receipts
                    using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                    {
                        int line = 0;
                        int luid = 0;
                        foreach (var roll in packLabel.Rolls)
                        {
                            // Process rolls with positive adjustments as receipts
                            if (roll.AdjustKgs > 0m)
                            {
                                roll.ScrapReason = "Bundle weight adjustment";

                                // Set the default storage location code if not specified
                                if (string.IsNullOrEmpty(roll.StorLocCode))
                                {
                                    roll.StorLocCode = "BUNDLE";
                                }

                                // Reset the scrap GL offset code
                                scrapGLOffset = string.Empty;

                                // Get the LUID for the SSCC
                                var so = AppData.GetLUIDForSSCC(roll.SSCC);
                                if (so.SuccessFlag)
                                {
                                    if (so.ReturnValue is int)
                                    {
                                        luid = (int)so.ReturnValue;
                                    }
                                }

                                roll.LUID = luid;

                                // Add a receipt line
                                invReceipt.AddLine(0, roll.ItemCode, Convert.ToDouble(roll.AdjustKgs), roll.RollNo.Last(), roll.StorLocCode, "RELEASED", roll.RollNo, roll.LUID, roll.SSCC, "Kgs", packLabel.YJNOrder, false, line++, shift, "Pack", roll.ScrapReason, scrapGLOffset, true);

                                // Reset adjustments and update roll quantities
                                roll.ScrapReason = string.Empty;
                                roll.Kgs += roll.AdjustKgs;
                                roll.AdjustKgs = 0m;

                                receiptsAdded = true;
                            }
                        }

                        // Save the inventory receipt if receipts were added
                        if (receiptsAdded)
                        {
                            if (invReceipt.Save() == false)
                            {
                                throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                            }
                        }
                    }
                }

                // Update the pack label quantity with the total weight
                packLabel.Qty = packLabel.TotalWeight;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                throw ex;
            }
        }


        /// <summary>
        /// Sets up the column for displaying described tasks in the ListView.
        /// </summary>
        private void SetupDescribedTaskColumn()
        {
            // Setup a described task renderer, which draws a large icon
            // with a title, and a description under the title.
            // Almost all of this configuration could be done through the Designer
            // but I've done it through code that make it clear what's going on.

            // Create and install an appropriately configured renderer 
            this.olvColPackDesc.Renderer = CreateDescribedTaskRenderer();

            // Now let's setup the couple of other bits that the column needs

            // Set the AspectName property to specify which property should be used to get the title
            this.olvColPackDesc.AspectName = "YJNOrder";

            // Set the ImageGetter delegate to determine the image for each row
            olvColPackDesc.ImageGetter = delegate (object rowObject)
            {
                if (rowObject is PackLabel)
                {
                    var packLabel = rowObject as PackLabel;
                    if (packLabel.Valid == true)
                    {
                        return "True";
                    }
                    else
                    {
                        return "False";
                    }
                }
                else
                {
                    return "False";
                }
            };
            // Set the CellPadding property to add space around the task and its description
            this.olvColPackDesc.CellPadding = new Rectangle(4, 2, 4, 2);
        }

        /// <summary>
        /// Creates and configures a DescribedTaskRenderer for displaying described tasks in the ListView.
        /// </summary>
        /// <returns>The configured DescribedTaskRenderer.</returns>
        private DescribedTaskRenderer CreateDescribedTaskRenderer()
        {
            // Create a new instance of DescribedTaskRenderer
            DescribedTaskRenderer renderer = new DescribedTaskRenderer();

            // Set the ImageList property to give the renderer its own collection of images
            renderer.ImageList = ilPackPrint;

            // Set the DescriptionAspectName property to specify which property holds the text to be used as a description
            renderer.DescriptionAspectName = "Description";

            // Customize the formatting of the renderer
            renderer.TitleFont = new Font("Tahoma", 12, FontStyle.Bold);
            renderer.DescriptionFont = new Font("Tahoma", 11);
            renderer.ImageTextSpace = 20;
            renderer.TitleDescriptionSpace = 1;

            // Use GDI text rendering for clearer text appearance
            renderer.UseGdiTextRendering = true;

            // Optionally, customize the colors of the title and description
            //renderer.TitleColor = Color.DarkBlue;
            //renderer.DescriptionColor = Color.CornflowerBlue;

            return renderer;
        }

        /// <summary>
        /// Event handler for the CellEditStarting event of the olvBundles control.
        /// Handles special cell edit behavior for the print click and total weight columns.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The CellEditEventArgs containing event data.</param>
        private void OlvBundlesOnCellEditStarting(object sender, CellEditEventArgs e)
        {
            try
            {
                // Special cell edit handling for the print click column
                if (e.Column == olvColPrint)
                {
                    e.Cancel = true; 
                }
                // Handling for the total weight column
                else if (e.Column == olvTotalWeight)
                {
                    if (!chkReprint.Checked)
                    {
                        var textBox = (TextBox)e.Control;
                        textBox.Text = "";
                    }
                    else
                    {
                        var packLabel = e.RowObject as PackLabel;
                        packLabel.TotalWeightEntered = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Print Pack Labels", $"Exception has occurred in {AppUtility.GetLoggingText()} Load Pack Labels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Print Pack Label.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }


        /// <summary>
        /// Sets up the Show Rolls hyperlink column in the olvBundles control.
        /// </summary>
        private void SetupShowRollsHyperLinkColumn()
        {
            // Set the aspect getter to return "Show Rolls" for all rows
            olvColAction.AspectGetter = delegate
            {
                return "Show Rolls";
            };

            // Subscribe to the IsHyperlink event to set the URL for the hyperlink
            olvBundles.IsHyperlink += delegate (object sender, IsHyperlinkEventArgs e)
            {
                e.Url = e.Text;
            };
        }

        /// <summary>
        /// Event handler for the Tick event of timer1 control.
        /// Loads pack labels by calling the LoadPackLabels method.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadPackLabels(false);
        }


        /// <summary>
        /// Displays a toast notification with the specified type, title, and text.
        /// </summary>
        /// <param name="type">The type of the toast notification.</param>
        /// <param name="title">The title of the toast notification.</param>
        /// <param name="text">The text of the toast notification.</param>
        /// <param name="timeOut">The timeout duration for displaying the toast notification.</param>
        public void DisplayToastNotification(ToastNotificationType type, string title, string text, int timeOut = 4000)
        {
            // Close any existing toast notification
            m_htmlToast.Close();

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
                    cssClass = "w3-error";
                    break;
            }

            // Generate the HTML content for the toast notification
            string html = AppUtility.GenerateHTMLToast(title, text, cssClass);

            // Calculate the size of the notification image
            int imgHeight = m_htmlToast.GetHTMLHeight(html) + 5;
            int imgWidth = this.Width - 25;

            // Set the size and HTML content of the toast notification
            m_htmlToast.SetImgSize(imgWidth, imgHeight);
            m_htmlToast.SetHTML(html);

            // Calculate the position of the notification on the screen
            Rectangle rect = this.Bounds;
            Point px = new Point(rect.Left, rect.Bottom);
            Point screenLocation = PointToScreen(px);

            // Set the location of the notification image
            m_htmlToast.SetImgLocation(px.X + offset, px.Y - imgHeight - offset);

            // Show the toast notification with the specified timeout
            m_htmlToast.Show(timeOut);
        }


        /// <summary>
        /// Event handler for the HyperlinkClicked event of the olvBundles object.
        /// Displays the rolls dialog for the clicked pack label.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The HyperlinkClickedEventArgs containing event data.</param>
        private void olvBundles_HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
        {
            // Extract the PackLabel object from the clicked model
            var packLabel = e.Model as PackLabel;

            // Open the Rolls dialog for the selected pack label
            if (packLabel != null)
            {
                using (FrmRollsDialog frmRolls = new FrmRollsDialog())
                {
                    // Set the rolls data source for the dialog
                    frmRolls.SetDataSource(packLabel.Rolls);

                    // Set the validation message for the dialog
                    frmRolls.SetValidationMessage(packLabel.ValidMessage);

                    // Display the Rolls dialog as a modal dialog
                    DialogResult dr = frmRolls.ShowDialog();
                }
            }
        }


        /// <summary>
        /// Prints the pack label for the specified pack label object.
        /// </summary>
        /// <param name="packLabel">The PackLabel object for which to print the label.</param>
        /// <exception cref="ApplicationException">Thrown if there is an error updating the pack labels.</exception>
        private void PrintPackLabel(PackLabel packLabel)
        {
            // Extract necessary information for label printing
            //var qty = packLabel.TotalNetKg.ToString("0.00");
            var qty = packLabel.Rolls.Sum(t => t.Quantity).ToString("0.00");
            var totalNetKg = packLabel.TotalNetKg.ToString("0.00");
            var labelPrintLocPack = AppUtility.GetBTTriggerLoc();
            var labelPrintExtension = AppUtility.GetLabelPrintExtension();
            var fileNamePackLabel = Path.Combine(labelPrintLocPack, "PackLabel" + labelPrintExtension);
            var packLabelPrinter = AppUtility.GetPackPrinterName();
            //var formatFilePathPackLabel = AppUtility.GetPGDefaultPackLabelFormat();
            var formatFilePathPackLabel = AppUtility.GetRocklineDefaultPackLabelFormat();

            // Create the content of the label file using a StringBuilder
            var sb = new StringBuilder(5000);
            sb.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathPackLabel, packLabelPrinter);
            sb.AppendLine();
            sb.Append(@"%END%");
            sb.AppendLine();
            sb.Append("ItemNumber,ProductionDate,ItemName,CustomerPartNumber,Quantity,UOM,LotNumber,PurchaseOrderNumber,Weight");
            sb.AppendLine();

            var uom = packLabel.Rolls?.FirstOrDefault()?.UOM ?? string.Empty;

            // Add the label data for the specified number of copies
            for (int i = 0; i < packLabel.Copies; i++)
            {
                sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                    packLabel.ItemCode,
                    packLabel.ProductionDate.ToShortDateString(),
                    packLabel.ItemName,
                    packLabel.IRMS,
                    qty,
                    uom,
                    packLabel.LotNo,
                    packLabel.PONumber,
                    totalNetKg);
                sb.AppendLine();
            }

            // Write the label file to disk
            using (StreamWriter sw = File.CreateText(fileNamePackLabel))
            {
                sw.Write(sb.ToString());
            }

            // Update the pack label in the database
            var so = AppData.UpdatePackLabel(packLabel, "Y");
            //var so = AppData.UpdatePackLabel(packLabel.ID, packLabel.Qty, "Y");
            if (!so.SuccessFlag) throw new ApplicationException($"Error updating pack labels. Error:{so.ServiceException}");
        }


        /// <summary>
        /// Handles the CheckedChanged event of the chkReprint checkbox control.
        /// Enables or disables reprint functionality based on the checkbox's checked state.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void chkReprint_CheckedChanged(object sender, EventArgs e)
        {
            // Enable or disable reprint functionality based on the checkbox's checked state
            EnableReprint(chkReprint.Checked);
        }


        /// <summary>
        /// Enables or disables the reprint functionality based on the specified boolean value.
        /// Adjusts the visibility of controls on the form accordingly.
        /// </summary>
        /// <param name="enable">A boolean value indicating whether to enable or disable the reprint functionality.</param>
        private void EnableReprint(bool enable)
        {
            // Adjust the visibility of controls on the form based on the specified boolean value
            lblOrder.Visible = enable;
            txtOrder.Visible = enable;
            lblMatchText.Visible = enable;
            txtMatch.Visible = enable;
        }


        /// <summary>
        /// Handles the click event of the "Refresh" button.
        /// Refreshes the pack labels displayed on the form based on the current settings.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Call the LoadPackLabels method to refresh the pack labels based on the current settings
            LoadPackLabels(chkReprint.Checked, txtOrder.Text);
        }


        /// <summary>
        /// Rebuilds the filters applied to the ObjectListView.
        /// </summary>
        private void RebuildFilters()
        {
            // Create a list to store the filters
            List<IModelFilter> filters = new List<IModelFilter>();

            // Check if the txtMatch textbox has a value
            if (!String.IsNullOrEmpty(txtMatch.Text))
            {
                // Add a TextMatchFilter to the filters list based on the txtMatch textbox value
                filters.Add(new TextMatchFilter(this.olvBundles, this.txtMatch.Text));
            }

            // Set the AdditionalFilter property of the olvBundles ObjectListView to the composite filter
            this.olvBundles.AdditionalFilter = filters.Count == 0 ? null : new CompositeAllFilter(filters);
        }

        /// <summary>
        /// Handles the text changed event of the txtMatch textbox.
        /// Rebuilds the filters applied to the ObjectListView based on the textbox value.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void txtMatch_TextChanged(object sender, EventArgs e)
        {
            RebuildFilters();
        }

    }
}
