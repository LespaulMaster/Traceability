using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DesignLibrary;
using System.Threading;
using BussinesLogicLayer;
using BussinesLogicLayer.localhost;
namespace ForkLift
{
    public partial class FormMain : Form
    {
       
        private String barcode;//za cuvanje ocitanog bar koda
        private bool isOffline = false;//oznacava da li je trenutna forma offline ili online
                                        //razlikuje se od stanja celog uredjaja koje se nalazi u klasi TerminalForkLift

        List<XZahtev> xzahtevi;//lista zahteva koji se prikazuju
        List<XZahtev> yellowZahtevi;//lista zahteva za prevozenje u zutu zonu

        
        FormSelectZahtev formSelectZahtev;//forma koja se prikazuje kada se selektuje zahtev, postavljena kao globalna
                                            // da bi mogla da se ugasi iz svih delova koda
        FormManual formManual; //forma koja prikazuje manualni mod
        public FormMain()
        {
            InitializeComponent();
            TerminalForkLift.Instance.InitialLoad();//pribavljanje IP adrese i ostalih elemenata
            
            updateView();//ucitava text boxove u formi
            
        }

        private void updateView()
        {
            labelIme.Text = TerminalForkLift.Instance.radnik.Ime + " " + TerminalForkLift.Instance.radnik.Prezime;

            labelMasina.Text = TerminalForkLift.Instance.masina.SifraMasine;
        }

        //funkcija vrsi update svih zahteva
        public void updateZahtevi()
        {
            this.xzahtevi = getAllZahtev();
            this.yellowZahtevi = getYellowZoned();
            showAllZahtev(xzahtevi);
            showYellowZoned(yellowZahtevi);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.xzahtevi = getAllZahtev();
            showAllZahtev(xzahtevi);
            this.yellowZahtevi = getYellowZoned();
            showYellowZoned(yellowZahtevi);
            labelClock.Text = DateTime.Now.ToShortTimeString();

            //TODO funkcija za proveru prijavljenog korisnika
            
            labelVreme.Text = DateTime.Now.ToShortTimeString();
   
        }

    
        //prikaz svih zahteva za prevoz
        private void showAllZahtev(List<XZahtev> zahtevi)
        {
            this.flowLayoutPanel.Controls.Clear();
            foreach (XZahtev zahtev in zahtevi)
            {
                this.flowLayoutPanel.Controls.Add(zahtev);
                zahtev.Width = flowLayoutPanel.Width;
            }
        }

        //prikaz svih zahteva za prevoz u zutu zonu
        private void showYellowZoned(List<XZahtev> yellowZahtevi)
        {
            this.flowLayoutPanelYellow.Controls.Clear();
            foreach (XZahtev zahtev in yellowZahtevi)
            {
                this.flowLayoutPanelYellow.Controls.Add(zahtev);
                zahtev.setColorYellow();
                zahtev.Width = flowLayoutPanelYellow.Width;
            }
        }

        //tajmer za osvezavanje vremena na prikazu terminala
        private void timer1_Tick(object sender, EventArgs e)
        {
            labelClock.Text = DateTime.Now.ToShortTimeString();
        }

        //prilikom izbora zahteva iz liste zahteva
        private void selectZahtev(object sender, EventArgs e)
        {
            XZahtev zahtev = (XZahtev)sender;
          
            TerminalForkLift.Instance.selectedZahtev = TerminalForkLift.Instance.listaZahteva[zahtev.indexOfZahtev];
            TerminalForkLift.Instance.inicijalniZahtev = TerminalForkLift.clone(TerminalForkLift.Instance.selectedZahtev);
            
            selectZahtev();
            

        }

        private void selectZahtev()
        {
            int selectStatus = TerminalForkLift.Instance.izaberiZahtev(TerminalForkLift.Instance.selectedZahtev);
            if (selectStatus==1)
            {
                formSelectZahtev = new FormSelectZahtev();

                XFormMasking maskingForm = new XFormMasking(formSelectZahtev);

                DialogResult result = maskingForm.ShowDialog();

                
                if (result == DialogResult.Cancel)
                {
                    TerminalForkLift.Instance.cancelZahtev();
                    formSelectZahtev = null;
                }
                else if (result == DialogResult.OK)
                {
                    this.xzahtevi = getAllZahtev();
                    showAllZahtev(xzahtevi);
                    formSelectZahtev = null;

                }
            }
                //potrebno selektovati tip zahteva
            else if(selectStatus==0)
            {
                FormSelectTipRegala selectTipRegala = new FormSelectTipRegala();

                XFormMasking maskingForm = new XFormMasking(selectTipRegala);

                DialogResult result = maskingForm.ShowDialog();

                
                if (result == DialogResult.Cancel)
                {
                    TerminalForkLift.Instance.cancelZahtev();
                    formSelectZahtev = null;
                }
                else if (result == DialogResult.OK)
                {
                    
                    
                    selectZahtev();
                }

            }
            else
            {
                TerminalForkLift.Instance.selectedZahtev = null;
                CustomFunctions.showError("Nemoguce izabrati zahtev, probajte drugi",this);
                this.xzahtevi = getAllZahtev();
                showAllZahtev(xzahtevi);
            }
        }

        private void selectYellowZoned(object sender, EventArgs e)
        {


            XZahtev zahtev = (XZahtev)sender;
            TerminalForkLift.Instance.selectedZahtev = TerminalForkLift.Instance.listaZutaZona[zahtev.indexOfZahtev];
            if (TerminalForkLift.Instance.izaberiZahtev(TerminalForkLift.Instance.listaZutaZona[zahtev.indexOfZahtev])==1)
            {

                //TODO promeniti prosledjivanje teksta, potrebno je definisati pocetnu poziciju i krajnju poziciju
                formSelectZahtev = new FormSelectZahtev();

                XFormMasking maskingForm = new XFormMasking(formSelectZahtev);

                DialogResult result = maskingForm.ShowDialog();

                
                if (result == DialogResult.Cancel)
                {
                    formSelectZahtev = null;
                    TerminalForkLift.Instance.cancelZahtev();
                }
                else if (result == DialogResult.OK)
                {

                    formSelectZahtev = null;
                    
                    this.xzahtevi = getAllZahtev();
                    showAllZahtev(xzahtevi);

                }
            }
            else
            {
                TerminalForkLift.Instance.selectedZahtev = null;
                MessageBox.Show("Zahtev je već izabran od strane drugog operatera");
                this.xzahtevi = getAllZahtev();
                showAllZahtev(xzahtevi);
            }

        }

        public void closingMessageForm(object sender, EventArgs e)
        {
            this.Show();
        }

        private void FormMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            //prilikom RETURN karaktera, obradjuje se otkucani bar kod
            if (e.KeyChar == 13)
            {
                try
                {
                    int stanje = TerminalForkLift.Instance.parseInputString(barcode);
                    if (stanje == TerminalForkLift.State.NOT_LOGGED)
                        Application.Restart();
                    else if (stanje == TerminalForkLift.State.IN_PROCESS || stanje == TerminalForkLift.State.LEFT_PRODS)
                    {
                           
                        formSelectZahtev = new FormSelectZahtev();

                        XFormMasking maskingForm = new XFormMasking(formSelectZahtev);

                        DialogResult result = maskingForm.ShowDialog();

                        
                        if (result == DialogResult.Cancel)
                        {
                            maskingForm = null;
                            TerminalForkLift.Instance.cancelZahtev();
                        }
                        else if (result == DialogResult.OK)
                        {
                            maskingForm = null;
                            this.xzahtevi = getAllZahtev();
                            showAllZahtev(xzahtevi);
                        }
                    }
                    else if (stanje == TerminalForkLift.State.MANUAL)
                    {
                       formManual = new FormManual();

                       XFormMasking maskingForm = new XFormMasking(formManual);

                        DialogResult result = maskingForm.ShowDialog();
                        formManual = null;
                    }
                    else if (stanje == TerminalForkLift.State.NOT_LOGGED)
                        Application.Restart();
                }
                catch (Exception exception)
                {
                    CustomFunctions.showError(exception.Message, this);
                }
                    barcode = "";
            }
            //Prihvatljivi karakteri
            else if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 90) || (e.KeyChar >= 97 && e.KeyChar <= 122))
            {
                barcode += e.KeyChar;
            }

        }

        //preuzimanje svih zahteva iz baze podataka
        public List<XZahtev> getAllZahtev()
        {
            List<XZahtev> xlistaZahteva = new List<XZahtev>();
            Zahtev[] zahtevi = TerminalForkLift.Instance.getZahtevi();

            for (int i = 0; i < zahtevi.Length; i++) 
            {
                String tekstZahteva = zahtevi[i].TekstZahteva;
                XZahtev xzahtev = new XZahtev(zahtevi[i].NazivMasine, tekstZahteva, "");
                xzahtev.Click += new EventHandler(selectZahtev);
                xzahtev.zahtevID = TerminalForkLift.Instance.listaZahteva[i].ID;
                xzahtev.indexOfZahtev = i;
                xlistaZahteva.Add(xzahtev);
                
            }


            return xlistaZahteva;
        }

        public List<XZahtev> getYellowZoned()
        {
            List<XZahtev> xlistaZahteva = new List<XZahtev>();

            TerminalForkLift.Instance.getYellowZoned();

            for (int i = 0; i < TerminalForkLift.Instance.listaZutaZona.Length; i++)
            {
                XZahtev xzahtev = new XZahtev("", TerminalForkLift.Instance.listaZutaZona[i].TekstZahteva, "");
                xzahtev.Click += new EventHandler(selectYellowZoned);
                xzahtev.zahtevID = TerminalForkLift.Instance.listaZutaZona[i].ID;
                xzahtev.indexOfZahtev = i;
                xlistaZahteva.Add(xzahtev);
            }
            return xlistaZahteva;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TerminalForkLift.Instance.OdjaviRadnika(TerminalForkLift.Instance.radnik.MBR, 0))
                Application.Restart();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.updateZahtevi();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            //provera dostupnosti na mrezi, ukoliko nije online onda se gase sve prednje forme
            
            if (Network.Instance.stanje == Network.State.OFFLINE)
            {
                if (formManual != null)
                    formManual.Close();
                if (formSelectZahtev != null)
                    formSelectZahtev.Close();
                isOffline = true;

            }
            else if (Network.Instance.stanje == Network.State.ONLINE && isOffline)
            {
                if (TerminalForkLift.Instance.stanje == TerminalForkLift.State.MANUAL)
                {
                    formManual = new FormManual();

                    XFormMasking maskingForm = new XFormMasking(formManual);

                    DialogResult result = maskingForm.ShowDialog();
                    formManual = null;
                    isOffline = false;
                }
                else if (TerminalForkLift.Instance.stanje == TerminalForkLift.State.NOT_LOGGED)
                {
                    isOffline = false;
                    Application.Restart();
                }

            }
            
             
        }

        
        private void bnt_Click(object sender, EventArgs e)
        {
            Regal regal = null;
            switch (((Button)sender).Name)
            {

                case "btn13":
                    regal = TerminalForkLift.Instance.getRackByColaza(13, TerminalForkLift.Instance.masina.RJ, "I");
                    break;
                case "btn14":
                    regal = TerminalForkLift.Instance.getRackByColaza(14, TerminalForkLift.Instance.masina.RJ, "I");
                    break;
                case "btn15":
                    regal = TerminalForkLift.Instance.getRackByColaza(15, TerminalForkLift.Instance.masina.RJ,"I");
                    break;
                case "btn16":
                    regal = TerminalForkLift.Instance.getRackByColaza(16, TerminalForkLift.Instance.masina.RJ,"I");
                    break;
                case "btn17":
                    regal = TerminalForkLift.Instance.getRackByColaza(17, TerminalForkLift.Instance.masina.RJ,"I");
                    break;
                case "btn18":
                    regal = TerminalForkLift.Instance.getRackByColaza(18, TerminalForkLift.Instance.masina.RJ,"I");
                    break;
                
            }

            if (regal != null)
            {
                FormFindRegal findRegal = new FormFindRegal(regal);

                XFormMasking maskingForm = new XFormMasking(findRegal);

                DialogResult result = maskingForm.ShowDialog(this);
                maskingForm = null;
                if (result == DialogResult.OK)
                {
                    if (TerminalForkLift.Instance.stanje == TerminalForkLift.State.MANUAL)
                    {
                       formManual = new FormManual();

                        maskingForm = new XFormMasking(formManual);

                        maskingForm.ShowDialog();
                        formManual = null;
                    }
                    else if (TerminalForkLift.Instance.stanje == TerminalForkLift.State.IN_PROCESS)
                    {
                        formSelectZahtev = new FormSelectZahtev();

                        maskingForm = new XFormMasking(formSelectZahtev);

                        result = maskingForm.ShowDialog();

                        if (result == DialogResult.Cancel)
                        {
                            formSelectZahtev = null;
                            TerminalForkLift.Instance.cancelZahtev();
                        }
                        else if (result == DialogResult.OK)
                        {

                            formSelectZahtev = null;

                            this.xzahtevi = getAllZahtev();
                            showAllZahtev(xzahtevi);

                        }
                    }
                }
            }
            else
            {

                CustomFunctions.showError("Trenutno nema slobodnog regala", this);
            }
        }
       
    }

    
}
