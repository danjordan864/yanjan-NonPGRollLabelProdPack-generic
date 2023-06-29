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
    public partial class SelectOrderDialog_old : Form
    {
        public RollLabelData SelectOrder { get; set; }
        private List<RollLabelData> _orders;
        public SelectOrderDialog_old()
        {
            InitializeComponent();
        }

        public void SetDataSource(int itemGroup)
        {
            try
            {
                var so = AppData.GetOpenProdOrders(itemGroup);
                if (!so.SuccessFlag) throw new ApplicationException("Error getting Production Orders. " + so.ServiceException);
                _orders = so.ReturnValue as List<RollLabelData>;
                var prodLines = _orders.Where(o => o.ProductionLine != null).Select(o => new { o.ProductionLine, o.ProductionMachineNo }).Distinct().OrderBy(o => o.ProductionLine).ToList();
                prodLines.Insert(0, new { ProductionLine = "<-Please select an Production Line->", ProductionMachineNo = "0" });
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
        private void SelectOrderDialog_old_Load(object sender, EventArgs e)
        {
            try
            {
                cboShift.Text = "<-Please Select Shift->";
                cboProductionOrder.Enabled = false;
                var so = AppData.GetOpenProdOrders_old(cboProductionLine.Text);
                if (!so.SuccessFlag) throw new ApplicationException("Error getting Production Orders. " + so.ServiceException);
                _orders = so.ReturnValue as List<RollLabelData>;


                var prodLines = _orders.Where(o => o.ProductionLine != null).Select(o => new { o.ProductionLine, o.ProductionMachineNo }).Distinct().OrderBy(o => o.ProductionLine).ToList();
                prodLines.Insert(0, new { ProductionLine = "<-Please select an Production Line->", ProductionMachineNo = "0" });
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
        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectOrder = _orders.Where(o => o.SAPOrderNo == (int)cboProductionOrder.SelectedValue).FirstOrDefault();
            SelectOrder.Employee = txtEmployee.Text;
            SelectOrder.Shift = cboShift.Text;
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
                var ordersForProductLine = _orders.Where(o => o.ProductionLine == cboProductionLine.Text).ToList();
                ordersForProductLine.Insert(0, new RollLabelData { YJNOrder = "<-Please select an Order->" });
                cboProductionOrder.DisplayMember = "YJNOrder";
                cboProductionOrder.ValueMember = "SAPOrderNo";
                cboProductionOrder.DataSource = ordersForProductLine;
                cboProductionOrder.Enabled = true;
            }
        }

      
    }
}
