using System.Windows.Forms;
namespace DesignLibrary
{
    partial class XZahtev
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelMasina = new DesignLibrary.XLabel();
            this.labelTime = new DesignLibrary.XLabel();
            this.labelText = new DesignLibrary.XLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(170)))), ((int)(((byte)(239)))));
            this.panel1.Controls.Add(this.labelMasina);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(91, 52);
            this.panel1.TabIndex = 3;
            // 
            // labelMasina
            // 
            this.labelMasina.AutoSize = true;
            this.labelMasina.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.labelMasina.ForeColor = System.Drawing.Color.White;
            this.labelMasina.Location = new System.Drawing.Point(7, 12);
            this.labelMasina.Name = "labelMasina";
            this.labelMasina.Size = new System.Drawing.Size(76, 25);
            this.labelMasina.TabIndex = 2;
            this.labelMasina.Text = "xLabel1";
            this.labelMasina.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTime
            // 
            this.labelTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.labelTime.ForeColor = System.Drawing.Color.White;
            this.labelTime.Location = new System.Drawing.Point(382, 12);
            this.labelTime.Name = "labelTime";
            this.labelTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelTime.Size = new System.Drawing.Size(76, 25);
            this.labelTime.TabIndex = 1;
            this.labelTime.Text = "xLabel2";
            // 
            // labelText
            // 
            this.labelText.AutoSize = true;
            this.labelText.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelText.ForeColor = System.Drawing.Color.White;
            this.labelText.Location = new System.Drawing.Point(107, 14);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(63, 21);
            this.labelText.TabIndex = 0;
            this.labelText.Text = "xLabel1";
            // 
            // XZahtev
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.labelText);
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.MaximumSize = new System.Drawing.Size(0, 52);
            this.MinimumSize = new System.Drawing.Size(400, 0);
            this.Name = "XZahtev";
            this.Size = new System.Drawing.Size(461, 52);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private XLabel labelText;
        private XLabel labelTime;
        private XLabel labelMasina;
        private System.Windows.Forms.Panel panel1;
    }
}
