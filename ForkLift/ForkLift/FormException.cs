using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ForkLift
{
    public partial class FormException : Form
    {
        
        public FormException(String exceptionText, bool shouldClose)
        {
            
            InitializeComponent();
            xLabelException.Text = exceptionText;
            this.Width = Screen.FromControl(this).Bounds.Width;
            this.xLabelException.Width = this.Width - 20;
            this.timer1.Enabled = shouldClose;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
