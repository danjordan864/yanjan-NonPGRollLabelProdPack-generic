using RollLabelProdPack.Library.Data;
using RollLabelProdPack.Library.Entities;
using RollLabelProdPack.Library.Utility;
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
    /// Partial class representing a Reprint Order Rolls dialog
    /// </summary>
    public partial class FrmReprintOrderRollsDialogMedline : Form
    {
        /// <summary>
        /// Gets or sets the form's current Yanjan Order Number
        /// </summary>
        public string YJNOrderNo { get; set; }

        private List<Roll> _rolls = null;

        /// <summary>
        /// Gets or sets the form's list of selected rolls
        /// </summary>
        public List<Roll> SelectedRolls { get; set; }

        /// <summary>
        /// Initializes a new FrmReprintOrderRollsDialog instance
        /// </summary>
        public FrmReprintOrderRollsDialogMedline()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Event handler for the Load event of the FrmReprintOrderRollsDialog form.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void FrmReprintOrderRollsDialog_Load(object sender, EventArgs e)
        {
            // Retrieve rolls for the order
            //var so = AppData.GetRollsForOrder(YJNOrderNo, "C1007");
            var so = AppData.GetRollsForOrder(YJNOrderNo, AppUtility.GetMedlineCustomerID());
            if (!so.SuccessFlag)
                throw new ApplicationException($"Failed to get rolls for order. Error:{so.ServiceException}");

            _rolls = so.ReturnValue as List<Roll>;

            // Set the objects of the objectListView control to the retrieved rolls
            olvOrderRolls.Objects = _rolls;
        }

        /// <summary>
        /// Event handler for the Click event of the btnPrint button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Get the selected rolls that should be printed
            SelectedRolls = _rolls.Where(r => r.Print).ToList();

            // Set the DialogResult of the form to OK to indicate that the printing operation is confirmed
            this.DialogResult = DialogResult.OK;
        }

    }
}
