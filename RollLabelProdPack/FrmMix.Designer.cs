namespace RollLabelProdPack
{
    partial class FrmMix
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.formsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reprintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtWeightKgs = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtProductionDateFull = new System.Windows.Forms.TextBox();
            this.txtProductionLine = new System.Windows.Forms.TextBox();
            this.txtShift = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOrderNo = new System.Windows.Forms.TextBox();
            this.lblOrderNo = new System.Windows.Forms.Label();
            this.txtItemCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.txtEmployee = new System.Windows.Forms.TextBox();
            this.btnProduce = new System.Windows.Forms.Button();
            this.lnkPlannedIssues = new System.Windows.Forms.LinkLabel();
            this.txtBatch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboToLine = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(808, 29);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // formsToolStripMenuItem
            // 
            this.formsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reprintToolStripMenuItem});
            this.formsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.formsToolStripMenuItem.Name = "formsToolStripMenuItem";
            this.formsToolStripMenuItem.Size = new System.Drawing.Size(66, 25);
            this.formsToolStripMenuItem.Text = "Forms";
            // 
            // reprintToolStripMenuItem
            // 
            this.reprintToolStripMenuItem.Name = "reprintToolStripMenuItem";
            this.reprintToolStripMenuItem.Size = new System.Drawing.Size(131, 26);
            this.reprintToolStripMenuItem.Text = "Reprint";
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(14, 33);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(142, 42);
            this.btnSelect.TabIndex = 52;
            this.btnSelect.Text = "SELECT ORDER";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtWeightKgs
            // 
            this.txtWeightKgs.Enabled = false;
            this.txtWeightKgs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWeightKgs.Location = new System.Drawing.Point(429, 30);
            this.txtWeightKgs.Name = "txtWeightKgs";
            this.txtWeightKgs.Size = new System.Drawing.Size(115, 26);
            this.txtWeightKgs.TabIndex = 53;
            this.txtWeightKgs.Validated += new System.EventHandler(this.txtWeightKgs_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(385, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 20);
            this.label4.TabIndex = 54;
            this.label4.Text = "Kgs";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(23, 296);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(124, 20);
            this.label9.TabIndex = 108;
            this.label9.Text = "Production Date";
            // 
            // txtProductionDateFull
            // 
            this.txtProductionDateFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductionDateFull.Location = new System.Drawing.Point(159, 293);
            this.txtProductionDateFull.Margin = new System.Windows.Forms.Padding(4);
            this.txtProductionDateFull.Name = "txtProductionDateFull";
            this.txtProductionDateFull.ReadOnly = true;
            this.txtProductionDateFull.Size = new System.Drawing.Size(91, 26);
            this.txtProductionDateFull.TabIndex = 107;
            // 
            // txtProductionLine
            // 
            this.txtProductionLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductionLine.Location = new System.Drawing.Point(159, 98);
            this.txtProductionLine.Margin = new System.Windows.Forms.Padding(4);
            this.txtProductionLine.Name = "txtProductionLine";
            this.txtProductionLine.ReadOnly = true;
            this.txtProductionLine.Size = new System.Drawing.Size(135, 26);
            this.txtProductionLine.TabIndex = 98;
            // 
            // txtShift
            // 
            this.txtShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtShift.Location = new System.Drawing.Point(159, 254);
            this.txtShift.Margin = new System.Windows.Forms.Padding(4);
            this.txtShift.Name = "txtShift";
            this.txtShift.ReadOnly = true;
            this.txtShift.Size = new System.Drawing.Size(68, 26);
            this.txtShift.TabIndex = 99;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 257);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 20);
            this.label3.TabIndex = 94;
            this.label3.Text = "Shift";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 101);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 90;
            this.label1.Text = "Production Line";
            // 
            // txtOrderNo
            // 
            this.txtOrderNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOrderNo.Location = new System.Drawing.Point(159, 136);
            this.txtOrderNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtOrderNo.Name = "txtOrderNo";
            this.txtOrderNo.ReadOnly = true;
            this.txtOrderNo.Size = new System.Drawing.Size(135, 26);
            this.txtOrderNo.TabIndex = 95;
            // 
            // lblOrderNo
            // 
            this.lblOrderNo.AutoSize = true;
            this.lblOrderNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderNo.Location = new System.Drawing.Point(23, 139);
            this.lblOrderNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOrderNo.Name = "lblOrderNo";
            this.lblOrderNo.Size = new System.Drawing.Size(113, 20);
            this.lblOrderNo.TabIndex = 96;
            this.lblOrderNo.Text = "SAP Order No.";
            // 
            // txtItemCode
            // 
            this.txtItemCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemCode.Location = new System.Drawing.Point(159, 175);
            this.txtItemCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.ReadOnly = true;
            this.txtItemCode.Size = new System.Drawing.Size(171, 26);
            this.txtItemCode.TabIndex = 101;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(23, 178);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 20);
            this.label5.TabIndex = 100;
            this.label5.Text = "Item";
            // 
            // txtItemName
            // 
            this.txtItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemName.Location = new System.Drawing.Point(338, 175);
            this.txtItemName.Margin = new System.Windows.Forms.Padding(4);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.ReadOnly = true;
            this.txtItemName.Size = new System.Drawing.Size(306, 26);
            this.txtItemName.TabIndex = 102;
            // 
            // lblEmployee
            // 
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployee.Location = new System.Drawing.Point(23, 218);
            this.lblEmployee.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(79, 20);
            this.lblEmployee.TabIndex = 92;
            this.lblEmployee.Text = "Employee";
            // 
            // txtEmployee
            // 
            this.txtEmployee.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmployee.Location = new System.Drawing.Point(159, 215);
            this.txtEmployee.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmployee.Name = "txtEmployee";
            this.txtEmployee.ReadOnly = true;
            this.txtEmployee.Size = new System.Drawing.Size(91, 26);
            this.txtEmployee.TabIndex = 91;
            // 
            // btnProduce
            // 
            this.btnProduce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProduce.Enabled = false;
            this.btnProduce.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProduce.Location = new System.Drawing.Point(580, 30);
            this.btnProduce.Margin = new System.Windows.Forms.Padding(4);
            this.btnProduce.Name = "btnProduce";
            this.btnProduce.Size = new System.Drawing.Size(142, 42);
            this.btnProduce.TabIndex = 109;
            this.btnProduce.Text = "PRODUCE";
            this.btnProduce.UseVisualStyleBackColor = true;
            this.btnProduce.Click += new System.EventHandler(this.btnProduce_Click);
            // 
            // lnkPlannedIssues
            // 
            this.lnkPlannedIssues.AutoSize = true;
            this.lnkPlannedIssues.Enabled = false;
            this.lnkPlannedIssues.Location = new System.Drawing.Point(539, 79);
            this.lnkPlannedIssues.Name = "lnkPlannedIssues";
            this.lnkPlannedIssues.Size = new System.Drawing.Size(105, 13);
            this.lnkPlannedIssues.TabIndex = 110;
            this.lnkPlannedIssues.TabStop = true;
            this.lnkPlannedIssues.Text = "View Planned Issues";
            this.lnkPlannedIssues.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPlannedIssues_LinkClicked);
            // 
            // txtBatch
            // 
            this.txtBatch.Enabled = false;
            this.txtBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBatch.Location = new System.Drawing.Point(227, 64);
            this.txtBatch.Name = "txtBatch";
            this.txtBatch.Size = new System.Drawing.Size(142, 26);
            this.txtBatch.TabIndex = 111;
            this.txtBatch.Validated += new System.EventHandler(this.txtBatch_Validated);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(156, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 112;
            this.label2.Text = "Batch";
            // 
            // cboToLine
            // 
            this.cboToLine.Enabled = false;
            this.cboToLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboToLine.FormattingEnabled = true;
            this.cboToLine.Location = new System.Drawing.Point(227, 30);
            this.cboToLine.Name = "cboToLine";
            this.cboToLine.Size = new System.Drawing.Size(142, 28);
            this.cboToLine.TabIndex = 113;
            this.cboToLine.SelectedIndexChanged += new System.EventHandler(this.cboToLine_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(159, 33);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 20);
            this.label6.TabIndex = 114;
            this.label6.Text = "To Line";
            // 
            // FrmMix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 369);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboToLine);
            this.Controls.Add(this.txtBatch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lnkPlannedIssues);
            this.Controls.Add(this.btnProduce);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtProductionDateFull);
            this.Controls.Add(this.txtProductionLine);
            this.Controls.Add(this.txtShift);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOrderNo);
            this.Controls.Add(this.lblOrderNo);
            this.Controls.Add(this.txtItemCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtItemName);
            this.Controls.Add(this.lblEmployee);
            this.Controls.Add(this.txtEmployee);
            this.Controls.Add(this.txtWeightKgs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMix";
            this.Text = "Resmix Production";
            this.Load += new System.EventHandler(this.FrmMix_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem formsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reprintToolStripMenuItem;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtWeightKgs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtProductionDateFull;
        private System.Windows.Forms.TextBox txtProductionLine;
        private System.Windows.Forms.TextBox txtShift;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOrderNo;
        private System.Windows.Forms.Label lblOrderNo;
        private System.Windows.Forms.TextBox txtItemCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.TextBox txtEmployee;
        private System.Windows.Forms.Button btnProduce;
        private System.Windows.Forms.LinkLabel lnkPlannedIssues;
        private System.Windows.Forms.TextBox txtBatch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboToLine;
        private System.Windows.Forms.Label label6;
    }
}