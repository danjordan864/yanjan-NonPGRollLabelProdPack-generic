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
    public partial class RocklineSelectionsUserControl : UserControl
    {
        public RocklineSelectionsUserControl()
        {
            InitializeComponent();
        }

        private void FilmProductionButton_Click(object sender, EventArgs e)
        {
            var frmMainRockline = new FrmMainRockline();
            frmMainRockline.ShowDialog();
        }

        private void PackPrintButton_Click(object sender, EventArgs e)
        {
            var frmPackPrintRockline = new FrmPackPrintRockline();
            frmPackPrintRockline.ShowDialog();
        }
    }
}
