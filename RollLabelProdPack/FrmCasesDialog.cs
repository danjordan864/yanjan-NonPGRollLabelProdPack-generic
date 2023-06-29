using RollLabelProdPack.Library.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RollLabelProdPack
{
    /// <summary>
    /// Partial class representing a dialog that displays cases
    /// </summary>
    public partial class FrmCasesDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the FrmCasesDialog class.
        /// </summary>
        public FrmCasesDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the data source of the olvCases ObjectListView to the provided
        /// list of cases.
        /// </summary>
        /// <param name="cases">List of cases to use as the data source</param>
        public void SetDataSource(List<Case> cases)
        {
            olvCases.SetObjects(cases);
        }

        /// <summary>
        /// Displays the provided validation message in the txtValidationMessage TextBox
        /// </summary>
        /// <param name="validationMessage">Message to be shown in the txtValidateMessage TextBox</param>
        public void SetValidationMessage(string validationMessage)
        {
            txtValidationMessage.Text = validationMessage;
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

    }
}
