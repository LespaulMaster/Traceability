namespace ForkLift
{
    partial class FormFindRegal
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
            this.xLabel2 = new DesignLibrary.XLabel();
            this.xLabelTipRegala = new DesignLibrary.XLabel();
            this.xLabelLokacija = new DesignLibrary.XLabel();
            this.xButtonMetro1 = new DesignLibrary.XButtonMetro(this.components);
            this.SuspendLayout();
            // 
            // xLabel1
            // 
            this.xLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabel1.AutoSize = true;
            this.xLabel1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.xLabel1.Location = new System.Drawing.Point(70, 61);
            this.xLabel1.MaximumSize = new System.Drawing.Size(600, 0);
            this.xLabel1.Name = "xLabel1";
            this.xLabel1.Size = new System.Drawing.Size(111, 32);
            this.xLabel1.TabIndex = 0;
            this.xLabel1.Text = "Regal za ";
            // 
            // xLabel2
            // 
            this.xLabel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabel2.AutoSize = true;
            this.xLabel2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xLabel2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.xLabel2.Location = new System.Drawing.Point(414, 61);
            this.xLabel2.MaximumSize = new System.Drawing.Size(600, 0);
            this.xLabel2.Name = "xLabel2";
            this.xLabel2.Size = new System.Drawing.Size(224, 32);
            this.xLabel2.TabIndex = 1;
            this.xLabel2.Text = "nalazi se na lokaciji:";
            // 
            // xLabelTipRegala
            // 
            this.xLabelTipRegala.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabelTipRegala.AutoSize = true;
            this.xLabelTipRegala.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xLabelTipRegala.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.xLabelTipRegala.Location = new System.Drawing.Point(187, 57);
            this.xLabelTipRegala.MaximumSize = new System.Drawing.Size(600, 0);
            this.xLabelTipRegala.MinimumSize = new System.Drawing.Size(200, 0);
            this.xLabelTipRegala.Name = "xLabelTipRegala";
            this.xLabelTipRegala.Size = new System.Drawing.Size(200, 40);
            this.xLabelTipRegala.TabIndex = 2;
            this.xLabelTipRegala.Text = "Opis regala";
            this.xLabelTipRegala.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xLabelLokacija
            // 
            this.xLabelLokacija.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabelLokacija.AutoSize = true;
            this.xLabelLokacija.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xLabelLokacija.ForeColor = System.Drawing.Color.Orange;
            this.xLabelLokacija.Location = new System.Drawing.Point(209, 124);
            this.xLabelLokacija.MaximumSize = new System.Drawing.Size(600, 0);
            this.xLabelLokacija.MinimumSize = new System.Drawing.Size(300, 0);
            this.xLabelLokacija.Name = "xLabelLokacija";
            this.xLabelLokacija.Size = new System.Drawing.Size(300, 45);
            this.xLabelLokacija.TabIndex = 3;
            this.xLabelLokacija.Text = "Pozicija";
            this.xLabelLokacija.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xButtonMetro1
            // 
            this.xButtonMetro1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xButtonMetro1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.xButtonMetro1.FlatAppearance.BorderSize = 0;
            this.xButtonMetro1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xButtonMetro1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.xButtonMetro1.ForeColor = System.Drawing.Color.White;
            this.xButtonMetro1.Location = new System.Drawing.Point(276, 201);
            this.xButtonMetro1.Name = "xButtonMetro1";
            this.xButtonMetro1.Size = new System.Drawing.Size(150, 50);
            this.xButtonMetro1.TabIndex = 4;
            this.xButtonMetro1.TabStop = false;
            this.xButtonMetro1.Text = "Otkazi";
            this.xButtonMetro1.UseVisualStyleBackColor = false;
            this.xButtonMetro1.Click += new System.EventHandler(this.xButtonMetro1_Click);
            // 
            // FormFindRegal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(151)))));
            this.ClientSize = new System.Drawing.Size(697, 285);
            this.Controls.Add(this.xButtonMetro1);
            this.Controls.Add(this.xLabelLokacija);
            this.Controls.Add(this.xLabelTipRegala);
            this.Controls.Add(this.xLabel2);
            this.Controls.Add(this.xLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FormFindRegal";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormManua";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormFindRegal_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DesignLibrary.XLabel xLabel1;
        private DesignLibrary.XLabel xLabel2;
        private DesignLibrary.XLabel xLabelTipRegala;
        private DesignLibrary.XLabel xLabelLokacija;
        private DesignLibrary.XButtonMetro xButtonMetro1;
    }
}