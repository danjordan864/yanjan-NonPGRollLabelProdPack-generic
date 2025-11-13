using BrightIdeasSoftware;
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
    /// Partial class representing a scrap rolls dialog form
    /// </summary>
    public partial class FrmScrapRollsDialog : Form
    {
        /// <summary>
        /// Gets or sets the form's Yanjan order number
        /// </summary>
        public string YJNOrderNo { get; set; }

        private List<Roll> _rolls = null;

        /// <summary>
        /// Gets or sets the customer identifier used when retrieving rolls for the order.
        /// Defaults to the Medline customer when not specified.
        /// </summary>
        public string CustomerId { get; set; } = AppUtility.GetMedlineCustomerID();

        /// <summary>
        /// Gets or sets the form's list of selected rolls
        /// </summary>
        public List<Roll> SelectedRolls { get; set; }

        /// <summary>
        /// Initialize a new instance of FrmScrapRollsDialog
        /// </summary>
        public FrmScrapRollsDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the Load event of the FrmScrapRollsDialog form.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void FrmScrapRollsDialog_Load(object sender, EventArgs e)
        {
            // Get the rolls for the YJNOrderNo
            //var so = AppData.GetRollsForOrder(YJNOrderNo, "C1007");
            var so = AppData.GetRollsForOrder(YJNOrderNo, CustomerId);


            // Check if the operation to get the rolls was successful
            if (!so.SuccessFlag)
            {
                // Throw an exception if there was an error getting the rolls
                throw new ApplicationException($"Failed to get rolls for order. Error:{so.ServiceException}");
            }

            // Assign the returned rolls to the _rolls field
            _rolls = so.ReturnValue as List<Roll>;

            // Set the objects of the olvOrderRolls control to the _rolls list
            olvOrderRolls.Objects = _rolls;
        }

        /// <summary>
        /// Event handler for the Click event of the btnScrap button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void btnScrap_Click(object sender, EventArgs e)
        {
            // Check if there are selected rolls that need a scrap reason
            if (_rolls.Where(r => r.Scrap && r.ScrapReason == null).Count() > 0)
            {
                // Display a message box to prompt the user to choose a scrap reason for the selected rolls
                MessageBox.Show("Please choose scrap reason for selected rolls");
            }
            else
            {
                // Set the SelectedRolls property to the rolls that are marked for scrap
                SelectedRolls = _rolls.Where(r => r.Scrap).ToList();

                // Set the DialogResult of the form to OK, indicating a successful operation
                this.DialogResult = DialogResult.OK;
            }
        }


        /// <summary>
        /// Event handler for the CellEditStarting event of the olvOrderRolls objectListView.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The CellEditEventArgs.</param>
        private void olvOrderRolls_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            var roll = (Roll)e.RowObject;

            // Ignore edit events for other columns
            if (e.Column != this.olvColScrapReason || !roll.Scrap) return;

            // Get the scrap reasons
            var so = AppData.GetScrapReasons();
            if (!so.SuccessFlag) throw new ApplicationException($"Failed to get scrap reasons. Error:{so.ServiceException}.");
            var scrapReasons = so.ReturnValue as List<string>;

            // Create a ComboBox control for the cell edit
            ComboBox cb = new ComboBox();
            cb.Bounds = e.CellBounds;
            cb.Font = ((ObjectListView)sender).Font;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.DataSource = scrapReasons;

            // Set the ComboBox control as the editing control for the cell
            e.Control = cb;
        }

        /// <summary>
        /// Event handler for the CellEditFinishing event of the olvOrderRolls objectListView.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The CellEditEventArgs.</param>
        private void olvOrderRolls_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            // Check if the editing control is a ComboBox
            if (e.Control is ComboBox)
            {
                // Check if the column is olvColScrapReason
                if (e.Column == this.olvColScrapReason)
                {
                    // Get the selected value from the ComboBox
                    string value = ((ComboBox)e.Control).SelectedItem.ToString();

                    // Update the ScrapReason property of the corresponding roll object
                    ((Roll)e.RowObject).ScrapReason = value;

                    // Refresh the object in the ObjectListView to reflect the changes
                    this.olvOrderRolls.RefreshObject((Roll)e.RowObject);

                    // Cancel the cell editing
                    e.Cancel = true;
                }
            }
        }


        /// <summary>
        /// Event handler for the ItemCheck event of the olvOrderRolls objectListView.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The ItemCheckEventArgs.</param>
        private void olvOrderRolls_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Get the roll object associated with the item being checked/unchecked
            Roll roll = olvOrderRolls.GetModelObject(e.Index) as Roll;

            // Check if the roll has a non-empty PG_SSCC value
            if (!string.IsNullOrEmpty(roll.PG_SSCC))
            {
                // Display a message indicating that a roll on a bundle cannot be scrapped
                MessageBox.Show("Cannot scrap a roll that is on a bundle.");

                // Set the new value of the item's check state to unchecked
                e.NewValue = CheckState.Unchecked;
            }
        }

    }
}
