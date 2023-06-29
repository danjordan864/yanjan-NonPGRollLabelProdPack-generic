namespace RollLabelProdPack
{
    partial class FrmCasesDialog
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
            this.btnOk = new System.Windows.Forms.Button();
            this.txtValidationMessage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.olvCases = new BrightIdeasSoftware.ObjectListView();
            this.olvColCaseNo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColYJNOrder = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColItemCode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColItemName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColUnits = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.olvCases)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(326, 505);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(87, 32);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtValidationMessage
            // 
            this.txtValidationMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtValidationMessage.Location = new System.Drawing.Point(0, 413);
            this.txtValidationMessage.Multiline = true;
            this.txtValidationMessage.Name = "txtValidationMessage";
            this.txtValidationMessage.Size = new System.Drawing.Size(795, 84);
            this.txtValidationMessage.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 390);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Validation Message";
            // 
            // olvCases
            // 
            this.olvCases.AllColumns.Add(this.olvColCaseNo);
            this.olvCases.AllColumns.Add(this.olvColYJNOrder);
            this.olvCases.AllColumns.Add(this.olvColItemCode);
            this.olvCases.AllColumns.Add(this.olvColItemName);
            this.olvCases.AllColumns.Add(this.olvColUnits);
            this.olvCases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvCases.CellEditUseWholeCell = false;
            this.olvCases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColCaseNo,
            this.olvColYJNOrder,
            this.olvColItemCode,
            this.olvColItemName,
            this.olvColUnits});
            this.olvCases.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvCases.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvCases.Location = new System.Drawing.Point(0, 0);
            this.olvCases.Name = "olvCases";
            this.olvCases.Size = new System.Drawing.Size(795, 387);
            this.olvCases.TabIndex = 0;
            this.olvCases.UseCompatibleStateImageBehavior = false;
            this.olvCases.View = System.Windows.Forms.View.Details;
            // 
            // olvColCaseNo
            // 
            this.olvColCaseNo.AspectName = "CaseNo";
            this.olvColCaseNo.Groupable = false;
            this.olvColCaseNo.Text = "Case No.";
            this.olvColCaseNo.Width = 200;
            // 
            // olvColYJNOrder
            // 
            this.olvColYJNOrder.AspectName = "YJNOrder";
            this.olvColYJNOrder.Text = "Order";
            this.olvColYJNOrder.Width = 120;
            // 
            // olvColItemCode
            // 
            this.olvColItemCode.AspectName = "ItemCode";
            this.olvColItemCode.Text = "Item Code";
            this.olvColItemCode.Width = 120;
            // 
            // olvColItemName
            // 
            this.olvColItemName.AspectName = "ItemName";
            this.olvColItemName.Text = "Item Name";
            this.olvColItemName.Width = 200;
            // 
            // olvColUnits
            // 
            this.olvColUnits.AspectName = "Units";
            this.olvColUnits.Text = "Units";
            this.olvColUnits.Width = 80;
            // 
            // FrmCasesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 549);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtValidationMessage);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.olvCases);
            this.Name = "FrmCasesDialog";
            this.Text = "Palletize Cases";
            ((System.ComponentModel.ISupportInitialize)(this.olvCases)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvCases;
        private System.Windows.Forms.Button btnOk;
        private BrightIdeasSoftware.OLVColumn olvColCaseNo;
        private BrightIdeasSoftware.OLVColumn olvColItemCode;
        private BrightIdeasSoftware.OLVColumn olvColItemName;
        private BrightIdeasSoftware.OLVColumn olvColUnits;
        private BrightIdeasSoftware.OLVColumn olvColYJNOrder;
        private System.Windows.Forms.TextBox txtValidationMessage;
        private System.Windows.Forms.Label label1;
    }
}