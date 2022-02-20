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
    public partial class FrmCasesDialog : Form
    {
        public FrmCasesDialog()
        {
            InitializeComponent();
        }

        public void SetDataSource(List<Case> cases)
        {
            olvCases.SetObjects(cases);
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
