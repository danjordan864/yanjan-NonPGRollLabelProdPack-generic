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
    /// <summary>
    /// Partial class representing a planned issues dialog form
    /// </summary>
    public partial class FrmPlannedIssueDialog : Form
    {
        /// <summary>
        /// Initialize a new instance of the FrmPlannedIssueDialog class
        /// </summary>
        public FrmPlannedIssueDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the data source of the olvPlannedIssues ObjectListView to the provided list
        /// of planned issues
        /// </summary>
        /// <param name="plannedIssues">List of planned issues</param>
        public void SetDataSource(List<InventoryIssueDetail> plannedIssues)
        {
            olvPlannedIssues.SetObjects(plannedIssues);
        }

        /// <summary>
        /// Handles the click of the OK button. Sets the DialogResult to OK.
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event arguments</param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Format the specified row of the olvPlannedIssues ObjectListView.
        /// If the associated InventoryIssueDetail object has a shortage, set the background
        /// Color to MistyRose.
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event arguments</param>
        private void olvPlannedIssues_FormatRow(object sender, FormatRowEventArgs e)
        {
            InventoryIssueDetail i = (InventoryIssueDetail)e.Model;
            if (i.ShortQty >0)
            {
                e.Item.BackColor = Color.MistyRose;
            }
        }
    }
}
