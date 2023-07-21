using BrightIdeasSoftware;
using RollLabelProdPack.Library.Data;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WinFormUtils;
using log4net;

namespace RollLabelProdPack
{
    /// <summary>
    /// Represents the form for printing Co-pack Pallet labels.
    /// </summary>
    public partial class FrmCoPackPrint : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        ILog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmCoPackPrint"/> class.
        /// </summary>
        public FrmCoPackPrint()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the "Load" event of the `FrmTubPackPrint` form.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void FrmCoPackPrint_Load(object sender, EventArgs e)
        {
            // Get logger object
            _log = LogManager.GetLogger(this.GetType());

            // Initialize the form controls and load the Tub Pallet labels

            // Set up the described task column
            SetupDescribedTaskColumn();

            // Enable showing images on sub-items in the ObjectListView
            olvBundles.ShowImagesOnSubItems = true;

            // Set the row height in the ObjectListView
            olvBundles.RowHeight = 100;

            // Set the empty list message in the ObjectListView
            olvBundles.EmptyListMsg = "No unprinted pallet labels";

            // Disable alternating back colors in the ObjectListView
            olvBundles.UseAlternatingBackColors = false;

            // Disable hot item highlighting in the ObjectListView
            olvBundles.UseHotItem = false;

            // Set the cell edit activation mode in the ObjectListView
            olvBundles.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;

            // Attach the cell edit starting event handler to the ObjectListView
            olvBundles.CellEditStarting += OlvBundlesOnCellEditStarting;

            // Set the small image list for the ObjectListView
            olvBundles.SmallImageList = ilCoPackPalletPrint;

            // Make the "Print" column editable
            olvColPrint.IsEditable = true;

            // Set up the hyperlink column for showing cases
            //SetupShowCasesHyperLinkColumn();

            // Set up the print button
            SetupPrintButton();

            // Load Co-Pack Pallet labels
            LoadCoPackPalletLabels(false);
        }


        /// <summary>
        /// Sets up the print button column in the ObjectListView.
        /// </summary>
        private void SetupPrintButton()
        {
            // Set the image getter delegate for the print button column
            olvColPrint.ImageGetter = delegate (object rowObject)
            {
                if (rowObject is CoPackPalletLabel)
                {
                    var tubPalletLabel = rowObject as CoPackPalletLabel;
                    if (tubPalletLabel.Valid == true)
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
        }

        /// <summary>
        /// Loads the Co-Pack Pallet labels and displays them in the ObjectListView.
        /// </summary>
        /// <param name="isReprint">Flag indicating whether it's a reprint operation.</param>
        /// <param name="order">Optional order parameter for filtering the Co-Pack Pallet labels.</param>
        private void LoadCoPackPalletLabels(bool isReprint, string order = null)
        {
            try
            {
                // Retrieve Tub Pallet labels from the data source
                var so = AppData.GetCoPackPalletLabels(isReprint, order);
                if (!so.SuccessFlag)
                    throw new ApplicationException($"Error getting Co-Pack Pallet labels. Error: {so.ServiceException}");

                // Cast the return value to a list of CoPackPalletLabel objects
                var coPackPalletLabels = so.ReturnValue as List<CoPackPalletLabel>;

                // Iterate through each Tub Pallet label
                foreach (var coPackPalletLabel in coPackPalletLabels)
                {
                    // Set the description of the Tub Pallet label
                    coPackPalletLabel.Description = $"Created: {coPackPalletLabel.Created.ToString("g")}\r\n{coPackPalletLabel.ItemCode} - {coPackPalletLabel.Description} Cases: {coPackPalletLabel.Qty.ToString("#.##")}\r\nSSCC: {coPackPalletLabel.SSCC}";

                    coPackPalletLabel.Valid = true;
                }

                // Clear the existing objects in the ObjectListView and set the Tub Pallet labels as the new objects
                olvBundles.Objects = null;
                olvBundles.SetObjects(coPackPalletLabels);
            }
            catch (Exception ex)
            {
                // Display error notification and log the exception
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Load Co-Pack Pallet Labels", $"Exception has occurred in {AppUtility.GetLoggingText()} Load Co-Pack Pallet Labels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }

        /// <summary>
        /// Sets up the DescribedTaskColumn in the ObjectListView.
        /// </summary>
        private void SetupDescribedTaskColumn()
        {
            // Create and install a renderer for the DescribedTaskColumn
            this.olvColPackDesc.Renderer = CreateDescribedTaskRenderer();

            // Set the AspectName property to specify the property used for the title
            this.olvColPackDesc.AspectName = "YJNOrder";

            // Set the ImageGetter delegate to determine the image for each row
            this.olvColPackDesc.ImageGetter = delegate (object rowObject)
            {
                if (rowObject is CoPackPalletLabel)
                {
                    var coPackPalletLabel = rowObject as CoPackPalletLabel;
                    if (coPackPalletLabel.Valid == true)
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

            // Set the cell padding to provide spacing around the task and description
            this.olvColPackDesc.CellPadding = new Rectangle(4, 2, 4, 2);
        }

        /// <summary>
        /// Creates and configures a DescribedTaskRenderer for the ObjectListView.
        /// </summary>
        /// <returns>The configured DescribedTaskRenderer instance.</returns>
        private DescribedTaskRenderer CreateDescribedTaskRenderer()
        {
            // Create a new instance of DescribedTaskRenderer
            DescribedTaskRenderer renderer = new DescribedTaskRenderer();

            // Set the ImageList property to provide the renderer with its own collection of images
            renderer.ImageList = ilCoPackPalletPrint;

            // Set the DescriptionAspectName property to specify the property holding the description text
            renderer.DescriptionAspectName = "Description";

            // Customize the formatting of the renderer
            renderer.TitleFont = new Font("Tahoma", 12, FontStyle.Bold);
            renderer.DescriptionFont = new Font("Tahoma", 11);
            renderer.ImageTextSpace = 20;
            renderer.TitleDescriptionSpace = 1;

            // Use GDI text rendering for clearer text (optional)
            renderer.UseGdiTextRendering = true;

            // Optionally, customize the colors for the title and description
            // renderer.TitleColor = Color.DarkBlue;
            // renderer.DescriptionColor = Color.CornflowerBlue;

            return renderer;
        }


        /// <summary>
        /// Event handler for the CellEditStarting event of the ObjectListView's olvBundles.
        /// Handles special cell edit behavior for the print click column.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The CellEditEventArgs containing event data.</param>
        private void OlvBundlesOnCellEditStarting(object sender, CellEditEventArgs e)
        {
            try
            {
                // Check if the cell being edited is the print click column
                if (e.Column == olvColPrint)
                {
                    e.Cancel = true; // Cancel the cell edit

                    // Get the associated CoPackPalletLabel object
                    var coPackPalletLabel = e.RowObject as CoPackPalletLabel;

                    // Check if the TubPalletLabel is valid and proceed with printing
                    if (coPackPalletLabel != null && coPackPalletLabel.Valid)
                    {
                        PrintCoPackPalletLabel(coPackPalletLabel);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Print Tub PalletLabels", $"Exception has occurred in {AppUtility.GetLoggingText()} Load Tub Pallet Labels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Print Tub Pallet Label.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }


        /// <summary>
        /// Sets up the hyperlink column for showing cases.
        /// </summary>
        private void SetupShowCasesHyperLinkColumn()
        {
            // Set the aspect getter to return the text "Show Cases" for all rows
            olvColAction.AspectGetter = delegate
            {
                return "Show Cases";
            };

            // Handle the IsHyperlink event to set the URL for the hyperlink
            olvBundles.IsHyperlink += delegate (object sender, IsHyperlinkEventArgs e)
            {
                // Set the URL to be the same as the displayed text
                e.Url = e.Text;
            };
        }

        /// <summary>
        /// Event handler for the Tick event of timer1.
        /// Loads Tub Pallet Labels periodically.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Call the LoadTubPalletLabels method to load Tub Pallet Labels
            LoadCoPackPalletLabels(false);
        }


        /// <summary>
        /// Displays a toast notification with the specified type, title, and text.
        /// </summary>
        /// <param name="type">The type of the toast notification (Success, Warning, Error).</param>
        /// <param name="title">The title of the toast notification.</param>
        /// <param name="text">The text content of the toast notification.</param>
        /// <param name="timeOut">Optional. The duration for which the toast notification should be displayed in milliseconds. Default is 4000ms.</param>
        public void DisplayToastNotification(ToastNotificationType type, string title, string text, int timeOut = 4000)
        {
            // Close any existing toast notification
            m_htmlToast.Close();

            // Set the CSS class based on the notification type
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

            // Calculate the size of the toast notification based on the HTML content
            int imgHeight = m_htmlToast.GetHTMLHeight(html) + 5;
            int imgWidth = this.Width - 25;

            // Set the size and HTML content of the toast notification
            m_htmlToast.SetImgSize(imgWidth, imgHeight);
            m_htmlToast.SetHTML(html);

            // Calculate the location of the toast notification
            Rectangle rect = this.Bounds;
            Point px = new Point(rect.Left, rect.Bottom);
            Point screenLocation = PointToScreen(px);

            // Set the location of the toast notification
            int offset = 15;
            m_htmlToast.SetImgLocation(px.X + offset, px.Y - imgHeight - offset);

            // Show the toast notification for the specified duration
            m_htmlToast.Show(timeOut);
        }


        /// <summary>
        /// Event handler for the HyperlinkClicked event of the olvBundles ObjectListView.
        /// Displays a dialog with cases information for the clicked TubPalletLabel.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The HyperlinkClickedEventArgs containing event data.</param>
        private void olvBundles_HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
        {
            // Retrieve the TubPalletLabel associated with the clicked hyperlink
            var tubPalletLabel = e.Model as TubPalletLabel;
            if (tubPalletLabel != null)
            {
                // Create and show a FrmCasesDialog to display cases information
                using (FrmCasesDialog frmCases = new FrmCasesDialog())
                {
                    frmCases.SetDataSource(tubPalletLabel.Cases);
                    frmCases.SetValidationMessage(tubPalletLabel.ValidMessage);
                    DialogResult dr = frmCases.ShowDialog();
                    // Optionally handle the dialog result
                }
            }
        }


        /// <summary>
        /// Prints the CoPackPalletLabel using the specified printer and label format.
        /// </summary>
        /// <param name="coPackPalletLabel">The CoPackPalletLabel to be printed.</param>
        private void PrintCoPackPalletLabel(CoPackPalletLabel coPackPalletLabel)
        {
            PrintCoPackPalletLabel(coPackPalletLabel.PMXSSCC, coPackPalletLabel.Copies);
        }

        /// <summary>
        /// Prints a co-pack pallet label for the given pallet SSCC.
        /// </summary>
        /// <param name="palletSSCC">The SSCC of the pallet.</param>
        /// <param name="numberOfCopies">Number of copies to print</param>
        private void PrintCoPackPalletLabel(string palletSSCC, int numberOfCopies)
        {
            try
            {
                // Get the label print location and extension
                var labelPrintLoc = AppUtility.GetBTTriggerLoc();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();

                // Generate a unique file name for the co-pack pallet label
                var fileNameCoPackLabels = Path.Combine(labelPrintLoc, $"CoPackLabel{Guid.NewGuid().ToString()}" + labelPrintExtension);

                // Get the format file path for co-pack labels
                var formatFilePathCoPackLabel = AppUtility.GetPGDefaultCoPackLabelFormat();

                // Create a StringBuilder to build the label content
                var sbMixLabel = new StringBuilder(5000);

                // Get default printer name
                var printer = AppUtility.GetCoPackPalletPrinterName();

                //if (_log.IsDebugEnabled)
                //{
                //    _log.Debug($"_selectOrder.Printer = {_selectOrder.Printer}");
                //}

                // Append the label print command to the StringBuilder
                sbMixLabel.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /C={2} /R=3 /P /DD", formatFilePathCoPackLabel, printer, numberOfCopies);
                sbMixLabel.AppendLine();
                sbMixLabel.Append(@"%END%");
                sbMixLabel.AppendLine();
                sbMixLabel.Append("Item, ItemName, IRMS, LotNo, PMXSSCC, SSCC, Qty, Order");
                sbMixLabel.AppendLine();

                // Get the bundle pack label for the pallet SSCC
                ServiceOutput so = AppData.GetBundlePackLabel(palletSSCC);

                if (so.SuccessFlag)
                {
                    IList<PackLabel> packLabels = (IList<PackLabel>)so.ReturnValue;

                    if (packLabels.Count == 0)
                    {
                        throw new ApplicationException($"No Bundle info for SSCC {palletSSCC}");
                    }
                    else
                    {
                        // Append the label data for the pack label
                        var packLabel = packLabels[0];
                        sbMixLabel.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7}", packLabel.ItemCode, packLabel.ItemName, "", packLabel.LotNo, packLabel.PMXSSCC, packLabel.SSCC, packLabel.Qty, packLabel.SAPOrder);
                        sbMixLabel.AppendLine();
                    }
                }
                else
                {
                    throw new ApplicationException(so.ServiceException);
                }

                // Write the label content to the file
                using (StreamWriter sw = File.CreateText(fileNameCoPackLabels))
                {
                    sw.Write(sbMixLabel.ToString());
                }

                // Display success notification and refresh order information
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Success", "Co-pack pallet label printed. Please check printer.");
            }
            catch (Exception ex)
            {
                // Log and display error notification
                _log.Error(ex.Message, ex);
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Printing Co-Pack Pallet Label", $"Exception has occurred in {AppUtility.GetLoggingText()} PrintRollLabels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} PrintCoPackPalletLabel.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }


        /// <summary>
        /// Event handler for the CheckedChanged event of the chkReprint checkbox.
        /// Enables or disables the reprint functionality based on the checkbox state.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void chkReprint_CheckedChanged(object sender, EventArgs e)
        {
            EnableReprint(chkReprint.Checked);
        }


        /// <summary>
        /// Enables or disables the reprint functionality based on the specified enable state.
        /// </summary>
        /// <param name="enable">A boolean value indicating whether to enable or disable the reprint functionality.</param>
        private void EnableReprint(bool enable)
        {
            lblOrder.Visible = enable;
            txtOrder.Visible = enable;
            lblMatchText.Visible = enable;
            txtMatch.Visible = enable;
            if (enable)
            {
                // Set the empty list message in the ObjectListView
                olvBundles.EmptyListMsg = "No available pallet labels to reprint";
            }
            else
            {
                // Set the empty list message in the ObjectListView
                olvBundles.EmptyListMsg = "No unprinted pallet labels";
            }
        }


        /// <summary>
        /// Event handler for the click event of the Refresh button.
        /// Reloads the Tub Pallet labels based on the current state of the reprint checkbox and the order text.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCoPackPalletLabels(chkReprint.Checked, txtOrder.Text);
        }


        /// <summary>
        /// Rebuilds the filters for the ObjectListView based on the filtering criteria.
        /// </summary>
        private void RebuildFilters()
        {
            // Build a composite filter that unifies the three possible filtering criteria

            List<IModelFilter> filters = new List<IModelFilter>();

            if (!String.IsNullOrEmpty(txtMatch.Text))
                filters.Add(new TextMatchFilter(this.olvBundles, this.txtMatch.Text));

            this.olvBundles.AdditionalFilter = filters.Count == 0 ? null : new CompositeAllFilter(filters);
        }


        /// <summary>
        /// Event handler for the TextChanged event of the txtMatch TextBox.
        /// Rebuilds the filters for the ObjectListView based on the new filtering criteria.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs object.</param>
        private void txtMatch_TextChanged(object sender, EventArgs e)
        {
            RebuildFilters();
        }

    }
}
