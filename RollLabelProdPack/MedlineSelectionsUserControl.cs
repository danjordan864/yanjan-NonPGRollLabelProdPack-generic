using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RollLabelProdPack
{
    public partial class MedlineSelectionsUserControl : UserControl
    {
        public MedlineSelectionsUserControl()
        {
            InitializeComponent();
        }

        private void FilmProductionButton_Click(object sender, EventArgs e)
        {
            var frmMainMedline = new FrmMainMedline();
            frmMainMedline.ShowDialog();
        }

        private void PackPrintButton_Click(object sender, EventArgs e)
        {
            var frmPackPrintMedline = new FrmPackPrintMedline();
            frmPackPrintMedline.ShowDialog();
        }
    }
}
