namespace ForkLift
{
    partial class FormManual
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
            this.xLabel1 = new DesignLibrary.XLabel();
            this.SuspendLayout();
            // 
            // xLabel1
            // 
            this.xLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabel1.AutoSize = true;
            this.xLabel1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.xLabel1.Location = new System.Drawing.Point(50, 101);
            this.xLabel1.MaximumSize = new System.Drawing.Size(600, 0);
            this.xLabel1.Name = "xLabel1";
            this.xLabel1.Size = new System.Drawing.Size(594, 64);
            this.xLabel1.TabIndex = 0;
            this.xLabel1.Text = "Izabran manualni način rada, skenirati bar-kod lokacije na koju odvezete regal";
            // 
            // FormManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(151)))));
            this.ClientSize = new System.Drawing.Size(686, 266);
            this.Controls.Add(this.xLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FormManual";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormManua";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormManual_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DesignLibrary.XLabel xLabel1;
    }
}