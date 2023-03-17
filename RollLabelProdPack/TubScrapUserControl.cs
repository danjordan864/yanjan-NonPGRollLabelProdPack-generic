using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RollLabelProdPack.Library.Entities;

namespace RollLabelProdPack
{
    public partial class TubScrapUserControl : UserControl
    {
        public event TubScrapHandler ScrapRequested;
        public event TubScrapValidationHandler ValidationFailed;

        private List<string> _scrapReasons;
        public List<string> ScrapReasons
        {
            get
            {
                return _scrapReasons;
            }
            set
            {
                _scrapReasons = value;
                if (!DesignMode && _scrapReasons != null)
                {
                    if (!_scrapReasons.Any(t => t == "<Select>"))
                    {
                        _scrapReasons.Insert(0, "<Select>");
                    }
                    scrapReasonsComboBox.DataSource = _scrapReasons;
                }
            }
        }

        private RollLabelData _order;
        public RollLabelData Order
        {
            get { return _order; }
            set
            {
                // RDJ 20230315 Not using scrap right now. Ignore exceptions.
                try
                {
                    _order = value;
                    if (!DesignMode && value != null)
                    {
                        _bindingSource.DataSource = _order;
                        if (!string.IsNullOrEmpty(_order.ScrapItem) && _order.ScrapItem != "N/A")
                        {
                            scrapItemLabel.DataBindings.Clear();
                            scrapItemLabel.DataBindings.Add("Text", _bindingSource, "ScrapItem");
                            scrapReasonsComboBox.Enabled = true;
                            scrapQtyTextBox.Enabled = true;
                            scrapButton.Enabled = true;
                        }
                    }

                }
                catch (Exception)
                {
                }
            }
        }

        private BindingSource _bindingSource;

        public TubScrapUserControl()
        {
            InitializeComponent();
        }

        private void TubScrapUserControl_Load(object sender, EventArgs e)
        {
            _bindingSource = new BindingSource();
        }

        private void scrapButton_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                int scrapQty = int.Parse(scrapQtyTextBox.Text);
                ScrapRequested?.Invoke(this, new TubScrapEventArgs { ScrapQty = scrapQty, ScrapReason = (string)scrapReasonsComboBox.SelectedItem });
            }
        }

        private void scrapQtyTextBox_Validating(object sender, CancelEventArgs e)
        {
            int scrapQty = -1;
            if (!string.IsNullOrEmpty(scrapQtyTextBox.Text))
            {
                if (int.TryParse(scrapQtyTextBox.Text, out scrapQty))
                {
                    if (scrapQty > 0)
                    {
                        // validation successful
                    }
                    else
                    {
                        ValidationFailed?.Invoke(this, new TubScrapValidationEventArgs { ValidationMessage = "Please enter a scrap qty > 0" });
                        scrapQtyTextBox.Focus();
                        scrapQtyTextBox.SelectAll();
                        e.Cancel = true;
                    }
                }
                else
                {
                    ValidationFailed?.Invoke(this, new TubScrapValidationEventArgs { ValidationMessage = "Please enter a numeric scrap qty" });
                    scrapQtyTextBox.Focus();
                    scrapQtyTextBox.SelectAll();
                    e.Cancel = true;
                }
            }
            else
            {
                ValidationFailed?.Invoke(this, new TubScrapValidationEventArgs { ValidationMessage = "Please enter a scrap qty" });
                scrapQtyTextBox.Focus();
                e.Cancel = true;
            }
        }

        private void scrapReasonsComboBox_Validating(object sender, CancelEventArgs e)
        {
            string scrapReason = (string)scrapReasonsComboBox.SelectedItem;
            if (!string.IsNullOrEmpty(scrapReason))
            {
                if (scrapReason != "<Select>")
                {
                    // validation successful
                }
                else
                {
                    ValidationFailed?.Invoke(this, new TubScrapValidationEventArgs { ValidationMessage = "Please select a scrap reason" });
                    scrapReasonsComboBox.Focus();
                    e.Cancel = true;
                }
            }
            else
            {
                ValidationFailed?.Invoke(this, new TubScrapValidationEventArgs { ValidationMessage = "Please select a scrap reason" });
                scrapReasonsComboBox.Focus();
                e.Cancel = true;
            }
        }
    }

    public delegate void TubScrapHandler(object source, TubScrapEventArgs e);

    public class TubScrapEventArgs : EventArgs
    {
        public int ScrapQty { get; set; }
        public string ScrapReason { get; set; }
    }

    public delegate void TubScrapValidationHandler(object source, TubScrapValidationEventArgs e);

    public class TubScrapValidationEventArgs : EventArgs
    {
        public string ValidationMessage { get; set; }
    }
}
