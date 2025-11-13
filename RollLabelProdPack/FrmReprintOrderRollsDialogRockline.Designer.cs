namespace RollLabelProdPack
{
    partial class FrmReprintOrderRollsDialogRockline
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
            this.olvOrderRolls = new BrightIdeasSoftware.ObjectListView();
            this.olvColRollNo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnPrint = new System.Windows.Forms.Button();
            this.olvColJumboRoll = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.olvOrderRolls)).BeginInit();
            this.SuspendLayout();
            // 
            // olvOrderRolls
            // 
            this.olvOrderRolls.AllColumns.Add(this.olvColJumboRoll);
            this.olvOrderRolls.AllColumns.Add(this.olvColRollNo);
            this.olvOrderRolls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvOrderRolls.CellEditUseWholeCell = false;
            this.olvOrderRolls.CheckBoxes = true;
            this.olvOrderRolls.CheckedAspectName = "Print";
            this.olvOrderRolls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColJumboRoll,
            this.olvColRollNo});
            this.olvOrderRolls.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvOrderRolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvOrderRolls.Location = new System.Drawing.Point(0, 0);
            this.olvOrderRolls.Name = "olvOrderRolls";
            this.olvOrderRolls.Size = new System.Drawing.Size(337, 402);
            this.olvOrderRolls.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.olvOrderRolls.TabIndex = 0;
            this.olvOrderRolls.UseCompatibleStateImageBehavior = false;
            this.olvOrderRolls.View = System.Windows.Forms.View.Details;
            // 
            // olvColRollNo
            // 
            this.olvColRollNo.AspectName = "RollNo";
            this.olvColRollNo.Groupable = false;
            this.olvColRollNo.Text = "Roll No.";
            this.olvColRollNo.Width = 200;
            // 
            // btnPrint
            // 
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Location = new System.Drawing.Point(0, 399);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(337, 32);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // olvColJumboRoll
            // 
            this.olvColJumboRoll.AspectName = "JumboRoll";
            this.olvColJumboRoll.Text = "Jumbo Roll";
            this.olvColJumboRoll.Width = 100;
            // 
            // FrmReprintOrderRollsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 431);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.olvOrderRolls);
            this.Name = "FrmReprintOrderRollsDialog";
            this.Text = "Reprint";
            this.Load += new System.EventHandler(this.FrmReprintOrderRollsDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvOrderRolls)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvOrderRolls;
        private System.Windows.Forms.Button btnPrint;
        private BrightIdeasSoftware.OLVColumn olvColRollNo;
        private BrightIdeasSoftware.OLVColumn olvColJumboRoll;
    }
}