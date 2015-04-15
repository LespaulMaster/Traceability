using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace BussinesLogicLayer
{
    public abstract class TerminalAbstract
    {
        public class State
        {
            public const int NOT_LOGGED = 0;
            public const int LOGGED = 1;
            public const int PRIJAVLJEN_PROPIS = 2;
            public const int PRIJAVLJEN_POLUPROIZVOD = 3;
            public const int PRIJAVLJEN_IZLAZNI_REGAL = 4;
            public const int NOVI_PROIZVOD = 5;
            public const int POPUNJEN_REGAL = 6;

            public const int PRIJAVLJEN_NOVI_POLUPROIZVOD = 30;
            public const int PRIJAVLJEN_NOVI_POLUPROIZVOD_WORK = 31;
        }


        public string IPAddress = "";
        public localhost.Radnik radnik;
        public localhost.Terminal terminal;
        public localhost.Masina masina;
        public localhost.Propis propis;
        //public localhost.Propis[] propisList;
        public localhost.Poluproizvod[] poluproizvodList;

        public localhost.Regal regalOut;
        public List<localhost.Regal> regalInList = new List<localhost.Regal>();

        public localhost.Propis[] listaPoluproizvoda;
        public localhost.Propis[] listaOtkucanihPoluproizvoda;
        public Boolean[] listaFound;

        public localhost.sp_AlarmMinMax_Result[] alarmMinMaxList;

        //public int brojacProizvoda=0;

        //preuzimanje IPAdrese uredjaja
        public string GetIP()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();

        }

        //TODO inicijalno podizanje aplikacije
        public virtual bool InitialLoad()
        {
            //String IPAddress = GetIP();
            //String IPAddress = "10.188.45.62";
            String IPAddress = "20.200.200.201";

            localhost.Service1 service = new localhost.Service1();

            masina = service.getLastState(IPAddress, out propis, out listaPoluproizvoda, out terminal, out radnik);
            
            if (listaPoluproizvoda != null)
            {
                listaFound = new Boolean[listaPoluproizvoda.Length];
                for (int i = 0; i < listaFound.Length; i++)
                {
                    listaFound[i] = false;
                }
            }

            alarmMinMaxList = service.SelectAlarmMinMax();

            return true;
        }

        public void select_alarmMinMaxList()
        {
            localhost.Service1 service = new localhost.Service1();
            alarmMinMaxList = service.SelectAlarmMinMax();
        }

        //prijava radnika vraca klasu Radnik
        public virtual bool PrijaviRadnika(int MaticniBrojRadnika)
        {
            bool result = true;
            // select Radnik; insert ARadnik

            localhost.Service1 service = new localhost.Service1();
            //string IP = "10.188.45.62";
            String IP = "20.200.200.201";
            radnik = service.PrijaviRadnika(MaticniBrojRadnika, IP);

            // update Terminal
            if (radnik != null)
                terminal = service.updateTerminalRadnik(radnik, IP);
            else
                throw new BussinesLogicLayer.RadnikException("Radnik sa zadatim maticnim brojem nije pronadjen u bazi podataka");

            return result;
        }

        //funkcija koja se poziva kada se radnik odjavljuje ili kada se prijavi novi radnik
        public virtual bool OdjaviRadnika(int MaticniBrojRadnika, int MaticniBrojNovi)
        {
            localhost.Service1 service = new localhost.Service1();

            service.OdjaviRadnika(radnik, terminal);

            if (MaticniBrojNovi == 0)
                radnik = null;
            else
                PrijaviRadnika(MaticniBrojNovi);


            //Insert into ARadnik
            return true;
        }

        //TODO Na klijentu implementirati funkciju za proveru popunjenosti izlaznog regala
        public virtual bool PrijavaIzlaznogRegala(int RegalID)
        {
            localhost.Service1 service = new localhost.Service1();

            if (regalOut != null)
            {
                //Promena 05/03/2015
                //Neophodno propis kupiti sa baze podataka. Ne postoji prijava propisa
                service.updateRegalPopunjen(regalOut, masina.ID);
                service.insertZahtevPunRegal(regalOut, masina.ID);
            }

            regalOut = service.SelectRegal(RegalID);

            if (regalOut != null)
            {
                service.UpdateMasinaStanje(masina.ID, State.PRIJAVLJEN_IZLAZNI_REGAL);
                return true;
            }
            else
                throw new BussinesLogicLayer.RackException("Regal sa zadatim identifikacionim brojem nije pronadjen u bazi podataka");
        }

        public virtual localhost.Regal proveraPrijaveRegala(int RegalID)
        {
            localhost.Service1 service = new localhost.Service1();

            localhost.Regal regalTmp = service.SelectRegal(RegalID);

            if (regalTmp != null)
                return regalTmp;
            else
                throw new BussinesLogicLayer.RackException("Regal sa zadatim identifikacionim brojem nije pronadjen u bazi podataka");


        }

        public virtual bool proveraPopunjenostiRegala(int brojac)
        {
            if (regalOut.Kapacitet < brojac)
                return false;
            else
                return true;
        }

        //prijava propisa vraca klasu Propis
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PropisID"></param>
        /// <exception cref="PropisNotFoundException">
        public virtual void PrijavaPropisa(int PropisID)
        {
            localhost.Service1 service = new localhost.Service1();

            if (service.selectPropisPrijava(PropisID))
                propis = service.PrijavaPropisa(PropisID, (int)terminal.MasinaID);
            else
                throw new BussinesLogicLayer.PropisNotFoundException
                    ("Pokusavate prijavu zastarelog propisa, postoji novija verzija ovog propisa");

            if (propis == null)
                throw new BussinesLogicLayer.PropisNotFoundException("Zadati propis nije pronadjen u bazi podataka");

            if (terminal.Uloga == TerminalRole.OUTPUT)
            {
                //Stanje 2 prestavlja prijavljeni propis
                service.UpdateMasinaStanje(masina.ID, 2);
            }
        }

        //TODO prijava zastoja vraca iii opis zastoja ili ceo objekat klase zastoj
        public virtual string PrijavaZastoja(int ZastojID)
        {
            string tipZastoja = "";
            return tipZastoja;
        }

        //TODO odjava zastoja nakon pocetka proizvodnje
        public virtual bool OdjavaZastoja(int ZastojID)
        {
            return true;
        }

        //prijavapoluproizvoda
        public virtual void prijavaPoluproizvoda(Int64 barCode)
        {
            localhost.Service1 service = new localhost.Service1();

            localhost.Propis otkucaniPoluproizvod = service.selectPropisProizvod(barCode);

            localhost.Proizvod proizvod = service.selectProizvod(barCode);

            if (otkucaniPoluproizvod != null)
            {
                if (terminal.Uloga == TerminalRole.INPUT)
                {
                    listaPoluproizvoda = service.proveraPoluproizvoda(propis.ID);
                    listaFound = new Boolean[listaPoluproizvoda.Length];
                    for (int i = 0; i < listaFound.Length; i++)
                    {
                        listaFound[i] = false;
                    }
                }

                bool findPropis = false;
                for (int i = 0; i < listaPoluproizvoda.Length; i++)
                {
                    if (otkucaniPoluproizvod.ID == listaPoluproizvoda[i].ID)
                    {
                        findPropis = true;
                        if (terminal.Uloga == TerminalRole.INPUT_OUTPUT) //uloga terminala je ulazno izlazna
                            listaFound[i] = true;
                    }
                }

                if (findPropis)
                {
                    service.InsertAPoluproizvod(radnik.ID, terminal.ID, proizvod.ID, masina.ID);
                    service.UpdateTrenutniPoluproizvodi(proizvod, masina.ID);

                    if (terminal.Uloga == TerminalRole.INPUT)
                    {
                        listaOtkucanihPoluproizvoda = service.SelectTrenutniPoluproizvodi(masina.ID);

                        if (listaOtkucanihPoluproizvoda.Length == listaPoluproizvoda.Length)
                        {
                            for (int i = 0; i < listaPoluproizvoda.Length; i++)
                            {
                                for (int j = 0; j < listaOtkucanihPoluproizvoda.Length; j++)
                                {
                                    if (listaOtkucanihPoluproizvoda[j].ID == listaPoluproizvoda[i].ID)
                                    {
                                        listaFound[i] = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    throw new BussinesLogicLayer.PropisNotFoundException("Dati poluproizvod ne odgovara propisu koji se proizvodi!");
            }
            else
                throw new BussinesLogicLayer.PropisNotFoundException("Dati poluproizvod nije otkucan u prethodnoj fazi");
        }

        public virtual void insertProizvod(Int64 barCode)
        {
            localhost.Service1 service = new localhost.Service1();
            int tipGreske;

            localhost.Proizvod proizvod = service.InsertProizvod(radnik.ID, masina.ID, propis.ID, barCode, out tipGreske);

            if (proizvod == null)
            {
                if (tipGreske == 2)
                    throw new BussinesLogicLayer.TyreNotFoundException("Zadati proizvod je vec otkucan kao napravljen sa date masine");
                if (tipGreske == 1)
                    throw new BussinesLogicLayer.TyreNotFoundException
                        (@"Nije moguce dodati novi proizvod \n
                           Na datoj masini je prijavljen novi propis \n
                           Potrebno je prijaviti ulazne poluproizvode(na modulu) \n
                           Potrebno je prijaviti izlazni regal");
            }

            service.InsertRegalProizvod(proizvod, regalOut);
            if (listaPoluproizvoda != null)
                service.insertProizvodIngredients(proizvod.ID, masina.ID);
        }

        //TODO provera popunjenosti regala kada se regal puni
        //TODO provera da li je regal prazan kada se regal  prazni

        public abstract int parseInputString(string inputString);



    }
}
