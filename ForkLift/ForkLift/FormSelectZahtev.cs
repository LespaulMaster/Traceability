using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BussinesLogicLayer;
using BussinesLogicLayer.localhost;
using DesignLibrary;

namespace ForkLift
{
    public partial class FormSelectZahtev : Form
    {
        private String barcode;
        int stanje;
        public FormSelectZahtev()
        {
            InitializeComponent();
            
            this.Width = Screen.FromControl(this).Bounds.Width;

            if (TerminalForkLift.Instance.stanje == TerminalForkLift.State.IN_PROCESS)
            {
                showText();
                xButtonMetro1.Visible = false;

            }

            else if (TerminalForkLift.Instance.stanje == TerminalForkLift.State.LEFT_PRODS)
            {
                FormLeftProdInPalet leftProds = new FormLeftProdInPalet();

                XFormMasking maskingForm = new XFormMasking(leftProds);
                maskingForm = new XFormMasking(leftProds);
                if (maskingForm.ShowDialog() == DialogResult.OK)
                {
                    showText();
                    xButtonMetro1.Visible = false;
                }

            }
            else if (TerminalForkLift.Instance.stanje == TerminalForkLift.State.CHOOSEN)
                showText();
}

        private void showText()
        {
            //pozicija do koje se vozi
            String posDO;
            String posOD;
            String nazivRegala;

            nazivRegala = TerminalForkLift.Instance.getZahtevStrings(out posOD, out posDO);

            xLabelRegal.Text = nazivRegala;

            if (posDO != null)
            {
                xLabelDO.Text = posDO;
                xLabel2.Visible = true;
                xLabelDO.Visible = true;
            }
            else
            {
                xLabel2.Visible = false;
                xLabelDO.Visible = false;
            }

            
            //pozicija sa koje se vozi
            
            if (posOD != null)
            {
                xLabelOD.Text = posOD;
                xLabel1.Visible = true;
                xLabelOD.Visible = true;

            }
            else
            {
                xLabel1.Visible = false;
                xLabelOD.Visible = false;
            }
            


            
        }

        private void FormSelectZahtev_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    try
                    {
                        stanje = TerminalForkLift.Instance.parseInputString(barcode);
                    }
                    catch (ChangedZahtevException exception)
                    {
                        CustomFunctions.showError(exception.Message, this);
                    }
                    if (stanje == TerminalForkLift.State.IN_PROCESS)
                    {
                        showText();
                        xButtonMetro1.Visible = false;
                    }
                    else if (stanje == TerminalForkLift.State.LEFT_PRODS)
                    {
                        FormLeftProdInPalet leftProds = new FormLeftProdInPalet();

                        XFormMasking maskingForm = new XFormMasking(leftProds);
                        maskingForm = new XFormMasking(leftProds);
                        if (maskingForm.ShowDialog() == DialogResult.OK)
                        {
                            showText();
                            xButtonMetro1.Visible = false;
                        }

                    }
                    else if (stanje == TerminalForkLift.State.LOGGED)
                    {
                        this.DialogResult = DialogResult.OK;
                    }

                    //TODO dodaj akciju koja salje zahtev ka bazi podataka
                    
                }
                catch (RackException exception)
                {
                    FormException formException = new FormException(exception.Message,true);
                    XFormMasking maskingForm = new XFormMasking(formException);
                    maskingForm = new XFormMasking(formException);
                    maskingForm.ShowDialog(this);
                    
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                   
                }
                finally
                {
                    barcode = "";
                }
                
            }
            else if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 90) || (e.KeyChar >= 97 && e.KeyChar <= 122))
            {
                barcode += e.KeyChar;
            }
        }

       
      

        
        

       
       
    }

    



}
