using System;
using System.ComponentModel;
using System.Windows.Forms;
using RollLabelProdPack.Library.Entities;

namespace RollLabelProdPack
{
    /// <summary>
    /// UserControl for tub production.
    /// </summary>
    public partial class TubProductionUserControl : UserControl
    {
        // Events
        public event TubProductionValidationHandler ValidationFailed;
        public event TubProductionRefreshIssuesHandler IssuesRefreshRequested;

        // Properties
        private RollLabelData _order;

        /// <summary>
        /// Gets or sets the RollLabelData object representing the order.
        /// </summary>
        public RollLabelData Order
        {
            get { return _order; }
            set
            {
                _order = value;
                if (!DesignMode && value != null)
                {
                    _bindingSource.DataSource = _order;
                    // Bind TextBox controls to corresponding properties of the RollLabelData object
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

                    // Set additional properties and enable controls
                    txtBatch.Text = _order.BatchNo;
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                    txtQty.Enabled = true;
                    txtNumberOfCases.Enabled = true;
                    btnProduce.Enabled = true;
                    lnkPlannedIssues.Enabled = true;
                }
            }
        }


        private int _qty;

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the number of cases.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the TubProductionUserControl class.
        /// </summary>
        public TubProductionUserControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Handles the load event of the TubProductionUserControl.
        /// Initializes the binding source.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void TubProductionUserControl_Load(object sender, EventArgs e)
        {
            // Initialize the binding source
            _bindingSource = new BindingSource();
        }

        /// <summary>
        /// Validates the txtQty TextBox.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
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

        /// <summary>
        /// Validates the txtNumberOfCases TextBox.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
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


        /// <summary>
        /// Raises the ValidationFailed event to notify subscribers about a validation failure.
        /// The event provides information about the control that failed validation 
        /// and the validation failure message.
        /// </summary>
        /// <param name="controlName">The name of the control that failed validation.</param>
        /// <param name="validationMessage">The validation failure message.</param>
        private void RaiseValidationFailure(string controlName, string validationMessage)
        {
            ValidationFailed?.Invoke(this, new TubProductionValidationEventArgs { ControlName = controlName, ValidationMessage = validationMessage });
        }


        /// <summary>
        /// Raises the IssuesRefreshRequested event.
        /// This method is called when a refresh of issues is requested.
        /// It takes the quantity per case, number of cases, and a flag indicating if the produce action was requested as parameters.
        /// It invokes the IssuesRefreshRequested event, passing a new instance of TubProductionRefreshIssuesEventArgs as the event argument.
        /// The event subscribers will handle the event and perform the necessary actions based on the provided arguments.
        /// </summary>
        /// <param name="qtyPerCase">The quantity per case.</param>
        /// <param name="numberOfCases">The number of cases.</param>
        /// <param name="produceRequested">A flag indicating if the produce action was requested.</param>
        private void RaiseIssuesRefreshRequested(int qtyPerCase, int numberOfCases, bool produceRequested)
        {
            IssuesRefreshRequested?.Invoke(this, new TubProductionRefreshIssuesEventArgs { QtyPerCase = qtyPerCase, NumberOfCases = numberOfCases, ProduceRequested = produceRequested });
        }


        /// <summary>
        /// Handles the click event of the btnProduce button.
        /// Raises the IssuesRefreshRequested event if validation is successful.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void btnProduce_Click(object sender, EventArgs e)
        {
            // Check if all child controls pass validation
            if (ValidateChildren())
            {
                // Raise the IssuesRefreshRequested event
                RaiseIssuesRefreshRequested(Qty, NumberOfCases, true);
            }
        }

        /// <summary>
        /// Handles the LinkClicked event of the lnkPlannedIssues LinkLabel.
        /// Raises the IssuesRefreshRequested event when the lnkPlannedIssues LinkLabel is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void lnkPlannedIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (ValidateChildren())
            {
                RaiseIssuesRefreshRequested(Qty, NumberOfCases, false);
            }
        }
    }

    /// <summary>
    /// Represents the handler for the TubProductionRefreshIssues event.
    /// </summary>
    /// <param name="source">The event source.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void TubProductionRefreshIssuesHandler(object source, TubProductionRefreshIssuesEventArgs e);

    /// <summary>
    /// Provides data for the TubProductionRefreshIssues event.
    /// </summary>
    public class TubProductionRefreshIssuesEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the quantity per case.
        /// </summary>
        public int QtyPerCase { get; set; }

        /// <summary>
        /// Gets or sets the number of cases.
        /// </summary>
        public int NumberOfCases { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the produce action was requested.
        /// </summary>
        public bool ProduceRequested { get; set; }
    }

    /// <summary>
    /// Represents the method that handles the validation failure event in the TubProductionUserControl.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="e">An instance of TubProductionValidationEventArgs containing event data.</param>
    public delegate void TubProductionValidationHandler(object source, TubProductionValidationEventArgs e);

    /// <summary>
    /// Represents the event arguments for the TubProductionValidationHandler event.
    /// </summary>
    public class TubProductionValidationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the name of the control that failed validation.
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// Gets or sets the validation failure message.
        /// </summary>
        public string ValidationMessage { get; set; }
    }


}
