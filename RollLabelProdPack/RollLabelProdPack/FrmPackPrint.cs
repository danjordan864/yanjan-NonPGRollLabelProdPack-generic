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
    public partial class FrmPackPrint : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        public FrmPackPrint()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void FrmPackPrint_Load(object sender, EventArgs e)
        {
            SetupDescribedTaskColumn();
            olvBundles.ShowImagesOnSubItems = true;
            olvBundles.RowHeight = 100;
            olvBundles.EmptyListMsg = "No Unprinted Pack labels";
            olvBundles.UseAlternatingBackColors = false;
            olvBundles.UseHotItem = false;
            olvBundles.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;
            olvBundles.CellEditStarting += OlvBundlesOnCellEditStarting;
            olvBundles.SmallImageList = ilPackPrint;
            
            olvColPrint.IsEditable = true;
           

           
            SetupShowRollsHyperLinkColumn();
            SetupPrintButton();
            
            LoadPackLabels(false);
        }

        private void SetupPrintButton()
        {
            //olvColPrint.AspectGetter = delegate {
            //    return "Print";
            //};
            olvColPrint.ImageGetter = delegate (object rowObject) {
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
            //olvColPrint.AspectToStringConverter = delegate {
            //    return String.Empty;
            //};
        }

        private void LoadPackLabels(bool isReprint, string order = null)
        {
            try
            {
                var so = AppData.GetPackLabels(isReprint,order);
                if (!so.SuccessFlag) throw new ApplicationException($"Error getting pack labels. Error:{so.ServiceException}");
                var packLabels = so.ReturnValue as List<PackLabel>;
                foreach (var packLabel in packLabels)
                {
                    packLabel.Description = $"Created: {packLabel.Created.ToString("g")}\r\n{packLabel.ItemCode} - {packLabel.Description} Kgs: {packLabel.Qty.ToString("#.##")}\r\nSSCC: {packLabel.SSCC}";
                    so = AppData.GetPackLabelRolls(packLabel.ID);
                    if (!so.SuccessFlag) throw new ApplicationException($"Error getting pack label Rolls. Error:{so.ServiceException}");
                    packLabel.Rolls = so.ReturnValue as List<Roll>;
                    //validate pack
                    if (packLabel.Rolls.Count() > packLabel.MaxRollsPerPack)
                    {
                        packLabel.ValidMessage = $"Rolls in pack: {packLabel.Rolls.Count().ToString()} exceeded maximum allowed rolls per pack: {packLabel.MaxRollsPerPack.ToString()} for this item.\r\n";
                    }
                    if (packLabel.Rolls.Select(r =>r.ItemCode).Distinct().Count()>1)
                    {
                        packLabel.ValidMessage += "Cannot combine different Items in a Pack.\r\n";
                    }
                    if (packLabel.Rolls.Select(r=>r.YJNOrder).Distinct().Count() > 1)
                    {
                        packLabel.ValidMessage += "Cannot combine different Lots in a Pack.\r\n";
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
                olvBundles.Objects = null;
                olvBundles.SetObjects(packLabels);
            }
            catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Load Pack Labels", $"Exception has occurred in {AppUtility.GetLoggingText()} Load Pack Labels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Create Click.\n\n{ex.Message}", EventLogEntryType.Error, true);
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
            renderer.ImageList = ilPackPrint;

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
                    var packLabel = e.RowObject as PackLabel;
                    if (packLabel != null && packLabel.Valid)
                    {
                        PrintPackLabel(packLabel);
                    }
                }
            }
             catch (Exception ex)
            {
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Error, "Print Pack Labels", $"Exception has occurred in {AppUtility.GetLoggingText()} Load Pack Labels.\n\n{ex.Message}");
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} Print Pack Label.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
      
        }

        private void SetupShowRollsHyperLinkColumn()
        {
            olvColAction.AspectGetter = delegate
            {
                return "Show Rolls";
            };
            olvBundles.IsHyperlink += delegate (object sender, IsHyperlinkEventArgs e)
            {
                e.Url = e.Text;
            };
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadPackLabels(false);
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
            var packLabel = e.Model as PackLabel;
            if (packLabel != null)
            {
                using (FrmRollsDialog frmRolls = new FrmRollsDialog())
                {
                    frmRolls.SetDataSource(packLabel.Rolls);
                    frmRolls.SetValidationMessage(packLabel.ValidMessage);
                    DialogResult dr = frmRolls.ShowDialog();
                }
            }
               
        }

        private void PrintPackLabel(PackLabel packLabel)
        {
            var qty = packLabel.Qty.ToString("0.00");
            var labelPrintLocPack = AppUtility.GetBTTriggerLoc();
            var labelPrintExtension = AppUtility.GetLabelPrintExtension();
            var fileNamePackLabel = Path.Combine(labelPrintLocPack, "PackLabel" + labelPrintExtension);
            var packLabelPrinter = AppUtility.GetPackPrinterName();
            var formatFilePathPackLabel = AppUtility.GetPGDefaultPackLabelFormat();

            var sb = new StringBuilder(5000);
            sb.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathPackLabel, packLabelPrinter);
            sb.AppendLine();
            sb.Append(@"%END%");
            sb.AppendLine();
            sb.Append("Supplier Product, Production Date, Item Desc., Order No., Cust. Part No.(IRMS), Qty, Customer Shipping Lot, SSCC, PMXSSCC");
            sb.AppendLine();
            for (int i = 0; i < packLabel.Copies; i++)
            {
                sb.AppendFormat("{0},{1},{2},{3},{4}", packLabel.ItemCode, packLabel.ProductionDate.ToShortDateString(), packLabel.ItemName, packLabel.YJNOrder, packLabel.IRMS);
                sb.AppendFormat(",{0},{1},{2},{3}", qty,packLabel.LotNo,packLabel.SSCC,packLabel.PMXSSCC);
                sb.AppendLine();
            }
            using (StreamWriter sw = File.CreateText(fileNamePackLabel))
            {
                sw.Write(sb.ToString());
            }
            var so = AppData.UpdatePackLabel(packLabel.ID, packLabel.Qty, "Y");
            if (!so.SuccessFlag) throw new ApplicationException($"Error updating pack labels. Error:{so.ServiceException}");
        }

        private void chkReprint_CheckedChanged(object sender, EventArgs e)
        {
            EnableReprint(chkReprint.Checked);        
        }

        private void EnableReprint(bool enable)
        {
            lblOrder.Visible = enable;
            txtOrder.Visible = enable;
            btnRefresh.Visible = enable;
            lblMatchText.Visible = enable;
            txtMatch.Visible = enable;
            if (enable)
            {
                timer1.Stop();
            }  
            else
            {
                timer1.Start();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var isReprint = true;
            LoadPackLabels(isReprint,txtOrder.Text);
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
