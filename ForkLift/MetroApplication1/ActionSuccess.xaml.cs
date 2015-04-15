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
using System.Net;
using BussinesLogicLayer;
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace MetroApplication1
{
    /// <summary>
    /// Interaction logic for ActionSuccess.xaml
    /// </summary>
    public partial class ActionSuccess : Elysium.Controls.Window
    {
        byte alpha = 5;
        SolidColorBrush textBrush = new SolidColorBrush();
        Color textColor = new Color();

        public ActionSuccess(string succ)
        {
            InitializeComponent();
            this.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            textBlock_successText.TextWrapping = TextWrapping.Wrap;
            
            textColor = Color.FromArgb(alpha, 255, 255, 255);
            textBrush.Color = textColor;

            textBlock_successText.Foreground = textBrush;
            textBlock_successText.Text = succ;

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 4);

            System.Windows.Threading.DispatcherTimer dispatcherTimer2 = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer2.Tick += new EventHandler(dispatcherTimer2_Tick);
            dispatcherTimer2.Interval = new TimeSpan(0, 0, 0, 0, 100);
            
            dispatcherTimer2.Start();
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dispatcherTimer2_Tick(object sender, EventArgs e)
        {
            alpha += 10;
            if (alpha > 0 && alpha < 11)
                alpha = 255;
            textColor.A = alpha;
            textBrush.Color = textColor;
            textBlock_successText.Foreground = textBrush;
        }
    }
}
