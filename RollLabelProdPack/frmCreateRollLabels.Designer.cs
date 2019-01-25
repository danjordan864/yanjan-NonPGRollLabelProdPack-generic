namespace RollLabelProdPack
{
    partial class FrmCreateRollLabels
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
            this.components = new System.ComponentModel.Container();
            this.lblFactoryCode = new System.Windows.Forms.Label();
            this.lblMachineNo = new System.Windows.Forms.Label();
            this.lblMatlCode = new System.Windows.Forms.Label();
            this.lblPrdName = new System.Windows.Forms.Label();
            this.lblPrdYr = new System.Windows.Forms.Label();
            this.lblPrdMo = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFactoryCode = new System.Windows.Forms.TextBox();
            this.txtPrdName = new System.Windows.Forms.TextBox();
            this.txtPrdYr = new System.Windows.Forms.TextBox();
            this.txtPrdMo = new System.Windows.Forms.TextBox();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.lblShippingLotNo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtItem = new System.Windows.Forms.TextBox();
            this.txtIRMS = new System.Windows.Forms.TextBox();
            this.txtRnShift = new System.Windows.Forms.TextBox();
            this.txtRnDie = new System.Windows.Forms.TextBox();
            this.txtRnPrdDate = new System.Windows.Forms.TextBox();
            this.nudJumboRollNo = new System.Windows.Forms.NumericUpDown();
            this.txtRnPrdYr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRnPrdMo = new System.Windows.Forms.TextBox();
            this.nudSlitPositions = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnPrintRollLabels = new System.Windows.Forms.Button();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnPrintPackLabel = new System.Windows.Forms.Button();
            this.nudMachNo = new System.Windows.Forms.NumericUpDown();
            this.txtMtlCode = new System.Windows.Forms.TextBox();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.nudCopies1x6 = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.nud4x6Copies = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.chkReprint = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtProductionDateFull = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDefaultItemDesc = new System.Windows.Forms.TextBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.nudPalletNo = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.nudCopiesPack = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tstbResults = new System.Windows.Forms.ToolStripTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtProdOrder = new System.Windows.Forms.TextBox();
            this.errorProviderProdOrder = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProviderQty = new System.Windows.Forms.ErrorProvider(this.components);
            this.olvRowLabels = new BrightIdeasSoftware.ObjectListView();
            this.olvColProdYr = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColProdMo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColProdDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColApertureDie = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColProdShift = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColJumboRollNo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColSlitPos = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColFactoryCode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColMachNo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColMtlCode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColProdName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColBatchNo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColItem = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColIRMS = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.nudJumboRollNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlitPositions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMachNo)).BeginInit();
            this.pnlBody.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCopies1x6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud4x6Copies)).BeginInit();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPalletNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCopiesPack)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderProdOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.olvRowLabels)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFactoryCode
            // 
            this.lblFactoryCode.AutoSize = true;
            this.lblFactoryCode.Location = new System.Drawing.Point(17, 88);
            this.lblFactoryCode.Name = "lblFactoryCode";
            this.lblFactoryCode.Size = new System.Drawing.Size(70, 13);
            this.lblFactoryCode.TabIndex = 0;
            this.lblFactoryCode.Text = "Factory Code";
            // 
            // lblMachineNo
            // 
            this.lblMachineNo.AutoSize = true;
            this.lblMachineNo.Location = new System.Drawing.Point(103, 88);
            this.lblMachineNo.Name = "lblMachineNo";
            this.lblMachineNo.Size = new System.Drawing.Size(68, 13);
            this.lblMachineNo.TabIndex = 1;
            this.lblMachineNo.Text = "Machine No.";
            // 
            // lblMatlCode
            // 
            this.lblMatlCode.AutoSize = true;
            this.lblMatlCode.Location = new System.Drawing.Point(187, 88);
            this.lblMatlCode.Name = "lblMatlCode";
            this.lblMatlCode.Size = new System.Drawing.Size(52, 13);
            this.lblMatlCode.TabIndex = 2;
            this.lblMatlCode.Text = "Mtl. Code";
            // 
            // lblPrdName
            // 
            this.lblPrdName.AutoSize = true;
            this.lblPrdName.Location = new System.Drawing.Point(255, 88);
            this.lblPrdName.Name = "lblPrdName";
            this.lblPrdName.Size = new System.Drawing.Size(57, 13);
            this.lblPrdName.TabIndex = 3;
            this.lblPrdName.Text = "Prd. Name";
            // 
            // lblPrdYr
            // 
            this.lblPrdYr.AutoSize = true;
            this.lblPrdYr.Location = new System.Drawing.Point(328, 88);
            this.lblPrdYr.Name = "lblPrdYr";
            this.lblPrdYr.Size = new System.Drawing.Size(42, 13);
            this.lblPrdYr.TabIndex = 4;
            this.lblPrdYr.Text = "Prd. Yr.";
            // 
            // lblPrdMo
            // 
            this.lblPrdMo.AutoSize = true;
            this.lblPrdMo.Location = new System.Drawing.Point(386, 88);
            this.lblPrdMo.Name = "lblPrdMo";
            this.lblPrdMo.Size = new System.Drawing.Size(47, 13);
            this.lblPrdMo.TabIndex = 5;
            this.lblPrdMo.Text = "Prd. Mo.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(449, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Batch No.";
            // 
            // txtFactoryCode
            // 
            this.txtFactoryCode.Location = new System.Drawing.Point(20, 104);
            this.txtFactoryCode.Name = "txtFactoryCode";
            this.txtFactoryCode.Size = new System.Drawing.Size(67, 20);
            this.txtFactoryCode.TabIndex = 7;
            // 
            // txtPrdName
            // 
            this.txtPrdName.Location = new System.Drawing.Point(258, 104);
            this.txtPrdName.Name = "txtPrdName";
            this.txtPrdName.Size = new System.Drawing.Size(54, 20);
            this.txtPrdName.TabIndex = 10;
            // 
            // txtPrdYr
            // 
            this.txtPrdYr.Location = new System.Drawing.Point(333, 105);
            this.txtPrdYr.Name = "txtPrdYr";
            this.txtPrdYr.Size = new System.Drawing.Size(37, 20);
            this.txtPrdYr.TabIndex = 11;
            // 
            // txtPrdMo
            // 
            this.txtPrdMo.Location = new System.Drawing.Point(389, 105);
            this.txtPrdMo.Name = "txtPrdMo";
            this.txtPrdMo.Size = new System.Drawing.Size(44, 20);
            this.txtPrdMo.TabIndex = 12;
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Location = new System.Drawing.Point(452, 104);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(52, 20);
            this.txtBatchNo.TabIndex = 13;
            this.txtBatchNo.TextChanged += new System.EventHandler(this.txtBatchNo_TextChanged);
            // 
            // lblShippingLotNo
            // 
            this.lblShippingLotNo.AutoSize = true;
            this.lblShippingLotNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShippingLotNo.Location = new System.Drawing.Point(17, 66);
            this.lblShippingLotNo.Name = "lblShippingLotNo";
            this.lblShippingLotNo.Size = new System.Drawing.Size(129, 17);
            this.lblShippingLotNo.TabIndex = 14;
            this.lblShippingLotNo.Text = "Shipping Lot No.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 29;
            this.label1.Text = "Roll No.\'s";
            // 
            // txtItem
            // 
            this.txtItem.Location = new System.Drawing.Point(20, 34);
            this.txtItem.Name = "txtItem";
            this.txtItem.Size = new System.Drawing.Size(52, 20);
            this.txtItem.TabIndex = 28;
            // 
            // txtIRMS
            // 
            this.txtIRMS.Location = new System.Drawing.Point(275, 34);
            this.txtIRMS.Name = "txtIRMS";
            this.txtIRMS.Size = new System.Drawing.Size(68, 20);
            this.txtIRMS.TabIndex = 27;
            // 
            // txtRnShift
            // 
            this.txtRnShift.Location = new System.Drawing.Point(245, 182);
            this.txtRnShift.Name = "txtRnShift";
            this.txtRnShift.Size = new System.Drawing.Size(37, 20);
            this.txtRnShift.TabIndex = 26;
            // 
            // txtRnDie
            // 
            this.txtRnDie.Location = new System.Drawing.Point(202, 181);
            this.txtRnDie.Name = "txtRnDie";
            this.txtRnDie.Size = new System.Drawing.Size(37, 20);
            this.txtRnDie.TabIndex = 25;
            // 
            // txtRnPrdDate
            // 
            this.txtRnPrdDate.Location = new System.Drawing.Point(147, 182);
            this.txtRnPrdDate.Name = "txtRnPrdDate";
            this.txtRnPrdDate.Size = new System.Drawing.Size(49, 20);
            this.txtRnPrdDate.TabIndex = 24;
            // 
            // nudJumboRollNo
            // 
            this.nudJumboRollNo.Enabled = false;
            this.nudJumboRollNo.Location = new System.Drawing.Point(290, 183);
            this.nudJumboRollNo.Name = "nudJumboRollNo";
            this.nudJumboRollNo.ReadOnly = true;
            this.nudJumboRollNo.Size = new System.Drawing.Size(76, 20);
            this.nudJumboRollNo.TabIndex = 23;
            this.nudJumboRollNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtRnPrdYr
            // 
            this.txtRnPrdYr.Location = new System.Drawing.Point(20, 181);
            this.txtRnPrdYr.Name = "txtRnPrdYr";
            this.txtRnPrdYr.Size = new System.Drawing.Size(67, 20);
            this.txtRnPrdYr.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(372, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Slit Pos.\'s";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(287, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Jumbo Roll No.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(242, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Shift.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(202, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Die";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(144, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Prd. Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(90, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Prd Mo.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 165);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Prd. Yr.";
            // 
            // txtRnPrdMo
            // 
            this.txtRnPrdMo.Location = new System.Drawing.Point(93, 182);
            this.txtRnPrdMo.Name = "txtRnPrdMo";
            this.txtRnPrdMo.Size = new System.Drawing.Size(49, 20);
            this.txtRnPrdMo.TabIndex = 30;
            // 
            // nudSlitPositions
            // 
            this.nudSlitPositions.Location = new System.Drawing.Point(372, 183);
            this.nudSlitPositions.Name = "nudSlitPositions";
            this.nudSlitPositions.Size = new System.Drawing.Size(65, 20);
            this.nudSlitPositions.TabIndex = 31;
            this.nudSlitPositions.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "Item";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(276, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "IRMS";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(15, -1);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 17);
            this.label13.TabIndex = 36;
            this.label13.Text = "General Info";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(106, 137);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(133, 23);
            this.btnGenerate.TabIndex = 38;
            this.btnGenerate.Text = "Generate Roll No\'s";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnPrintRollLabels
            // 
            this.btnPrintRollLabels.Location = new System.Drawing.Point(258, 137);
            this.btnPrintRollLabels.Name = "btnPrintRollLabels";
            this.btnPrintRollLabels.Size = new System.Drawing.Size(133, 23);
            this.btnPrintRollLabels.TabIndex = 39;
            this.btnPrintRollLabels.Text = "Print Roll Labels";
            this.btnPrintRollLabels.UseVisualStyleBackColor = true;
            this.btnPrintRollLabels.Click += new System.EventHandler(this.btnPrintRollLabels_Click);
            // 
            // txtQty
            // 
            this.txtQty.Location = new System.Drawing.Point(202, 28);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(67, 20);
            this.txtQty.TabIndex = 41;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(199, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 13);
            this.label14.TabIndex = 40;
            this.label14.Text = "Quantity (Kg)";
            // 
            // btnPrintPackLabel
            // 
            this.btnPrintPackLabel.Location = new System.Drawing.Point(336, 25);
            this.btnPrintPackLabel.Name = "btnPrintPackLabel";
            this.btnPrintPackLabel.Size = new System.Drawing.Size(133, 23);
            this.btnPrintPackLabel.TabIndex = 42;
            this.btnPrintPackLabel.Text = "Print Pack Label";
            this.btnPrintPackLabel.UseVisualStyleBackColor = true;
            this.btnPrintPackLabel.Click += new System.EventHandler(this.btnPrintPackLabel_Click);
            // 
            // nudMachNo
            // 
            this.nudMachNo.Location = new System.Drawing.Point(106, 105);
            this.nudMachNo.Name = "nudMachNo";
            this.nudMachNo.Size = new System.Drawing.Size(65, 20);
            this.nudMachNo.TabIndex = 8;
            this.nudMachNo.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // txtMtlCode
            // 
            this.txtMtlCode.Location = new System.Drawing.Point(190, 105);
            this.txtMtlCode.Name = "txtMtlCode";
            this.txtMtlCode.Size = new System.Drawing.Size(54, 20);
            this.txtMtlCode.TabIndex = 43;
            // 
            // pnlBody
            // 
            this.pnlBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBody.Controls.Add(this.olvRowLabels);
            this.pnlBody.Location = new System.Drawing.Point(0, 226);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(934, 227);
            this.pnlBody.TabIndex = 45;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.nudCopies1x6);
            this.pnlHeader.Controls.Add(this.label19);
            this.pnlHeader.Controls.Add(this.nud4x6Copies);
            this.pnlHeader.Controls.Add(this.label18);
            this.pnlHeader.Controls.Add(this.label17);
            this.pnlHeader.Controls.Add(this.chkReprint);
            this.pnlHeader.Controls.Add(this.label16);
            this.pnlHeader.Controls.Add(this.txtProductionDateFull);
            this.pnlHeader.Controls.Add(this.label15);
            this.pnlHeader.Controls.Add(this.txtDefaultItemDesc);
            this.pnlHeader.Controls.Add(this.label13);
            this.pnlHeader.Controls.Add(this.lblFactoryCode);
            this.pnlHeader.Controls.Add(this.lblMachineNo);
            this.pnlHeader.Controls.Add(this.txtMtlCode);
            this.pnlHeader.Controls.Add(this.lblMatlCode);
            this.pnlHeader.Controls.Add(this.lblPrdName);
            this.pnlHeader.Controls.Add(this.lblPrdYr);
            this.pnlHeader.Controls.Add(this.lblPrdMo);
            this.pnlHeader.Controls.Add(this.btnPrintRollLabels);
            this.pnlHeader.Controls.Add(this.label7);
            this.pnlHeader.Controls.Add(this.btnGenerate);
            this.pnlHeader.Controls.Add(this.txtFactoryCode);
            this.pnlHeader.Controls.Add(this.nudMachNo);
            this.pnlHeader.Controls.Add(this.label11);
            this.pnlHeader.Controls.Add(this.txtPrdName);
            this.pnlHeader.Controls.Add(this.label10);
            this.pnlHeader.Controls.Add(this.txtPrdYr);
            this.pnlHeader.Controls.Add(this.nudSlitPositions);
            this.pnlHeader.Controls.Add(this.txtPrdMo);
            this.pnlHeader.Controls.Add(this.txtRnPrdMo);
            this.pnlHeader.Controls.Add(this.txtBatchNo);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Controls.Add(this.lblShippingLotNo);
            this.pnlHeader.Controls.Add(this.txtItem);
            this.pnlHeader.Controls.Add(this.label9);
            this.pnlHeader.Controls.Add(this.txtIRMS);
            this.pnlHeader.Controls.Add(this.label8);
            this.pnlHeader.Controls.Add(this.txtRnShift);
            this.pnlHeader.Controls.Add(this.label6);
            this.pnlHeader.Controls.Add(this.txtRnDie);
            this.pnlHeader.Controls.Add(this.label5);
            this.pnlHeader.Controls.Add(this.txtRnPrdDate);
            this.pnlHeader.Controls.Add(this.label4);
            this.pnlHeader.Controls.Add(this.nudJumboRollNo);
            this.pnlHeader.Controls.Add(this.label3);
            this.pnlHeader.Controls.Add(this.txtRnPrdYr);
            this.pnlHeader.Controls.Add(this.label2);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(934, 220);
            this.pnlHeader.TabIndex = 46;
            // 
            // nudCopies1x6
            // 
            this.nudCopies1x6.Location = new System.Drawing.Point(691, 137);
            this.nudCopies1x6.Name = "nudCopies1x6";
            this.nudCopies1x6.Size = new System.Drawing.Size(47, 20);
            this.nudCopies1x6.TabIndex = 53;
            this.nudCopies1x6.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(620, 141);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(65, 13);
            this.label19.TabIndex = 52;
            this.label19.Text = "Copies (1x6)";
            // 
            // nud4x6Copies
            // 
            this.nud4x6Copies.Location = new System.Drawing.Point(554, 137);
            this.nud4x6Copies.Name = "nud4x6Copies";
            this.nud4x6Copies.Size = new System.Drawing.Size(47, 20);
            this.nud4x6Copies.TabIndex = 51;
            this.nud4x6Copies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(483, 141);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 13);
            this.label18.TabIndex = 50;
            this.label18.Text = "Copies (4x6)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(425, 141);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(41, 13);
            this.label17.TabIndex = 49;
            this.label17.Text = "Reprint";
            // 
            // chkReprint
            // 
            this.chkReprint.AutoSize = true;
            this.chkReprint.Location = new System.Drawing.Point(409, 141);
            this.chkReprint.Name = "chkReprint";
            this.chkReprint.Size = new System.Drawing.Size(15, 14);
            this.chkReprint.TabIndex = 48;
            this.chkReprint.UseVisualStyleBackColor = true;
            this.chkReprint.CheckedChanged += new System.EventHandler(this.chkReprint_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(343, 18);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(84, 13);
            this.label16.TabIndex = 47;
            this.label16.Text = "Production Date";
            // 
            // txtProductionDateFull
            // 
            this.txtProductionDateFull.Location = new System.Drawing.Point(346, 34);
            this.txtProductionDateFull.Name = "txtProductionDateFull";
            this.txtProductionDateFull.Size = new System.Drawing.Size(78, 20);
            this.txtProductionDateFull.TabIndex = 46;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(78, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 13);
            this.label15.TabIndex = 45;
            this.label15.Text = "Item Desc.";
            // 
            // txtDefaultItemDesc
            // 
            this.txtDefaultItemDesc.Location = new System.Drawing.Point(79, 34);
            this.txtDefaultItemDesc.Name = "txtDefaultItemDesc";
            this.txtDefaultItemDesc.Size = new System.Drawing.Size(191, 20);
            this.txtDefaultItemDesc.TabIndex = 44;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.nudPalletNo);
            this.pnlFooter.Controls.Add(this.label21);
            this.pnlFooter.Controls.Add(this.nudCopiesPack);
            this.pnlFooter.Controls.Add(this.label20);
            this.pnlFooter.Controls.Add(this.toolStrip1);
            this.pnlFooter.Controls.Add(this.label12);
            this.pnlFooter.Controls.Add(this.txtProdOrder);
            this.pnlFooter.Controls.Add(this.btnPrintPackLabel);
            this.pnlFooter.Controls.Add(this.label14);
            this.pnlFooter.Controls.Add(this.txtQty);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 450);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(934, 150);
            this.pnlFooter.TabIndex = 47;
            // 
            // nudPalletNo
            // 
            this.nudPalletNo.Enabled = false;
            this.nudPalletNo.Location = new System.Drawing.Point(86, 28);
            this.nudPalletNo.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudPalletNo.Name = "nudPalletNo";
            this.nudPalletNo.Size = new System.Drawing.Size(110, 20);
            this.nudPalletNo.TabIndex = 55;
            this.nudPalletNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(83, 12);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(53, 13);
            this.label21.TabIndex = 54;
            this.label21.Text = "Pallet No.";
            // 
            // nudCopiesPack
            // 
            this.nudCopiesPack.Location = new System.Drawing.Point(278, 28);
            this.nudCopiesPack.Name = "nudCopiesPack";
            this.nudCopiesPack.Size = new System.Drawing.Size(47, 20);
            this.nudCopiesPack.TabIndex = 53;
            this.nudCopiesPack.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(275, 12);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(39, 13);
            this.label20.TabIndex = 52;
            this.label20.Text = "Copies";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstbResults});
            this.toolStrip1.Location = new System.Drawing.Point(0, 125);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(934, 25);
            this.toolStrip1.TabIndex = 45;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tstbResults
            // 
            this.tstbResults.Name = "tstbResults";
            this.tstbResults.Size = new System.Drawing.Size(900, 25);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 12);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 13);
            this.label12.TabIndex = 43;
            this.label12.Text = "Prod. Order";
            // 
            // txtProdOrder
            // 
            this.txtProdOrder.Location = new System.Drawing.Point(15, 28);
            this.txtProdOrder.Name = "txtProdOrder";
            this.txtProdOrder.Size = new System.Drawing.Size(67, 20);
            this.txtProdOrder.TabIndex = 44;
            // 
            // errorProviderProdOrder
            // 
            this.errorProviderProdOrder.ContainerControl = this;
            // 
            // errorProviderQty
            // 
            this.errorProviderQty.ContainerControl = this;
            // 
            // olvRowLabels
            // 
            this.olvRowLabels.AllColumns.Add(this.olvColProdYr);
            this.olvRowLabels.AllColumns.Add(this.olvColProdMo);
            this.olvRowLabels.AllColumns.Add(this.olvColProdDate);
            this.olvRowLabels.AllColumns.Add(this.olvColApertureDie);
            this.olvRowLabels.AllColumns.Add(this.olvColProdShift);
            this.olvRowLabels.AllColumns.Add(this.olvColJumboRollNo);
            this.olvRowLabels.AllColumns.Add(this.olvColSlitPos);
            this.olvRowLabels.AllColumns.Add(this.olvColFactoryCode);
            this.olvRowLabels.AllColumns.Add(this.olvColMachNo);
            this.olvRowLabels.AllColumns.Add(this.olvColMtlCode);
            this.olvRowLabels.AllColumns.Add(this.olvColProdName);
            this.olvRowLabels.AllColumns.Add(this.olvColBatchNo);
            this.olvRowLabels.AllColumns.Add(this.olvColItem);
            this.olvRowLabels.AllColumns.Add(this.olvColIRMS);
            this.olvRowLabels.CellEditUseWholeCell = false;
            this.olvRowLabels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColProdYr,
            this.olvColProdMo,
            this.olvColProdDate,
            this.olvColApertureDie,
            this.olvColProdShift,
            this.olvColJumboRollNo,
            this.olvColSlitPos});
            this.olvRowLabels.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvRowLabels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvRowLabels.Location = new System.Drawing.Point(0, 0);
            this.olvRowLabels.Name = "olvRowLabels";
            this.olvRowLabels.ShowGroups = false;
            this.olvRowLabels.Size = new System.Drawing.Size(934, 227);
            this.olvRowLabels.TabIndex = 44;
            this.olvRowLabels.UseCompatibleStateImageBehavior = false;
            this.olvRowLabels.View = System.Windows.Forms.View.Details;
            // 
            // olvColProdYr
            // 
            this.olvColProdYr.AspectName = "ProductionYear";
            this.olvColProdYr.Text = "Prd. Yr.";
            // 
            // olvColProdMo
            // 
            this.olvColProdMo.AspectName = "ProductionMonth";
            this.olvColProdMo.Text = "Prd. Mo.";
            // 
            // olvColProdDate
            // 
            this.olvColProdDate.AspectName = "ProductionDate";
            this.olvColProdDate.Text = "Prd. Dt";
            // 
            // olvColApertureDie
            // 
            this.olvColApertureDie.AspectName = "AperatureDieNo";
            this.olvColApertureDie.Text = "Die";
            // 
            // olvColProdShift
            // 
            this.olvColProdShift.AspectName = "ProductionShift";
            this.olvColProdShift.Text = "Shift";
            // 
            // olvColJumboRollNo
            // 
            this.olvColJumboRollNo.AspectName = "JumboRollNo";
            this.olvColJumboRollNo.Text = "Roll No.";
            // 
            // olvColSlitPos
            // 
            this.olvColSlitPos.AspectName = "SlitPosition";
            this.olvColSlitPos.Text = "Slit Pos.";
            // 
            // olvColFactoryCode
            // 
            this.olvColFactoryCode.AspectName = "FactoryCode";
            this.olvColFactoryCode.DisplayIndex = 7;
            this.olvColFactoryCode.IsVisible = false;
            this.olvColFactoryCode.Text = "Fact. Code";
            // 
            // olvColMachNo
            // 
            this.olvColMachNo.AspectName = "ProductionMachineNo";
            this.olvColMachNo.DisplayIndex = 8;
            this.olvColMachNo.IsVisible = false;
            this.olvColMachNo.Text = "Mach. No.";
            this.olvColMachNo.Width = 80;
            // 
            // olvColMtlCode
            // 
            this.olvColMtlCode.AspectName = "MaterialCode";
            this.olvColMtlCode.DisplayIndex = 9;
            this.olvColMtlCode.IsVisible = false;
            this.olvColMtlCode.Text = "Mtl. Code";
            // 
            // olvColProdName
            // 
            this.olvColProdName.AspectName = "ProductName";
            this.olvColProdName.DisplayIndex = 10;
            this.olvColProdName.IsVisible = false;
            this.olvColProdName.Text = "Prod. Name";
            // 
            // olvColBatchNo
            // 
            this.olvColBatchNo.AspectName = "BatchNo";
            this.olvColBatchNo.DisplayIndex = 11;
            this.olvColBatchNo.IsVisible = false;
            this.olvColBatchNo.Text = "Batch No.";
            // 
            // olvColItem
            // 
            this.olvColItem.AspectName = "ItemCode";
            this.olvColItem.DisplayIndex = 12;
            this.olvColItem.IsVisible = false;
            this.olvColItem.Text = "Item";
            // 
            // olvColIRMS
            // 
            this.olvColIRMS.AspectName = "IRMS";
            this.olvColIRMS.DisplayIndex = 13;
            this.olvColIRMS.IsVisible = false;
            this.olvColIRMS.Text = "IRMS#";
            // 
            // FrmCreateRollLabels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 600);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlBody);
            this.Name = "FrmCreateRollLabels";
            this.Text = "Create Roll Labels";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCreateRollLabels_FormClosing);
            this.Load += new System.EventHandler(this.FrmCreateRollLabels_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudJumboRollNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlitPositions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMachNo)).EndInit();
            this.pnlBody.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCopies1x6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud4x6Copies)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPalletNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCopiesPack)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderProdOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.olvRowLabels)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblFactoryCode;
        private System.Windows.Forms.Label lblMachineNo;
        private System.Windows.Forms.Label lblMatlCode;
        private System.Windows.Forms.Label lblPrdName;
        private System.Windows.Forms.Label lblPrdYr;
        private System.Windows.Forms.Label lblPrdMo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtFactoryCode;
        private System.Windows.Forms.TextBox txtPrdName;
        private System.Windows.Forms.TextBox txtPrdYr;
        private System.Windows.Forms.TextBox txtPrdMo;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.Label lblShippingLotNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtItem;
        private System.Windows.Forms.TextBox txtIRMS;
        private System.Windows.Forms.TextBox txtRnShift;
        private System.Windows.Forms.TextBox txtRnDie;
        private System.Windows.Forms.TextBox txtRnPrdDate;
        private System.Windows.Forms.NumericUpDown nudJumboRollNo;
        private System.Windows.Forms.TextBox txtRnPrdYr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRnPrdMo;
        private System.Windows.Forms.NumericUpDown nudSlitPositions;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnPrintRollLabels;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnPrintPackLabel;
        private System.Windows.Forms.NumericUpDown nudMachNo;
        private System.Windows.Forms.TextBox txtMtlCode;
        private BrightIdeasSoftware.ObjectListView olvRowLabels;
        private BrightIdeasSoftware.OLVColumn olvColProdYr;
        private BrightIdeasSoftware.OLVColumn olvColProdMo;
        private BrightIdeasSoftware.OLVColumn olvColProdDate;
        private BrightIdeasSoftware.OLVColumn olvColApertureDie;
        private BrightIdeasSoftware.OLVColumn olvColProdShift;
        private BrightIdeasSoftware.OLVColumn olvColJumboRollNo;
        private BrightIdeasSoftware.OLVColumn olvColSlitPos;
        private BrightIdeasSoftware.OLVColumn olvColIRMS;
        private BrightIdeasSoftware.OLVColumn olvColFactoryCode;
        private BrightIdeasSoftware.OLVColumn olvColMachNo;
        private BrightIdeasSoftware.OLVColumn olvColMtlCode;
        private BrightIdeasSoftware.OLVColumn olvColBatchNo;
        private BrightIdeasSoftware.OLVColumn olvColItem;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtProdOrder;
        private BrightIdeasSoftware.OLVColumn olvColProdName;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox tstbResults;
        private System.Windows.Forms.ErrorProvider errorProviderProdOrder;
        private System.Windows.Forms.ErrorProvider errorProviderQty;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtDefaultItemDesc;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtProductionDateFull;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox chkReprint;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown nudCopies1x6;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown nud4x6Copies;
        private System.Windows.Forms.NumericUpDown nudCopiesPack;
        private System.Windows.Forms.NumericUpDown nudPalletNo;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
    }
}