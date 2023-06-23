namespace RollLabelProdPack
{
    partial class frmCoPack
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.coPackProductionUserControl1 = new RollLabelProdPack.CoPackProductionUserControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.coPackProductionUserControl1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(942, 399);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Produce";
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(52, 15);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(142, 42);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "SELECT ORDER";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // coPackProductionUserControl1
            // 
            this.coPackProductionUserControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.coPackProductionUserControl1.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.coPackProductionUserControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coPackProductionUserControl1.Location = new System.Drawing.Point(7, 18);
            this.coPackProductionUserControl1.Margin = new System.Windows.Forms.Padding(4);
            this.coPackProductionUserControl1.Name = "coPackProductionUserControl1";
            this.coPackProductionUserControl1.NumberOfCases = 0;
            this.coPackProductionUserControl1.Order = null;
            this.coPackProductionUserControl1.Qty = 1;
            this.coPackProductionUserControl1.Size = new System.Drawing.Size(911, 370);
            this.coPackProductionUserControl1.TabIndex = 1;
            this.coPackProductionUserControl1.ValidationFailed += new CoPackProductionUserControl.CoPackProductionValidationHandler(this.coPackProductionUserControl1_ValidationFailed);
            this.coPackProductionUserControl1.IssuesRefreshRequested += new CoPackProductionUserControl.CoPackProductionRefreshIssuesHandler(this.coPackProductionUserControl1_IssuesRefreshRequested);
            this.coPackProductionUserControl1.LotNumberEntered += new CoPackProductionUserControl.CoPackProductionLotNumberEnteredHandler(this.coPackProductionUserControl1_LotNumberEntered);
            // 
            // frmCoPack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 474);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(982, 513);
            this.Name = "frmCoPack";
            this.Text = "Co-Pack Production";
            this.Load += new System.EventHandler(this.frmCoPack_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelect;
        private CoPackProductionUserControl coPackProductionUserControl1;
    }
}