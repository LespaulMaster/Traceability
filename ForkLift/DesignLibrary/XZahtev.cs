using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DesignLibrary
{
    public partial class XZahtev : UserControl
    {
        public long zahtevID;
        public int indexOfZahtev;//index zahteva u listi zahteva pribavljenih sa servera

        public XZahtev(String masina, String infoText, String time)
        {
            
            InitializeComponent();

            //postavljanje labela unutar kontrole
            this.labelMasina.Text = masina;
            this.labelText.Text = infoText;
            this.labelTime.Text = time;


            foreach (Control c in this.Controls)
            {
                c.Click += new System.EventHandler(this.XZahtev_Click);

            }

            
        }

        public String Masina
        {
            get{   return this.labelMasina.Text;   }
            set{   this.labelMasina.Text = value;  }
        }
        public String Text
        {
            get { return this.labelText.Text; }
            set { this.labelText.Text = value; }
        }
        public String Time
        {
            get { return this.labelTime.Text; }
            set { this.labelTime.Text = value; }
        }

        public void setColorYellow()
        {
            this.BackColor = Color.FromArgb(255,196,13);
            this.panel1.BackColor = Color.FromArgb(255,211,71);
            this.labelMasina.ForeColor = Color.FromArgb(29, 29, 29);
            this.labelText.ForeColor = Color.FromArgb(29, 29, 29);

        }

        private void XZahtev_Click(object sender, EventArgs e)
        {
            this.OnClick(new EventArgs());
        }
    }
}
