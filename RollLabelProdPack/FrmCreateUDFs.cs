using System;
using System.Windows.Forms;
using RollLabelProdPack.Library.Utility;
using RollLabelProdPack.SAP.B1;

namespace RollLabelProdPack
{
    /// <summary>
    /// Simple form to create UDFs for generic roll label production
    /// Add this form to your project and call it from a menu item or button
    /// </summary>
    public partial class FrmCreateUDFs : Form
    {
        public FrmCreateUDFs()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // btnCreate
            //
            this.btnCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.Location = new System.Drawing.Point(12, 120);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(776, 50);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Create UDFs Now";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            //
            // txtOutput
            //
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(12, 186);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(776, 352);
            this.txtOutput.TabIndex = 1;
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(428, 24);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Create UDFs for Generic Roll Label Production";
            //
            // lblInstructions
            //
            this.lblInstructions.Location = new System.Drawing.Point(12, 45);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(776, 60);
            this.lblInstructions.TabIndex = 3;
            this.lblInstructions.Text = @"This utility will create 6 User-Defined Fields on the OWOR (Production Orders) table:
• U_SII_RollLabel, U_SII_BunLabel, U_SII_CoreLabel (label format paths)
• U_SII_RLabelQty, U_SII_BLabel, U_SII_CLabel (label quantities)

Click the button below to create these UDFs using SAP DI API.";
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(713, 544);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // FrmCreateUDFs
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 579);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnCreate);
            this.MinimumSize = new System.Drawing.Size(816, 618);
            this.Name = "FrmCreateUDFs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create UDFs - Roll Label Production";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Button btnClose;

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Disable button while processing
            btnCreate.Enabled = false;
            btnCreate.Text = "Creating UDFs...";
            txtOutput.Clear();
            Application.DoEvents();

            SAPB1 sapB1 = null;

            try
            {
                AppendOutput("Connecting to SAP Business One...");
                AppendOutput("");

                // Get SAP credentials from config
                var userName = AppUtility.GetSAPUser_Line1();
                var password = AppUtility.GetSAPPass_Line1();

                // Connect to SAP
                sapB1 = new SAPB1(userName, password);

                AppendOutput($"✓ Connected to SAP Business One");
                AppendOutput($"  Company: {sapB1.SapCompany.CompanyName}");
                AppendOutput($"  Database: {sapB1.SapCompany.CompanyDB}");
                AppendOutput("");

                // Create UDF creator
                var udfCreator = new UDFCreator(sapB1.SapCompany);

                AppendOutput("Creating UDFs on OWOR (Production Orders) table...");
                AppendOutput("==========================================");
                AppendOutput("");

                // Create all UDFs
                var result = udfCreator.CreateAllLabelUDFs();

                // Display results
                AppendOutput(result.Message);

                if (result.Success)
                {
                    AppendOutput("");
                    AppendOutput("Verifying UDFs created...");
                    AppendOutput("==========================================");

                    var existingUDFs = udfCreator.GetExistingUDFs("OWOR");
                    if (existingUDFs.Count > 0)
                    {
                        AppendOutput("Found UDFs:");
                        foreach (var udf in existingUDFs)
                        {
                            AppendOutput($"  {udf}");
                        }
                    }

                    AppendOutput("");
                    AppendOutput("==========================================");
                    AppendOutput("SUCCESS: All UDFs created successfully!");
                    AppendOutput("==========================================");
                    AppendOutput("");
                    AppendOutput("You can now use these UDFs in Production Orders to specify:");
                    AppendOutput("  • Custom label format files per order");
                    AppendOutput("  • Number of label copies to print");

                    MessageBox.Show(
                        "All UDFs created successfully!\n\nYou can now close this window.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    AppendOutput("");
                    AppendOutput("==========================================");
                    AppendOutput("WARNING: Some UDFs failed to create.");
                    AppendOutput("See error messages above for details.");
                    AppendOutput("==========================================");

                    MessageBox.Show(
                        "Some UDFs failed to create. See the output window for details.",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                AppendOutput("");
                AppendOutput("==========================================");
                AppendOutput("ERROR: Failed to create UDFs");
                AppendOutput("==========================================");
                AppendOutput($"Exception: {ex.Message}");
                AppendOutput("");
                AppendOutput("Stack Trace:");
                AppendOutput(ex.StackTrace);

                MessageBox.Show(
                    $"Error creating UDFs:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                // Disconnect from SAP
                if (sapB1 != null)
                {
                    sapB1.Dispose();
                    AppendOutput("");
                    AppendOutput("Disconnected from SAP Business One");
                }

                // Re-enable button
                btnCreate.Enabled = true;
                btnCreate.Text = "Create UDFs Now";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AppendOutput(string text)
        {
            txtOutput.AppendText(text + Environment.NewLine);
            txtOutput.SelectionStart = txtOutput.Text.Length;
            txtOutput.ScrollToCaret();
            Application.DoEvents();
        }
    }
}
