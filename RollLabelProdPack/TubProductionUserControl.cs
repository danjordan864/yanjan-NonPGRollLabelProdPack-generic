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
    public partial class TubProductionUserControl : UserControl
    {
        public event TubProductionValidationHandler ValidationFailed;
        public event TubProductionRefreshIssuesHandler IssuesRefreshRequested;

        private RollLabelData _order;
        public RollLabelData Order { get { return _order; }
        set
            {
                _order = value;
                if (!DesignMode && value != null)
                {
                    _bindingSource.DataSource = _order;
                    txtEmployee.DataBindings.Clear();
                    txtEmployee.DataBindings.Add("Text", _bindingSource, "Employee");
                    txtOrderNo.DataBindings.Clear();
                    txtOrderNo.DataBindings.Add("Text", _bindingSource, "SAPOrderNo");
                    txtShift.DataBindings.Clear();
                    txtShift.DataBindings.Add("Text", _bindingSource, "Shift");
                    txtProductionLine.DataBindings.Clear();
                    txtProductionLine.DataBindings.Add("Text", _bindingSource, "ProductionLine");
                    txtItemCode.DataBindings.Clear();
                    txtItemCode.DataBindings.Add("Text", _bindingSource, "ItemCode");
                    txtItemName.DataBindings.Clear();
                    txtItemName.DataBindings.Add("Text", _bindingSource, "ItemDescription");
                    casesProducedLabel.DataBindings.Clear();
                    casesProducedLabel.DataBindings.Add("Text", _bindingSource, "InvRolls");
                    maxCasesLabel.DataBindings.Clear();
                    maxCasesLabel.DataBindings.Add("Text", _bindingSource, "TargetRolls");
                    txtBatch.Text = _order.BatchNo; // $"{_order.SAPOrderNo.ToString()}{_order.ProductionLine.Replace("TUB", "T")}";
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                    txtQty.Enabled = true;
                    txtNumberOfCases.Enabled = true;
                    btnProduce.Enabled = true;
                    lnkPlannedIssues.Enabled = true;
                }
            }
        }

        private int _qty;
        public int Qty
        {
            get
            {
                return _qty;
            }
            set
            {
                _qty = value;
            }
        }

        private int _numberOfCases;
        public int NumberOfCases
        {
            get
            {
                return _numberOfCases;
            }
            set
            {
                _numberOfCases = value;
            }
        }

        private BindingSource _bindingSource;

        public TubProductionUserControl()
        {
            InitializeComponent();
        }

        private void TubProductionUserControl_Load(object sender, EventArgs e)
        {
            _bindingSource = new BindingSource();
        }

        private void txtQty_Validating(object sender, CancelEventArgs e)
        {
            if (int.TryParse(txtQty.Text, out _qty))
            {
                if (_qty > 0)
                {
                    // Validation is successful
                }
                else
                {
                    RaiseValidationFailure("txtQty", "Please enter a quantity > 0");
                    txtQty.Focus();
                    txtQty.SelectAll();
                    e.Cancel = true;
                }
            }
            else
            {
                RaiseValidationFailure("txtQty", "Please enter a quantity");
                txtQty.Focus();
                txtQty.SelectAll();
                e.Cancel = true;
            }
        }

        private void txtNumberOfCases_Validating(object sender, CancelEventArgs e)
        {
            if (int.TryParse(txtNumberOfCases.Text, out _numberOfCases))
            {
                if (_numberOfCases > 0)
                {
                    // Validation is successful
                }
                else
                {
                    RaiseValidationFailure("txtNumberOfCases", "Please enter a number of cases > 0");
                    txtNumberOfCases.Focus();
                    txtNumberOfCases.SelectAll();
                    e.Cancel = true;
                }
            }
            else
            {
                RaiseValidationFailure("txtNumberOfCases", "Please enter a number of cases");
                txtNumberOfCases.Focus();
                txtNumberOfCases.SelectAll();
                e.Cancel = true;
            }
        }

        private void RaiseValidationFailure(string controlName, string validationMessage)
        {
            ValidationFailed?.Invoke(this, new TubProductionValidationEventArgs { ControlName = controlName, ValidationMessage = validationMessage });
        }

        private void RaiseIssuesRefreshRequested(int qtyPerCase, int numberOfCases, bool produceRequested)
        {
            IssuesRefreshRequested?.Invoke(this, new TubProductionRefreshIssuesEventArgs { NumberOfCases = numberOfCases, QtyPerCase = qtyPerCase, ProduceRequested = produceRequested });
        }

        private void btnProduce_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                RaiseIssuesRefreshRequested(Qty, NumberOfCases, true);
            }
        }

        private void lnkPlannedIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (ValidateChildren())
            {
                RaiseIssuesRefreshRequested(Qty, NumberOfCases, false);
            }
        }
    }

    public delegate void TubProductionRefreshIssuesHandler(object source, TubProductionRefreshIssuesEventArgs e);

    public class TubProductionRefreshIssuesEventArgs: EventArgs
    {
        public int QtyPerCase { get; set; }
        public int NumberOfCases { get; set; }
        public bool ProduceRequested { get; set; }
    }

    public delegate void TubProductionValidationHandler(object source, TubProductionValidationEventArgs e);

    public class TubProductionValidationEventArgs : EventArgs
    {
        public string ControlName { get; set; }
        public string ValidationMessage { get; set; }
    }

}
