using System;
using System.ComponentModel;
using System.Windows.Forms;
using RollLabelProdPack.Library.Entities;
using System.Text.RegularExpressions;

namespace RollLabelProdPack
{
    /// <summary>
    /// Represents a user control for co-pack production
    /// </summary>
    public partial class CoPackProductionUserControl : UserControl
    {
        // Event raised when validation fails
        public event CoPackProductionValidationHandler ValidationFailed;

        // Event raised when issues refresh is requested
        public event CoPackProductionRefreshIssuesHandler IssuesRefreshRequested;

        // Event raised when lot number is entered
        public event CoPackProductionLotNumberEnteredHandler LotNumberEntered;

        private RollLabelData _order;

        /// <summary>
        /// Gets or sets the order data for co-pack production.
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

                    // Data bindings for various controls
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
                    txtProductionDateFull.Text = DateTime.Now.ToShortDateString();
                    lotNumberTextBox.Text = string.Empty;
                    lotNumberConfirmTextBox.Text = string.Empty;
                    lotNumberTextBox.Enabled = true;
                    lotNumberConfirmTextBox.Enabled = true;
                    txtNumberOfCases.Enabled = true;
                    btnProduce.Enabled = true;
                    lnkPlannedIssues.Enabled = true;
                }
            }
        }

        private int _qty;

        /// <summary>
        /// Gets or sets the quantity for co-pack production.
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
        /// Gets or sets the number of cases for co-pack production.
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
        /// Initializes a new instance of the CoPackProductionUserControl class.
        /// </summary>
        public CoPackProductionUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event for the CoPackProductionUserControl
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The event arguments</param>
        private void CoPackProductionUserControl_Load(object sender, EventArgs e)
        {
            _bindingSource = new BindingSource();
        }


        /// <summary>
        /// Validates the number of cases entered by the user.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="CancelEventArgs"/> that contains the event data.</param>
        private void txtNumberOfCases_Validating(object sender, CancelEventArgs e)
        {
            // Try parsing the text as an integer
            if (int.TryParse(txtNumberOfCases.Text, out _numberOfCases))
            {
                if (_numberOfCases > 0)
                {
                    // Validation is successful
                }
                else
                {
                    // Validation failed: number of cases must be greater than 0
                    RaiseValidationFailure("txtNumberOfCases", "Please enter a number of cases > 0");
                    txtNumberOfCases.Focus();
                    txtNumberOfCases.SelectAll();
                    e.Cancel = true;
                }
            }
            else
            {
                // Validation failed: unable to parse the input as an integer
                RaiseValidationFailure("txtNumberOfCases", "Please enter a number of cases");
                txtNumberOfCases.Focus();
                txtNumberOfCases.SelectAll();
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Raises the event indicating that a validation failure has occurred.
        /// </summary>
        /// <param name="controlName">The name of the control that failed validation.</param>
        /// <param name="validationMessage">The validation error message.</param>
        private void RaiseValidationFailure(string controlName, string validationMessage)
        {
            // Invoke the ValidationFailed event and pass the control name and validation message as event arguments
            ValidationFailed?.Invoke(this, new CoPackProductionValidationEventArgs { ControlName = controlName, ValidationMessage = validationMessage });
        }


        /// <summary>
        /// Raises the event indicating that a refresh of issues is requested.
        /// </summary>
        /// <param name="qtyPerCase">The quantity per case.</param>
        /// <param name="numberOfCases">The number of cases.</param>
        /// <param name="produceRequested">A flag indicating whether a produce request is made.</param>
        private void RaiseIssuesRefreshRequested(int qtyPerCase, int numberOfCases, bool produceRequested)
        {
            // Invoke the IssuesRefreshRequested event and pass the quantity per case, number of cases, and produce request flag as event arguments
            IssuesRefreshRequested?.Invoke(this, new CoPackProductionRefreshIssuesEventArgs { QtyPerCase = qtyPerCase, NumberOfCases = numberOfCases, ProduceRequested = produceRequested });
        }


        /// <summary>
        /// Handles the click event of the Produce button.
        /// Validates the input fields and raises the IssuesRefreshRequested event if the validation is successful.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnProduce_Click(object sender, EventArgs e)
        {
            // Check if the input fields are valid by calling the ValidateChildren method
            if (ValidateChildren())
            {
                // If the validation is successful, raise the IssuesRefreshRequested event and pass the quantity, number of cases, and produce request flag as event arguments
                RaiseIssuesRefreshRequested(Qty, NumberOfCases, true);
            }
        }


        /// <summary>
        /// Handles the LinkClicked event of the Planned Issues link label.
        /// Validates the input fields and raises the IssuesRefreshRequested event if the validation is successful.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void lnkPlannedIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Check if the input fields are valid by calling the ValidateChildren method
            if (ValidateChildren())
            {
                // If the validation is successful, raise the IssuesRefreshRequested event and pass the quantity, number of cases, and produce request flag as event arguments
                RaiseIssuesRefreshRequested(Qty, NumberOfCases, false);
            }
        }


        /// <summary>
        /// Handles the Validating event of the lotNumberTextBox control.
        /// Validates the entered lot number format and raises the ValidationFailed event if the validation fails.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void lotNumberTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (lotNumberTextBox.Text.Length == 0)
            {
                // If the lot number is not entered, raise the ValidationFailed event with the appropriate error message
                RaiseValidationFailure("lotNumberTextBox", "Please enter a lot number");
                lotNumberTextBox.Focus();
                lotNumberTextBox.SelectAll();
                e.Cancel = true;
            }
            else
            {
                // Co-pack lot numbers are 15 characters with the following format:
                // Position 1 (numeric) represents the year of the oldest feeder in pack
                // Positions 2-4 (numeric) represent the Julian date of the oldest feeder in pack
                // Positions 5-8 (uppercase letter or numeric) represent the CM QA plant code
                // Position 9 (numeric) represents the day # of CM production for single feeder lot
                // Position 10 (numeric) represents the production line
                // Position 11 is a space
                // Position 12 (numeric) represents the year of CM production
                // Positions 13-15 (numeric) represent the Julian date of CM production

                Regex r = new Regex(@"\d{4}[\dA-Z]{4}\d{2} \d{4}");
                if (!r.IsMatch(lotNumberTextBox.Text))
                {
                    // If the lot number format is invalid, raise the ValidationFailed event with the appropriate error message
                    RaiseValidationFailure("lotNumberTextBox", "Please enter a valid Co-Pack lot number");
                    lotNumberTextBox.Focus();
                    lotNumberTextBox.SelectAll();
                    e.Cancel = true;
                }
            }
        }


        /// <summary>
        /// Handles the Validating event of the lotNumberConfirmTextBox control.
        /// Validates the entered lot number confirmation and raises the ValidationFailed event if the validation fails.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void lotNumberConfirmTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (lotNumberConfirmTextBox.Text.Length == 0)
            {
                // If the lot number confirmation is not entered, raise the ValidationFailed event with the appropriate error message
                RaiseValidationFailure("lotNumberConfirmTextBox", "Please confirm the lot number");
                lotNumberConfirmTextBox.Focus();
                lotNumberConfirmTextBox.SelectAll();
                e.Cancel = true;
            }
            else if (lotNumberConfirmTextBox.Text != lotNumberTextBox.Text)
            {
                // If the lot number confirmation does not match the lot number, raise the ValidationFailed event with the appropriate error message
                RaiseValidationFailure("lotNumberConfirmTextBox", "Confirmation does not match the lot number");
                lotNumberConfirmTextBox.Focus();
                lotNumberConfirmTextBox.SelectAll();
                e.Cancel = true;
            }
            else
            {
                // Lot number entries are valid
                _order.BatchNo = lotNumberConfirmTextBox.Text;
                LotNumberEntered?.Invoke(this, new CoPackProductionLotNumberEnteredEventArgs { LotNumber = _order.BatchNo });
            }
        }


        /// <summary>
        /// Represents the method that will handle the event when a refresh of production issues is requested in the co-pack production.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The event arguments containing information about the refresh request.</param>
        public delegate void CoPackProductionRefreshIssuesHandler(object source, CoPackProductionRefreshIssuesEventArgs e);

        /// <summary>
        /// Provides event data for the CoPackProductionRefreshIssuesHandler event.
        /// </summary>
        public class CoPackProductionRefreshIssuesEventArgs : EventArgs
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
            /// Gets or sets a value indicating whether a produce request was made.
            /// </summary>
            public bool ProduceRequested { get; set; }
        }


        /// <summary>
        /// Represents the method that will handle the validation failure event in the CoPackProductionUserControl.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="CoPackProductionValidationEventArgs"/> instance containing the event data.</param>
        public delegate void CoPackProductionValidationHandler(object source, CoPackProductionValidationEventArgs e);

        /// <summary>
        /// Provides data for the validation failure event in the CoPackProductionUserControl.
        /// </summary>
        public class CoPackProductionValidationEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or sets the name of the control that failed validation.
            /// </summary>
            public string ControlName { get; set; }

            /// <summary>
            /// Gets or sets the validation message associated with the control.
            /// </summary>
            public string ValidationMessage { get; set; }
        }

        /// <summary>
        /// Represents the method that will handle the event when a lot number is entered in the CoPackProductionUserControl.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The event data.</param>
        public delegate void CoPackProductionLotNumberEnteredHandler(object source, CoPackProductionLotNumberEnteredEventArgs e);

        /// <summary>
        /// Provides data for the event when a lot number is entered in the CoPackProductionUserControl.
        /// </summary>
        public class CoPackProductionLotNumberEnteredEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or sets the entered lot number.
            /// </summary>
            public string LotNumber { get; set; }
        }

    }
}