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

namespace AdministrationApp
{
    /// <summary>
    /// Interaction logic for ActionFailure.xaml
    /// </summary>
    public partial class ActionFailure : Elysium.Controls.Window
    {
        byte alpha = 0;
        SolidColorBrush textBrush = new SolidColorBrush();
        Color textColor = new Color();

        public ActionFailure(string fail)
        {
            InitializeComponent();
            textBlock_failureText.Foreground = new SolidColorBrush(Colors.DarkBlue);
            textBlock_failureText.TextWrapping = TextWrapping.Wrap;

            textColor = Color.FromArgb(alpha, 192, 57, 43);
            textBrush.Color = textColor;

            textBlock_failureText.Foreground = textBrush;
            textBlock_failureText.Text = fail;

            /*System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 4);
            dispatcherTimer.Start();*/

            System.Windows.Threading.DispatcherTimer dispatcherTimer2 = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer2.Tick += new EventHandler(dispatcherTimer2_Tick);
            dispatcherTimer2.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer2.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dispatcherTimer2_Tick(object sender, EventArgs e)
        {
            alpha += 10;
            if (alpha > 0 && alpha < 10)
                alpha = 255;
            textColor.A = alpha;
            textBrush.Color = textColor;
            textBlock_failureText.Foreground = textBrush;
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


