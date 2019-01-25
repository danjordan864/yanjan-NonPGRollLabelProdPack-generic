namespace RollLabelProdPack
{
    partial class FrmCreateRolls
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
            this.chkSP16 = new System.Windows.Forms.CheckBox();
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
            this.olvColRollNo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColScrap = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColKgs = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.chkSP18 = new System.Windows.Forms.CheckBox();
            this.chkSP17 = new System.Windows.Forms.CheckBox();
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
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formsToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1037, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // formsToolStripMenuItem
            // 
            this.formsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reprintToolStripMenuItem});
            this.formsToolStripMenuItem.Name = "formsToolStripMenuItem";
            this.formsToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.formsToolStripMenuItem.Text = "Forms";
            // 
            // reprintToolStripMenuItem
            // 
            this.reprintToolStripMenuItem.Name = "reprintToolStripMenuItem";
            this.reprintToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.reprintToolStripMenuItem.Text = "Reprint";
            this.reprintToolStripMenuItem.Click += new System.EventHandler(this.reprintToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testSQLConnectionToolStripMenuItem,
            this.testSAPB1ConnectionToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // testSQLConnectionToolStripMenuItem
            // 
            this.testSQLConnectionToolStripMenuItem.Name = "testSQLConnectionToolStripMenuItem";
            this.testSQLConnectionToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.testSQLConnectionToolStripMenuItem.Text = "Test SQL Connection";
            this.testSQLConnectionToolStripMenuItem.Click += new System.EventHandler(this.testSQLConnectionToolStripMenuItem_Click);
            // 
            // testSAPB1ConnectionToolStripMenuItem
            // 
            this.testSAPB1ConnectionToolStripMenuItem.Name = "testSAPB1ConnectionToolStripMenuItem";
            this.testSAPB1ConnectionToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.testSAPB1ConnectionToolStripMenuItem.Text = "Test SAP B1 Connection";
            this.testSAPB1ConnectionToolStripMenuItem.Click += new System.EventHandler(this.testSAPB1ConnectionToolStripMenuItem_Click);
            // 
            // txtIRMS
            // 
            this.txtIRMS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIRMS.Location = new System.Drawing.Point(146, 279);
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
            this.label10.Location = new System.Drawing.Point(10, 282);
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
            this.label8.Location = new System.Drawing.Point(10, 321);
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
            this.label7.Location = new System.Drawing.Point(169, 7);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 20);
            this.label7.TabIndex = 32;
            this.label7.Text = "Die";
            // 
            // txtDie
            // 
            this.txtDie.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDie.Location = new System.Drawing.Point(209, 4);
            this.txtDie.Margin = new System.Windows.Forms.Padding(4);
            this.txtDie.Name = "txtDie";
            this.txtDie.Size = new System.Drawing.Size(76, 26);
            this.txtDie.TabIndex = 31;
            // 
            // txtJumboRoll
            // 
            this.txtJumboRoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJumboRoll.Location = new System.Drawing.Point(146, 318);
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
            this.label6.Location = new System.Drawing.Point(10, 8);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 20);
            this.label6.TabIndex = 30;
            this.label6.Text = "No. Of Slits";
            // 
            // txtNoOfSlits
            // 
            this.txtNoOfSlits.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoOfSlits.Location = new System.Drawing.Point(98, 4);
            this.txtNoOfSlits.Margin = new System.Windows.Forms.Padding(4);
            this.txtNoOfSlits.Name = "txtNoOfSlits";
            this.txtNoOfSlits.Size = new System.Drawing.Size(59, 26);
            this.txtNoOfSlits.TabIndex = 29;
            this.txtNoOfSlits.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNoOfSlits_KeyPress);
            this.txtNoOfSlits.Validated += new System.EventHandler(this.txtNoOfSlits_Validated);
            // 
            // txtItemName
            // 
            this.txtItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemName.Location = new System.Drawing.Point(258, 240);
            this.txtItemName.Margin = new System.Windows.Forms.Padding(4);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.ReadOnly = true;
            this.txtItemName.Size = new System.Drawing.Size(216, 26);
            this.txtItemName.TabIndex = 28;
            // 
            // txtItemCode
            // 
            this.txtItemCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemCode.Location = new System.Drawing.Point(146, 240);
            this.txtItemCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.ReadOnly = true;
            this.txtItemCode.Size = new System.Drawing.Size(104, 26);
            this.txtItemCode.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 243);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 20);
            this.label5.TabIndex = 26;
            this.label5.Text = "Item";
            // 
            // txtShift
            // 
            this.txtShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtShift.Location = new System.Drawing.Point(146, 396);
            this.txtShift.Margin = new System.Windows.Forms.Padding(4);
            this.txtShift.Name = "txtShift";
            this.txtShift.ReadOnly = true;
            this.txtShift.Size = new System.Drawing.Size(68, 26);
            this.txtShift.TabIndex = 24;
            // 
            // txtProductionLine
            // 
            this.txtProductionLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductionLine.Location = new System.Drawing.Point(146, 123);
            this.txtProductionLine.Margin = new System.Windows.Forms.Padding(4);
            this.txtProductionLine.Name = "txtProductionLine";
            this.txtProductionLine.ReadOnly = true;
            this.txtProductionLine.Size = new System.Drawing.Size(68, 26);
            this.txtProductionLine.TabIndex = 23;
            // 
            // txtYJNProdOrder
            // 
            this.txtYJNProdOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtYJNProdOrder.Location = new System.Drawing.Point(146, 162);
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
            this.lblOrderNo.Location = new System.Drawing.Point(10, 204);
            this.lblOrderNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOrderNo.Name = "lblOrderNo";
            this.lblOrderNo.Size = new System.Drawing.Size(113, 20);
            this.lblOrderNo.TabIndex = 21;
            this.lblOrderNo.Text = "SAP Order No.";
            // 
            // txtOrderNo
            // 
            this.txtOrderNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOrderNo.Location = new System.Drawing.Point(146, 201);
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
            this.label3.Location = new System.Drawing.Point(10, 399);
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
            this.label2.Location = new System.Drawing.Point(10, 165);
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
            this.lblEmployee.Location = new System.Drawing.Point(10, 360);
            this.lblEmployee.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(79, 20);
            this.lblEmployee.TabIndex = 15;
            this.lblEmployee.Text = "Employee";
            // 
            // txtEmployee
            // 
            this.txtEmployee.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmployee.Location = new System.Drawing.Point(146, 357);
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
            this.label1.Location = new System.Drawing.Point(10, 126);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Production Line";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chkSP18);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP17);
            this.splitContainer1.Panel1.Controls.Add(this.chkSP16);
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
            this.splitContainer1.Size = new System.Drawing.Size(1037, 553);
            this.splitContainer1.SplitterDistance = 740;
            this.splitContainer1.TabIndex = 5;
            // 
            // chkSP16
            // 
            this.chkSP16.AutoSize = true;
            this.chkSP16.Checked = true;
            this.chkSP16.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP16.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP16.Location = new System.Drawing.Point(616, 52);
            this.chkSP16.Name = "chkSP16";
            this.chkSP16.Size = new System.Drawing.Size(38, 17);
            this.chkSP16.TabIndex = 90;
            this.chkSP16.Tag = "16";
            this.chkSP16.Text = "16";
            this.chkSP16.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP16.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(10, 438);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(124, 20);
            this.label9.TabIndex = 89;
            this.label9.Text = "Production Date";
            // 
            // txtProductionDateFull
            // 
            this.txtProductionDateFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductionDateFull.Location = new System.Drawing.Point(146, 435);
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
            this.chkSP15.Location = new System.Drawing.Point(578, 52);
            this.chkSP15.Name = "chkSP15";
            this.chkSP15.Size = new System.Drawing.Size(38, 17);
            this.chkSP15.TabIndex = 56;
            this.chkSP15.Tag = "15";
            this.chkSP15.Text = "15";
            this.chkSP15.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP15.UseVisualStyleBackColor = true;
            // 
            // chkSP14
            // 
            this.chkSP14.AutoSize = true;
            this.chkSP14.Checked = true;
            this.chkSP14.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP14.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP14.Location = new System.Drawing.Point(538, 52);
            this.chkSP14.Name = "chkSP14";
            this.chkSP14.Size = new System.Drawing.Size(38, 17);
            this.chkSP14.TabIndex = 55;
            this.chkSP14.Tag = "14";
            this.chkSP14.Text = "14";
            this.chkSP14.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP14.UseVisualStyleBackColor = true;
            // 
            // chkSP13
            // 
            this.chkSP13.AutoSize = true;
            this.chkSP13.Checked = true;
            this.chkSP13.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP13.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP13.Location = new System.Drawing.Point(498, 52);
            this.chkSP13.Name = "chkSP13";
            this.chkSP13.Size = new System.Drawing.Size(38, 17);
            this.chkSP13.TabIndex = 54;
            this.chkSP13.Tag = "13";
            this.chkSP13.Text = "13";
            this.chkSP13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP13.UseVisualStyleBackColor = true;
            // 
            // chkSP12
            // 
            this.chkSP12.AutoSize = true;
            this.chkSP12.Checked = true;
            this.chkSP12.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP12.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP12.Location = new System.Drawing.Point(458, 52);
            this.chkSP12.Name = "chkSP12";
            this.chkSP12.Size = new System.Drawing.Size(38, 17);
            this.chkSP12.TabIndex = 53;
            this.chkSP12.Tag = "12";
            this.chkSP12.Text = "12";
            this.chkSP12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP12.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Location = new System.Drawing.Point(2, 111);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(735, 2);
            this.label11.TabIndex = 52;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(381, 117);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(119, 32);
            this.btnSelect.TabIndex = 51;
            this.btnSelect.Text = "SELECT ORDER";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnGenerateRolls
            // 
            this.btnGenerateRolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateRolls.Location = new System.Drawing.Point(561, 9);
            this.btnGenerateRolls.Name = "btnGenerateRolls";
            this.btnGenerateRolls.Size = new System.Drawing.Size(55, 24);
            this.btnGenerateRolls.TabIndex = 50;
            this.btnGenerateRolls.Text = ">>";
            this.btnGenerateRolls.UseVisualStyleBackColor = true;
            this.btnGenerateRolls.Click += new System.EventHandler(this.btnGenerateRolls_Click);
            // 
            // chkSP11
            // 
            this.chkSP11.AutoSize = true;
            this.chkSP11.Checked = true;
            this.chkSP11.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP11.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP11.Location = new System.Drawing.Point(418, 52);
            this.chkSP11.Name = "chkSP11";
            this.chkSP11.Size = new System.Drawing.Size(38, 17);
            this.chkSP11.TabIndex = 49;
            this.chkSP11.Tag = "11";
            this.chkSP11.Text = "11";
            this.chkSP11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP11.UseVisualStyleBackColor = true;
            // 
            // chkSP10
            // 
            this.chkSP10.AutoSize = true;
            this.chkSP10.Checked = true;
            this.chkSP10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP10.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP10.Location = new System.Drawing.Point(378, 52);
            this.chkSP10.Name = "chkSP10";
            this.chkSP10.Size = new System.Drawing.Size(38, 17);
            this.chkSP10.TabIndex = 48;
            this.chkSP10.Tag = "10";
            this.chkSP10.Text = "10";
            this.chkSP10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP10.UseVisualStyleBackColor = true;
            // 
            // chkSP9
            // 
            this.chkSP9.AutoSize = true;
            this.chkSP9.Checked = true;
            this.chkSP9.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP9.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP9.Location = new System.Drawing.Point(338, 52);
            this.chkSP9.Name = "chkSP9";
            this.chkSP9.Size = new System.Drawing.Size(32, 17);
            this.chkSP9.TabIndex = 47;
            this.chkSP9.Tag = "9";
            this.chkSP9.Text = "9";
            this.chkSP9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP9.UseVisualStyleBackColor = true;
            // 
            // chkSP8
            // 
            this.chkSP8.AutoSize = true;
            this.chkSP8.Checked = true;
            this.chkSP8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP8.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP8.Location = new System.Drawing.Point(298, 52);
            this.chkSP8.Name = "chkSP8";
            this.chkSP8.Size = new System.Drawing.Size(32, 17);
            this.chkSP8.TabIndex = 46;
            this.chkSP8.Tag = "8";
            this.chkSP8.Text = "8";
            this.chkSP8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP8.UseVisualStyleBackColor = true;
            // 
            // chkSP7
            // 
            this.chkSP7.AutoSize = true;
            this.chkSP7.Checked = true;
            this.chkSP7.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP7.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP7.Location = new System.Drawing.Point(258, 52);
            this.chkSP7.Name = "chkSP7";
            this.chkSP7.Size = new System.Drawing.Size(32, 17);
            this.chkSP7.TabIndex = 45;
            this.chkSP7.Tag = "7";
            this.chkSP7.Text = "7";
            this.chkSP7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP7.UseVisualStyleBackColor = true;
            // 
            // chkSP6
            // 
            this.chkSP6.AutoSize = true;
            this.chkSP6.Checked = true;
            this.chkSP6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP6.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP6.Location = new System.Drawing.Point(218, 52);
            this.chkSP6.Name = "chkSP6";
            this.chkSP6.Size = new System.Drawing.Size(32, 17);
            this.chkSP6.TabIndex = 44;
            this.chkSP6.Tag = "6";
            this.chkSP6.Text = "6";
            this.chkSP6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP6.UseVisualStyleBackColor = true;
            // 
            // chkSP5
            // 
            this.chkSP5.AutoSize = true;
            this.chkSP5.Checked = true;
            this.chkSP5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP5.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP5.Location = new System.Drawing.Point(178, 52);
            this.chkSP5.Name = "chkSP5";
            this.chkSP5.Size = new System.Drawing.Size(32, 17);
            this.chkSP5.TabIndex = 43;
            this.chkSP5.Tag = "5";
            this.chkSP5.Text = "5";
            this.chkSP5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP5.UseVisualStyleBackColor = true;
            // 
            // chkSP4
            // 
            this.chkSP4.AutoSize = true;
            this.chkSP4.Checked = true;
            this.chkSP4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP4.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP4.Location = new System.Drawing.Point(138, 52);
            this.chkSP4.Name = "chkSP4";
            this.chkSP4.Size = new System.Drawing.Size(32, 17);
            this.chkSP4.TabIndex = 42;
            this.chkSP4.Tag = "4";
            this.chkSP4.Text = "4";
            this.chkSP4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP4.UseVisualStyleBackColor = true;
            // 
            // chkSP3
            // 
            this.chkSP3.AutoSize = true;
            this.chkSP3.Checked = true;
            this.chkSP3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP3.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP3.Location = new System.Drawing.Point(98, 52);
            this.chkSP3.Name = "chkSP3";
            this.chkSP3.Size = new System.Drawing.Size(32, 17);
            this.chkSP3.TabIndex = 41;
            this.chkSP3.Tag = "3";
            this.chkSP3.Text = "3";
            this.chkSP3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP3.UseVisualStyleBackColor = true;
            // 
            // chkSP2
            // 
            this.chkSP2.AutoSize = true;
            this.chkSP2.Checked = true;
            this.chkSP2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP2.Location = new System.Drawing.Point(58, 52);
            this.chkSP2.Name = "chkSP2";
            this.chkSP2.Size = new System.Drawing.Size(32, 17);
            this.chkSP2.TabIndex = 40;
            this.chkSP2.Tag = "2";
            this.chkSP2.Text = "2";
            this.chkSP2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP2.UseVisualStyleBackColor = true;
            // 
            // chkSP1
            // 
            this.chkSP1.AutoSize = true;
            this.chkSP1.Checked = true;
            this.chkSP1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP1.Location = new System.Drawing.Point(18, 52);
            this.chkSP1.Name = "chkSP1";
            this.chkSP1.Size = new System.Drawing.Size(32, 17);
            this.chkSP1.TabIndex = 39;
            this.chkSP1.Tag = "1";
            this.chkSP1.Text = "1";
            this.chkSP1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP1.UseVisualStyleBackColor = true;
            // 
            // txtWeightKgs
            // 
            this.txtWeightKgs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWeightKgs.Location = new System.Drawing.Point(338, 5);
            this.txtWeightKgs.Name = "txtWeightKgs";
            this.txtWeightKgs.Size = new System.Drawing.Size(77, 26);
            this.txtWeightKgs.TabIndex = 10;
            this.txtWeightKgs.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(293, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Kgs";
            this.label4.Visible = false;
            // 
            // btnCreate
            // 
            this.btnCreate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.Location = new System.Drawing.Point(0, 517);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(293, 36);
            this.btnCreate.TabIndex = 12;
            this.btnCreate.Text = "Create/Print";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // olvRolls
            // 
            this.olvRolls.AllColumns.Add(this.olvColRollNo);
            this.olvRolls.AllColumns.Add(this.olvColScrap);
            this.olvRolls.AllColumns.Add(this.olvColKgs);
            this.olvRolls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvRolls.CellEditUseWholeCell = false;
            this.olvRolls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColRollNo});
            this.olvRolls.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvRolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvRolls.Location = new System.Drawing.Point(3, 3);
            this.olvRolls.Name = "olvRolls";
            this.olvRolls.Size = new System.Drawing.Size(287, 518);
            this.olvRolls.TabIndex = 11;
            this.olvRolls.UseCompatibleStateImageBehavior = false;
            this.olvRolls.View = System.Windows.Forms.View.Details;
            // 
            // olvColRollNo
            // 
            this.olvColRollNo.AspectName = "RollNo";
            this.olvColRollNo.Groupable = false;
            this.olvColRollNo.Text = "Roll No.";
            this.olvColRollNo.Width = 200;
            // 
            // olvColScrap
            // 
            this.olvColScrap.AspectName = "Scrap";
            this.olvColScrap.CheckBoxes = true;
            this.olvColScrap.DisplayIndex = 1;
            this.olvColScrap.IsVisible = false;
            this.olvColScrap.Text = "Scrap";
            // 
            // olvColKgs
            // 
            this.olvColKgs.AspectName = "Kgs";
            this.olvColKgs.DisplayIndex = 2;
            this.olvColKgs.IsVisible = false;
            this.olvColKgs.Text = "Kgs";
            this.olvColKgs.Width = 100;
            // 
            // chkSP18
            // 
            this.chkSP18.AutoSize = true;
            this.chkSP18.Checked = true;
            this.chkSP18.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP18.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP18.Location = new System.Drawing.Point(698, 52);
            this.chkSP18.Name = "chkSP18";
            this.chkSP18.Size = new System.Drawing.Size(38, 17);
            this.chkSP18.TabIndex = 92;
            this.chkSP18.Tag = "18";
            this.chkSP18.Text = "18";
            this.chkSP18.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP18.UseVisualStyleBackColor = true;
            // 
            // chkSP17
            // 
            this.chkSP17.AutoSize = true;
            this.chkSP17.Checked = true;
            this.chkSP17.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSP17.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSP17.Location = new System.Drawing.Point(660, 52);
            this.chkSP17.Name = "chkSP17";
            this.chkSP17.Size = new System.Drawing.Size(38, 17);
            this.chkSP17.TabIndex = 91;
            this.chkSP17.Tag = "17";
            this.chkSP17.Text = "17";
            this.chkSP17.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.chkSP17.UseVisualStyleBackColor = true;
            // 
            // FrmCreateRolls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 577);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmCreateRolls";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.FrmCreateRolls_Load);
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
        private System.Windows.Forms.CheckBox chkSP16;
        private System.Windows.Forms.CheckBox chkSP18;
        private System.Windows.Forms.CheckBox chkSP17;
    }
}

