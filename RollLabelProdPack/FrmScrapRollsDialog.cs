using BrightIdeasSoftware;
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
    public partial class FrmScrapRollsDialog : Form
    {
        public string YJNOrderNo { get; set; }

        private List<Roll> _rolls = null;
        public List<Roll> SelectedRolls { get; set; }
        public FrmScrapRollsDialog()
        {
            InitializeComponent();
        }

        private void FrmScrapRollsDialog_Load(object sender, EventArgs e)
        {
            var so = AppData.GetRollsForOrder(YJNOrderNo);
            if (!so.SuccessFlag) throw new ApplicationException($"Failed to get rolls for order. Error:{so.ServiceException}");
            _rolls = so.ReturnValue as List<Roll>;
            olvOrderRolls.Objects = _rolls;
        }

        private void btnScrap_Click(object sender, EventArgs e)
        {
            if (_rolls.Where(r => r.Scrap && r.ScrapReason == null).Count() > 0)
            {
                MessageBox.Show("Please choose scrap reason for selected rolls");
            }
            else
            {
                SelectedRolls = _rolls.Where(r => r.Scrap).ToList();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void olvOrderRolls_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            var roll = (Roll)e.RowObject;
            //Ignore edit events for other columns
            if (e.Column != this.olvColScrapReason || !roll.Scrap) return;
            var so = AppData.GetScrapReasons();
            if (!so.SuccessFlag) throw new ApplicationException($"Failed to get scrap reasons. Error:{so.ServiceException}.");
            var scrapReasons = so.ReturnValue as List<string>;
            ComboBox cb = new ComboBox();
            cb.Bounds = e.CellBounds;
            cb.Font = ((ObjectListView)sender).Font;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.DataSource = scrapReasons;
            e.Control = cb;


        }
        private void olvOrderRolls_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                if (e.Column == this.olvColScrapReason)
                {
                    string value = ((ComboBox)e.Control).SelectedItem.ToString();
                    ((Roll)e.RowObject).ScrapReason = value;
                    this.olvOrderRolls.RefreshObject((Roll)e.RowObject);
                    e.Cancel = true;
                }
            }
        }

        private void olvOrderRolls_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Roll roll = olvOrderRolls.GetModelObject(e.Index) as Roll;
            if(!string.IsNullOrEmpty(roll.PG_SSCC))
            {
                MessageBox.Show("Can not scrap a roll that is on a bundle.");
                e.NewValue = CheckState.Unchecked;
            }
        }
    }
}
