namespace RollLabelProdPack
{
    partial class SelectOrderDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtEmployee = new System.Windows.Forms.TextBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cboProductionOrder = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboProductionLine = new System.Windows.Forms.ComboBox();
            this.cboShift = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Production Line";
            // 
            // txtEmployee
            // 
            this.txtEmployee.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmployee.Location = new System.Drawing.Point(174, 112);
            this.txtEmployee.Name = "txtEmployee";
            this.txtEmployee.Size = new System.Drawing.Size(305, 26);
            this.txtEmployee.TabIndex = 3;
            // 
            // lblEmployee
            // 
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployee.Location = new System.Drawing.Point(3, 112);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(79, 20);
            this.lblEmployee.TabIndex = 3;
            this.lblEmployee.Text = "Employee";
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(6, 197);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 36);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Production Order";
            // 
            // cboProductionOrder
            // 
            this.cboProductionOrder.Enabled = false;
            this.cboProductionOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProductionOrder.FormattingEnabled = true;
            this.cboProductionOrder.Location = new System.Drawing.Point(174, 69);
            this.cboProductionOrder.Name = "cboProductionOrder";
            this.cboProductionOrder.Size = new System.Drawing.Size(305, 28);
            this.cboProductionOrder.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Shift";
            // 
            // cboProductionLine
            // 
            this.cboProductionLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProductionLine.FormattingEnabled = true;
            this.cboProductionLine.Location = new System.Drawing.Point(174, 27);
            this.cboProductionLine.Name = "cboProductionLine";
            this.cboProductionLine.Size = new System.Drawing.Size(305, 28);
            this.cboProductionLine.TabIndex = 0;
            this.cboProductionLine.SelectedIndexChanged += new System.EventHandler(this.cboProductionLine_SelectedIndexChanged);
            // 
            // cboShift
            // 
            this.cboShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboShift.FormattingEnabled = true;
            this.cboShift.Items.AddRange(new object[] {
            "<-Please Select Shift->",
            "A",
            "B",
            "C",
            "D"});
            this.cboShift.Location = new System.Drawing.Point(175, 148);
            this.cboShift.Name = "cboShift";
            this.cboShift.Size = new System.Drawing.Size(305, 28);
            this.cboShift.TabIndex = 4;
            // 
            // SelectOrderDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 240);
            this.Controls.Add(this.cboShift);
            this.Controls.Add(this.cboProductionLine);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboProductionOrder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblEmployee);
            this.Controls.Add(this.txtEmployee);
            this.Controls.Add(this.label1);
            this.Name = "SelectOrderDialog";
            this.Text = "Select Order";
            this.Load += new System.EventHandler(this.SelectOrderDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEmployee;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboProductionOrder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboProductionLine;
        private System.Windows.Forms.ComboBox cboShift;
    }
}