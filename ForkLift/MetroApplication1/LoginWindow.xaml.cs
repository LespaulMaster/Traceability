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
using System.Windows.Shapes;
using BussinesLogicLayer;

namespace MetroApplication1
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Elysium.Controls.Window
    {

        BussinesLogicLayer.localhost.Radnik radnik;
        
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            connectFunction();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                connectFunction();
            }
        }

        private void ExecuteException(string ExceptionMessage)
        {
            ActionFailure af = new ActionFailure(ExceptionMessage);
            af.Show();
        }

        private void selectRadnik()
        {
            string password = passwordBox_password.Password;
            string username = textBox_username.Text;

            if (password == "")
                throw new Exception("Niste uneli lozinku za pristup sistemu");
            if (username == "")
                throw new Exception("Niste uneli korisnicko ime za pristup sistemu");
            
            radnik = ControlClass.Instance.selectRadnikLogin(username, password);

            if (radnik == null)
                throw new BussinesLogicLayer.RadnikException("Pogresni parametri kojima ste pokusali da se logujete na sistem");
        }

        private void connectFunction()
        {
            try
            {
                selectRadnik();

                MainWindow mw = new MainWindow(radnik);
                mw.Show();
                this.Close();
            }
            catch (BussinesLogicLayer.RadnikException radnikException)
            {
                ExecuteException(radnikException.Message);
            }
            catch (Exception exception)
            {
                ExecuteException(exception.Message);
            }
        }
    }
}
