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

namespace RollLabelProdPack
{
    public partial class FrmPack : Form
    {
        private RollLabelData _selectOrder = null;
        private BindingSource bindingSource1;
        public FrmPack()
        {
            InitializeComponent();
        }

        private void FrmPack_Load(object sender, EventArgs e)
        {
            bindingSource1 = new BindingSource();
            ChangeOrder();
            txtEmployee.DataBindings.Add("Text", bindingSource1, "Employee");
            txtYJNProdOrder.DataBindings.Add("Text", bindingSource1, "YJNOrder");
            txtOrderNo.DataBindings.Add("Text", bindingSource1, "SAPOrderNo");
            txtShift.DataBindings.Add("Text", bindingSource1, "Shift");
            txtProductionLine.DataBindings.Add("Text", bindingSource1, "ProductionLine");
            txtItemCode.DataBindings.Add("Text", bindingSource1, "ItemCode");
            txtItemName.DataBindings.Add("Text", bindingSource1, "ItemDescription");
            txtJumboRoll.DataBindings.Add("Text", bindingSource1, "JumboRollNo");
            txtIRMS.DataBindings.Add("Text", bindingSource1, "IRMS");
        }

        private void GetNextPallet()
        {
            try
            {
                var so = AppData.NewPGPalletNo();
                if (!so.SuccessFlag) throw new ApplicationException($"Error getting new Pallet. {so.ServiceException}");
                txtPalletNo.Text = ((int)so.ReturnValue).ToString();
            }
            catch (Exception ex)
            {
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} ChangeOrder.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
        }

        private void ChangeOrder()
        {
            try
            {
                using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
                {
                    
                    DialogResult dr = frmSignInDialog.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        _selectOrder = frmSignInDialog.SelectOrder;
                        bindingSource1.DataSource = _selectOrder;
                        txtQty.Text = "0";
                        GetNextPallet();
                        RefreshProdDate();
                    }
                    else
                    {
                        MessageBox.Show("Please Complete all fields.");
                    }
                }
            }
            catch (Exception ex)
            {
                AppUtility.WriteToEventLog($"Exception has occurred in {AppUtility.GetLoggingText()} ChangeOrder.\n\n{ex.Message}", EventLogEntryType.Error, true);
            }
            
        }

        private void RefreshProdDate()
        {
            var currentDateTime = DateTime.Now;
            txtProductionDateFull.Text = currentDateTime.ToShortDateString();
            _selectOrder.ProductionYear = currentDateTime.Year.ToString().Last().ToString();
            _selectOrder.ProductionMonth = AppUtility.GetYanJanProdMo(currentDateTime);
            _selectOrder.ProductionDate = currentDateTime.Day.ToString("00");
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }

        private void btnPrintPackLabel_Click(object sender, EventArgs e)
        {
            var qty = Decimal.Parse(txtQty.Text).ToString("0.00");
            var qtyNoDecmial = qty.Replace(".", "");
            var labelPrintLocPack = AppUtility.GetBTTriggerLoc();
            var labelPrintExtension = AppUtility.GetLabelPrintExtension();
            var fileNamePackLabel = Path.Combine(labelPrintLocPack, "PackLabel" + labelPrintExtension);
            var packLabelPrinter = AppUtility.GetPackPrinterName();
            var lotWithPrefix = AppUtility.GetSupplierId() + _selectOrder.YJNOrder;
            var palletId = txtPalletNo.Text;
                var formatFilePathPackLabel = AppUtility.GetPGDefaultPackLabelFormat();

            var sb = new StringBuilder(5000);
            sb.AppendFormat(@"%BTW% /AF=""{0}"" /D=""%Trigger File Name%"" /PRN=""{1}"" /R=3 /P /DD", formatFilePathPackLabel, packLabelPrinter);
            sb.AppendLine();
            sb.Append(@"%END%");
            sb.AppendLine();
            sb.Append("Supplier Product, Production Date, Item Desc., Order No., Cust. Part No.(IRMS), Qty, QtyNoDecimal, Customer Shipping Lot, LotWithPrefix, PalletId");
            sb.AppendLine();
            for (int i = 0; i < nudCopiesPack.Value; i++)
            {
                sb.AppendFormat("{0},{1},{2},{3},{4}", _selectOrder.ItemCode, txtProductionDateFull.Text, _selectOrder.ItemDescription, _selectOrder.YJNOrder, _selectOrder.IRMS);
                sb.AppendFormat(",{0},{1},{2},{3},{4}", qty, qtyNoDecmial, _selectOrder.YJNOrder,lotWithPrefix, palletId);
                sb.AppendLine();
            }
            using (StreamWriter sw = File.CreateText(fileNamePackLabel))
            {
                sw.Write(sb.ToString());
            }
            //tstbResults.Text = "Pack label printed. Please check printer.";
            txtQty.Text = "0";
            GetNextPallet();
        }

    }
}
