using System;
using System.Collections.Generic;
using System.Text;
using BussinesLogicLayer.localhost;
using BussinesLogicLayer;
namespace BussinesLogicLayer
{
    public sealed class TerminalForkLift : TerminalAbstract
    {
        private static volatile TerminalForkLift instance = new TerminalForkLift();
        private static object syncRoot = new Object();
        public Zahtev[] listaZahteva;

        //za zutu zonu
        public Zahtev[] listaZutaZona;

        public Zahtev[] povezaniZahtevi;
        public Zahtev selectedZahtev;
        public Zahtev inicijalniZahtev;//zahtev koji je inicijalno preuzet sa servera
        public Regal selectedRegal;


        public int stanje = State.NOT_LOGGED;

        public TerminalForkLift()
        {

            //getAllMachines();
        }

        public static TerminalForkLift Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TerminalForkLift();
                    }
                }

                return instance;
            }
        }
        /// <summary>
        /// parsira ulazni string i na osnovu njega izvrsava odredjene akcije
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        /// <exception cref="BusinesLogicLayer.TyreNotFoundException">Ukoliko guma nije pronadjena</exception>
        public override int parseInputString(string inputString)
        {
            if (inputString.Length > 2)
            {
                string substring = inputString.Substring(0, 2);
                string barcode = inputString.Substring(2, inputString.Length - 2);
                int barcodeNumber = -1;
                Int32.TryParse(barcode, out barcodeNumber);


                if (Network.Instance.stanje == Network.State.ONLINE)
                {
                    switch (this.stanje)
                    {
                        case State.NOT_LOGGED:
                            if (substring == "QV" && barcodeNumber != -1)
                            {
                                if (barcodeNumber != 0)
                                {
                                    if (PrijaviRadnika(barcodeNumber))
                                    {

                                        stanje = State.LOGGED;
                                    }
                                    else
                                        throw new RadnikException("Radnik ne postoji u bazi podataka");
                                }
                                else
                                {
                                    throw new RadnikException("Prijavite se");
                                }

                            }
                            else
                            {
                                throw new RadnikException("Prijavite se");
                            }
                            break;
                        case State.LOGGED:
                            if (substring == "QV" && barcodeNumber != -1)
                            {
                                if (barcodeNumber == 0)
                                {
                                    if (OdjaviRadnika(barcodeNumber, 0))
                                        stanje = State.NOT_LOGGED;

                                }

                            }
                            else if (substring == "RA" && barcodeNumber != -1)
                            {
                                try
                                {
                                    if (this.regalSlobodan(barcodeNumber))
                                    {
                                        selectedZahtev = getZahtevByRackID(barcodeNumber);
                                        if (selectedZahtev != null)
                                        {
                                            stanje = State.IN_PROCESS;
                                            if (posIsUlazna())
                                            {
                                                stanje = State.LEFT_PRODS;
                                            }
                                        }
                                        else
                                        {
                                            stanje = State.MANUAL;
                                        }
                                    }
                                    else
                                    {
                                        throw new RackException("Regal je zakljucan i zabranjen za upotrebu");
                                    }
                                }
                                catch (RackException rackEx)
                                {
                                    throw rackEx;
                                }


                            }

                            else
                            {
                                throw new DataFlowException("Izaberite zahtev iz liste ili ručno skenirajte bar-kod pozicije");
                            }
                            break;
                        case State.CHOOSEN:
                            if (substring == "RA")
                            {
                                if (this.regalSlobodan(barcodeNumber))
                                {
                                    if (barcodeNumber == selectedZahtev.RegalID)
                                    {
                                        Zahtev result = updateZahtev();
                                        //stanje sistema postavljeno u updateZahtev() funkciji
                                        if (result == null)
                                            throw new RackException("Greska u procesiranju zahteva, nije pronadjena lokacija za odlaganje");
                                        else
                                            selectedZahtev = result;

                                    }
                                    else
                                    {
                                        throw new RackException("Pogrešan regal, proverite zahtev koji ste izabrali");
                                    }
                                }
                                else
                                {
                                    throw new RackException("Regal je zakljucan i zabranjen za upotrebu");
                                }

                            }
                            else if (inputString == selectedZahtev.OcekivaniBarKod)
                            {


                            }
                            else
                            {
                                throw new RackException("Skenirajte bar-kod na regalu");
                            }
                            break;
                        case State.IN_PROCESS:
                            if (substring == "PO")
                            {
                                int idPozicije;
                                if (Int32.TryParse(barcode, out idPozicije))
                                {
                                    if (idPozicije == selectedZahtev.Pozicija_DO)
                                    {
                                        Service1 service = new Service1();
                                        if (service.pozicijaSlobodna(idPozicije))
                                        {
                                            Zahtev tmpZahtev = service.updateZahtevIzvrsen(selectedZahtev.ID);
                                            if (tmpZahtev.StatusIzvrsavanja == 3)
                                            {
                                                inicijalniZahtev = null;
                                                selectedZahtev = null;
                                                stanje = State.LOGGED;
                                            }
                                            else if (tmpZahtev.Pozicija_DO != this.selectedZahtev.Pozicija_DO)
                                            {
                                                selectedZahtev = tmpZahtev;
                                                throw new ChangedZahtevException("Pozicija zaključana u međuvremenu, nova pozicija predložena");
                                            }
                                            else
                                                throw new Exception("nemoguce izvrsiti zahtev");
                                        }
                                        else
                                        {
                                            throw new Exception("pozicija nije slobodna");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Pogrešna pozicija");
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("Skenirajte bar kod odredišne pozicije");
                            }
                            break;
                        case State.MANUAL:
                            if (substring == "PO")
                            {
                                int posID;
                                if (Int32.TryParse(barcode, out posID))
                                {
                                    if (this.updateRegalPozicija(selectedRegal, posID))
                                        stanje = State.LOGGED;
                                    else
                                        throw new RackException("Lokacija zaključana za korišćenje");


                                }
                            }
                            else
                                throw new DataFlowException("Otkucajte bar-kod odredišne lokacije");
                            break;
                    }
                }
                else if (Network.Instance.stanje == Network.State.OFFLINE )
                    switch (stanje)
                    {
                        case State.NOT_LOGGED:
                            if (substring == "QV")
                                if (barcodeNumber != 0)
                                {
                                    radnik = new Radnik();


                                    radnik.MBR = barcodeNumber;
                                    this.stanje = State.LOGGED;
                                }
                                else
                                    throw new RadnikException("Prijavite se");
                            break;
                        case State.LOGGED:
                        case State.CHOOSEN:
                            if (substring == "PO")
                            {

                                this.stanje = State.WAITING_REGAL;
                            }
                            if (substring == "RA")
                            {
                                selectedRegal = new Regal();
                                selectedRegal.ID = barcodeNumber;
                                this.stanje = State.MANUAL;

                            }
                            else if (substring == "QV" && barcodeNumber == 0)
                            {
                                radnik = null;

                                stanje = State.NOT_LOGGED;
                            }
                            else if (substring == "QV" && barcodeNumber != 0)
                                throw new RackException("Već ste prijavljeni");
                            else
                                throw new RackException("Ocekuje se regal");
                            break;
                        case State.IN_PROCESS:
                        case State.MANUAL:
                            if (substring == "PO")
                            {

                                XmlDB.Instance.Insert(radnik.MBR, selectedRegal.ID, selectedRegal.PozicijaID == null ? 0 : (int)selectedRegal.PozicijaID, barcodeNumber, DateTime.Now);
                                selectedRegal = null;
                                selectedZahtev = null;
                                this.stanje = State.LOGGED;
                            }
                            break;
                    }
                else if (Network.Instance.stanje == Network.State.SYNCING)
                {
                    throw new SyncingException("Molimo sačekajte, u toku je sinhronizacija podataka");
                }





            }
            return stanje;



        }

        private bool regalSlobodan(int barcodeNumber)
        {
            Service1 service = new Service1();
            return service.regalZakljucan(barcodeNumber);
        }

        private bool posIsUlazna()
        {
            Service1 service = new Service1();
            return service.posIsUlazna(Convert.ToInt32(selectedZahtev.Pozicija_OD));

        }

        private Zahtev updateZahtev()
        {
            Service1 service = new Service1();
            Zahtev result = null;

            //zahtev za dovozenje
            if (selectedZahtev.Pozicija_DO != null)
            {
                result = service.updateZahtevDovezi(selectedZahtev.ID);
                if (result != null)
                    stanje = State.IN_PROCESS;

            }//zahtev za odvozenje
            else if (selectedZahtev.Pozicija_OD != null)
            {
                result = service.updateZahtevOdvezi(selectedZahtev.ID, this.terminal.ID);
                stanje = State.IN_PROCESS;
                if (posIsUlazna())
                    stanje = State.LEFT_PRODS;

            }

            return result;
        }

        public bool updateZahtev(Zahtev zahtev)
        {
            Service1 service = new Service1();
            return service.updateZahtev(zahtev);

        }

        public bool cancelZahtev()
        {
            Service1 service = new Service1();
            if (service.cancelZahtev(this.inicijalniZahtev, povezaniZahtevi))
            {
                stanje = State.LOGGED;
                selectedZahtev = null;
                povezaniZahtevi = null;
                return true;
            }
            return false;

        }
        /// <summary>
        /// funkcija kojom operater selektuje zahtev
        /// </summary>
        /// <param name="zahtev"> <i>-1 oznacava da je nemoguce selektovati zahtev</i> <br/> 
        ///                     <i>0 oznacava da je potrebno izabrati tip regala</i>
        ///                     <i>1 oznacava da da je zahtev uspesno selektovan</i></param>
        /// <returns></returns>
        public int izaberiZahtev(Zahtev zahtev)
        {
            int result = -1;

            using (Service1 service = new Service1())
            {

                if (this.selectedZahtev.Colaza == null && service.potrebnoIzabratiTipRegala(this.selectedZahtev.Pozicija_DO))
                {
                    result = 0;
                }
                else
                {
                    selectedZahtev = service.selectZahtev(zahtev.ID, this.terminal.ID, out this.povezaniZahtevi);
                    if (selectedZahtev != null)
                    {
                        stanje = State.CHOOSEN;
                        result = 1;

                    }
                    else
                        result = -1;
                }

            }
            return result;
        }

        public Zahtev[] getZahtevi()
        {
            using (Service1 service = new Service1())
            {
                try
                {
                    listaZahteva = service.getAllZahtevi(this.terminal.ID);
                }
                catch (Exception e)
                {
                    string txt = e.Message;
                }
            }
            return listaZahteva;
        }

        public Zahtev[] getYellowZoned()
        {
            Service1 service = new Service1();
            try
            {
                listaZutaZona = service.getYellowZoned(this.terminal.ID);
            }
            catch
            {
            }
            return listaZutaZona;


        }

        public String getZahtevStrings(out String pozicijaOD, out String pozicijaDO)
        {
            Service1 service = new Service1();

            return service.getZahtevStrings(this.selectedZahtev.ID, out pozicijaOD, out pozicijaDO);

        }



        public String getPositionName(int idPos)
        {
            Service1 service = new Service1();
            return service.getPositionName(idPos);
        }

        public bool updateRegalBrojProizvoda(int noProds)
        {
            Service1 service = new Service1();
            stanje = State.IN_PROCESS;
            return service.updateRegalBrojProizvoda((int)selectedZahtev.RegalID, noProds);

        }

        public bool updateRegalPozicija(Regal regal, int posDo)
        {
            Service1 service = new Service1();
            return service.updateRegalPozicija(regal.ID, (int)regal.PozicijaID, posDo);
        }
        public String getZahtevPozicijaDO()
        {

            Service1 service = new Service1();
            if (selectedZahtev.Pozicija_DO == null)
                return null;
            else
            {
                String kodPozicije = service.getPositionName((int)selectedZahtev.Pozicija_DO);
                return kodPozicije;
            }

        }



        public new class State
        {
            public const int NOT_LOGGED = 0;
            public const int LOGGED = 1;
            public const int CHOOSEN = 2;
            public const int IN_PROCESS = 3;
            public const int LEFT_PRODS = 4;
            public const int MANUAL = 5;
            public const int WAITING_REGAL = 6;


        }

        public int goOnline()
        {
            Service1 service = new Service1();

            if (radnik != null)
            {
                stanje = State.LOGGED;
                radnik = service.PrijaviRadnika(radnik.MBR, IPAddress);
            }
            if (selectedRegal != null && selectedZahtev != null)
            {
                selectedRegal = service.SelectRegal(selectedRegal.ID);
                stanje = State.MANUAL;
            }

            return stanje;

        }

        public Zahtev getZahtevByRackID(int rackID)
        {
            Zahtev zahtev = null;
            //selektujemo regal koji prevozimo
            Service1 service = new Service1();
            selectedRegal = service.SelectRegal(rackID);
            if (selectedRegal == null)
                throw new RackException("Regal ne postoji u bazi podataka");
            foreach (Zahtev zht in listaZahteva)
            {
                if (zht.RegalID == rackID)
                {
                    if (izaberiZahtev(zht) == 1)
                        zahtev = zht;
                    break;
                }
            }

            if (zahtev == null)
                foreach (Zahtev zht in listaZutaZona)
                {
                    if (zht.RegalID == rackID)
                    {
                        if (izaberiZahtev(zht) == 1)
                            zahtev = zht;
                        break;
                    }
                }

            return zahtev;
        }

        public String getTextZahtevaByZahtevID(int id)
        {
            String result = null;
            for (int i = 0; i < listaZahteva.Length; i++)
                if (listaZahteva[i].ID == id)
                    result = listaZahteva[i].TekstZahteva;
            return result;
        }



        public DateTime synchronizeTime()
        {
            Service1 service = new Service1();


            return service.synchronizeTime();
        }


        /// <summary>
        /// nalazi prazan regal na skladistu odredjene colaze 
        /// </summary>
        /// <param name="colaza"></param>
        /// <returns></returns>
        public Regal getRackByColaza(int colaza, int RJ, String tipPozicije)
        {
            Service1 service = new Service1();

            return service.getRackByType(colaza, RJ, tipPozicije);
        }

        public void releaseRegal(int regalID)
        {
            Service1 service = new Service1();
            service.releaseRegal(regalID);
        }

        public static Zahtev clone(Zahtev zahtev)
        {
            Zahtev cloned = new Zahtev();
            cloned.ID = zahtev.ID;
            cloned.Pozicija_DO = zahtev.Pozicija_DO;
            cloned.Pozicija_OD = zahtev.Pozicija_OD;
            cloned.PropisID = zahtev.PropisID;
            cloned.RegalID = zahtev.RegalID;
            cloned.StatusIzvrsavanja = 0;
            cloned.VremeKreiranja = zahtev.VremeKreiranja;

            return cloned;
        }
    }

}
