namespace RollLabelProdPack
{
    partial class FrmRollsDialog
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
            this.olvRolls = new BrightIdeasSoftware.ObjectListView();
            this.olvColRollNo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColYJNOrder = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColItemCode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColItemName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColKgs = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColTareKg = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColAdjustKgs = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvNetKg = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.olvRolls)).BeginInit();
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
            this.txtValidationMessage.Size = new System.Drawing.Size(863, 84);
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
            // olvRolls
            // 
            this.olvRolls.AllColumns.Add(this.olvColRollNo);
            this.olvRolls.AllColumns.Add(this.olvColYJNOrder);
            this.olvRolls.AllColumns.Add(this.olvColItemCode);
            this.olvRolls.AllColumns.Add(this.olvColItemName);
            this.olvRolls.AllColumns.Add(this.olvColKgs);
            this.olvRolls.AllColumns.Add(this.olvColTareKg);
            this.olvRolls.AllColumns.Add(this.olvColAdjustKgs);
            this.olvRolls.AllColumns.Add(this.olvNetKg);
            this.olvRolls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvRolls.CellEditUseWholeCell = false;
            this.olvRolls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColRollNo,
            this.olvColYJNOrder,
            this.olvColItemCode,
            this.olvColItemName,
            this.olvColKgs,
            this.olvColTareKg,
            this.olvColAdjustKgs,
            this.olvNetKg});
            this.olvRolls.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvRolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvRolls.Location = new System.Drawing.Point(0, 0);
            this.olvRolls.Name = "olvRolls";
            this.olvRolls.Size = new System.Drawing.Size(863, 387);
            this.olvRolls.TabIndex = 0;
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
            // olvColKgs
            // 
            this.olvColKgs.AspectName = "Kgs";
            this.olvColKgs.AspectToStringFormat = "{0:0.0}";
            this.olvColKgs.Text = "Kgs";
            this.olvColKgs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColKgs.Width = 80;
            // 
            // olvColTareKg
            // 
            this.olvColTareKg.AspectName = "TareKg";
            this.olvColTareKg.AspectToStringFormat = "{0:0.0}";
            this.olvColTareKg.Text = "Tare kg";
            this.olvColTareKg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColTareKg.Width = 70;
            // 
            // olvColAdjustKgs
            // 
            this.olvColAdjustKgs.AspectName = "AdjustKgs";
            this.olvColAdjustKgs.AspectToStringFormat = "{0:N5}";
            this.olvColAdjustKgs.IsEditable = false;
            this.olvColAdjustKgs.Text = "Adjust kg";
            this.olvColAdjustKgs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColAdjustKgs.Width = 100;
            // 
            // olvNetKg
            // 
            this.olvNetKg.AspectName = "NetKg";
            this.olvNetKg.AspectToStringFormat = "{0:0.0}";
            this.olvNetKg.Text = "Net kg";
            this.olvNetKg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // FrmRollsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 549);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtValidationMessage);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.olvRolls);
            this.Name = "FrmRollsDialog";
            this.Text = "Pack Rolls";
            ((System.ComponentModel.ISupportInitialize)(this.olvRolls)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvRolls;
        private System.Windows.Forms.Button btnOk;
        private BrightIdeasSoftware.OLVColumn olvColRollNo;
        private BrightIdeasSoftware.OLVColumn olvColItemCode;
        private BrightIdeasSoftware.OLVColumn olvColItemName;
        private BrightIdeasSoftware.OLVColumn olvColKgs;
        private BrightIdeasSoftware.OLVColumn olvColYJNOrder;
        private System.Windows.Forms.TextBox txtValidationMessage;
        private System.Windows.Forms.Label label1;
        private BrightIdeasSoftware.OLVColumn olvColAdjustKgs;
        private BrightIdeasSoftware.OLVColumn olvColTareKg;
        private BrightIdeasSoftware.OLVColumn olvNetKg;
    }
}