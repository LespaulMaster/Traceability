using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BussinesLogicLayer;
using DesignLibrary;

namespace ForkLift
{
    public partial class FormManual : Form
    {
        String barcode;
        public FormManual()
        {
            InitializeComponent();
            this.Width = Screen.FromControl(this).Bounds.Width;
            
        }

        private void FormManual_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    int stanje = TerminalForkLift.Instance.parseInputString(barcode);
                    if (stanje == TerminalForkLift.State.LOGGED)
                    {
                        this.DialogResult = DialogResult.OK;
                        

                    }

                }
                catch (Exception exception)
                {
                    FormException formException = new FormException(exception.Message, true);
                    XFormMasking maskingForm = new XFormMasking(formException);
                    maskingForm = new XFormMasking(formException);
                    maskingForm.ShowDialog(this);
                }
                barcode = "";

            }
            else if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 90) || (e.KeyChar >= 97 && e.KeyChar <= 122))
            {
                barcode += e.KeyChar;
            }
        }
    }
}
