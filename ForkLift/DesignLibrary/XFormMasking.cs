using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesignLibrary
{
    public partial class XFormMasking : Form
    {
        Form frontForm;
        public XFormMasking(Form front)
        {
            InitializeComponent();
            frontForm = front;
            
            
        }

        private void XFormMasking_Load(object sender, EventArgs e)
        {
            DialogResult result = frontForm.ShowDialog();
            
            if ( result == DialogResult.Cancel)
            {
                 this.DialogResult = DialogResult.Cancel;

            }
            else if (result == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;

            }
        }

        public void closeOK()
        {
            this.DialogResult=DialogResult.OK;
        }
    }
}
