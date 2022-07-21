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
    public partial class FrmPackPrint : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        private ILog _log;
        private bool _loading;

        public FrmPackPrint()
        {
            InitializeComponent();
            //timer1.Start();
            _log = LogManager.GetLogger(this.GetType());
        }

        private void FrmPackPrint_Load(object sender, EventArgs e)
        {
            _loading = false;
            SetupDescribedTaskColumn();
            olvBundles.ShowImagesOnSubItems = true;
            olvBundles.RowHeight = 100;
            olvBundles.EmptyListMsg = "No Unprinted Pack labels";
            olvBundles.UseAlternatingBackColors = false;
            olvBundles.UseHotItem = false;
            olvBundles.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;
            olvBundles.CellEditStarting += OlvBundlesOnCellEditStarting;

            int printColumnIndex = 0;
            for (; olvBundles.ColumnsInDisplayOrder[printColumnIndex].AspectName != "PrintButtonText"; printColumnIndex++)
                ;
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

            SetupShowRollsHyperLinkColumn();
            SetupPrintButton();

            LoadPackLabels(false);
        }


        private void OlvBundles_ButtonClick(object sender, CellClickEventArgs e)
        {
            if (e.Column.AspectName == "PrintButtonText")
            {
                var packLabel = e.Model as PackLabel;
                if (packLabel != null && packLabel.Valid)
                {
                    try
                    {
                        if (packLabel.TotalWeightEntered)
                        {
                            var so = AppData.GetRollMinMaxKgForItem(packLabel.ItemCode);
                            if (so.SuccessFlag)
                            {
                                RollMinMaxKg minMax = (RollMinMaxKg)so.ReturnValue;
                                if (packLabel.TotalNetKg >= minMax.MinRollKg * packLabel.Rolls.Count &&
                                    packLabel.TotalNetKg <= minMax.MaxRollKg * packLabel.Rolls.Count)
                                {
                                    Cursor = Cursors.WaitCursor;
                                    AdjustRollQuantities(packLabel);
                                    PrintPackLabel(packLabel);
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

        private void SetupPrintButton()
        {
            //olvColPrint.AspectGetter = delegate {
            //    return "Print";
            //};
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
            //olvColPrint.AspectToStringConverter = delegate {
            //    return String.Empty;
            //};
        }

        private void LoadPackLabels(bool isReprint, string order = null)
        {
            //timer1.Stop();
            try
            {
                _loading = true;
                var so = AppData.GetPackLabels(isReprint, order);
                if (!so.SuccessFlag) throw new ApplicationException($"Error getting pack labels. Error:{so.ServiceException}");
                var packLabels = so.ReturnValue as List<PackLabel>;
                foreach (var packLabel in packLabels)
                {
                    packLabel.PropertyChanged += PackLabel_PropertyChanged;
                    packLabel.Description = $"Created: {packLabel.Created.ToString("g")}\r\n{packLabel.ItemCode} - {packLabel.Description} Kgs: {packLabel.Qty.ToString("#.##")}\r\nSSCC: {packLabel.SSCC}";
                    so = AppData.GetPackLabelRolls(packLabel.ID);
                    if (!so.SuccessFlag) throw new ApplicationException($"Error getting pack label Rolls. Error:{so.ServiceException}");
                    packLabel.Rolls = so.ReturnValue as List<Roll>;
                    packLabel.TotalNetKg = packLabel.Rolls.Sum(t => t.NetKg);
                    packLabel.TotalTareKg = packLabel.Rolls.Sum(t => t.TareKg);
                    packLabel.TotalWeight = packLabel.TotalNetKg + packLabel.TotalTareKg;

                    //validate pack
                    if (packLabel.Rolls.Count() > packLabel.MaxRollsPerPack)
                    {
                        packLabel.ValidMessage = $"Rolls in pack: {packLabel.Rolls.Count().ToString()} exceeded maximum allowed rolls per pack: {packLabel.MaxRollsPerPack.ToString()} for this item.\r\n";
                    }
                    if (packLabel.Rolls.Select(r => r.ItemCode).Distinct().Count() > 1)
                    {
                        packLabel.ValidMessage += "Cannot combine different Items in a Pack.\r\n";
                    }
                    if (packLabel.Rolls.Select(r => r.YJNOrder).Distinct().Count() > 1)
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
            finally
            {
                //timer1.Start();
                _loading = false;
            }

        }

        private void PackLabel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PackLabel packLabel = (PackLabel)sender;
            if (e.PropertyName == "TotalWeight")
            {
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
                if (!_loading && !packLabel.TotalWeightEntered)
                {
                    packLabel.TotalWeightEntered = true;
                }
            }
        }

        private void AdjustRollQuantities(PackLabel packLabel)
        {
            try
            {
                var user = AppUtility.GetSAPUser();
                var password = AppUtility.GetSAPPassword();
                using (SAPB1 sapB1 = new SAPB1(user, password))
                {
                    var scrapLocCode = AppUtility.GetScrapLocCode();
                    var scrapGLOffset = AppUtility.GetScrapOffsetCode();
                    var shift = "D";
                    var scrapsAdded = false;
                    var receiptsAdded = false;

                    using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                    {
                        int luid = 0;
                        foreach (var roll in packLabel.Rolls)
                        {
                            if (roll.AdjustKgs < 0m)
                            {
                                roll.ScrapReason = "Bundle weight adjustment";
                                if (string.IsNullOrEmpty(roll.StorLocCode))
                                {
                                    roll.StorLocCode = "BUNDLE";
                                }
                                _log.Debug("About to call AppData.GetLUIDForSSCC:");
                                _log.Debug($"    roll.SSCC = {roll.SSCC}");
                                var so = AppData.GetLUIDForSSCC(roll.SSCC);
                                _log.Debug($"    so.SuccessFlag = {so.SuccessFlag}");
                                if (so.SuccessFlag)
                                {
                                    _log.Debug($"    so.ReturnValue = {so.ReturnValue}");
                                    if (so.ReturnValue is int)
                                    {
                                        luid = (int)so.ReturnValue;
                                    }
                                }
                                else
                                {
                                    _log.Debug($"    so.ServiceException = {so.ServiceException}");
                                }
                                roll.LUID = luid;
                                invIssue.AddScrapIssueLine(roll.ItemCode, Convert.ToDouble(roll.AdjustKgs * -1m), roll.StorLocCode, "RELEASED", roll.RollNo, roll.LUID, roll.SSCC, roll.UOM, roll.YJNOrder, scrapGLOffset, roll.ScrapReason, shift);
                                roll.ScrapReason = string.Empty;
                                roll.Kgs += roll.AdjustKgs;
                                roll.AdjustKgs = 0m;
                                scrapsAdded = true;
                            }
                        }

                        if (scrapsAdded)
                        {
                            if (invIssue.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                        }
                    }

                    using (InventoryReceipt invReceipt = (InventoryReceipt)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry, 0))
                    {
                        int line = 0;
                        int luid = 0;
                        foreach (var roll in packLabel.Rolls)
                        {
                            if (roll.AdjustKgs > 0m)
                            {
                                roll.ScrapReason = "Bundle weight adjustment";
                                if (string.IsNullOrEmpty(roll.StorLocCode))
                                {
                                    roll.StorLocCode = "BUNDLE";
                                }
                                scrapGLOffset = string.Empty;
                                _log.Debug("About to call AppData.GetLUIDForSSCC:");
                                _log.Debug($"    roll.SSCC = {roll.SSCC}");
                                var so = AppData.GetLUIDForSSCC(roll.SSCC);
                                _log.Debug($"    so.SuccessFlag = {so.SuccessFlag}");
                                if (so.SuccessFlag)
                                {
                                    _log.Debug($"    so.ReturnValue = {so.ReturnValue}");
                                    if (so.ReturnValue is int)
                                    {
                                        luid = (int)so.ReturnValue;
                                    }
                                }
                                else
                                {
                                    _log.Debug($"    so.ServiceException = {so.ServiceException}");
                                }
                                roll.LUID = luid;
                                invReceipt.AddLine(0, roll.ItemCode, Convert.ToDouble(roll.AdjustKgs), roll.RollNo.Last(), roll.StorLocCode, "RELEASED", roll.RollNo, roll.LUID, roll.SSCC, "Kgs", packLabel.YJNOrder, false, line++, shift, "Pack", roll.ScrapReason, scrapGLOffset, true);
                                roll.ScrapReason = string.Empty;
                                roll.Kgs += roll.AdjustKgs;
                                roll.AdjustKgs = 0m;
                                receiptsAdded = true;
                            }
                        }
                        if (receiptsAdded)
                        {
                            if (invReceipt.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
                        }
                    }

                }
                packLabel.Qty = packLabel.TotalWeight;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                throw ex;
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
                    //var packLabel = e.RowObject as PackLabel;
                    //if (packLabel != null && packLabel.Valid)
                    //{
                    //    try
                    //    {
                    //        Cursor = Cursors.WaitCursor;
                    //        AdjustRollQuantities(packLabel);
                    //        PrintPackLabel(packLabel);
                    //        LoadPackLabels(chkReprint.Checked, txtOrder.Text);
                    //    }
                    //    finally
                    //    {
                    //        Cursor = Cursors.Arrow;
                    //    }
                    //}
                }
                // RDJ 20220721 Don't require total weight to be entered if reprinting
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
            var qty = packLabel.TotalNetKg.ToString("0.00");
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
                sb.AppendFormat(",{0},{1},{2},{3}", qty, packLabel.LotNo, packLabel.SSCC, packLabel.PMXSSCC);
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
            LoadPackLabels(chkReprint.Checked, txtOrder.Text);
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
