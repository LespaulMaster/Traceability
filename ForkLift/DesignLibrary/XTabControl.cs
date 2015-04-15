using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DesignLibrary
{
    public partial class XTabControl : TabControl
    {
        public XTabControl()
        {
            InitializeComponent();

            this.Font = new Font("Microsoft Sans Serif", 12);

            foreach (TabPage tp in this.TabPages)
            {
                tp.BackColor = Color.FromArgb(0, 98, 209);

                tp.Text = "Novi text";
            }
        }
    }
}
