using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BussinesLogicLayer.localhost;
using BussinesLogicLayer;
using DesignLibrary;

namespace ForkLift
{
    public partial class FormSelectTipRegala : Form
    {
        public FormSelectTipRegala()
        {
            InitializeComponent();
        }

        private void btn13_Click(object sender, EventArgs e)
        {
            
            switch (((Button)sender).Name)
            {

                case "btn13":
                    TerminalForkLift.Instance.selectedZahtev.Colaza = 13;
                    break;
                case "btn14":
                    TerminalForkLift.Instance.selectedZahtev.Colaza = 14;
                    break;
                case "btn15":
                    TerminalForkLift.Instance.selectedZahtev.Colaza = 15;
                    break;
                case "btn16":
                    TerminalForkLift.Instance.selectedZahtev.Colaza = 16;
                    break;
                case "btn17":
                    TerminalForkLift.Instance.selectedZahtev.Colaza = 17;
                    break;
                case "btn18":
                    TerminalForkLift.Instance.selectedZahtev.Colaza = 18;
                    break;
                
            }
            try
            {
                if (TerminalForkLift.Instance.updateZahtev(TerminalForkLift.Instance.selectedZahtev))
                    this.DialogResult = DialogResult.OK;
                else
                {
                    CustomFunctions.showError("Greška pri ažuriranju, pokusajte ponovo",this);
                }
            }
            catch (Exception exeption)
            {
                CustomFunctions.showError(exeption.Message, this);
            }
        
        }

        private void xButtonMetro1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        
    }
}
