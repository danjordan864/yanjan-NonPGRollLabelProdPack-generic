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
    public partial class FrmRollsDialog : Form
    {
        public FrmRollsDialog()
        {
            InitializeComponent();
        }

        public void SetDataSource(List<Roll> rolls)
        {
            olvRolls.SetObjects(rolls);
        }

        public void SetValidationMessage(string validationMessage)
        {
            txtValidationMessage.Text = validationMessage;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

    }
}
