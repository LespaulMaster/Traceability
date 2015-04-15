using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BussinesLogicLayer;
using DesignLibrary;
using BussinesLogicLayer.localhost;
namespace ForkLift
{
    public partial class FormFindRegal : Form
    {
        Regal foundRack;

        String barcode;
        public FormFindRegal(Regal rack)
        {
            InitializeComponent();
            this.Width = Screen.FromControl(this).Bounds.Width;

            xLabelTipRegala.Text = rack.Colaza.ToString() + " cola";
            xLabelLokacija.Text = TerminalForkLift.Instance.getPositionName((int)rack.PozicijaID);

            foundRack = rack;
        }

        private void FormFindRegal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    int stanje = TerminalForkLift.Instance.parseInputString(barcode);
                    if (stanje == TerminalForkLift.State.MANUAL || stanje == TerminalForkLift.State.IN_PROCESS)
                    {
                        
                        this.DialogResult = DialogResult.OK;
                        
                    }
                    else 
                    {
                        FormException formException = new FormException("Skenirajte odgovarajući regal ili otkažite zahtev", true);
                        XFormMasking maskingForm = new XFormMasking(formException);
                        maskingForm = new XFormMasking(formException);
                        maskingForm.ShowDialog(this); 
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

        private void xButtonMetro1_Click(object sender, EventArgs e)
        {
            TerminalForkLift.Instance.releaseRegal(foundRack.ID);
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
