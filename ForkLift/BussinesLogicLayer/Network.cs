using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Data;


namespace BussinesLogicLayer
{
    public class Network
    {
        public enum State
        {
            ONLINE, OFFLINE, SYNCING
        };

        public State stanje;

        private static volatile Network instance;
        private static object syncRoot = new Object();

        public Network()
        {

        }

        public static Network Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Network();
                    }
                }

                return instance;
            }
        }


        public static void show()
        { }

       

        public bool checkServiceConnection()
        {
            bool result = false;
            localhost.Service1 service = new localhost.Service1();
            try
            {
                service.SelectRegal(1);
                result = true;

            }
            catch (WebException exception)
            {
                result = false;
            }
            return result;

        }


        public bool Synchronize(DataTable dt,String IPAddress)
        {
            Boolean result = false;

            localhost.Service1 service = new localhost.Service1();

            try
            {
                service.synchronizeTable(dt, IPAddress);
            }
            catch
            {
                result = false;
            }
            return result;
        }


        
    }
}
