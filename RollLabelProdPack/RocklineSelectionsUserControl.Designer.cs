namespace RollLabelProdPack
{
    partial class RocklineSelectionsUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.FilmProductionButton = new System.Windows.Forms.Button();
            this.PackPrintButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.PackPrintButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.FilmProductionButton, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(740, 390);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // FilmProductionButton
            // 
            this.FilmProductionButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.FilmProductionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilmProductionButton.Location = new System.Drawing.Point(87, 59);
            this.FilmProductionButton.MaximumSize = new System.Drawing.Size(195, 77);
            this.FilmProductionButton.MinimumSize = new System.Drawing.Size(195, 77);
            this.FilmProductionButton.Name = "FilmProductionButton";
            this.FilmProductionButton.Size = new System.Drawing.Size(195, 77);
            this.FilmProductionButton.TabIndex = 0;
            this.FilmProductionButton.Text = "Rockline Film Production";
            this.FilmProductionButton.UseVisualStyleBackColor = true;
            this.FilmProductionButton.Click += new System.EventHandler(this.FilmProductionButton_Click);
            // 
            // PackPrintButton
            // 
            this.PackPrintButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PackPrintButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PackPrintButton.Location = new System.Drawing.Point(457, 59);
            this.PackPrintButton.MaximumSize = new System.Drawing.Size(195, 77);
            this.PackPrintButton.MinimumSize = new System.Drawing.Size(195, 77);
            this.PackPrintButton.Name = "PackPrintButton";
            this.PackPrintButton.Size = new System.Drawing.Size(195, 77);
            this.PackPrintButton.TabIndex = 1;
            this.PackPrintButton.Text = "Rockline Pack Print";
            this.PackPrintButton.UseVisualStyleBackColor = true;
            this.PackPrintButton.Click += new System.EventHandler(this.PackPrintButton_Click);
            // 
            // RocklineSelectionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RocklineSelectionsUserControl";
            this.Size = new System.Drawing.Size(740, 390);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button FilmProductionButton;
        private System.Windows.Forms.Button PackPrintButton;
    }
}
