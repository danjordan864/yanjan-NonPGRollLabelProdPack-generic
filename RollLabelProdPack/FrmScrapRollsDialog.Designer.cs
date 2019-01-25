namespace RollLabelProdPack
{
    partial class FrmScrapRollsDialog
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
            this.olvColJumboRoll = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColRollNo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColScrapReason = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnScrap = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.olvOrderRolls)).BeginInit();
            this.SuspendLayout();
            // 
            // olvOrderRolls
            // 
            this.olvOrderRolls.AllColumns.Add(this.olvColJumboRoll);
            this.olvOrderRolls.AllColumns.Add(this.olvColRollNo);
            this.olvOrderRolls.AllColumns.Add(this.olvColScrapReason);
            this.olvOrderRolls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvOrderRolls.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvOrderRolls.CellEditUseWholeCell = false;
            this.olvOrderRolls.CheckBoxes = true;
            this.olvOrderRolls.CheckedAspectName = "Scrap";
            this.olvOrderRolls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColJumboRoll,
            this.olvColRollNo,
            this.olvColScrapReason});
            this.olvOrderRolls.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvOrderRolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvOrderRolls.Location = new System.Drawing.Point(0, 0);
            this.olvOrderRolls.Name = "olvOrderRolls";
            this.olvOrderRolls.Size = new System.Drawing.Size(533, 427);
            this.olvOrderRolls.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.olvOrderRolls.TabIndex = 0;
            this.olvOrderRolls.UseCompatibleStateImageBehavior = false;
            this.olvOrderRolls.View = System.Windows.Forms.View.Details;
            this.olvOrderRolls.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.olvOrderRolls_CellEditFinishing);
            this.olvOrderRolls.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvOrderRolls_CellEditStarting);
            // 
            // olvColJumboRoll
            // 
            this.olvColJumboRoll.AspectName = "JumboRoll";
            this.olvColJumboRoll.Text = "Jumbo Roll";
            this.olvColJumboRoll.Width = 100;
            // 
            // olvColRollNo
            // 
            this.olvColRollNo.AspectName = "RollNo";
            this.olvColRollNo.Groupable = false;
            this.olvColRollNo.Text = "Roll No.";
            this.olvColRollNo.Width = 200;
            // 
            // olvColScrapReason
            // 
            this.olvColScrapReason.AspectName = "ScrapReason";
            this.olvColScrapReason.Text = "Scrap Reason";
            this.olvColScrapReason.Width = 218;
            // 
            // btnScrap
            // 
            this.btnScrap.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnScrap.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScrap.Location = new System.Drawing.Point(0, 424);
            this.btnScrap.Name = "btnScrap";
            this.btnScrap.Size = new System.Drawing.Size(533, 32);
            this.btnScrap.TabIndex = 1;
            this.btnScrap.Text = "Scrap";
            this.btnScrap.UseVisualStyleBackColor = true;
            this.btnScrap.Click += new System.EventHandler(this.btnScrap_Click);
            // 
            // FrmScrapRollsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 456);
            this.Controls.Add(this.btnScrap);
            this.Controls.Add(this.olvOrderRolls);
            this.Name = "FrmScrapRollsDialog";
            this.Text = "Scrap";
            this.Load += new System.EventHandler(this.FrmScrapRollsDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvOrderRolls)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvOrderRolls;
        private System.Windows.Forms.Button btnScrap;
        private BrightIdeasSoftware.OLVColumn olvColRollNo;
        private BrightIdeasSoftware.OLVColumn olvColJumboRoll;
        private BrightIdeasSoftware.OLVColumn olvColScrapReason;
    }
}