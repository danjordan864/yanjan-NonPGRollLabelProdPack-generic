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
    public partial class FrmTubPackPrint : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        public FrmTubPackPrint()
        {
            InitializeComponent();
            //timer1.Start();
        }

        private void FrmTubPackPrint_Load(object sender, EventArgs e)
        {
            SetupDescribedTaskColumn();
            olvBundles.ShowImagesOnSubItems = true;
            olvBundles.RowHeight = 100;
            olvBundles.EmptyListMsg = "No Unprinted Tub pallet labels";
            olvBundles.UseAlternatingBackColors = false;
            olvBundles.UseHotItem = false;
            olvBundles.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;
            olvBundles.CellEditStarting += OlvBundlesOnCellEditStarting;
            olvBundles.SmallImageList = ilTubPalletPrint;
            
            olvColPrint.IsEditable = true;
           

           
            SetupShowCasesHyperLinkColumn();
            SetupPrintButton();
            
            LoadTubPalletLabels(false);
        }

        private void SetupPrintButton()
        {
            //olvColPrint.AspectGetter = delegate {
            //    return "Print";
            //};
            olvColPrint.ImageGetter = delegate (object rowObject) {
                if (rowObject is TubPalletLabel)
                {
                    var tubPalletLabel= rowObject as TubPalletLabel;
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
            //olvColPrint.AspectToStringConverter = delegate {
            //    return String.Empty;
            //};
        }

        private void LoadTubPalletLabels(bool isReprint, string order = null)
        {
            //timer1.Stop();
            try
            {
                var so = AppData.GetTubPalletLabels(isReprint,order);
                if (!so.SuccessFlag) throw new ApplicationException($"Error getting Tub Pallet labels. Error:{so.ServiceException}");
                var tubPalletLabels = so.ReturnValue as List<TubPalletLabel>;
                foreach (var tubPalletLabel in tubPalletLabels)
                {
                    tubPalletLabel.Description = $"Created: {tubPalletLabel.Created.ToString("g")}\r\n{tubPalletLabel.ItemCode} - {tubPalletLabel.Description} Cases: {tubPalletLabel.Qty.ToString("#.##")}\r\nSSCC: {tubPalletLabel.SSCC}";
                    so = AppData.GetTubPalletLabelCases(tubPalletLabel.ID);
                    if (!so.SuccessFlag) throw new ApplicationException($"Error getting Tub label Cases. Error:{so.ServiceException}");
                    tubPalletLabel.Cases = so.ReturnValue as List<Case>;
                    //validate pack
                    if (tubPalletLabel.Cases.Count() > tubPalletLabel.MaxCasesPerPack)
                    {
                        tubPalletLabel.ValidMessage = $"Cases on pallet: {tubPalletLabel.Cases.Count().ToString()} exceeded maximum allowed cases per pallet: {tubPalletLabel.MaxCasesPerPack.ToString()} for this item.\r\n";
                    }
                    if (tubPalletLabel.Cases.Select(r =>r.ItemCode).Distinct().Count()>1)
                    {
                        tubPalletLabel.ValidMessage += "Cannot combine different Items in a Pallet.\r\n";
                    }
                    if (tubPalletLabel.Cases.Select(r=>r.YJNOrder).Distinct().Count() > 1)
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
                olvBundles.Objects = null;
                olvBundles.SetObjects(tubPalletLabels);
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Load Tub Pallet Labels", $"Exception has occurred in {AppUtility.GetLoggingText()} Load Tub Pallet Labels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
            finally
            {
                //timer1.Start();
            }
            
        }
        private void SetupDescribedTaskColumn()
        {
            // Setup a described task renderer, which draws a large icon
            // with a title, and a description under the title.
            // Almost all of this configuration could be done through the Designer
            // but I've done it through code that make it clear what's going on.

            // Create and install an appropriately configured renderer 
            this.olvColPackDesc.Renderer = CreateDescribedTaskRenderer();

            // Now let's setup the couple of other bits that the column needs

            // Tell the column which property should be used to get the title
            this.olvColPackDesc.AspectName = "YJNOrder";

            // Tell the column which property holds the identifier for the image for row.
            // We could also have installed an ImageGetter
            //this.olvColPackDesc.ImageAspectName = "ValidImage";
            olvColPackDesc.ImageGetter = delegate (object rowObject) {
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
            // Put a little bit of space around the task and its description
            this.olvColPackDesc.CellPadding = new Rectangle(4, 2, 4, 2);
        }
        private DescribedTaskRenderer CreateDescribedTaskRenderer()
        {

            // Let's create an appropriately configured renderer.
            DescribedTaskRenderer renderer = new DescribedTaskRenderer();

            // Give the renderer its own collection of images.
            // If this isn't set, the renderer will use the SmallImageList from the ObjectListView.
            // (this is standard Renderer behaviour, not specific to DescribedTaskRenderer).
            renderer.ImageList = ilTubPalletPrint;

            // Tell the renderer which property holds the text to be used as a description
            renderer.DescriptionAspectName = "Description";

            // Change the formatting slightly
            renderer.TitleFont = new Font("Tahoma", 12, FontStyle.Bold);
            renderer.DescriptionFont = new Font("Tahoma", 11);
            renderer.ImageTextSpace = 20;
            renderer.TitleDescriptionSpace = 1;

            // Use older Gdi renderering, since most people think the text looks clearer
            renderer.UseGdiTextRendering = true;

            // If you like colours other than black and grey, you could uncomment these
            //            renderer.TitleColor = Color.DarkBlue;
            //            renderer.DescriptionColor = Color.CornflowerBlue;

            return renderer;
        }

        private void OlvBundlesOnCellEditStarting(object sender, CellEditEventArgs e)
        {
            try
            {
                // special cell edit handling for our print click
                if (e.Column == olvColPrint)
                {
                    e.Cancel = true;        // we don't want to edit anything
                    var tubPalletLabel = e.RowObject as TubPalletLabel;
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

        private void SetupShowCasesHyperLinkColumn()
        {
            olvColAction.AspectGetter = delegate
            {
                return "Show Cases";
            };
            olvBundles.IsHyperlink += delegate (object sender, IsHyperlinkEventArgs e)
            {
                e.Url = e.Text;
            };
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadTubPalletLabels(false);
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

        private void olvBundles_HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
        {
            var tubPalletLabel = e.Model as TubPalletLabel;
            if (tubPalletLabel != null)
            {
                using (FrmCasesDialog frmCases = new FrmCasesDialog())
                {
                    frmCases.SetDataSource(tubPalletLabel.Cases);
                    frmCases.SetValidationMessage(tubPalletLabel.ValidMessage);
                    DialogResult dr = frmCases.ShowDialog();
                }
            }
               
        }

        private void PrintTubPalletLabel(TubPalletLabel tubPalletLabel)
        {
            var qty = tubPalletLabel.Qty.ToString("0.00");
            var labelPrintLocPack = AppUtility.GetBTTriggerLoc();
            var labelPrintExtension = AppUtility.GetLabelPrintExtension();
            var fileNameTubPalletLabel = Path.Combine(labelPrintLocPack, "TubPalletLabel" + labelPrintExtension);
            var tubPalletLabelPrinter = AppUtility.GetTubPalletPrinterName();
            var formatFilePathPackLabel = AppUtility.GetPGDefaultTubPalletLabelFormat();

            var sb = new StringBuilder(5000);
            sb.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathPackLabel, tubPalletLabelPrinter);
            sb.AppendLine();
            sb.Append(@"%END%");
            sb.AppendLine();
            sb.Append("Supplier Product, Production Date, Item Desc., Order No., Cust. Part No.(IRMS), Qty, Customer Shipping Lot, SSCC, PMXSSCC");
            sb.AppendLine();
            for (int i = 0; i < tubPalletLabel.Copies; i++)
            {
                sb.AppendFormat("{0},{1},{2},{3},{4}", tubPalletLabel.ItemCode, tubPalletLabel.ProductionDate.ToShortDateString(), tubPalletLabel.ItemName, tubPalletLabel.YJNOrder, tubPalletLabel.IRMS);
                sb.AppendFormat(",{0},{1},{2},{3}", qty, tubPalletLabel.LotNo, tubPalletLabel.SSCC, tubPalletLabel.PMXSSCC);
                sb.AppendLine();
            }
            using (StreamWriter sw = File.CreateText(fileNameTubPalletLabel))
            {
                sw.Write(sb.ToString());
            }
            var so = AppData.UpdatePackLabel(tubPalletLabel.ID, tubPalletLabel.Qty, "Y");
            if (!so.SuccessFlag) throw new ApplicationException($"Error updating tub pallet labels. Error:{so.ServiceException}");
        }

        private void chkReprint_CheckedChanged(object sender, EventArgs e)
        {
            EnableReprint(chkReprint.Checked);        
        }

        private void EnableReprint(bool enable)
        {
            lblOrder.Visible = enable;
            txtOrder.Visible = enable;
            lblMatchText.Visible = enable;
            txtMatch.Visible = enable;
            //if (enable)
            //{
            //    timer1.Stop();
            //}  
            //else
            //{
            //    timer1.Start();
            //}
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTubPalletLabels(chkReprint.Checked,txtOrder.Text);
        }

        private void RebuildFilters()
        {
            // Build a composite filter that unify the three possible filtering criteria

            List<IModelFilter> filters = new List<IModelFilter>();

            if (!String.IsNullOrEmpty(txtMatch.Text))
                filters.Add(new TextMatchFilter(this.olvBundles, this.txtMatch.Text));

            this.olvBundles.AdditionalFilter = filters.Count == 0 ? null : new CompositeAllFilter(filters);
        }

        private void txtMatch_TextChanged(object sender, EventArgs e)
        {
            RebuildFilters();
        }
    }
}
