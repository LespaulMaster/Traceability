using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccessWebService
{
    public partial class Zahtev
    {
        public void updateText()
        {
            string prazan_pun = "";
            if (this.Pozicija1 != null && this.Pozicija == null)
            {
                if (this.Pozicija1.Ulazna == true)
                    prazan_pun = "pun";
                else
                    prazan_pun = "prazan";

                string propis_string = "";

                if (this.Propi != null)
                {


                    if (this.Pozicija1.Ulazna == true)
                        propis_string = " sa propisom " + this.Propi.Sifra;
                    else if (this.Pozicija1.Ulazna == false)
                        propis_string = " za propis " + this.Propi.Sifra;
                }

                this.TekstZahteva = "Do pozicije " + this.Pozicija1.KodPozicije +
                    " potreban " + prazan_pun + " regal " + propis_string;


            }
            else if (this.Pozicija != null && this.Pozicija1 == null)
            {
                this.TekstZahteva = "Sa pozicije " + this.Pozicija.KodPozicije +
                                    " pomeriti regal";
            }


        }

        public void updateOcekivaniBarKod()
        {
            if (this.StatusIzvrsavanja == 2)
            {
                this.OcekivaniBarKod = "PO" + this.Pozicija1.ID.ToString().PadLeft(5, '0');
            }
        }
    }
}