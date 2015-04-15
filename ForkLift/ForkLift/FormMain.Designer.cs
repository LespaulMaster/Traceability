namespace ForkLift
{
    partial class FormMain
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
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelIme = new DesignLibrary.XLabel();
            this.labelVreme = new DesignLibrary.XLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.labelClock = new DesignLibrary.XLabel();
            this.xLabel5 = new DesignLibrary.XLabel();
            this.xLabel4 = new DesignLibrary.XLabel();
            this.xLabel3 = new DesignLibrary.XLabel();
            this.button2 = new System.Windows.Forms.Button();
            this.labelMasina = new DesignLibrary.XLabel();
            this.flowLayoutPanelYellow = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.btn13 = new DesignLibrary.XButtonMetro(this.components);
            this.btn14 = new DesignLibrary.XButtonMetro(this.components);
            this.btn15 = new DesignLibrary.XButtonMetro(this.components);
            this.btn16 = new DesignLibrary.XButtonMetro(this.components);
            this.btn17 = new DesignLibrary.XButtonMetro(this.components);
            this.btn18 = new DesignLibrary.XButtonMetro(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(620, 264);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.labelIme);
            this.flowLayoutPanel1.Controls.Add(this.labelVreme);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(593, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(283, 78);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // labelIme
            // 
            this.labelIme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelIme.AutoSize = true;
            this.labelIme.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIme.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelIme.Location = new System.Drawing.Point(119, 0);
            this.labelIme.Name = "labelIme";
            this.labelIme.Size = new System.Drawing.Size(161, 32);
            this.labelIme.TabIndex = 0;
            this.labelIme.Text = "Ime i prezime";
            this.labelIme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelVreme
            // 
            this.labelVreme.AutoSize = true;
            this.labelVreme.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelVreme.ForeColor = System.Drawing.Color.Gainsboro;
            this.labelVreme.Location = new System.Drawing.Point(175, 32);
            this.labelVreme.Name = "labelVreme";
            this.labelVreme.Size = new System.Drawing.Size(105, 21);
            this.labelVreme.TabIndex = 2;
            this.labelVreme.Text = "vreme prijave";
            this.labelVreme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(16, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(849, 1);
            this.label1.TabIndex = 8;
            // 
            // labelClock
            // 
            this.labelClock.AutoSize = true;
            this.labelClock.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClock.ForeColor = System.Drawing.Color.White;
            this.labelClock.Location = new System.Drawing.Point(15, 14);
            this.labelClock.Name = "labelClock";
            this.labelClock.Size = new System.Drawing.Size(97, 45);
            this.labelClock.TabIndex = 6;
            this.labelClock.Text = "-- : --";
            this.labelClock.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // xLabel5
            // 
            this.xLabel5.AutoSize = true;
            this.xLabel5.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.xLabel5.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.xLabel5.Location = new System.Drawing.Point(130, 106);
            this.xLabel5.Name = "xLabel5";
            this.xLabel5.Size = new System.Drawing.Size(39, 21);
            this.xLabel5.TabIndex = 5;
            this.xLabel5.Text = "opis";
            // 
            // xLabel4
            // 
            this.xLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.xLabel4.AutoSize = true;
            this.xLabel4.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.xLabel4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.xLabel4.Location = new System.Drawing.Point(562, 106);
            this.xLabel4.Name = "xLabel4";
            this.xLabel4.Size = new System.Drawing.Size(54, 21);
            this.xLabel4.TabIndex = 5;
            this.xLabel4.Text = "vreme";
            this.xLabel4.Visible = false;
            // 
            // xLabel3
            // 
            this.xLabel3.AutoSize = true;
            this.xLabel3.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.xLabel3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.xLabel3.Location = new System.Drawing.Point(15, 106);
            this.xLabel3.Name = "xLabel3";
            this.xLabel3.Size = new System.Drawing.Size(60, 21);
            this.xLabel3.TabIndex = 4;
            this.xLabel3.Text = "mašina";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button2.Location = new System.Drawing.Point(332, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 9;
            this.button2.TabStop = false;
            this.button2.Text = "EXIT";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // labelMasina
            // 
            this.labelMasina.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMasina.AutoSize = true;
            this.labelMasina.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.labelMasina.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelMasina.Location = new System.Drawing.Point(786, 85);
            this.labelMasina.Name = "labelMasina";
            this.labelMasina.Size = new System.Drawing.Size(81, 25);
            this.labelMasina.TabIndex = 10;
            this.labelMasina.Text = "viljuskar";
            // 
            // flowLayoutPanelYellow
            // 
            this.flowLayoutPanelYellow.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanelYellow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelYellow.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelYellow.Name = "flowLayoutPanelYellow";
            this.flowLayoutPanelYellow.Size = new System.Drawing.Size(620, 141);
            this.flowLayoutPanelYellow.TabIndex = 12;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 130);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanelYellow);
            this.splitContainer1.Size = new System.Drawing.Size(620, 409);
            this.splitContainer1.SplitterDistance = 264;
            this.splitContainer1.TabIndex = 13;
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 10000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // btn13
            // 
            this.btn13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btn13.FlatAppearance.BorderSize = 0;
            this.btn13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn13.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btn13.ForeColor = System.Drawing.Color.White;
            this.btn13.Location = new System.Drawing.Point(647, 131);
            this.btn13.Name = "btn13";
            this.btn13.Size = new System.Drawing.Size(218, 61);
            this.btn13.TabIndex = 14;
            this.btn13.Text = "13 cola";
            this.btn13.UseVisualStyleBackColor = false;
            this.btn13.Click += new System.EventHandler(this.bnt_Click);
            // 
            // btn14
            // 
            this.btn14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btn14.FlatAppearance.BorderSize = 0;
            this.btn14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn14.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btn14.ForeColor = System.Drawing.Color.White;
            this.btn14.Location = new System.Drawing.Point(647, 198);
            this.btn14.Name = "btn14";
            this.btn14.Size = new System.Drawing.Size(218, 61);
            this.btn14.TabIndex = 15;
            this.btn14.Text = "14 cola";
            this.btn14.UseVisualStyleBackColor = false;
            this.btn14.Click += new System.EventHandler(this.bnt_Click);
            // 
            // btn15
            // 
            this.btn15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btn15.FlatAppearance.BorderSize = 0;
            this.btn15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn15.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btn15.ForeColor = System.Drawing.Color.White;
            this.btn15.Location = new System.Drawing.Point(647, 265);
            this.btn15.Name = "btn15";
            this.btn15.Size = new System.Drawing.Size(218, 61);
            this.btn15.TabIndex = 16;
            this.btn15.Text = "15 cola";
            this.btn15.UseVisualStyleBackColor = false;
            this.btn15.Click += new System.EventHandler(this.bnt_Click);
            // 
            // btn16
            // 
            this.btn16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btn16.FlatAppearance.BorderSize = 0;
            this.btn16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn16.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btn16.ForeColor = System.Drawing.Color.White;
            this.btn16.Location = new System.Drawing.Point(647, 332);
            this.btn16.Name = "btn16";
            this.btn16.Size = new System.Drawing.Size(218, 61);
            this.btn16.TabIndex = 17;
            this.btn16.Text = "16 cola";
            this.btn16.UseVisualStyleBackColor = false;
            this.btn16.Click += new System.EventHandler(this.bnt_Click);
            // 
            // btn17
            // 
            this.btn17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btn17.FlatAppearance.BorderSize = 0;
            this.btn17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn17.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btn17.ForeColor = System.Drawing.Color.White;
            this.btn17.Location = new System.Drawing.Point(647, 399);
            this.btn17.Name = "btn17";
            this.btn17.Size = new System.Drawing.Size(218, 61);
            this.btn17.TabIndex = 18;
            this.btn17.Text = "17 cola";
            this.btn17.UseVisualStyleBackColor = false;
            this.btn17.Click += new System.EventHandler(this.bnt_Click);
            // 
            // btn18
            // 
            this.btn18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btn18.FlatAppearance.BorderSize = 0;
            this.btn18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn18.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btn18.ForeColor = System.Drawing.Color.White;
            this.btn18.Location = new System.Drawing.Point(647, 466);
            this.btn18.Name = "btn18";
            this.btn18.Size = new System.Drawing.Size(218, 61);
            this.btn18.TabIndex = 19;
            this.btn18.Text = "18 cola";
            this.btn18.UseVisualStyleBackColor = false;
            this.btn18.Click += new System.EventHandler(this.bnt_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(877, 560);
            this.Controls.Add(this.btn18);
            this.Controls.Add(this.btn17);
            this.Controls.Add(this.btn16);
            this.Controls.Add(this.btn15);
            this.Controls.Add(this.btn14);
            this.Controls.Add(this.btn13);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.labelMasina);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.labelClock);
            this.Controls.Add(this.xLabel5);
            this.Controls.Add(this.xLabel4);
            this.Controls.Add(this.xLabel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FormMain";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormMain_KeyPress);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private DesignLibrary.XLabel xLabel3;
        private DesignLibrary.XLabel xLabel4;
        private DesignLibrary.XLabel xLabel5;
        private DesignLibrary.XLabel labelClock;
        private System.Windows.Forms.Timer timer1;
        private DesignLibrary.XLabel labelVreme;
        private DesignLibrary.XLabel labelIme;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private DesignLibrary.XLabel labelMasina;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelYellow;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private DesignLibrary.XButtonMetro btn13;
        private DesignLibrary.XButtonMetro btn14;
        private DesignLibrary.XButtonMetro btn15;
        private DesignLibrary.XButtonMetro btn16;
        private DesignLibrary.XButtonMetro btn17;
        private DesignLibrary.XButtonMetro btn18;



    }
}

