namespace RollLabelProdPack
{
    partial class frmTub2
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.toLineComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tubScrapUserControl1 = new RollLabelProdPack.TubScrapUserControl();
            this.tubProductionUserControl1 = new RollLabelProdPack.TubProductionUserControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tubProductionUserControl1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(903, 399);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Produce";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tubScrapUserControl1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 478);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(903, 110);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scrap";
            this.groupBox2.Visible = false;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(13, 15);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(142, 42);
            this.btnSelect.TabIndex = 143;
            this.btnSelect.Text = "SELECT ORDER";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // toLineComboBox
            // 
            this.toLineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toLineComboBox.Enabled = false;
            this.toLineComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toLineComboBox.FormattingEnabled = true;
            this.toLineComboBox.Location = new System.Drawing.Point(245, 23);
            this.toLineComboBox.Name = "toLineComboBox";
            this.toLineComboBox.Size = new System.Drawing.Size(142, 28);
            this.toLineComboBox.TabIndex = 144;
            this.toLineComboBox.SelectedIndexChanged += new System.EventHandler(this.toLineComboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(174, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 20);
            this.label7.TabIndex = 145;
            this.label7.Text = "To Line";
            // 
            // tubScrapUserControl1
            // 
            this.tubScrapUserControl1.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.tubScrapUserControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tubScrapUserControl1.Location = new System.Drawing.Point(23, 25);
            this.tubScrapUserControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tubScrapUserControl1.Name = "tubScrapUserControl1";
            this.tubScrapUserControl1.Order = null;
            this.tubScrapUserControl1.ScrapReasons = null;
            this.tubScrapUserControl1.Size = new System.Drawing.Size(867, 72);
            this.tubScrapUserControl1.TabIndex = 0;
            // 
            // tubProductionUserControl1
            // 
            this.tubProductionUserControl1.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.tubProductionUserControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tubProductionUserControl1.Location = new System.Drawing.Point(23, 22);
            this.tubProductionUserControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tubProductionUserControl1.Name = "tubProductionUserControl1";
            this.tubProductionUserControl1.NumberOfCases = 0;
            this.tubProductionUserControl1.Order = null;
            this.tubProductionUserControl1.Qty = 0;
            this.tubProductionUserControl1.Size = new System.Drawing.Size(863, 370);
            this.tubProductionUserControl1.TabIndex = 0;
            // 
            // frmTub2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 474);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.toLineComboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTub2";
            this.Text = "Tub Production";
            this.Load += new System.EventHandler(this.frmTub2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private TubProductionUserControl tubProductionUserControl1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ComboBox toLineComboBox;
        private System.Windows.Forms.Label label7;
        private TubScrapUserControl tubScrapUserControl1;
    }
}