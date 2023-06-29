using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using RollLabelProdPack.Library.Entities;

namespace RollLabelProdPack
{
    /// <summary>
    /// Represents a user control for the tub scrap operation.
    /// </summary>
    public partial class TubScrapUserControl : UserControl
    {
        /// <summary>
        /// Event raised when a tub scrap is requested.
        /// </summary>
        public event TubScrapHandler ScrapRequested;

        /// <summary>
        /// Event raised when the validation for the tub scrap operation fails.
        /// </summary>
        public event TubScrapValidationHandler ValidationFailed;

        private List<string> _scrapReasons;

        /// <summary>
        /// Gets or sets the list of scrap reasons.
        /// </summary>
        public List<string> ScrapReasons
        {
            get
            {
                return _scrapReasons;
            }
            set
            {
                _scrapReasons = value;

                // Bind the scrap reasons to the combo box if not in design mode
                if (!DesignMode && _scrapReasons != null)
                {
                    // Add a "<Select>" option if not already present
                    if (!_scrapReasons.Any(t => t == "<Select>"))
                    {
                        _scrapReasons.Insert(0, "<Select>");
                    }
                    scrapReasonsComboBox.DataSource = _scrapReasons;
                }
            }
        }

        private RollLabelData _order;

        /// <summary>
        /// Gets or sets the roll label data for the tub scrap operation.
        /// </summary>
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
                    // Exception occurred while setting the order data
                    // Ignore the exception for now
                }
            }
        }


        private BindingSource _bindingSource;

        /// <summary>
        /// Initializes a new instance of the TubScrapUserControl class.
        /// </summary>
        public TubScrapUserControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Handles the Load event of the TubScrapUserControl.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void TubScrapUserControl_Load(object sender, EventArgs e)
        {
            _bindingSource = new BindingSource();
        }


        /// <summary>
        /// Event handler for the Scrap button click event.
        /// Triggers the ScrapRequested event and passes the scrap quantity and reason as event arguments.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An instance of the <see cref="TubScrapEventArgs"/> class containing the scrap quantity and reason.</param>
        private void scrapButton_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                int scrapQty = int.Parse(scrapQtyTextBox.Text);
                ScrapRequested?.Invoke(this, new TubScrapEventArgs { ScrapQty = scrapQty, ScrapReason = (string)scrapReasonsComboBox.SelectedItem });
            }
        }


        /// <summary>
        /// Handles the validating event of the scrapQtyTextBox control.
        /// Validates the entered scrap quantity and raises the ValidationFailed event if validation fails.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A CancelEventArgs that contains the event data.</param>
        private void scrapQtyTextBox_Validating(object sender, CancelEventArgs e)
        {
            int scrapQty = -1;
            if (!string.IsNullOrEmpty(scrapQtyTextBox.Text))
            {
                if (int.TryParse(scrapQtyTextBox.Text, out scrapQty))
                {
                    if (scrapQty > 0)
                    {
                        // Validation is successful
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


        /// <summary>
        /// Validates the selected scrap reason from the scrapReasonsComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A CancelEventArgs that contains the event data.</param>
        /// <remarks>
        /// This method is called when the scrapReasonsComboBox control is being validated.
        /// It checks if a scrap reason is selected from the combo box. If a scrap reason is selected,
        /// the validation is considered successful. Otherwise, a validation failure event is raised
        /// with a specific validation message indicating that a scrap reason should be selected.
        /// </remarks>
        private void scrapReasonsComboBox_Validating(object sender, CancelEventArgs e)
        {
            string scrapReason = (string)scrapReasonsComboBox.SelectedItem;
            if (!string.IsNullOrEmpty(scrapReason))
            {
                if (scrapReason != "<Select>")
                {
                    // Validation is successful
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

    /// <summary>
    /// Represents the method that will handle the TubScrap event.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="e">A TubScrapEventArgs that contains the event data.</param>
    /// <remarks>
    /// The TubScrapHandler delegate defines the signature for methods that handle the TubScrap event.
    /// It is used to notify subscribers when a scrap operation is requested. The event handler
    /// should accept two parameters: the source of the event (typically the instance raising the event)
    /// and a TubScrapEventArgs object that contains the scrap quantity and scrap reason information.
    /// </remarks>
    public delegate void TubScrapHandler(object source, TubScrapEventArgs e);

    /// <summary>
    /// Provides data for the TubScrap event.
    /// </summary>
    /// <remarks>
    /// The TubScrapEventArgs class contains information about a scrap operation, including
    /// the scrap quantity and the scrap reason. It is used as the event data when raising
    /// the TubScrap event, allowing subscribers to access the relevant information about
    /// the scrap operation.
    /// </remarks>
    public class TubScrapEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the quantity of scrap.
        /// </summary>
        public int ScrapQty { get; set; }

        /// <summary>
        /// Gets or sets the reason for the scrap.
        /// </summary>
        public string ScrapReason { get; set; }
    }


    /// <summary>
    /// Represents the method that handles the TubScrapValidation event.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="e">A TubScrapValidationEventArgs that contains the event data.</param>
    /// <remarks>
    /// The TubScrapValidationHandler delegate defines the signature for methods that handle
    /// the TubScrapValidation event. It takes two parameters: the source of the event, and
    /// a TubScrapValidationEventArgs object that contains the event data, including the
    /// validation message.
    /// </remarks>
    public delegate void TubScrapValidationHandler(object source, TubScrapValidationEventArgs e);

    /// <summary>
    /// Provides data for the TubScrapValidation event.
    /// </summary>
    /// <remarks>
    /// The TubScrapValidationEventArgs class contains information about the validation failure
    /// during the TubScrapUserControl validation process. It includes the validation message
    /// describing the reason for the validation failure.
    /// </remarks>
    public class TubScrapValidationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>The validation message describing the reason for the validation failure.</value>
        public string ValidationMessage { get; set; }
    }
}
