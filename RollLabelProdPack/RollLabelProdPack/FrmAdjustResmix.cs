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

    public partial class FrmAdjustResmix : Form
    {
        FloatingHTML m_htmlToast = new FloatingHTML();
        private RollLabelData _selectOrder = new RollLabelData();
        private BindingSource bindingSource1;
        private List<InventoryDetail> _inputLocMaterial;
        private List<InventoryIssueDetail> _plannedIssue;
        public FrmAdjustResmix()
        {
            InitializeComponent();
        }

        private void FrmAdjustResmix_Load(object sender, EventArgs e)
        {
            bindingSource1 = new BindingSource();
            bindingSource1.DataSource = _selectOrder;
            txtEmployee.DataBindings.Add("Text", bindingSource1, "Employee");
            txtOrderNo.DataBindings.Add("Text", bindingSource1, "SAPOrderNo");
            txtShift.DataBindings.Add("Text", bindingSource1, "Shift");
            txtProductionLine.DataBindings.Add("Text", bindingSource1, "ProductionLine");
            txtItemCode.DataBindings.Add("Text", bindingSource1, "ItemCode");
            txtItemName.DataBindings.Add("Text", bindingSource1, "ItemDescription");
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            ChangeOrder();
        }

        private void ChangeOrder()
        {
            using (SelectOrderDialog frmSignInDialog = new SelectOrderDialog())
            {
                frmSignInDialog.SetDataSource(104); //pass in item group 103 = resmix, 104=formedfilm
                DialogResult dr = frmSignInDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    _selectOrder = frmSignInDialog.SelectOrder;
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                    bindingSource1.DataSource = _selectOrder;
                    var packingMtlLoc = AppUtility.GetPackingMtlLocation();
                    var so = AppData.GetProdLineInputMaterial(_selectOrder.ProductionLine, packingMtlLoc);
                    if (!so.SuccessFlag) throw new ApplicationException($"Error getting Prod. Line Input Material. Error:{so.ServiceException}");
                    _inputLocMaterial = so.ReturnValue as List<InventoryDetail>;
                    cboCurrentBatch.DisplayMember = "Batch";
                    cboCurrentBatch.ValueMember = "Batch";
                    var cboDataSource = _inputLocMaterial.Where(i => !i.PackagingMtl).ToList();
                    cboDataSource.Insert(0,new InventoryDetail { Batch = "Please Select Batch" });
                    cboCurrentBatch.DataSource = cboDataSource;
                    cboCurrentBatch.Enabled = true;
                }
            }
        }


        private void CheckReadyToProduce()
        {
            if (cboCurrentBatch.Text == "Please Select Batch")
            {
                btnAdjust.Enabled = false;
                DisplayToastNotification(WinFormUtils.ToastNotificationType.Warning, "Select Batch", "Please Select current batch.");
                return;
            }
            else
            {
                var so = AppData.GetProdOrderIssueMaterial(_selectOrder.SAPOrderNo, 1);

                var prodOrderLines = so.ReturnValue as List<InventoryIssueDetail>;
                if (!so.SuccessFlag) throw new ApplicationException($"Error getting Issue Material. Error:{so.ServiceException}");
                //should only be one batch controlled item in input locaion
                var prodOrderLineBatchControlled = prodOrderLines.Where(p => p.BatchControlled).ToList();
                if (prodOrderLineBatchControlled.Count() > 1) throw new ApplicationException("There is more than on batch controlled item on the production order.");
                var prodOrderLine = prodOrderLineBatchControlled.FirstOrDefault();
                var inputLocMatlMathingOrder = _inputLocMaterial.Where(i => i.ItemCode == prodOrderLine.ItemCode);
                var noOfBatches = inputLocMatlMathingOrder.Count();
                if (noOfBatches <= 3) //two batches in machine and one batch feeding
                {
                    DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "No Adjustment needed", $"There are {noOfBatches} batches in input location, no adjustment needed.");
                }
                else
                {
                    _plannedIssue = new List<InventoryIssueDetail>();
                    var batchInUseCount = 0;
                    foreach (var batch in inputLocMatlMathingOrder.Reverse())
                    {
                        
                        if (Convert.ToInt32(batch.Batch.Replace(" - ",string.Empty)) <= Convert.ToInt32(cboCurrentBatch.Text.Replace(" - ",string.Empty)) && batchInUseCount <3)
                        {
                            batchInUseCount += 1;
                        }
                        else
                        {
                            _plannedIssue.Add(AppUtility.CreatePlannedIssueDetail(batch.ItemCode, batch.UOM, prodOrderLine.BaseEntry, prodOrderLine.BaseLine, batch.StorageLocation, batch.QualityStatus,
                                      batch.LUID, batch.SSCC, batch.Quantity, batch.Quantity, batch.Batch, 0, batch.BatchControlled, batch.PackagingMtl));
                        }
                    }
                    btnAdjust.Enabled = true;
                    lnkPlannedIssues.Enabled = true;
                }
                
            }
    }

    private void Adjust()
    {
        var userNamePW = AppUtility.GetUserNameAndPasswordFilm(_selectOrder.ProductionMachineNo);

        using (SAPB1 sapB1 = new SAPB1(userNamePW.Key, userNamePW.Value))
        {
            using (InventoryIssue invIssue = (InventoryIssue)sapB1.B1Factory(SAPbobsCOM.BoObjectTypes.oInventoryGenExit, 0))
            {
                foreach (var plIssue in _plannedIssue.Where(i => i.BatchControlled)) // don't issue packaging material for box scrap
                {
                    invIssue.AddLine(plIssue.BaseEntry, plIssue.BaseLine, plIssue.ItemCode, plIssue.PlannedIssueQty, plIssue.StorageLocation, plIssue.QualityStatus, plIssue.Batch, plIssue.LUID, plIssue.SSCC, plIssue.UOM, _selectOrder.YJNOrder);
                }
                if (_plannedIssue.Where(i => i.BatchControlled).Sum(q => q.PlannedIssueQty) > 0 && invIssue.Save() == false) { throw new B1Exception(sapB1.SapCompany, sapB1.GetLastExceptionMessage()); }
            }
        }
        DisplayToastNotification(WinFormUtils.ToastNotificationType.Success, "Resin Issued", "Resin issued to order: {txtOrderNo.Text}");
    }

    private void lnkPlannedIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        using (FrmPlannedIssueDialog frmPlannedIssues = new FrmPlannedIssueDialog())
        {
            frmPlannedIssues.SetDataSource(_plannedIssue.Where(i => i.BatchControlled).ToList());
            DialogResult dr = frmPlannedIssues.ShowDialog();
        }
    }

    private void btnAdjust_Click(object sender, EventArgs e)
    {
        Adjust();
        lnkPlannedIssues.Enabled = false;
        btnAdjust.Enabled = false;
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

        private void cboCurrentBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckReadyToProduce();
        }
    }
}
