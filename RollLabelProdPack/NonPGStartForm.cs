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
    public partial class NonPGStartForm : Form
    {
        private UserControl[] customerSelectionUserControls = { new NoCustomerSelectedUserControl(), new MedlineSelectionsUserControl() };

        public NonPGStartForm()
        {
            InitializeComponent();
        }

        private void NonPGStartForm_Load(object sender, EventArgs e)
        {
            CustomerComboBox.SelectedIndex = 0;
        }

        private void CustomerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(customerSelectionUserControls[CustomerComboBox.SelectedIndex]);
        }
    }
}
