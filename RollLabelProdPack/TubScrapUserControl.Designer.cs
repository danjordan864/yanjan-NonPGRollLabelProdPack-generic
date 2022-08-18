namespace RollLabelProdPack
{
    partial class TubScrapUserControl
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
            this.label13 = new System.Windows.Forms.Label();
            this.scrapItemLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.scrapQtyTextBox = new System.Windows.Forms.TextBox();
            this.scrapButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.scrapReasonsComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(471, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 20);
            this.label13.TabIndex = 148;
            this.label13.Text = "Scrap Qty";
            // 
            // scrapItemLabel
            // 
            this.scrapItemLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scrapItemLabel.Location = new System.Drawing.Point(104, 25);
            this.scrapItemLabel.Name = "scrapItemLabel";
            this.scrapItemLabel.Size = new System.Drawing.Size(123, 20);
            this.scrapItemLabel.TabIndex = 147;
            this.scrapItemLabel.Text = "N/A";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(13, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(87, 20);
            this.label11.TabIndex = 146;
            this.label11.Text = "Scrap Item";
            // 
            // scrapQtyTextBox
            // 
            this.scrapQtyTextBox.Enabled = false;
            this.scrapQtyTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scrapQtyTextBox.Location = new System.Drawing.Point(581, 22);
            this.scrapQtyTextBox.Name = "scrapQtyTextBox";
            this.scrapQtyTextBox.Size = new System.Drawing.Size(115, 26);
            this.scrapQtyTextBox.TabIndex = 2;
            this.scrapQtyTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.scrapQtyTextBox_Validating);
            // 
            // scrapButton
            // 
            this.scrapButton.Enabled = false;
            this.scrapButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scrapButton.Location = new System.Drawing.Point(712, 14);
            this.scrapButton.Margin = new System.Windows.Forms.Padding(4);
            this.scrapButton.Name = "scrapButton";
            this.scrapButton.Size = new System.Drawing.Size(142, 42);
            this.scrapButton.TabIndex = 3;
            this.scrapButton.Text = "SCRAP";
            this.scrapButton.UseVisualStyleBackColor = true;
            this.scrapButton.Click += new System.EventHandler(this.scrapButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(233, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 149;
            this.label1.Text = "Reason";
            // 
            // scrapReasonsComboBox
            // 
            this.scrapReasonsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scrapReasonsComboBox.Enabled = false;
            this.scrapReasonsComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scrapReasonsComboBox.FormattingEnabled = true;
            this.scrapReasonsComboBox.Location = new System.Drawing.Point(305, 23);
            this.scrapReasonsComboBox.Name = "scrapReasonsComboBox";
            this.scrapReasonsComboBox.Size = new System.Drawing.Size(146, 28);
            this.scrapReasonsComboBox.TabIndex = 1;
            this.scrapReasonsComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.scrapReasonsComboBox_Validating);
            // 
            // TubScrapUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.Controls.Add(this.scrapReasonsComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.scrapItemLabel);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.scrapQtyTextBox);
            this.Controls.Add(this.scrapButton);
            this.Name = "TubScrapUserControl";
            this.Size = new System.Drawing.Size(867, 72);
            this.Load += new System.EventHandler(this.TubScrapUserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label scrapItemLabel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox scrapQtyTextBox;
        private System.Windows.Forms.Button scrapButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox scrapReasonsComboBox;
    }
}
