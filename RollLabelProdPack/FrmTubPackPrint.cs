using BrightIdeasSoftware;
using RollLabelProdPack.Library.Data;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;
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
    /// Represents the form for printing Tub Pallet labels.
    /// </summary>
    public partial class FrmTubPackPrint : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmTubPackPrint"/> class.
        /// </summary>
        public FrmTubPackPrint()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the "Load" event of the `FrmTubPackPrint` form.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void FrmTubPackPrint_Load(object sender, EventArgs e)
        {
            // Initialize the form controls and load the Tub Pallet labels

            // Set up the described task column
            SetupDescribedTaskColumn();

            // Enable showing images on sub-items in the ObjectListView
            olvBundles.ShowImagesOnSubItems = true;

            // Set the row height in the ObjectListView
            olvBundles.RowHeight = 100;

            // Set the empty list message in the ObjectListView
            olvBundles.EmptyListMsg = "No Unprinted Tub pallet labels";

            // Disable alternating back colors in the ObjectListView
            olvBundles.UseAlternatingBackColors = false;

            // Disable hot item highlighting in the ObjectListView
            olvBundles.UseHotItem = false;

            // Set the cell edit activation mode in the ObjectListView
            olvBundles.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;

            // Attach the cell edit starting event handler to the ObjectListView
            olvBundles.CellEditStarting += OlvBundlesOnCellEditStarting;

            // Set the small image list for the ObjectListView
            olvBundles.SmallImageList = ilTubPalletPrint;

            // Make the "Print" column editable
            olvColPrint.IsEditable = true;

            // Set up the hyperlink column for showing cases
            SetupShowCasesHyperLinkColumn();

            // Set up the print button
            SetupPrintButton();

            // Load Tub Pallet labels
            LoadTubPalletLabels(false);
        }


        /// <summary>
        /// Sets up the print button column in the ObjectListView.
        /// </summary>
        private void SetupPrintButton()
        {
            // Set the image getter delegate for the print button column
            olvColPrint.ImageGetter = delegate (object rowObject)
            {
                if (rowObject is TubPalletLabel)
                {
                    var tubPalletLabel = rowObject as TubPalletLabel;
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
        /// Loads the Tub Pallet labels and displays them in the ObjectListView.
        /// </summary>
        /// <param name="isReprint">Flag indicating whether it's a reprint operation.</param>
        /// <param name="order">Optional order parameter for filtering the Tub Pallet labels.</param>
        private void LoadTubPalletLabels(bool isReprint, string order = null)
        {
            try
            {
                // Retrieve Tub Pallet labels from the data source
                var so = AppData.GetTubPalletLabels(isReprint, order);
                if (!so.SuccessFlag)
                    throw new ApplicationException($"Error getting Tub Pallet labels. Error: {so.ServiceException}");

                // Cast the return value to a list of TubPalletLabel objects
                var tubPalletLabels = so.ReturnValue as List<TubPalletLabel>;

                // Iterate through each Tub Pallet label
                foreach (var tubPalletLabel in tubPalletLabels)
                {
                    // Set the description of the Tub Pallet label
                    tubPalletLabel.Description = $"Created: {tubPalletLabel.Created.ToString("g")}\r\n{tubPalletLabel.ItemCode} - {tubPalletLabel.Description} Cases: {tubPalletLabel.Qty.ToString("#.##")}\r\nSSCC: {tubPalletLabel.SSCC}";

                    // Retrieve the Tub label Cases for the current Tub Pallet label
                    so = AppData.GetTubPalletLabelCases(tubPalletLabel.ID);
                    if (!so.SuccessFlag)
                        throw new ApplicationException($"Error getting Tub label Cases. Error: {so.ServiceException}");

                    // Cast the return value to a list of Case objects and assign it to the Cases property of the Tub Pallet label
                    tubPalletLabel.Cases = so.ReturnValue as List<Case>;

                    // Validate the pack
                    if (tubPalletLabel.Cases.Count() > tubPalletLabel.MaxCasesPerPack)
                    {
                        tubPalletLabel.ValidMessage = $"Cases on pallet: {tubPalletLabel.Cases.Count().ToString()} exceeded maximum allowed cases per pallet: {tubPalletLabel.MaxCasesPerPack.ToString()} for this item.\r\n";
                    }
                    if (tubPalletLabel.Cases.Select(r => r.ItemCode).Distinct().Count() > 1)
                    {
                        tubPalletLabel.ValidMessage += "Cannot combine different Items in a Pallet.\r\n";
                    }
                    if (tubPalletLabel.Cases.Select(r => r.YJNOrder).Distinct().Count() > 1)
                    {
                        tubPalletLabel.ValidMessage += "Cannot combine different Lots on a Pallet.\r\n";
                    }
                    if (string.IsNullOrEmpty(tubPalletLabel.ValidMessage))
                    {
                        tubPalletLabel.Valid = true;
                    }
                    else
                    {
                        tubPalletLabel.Valid = false;
                    }
                }

                // Clear the existing objects in the ObjectListView and set the Tub Pallet labels as the new objects
                olvBundles.Objects = null;
                olvBundles.SetObjects(tubPalletLabels);
            }
            catch (Exception ex)
            {
                // Display error notification and log the exception
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Load Tub Pallet Labels", $"Exception has occurred in {AppUtility.GetLoggingText()} Load Tub Pallet Labels.\n\n{ex.Message}");
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
                if (rowObject is TubPalletLabel)
                {
                    var tubPalletLabel = rowObject as TubPalletLabel;
                    if (tubPalletLabel.Valid == true)
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
            renderer.ImageList = ilTubPalletPrint;

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

                    // Get the associated TubPalletLabel object
                    var tubPalletLabel = e.RowObject as TubPalletLabel;

                    // Check if the TubPalletLabel is valid and proceed with printing
                    if (tubPalletLabel != null && tubPalletLabel.Valid)
                    {
                        PrintTubPalletLabel(tubPalletLabel);
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
            LoadTubPalletLabels(false);
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
        /// Prints the TubPalletLabel using the specified printer and label format.
        /// </summary>
        /// <param name="tubPalletLabel">The TubPalletLabel to be printed.</param>
        private void PrintTubPalletLabel(TubPalletLabel tubPalletLabel)
        {
            // Retrieve necessary configuration values
            var qty = tubPalletLabel.Qty.ToString("0.00");
            var labelPrintLocPack = AppUtility.GetBTTriggerLoc();
            var labelPrintExtension = AppUtility.GetLabelPrintExtension();
            var fileNameTubPalletLabel = Path.Combine(labelPrintLocPack, "TubPalletLabel" + labelPrintExtension);
            var tubPalletLabelPrinter = AppUtility.GetTubPalletPrinterName();
            var formatFilePathPackLabel = AppUtility.GetPGDefaultTubPalletLabelFormat();

            // Generate the command string for Bartender to print the label
            var sb = new StringBuilder(5000);
            sb.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathPackLabel, tubPalletLabelPrinter);
            sb.AppendLine();
            sb.Append(@"%END%");
            sb.AppendLine();
            sb.Append("Supplier Product, Production Date, Item Desc., Order No., Cust. Part No.(IRMS), Qty, Customer Shipping Lot, SSCC, PMXSSCC");
            sb.AppendLine();

            // Append the label data for the specified number of copies
            for (int i = 0; i < tubPalletLabel.Copies; i++)
            {
                sb.AppendFormat("{0},{1},{2},{3},{4}", tubPalletLabel.ItemCode, tubPalletLabel.ProductionDate.ToShortDateString(), tubPalletLabel.ItemName, tubPalletLabel.YJNOrder, tubPalletLabel.IRMS);
                sb.AppendFormat(",{0},{1},{2},{3}", qty, tubPalletLabel.LotNo, tubPalletLabel.SSCC, tubPalletLabel.PMXSSCC);
                sb.AppendLine();
            }

            // Write the command string to the label file
            using (StreamWriter sw = File.CreateText(fileNameTubPalletLabel))
            {
                sw.Write(sb.ToString());
            }

            // Update the TubPalletLabel status in the database
            var so = AppData.UpdatePackLabel(tubPalletLabel.ID, tubPalletLabel.Qty, "Y");
            if (!so.SuccessFlag) throw new ApplicationException($"Error updating tub pallet labels. Error:{so.ServiceException}");
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
        }


        /// <summary>
        /// Event handler for the click event of the Refresh button.
        /// Reloads the Tub Pallet labels based on the current state of the reprint checkbox and the order text.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTubPalletLabels(chkReprint.Checked, txtOrder.Text);
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
