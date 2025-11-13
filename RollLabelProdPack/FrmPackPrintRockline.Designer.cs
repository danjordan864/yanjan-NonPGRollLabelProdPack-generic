namespace RollLabelProdPack
{
    partial class FrmPackPrintRockline
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPackPrintRockline));
            this.olvBundles = new BrightIdeasSoftware.ObjectListView();
            this.olvColPackDesc = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvTotalWeight = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvTotalTareWeight = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvTotalNetWeight = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvCopies = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColPrint = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColAction = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColEmployee = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ilPackPrint = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtMatch = new System.Windows.Forms.TextBox();
            this.lblMatchText = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtOrder = new System.Windows.Forms.TextBox();
            this.lblOrder = new System.Windows.Forms.Label();
            this.chkReprint = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.olvBundles)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvBundles
            // 
            this.olvBundles.AllColumns.Add(this.olvColPackDesc);
            this.olvBundles.AllColumns.Add(this.olvTotalWeight);
            this.olvBundles.AllColumns.Add(this.olvTotalTareWeight);
            this.olvBundles.AllColumns.Add(this.olvTotalNetWeight);
            this.olvBundles.AllColumns.Add(this.olvCopies);
            this.olvBundles.AllColumns.Add(this.olvColPrint);
            this.olvBundles.AllColumns.Add(this.olvColAction);
            this.olvBundles.AllColumns.Add(this.olvColEmployee);
            this.olvBundles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvBundles.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvBundles.CellEditUseWholeCell = false;
            this.olvBundles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColPackDesc,
            this.olvTotalWeight,
            this.olvTotalTareWeight,
            this.olvTotalNetWeight,
            this.olvCopies,
            this.olvColPrint,
            this.olvColAction});
            this.olvBundles.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvBundles.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvBundles.Location = new System.Drawing.Point(0, 36);
            this.olvBundles.Name = "olvBundles";
            this.olvBundles.Size = new System.Drawing.Size(718, 527);
            this.olvBundles.SmallImageList = this.ilPackPrint;
            this.olvBundles.TabIndex = 0;
            this.olvBundles.UseCompatibleStateImageBehavior = false;
            this.olvBundles.UseHyperlinks = true;
            this.olvBundles.View = System.Windows.Forms.View.Details;
            this.olvBundles.HyperlinkClicked += new System.EventHandler<BrightIdeasSoftware.HyperlinkClickedEventArgs>(this.olvBundles_HyperlinkClicked);
            // 
            // olvColPackDesc
            // 
            this.olvColPackDesc.AspectName = "Description";
            this.olvColPackDesc.Groupable = false;
            this.olvColPackDesc.IsEditable = false;
            this.olvColPackDesc.Text = "Pack Label";
            this.olvColPackDesc.Width = 324;
            // 
            // olvTotalWeight
            // 
            this.olvTotalWeight.AspectName = "TotalWeight";
            this.olvTotalWeight.AspectToStringFormat = "{0:0.0}";
            this.olvTotalWeight.Text = "Total Wt";
            this.olvTotalWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvTotalWeight.Width = 79;
            // 
            // olvTotalTareWeight
            // 
            this.olvTotalTareWeight.AspectName = "TotalTareKg";
            this.olvTotalTareWeight.AspectToStringFormat = "{0:0.0}";
            this.olvTotalTareWeight.IsEditable = false;
            this.olvTotalTareWeight.Text = "Tare Wt";
            this.olvTotalTareWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvTotalTareWeight.Width = 69;
            // 
            // olvTotalNetWeight
            // 
            this.olvTotalNetWeight.AspectName = "TotalNetKg";
            this.olvTotalNetWeight.AspectToStringFormat = "{0:0.0}";
            this.olvTotalNetWeight.IsEditable = false;
            this.olvTotalNetWeight.Text = "Net Wt";
            this.olvTotalNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olvCopies
            // 
            this.olvCopies.AspectName = "Copies";
            this.olvCopies.Text = "Copies";
            this.olvCopies.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // olvColPrint
            // 
            this.olvColPrint.AspectName = "PrintButtonText";
            this.olvColPrint.IsEditable = false;
            this.olvColPrint.Text = "Print";
            // 
            // olvColAction
            // 
            this.olvColAction.AspectName = "";
            this.olvColAction.Hyperlink = true;
            this.olvColAction.IsEditable = false;
            this.olvColAction.Text = "Action";
            this.olvColAction.Width = 100;
            // 
            // olvColEmployee
            // 
            this.olvColEmployee.AspectName = "Employee";
            this.olvColEmployee.DisplayIndex = 7;
            this.olvColEmployee.IsVisible = false;
            this.olvColEmployee.Text = "Initials";
            // 
            // ilPackPrint
            // 
            this.ilPackPrint.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilPackPrint.ImageStream")));
            this.ilPackPrint.TransparentColor = System.Drawing.Color.Transparent;
            this.ilPackPrint.Images.SetKeyName(0, "False");
            this.ilPackPrint.Images.SetKeyName(1, "True");
            this.ilPackPrint.Images.SetKeyName(2, "Print");
            // 
            // timer1
            // 
            this.timer1.Interval = 15000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.txtMatch);
            this.pnlTop.Controls.Add(this.lblMatchText);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.txtOrder);
            this.pnlTop.Controls.Add(this.lblOrder);
            this.pnlTop.Controls.Add(this.chkReprint);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(718, 40);
            this.pnlTop.TabIndex = 1;
            // 
            // txtMatch
            // 
            this.txtMatch.Location = new System.Drawing.Point(435, 12);
            this.txtMatch.Name = "txtMatch";
            this.txtMatch.Size = new System.Drawing.Size(100, 20);
            this.txtMatch.TabIndex = 5;
            this.txtMatch.Visible = false;
            this.txtMatch.TextChanged += new System.EventHandler(this.txtMatch_TextChanged);
            // 
            // lblMatchText
            // 
            this.lblMatchText.AutoSize = true;
            this.lblMatchText.Location = new System.Drawing.Point(368, 13);
            this.lblMatchText.Name = "lblMatchText";
            this.lblMatchText.Size = new System.Drawing.Size(61, 13);
            this.lblMatchText.TabIndex = 4;
            this.lblMatchText.Text = "Match Text";
            this.lblMatchText.Visible = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(268, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtOrder
            // 
            this.txtOrder.Location = new System.Drawing.Point(149, 10);
            this.txtOrder.Name = "txtOrder";
            this.txtOrder.Size = new System.Drawing.Size(100, 20);
            this.txtOrder.TabIndex = 2;
            this.txtOrder.Visible = false;
            // 
            // lblOrder
            // 
            this.lblOrder.AutoSize = true;
            this.lblOrder.Location = new System.Drawing.Point(110, 13);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(33, 13);
            this.lblOrder.TabIndex = 1;
            this.lblOrder.Text = "Order";
            this.lblOrder.Visible = false;
            // 
            // chkReprint
            // 
            this.chkReprint.AutoSize = true;
            this.chkReprint.Location = new System.Drawing.Point(12, 12);
            this.chkReprint.Name = "chkReprint";
            this.chkReprint.Size = new System.Drawing.Size(60, 17);
            this.chkReprint.TabIndex = 0;
            this.chkReprint.Text = "Reprint";
            this.chkReprint.UseVisualStyleBackColor = true;
            this.chkReprint.CheckedChanged += new System.EventHandler(this.chkReprint_CheckedChanged);
            // 
            // FrmPackPrintRockline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 563);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.olvBundles);
            this.Name = "FrmPackPrintRockline";
            this.Text = "Rockline Pack Print";
            this.Load += new System.EventHandler(this.FrmPackPrint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvBundles)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvBundles;
        private BrightIdeasSoftware.OLVColumn olvColPackDesc;
        private BrightIdeasSoftware.OLVColumn olvColPrint;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ImageList ilPackPrint;
        private BrightIdeasSoftware.OLVColumn olvColAction;
        private BrightIdeasSoftware.OLVColumn olvCopies;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtOrder;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.CheckBox chkReprint;
        private System.Windows.Forms.TextBox txtMatch;
        private System.Windows.Forms.Label lblMatchText;
        private BrightIdeasSoftware.OLVColumn olvColEmployee;
        private BrightIdeasSoftware.OLVColumn olvTotalWeight;
        private BrightIdeasSoftware.OLVColumn olvTotalTareWeight;
        private BrightIdeasSoftware.OLVColumn olvTotalNetWeight;
    }
}