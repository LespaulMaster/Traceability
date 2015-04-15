using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BussinesLogicLayer;
using DesignLibrary;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

namespace ForkLift
{
    public partial class FormCheckin : Form
    {
        int color;
                
        String barcode;
        BackgroundWorker bw = new BackgroundWorker();

        public FormCheckin()
        {
            color = 54;
            InitializeComponent();
            barcode = "";
          
            xLabelClock.Text = DateTime.Now.ToShortTimeString();

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.WorkerSupportsCancellation = true;
            
            
            
           
           
                  // the code that you want to measure comes here
                
        }

       

        
        private void timer_Tick(object sender, EventArgs e)
        {

            xLabelClock.Text = DateTime.Now.ToShortTimeString();
            
        }

        private void FormCheckin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //startovanje animacije
                
                
                try
                {
                    int stanje = TerminalForkLift.Instance.parseInputString(barcode);
                    if (stanje == TerminalForkLift.State.LOGGED)
                    {
                        xLabel1.ForeColor = Color.FromArgb(255, 54, 54, 54);
                        xLabel1.Text = "Dobrodosli " + TerminalForkLift.Instance.radnik.Ime;
                        timer2.Enabled = true;
                        pictureBoxLoading.Visible = false;
                        

                    }

                }
                catch (Exception exception)
                {
                    FormException formException = new FormException(exception.Message, true);
                    XFormMasking maskingForm = new XFormMasking(formException);
                    maskingForm = new XFormMasking(formException);
                    maskingForm.ShowDialog(this);
                    pictureBoxLoading.Visible = false;
                }
                barcode = "";

                //prekid animacije
                
               
                

            }
            else if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 90) || (e.KeyChar >= 97 && e.KeyChar <= 122))
            {
                barcode += e.KeyChar;
            }
        }
      
    
        private void timer2_Tick(object sender, EventArgs e)
        {
            pictureBoxLoading.Visible = false;
            if (color > 255)
                this.DialogResult = DialogResult.OK;
            else
            {
                
                xLabel1.ForeColor = Color.FromArgb(255, color, color, color);
                color += 6;
            }
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*

            if(Network.Instance.stanje==Network.State.OFFLINE && !errorShowed)
            {
                errorShowed = true;
                netError = new FormException("error", false);
                maskingForm2 = new XFormMasking(netError);
                maskingForm2.ShowDialog(this);

               
                
           }
             else if (Network.Instance.stanje==Network.State.ONLINE && errorShowed)
            {
                errorShowed = false;
                netError.DialogResult = DialogResult.OK;
               
            }
             */
        }




        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
           
            this.Invoke((MethodInvoker)delegate
            {
                pictureBoxLoading.Visible = true; // runs on UI thread
            });
            while (true)
            {
                //BESKONACNA PETLJA
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.Invoke((MethodInvoker)delegate
            {
                pictureBoxLoading.Visible = false; // runs on UI thread
            });
        }

       

    }
         
}
