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
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

namespace RollLabelProdPack
{
    public partial class FrmCreateRollLabels : Form
    {
        public FrmCreateRollLabels()
        {
            InitializeComponent();
        }

        private void FrmCreateRollLabels_Load(object sender, EventArgs e)
        {
            txtFactoryCode.Text = AppUtility.GetFactoryCode();
            txtItem.Text = AppUtility.GetDefaultItemCode();
            txtIRMS.Text = AppUtility.GetDefaultIMRS();
            txtPrdYr.Text = (DateTime.Now.Year % 10).ToString();
            txtPrdMo.Text = AppUtility.GetYanJanProdMo(); //1-0 Jan-Oct, N = November, D = December
            txtRnPrdMo.Text = txtPrdMo.Text;
            txtRnPrdYr.Text = txtPrdYr.Text;
            txtRnPrdDate.Text = DateTime.Now.Day.ToString();
            nudMachNo.Value = int.Parse(AppUtility.GetDefaultMacineNo());
            nudSlitPositions.Value = int.Parse(AppUtility.GetDefaultNoOfSlitPositions());
            txtBatchNo.Text = AppUtility.GetLastBatch();
            nudJumboRollNo.Value = Convert.ToDecimal(AppUtility.GetLastRoll());
            txtMtlCode.Text = AppUtility.GetDefaultMaterialCode();
            txtRnShift.Text = AppUtility.GetDefaultShift();
            txtRnDie.Text = AppUtility.GetDefaultDie();
            txtPrdName.Text = AppUtility.GetDefaultProdName();
            txtDefaultItemDesc.Text = AppUtility.GetDefaultItemDesc();
            txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var rollLabels = new List<RollLabelData>();
            for (int i = 1; i <= nudSlitPositions.Value; i++)
            {
                var rollLabel = new RollLabelData
                {
                    ProductionYear = txtPrdYr.Text,
                    ProductionMonth = txtPrdMo.Text,
                    ProductionDate = txtRnPrdDate.Text,
                    AperatureDieNo = txtRnDie.Text,
                    ProductionShift = txtRnShift.Text,
                    JumboRollNo = nudJumboRollNo.Value.ToString("00"),
                    SlitPosition = i.ToString("00"),
                    ItemCode = txtItem.Text,
                    FactoryCode = txtFactoryCode.Text,
                    ProductionMachineNo = nudMachNo.Value.ToString(),
                    MaterialCode = txtMtlCode.Text,
                    ProductName = txtPrdName.Text,
                    BatchNo = txtBatchNo.Text,
                    IRMS = txtIRMS.Text
                };
                rollLabels.Add(rollLabel);
            }
            olvRowLabels.SetObjects(rollLabels);
        }

        private void btnPrintRollLabels_Click(object sender, EventArgs e)
        {
            var labelPrintLoc4x6 = AppUtility.GetPrintLocRollLabel4by6();
            var labelPrintLoc1x6 = AppUtility.GetPrintLocRollLabel1by6();
            var labelPrintExtension = AppUtility.GetLabelPrintExtension();
            var fileNameRollLabels4x6 = Path.Combine(labelPrintLoc4x6, "RollLabels_4by6" + labelPrintExtension);
            var fileNameRollLabels1x6 = Path.Combine(labelPrintLoc1x6, "RollLabels_1by6" + labelPrintExtension);
            var labels = olvRowLabels.Objects as List<RollLabelData>;
            if (labels != null)
            {
                var sb = new StringBuilder(1000);
                sb.Append("Production Year, Production Month, Production date, Aperature die No., Production Shift, Jumbo Roll No.,");
                sb.AppendLine("Slit Position, ItemCode, IRMS#, Factory Code, Production Machine No., Material Code, Product Name, Batch No");
                foreach (var label in labels)
                {
                    sb.AppendFormat("{0},{1},{2},{3},{4}", label.ProductionYear, label.ProductionMonth, label.ProductionDate, label.AperatureDieNo, label.ProductionShift);
                    sb.AppendFormat(",{0},{1},{2},{3},{4}", label.JumboRollNo, label.SlitPosition, label.ItemCode, label.IRMS, label.FactoryCode);
                    sb.AppendFormat(",{0},{1},{2},{3}", label.ProductionMachineNo, label.MaterialCode, label.ProductName, label.BatchNo);
                    sb.AppendLine();
                }
                using (StreamWriter sw = File.CreateText(fileNameRollLabels4x6))
                {
                    sw.Write(sb.ToString());
                }
                using (StreamWriter sw = File.CreateText(fileNameRollLabels1x6))
                {
                    sw.Write(sb.ToString());
                }
                tstbResults.Text = "4x6 and 1 inch roll labels printed. Please check printer.";
                olvRowLabels.Objects = null;
                nudJumboRollNo.Value += 1;
            }

        }

        private void btnPrintPackLabel_Click(object sender, EventArgs e)
        {
            var qty = Decimal.Parse(txtQty.Text).ToString("0.00");
            var qtyNoDecmial = qty.Replace(".","");
            if (PackLabelIsValid())
            {
                var labelPrintLocPack = AppUtility.GetPrintLocPack();
                var labelPrintExtension = AppUtility.GetLabelPrintExtension();
                var fileNamePackLabel = Path.Combine(labelPrintLocPack, "PackLabel" + labelPrintExtension);
                var customerShippingLot = $"{txtFactoryCode.Text}{nudMachNo.Value.ToString()}{txtMtlCode.Text}{txtPrdName.Text}{txtPrdYr.Text}{txtPrdMo.Text}{txtBatchNo.Text}";
                var lotWithPrefix = AppUtility.GetSupplierId() + customerShippingLot;
                var sb = new StringBuilder(1000);
                sb.Append("Supplier Product, Production Date, Item Desc., Order No., Cust. Part No.(IRMS), Qty, QtyNoDecimal, Customer Shipping Lot, LotWithPrefix");
                sb.AppendLine();
                sb.AppendFormat("{0},{1},{2},{3},{4}", txtItem.Text, txtProductionDateFull.Text, txtDefaultItemDesc.Text, txtProdOrder.Text, txtIRMS.Text);
                sb.AppendFormat(",{0},{1},{2},{3}", qty,  qtyNoDecmial, customerShippingLot, lotWithPrefix);
                using (StreamWriter sw = File.CreateText(fileNamePackLabel))
                {
                    sw.Write(sb.ToString());
                }
                tstbResults.Text = "Pack label printed. Please check printer.";
                txtProdOrder.Text = string.Empty;
                txtQty.Text = string.Empty;
            }
           
        }

        private bool PackLabelIsValid()
        {
            var valid = true;
            if (string.IsNullOrEmpty(txtProdOrder.Text))
            {
                errorProviderProdOrder.SetError(txtProdOrder, "Please Provide a value for Productoin Order");
                valid = false;
            }
            else
            {
                errorProviderProdOrder.SetError(txtProdOrder, "");
            }
            decimal qtyDec;

            if (!decimal.TryParse(txtQty.Text,out qtyDec))
            {
                errorProviderQty.SetError(txtQty, "Please specify a numeric value.");
                valid = false;
            }
            else
            {
                errorProviderQty.SetError(txtQty, "");
            }
            return valid;
        }

        private void FrmCreateRollLabels_FormClosing(object sender, FormClosingEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["LastRoll"].Value = nudJumboRollNo.Value.ToString("0");
            config.AppSettings.Settings["LastBatch"].Value = txtBatchNo.Text; 
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}


