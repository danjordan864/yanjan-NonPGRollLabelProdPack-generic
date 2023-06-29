using RollLabelProdPack.Library.Data;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace RollLabelProdPack
{
    /// <summary>
    /// Represents a dialog for selecting an order.
    /// </summary>
    public partial class SelectOrderDialog : Form
    {
        /// <summary>
        /// Gets or sets the selected order.
        /// </summary>
        public RollLabelData SelectOrder { get; set; }

        private List<RollLabelData> _orders;
        private string _itemGroupFilter = string.Empty;

        /// <summary>
        /// Initializes a new instance of the SelectOrderDialog class.
        /// </summary>
        public SelectOrderDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the data source for the SelectOrderDialog.
        /// </summary>
        /// <param name="itemGroupFilter">The item group filter to apply.</param>
        internal void SetDataSource(string itemGroupFilter)
        {
            try
            {
                // RDJ 20220812 - Save off item group for validating selections
                _itemGroupFilter = itemGroupFilter;

                // Get the open production orders based on the item group filter
                var so = AppData.GetOpenProdOrders(itemGroupFilter);

                // Throw an exception if there was an error getting the production orders
                if (!so.SuccessFlag)
                    throw new ApplicationException("Error getting Production Orders. " + so.ServiceException);

                // Cast the return value as a list of RollLabelData objects
                _orders = so.ReturnValue as List<RollLabelData>;

                // Create a list of ProductionLineMachineNo objects based on the available production lines
                List<ProductionLineMachineNo> prodLines = new List<ProductionLineMachineNo>();
                prodLines = _orders
                    .Where(o => o.ProductionLine != null)
                    .Select(o => new ProductionLineMachineNo { ProductionLine = o.ProductionLine, ProductionMachineNo = o.ProductionMachineNo, InputLocationCode = "", OutputLocationCode = "", Printer = "" })
                    .OrderBy(o => o.ProductionLine)
                    .Distinct()
                    .ToList();

                // If the item group filter is "TUB", retrieve the production lines associated with tubs
                if (_itemGroupFilter == "TUB")
                {
                    so = AppData.GetProdLines(false);

                    // Throw an exception if there was an error getting the tub production lines
                    if (!so.SuccessFlag)
                        throw new ApplicationException("Error getting tub production lines. " + so.ServiceException);

                    // Get the tub lines and create corresponding ProductionLineMachineNo objects
                    List<ProductionLine> tubLines = ((List<ProductionLine>)so.ReturnValue).Where(t => t.Code.StartsWith("TUB")).ToList();
                    prodLines = tubLines.Select(t => new ProductionLineMachineNo { ProductionLine = t.Code, ProductionMachineNo = t.LineNo, InputLocationCode = t.InputLocationCode, OutputLocationCode = t.OutputLocationCode, Printer = t.Printer })
                        .OrderBy(t => t.ProductionLine)
                        .ToList();

                    // Prepend a generic "TUB" line for future TUB orders
                    prodLines.Insert(0, new ProductionLineMachineNo
                    {
                        ProductionLine = AppUtility.GetGenericTubLineCode(),
                        ProductionMachineNo = AppUtility.GetGenericTubLineMachineNo(),
                        InputLocationCode = AppUtility.GetGenericTubInputLocationCode(),
                        OutputLocationCode = AppUtility.GetGenericTubOutputLocationCode(),
                        Printer = AppUtility.GetGenericTubPrinter()
                    });
                }

                // Insert a default selection option for the production line
                prodLines.Insert(0, new ProductionLineMachineNo { ProductionLine = "<-Please select a Production Line->", ProductionMachineNo = "0" });

                // Set the data source for the cboProductionLine ComboBox
                cboProductionLine.DisplayMember = "ProductionLine";
                cboProductionLine.ValueMember = "ProductionMachineNo";
                cboProductionLine.DataSource = prodLines;
            }
            catch (Exception ex)
            {
                // Log and re-throw the exception if an error occurred
                AppUtility.WriteToEventLog($"Form Load error: {ex.Message}", System.Diagnostics.EventLogEntryType.Error, false);
                throw;
            }
        }

        /// <summary>
        /// Event handler for the SelectOrderDialog's Load event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SelectOrderDialog_Load(object sender, EventArgs e)
        {
            // Set the default text for the cboShift ComboBox
            cboShift.Text = "<-Please Select Shift->";

            // Disable the cboProductionOrder ComboBox
            cboProductionOrder.Enabled = false;
        }

        /// <summary>
        /// Event handler for the OK button click event.
        /// </summary>
        /// <param name="sender">The event sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            // Get the selected production order based on the selected value in the cboProductionOrder ComboBox
            SelectOrder = _orders.Where(o => o.SAPOrderNo == (int)cboProductionOrder.SelectedValue).FirstOrDefault();

            // Set additional properties of the selected order
            SelectOrder.Employee = txtEmployee.Text;
            SelectOrder.Shift = cboShift.Text;

            // If the selected order is for TUBs, use the production line selected instead of the one associated with the order
            if (_itemGroupFilter == "TUB")
            {
                ProductionLineMachineNo selectedLineMachineNo = (ProductionLineMachineNo)cboProductionLine.SelectedItem;
                SelectOrder.ProductionLine = selectedLineMachineNo.ProductionLine;
                SelectOrder.ProductionMachineNo = selectedLineMachineNo.ProductionMachineNo;
                SelectOrder.InputLoc = selectedLineMachineNo.InputLocationCode;
                SelectOrder.OutputLoc = selectedLineMachineNo.OutputLocationCode;
                SelectOrder.Printer = selectedLineMachineNo.Printer;
            }

            // Validate the selected order
            if (ValidateSelectedOrder())
            {
                // Set the DialogResult to OK and close the dialog
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                // Set the DialogResult to None to prevent closing the dialog
                this.DialogResult = DialogResult.None;
            }
        }

        /// <summary>
        /// Validates the selected order in the SelectOrderDialog.
        /// </summary>
        /// <returns><c>true</c> if the selected order is valid; otherwise, <c>false</c>.</returns>
        private bool ValidateSelectedOrder()
        {
            // Ensure that a production line, YJN order, employee, and shift are selected
            return SelectOrder.ProductionLine != "<-Please select a Production Line->" &&
                SelectOrder.YJNOrder != "<-Please select an Order->" &&
                !string.IsNullOrEmpty(SelectOrder.Employee) &&
                SelectOrder.Shift != "<-Please Select Shift->";
        }

        /// <summary>
        /// Event handler for the SelectedIndexChanged event of the cboProductionLine ComboBox.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void cboProductionLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProductionLine.Text != "<-Please select a Production Line->")
            {
                // Retrieve the orders for the selected production line
                // If _itemGroupFilter is "TUB", only consider orders with a matching production line
                var ordersForProductLine = _orders.Where(o => o.ProductionLine == cboProductionLine.Text).ToList();

                // Insert a default selection option for the production order
                ordersForProductLine.Insert(0, new RollLabelData { YJNOrder = "<-Please select an Order->" });

                // Set the data source for the cboProductionOrder ComboBox
                cboProductionOrder.DisplayMember = "OrderDisplay";
                cboProductionOrder.ValueMember = "SAPOrderNo";
                cboProductionOrder.DataSource = ordersForProductLine;

                // Enable the cboProductionOrder ComboBox
                cboProductionOrder.Enabled = true;
            }
        }
    }
}
