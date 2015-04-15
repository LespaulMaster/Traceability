using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
namespace DesignLibrary
{
    public partial class XButtonMetro : Button
    {
        public XButtonMetro()
        {
            InitializeComponent();
        }

        public XButtonMetro(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
