using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BussinesLogicLayer;

namespace AdministrationApp
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Elysium.Controls.Window
    {
        // sekcija Radnik
        BussinesLogicLayer.localhost.Radnik radnik;

        //sekcija Propis
        int RJChecked = 1;
        List<BussinesLogicLayer.localhost.Propis> listaPoluproizvodaDodaj = new List<BussinesLogicLayer.localhost.Propis>();
        List<BussinesLogicLayer.localhost.Propis> listaPoluproizvodaPromeni = new List<BussinesLogicLayer.localhost.Propis>();
        List<BussinesLogicLayer.localhost.Propis> listaPoluproizvodaBrisanje = new List<BussinesLogicLayer.localhost.Propis>();
        
        List<BussinesLogicLayer.localhost.Propis> listaPropisa = new List<BussinesLogicLayer.localhost.Propis>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void textBox_RadnikPromenaMBR_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (button_RadnikPromeni.IsEnabled == true)
                button_RadnikPromeni.IsEnabled = false;
            if (textBox_RadnikPromenaIme.IsEnabled == true)
                textBox_RadnikPromenaIme.IsEnabled = false;
            if (label_RadnikPromenaPrezime.IsEnabled == true)
                textBox_RadnikPromenaPrezime.IsEnabled = false;
            if (textBox_RadnikPromenaOJ.IsEnabled == true)
                textBox_RadnikPromenaOJ.IsEnabled = false;
            if (textBox_RadnikPromenaNazivRM.IsEnabled == true)
                textBox_RadnikPromenaNazivRM.IsEnabled = false;
            if (textBox_RadnikPromenaSifraRM.IsEnabled == true)
                textBox_RadnikPromenaSifraRM.IsEnabled = false;
            
            TextBox tb = (TextBox)sender;
            textBox_TextChanged(tb, label_RadnikPromenaMBRGreska);     
        }

        private void textBox_RadnikPromenaSifraRM_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            textBox_TextChanged(tb, label_RadnikPromenaSRMGreska);
        }

        private void textBox_RadnikBrisanjeMBR_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (button_RadnikObrisi.IsEnabled == true)
                button_RadnikObrisi.IsEnabled = false;

            TextBox tb = (TextBox)sender;
            textBox_TextChanged(tb, label_RadnikBrisanjeMBRGreska);
        }

        private void textBox_TextChanged(TextBox tb, Label label)
        {
            char lastChar = new Char();
            string text = tb.Text;
            if (text != "")
                lastChar = text.Last();

            if (lastChar != 0)
            {
                if (lastChar < '0' || lastChar > '9')
                {
                    tb.Text = text.Substring(0, text.Length - 1);
                    label.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    if (label.IsVisible)
                    {
                        label.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }
        }

        private void button_RadnikPromeni_Click(object sender, RoutedEventArgs e)
        {
            int QV = Int32.Parse(textBox_RadnikPromenaMBR.Text);

            if (radnik.MBR == QV)
            {
                int SifraRM = new Int32();
                string NazivRM = "";

                string Ime = textBox_RadnikPromenaIme.Text;
                string Prezime = textBox_RadnikPromenaPrezime.Text;
                int OJ = Int32.Parse(textBox_RadnikPromenaOJ.Text);
                if (textBox_RadnikPromenaSifraRM.Text != "")
                    SifraRM = Int32.Parse(textBox_RadnikPromenaSifraRM.Text);
                if (textBox_RadnikPromenaNazivRM.Text != "")
                    NazivRM = textBox_RadnikPromenaNazivRM.Text;

                bool pol;
                if (radioButton_RadnikPromenaMuski.IsChecked == true)
                    pol = true;
                else
                    pol = false;

                AdministrationClass.Instance.updateRadnik(radnik.ID, QV, Ime, Prezime, OJ, NazivRM, SifraRM, pol);
                showSuccessMessage("Uspesno ste promenili podatke o radniku" + Ime + " " + Prezime);
            }
            else
            {
                showFailureMessage("Greska : Promena QV radnika");
                
            }
        }

        #region PronadjiRadnik
        private void button_RadnikPromenaPronadji_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool pronadjen = 
                    RadnikPronadji(textBox_RadnikPromenaMBR, textBox_RadnikPromenaIme, textBox_RadnikPromenaPrezime, textBox_RadnikPromenaOJ,
                    textBox_RadnikPromenaSifraRM, textBox_RadnikPromenaNazivRM, radioButton_RadnikPromenaMuski, radioButton_RadnikPromenaZenski);

                if (pronadjen)
                {
                    button_RadnikPromeni.IsEnabled = true;
                    textBox_RadnikPromenaIme.IsEnabled = true;
                    textBox_RadnikPromenaPrezime.IsEnabled = true;
                    textBox_RadnikPromenaSifraRM.IsEnabled = true;
                    textBox_RadnikPromenaNazivRM.IsEnabled = true;
                    textBox_RadnikPromenaOJ.IsEnabled = true;
                }
            }
            catch (RadnikException radnikException)
            {
                showFailureMessage(radnikException.Message);
            }
        }

        private void button_RadnikBrisanjePronadji_Click(object sender, RoutedEventArgs e)
        {
            bool pronadjen =
                RadnikPronadji(textBox_RadnikBrisanjeMBR, textBox_RadnikBrisanjeIme, textBox_RadnikBrisanjePrezime, textBox_RadnikBrisanjeOJ,
                textBox_RadnikBrisanjeSifraRM, textBox_RadnikBrisanjeNazivRM, radioButton_RadnikBrisanjeMuski, radioButton_RadnikBrisanjeZenski);

            if (pronadjen)
                button_RadnikObrisi.IsEnabled = true;
        }

        private bool RadnikPronadji
            (TextBox tbQV, TextBox tbIme, TextBox tbPrezime, TextBox tbOJ, 
            TextBox tbSifraRM, TextBox tbNazivRM, RadioButton rbMuski, RadioButton rbZenski)
        {
            radnik = AdministrationClass.Instance.selectRadnik(Int32.Parse(tbQV.Text));
            if (radnik == null)
            {
                showFailureMessage("Radnik sa zadatim maticnim brojem nije pornadjen u bazi podataka");
                return false;
            }

            tbIme.Text = radnik.Ime;
            tbPrezime.Text = radnik.Prezime;
            tbOJ.Text = radnik.OJ.ToString();
            tbSifraRM.Text = radnik.Sifra_rad_mesta.ToString();
            tbNazivRM.Text = radnik.Naziv_rad_mesta;

            if (radnik.Pol == "m")
                rbMuski.IsChecked = true;
            else if (radnik.Pol == "z")
                rbZenski.IsChecked = true;

            return true;
        }

        #endregion

        private void button_RadnikDodaj_Click(object sender, RoutedEventArgs e)
        {
            label_RadnikDodavanjeImeGreska.Visibility = System.Windows.Visibility.Hidden;
            label_RadnikDodavanjeMBRGreska.Visibility = System.Windows.Visibility.Hidden;
            label_RadnikDodavanjeOJGreska.Visibility = System.Windows.Visibility.Hidden;
            label_RadnikDodavanjePrezimeGreska.Visibility = System.Windows.Visibility.Hidden;

            if (textBox_RadnikDodavanjeMBR.Text != "" && textBox_RadnikDodavanjeIme.Text != ""
                && textBox_RadnikDodavanjePrezime.Text != "" && textBox_RadnikDodavanjeOJ.Text != "")
            {
                int QV = Int32.Parse(textBox_RadnikDodavanjeMBR.Text.Trim());
                radnik = AdministrationClass.Instance.selectRadnik(QV);

                if (radnik == null)
                {
                    int SifraRM = new Int32();
                    string NazivRM = "";

                    string Ime = textBox_RadnikDodavanjeIme.Text;
                    string Prezime = textBox_RadnikDodavanjePrezime.Text;
                    int OJ = Int32.Parse(textBox_RadnikDodavanjeOJ.Text);
                    if (textBox_RadnikDodavanjeSifraRM.Text != "")
                        SifraRM = Int32.Parse(textBox_RadnikDodavanjeSifraRM.Text);
                    if (textBox_RadnikDodavanjeNazivRM.Text != "")
                        NazivRM = textBox_RadnikDodavanjeNazivRM.Text;

                    bool pol;
                    if (radioButton_RadnikDodavanjeMuski.IsChecked == true)
                        pol = true;
                    else
                        pol = false;

                    AdministrationClass.Instance.addRadnik(QV, Ime, Prezime, OJ, NazivRM, SifraRM, pol);

                    showSuccessMessage("Radnik :  " + QV + "- " + Ime + " " + Prezime + " je uspesno dodat");
                }
                else
                {
                    showFailureMessage("zadati radnik vec postoji u bazi");
                }
            }
            else 
            {
                if (textBox_RadnikDodavanjeMBR.Text == "")
                    label_RadnikDodavanjeMBRGreska.Visibility = System.Windows.Visibility.Visible;
                if (textBox_RadnikDodavanjeIme.Text == "")
                    label_RadnikDodavanjeImeGreska.Visibility = System.Windows.Visibility.Visible;
                if (textBox_RadnikDodavanjePrezime.Text == "")
                    label_RadnikDodavanjePrezimeGreska.Visibility = System.Windows.Visibility.Visible;
                if (textBox_RadnikDodavanjeOJ.Text == "")
                    label_RadnikDodavanjeOJGreska.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button_RadnikObrisi_Click(object sender, RoutedEventArgs e)
        {
            AdministrationClass.Instance.deleteRadnik(Int32.Parse(textBox_RadnikBrisanjeMBR.Text));

            textBox_RadnikBrisanjeMBR.Text = "";
            textBox_RadnikBrisanjeIme.Text = "";
            textBox_RadnikBrisanjePrezime.Text = "";
            textBox_RadnikBrisanjeOJ.Text = "";
            textBox_RadnikBrisanjeSifraRM.Text = "";
            textBox_RadnikBrisanjeNazivRM.Text = "";
            radioButton_RadnikBrisanjeMuski.IsChecked = false;
            radioButton_RadnikBrisanjeZenski.IsChecked = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        #region Promena Radne Jedinice
        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            RJChecked = RadnaJedinica.VALJARA;
            ChangeRadnaJedinica();
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            RJChecked = RadnaJedinica.PRERADA;
            ChangeRadnaJedinica();
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            RJChecked = RadnaJedinica.KONFEKCIJA;
            ChangeRadnaJedinica();
        }

        private void radioButton4_Checked(object sender, RoutedEventArgs e)
        {
            RJChecked = RadnaJedinica.VULKANIZACIJA;
            ChangeRadnaJedinica();
        }
        #endregion

        private void ChangeRadnaJedinica()
        {
            if (comboBox_PropisTipMasine != null && comboBox_PropisTipMasine.HasItems)
                comboBox_PropisTipMasine.Items.Clear();
            if (comboBox_PropisPromenaTipMasine != null && comboBox_PropisPromenaTipMasine.HasItems)
                comboBox_PropisPromenaTipMasine.Items.Clear();
            if (comboBox_PropisPoluproizvod != null && comboBox_PropisPoluproizvod.HasItems)
                comboBox_PropisPoluproizvod.Items.Clear();
            if (comboBox_PropisPromenaPoluproizvod != null && comboBox_PropisPromenaPoluproizvod.HasItems)
                comboBox_PropisPromenaPoluproizvod.Items.Clear();

            AdministrationClass.Instance.ChangeRadnaJedinica(RJChecked);

            foreach (String tm in AdministrationClass.Instance.MasinaTipList)
            {
                comboBox_PropisTipMasine.Items.Add(tm);
                comboBox_PropisPromenaTipMasine.Items.Add(tm);
            }
            foreach (BussinesLogicLayer.localhost.Propis pr in AdministrationClass.Instance.PoluproizvodList)
            {
                comboBox_PropisPoluproizvod.Items.Add(pr.Sifra + " ; opis :" + pr.Opis + " ; DK:" + pr.DatumKreiranja.ToString());
                comboBox_PropisPromenaPoluproizvod.Items.Add(pr.Sifra + " ; opis :" + pr.Opis + " ; DK:" + pr.DatumKreiranja.ToString());
            }
        }

        private void button_PropisPoluproizvodDodaj_Click(object sender, RoutedEventArgs e)
        {
            int selektovan = comboBox_PropisPoluproizvod.SelectedIndex;
            BussinesLogicLayer.localhost.Propis propis = AdministrationClass.Instance.PoluproizvodList[selektovan];

            popuniListuPoluproizvoda(propis, listView_PropisDodavanje, listaPoluproizvodaDodaj);
        }

        private void button_PropisPromenaPoluproizvodDodaj_Click(object sender, RoutedEventArgs e)
        {
            int selektovan = comboBox_PropisPromenaPoluproizvod.SelectedIndex;
            BussinesLogicLayer.localhost.Propis propis = AdministrationClass.Instance.PoluproizvodList[selektovan];

            popuniListuPoluproizvoda(propis, listView_PropisPromenaDodavanje, listaPoluproizvodaPromeni);
        }

        private void popuniListuPoluproizvoda
            (BussinesLogicLayer.localhost.Propis propis, ListView lv, List<BussinesLogicLayer.localhost.Propis> listaPoluproizvoda)
        {
            Grid gridTmp = new Grid();
            gridTmp.Width = 275;
            gridTmp.Height = 40;

            TextBlock tb = new TextBlock();
            tb.Foreground = new SolidColorBrush(Colors.White);
            tb.Text = propis.Sifra + " __ " + propis.Opis;
            tb.Margin = new Thickness(10, 10, 0, 0); 

            Separator sep = new Separator();
            sep.Width = gridTmp.Width;
            sep.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

            gridTmp.Children.Add(tb);
            
            if (!tabItem_PropisBrisanje.IsSelected)
            {
                Button removeButton = new Button();
                removeButton.Content = "remove";
                if (tabItem_PropisDodavanje.IsSelected)
                    removeButton.Name = "button_PropisPoluproizvodDodavanjeRemove" + (listaPoluproizvoda.Count + 1);
                else if (tabItem_PropisPromena.IsSelected)
                    removeButton.Name = "button_PropisPoluproizvodPromenaRemove" + (listaPoluproizvoda.Count + 1);
                removeButton.Margin = new Thickness(0, 5, 20, 0);
                removeButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                removeButton.Height = 30;
                removeButton.Click += removeButton_Clicked;
                gridTmp.Children.Add(removeButton);
            }
            
            gridTmp.Children.Add(sep);

            lv.Items.Add(gridTmp);

            listaPoluproizvoda.Add(propis);
        }

        private void removeButton_Clicked(object sender, EventArgs e)
        {
            Button bTmp = (Button)sender;
            ListView lv = new ListView();

            if (tabControl_Propis.SelectedIndex == 0)
                lv = listView_PropisDodavanje;
            else if (tabControl_Propis.SelectedIndex == 1)
                lv = listView_PropisPromenaDodavanje;
            
            lv.Items.Remove(bTmp.Parent);

            string tmpString = bTmp.Name;
            char lastChar = bTmp.Name.Last();
            int rednibroj = Int32.Parse(lastChar.ToString());

            if (tabItem_PropisDodavanje.IsSelected)
                listaPoluproizvodaDodaj.RemoveAt(rednibroj - 1);
            else if (tabItem_PropisPromena.IsSelected)
                listaPoluproizvodaPromeni.RemoveAt(rednibroj - 1);
        }

        private void button_PropisDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_PropisTipMasine.SelectedIndex != -1)
            {
                if (textBox_PropisSifra.Text != "")
                {
                    String sifraPropisa = textBox_PropisSifra.Text;
                    String masinaTip = comboBox_PropisTipMasine.SelectedItem.ToString();
                    String opis = textBox_PropisOpis.Text;

                    if(tabItem_PropisDodavanje.IsSelected)
                        AdministrationClass.Instance.addPropis(sifraPropisa, opis, masinaTip, RJChecked, listaPoluproizvodaDodaj);

                    showSuccessMessage("Uspesno ste kreirali novi proizvod");
                }
                else
                {
                    showFailureMessage("Upisite naziv propisa koji dodajete");
                    
                }
            }
            else
            {
                showFailureMessage("Morate selektovati Tip Masine za koju dodajete propis");
                
            }
        }

        private void button_PromenaPronadjiPropis_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl_Propis.SelectedIndex == 1)
            {
                string sifraPropisa = textBox_PropisPromenaSifra.Text;

                if (sifraPropisa != "")
                {
                    try
                    {
                        AdministrationClass.Instance.findPropis(sifraPropisa, RJChecked);
                        textBox_PropisPromenaOpis.Text = AdministrationClass.Instance.propisFound.Opis;

                        int index = -1;
                        for (int i = 0; i < AdministrationClass.Instance.MasinaTipList.Length; i++)
                        {
                            if (AdministrationClass.Instance.MasinaTipList[i] == AdministrationClass.Instance.propisFound.TipMasine)
                                index = i;
                        }
                        comboBox_PropisPromenaTipMasine.SelectedIndex = index;

                        foreach (BussinesLogicLayer.localhost.Propis p in AdministrationClass.Instance.pList)
                        {
                            popuniListuPoluproizvoda(p, listView_PropisPromenaDodavanje, listaPoluproizvodaPromeni);
                        }
                    }
                    catch (BussinesLogicLayer.PropisNotFoundException propisException)
                    {
                        textBox_PropisPromenaOpis.Text = "";
                        textBox_PropisPromenaSifra.Text = "";
                        listView_PropisPromenaDodavanje.Items.Clear();
                        showFailureMessage(propisException.Message);                   
                    }
                }
                else
                    showFailureMessage("Niste uneli sifru propisa za pretrazivanje");    
            }
            else
            {
                comboBox_PropisBrisanjeVerzija.Items.Clear();
                string sifraPropisa = textBox_PropisBrisanjeSifra.Text;

                if (sifraPropisa != "")
                {
                    try
                    {
                        listaPropisa = AdministrationClass.Instance.findPropisList(sifraPropisa, RJChecked).ToList();

                        foreach (BussinesLogicLayer.localhost.Propis pr in listaPropisa)
                        {
                            comboBox_PropisBrisanjeVerzija.Items.Add(pr.Sifra + " ; " + pr.Opis + " ; " + pr.DatumKreiranja);
                        }
                    }
                    catch (BussinesLogicLayer.PropisNotFoundException propisException)
                    {
                        textBox_PropisBrisanjeOpis.Text = "";
                        textBox_PropisBrisanjeSifra.Text = "";
                        listView_PropisPromenaDodavanje.Items.Clear();
                        showFailureMessage(propisException.Message);
                        
                    }
                }
                else
                    showFailureMessage("Niste uneli sifru propisa za pretrazivanje");
            }
        }

        private void button_PropisPromeni_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_PropisPromenaTipMasine.SelectedIndex != -1)
            {
                if (textBox_PropisPromenaSifra.Text != "")
                {
                    String sifraPropisa = textBox_PropisPromenaSifra.Text;
                    String masinaTip = comboBox_PropisPromenaTipMasine.SelectedItem.ToString();
                    String opis = textBox_PropisPromenaOpis.Text;

                    AdministrationClass.Instance.addPropis(sifraPropisa, opis, masinaTip, RJChecked, listaPoluproizvodaPromeni);

                    showSuccessMessage("Uspeno ste izvrsili promenu propisa");
                }
                else
                {
                    showFailureMessage("Upisite naziv propisa koji dodajete");
                }
            }
            else
                showFailureMessage("Morate selektovati Tip Masine za koju dodajete propis");
        }

        private void comboBox_PropisBrisanjeVerzija_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listView_PropisBrisanje.Items.Clear();

            int selectedIndex = comboBox_PropisBrisanjeVerzija.SelectedIndex;
            textBox_PropisBrisanjeOpis.Text = listaPropisa[selectedIndex].Opis;
            textBox_PropisBrisanjeTipMasine.Text = listaPropisa[selectedIndex].TipMasine;

            AdministrationClass.Instance.selectPropisPoluproizvodList(listaPropisa[selectedIndex].ID);

            foreach (BussinesLogicLayer.localhost.Propis p in AdministrationClass.Instance.pList)
            {
                popuniListuPoluproizvoda(p, listView_PropisBrisanje, listaPoluproizvodaBrisanje);
            }
        }

        private void button_Obrisi_Click(object sender, RoutedEventArgs e)
        {
            int selecedIndex = comboBox_PropisBrisanjeVerzija.SelectedIndex;
            if (selecedIndex != -1)
            {
                AdministrationClass.Instance.deletePropis(listaPropisa[selecedIndex].ID);
                showSuccessMessage("Slektovani propis je uspesno obrisan");
            }
            else
                showFailureMessage("Niste selektovali ni jedan propis za brisanje");
        }

        private void showSuccessMessage(string suc)
        {
            ActionSuccess ac = new ActionSuccess(suc);
            ac.Show();
        }

        private void showFailureMessage(string fail)
        {
            ActionFailure af = new ActionFailure(fail);
            af.Show();
        }

        private void tabControl_Propis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
