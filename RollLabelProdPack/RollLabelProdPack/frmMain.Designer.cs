namespace RollLabelProdPack
{
    partial class FrmMain
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
            this.btnCreateRollLabels = new System.Windows.Forms.Button();
            this.btnPackAndProduce = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateRollLabels
            // 
            this.btnCreateRollLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateRollLabels.Location = new System.Drawing.Point(0, 32);
            this.btnCreateRollLabels.Name = "btnCreateRollLabels";
            this.btnCreateRollLabels.Size = new System.Drawing.Size(248, 92);
            this.btnCreateRollLabels.TabIndex = 0;
            this.btnCreateRollLabels.Text = "Create Roll Labels";
            this.btnCreateRollLabels.UseVisualStyleBackColor = true;
            this.btnCreateRollLabels.Click += new System.EventHandler(this.btnCreateRollLabels_Click);
            // 
            // btnPackAndProduce
            // 
            this.btnPackAndProduce.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPackAndProduce.Location = new System.Drawing.Point(0, 148);
            this.btnPackAndProduce.Name = "btnPackAndProduce";
            this.btnPackAndProduce.Size = new System.Drawing.Size(248, 107);
            this.btnPackAndProduce.TabIndex = 1;
            this.btnPackAndProduce.Text = "Pack and Produce";
            this.btnPackAndProduce.UseVisualStyleBackColor = true;
            this.btnPackAndProduce.Click += new System.EventHandler(this.btnPackAndProduce_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 452);
            this.Controls.Add(this.btnPackAndProduce);
            this.Controls.Add(this.btnCreateRollLabels);
            this.IsMdiContainer = true;
            this.Name = "Main";
            this.Text = "Main";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateRollLabels;
        private System.Windows.Forms.Button btnPackAndProduce;
    }
}

