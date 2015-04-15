using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DesignLibrary;
using BussinesLogicLayer;

namespace Terminal
{
    public partial class Terminal : FormTerminal
    {
        String barcode = "";
        bool greska = false;


        public Terminal()
        {
            InitializeComponent();
            label_time.Text = DateTime.Now.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            label_time.Text = dt.ToString("HH : mm");
        }

        private void Terminal_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (greska)
                {
                    textBox_information.Visible = true;
                    textBox1.Visible = false;
                    panel_main.BackColor = Color.FromArgb(54, 54, 54);
                    greska = false;
                }
                if (e.KeyChar == 13)
                {
                    int stanje = TerminalPanel.Instance.parseInputString(barcode);
                    switch (stanje)
                    {
                        case TerminalPanel.State.LOGGED:
                            string imeRadnika = TerminalPanel.Instance.radnik.Ime.Trim();
                            string prezimeRadnika = TerminalPanel.Instance.radnik.Prezime.Trim();
                            xLabel_Radnik.Text = imeRadnika + " " + prezimeRadnika;
                            xLabel_Masina.Text = TerminalPanel.Instance.masina.SifraMasine.Trim();
                            //xLabel_Propis.Text = TerminalPanel.Instance.propis.Sifra.Trim();
                            barcode = "";
                            textBox_information.Text = "Prijavljen je radnik " + imeRadnika + " " + prezimeRadnika;;
                            break;
                        case TerminalPanel.State.PRIJAVLJEN_PROPIS:
                            string prijavljeniPropis = TerminalPanel.Instance.propis.Sifra.Trim();
                            xLabel_Propis.Text = prijavljeniPropis;
                            barcode = "";
                            textBox_information.Text = "Prijavljen je propis " + prijavljeniPropis;
                            break;
                        case TerminalPanel.State.PRIJAVLJEN_IZLAZNI_REGAL:
                            string prijavljeniRegal = TerminalPanel.Instance.regalOut.Opis.Trim() + " " +
                                TerminalPanel.Instance.regalOut.Colaza + " cola";
                            xLabel_Regal.Text = prijavljeniRegal;
                            barcode = "";
                            textBox_information.Text = "Prijavljen je " + prijavljeniRegal;
                            break;
                        case TerminalPanel.State.NOVI_PROIZVOD:
                            textBox_information.Text = "Kreiran je proizvod sa bar-kodom " + barcode;
                            barcode = "";
                            break;
                        case TerminalPanel.State.NOT_LOGGED:
                            textBox_information.Text = "Radnik je odjavljen";
                            xLabel_Radnik.Text = "";
                            barcode = "";
                            break;
                        default:
                            xLabel1.Text = "Pogresan bar-kod, pokusajte ponovo";
                            barcode = "";
                            break;
                    }

                    textBox_information.Font = new Font("Segoue UI", 48);

                    Size size = TextRenderer.MeasureText(textBox_information.Text, textBox_information.Font);
                    textBox_information.Height = (size.Width / textBox_information.Width + 1) * size.Height + 15;
                    textBox_information.Width = size.Width;

                }
                else
                    if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 90)
                        || (e.KeyChar >= 97 && e.KeyChar <= 122))
                    {
                        barcode += e.KeyChar;
                    }
                    else
                        if (e.KeyChar == 8)
                        {
                            barcode = barcode.Substring(0, barcode.Length - 2);
                        }
            }
            catch (BussinesLogicLayer.RackException rackException)
            {
                ExecuteExceptions(rackException.Message);
            }
            catch (BussinesLogicLayer.PropisNotFoundException propisException)
            {
                ExecuteExceptions(propisException.Message);
            }
            catch (BussinesLogicLayer.TyreNotFoundException tyreException)
            {
                ExecuteExceptions(tyreException.Message);
            }
            catch (BussinesLogicLayer.RadnikException radnikException)
            {
                ExecuteExceptions(radnikException.Message);
            }
            catch (BussinesLogicLayer.defaultException defaultException)
            {
                ExecuteExceptions(defaultException.Message);
            }
        }

        public void ExecuteExceptions(string ExceptionMessage)
        {
            textBox_information.Text = "";

            barcode = "";

            panel_main.BackColor = Color.FromArgb(218, 83, 44);

            textBox1.BackColor = Color.FromArgb(218, 83, 44);
            textBox1.ForeColor = Color.White;
            textBox1.Font = new Font("Segoue UI", 48);

            textBox1.Text = ExceptionMessage;
            textBox1.Visible = true;
            textBox_information.Visible = false;

            Size size = TextRenderer.MeasureText(textBox1.Text, textBox1.Font);

            textBox1.Height = (size.Width / textBox1.Width + 1) * size.Height + 15;
            textBox1.Width = size.Width;

            greska = true;
        }

        private void Terminal_Load(object sender, EventArgs e)
        {
            TerminalPanel.Instance.InitialLoad();
        }
    }
}
