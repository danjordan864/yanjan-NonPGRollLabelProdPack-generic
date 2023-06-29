using RollLabelProdPack.Library.Data;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;
using RollLabelProdPack.SAP.B1;
using RollLabelProdPack.SAP.B1.DocumentObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormUtils;

namespace RollLabelProdPack
{

    /// <summary>
    /// Represents the form for adjusting the Resmix in the production pack.
    /// </summary>
    public partial class FrmAdjustResmix : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        private RollLabelData _selectOrder = new RollLabelData();
        private BindingSource bindingSource1;
        private List<InventoryDetail> _inputLocMaterial;
        private List<InventoryIssueDetail> _plannedIssue;
        private bool _noAdjustmentRequired;
        private List<InventoryDetail> _inputLocMatlMatchingOrder;
        private List<InventoryIssueDetail> _prodOrderLines;

        /// <summary>
        /// Initializes a new instance of the FrmAdjustResmix class.
        /// </summary>
        public FrmAdjustResmix()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the load event of the FrmAdjustResmix form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void FrmAdjustResmix_Load(object sender, EventArgs e)
        {
            // Create a new instance of the BindingSource and set its data source to _selectOrder
            bindingSource1 = new BindingSource();
            bindingSource1.DataSource = _selectOrder;

            // Bind the text properties of various TextBox controls to the corresponding properties of the bindingSource1
            txtEmployee.DataBindings.Add("Text", bindingSource1, "Employee");
            txtOrderNo.DataBindings.Add("Text", bindingSource1, "SAPOrderNo");
            txtShift.DataBindings.Add("Text", bindingSource1, "Shift");
            txtProductionLine.DataBindings.Add("Text", bindingSource1, "ProductionLine");
            txtItemCode.DataBindings.Add("Text", bindingSource1, "ItemCode");
            txtItemName.DataBindings.Add("Text", bindingSource1, "ItemDescription");
        }

        /// <summary>
        /// Handles the click event of the btnSelect button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            // Call the ChangeOrder method when the btnSelect button is clicked
            ChangeOrder();
        }

        /// <summary>
        /// Changes the selected order.
        /// </summary>
        private void ChangeOrder()
        {
            // Open the SelectOrderDialog to choose a new order
            using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
            {
                // Reset the current batch combo box and adjustment flag
                cboCurrentBatch.DataSource = null;
                _noAdjustmentRequired = false;

                // Set the data source of the SelectOrderDialog
                frmSignInDialog.SetDataSource("MIX"); // pass in item group 103 = resmix, 104 = formedfilm

                // Show the dialog and get the result
                DialogResult dr = frmSignInDialog.ShowDialog();

                // Check if the result is OK
                if (dr == DialogResult.OK)
                {
                    // Update the selected order and other fields on the form
                    _selectOrder = frmSignInDialog.SelectOrder;
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                    bindingSource1.DataSource = _selectOrder;

                    // Get the input material for the production line from the packing material location
                    var packingMtlLoc = AppUtility.GetPackingMtlLocation();
                    var so = AppData.GetProdLineInputMaterial(_selectOrder.ProductionLine, packingMtlLoc);
                    if (!so.SuccessFlag)
                        throw new ApplicationException($"Error getting Prod. Line Input Material. Error:{so.ServiceException}");
                    _inputLocMaterial = so.ReturnValue as List<InventoryDetail>;

                    // Get the issue material for the production order
                    so = AppData.GetProdOrderIssueMaterial(_selectOrder.SAPOrderNo, 1);
                    _prodOrderLines = so.ReturnValue as List<InventoryIssueDetail>;
                    if (!so.SuccessFlag)
                        throw new ApplicationException($"Error getting Issue Material. Error:{so.ServiceException}");

                    // Retrieve the batch controlled item from the production order lines
                    var prodOrderLineBatchControlled = _prodOrderLines.Where(p => p.BatchControlled).ToList();
                    if (prodOrderLineBatchControlled.Count() > 1)
                        throw new ApplicationException("There is more than one batch controlled item on the production order.");
                    var prodOrderLine = prodOrderLineBatchControlled.FirstOrDefault();

                    // Find the input location material that matches the item code of the production order line
                    _inputLocMatlMatchingOrder = _inputLocMaterial.Where(i => i.ItemCode == prodOrderLine.ItemCode).ToList();

                    // Check the number of batches in the input location
                    var noOfBatches = _inputLocMatlMatchingOrder.Count();
                    if (noOfBatches <= 3)
                    {
                        // No adjustment is required if there are two batches in the machine and one batch feeding
                        _noAdjustmentRequired = true;
                        DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "No Adjustment needed", $"There are {noOfBatches} batches in input location, no adjustment needed.");
                    }

                    if (!_noAdjustmentRequired)
                    {
                        // Bind the current batch combo box to the available batches
                        cboCurrentBatch.DisplayMember = "Batch";
                        cboCurrentBatch.ValueMember = "Batch";
                        var cboDataSource = _inputLocMaterial.Where(i => !i.PackagingMtl).ToList();
                        cboDataSource.Insert(0, new InventoryDetail { Batch = "Please Select Batch" });
                        cboCurrentBatch.DataSource = cboDataSource;
                        cboCurrentBatch.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the system is ready to produce based on the selected batch.
        /// </summary>
        private void CheckReadyToProduce()
        {
            // Check if a batch is selected
            if (cboCurrentBatch.Text == "Please Select Batch")
            {
                btnAdjust.Enabled = false;
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Select Batch", "Please Select current batch.");
                return;
            }
            else
            {
                _plannedIssue = new List<InventoryIssueDetail>();
                var batchInUseCount = 0;

                // Get the batch controlled item from the production order lines
                var prodOrderLineBatchControlled = _prodOrderLines.Where(p => p.BatchControlled).ToList();
                var prodOrderLine = prodOrderLineBatchControlled.FirstOrDefault();

                // Iterate through the input location material matching the order
                for (int i = _inputLocMatlMatchingOrder.Count - 1; i >= 0; i--)
                {
                    var batch = _inputLocMatlMatchingOrder[i];

                    // Check if the batch is within the range of selected batch and previous batches
                    if (Convert.ToInt32(batch.Batch.Replace("-", string.Empty)) <= Convert.ToInt32(cboCurrentBatch.Text.Replace("-", string.Empty)))
                    {
                        if (batchInUseCount < 3)
                        {
                            batchInUseCount += 1;
                        }
                        else
                        {
                            // Create planned issue detail for batches more than 3 back from the selected batch
                            _plannedIssue.Add(AppUtility.CreatePlannedIssueDetail(batch.ItemCode, batch.UOM, prodOrderLine.BaseEntry, prodOrderLine.BaseLine, batch.StorageLocation, batch.QualityStatus,
                                                      batch.LUID, batch.SSCC, batch.Quantity, batch.Quantity, batch.Batch, 0, batch.BatchControlled, batch.PackagingMtl));
                        }
                    }
                }

                // Check if there are planned issues
                if (_plannedIssue.Count > 0)
                {
                    btnAdjust.Enabled = true;
                    lnkPlannedIssues.Enabled = true;
                }
                else
                {
                    DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "No Adjustment needed", $"There are no batches more than 3 back from selected batch {cboCurrentBatch.Text}.");
                }
            }
        }


        /// <summary>
        /// Adjusts the inventory by issuing resin to the selected order.
        /// </summary>
        private void Adjust()
        {
            // Get the username and password for SAP B1
            var userNamePW = AppUtility.GetUserNameAndPasswordFilm(_selectOrder.ProductionMachineNo);

            using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
            {
                using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
                {
                    // Iterate through the planned issues (batch controlled items)
                    foreach (var plIssue in _plannedIssue.Where(i => i.BatchControlled))
                    {
                        // Add an order issue line for each planned issue
                        invIssue.AddOrderIssueLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.YJNOrder);
                    }

                    // Save the inventory issue
                    if (_plannedIssue.Where(i => i.BatchControlled).Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false)
                    {
                        throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage());
                    }
                }
            }

            DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Resin Issued", "Resin issued to order: {txtOrderNo.Text}");
        }


        /// <summary>
        /// Event handler for the click event of the "Planned Issues" link label.
        /// Opens the "PlannedIssueDialog" form to display the planned issue details.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void lnkPlannedIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
            {
                // Set the data source for the planned issue dialog form
                frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());

                // Show the planned issue dialog form
                DialogResult dr = frmPlannedIssues.ShowDialog();
            }
        }

        /// <summary>
        /// Event handler for the "Adjust" button click event. Performs the resin adjustment process.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnAdjust_Click(object sender, EventArgs e)
        {
            Adjust(); // Call the Adjust method to perform the resin adjustment

            lnkPlannedIssues.Enabled = false;
            btnAdjust.Enabled = false;
        }


        /// <summary>
        /// Displays a toast notification with the specified type, title, and text.
        /// </summary>
        /// <param name="type">The type of the toast notification.</param>
        /// <param name="title">The title of the toast notification.</param>
        /// <param name="text">The text of the toast notification.</param>
        /// <param name="timeOut">The timeout duration in milliseconds (optional, default is 4000ms).</param>
        internal void DisplayToastNotification(ToastNotificationType type, string title, string text, int timeOut = 4000)
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

            // Generate the HTML for the toast notification
            string html = AppUtility.GenerateHTMLToast(title, text, cssClass);

            int imgHeight = m_htmlToast.GetHTMLHeight(html) + 5;
            int imgWidth = this.Width - 25;

            m_htmlToast.SetImgSize(imgWidth, imgHeight);
            m_htmlToast.SetHTML(html);

            // Calculate the location to display the toast notification
            Rectangle rect = this.Bounds;
            Point px = new Point(rect.Left, rect.Bottom);
            Point screenLocation = PointToScreen(px);

            m_htmlToast.SetImgLocation(px.X + offset, px.Y - imgHeight - offset);

            // Show the toast notification for the specified timeout duration
            m_htmlToast.Show(timeOut);
        }


        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboCurrentBatch control.
        /// Checks if the current batch is selected and determines if the system is ready to produce.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void cboCurrentBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckReadyToProduce();
        }
    }
}
