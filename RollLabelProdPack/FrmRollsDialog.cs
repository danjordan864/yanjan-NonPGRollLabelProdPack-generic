using RollLabelProdPack.Library.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RollLabelProdPack
{

    /// <summary>
    /// Partial class representing a rolls dialog
    /// </summary>
    public partial class FrmRollsDialog : Form
    {

        /// <summary>
        /// Initializes a new FrmRollsDialog instance.
        /// </summary>
        public FrmRollsDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the data source of the olvRolls ObjectListView to the provided list of Rolls.
        /// </summary>
        /// <param name="rolls">List of rows representing the new data source for the olvRolls ObjectListView</param>
        public void SetDataSource(List<Roll> rolls)
        {
            olvRolls.SetObjects(rolls);
        }

        /// <summary>
        /// Set the contents of the txtValidationMessage TextBox to the supplied validation message
        /// </summary>
        /// <param name="validationMessage">Supplied validation message</param>
        public void SetValidationMessage(string validationMessage)
        {
            // Set the text of the txtValidationMesage TextBox to the supplied validation message.
            txtValidationMessage.Text = validationMessage;
        }

        /// <summary>
        /// Handle the OK button's click event. Set the DialogResult to OK.
        /// </summary>
        /// <param name="sender">Sending argument</param>
        /// <param name="e">Event arguments</param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

    }
}
