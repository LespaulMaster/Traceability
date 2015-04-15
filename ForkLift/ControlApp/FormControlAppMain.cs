using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows;


using System.Collections;
using System.Runtime.InteropServices;
using System.Net;


namespace ControlApp
{
    public partial class FormControlAppMain : Form
    {
        public FormControlAppMain()
        {
            InitializeComponent();


            
        }

        private void xButtonMetroProizvod_Click(object sender, EventArgs e)
        {
            fLPanelFilter.Controls.Clear();
            
            DesignLibrary.XLabel xLabelDimenzija = new DesignLibrary.XLabel();
            xLabelDimenzija.Text = "Dimenzija Proizvoda : ";
            xLabelDimenzija.Dock = DockStyle.Left;
            xLabelDimenzija.Dock = DockStyle.Top;
            int margin = (int)xLabelDimenzija.Margin.Right;
            xLabelDimenzija.Margin = new Padding(3, 3, 10, 3);
            xLabelDimenzija.AutoSize = true;

            TextBox textBoxDimenzija = new TextBox();
            textBoxDimenzija.Dock = DockStyle.Left;
            textBoxDimenzija.Dock = DockStyle.Top;
            textBoxDimenzija.Margin = new Padding(3, 3, 3, 3);


            DesignLibrary.XLabel xLabelSpecificBarKod = new DesignLibrary.XLabel();
            xLabelSpecificBarKod.Dock = DockStyle.Left;
            xLabelSpecificBarKod.Dock = DockStyle.Top;
            xLabelSpecificBarKod.Margin = new Padding(3, 20, 3, 3);
            xLabelSpecificBarKod.Text = "Bar-kod Proizvoda : ";
            xLabelSpecificBarKod.AutoSize = true;

            TextBox textBoxSpecificBarKod = new TextBox();
            textBoxSpecificBarKod.Dock = DockStyle.Left;
            textBoxSpecificBarKod.Dock = DockStyle.Top;
            textBoxSpecificBarKod.Margin = new Padding(3, 3, 3, 3);


            DesignLibrary.XButtonMetro xButtonOK = new DesignLibrary.XButtonMetro();
            xButtonOK.Text = "Pronadji";
            xButtonOK.Dock = DockStyle.Right;
            xButtonOK.Dock = DockStyle.Top;
            xButtonOK.Margin = new Padding(3, 20, 3, 3);
            xButtonOK.Height = 50;
            xButtonOK.Width = 200;


            fLPanelFilter.Controls.Add(xLabelDimenzija);
            fLPanelFilter.Controls.Add(textBoxDimenzija);
            fLPanelFilter.Controls.Add(xLabelSpecificBarKod);
            fLPanelFilter.Controls.Add(textBoxSpecificBarKod);
            fLPanelFilter.Controls.Add(xButtonOK);

            MessageBox.Show(fLPanelFilter.Height + " " + panel5.Height);

        }

        private void xButtonMetroRegal_Click(object sender, EventArgs e)
        {
            fLPanelFilter.Controls.Clear();

            //povuci broj regala iz proizvodnje po tipu
            ComboBox xComboBox = new ComboBox();
            xComboBox.Dock = DockStyle.Top;
            xComboBox.Margin = new Padding(3, 3, 20, 3);
            xComboBox.Width = 200;
            //**populate combobox
            
            DesignLibrary.XButtonMetro xButtonOK = new DesignLibrary.XButtonMetro();
            xButtonOK.Text = "Pronadji";
            xButtonOK.Dock = DockStyle.Right;
            xButtonOK.Dock = DockStyle.Top;
            xButtonOK.Margin = new Padding(3, 3, 20, 3);
            xButtonOK.Height = 50;
            xButtonOK.Width = 200;

            fLPanelFilter.Controls.Add(xComboBox);
            fLPanelFilter.Controls.Add(xButtonOK);
        }

        private void FormControlAppMain_Paint(object sender, PaintEventArgs e)
        {
            /*Rectangle r = tabControl_test.ClientRectangle;
            r.Inflate(tabControl_test.Left, tabControl_test.Top);
            TabPage tp = tabControl_test.SelectedTab;
            SolidBrush PaintBrush = new SolidBrush(Color.Red);
            e.Graphics.FillRectangle(PaintBrush, r);
            ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, ButtonBorderStyle.Outset);*/
        }

        private void xButtonMetroStampa_Click(object sender, EventArgs e)
        {
            panelIzvestavanje.Visible = false;
            panelStampa.Visible = true;
            panelStampa.Dock = DockStyle.Fill;
        }

        private void xButtonMetroIzvestaj_Click(object sender, EventArgs e)
        {
            panelStampa.Visible = false;
            panelIzvestavanje.Visible = true;
            panelIzvestavanje.Dock = DockStyle.Fill;
        }

        private void FormControlAppMain_Load(object sender, EventArgs e)
        {
            panelStampa.Visible = false;
            panelIzvestavanje.Visible = false;
        }

    }
}
 