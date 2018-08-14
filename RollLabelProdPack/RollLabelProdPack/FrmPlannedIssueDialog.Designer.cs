namespace RollLabelProdPack
{
    partial class FrmPlannedIssueDialog
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
            this.olvPlannedIssues = new BrightIdeasSoftware.ObjectListView();
            this.olvColItem = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColStorageLoc = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColBatch = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColReqQty = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColPlannedIssueQty = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColShort = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.olvPlannedIssues)).BeginInit();
            this.SuspendLayout();
            // 
            // olvPlannedIssues
            // 
            this.olvPlannedIssues.AllColumns.Add(this.olvColItem);
            this.olvPlannedIssues.AllColumns.Add(this.olvColStorageLoc);
            this.olvPlannedIssues.AllColumns.Add(this.olvColBatch);
            this.olvPlannedIssues.AllColumns.Add(this.olvColReqQty);
            this.olvPlannedIssues.AllColumns.Add(this.olvColPlannedIssueQty);
            this.olvPlannedIssues.AllColumns.Add(this.olvColShort);
            this.olvPlannedIssues.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvPlannedIssues.CellEditUseWholeCell = false;
            this.olvPlannedIssues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColItem,
            this.olvColStorageLoc,
            this.olvColBatch,
            this.olvColReqQty,
            this.olvColPlannedIssueQty,
            this.olvColShort});
            this.olvPlannedIssues.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvPlannedIssues.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvPlannedIssues.Location = new System.Drawing.Point(0, 0);
            this.olvPlannedIssues.Name = "olvPlannedIssues";
            this.olvPlannedIssues.Size = new System.Drawing.Size(753, 431);
            this.olvPlannedIssues.TabIndex = 0;
            this.olvPlannedIssues.UseCompatibleStateImageBehavior = false;
            this.olvPlannedIssues.View = System.Windows.Forms.View.Details;
            this.olvPlannedIssues.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.olvPlannedIssues_FormatRow);
            // 
            // olvColItem
            // 
            this.olvColItem.AspectName = "ItemCode";
            this.olvColItem.Text = "Item";
            this.olvColItem.Width = 200;
            // 
            // olvColStorageLoc
            // 
            this.olvColStorageLoc.AspectName = "StorageLocation";
            this.olvColStorageLoc.Text = "Storage Loc.";
            this.olvColStorageLoc.Width = 120;
            // 
            // olvColBatch
            // 
            this.olvColBatch.AspectName = "Batch";
            this.olvColBatch.Text = "Batch";
            this.olvColBatch.Width = 100;
            // 
            // olvColReqQty
            // 
            this.olvColReqQty.AspectName = "Quantity";
            this.olvColReqQty.Text = "Req. Qty";
            this.olvColReqQty.Width = 80;
            // 
            // olvColPlannedIssueQty
            // 
            this.olvColPlannedIssueQty.AspectName = "PlannedIssueQty";
            this.olvColPlannedIssueQty.Text = "Planned Issue";
            this.olvColPlannedIssueQty.Width = 120;
            // 
            // olvColShort
            // 
            this.olvColShort.AspectName = "ShortQty";
            this.olvColShort.Text = "Short";
            this.olvColShort.Width = 80;
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(332, 451);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(87, 32);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FrmPlannedIssueDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 495);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.olvPlannedIssues);
            this.Name = "FrmPlannedIssueDialog";
            this.Text = "Planned Issues";
            ((System.ComponentModel.ISupportInitialize)(this.olvPlannedIssues)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvPlannedIssues;
        private System.Windows.Forms.Button btnOk;
        private BrightIdeasSoftware.OLVColumn olvColItem;
        private BrightIdeasSoftware.OLVColumn olvColStorageLoc;
        private BrightIdeasSoftware.OLVColumn olvColBatch;
        private BrightIdeasSoftware.OLVColumn olvColReqQty;
        private BrightIdeasSoftware.OLVColumn olvColPlannedIssueQty;
        private BrightIdeasSoftware.OLVColumn olvColShort;
    }
}