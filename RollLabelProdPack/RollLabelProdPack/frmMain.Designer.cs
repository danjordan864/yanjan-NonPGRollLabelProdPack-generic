namespace RollLabelProdPack
{
    partial class FrmMain
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
            this.boxScrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adjustResmixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testSQLConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testSAPB1ConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtIRMS = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDie = new System.Windows.Forms.TextBox();
            this.txtJumboRoll = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNoOfSlits = new System.Windows.Forms.TextBox();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.txtItemCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtShift = new System.Windows.Forms.TextBox();
            this.txtProductionLine = new System.Windows.Forms.TextBox();
            this.txtYJNProdOrder = new System.Windows.Forms.TextBox();
            this.lblOrderNo = new System.Windows.Forms.Label();
            this.txtOrderNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.txtEmployee = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lnkPlannedIssues = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.txtProductionDateFull = new System.Windows.Forms.TextBox();
            this.chkSP15 = new System.Windows.Forms.CheckBox();
            this.chkSP14 = new System.Windows.Forms.CheckBox();
            this.chkSP13 = new System.Windows.Forms.CheckBox();
            this.chkSP12 = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnGenerateRolls = new System.Windows.Forms.Button();
            this.chkSP11 = new System.Windows.Forms.CheckBox();
            this.chkSP10 = new System.Windows.Forms.CheckBox();
            this.chkSP9 = new System.Windows.Forms.CheckBox();
            this.chkSP8 = new System.Windows.Forms.CheckBox();
            this.chkSP7 = new System.Windows.Forms.CheckBox();
            this.chkSP6 = new System.Windows.Forms.CheckBox();
            this.chkSP5 = new System.Windows.Forms.CheckBox();
            this.chkSP4 = new System.Windows.Forms.CheckBox();
            this.chkSP3 = new System.Windows.Forms.CheckBox();
            this.chkSP2 = new System.Windows.Forms.CheckBox();
            this.chkSP1 = new System.Windows.Forms.CheckBox();
            this.txtWeightKgs = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.olvRolls = new BrightIdeasSoftware.ObjectListView();
            this.olvColScrap = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColRollNo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColKgs = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.chkSP18 = new System.Windows.Forms.CheckBox();
            this.chkSP17 = new System.Windows.Forms.CheckBox();
            this.chkSP16 = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvRolls)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formsToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1065, 29);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // formsToolStripMenuItem
            // 
            this.formsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reprintToolStripMenuItem,
            this.boxScrapToolStripMenuItem,
            this.adjustResmixToolStripMenuItem});
            this.formsToolStripMenuItem.Name = "formsToolStripMenuItem";
            this.formsToolStripMenuItem.Size = new System.Drawing.Size(66, 25);
            this.formsToolStripMenuItem.Text = "Forms";
            // 
            // reprintToolStripMenuItem
            // 
            this.reprintToolStripMenuItem.Name = "reprintToolStripMenuItem";
            this.reprintToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            this.reprintToolStripMenuItem.Text = "Reprint";
            this.reprintToolStripMenuItem.Click += new System.EventHandler(this.reprintToolStripMenuItem_Click);
            // 
            // boxScrapToolStripMenuItem
            // 
            this.boxScrapToolStripMenuItem.Name = "boxScrapToolStripMenuItem";
            this.boxScrapToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            this.boxScrapToolStripMenuItem.Text = "Box Scrap";
            this.boxScrapToolStripMenuItem.Click += new System.EventHandler(this.boxScrapToolStripMenuItem_Click);
            // 
            // adjustResmixToolStripMenuItem
            // 
            this.adjustResmixToolStripMenuItem.Name = "adjustResmixToolStripMenuItem";
            this.adjustResmixToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            this.adjustResmixToolStripMenuItem.Text = "Adjust Resmix";
            this.adjustResmixToolStripMenuItem.Click += new System.EventHandler(this.adjustResmixToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testSQLConnectionToolStripMenuItem,
            this.testSAPB1ConnectionToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(57, 25);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // testSQLConnectionToolStripMenuItem
            // 
            this.testSQLConnectionToolStripMenuItem.Name = "testSQLConnectionToolStripMenuItem";
            this.testSQLConnectionToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this.testSQLConnectionToolStripMenuItem.Text = "Test SQL Connection";
            this.testSQLConnectionToolStripMenuItem.Click += new System.EventHandler(this.testSQLConnectionToolStripMenuItem_Click);
            // 
            // testSAPB1ConnectionToolStripMenuItem
            // 
            this.testSAPB1ConnectionToolStripMenuItem.Name = "testSAPB1ConnectionToolStripMenuItem";
            this.testSAPB1ConnectionToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this.testSAPB1ConnectionToolStripMenuItem.Text = "Test SAP B1 Connection";
            this.testSAPB1ConnectionToolStripMenuItem.Click += new System.EventHandler(this.testSAPB1ConnectionToolStripMenuItem_Click);
            // 
            // txtIRMS
            // 
            this.txtIRMS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIRMS.Location = new System.Drawing.Point(146, 315);
            this.txtIRMS.Margin = new System.Windows.Forms.Padding(4);
            this.txtIRMS.Name = "txtIRMS";
            this.txtIRMS.ReadOnly = true;
            this.txtIRMS.Size = new System.Drawing.Size(85, 26);
            this.txtIRMS.TabIndex = 38;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(10, 318);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 20);
            this.label10.TabIndex = 37;
            this.label10.Text = "IRMS";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(10, 357);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 20);
            this.label8.TabIndex = 34;
            this.label8.Text = "Jumbo Roll";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(165, 53);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 20);
            this.label7.TabIndex = 32;
            this.label7.Text = "Die";
            // 
            // txtDie
            // 
            this.txtDie.Enabled = false;
            this.txtDie.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDie.Location = new System.Drawing.Point(205, 51);
            this.txtDie.Margin = new System.Windows.Forms.Padding(4);
            this.txtDie.Name = "txtDie";
            this.txtDie.Size = new System.Drawing.Size(76, 26);
            this.txtDie.TabIndex = 1;
            this.txtDie.Validated += new System.EventHandler(this.txtDie_Validated);
            // 
            // txtJumboRoll
            // 
            this.txtJumboRoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJumboRoll.Location = new System.Drawing.Point(146, 354);
            this.txtJumboRoll.Margin = new System.Windows.Forms.Padding(4);
            this.txtJumboRoll.Name = "txtJumboRoll";
            this.txtJumboRoll.ReadOnly = true;
            this.txtJumboRoll.Size = new System.Drawing.Size(44, 26);
            this.txtJumboRoll.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 54);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 20);
            this.label6.TabIndex = 30;
            this.label6.Text = "No. Of Slits";
            // 
            // txtNoOfSlits
            // 
            this.txtNoOfSlits.Enabled = false;
            this.txtNoOfSlits.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoOfSlits.Location = new System.Drawing.Point(94, 50);
            this.txtNoOfSlits.Margin = new System.Windows.Forms.Padding(4);
            this.txtNoOfSlits.Name = "txtNoOfSlits";
            this.txtNoOfSlits.Size = new System.Drawing.Size(59, 26);
            this.txtNoOfSlits.TabIndex = 0;
            this.txtNoOfSlits.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNoOfSlits_KeyPress);
            this.txtNoOfSlits.Validated += new System.EventHandler(this.txtNoOfSlits_Validated);
            // 
            // txtItemName
            // 
            this.txtItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemName.Location = new System.Drawing.Point(325, 276);
            this.txtItemName.Margin = new System.Windows.Forms.Padding(4);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.ReadOnly = true;
            this.txtItemName.Size = new System.Drawing.Size(306, 26);
            this.txtItemName.TabIndex = 28;
            // 
            // txtItemCode
            // 
            this.txtItemCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemCode.Location = new System.Drawing.Point(146, 276);
            this.txtItemCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.ReadOnly = true;
            this.txtItemCode.Size = new System.Drawing.Size(171, 26);
            this.txtItemCode.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 279);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 20);
            this.label5.TabIndex = 26;
            this.label5.Text = "Item";
            // 
            // txtShift
            // 
            this.txtShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtShift.Location = new System.Drawing.Point(146, 432);
            this.txtShift.Margin = new System.Windows.Forms.Padding(4);
            this.txtShift.Name = "txtShift";
            this.txtShift.ReadOnly = true;
            this.txtShift.Size = new System.Drawing.Size(68, 26);
            this.txtShift.TabIndex = 24;
            // 
            // txtProductionLine
            // 
            this.txtProductionLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductionLine.Location = new System.Drawing.Point(146, 159);
            this.txtProductionLine.Margin = new System.Windows.Forms.Padding(4);
            this.txtProductionLine.Name = "txtProductionLine";
            this.txtProductionLine.ReadOnly = true;
            this.txtProductionLine.Size = new System.Drawing.Size(68, 26);
            this.txtProductionLine.TabIndex = 23;
            // 
            // txtYJNProdOrder
            // 
            this.txtYJNProdOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtYJNProdOrder.Location = new System.Drawing.Point(146, 198);
            this.txtYJNProdOrder.Margin = new System.Windows.Forms.Padding(4);
            this.txtYJNProdOrder.Name = "txtYJNProdOrder";
            this.txtYJNProdOrder.ReadOnly = true;
            this.txtYJNProdOrder.Size = new System.Drawing.Size(216, 26);
            this.txtYJNProdOrder.TabIndex = 22;
            // 
            // lblOrderNo
            // 
            this.lblOrderNo.AutoSize = true;
            this.lblOrderNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderNo.Location = new System.Drawing.Point(10, 240);
            this.lblOrderNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOrderNo.Name = "lblOrderNo";
            this.lblOrderNo.Size = new System.Drawing.Size(113, 20);
            this.lblOrderNo.TabIndex = 21;
            this.lblOrderNo.Text = "SAP Order No.";
            // 
            // txtOrderNo
            // 
            this.txtOrderNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOrderNo.Location = new System.Drawing.Point(146, 237);
            this.txtOrderNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtOrderNo.Name = "txtOrderNo";
            this.txtOrderNo.ReadOnly = true;
            this.txtOrderNo.Size = new System.Drawing.Size(135, 26);
            this.txtOrderNo.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 435);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "Shift";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 201);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "Order";
            // 
            // lblEmployee
            // 
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployee.Location = new System.Drawing.Point(10, 396);
            this.lblEmployee.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(79, 20);
            this.lblEmployee.TabIndex = 15;
            this.lblEmployee.Text = "Employee";
            // 
            // txtEmployee
            // 
            this.txtEmployee.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmployee.Location = new System.Drawing.Point(146, 393);
            this.txtEmployee.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmployee.Name = "txtEmployee";
            this.txtEmployee.ReadOnly = true;
            this.txtEmployee.Size = new System.Drawing.Size(91, 26);
            this.txtEmployee.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 162);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Production Line";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 29);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chkSP16);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP17);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP18);
            this.splitContainer1.Panel1.Controls.Add(this.lnkPlannedIssues);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.txtProductionDateFull);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP15);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP14);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP13);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP12);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.btnSelect);
            this.splitContainer1.Panel1.Controls.Add(this.btnGenerateRolls);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP11);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP10);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP9);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP8);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP7);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP6);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP5);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP4);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP3);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP2);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP1);
            this.splitContainer1.Panel1.Controls.Add(this.txtDie);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.txtNoOfSlits);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.txtWeightKgs);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.txtIRMS);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.txtProductionLine);
            this.splitContainer1.Panel1.Controls.Add(this.txtShift);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.txtYJNProdOrder);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.txtOrderNo);
            this.splitContainer1.Panel1.Controls.Add(this.lblOrderNo);
            this.splitContainer1.Panel1.Controls.Add(this.txtItemCode);
            this.splitContainer1.Panel1.Controls.Add(this.txtJumboRoll);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.txtItemName);
            this.splitContainer1.Panel1.Controls.Add(this.lblEmployee);
            this.splitContainer1.Panel1.Controls.Add(this.txtEmployee);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnCreate);
            this.splitContainer1.Panel2.Controls.Add(this.olvRolls);
            this.splitContainer1.Size = new System.Drawing.Size(1065, 534);
            this.splitContainer1.SplitterDistance = 646;
            this.splitContainer1.TabIndex = 5;
            // 
            // lnkPlannedIssues
            // 
            this.lnkPlannedIssues.AutoSize = true;
            this.lnkPlannedIssues.Enabled = false;
            this.lnkPlannedIssues.Location = new System.Drawing.Point(539, 111);
            this.lnkPlannedIssues.Name = "lnkPlannedIssues";
            this.lnkPlannedIssues.Size = new System.Drawing.Size(105, 13);
            this.lnkPlannedIssues.TabIndex = 91;
            this.lnkPlannedIssues.TabStop = true;
            this.lnkPlannedIssues.Text = "View Planned Issues";
            this.lnkPlannedIssues.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPlannedIssues_LinkClicked);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(10, 474);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(124, 20);
            this.label9.TabIndex = 89;
            this.label9.Text = "Production Date";
            // 
            // txtProductionDateFull
            // 
            this.txtProductionDateFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductionDateFull.Location = new System.Drawing.Point(146, 471);
            this.txtProductionDateFull.Margin = new System.Windows.Forms.Padding(4);
            this.txtProductionDateFull.Name = "txtProductionDateFull";
            this.txtProductionDateFull.ReadOnly = true;
            this.txtProductionDateFull.Size = new System.Drawing.Size(91, 26);
            this.txtProductionDateFull.TabIndex = 88;
            // 
            // chkSP15
            // 
            this.chkSP15.AutoSize = true;
            this.chkSP15.Checked = true;
            this.chkSP15.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP15.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP15.Location = new System.Drawing.Point(489, 91);
            this.chkSP15.Name = "chkSP15";
            this.chkSP15.Size = new System.Drawing.Size(38, 17);
            this.chkSP15.TabIndex = 56;
            this.chkSP15.Tag = "15";
            this.chkSP15.Text = "15";
            this.chkSP15.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP15.UseVisualStyleBackColor = true;
            this.chkSP15.Visible = false;
            // 
            // chkSP14
            // 
            this.chkSP14.AutoSize = true;
            this.chkSP14.Checked = true;
            this.chkSP14.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP14.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP14.Location = new System.Drawing.Point(451, 91);
            this.chkSP14.Name = "chkSP14";
            this.chkSP14.Size = new System.Drawing.Size(38, 17);
            this.chkSP14.TabIndex = 55;
            this.chkSP14.Tag = "14";
            this.chkSP14.Text = "14";
            this.chkSP14.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP14.UseVisualStyleBackColor = true;
            this.chkSP14.Visible = false;
            // 
            // chkSP13
            // 
            this.chkSP13.AutoSize = true;
            this.chkSP13.Checked = true;
            this.chkSP13.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP13.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP13.Location = new System.Drawing.Point(413, 91);
            this.chkSP13.Name = "chkSP13";
            this.chkSP13.Size = new System.Drawing.Size(38, 17);
            this.chkSP13.TabIndex = 54;
            this.chkSP13.Tag = "13";
            this.chkSP13.Text = "13";
            this.chkSP13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP13.UseVisualStyleBackColor = true;
            this.chkSP13.Visible = false;
            // 
            // chkSP12
            // 
            this.chkSP12.AutoSize = true;
            this.chkSP12.Checked = true;
            this.chkSP12.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP12.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP12.Location = new System.Drawing.Point(375, 91);
            this.chkSP12.Name = "chkSP12";
            this.chkSP12.Size = new System.Drawing.Size(38, 17);
            this.chkSP12.TabIndex = 53;
            this.chkSP12.Tag = "12";
            this.chkSP12.Text = "12";
            this.chkSP12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP12.UseVisualStyleBackColor = true;
            this.chkSP12.Visible = false;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Location = new System.Drawing.Point(3, 140);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(641, 2);
            this.label11.TabIndex = 52;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(0, 0);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(142, 42);
            this.btnSelect.TabIndex = 51;
            this.btnSelect.Text = "SELECT ORDER";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnGenerateRolls
            // 
            this.btnGenerateRolls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateRolls.Enabled = false;
            this.btnGenerateRolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateRolls.Location = new System.Drawing.Point(548, 45);
            this.btnGenerateRolls.Name = "btnGenerateRolls";
            this.btnGenerateRolls.Size = new System.Drawing.Size(95, 37);
            this.btnGenerateRolls.TabIndex = 4;
            this.btnGenerateRolls.Text = ">>>>";
            this.btnGenerateRolls.UseVisualStyleBackColor = true;
            this.btnGenerateRolls.Click += new System.EventHandler(this.btnGenerateRolls_Click);
            // 
            // chkSP11
            // 
            this.chkSP11.AutoSize = true;
            this.chkSP11.Checked = true;
            this.chkSP11.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP11.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP11.Location = new System.Drawing.Point(337, 91);
            this.chkSP11.Name = "chkSP11";
            this.chkSP11.Size = new System.Drawing.Size(38, 17);
            this.chkSP11.TabIndex = 49;
            this.chkSP11.Tag = "11";
            this.chkSP11.Text = "11";
            this.chkSP11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP11.UseVisualStyleBackColor = true;
            this.chkSP11.Visible = false;
            // 
            // chkSP10
            // 
            this.chkSP10.AutoSize = true;
            this.chkSP10.Checked = true;
            this.chkSP10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP10.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP10.Location = new System.Drawing.Point(299, 91);
            this.chkSP10.Name = "chkSP10";
            this.chkSP10.Size = new System.Drawing.Size(38, 17);
            this.chkSP10.TabIndex = 48;
            this.chkSP10.Tag = "10";
            this.chkSP10.Text = "10";
            this.chkSP10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP10.UseVisualStyleBackColor = true;
            this.chkSP10.Visible = false;
            // 
            // chkSP9
            // 
            this.chkSP9.AutoSize = true;
            this.chkSP9.Checked = true;
            this.chkSP9.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP9.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP9.Location = new System.Drawing.Point(267, 91);
            this.chkSP9.Name = "chkSP9";
            this.chkSP9.Size = new System.Drawing.Size(32, 17);
            this.chkSP9.TabIndex = 47;
            this.chkSP9.Tag = "9";
            this.chkSP9.Text = "9";
            this.chkSP9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP9.UseVisualStyleBackColor = true;
            this.chkSP9.Visible = false;
            // 
            // chkSP8
            // 
            this.chkSP8.AutoSize = true;
            this.chkSP8.Checked = true;
            this.chkSP8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP8.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP8.Location = new System.Drawing.Point(235, 91);
            this.chkSP8.Name = "chkSP8";
            this.chkSP8.Size = new System.Drawing.Size(32, 17);
            this.chkSP8.TabIndex = 46;
            this.chkSP8.Tag = "8";
            this.chkSP8.Text = "8";
            this.chkSP8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP8.UseVisualStyleBackColor = true;
            this.chkSP8.Visible = false;
            // 
            // chkSP7
            // 
            this.chkSP7.AutoSize = true;
            this.chkSP7.Checked = true;
            this.chkSP7.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP7.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP7.Location = new System.Drawing.Point(203, 91);
            this.chkSP7.Name = "chkSP7";
            this.chkSP7.Size = new System.Drawing.Size(32, 17);
            this.chkSP7.TabIndex = 45;
            this.chkSP7.Tag = "7";
            this.chkSP7.Text = "7";
            this.chkSP7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP7.UseVisualStyleBackColor = true;
            this.chkSP7.Visible = false;
            // 
            // chkSP6
            // 
            this.chkSP6.AutoSize = true;
            this.chkSP6.Checked = true;
            this.chkSP6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP6.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP6.Location = new System.Drawing.Point(171, 91);
            this.chkSP6.Name = "chkSP6";
            this.chkSP6.Size = new System.Drawing.Size(32, 17);
            this.chkSP6.TabIndex = 44;
            this.chkSP6.Tag = "6";
            this.chkSP6.Text = "6";
            this.chkSP6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP6.UseVisualStyleBackColor = true;
            this.chkSP6.Visible = false;
            // 
            // chkSP5
            // 
            this.chkSP5.AutoSize = true;
            this.chkSP5.Checked = true;
            this.chkSP5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP5.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP5.Location = new System.Drawing.Point(139, 91);
            this.chkSP5.Name = "chkSP5";
            this.chkSP5.Size = new System.Drawing.Size(32, 17);
            this.chkSP5.TabIndex = 43;
            this.chkSP5.Tag = "5";
            this.chkSP5.Text = "5";
            this.chkSP5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP5.UseVisualStyleBackColor = true;
            this.chkSP5.Visible = false;
            // 
            // chkSP4
            // 
            this.chkSP4.AutoSize = true;
            this.chkSP4.Checked = true;
            this.chkSP4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP4.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP4.Location = new System.Drawing.Point(107, 91);
            this.chkSP4.Name = "chkSP4";
            this.chkSP4.Size = new System.Drawing.Size(32, 17);
            this.chkSP4.TabIndex = 42;
            this.chkSP4.Tag = "4";
            this.chkSP4.Text = "4";
            this.chkSP4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP4.UseVisualStyleBackColor = true;
            this.chkSP4.Visible = false;
            // 
            // chkSP3
            // 
            this.chkSP3.AutoSize = true;
            this.chkSP3.Checked = true;
            this.chkSP3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP3.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP3.Location = new System.Drawing.Point(75, 91);
            this.chkSP3.Name = "chkSP3";
            this.chkSP3.Size = new System.Drawing.Size(32, 17);
            this.chkSP3.TabIndex = 41;
            this.chkSP3.Tag = "3";
            this.chkSP3.Text = "3";
            this.chkSP3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP3.UseVisualStyleBackColor = true;
            this.chkSP3.Visible = false;
            // 
            // chkSP2
            // 
            this.chkSP2.AutoSize = true;
            this.chkSP2.Checked = true;
            this.chkSP2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP2.Location = new System.Drawing.Point(43, 91);
            this.chkSP2.Name = "chkSP2";
            this.chkSP2.Size = new System.Drawing.Size(32, 17);
            this.chkSP2.TabIndex = 40;
            this.chkSP2.Tag = "2";
            this.chkSP2.Text = "2";
            this.chkSP2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP2.UseVisualStyleBackColor = true;
            this.chkSP2.Visible = false;
            // 
            // chkSP1
            // 
            this.chkSP1.AutoSize = true;
            this.chkSP1.Checked = true;
            this.chkSP1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP1.Location = new System.Drawing.Point(11, 91);
            this.chkSP1.Name = "chkSP1";
            this.chkSP1.Size = new System.Drawing.Size(32, 17);
            this.chkSP1.TabIndex = 39;
            this.chkSP1.Tag = "1";
            this.chkSP1.Text = "1";
            this.chkSP1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP1.UseVisualStyleBackColor = true;
            this.chkSP1.Visible = false;
            // 
            // txtWeightKgs
            // 
            this.txtWeightKgs.Enabled = false;
            this.txtWeightKgs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWeightKgs.Location = new System.Drawing.Point(334, 51);
            this.txtWeightKgs.Name = "txtWeightKgs";
            this.txtWeightKgs.Size = new System.Drawing.Size(77, 26);
            this.txtWeightKgs.TabIndex = 2;
            this.txtWeightKgs.Validated += new System.EventHandler(this.txtWeightKgs_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(289, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Kgs";
            this.label4.Validated += new System.EventHandler(this.txtWeightKgs_Validated);
            // 
            // btnCreate
            // 
            this.btnCreate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCreate.Enabled = false;
            this.btnCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.Location = new System.Drawing.Point(0, 498);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(415, 36);
            this.btnCreate.TabIndex = 12;
            this.btnCreate.Text = "Create/Print";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // olvRolls
            // 
            this.olvRolls.AllColumns.Add(this.olvColScrap);
            this.olvRolls.AllColumns.Add(this.olvColRollNo);
            this.olvRolls.AllColumns.Add(this.olvColKgs);
            this.olvRolls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvRolls.CellEditUseWholeCell = false;
            this.olvRolls.CheckBoxes = true;
            this.olvRolls.CheckedAspectName = "Scrap";
            this.olvRolls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColScrap,
            this.olvColRollNo,
            this.olvColKgs});
            this.olvRolls.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvRolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvRolls.Location = new System.Drawing.Point(3, 3);
            this.olvRolls.Name = "olvRolls";
            this.olvRolls.Size = new System.Drawing.Size(409, 499);
            this.olvRolls.TabIndex = 11;
            this.olvRolls.UseCompatibleStateImageBehavior = false;
            this.olvRolls.View = System.Windows.Forms.View.Details;
            // 
            // olvColScrap
            // 
            this.olvColScrap.AspectName = "Scrap";
            this.olvColScrap.CheckBoxes = true;
            this.olvColScrap.Text = "Scrap";
            // 
            // olvColRollNo
            // 
            this.olvColRollNo.AspectName = "RollNo";
            this.olvColRollNo.Groupable = false;
            this.olvColRollNo.Text = "Roll No.";
            this.olvColRollNo.Width = 200;
            // 
            // olvColKgs
            // 
            this.olvColKgs.AspectName = "Kgs";
            this.olvColKgs.Text = "Kgs";
            this.olvColKgs.Width = 100;
            // 
            // chkSP18
            // 
            this.chkSP18.AutoSize = true;
            this.chkSP18.Checked = true;
            this.chkSP18.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP18.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP18.Location = new System.Drawing.Point(603, 91);
            this.chkSP18.Name = "chkSP18";
            this.chkSP18.Size = new System.Drawing.Size(38, 17);
            this.chkSP18.TabIndex = 92;
            this.chkSP18.Tag = "18";
            this.chkSP18.Text = "18";
            this.chkSP18.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP18.UseVisualStyleBackColor = true;
            this.chkSP18.Visible = false;
            // 
            // chkSP17
            // 
            this.chkSP17.AutoSize = true;
            this.chkSP17.Checked = true;
            this.chkSP17.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP17.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP17.Location = new System.Drawing.Point(565, 91);
            this.chkSP17.Name = "chkSP17";
            this.chkSP17.Size = new System.Drawing.Size(38, 17);
            this.chkSP17.TabIndex = 93;
            this.chkSP17.Tag = "17";
            this.chkSP17.Text = "17";
            this.chkSP17.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP17.UseVisualStyleBackColor = true;
            this.chkSP17.Visible = false;
            // 
            // chkSP16
            // 
            this.chkSP16.AutoSize = true;
            this.chkSP16.Checked = true;
            this.chkSP16.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP16.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP16.Location = new System.Drawing.Point(527, 91);
            this.chkSP16.Name = "chkSP16";
            this.chkSP16.Size = new System.Drawing.Size(38, 17);
            this.chkSP16.TabIndex = 94;
            this.chkSP16.Tag = "16";
            this.chkSP16.Text = "16";
            this.chkSP16.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP16.UseVisualStyleBackColor = true;
            this.chkSP16.Visible = false;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 563);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvRolls)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testSQLConnectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testSAPB1ConnectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reprintToolStripMenuItem;
        private System.Windows.Forms.TextBox txtShift;
        private System.Windows.Forms.TextBox txtProductionLine;
        private System.Windows.Forms.TextBox txtYJNProdOrder;
        private System.Windows.Forms.Label lblOrderNo;
        private System.Windows.Forms.TextBox txtOrderNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.TextBox txtEmployee;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtItemCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDie;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNoOfSlits;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtJumboRoll;
        private System.Windows.Forms.TextBox txtIRMS;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox chkSP1;
        private System.Windows.Forms.TextBox txtWeightKgs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCreate;
        private BrightIdeasSoftware.ObjectListView olvRolls;
        private BrightIdeasSoftware.OLVColumn olvColScrap;
        private BrightIdeasSoftware.OLVColumn olvColRollNo;
        private BrightIdeasSoftware.OLVColumn olvColKgs;
        private System.Windows.Forms.CheckBox chkSP11;
        private System.Windows.Forms.CheckBox chkSP10;
        private System.Windows.Forms.CheckBox chkSP9;
        private System.Windows.Forms.CheckBox chkSP8;
        private System.Windows.Forms.CheckBox chkSP7;
        private System.Windows.Forms.CheckBox chkSP6;
        private System.Windows.Forms.CheckBox chkSP5;
        private System.Windows.Forms.CheckBox chkSP4;
        private System.Windows.Forms.CheckBox chkSP3;
        private System.Windows.Forms.CheckBox chkSP2;
        private System.Windows.Forms.Button btnGenerateRolls;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkSP15;
        private System.Windows.Forms.CheckBox chkSP14;
        private System.Windows.Forms.CheckBox chkSP13;
        private System.Windows.Forms.CheckBox chkSP12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtProductionDateFull;
        private System.Windows.Forms.LinkLabel lnkPlannedIssues;
        private System.Windows.Forms.ToolStripMenuItem boxScrapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adjustResmixToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkSP16;
        private System.Windows.Forms.CheckBox chkSP17;
        private System.Windows.Forms.CheckBox chkSP18;
    }
}

