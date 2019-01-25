using RollLabelProdPack.Library.Data;
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
    public partial class FrmReprintOrderRollsDialog : Form
    {
        public string YJNOrderNo { get; set; }

        private List<Roll> _rolls = null;
        public List<Roll> SelectedRolls { get; set; }
        public FrmReprintOrderRollsDialog()
        {
            InitializeComponent();
        }

        private void FrmReprintOrderRollsDialog_Load(object sender, EventArgs e)
        {
            var so = AppData.GetRollsForOrder(YJNOrderNo);
            if (!so.SuccessFlag) throw new ApplicationException($"Failed to get rolls for order. Error:{so.ServiceException}");
            _rolls = so.ReturnValue as List<Roll>;
            olvOrderRolls.Objects = _rolls;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SelectedRolls = _rolls.Where(r => r.Print).ToList();
            this.DialogResult = DialogResult.OK;
        }
    }
}
