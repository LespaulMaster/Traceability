using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;


namespace DesignLibrary
{
    public partial class XRadioButton : RadioButton
    {
        public XRadioButton()
        {
            InitializeComponent();
        }

        public XRadioButton(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
