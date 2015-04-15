using System.Windows.Forms;
using System.Drawing;
namespace ForkLift
{
    partial class FormSelectZahtev
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
            this.components = new System.ComponentModel.Container();
            this.xLabel1 = new DesignLibrary.XLabel();
            this.xButtonMetro1 = new DesignLibrary.XButtonMetro(this.components);
            this.xLabel2 = new DesignLibrary.XLabel();
            this.xLabelOD = new DesignLibrary.XLabel();
            this.xLabelDO = new DesignLibrary.XLabel();
            this.xLabel3 = new DesignLibrary.XLabel();
            this.xLabelRegal = new DesignLibrary.XLabel();
            this.SuspendLayout();
            // 
            // xLabel1
            // 
            this.xLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabel1.AutoSize = true;
            this.xLabel1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.xLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.xLabel1.Location = new System.Drawing.Point(208, 69);
            this.xLabel1.Name = "xLabel1";
            this.xLabel1.Size = new System.Drawing.Size(131, 25);
            this.xLabel1.TabIndex = 0;
            this.xLabel1.Text = "pomeriti regal";
            this.xLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xButtonMetro1
            // 
            this.xButtonMetro1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xButtonMetro1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.xButtonMetro1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.xButtonMetro1.FlatAppearance.BorderSize = 0;
            this.xButtonMetro1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xButtonMetro1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.xButtonMetro1.ForeColor = System.Drawing.Color.White;
            this.xButtonMetro1.Location = new System.Drawing.Point(276, 262);
            this.xButtonMetro1.Name = "xButtonMetro1";
            this.xButtonMetro1.Size = new System.Drawing.Size(176, 59);
            this.xButtonMetro1.TabIndex = 1;
            this.xButtonMetro1.TabStop = false;
            this.xButtonMetro1.Text = "Otkaži";
            this.xButtonMetro1.UseVisualStyleBackColor = false;
            // 
            // xLabel2
            // 
            this.xLabel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabel2.AutoSize = true;
            this.xLabel2.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.xLabel2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.xLabel2.Location = new System.Drawing.Point(235, 193);
            this.xLabel2.Name = "xLabel2";
            this.xLabel2.Size = new System.Drawing.Size(104, 25);
            this.xLabel2.TabIndex = 2;
            this.xLabel2.Text = "na poziciju";
            this.xLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xLabelOD
            // 
            this.xLabelOD.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabelOD.AutoSize = true;
            this.xLabelOD.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xLabelOD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.xLabelOD.Location = new System.Drawing.Point(375, 121);
            this.xLabelOD.Name = "xLabelOD";
            this.xLabelOD.Size = new System.Drawing.Size(157, 37);
            this.xLabelOD.TabIndex = 3;
            this.xLabelOD.Text = "pozicija od";
            this.xLabelOD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xLabelDO
            // 
            this.xLabelDO.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabelDO.AutoSize = true;
            this.xLabelDO.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xLabelDO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.xLabelDO.Location = new System.Drawing.Point(375, 183);
            this.xLabelDO.Name = "xLabelDO";
            this.xLabelDO.Size = new System.Drawing.Size(157, 37);
            this.xLabelDO.TabIndex = 4;
            this.xLabelDO.Text = "pozicija do";
            this.xLabelDO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xLabel3
            // 
            this.xLabel3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabel3.AutoSize = true;
            this.xLabel3.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.xLabel3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.xLabel3.Location = new System.Drawing.Point(235, 131);
            this.xLabel3.Name = "xLabel3";
            this.xLabel3.Size = new System.Drawing.Size(100, 25);
            this.xLabel3.TabIndex = 5;
            this.xLabel3.Text = "sa pozicije";
            this.xLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xLabelRegal
            // 
            this.xLabelRegal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabelRegal.AutoSize = true;
            this.xLabelRegal.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xLabelRegal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.xLabelRegal.Location = new System.Drawing.Point(375, 59);
            this.xLabelRegal.Name = "xLabelRegal";
            this.xLabelRegal.Size = new System.Drawing.Size(83, 37);
            this.xLabelRegal.TabIndex = 6;
            this.xLabelRegal.Text = "regal";
            this.xLabelRegal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormSelectZahtev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(724, 368);
            this.Controls.Add(this.xLabelRegal);
            this.Controls.Add(this.xLabel3);
            this.Controls.Add(this.xLabelDO);
            this.Controls.Add(this.xLabelOD);
            this.Controls.Add(this.xLabel2);
            this.Controls.Add(this.xButtonMetro1);
            this.Controls.Add(this.xLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FormSelectZahtev";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormSelectZahtev";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormSelectZahtev_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DesignLibrary.XLabel xLabel1;
        private DesignLibrary.XButtonMetro xButtonMetro1;
        private DesignLibrary.XLabel xLabel2;
        private DesignLibrary.XLabel xLabelOD;
        private DesignLibrary.XLabel xLabelDO;
        private DesignLibrary.XLabel xLabel3;
        private DesignLibrary.XLabel xLabelRegal;

    }
}