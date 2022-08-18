using RollLabelProdPack.Library.Data;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RollLabelProdPack
{
    public partial class SelectOrderDialog : Form
    {
        public RollLabelData SelectOrder { get; set; }
        private List<RollLabelData> _orders;
        private string _itemGroupFilter = string.Empty;

        public SelectOrderDialog()
        {
            InitializeComponent();
        }

        public void SetDataSource(string itemGroupFilter)
        {
            try
            {
                // RDJ 20220812 - Save off item group for validating selections
                _itemGroupFilter = itemGroupFilter;
                var so = AppData.GetOpenProdOrders(itemGroupFilter);
                if (!so.SuccessFlag) throw new ApplicationException("Error getting Production Orders. " + so.ServiceException);
                _orders = so.ReturnValue as List<RollLabelData>;
                List<ProductionLineMachineNo> prodLines = new List<ProductionLineMachineNo>();
                prodLines = _orders.Where(o => o.ProductionLine != null).Select(o => new ProductionLineMachineNo { ProductionLine = o.ProductionLine, ProductionMachineNo = o.ProductionMachineNo, InputLocationCode = "", OutputLocationCode = "", Printer = "" }).OrderBy(o => o.ProductionLine).Distinct().ToList();
                // RDJ 20220812 - If we are selecting TUB orders, get the list of production lines that are associated with tubs.
                if (_itemGroupFilter == "TUB")
                {
                    so = AppData.GetProdLines(false);
                    if (!so.SuccessFlag) throw new ApplicationException("Error getting tub production lines. " + so.ServiceException);
                    List<ProductionLine> tubLines = ((List<ProductionLine>)so.ReturnValue).Where(t => t.Code.StartsWith("TUB")).ToList();
                    prodLines = tubLines.Select(t => new ProductionLineMachineNo { ProductionLine = t.Code, ProductionMachineNo = t.LineNo, InputLocationCode = t.InputLocationCode, OutputLocationCode = t.OutputLocationCode, Printer = t.Printer }).OrderBy(t => t.ProductionLine).ToList();
                }
                prodLines.Insert(0, new ProductionLineMachineNo { ProductionLine = "<-Please select an Production Line->", ProductionMachineNo = "0" });
                cboProductionLine.DisplayMember = "ProductionLine";
                cboProductionLine.ValueMember = "ProductionMachineNo";
                cboProductionLine.DataSource = prodLines;
            }
            catch (Exception ex)
            {
                AppUtility.WriteToEventLog($"Form Load error: {ex.Message}", System.Diagnostics.EventLogEntryType.Error, false);
                throw;
            }
        }
        private void SelectOrderDialog_Load(object sender, EventArgs e)
        {
            cboShift.Text = "<-Please Select Shift->";
            cboProductionOrder.Enabled = false;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectOrder = _orders.Where(o => o.SAPOrderNo == (int)cboProductionOrder.SelectedValue).FirstOrDefault();
            SelectOrder.Employee = txtEmployee.Text;
            SelectOrder.Shift = cboShift.Text;
            // RDJ 20220812 - In the case of TUB orders, the production line is not necessarily the one associated with the production
            // order. Use the line that was selected instead.
            if (_itemGroupFilter == "TUB")
            {
                ProductionLineMachineNo selectedLineMachineNo = (ProductionLineMachineNo)cboProductionLine.SelectedItem;
                SelectOrder.ProductionLine = selectedLineMachineNo.ProductionLine;
                SelectOrder.ProductionMachineNo = selectedLineMachineNo.ProductionMachineNo;
                SelectOrder.InputLoc = selectedLineMachineNo.InputLocationCode;
                SelectOrder.OutputLoc = selectedLineMachineNo.OutputLocationCode;
                SelectOrder.Printer = selectedLineMachineNo.Printer;
            }

            if (ValidateSelectedOrder())
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.None;
            }
        }

        private bool ValidateSelectedOrder()
        {
            return SelectOrder.ProductionLine != "<-Please select an Production Line->" &&
                SelectOrder.YJNOrder != "<-Please select an Order->" &&
                !string.IsNullOrEmpty(SelectOrder.Employee) &&
                SelectOrder.Shift != "<-Please Select Shift->";
        }



        private void cboProductionLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProductionLine.Text != "<-Please select an Production Line->")
            {
                // RDJ 20220812 - If this is for TUBs then any TUB production line is OK
                var ordersForProductLine = _orders.Where(o => _itemGroupFilter == "TUB" || o.ProductionLine == cboProductionLine.Text).ToList();
                ordersForProductLine.Insert(0, new RollLabelData { YJNOrder = "<-Please select an Order->" });
                cboProductionOrder.DisplayMember = "OrderDisplay";
                cboProductionOrder.ValueMember = "SAPOrderNo";
                cboProductionOrder.DataSource = ordersForProductLine;
                cboProductionOrder.Enabled = true;
            }
        }
    }
}
