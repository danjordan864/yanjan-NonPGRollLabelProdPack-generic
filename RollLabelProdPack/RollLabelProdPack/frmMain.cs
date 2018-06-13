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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnCreateRollLabels_Click(object sender, EventArgs e)
        {
            var frmCreateRollLabels = new FrmCreateRollLabels();
            frmCreateRollLabels.Show();
        }

        private void btnPackAndProduce_Click(object sender, EventArgs e)
        {
            var frmPackAndProduce = new FrmProdPack();
            frmPackAndProduce.Show();
        }

       
    }
}
