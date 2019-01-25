using BrightIdeasSoftware;
using RollLabelProdPack.Library.Entities;
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
    public partial class FrmPlannedIssueDialog : Form
    {
        public FrmPlannedIssueDialog()
        {
            InitializeComponent();
        }

        public void SetDataSource(List<InventoryIssueDetail> plannedIssues)
        {
            olvPlannedIssues.SetObjects(plannedIssues);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void olvPlannedIssues_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            InventoryIssueDetail i = (InventoryIssueDetail)e.Model;
            if (i.ShortQty >0)
            {
                e.Item.BackColor = Color.MistyRose;
            }
        }
    }
}
