using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BussinesLogicLayer;
using System.Threading;
using System.Runtime.InteropServices;
using BussinesLogicLayer.localhost;






namespace ForkLift
{
    static class Program
    {
       
        public static bool isWorking = true;
        
       

        static void StartThread()
        {
            Boolean offlineModeVisible = false;

            
            FormNoNetworkMode noNetForm = null;
            while (isWorking)
            {
                
                Boolean isOnline = Network.Instance.checkServiceConnection();

                if (!isOnline && !offlineModeVisible )
                {
                    noNetForm = new FormNoNetworkMode();
                    Network.Instance.stanje = Network.State.OFFLINE;
                    offlineModeVisible = true;
                    noNetForm.Show();
                }
                if (isOnline && offlineModeVisible == true)
                {
                    
                    offlineModeVisible = false;
                       
                }
                
                Thread.Sleep(5000);

            }
        }
        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormCheckin checkin = new FormCheckin();
            
            Thread thread = new Thread(new ThreadStart(StartThread));

            thread.IsBackground = true;
            thread.Start();

            

            if (checkin.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FormMain());

            }
        }


        
       
        
    }
}
