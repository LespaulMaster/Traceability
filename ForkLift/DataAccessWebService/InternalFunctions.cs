using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccessWebService
{
    public class InternalFunctions
    {
        /// <summary>
        /// Prebacuje zahtev u stanje 1 (zakljucava odlaznu poziciju i regal stavlja u status izvrsavanja)
        /// </summary>
        /// <param name="zahtev"></param>
        /// <returns></returns>
        public static Zahtev prebaciUStanje1(Zahtev zahtev)
        {
            zahtev.StatusIzvrsavanja = 1;
            if (zahtev.Pozicija1 != null && zahtev.Pozicija1.KapacitetPreostalo == 1)
                zahtev.Pozicija1.Zakljucana = 1;
            if (zahtev.Regal != null)
                zahtev.Regal.StatusIzvrsavanja = true;


            return zahtev;
        }
    }
}