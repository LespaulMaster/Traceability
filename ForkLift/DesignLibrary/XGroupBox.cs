using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace DesignLibrary
{
	public partial class XGroupBox: GroupBox
	{	
		public XGroupBox()
		{
			InitializeComponent();
		}

		public XGroupBox(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
            this.Font = new System.Drawing.Font("Segoe UI", 10);
		}
	}
}
