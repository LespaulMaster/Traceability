using System.Windows.Forms;
namespace ForkLift
{
    partial class FormLeftProdInPalet
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
            this.xButtonMetro1 = new DesignLibrary.XButtonMetro(this.components);
            this.xButtonMetro2 = new DesignLibrary.XButtonMetro(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.xButtonMetro3 = new DesignLibrary.XButtonMetro(this.components);
            this.xLabel1 = new DesignLibrary.XLabel();
            this.SuspendLayout();
            // 
            // xButtonMetro1
            // 
            this.xButtonMetro1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xButtonMetro1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.xButtonMetro1.FlatAppearance.BorderSize = 0;
            this.xButtonMetro1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xButtonMetro1.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.xButtonMetro1.ForeColor = System.Drawing.Color.White;
            this.xButtonMetro1.Location = new System.Drawing.Point(33, 183);
            this.xButtonMetro1.Margin = new System.Windows.Forms.Padding(48, 22, 48, 22);
            this.xButtonMetro1.Name = "xButtonMetro1";
            this.xButtonMetro1.Size = new System.Drawing.Size(54, 53);
            this.xButtonMetro1.TabIndex = 0;
            this.xButtonMetro1.Text = "-";
            this.xButtonMetro1.UseVisualStyleBackColor = false;
            this.xButtonMetro1.Click += new System.EventHandler(this.RemoveProduct);
            // 
            // xButtonMetro2
            // 
            this.xButtonMetro2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xButtonMetro2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.xButtonMetro2.FlatAppearance.BorderSize = 0;
            this.xButtonMetro2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xButtonMetro2.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.xButtonMetro2.ForeColor = System.Drawing.Color.White;
            this.xButtonMetro2.Location = new System.Drawing.Point(115, 183);
            this.xButtonMetro2.Margin = new System.Windows.Forms.Padding(48, 22, 48, 22);
            this.xButtonMetro2.Name = "xButtonMetro2";
            this.xButtonMetro2.Size = new System.Drawing.Size(54, 53);
            this.xButtonMetro2.TabIndex = 1;
            this.xButtonMetro2.Text = "+";
            this.xButtonMetro2.UseVisualStyleBackColor = false;
            this.xButtonMetro2.Click += new System.EventHandler(this.AddProduct);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.textBox1.Location = new System.Drawing.Point(33, 132);
            this.textBox1.Margin = new System.Windows.Forms.Padding(48, 22, 48, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(136, 25);
            this.textBox1.TabIndex = 2;
            // 
            // xButtonMetro3
            // 
            this.xButtonMetro3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xButtonMetro3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.xButtonMetro3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.xButtonMetro3.FlatAppearance.BorderSize = 0;
            this.xButtonMetro3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xButtonMetro3.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.xButtonMetro3.ForeColor = System.Drawing.Color.White;
            this.xButtonMetro3.Location = new System.Drawing.Point(250, 132);
            this.xButtonMetro3.Margin = new System.Windows.Forms.Padding(48, 22, 48, 22);
            this.xButtonMetro3.Name = "xButtonMetro3";
            this.xButtonMetro3.Size = new System.Drawing.Size(164, 104);
            this.xButtonMetro3.TabIndex = 3;
            this.xButtonMetro3.Text = "Odvezi na skladište";
            this.xButtonMetro3.UseVisualStyleBackColor = false;
            this.xButtonMetro3.Click += new System.EventHandler(this.xButtonMetro3_Click);
            // 
            // xLabel1
            // 
            this.xLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xLabel1.AutoSize = true;
            this.xLabel1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.xLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.xLabel1.Location = new System.Drawing.Point(110, 52);
            this.xLabel1.Margin = new System.Windows.Forms.Padding(48, 0, 48, 0);
            this.xLabel1.Name = "xLabel1";
            this.xLabel1.Size = new System.Drawing.Size(213, 25);
            this.xLabel1.TabIndex = 4;
            this.xLabel1.Text = "Broj proizvoda na paleti";
            // 
            // FormLeftProdInPalet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(443, 292);
            this.Controls.Add(this.xLabel1);
            this.Controls.Add(this.xButtonMetro3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.xButtonMetro2);
            this.Controls.Add(this.xButtonMetro1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(48, 22, 48, 22);
            this.Name = "FormLeftProdInPalet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormLeftProdInPalet";
            this.Load += new System.EventHandler(this.FormLeftProdInPalet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DesignLibrary.XButtonMetro xButtonMetro1;
        private DesignLibrary.XButtonMetro xButtonMetro2;
        private System.Windows.Forms.TextBox textBox1;
        private DesignLibrary.XButtonMetro xButtonMetro3;
        private DesignLibrary.XLabel xLabel1;
    }
}