using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BussinesLogicLayer;
using System.Runtime.InteropServices;

namespace ForkLift
{
    public partial class FormNoNetworkMode : Form
    {
        int stanje = 0;
        private String barcode;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetSystemTime(ref SYSTEMTIME st);

        [DllImport("kernel32.dll", SetLastError = true)]
        private extern static void GetSystemTime(ref SYSTEMTIME st);

        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        public FormNoNetworkMode()
        {
            InitializeComponent();

           

            if (TerminalForkLift.Instance.radnik != null)
                this.xLabelMBR.Text = TerminalForkLift.Instance.radnik.MBR.ToString();
            else
                this.xLabelMBR.Text = "Prijavite se";
            if (TerminalForkLift.Instance.selectedRegal != null)
                this.xLabelRegal.Text = TerminalForkLift.Instance.selectedRegal.ID.ToString();
            else
                this.xLabelRegal.Text = "";
            if (TerminalForkLift.Instance.selectedZahtev != null)
                this.xLabelZahtev.Text = TerminalForkLift.Instance.getTextZahtevaByZahtevID((int)TerminalForkLift.Instance.selectedZahtev.ID);
                

            else
            {
                this.xLabelZahtev.Visible = false;
                this.xLabel3.Visible = false;
            }

            this.dataGridView1.DataSource = XmlDB.Instance.SelectAll();

           


            
            

        }


        private void FormNoNetworkMode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                stanje = TerminalForkLift.Instance.parseInputString(barcode);
                switch (stanje)
                {
                    case(TerminalForkLift.State.NOT_LOGGED):
                        this.xLabelMBR.Text = "";
                        this.xLabelRegal.Text = "";
                        this.xLabelZahtev.Text = "";
                        break;
                    case(TerminalForkLift.State.LOGGED):
                        this.xLabelMBR.Text = TerminalForkLift.Instance.radnik.MBR.ToString();
                        this.xLabelRegal.Text = "";
                        this.xLabelZahtev.Text = "";
                        break;
                    case(TerminalForkLift.State.MANUAL):
                        this.xLabelRegal.Text = TerminalForkLift.Instance.selectedRegal.ID.ToString();
                        break;
                }
                dataGridView1.DataSource = XmlDB.Instance.SelectAll();
                barcode = "";
            }
            else if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 90) || (e.KeyChar >= 97 && e.KeyChar <= 122))
            {
                barcode += e.KeyChar;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Network.Instance.checkServiceConnection())
            {

                synchonizeTime();

                
                Network.Instance.stanje = Network.State.SYNCING;
                if (XmlDB.Instance.Count() != 0)
                    Network.Instance.Synchronize(XmlDB.Instance.GetDataTable(), TerminalForkLift.Instance.GetIP());
                XmlDB.Instance.DeleteAll();
                
                
                Network.Instance.stanje = BussinesLogicLayer.Network.State.ONLINE;

                this.Close();
            }
        }

        private void synchonizeTime()
        {
            SYSTEMTIME sync = new SYSTEMTIME();
            GetSystemTime(ref sync);

            DateTime newTime = TerminalForkLift.Instance.synchronizeTime();

            sync.wDay = (ushort)newTime.Day;
            sync.wDayOfWeek = (ushort)newTime.Day;
            sync.wHour = (ushort)newTime.Hour;
            sync.wMinute = (ushort)newTime.Minute;
            sync.wYear = (ushort)newTime.Year;
            sync.wSecond = (ushort)newTime.Second;
            sync.wMonth = (ushort)newTime.Month;

            SetSystemTime(ref sync);
        }



    }
}
