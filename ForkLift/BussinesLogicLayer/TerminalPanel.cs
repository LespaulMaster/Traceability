using System;
using System.Collections.Generic;
using System.Text;


namespace BussinesLogicLayer
{
    public static class RadnaJedinica
    {
        public const int VALJARA = 1;
        public const int PRERADA = 2;
        public const int KONFEKCIJA = 3;
        public const int VULKANIZACIJA = 4;
    }
    public class TerminalRole
    {
        
        public const int OUTPUT = 0;
        public const int INPUT = 1;
        public const int INPUT_OUTPUT = 2;
    }

    public class TerminalPanel : TerminalAbstract
    {
        //definise stanje u kome se nalazi aplikacija, pocetno stanje je nula;
        public int stanje = 0;
        public int brojacProizvoda = 0;

        private static readonly TerminalPanel instance = new TerminalPanel();

        public TerminalPanel()
        {
            
        }

        public static TerminalPanel Instance
        {
            get { return instance; }
        }

        public override void PrijavaPropisa(int PropisID)
        {
            base.PrijavaPropisa(PropisID);
            localhost.Service1 service = new localhost.Service1();

            poluproizvodList = service.SelectPoluproizvodList(PropisID);
        }


        public void InitialTerminalLoad()
        {
            InitialLoad();
            if (radnik == null)
                stanje = State.NOT_LOGGED;
            else
                stanje = (int)masina.Stanje;
        }

        //TODO parse input string
        /// <summary>
        /// vraca se stanje u kome se nalazi terminal nakon citanja bar koda, na osnovu cega se preduzimaju akcije
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public override int parseInputString(string inputString)
        {
            string barcode = "";
            barcode = inputString.Substring(2, inputString.Length - 2);

            if (inputString.Length >= 2)
            {
                string substring = inputString.Substring(0, 2);

                //ukoliko se radi  o terminalu koji ima ulogu koja nije ulazno-izlazna, potrebno je ponovo ucitati parametre terminala
                InitialTerminalLoad();

                if (substring == "QV")
                {
                    int mbr;
                    if (Int32.TryParse(barcode, out mbr))
                    {
                        if (stanje == State.NOT_LOGGED)
                        {
                            PrijaviRadnika(mbr);
                            stanje = (int)masina.Stanje;
                            return State.LOGGED;
                        }
                        else
                        {
                            if (OdjaviRadnika((int)radnik.MBR, mbr))
                            {
                                if (mbr == 0)
                                    stanje = State.NOT_LOGGED;
                                else
                                {
                                    stanje = (int)masina.Stanje;
                                    return State.LOGGED;
                                }
                            }
                        }
                    }
                }
                else if (substring == "31" || substring == "41" || substring == "51")
                {
                    localhost.Service1 service = new localhost.Service1();
                    localhost.Propis propisTmp = service.selectPropisID(Int32.Parse(barcode));
                    if (propisTmp != null)
                    {
                        if ((propisTmp.TipMasine == masina.TipMasine || masina.TipMasine == "T")
                            && terminal.Uloga != TerminalRole.INPUT)
                        {
                            int propisID;

                            if (Int32.TryParse(barcode, out propisID))
                            {
                                if (stanje >= State.LOGGED)
                                {
                                    PrijavaPropisa(propisID);
                                    stanje = State.PRIJAVLJEN_PROPIS;
                                }
                                else
                                    throw new BussinesLogicLayer.PropisNotFoundException
                                        ("Nije prijavljen radnik, Da biste prijavili propis morate prijaviti radnika");
                            }
                        }
                        else
                        {
                            if (terminal.Uloga == TerminalRole.INPUT)
                                throw new BussinesLogicLayer.defaultException
                                    ("Ovaj terminal je ulaznog karaktera i sa njega nije moguce prijavljivati propise");
                            else
                                throw new BussinesLogicLayer.PropisNotFoundException("Ovaj propis nije adekvatan za ovaj tip masine");
                        }
                    }
                    else
                        throw new BussinesLogicLayer.PropisNotFoundException("Ovaj propis nije pronadjen u okviru baze podataka");
                }
                else if (substring == "30" || substring == "40" || substring == "50")
                {
                    //TODO barkod je definisan sa 3 cifre pocetno
                    //int proizvodPrerada = Int32.Parse(inputString.Substring(0, 3));
                    int proizvod_IzlazniBarKod = Int32.Parse(inputString.Substring(0, 3));

                    if (masina.RJ != RadnaJedinica.VULKANIZACIJA && masina.TipMasine != "T")
                    {
                        if (masina.IzlazniBarKod > proizvod_IzlazniBarKod && stanje >= State.PRIJAVLJEN_PROPIS
                            && terminal.Uloga != TerminalRole.OUTPUT)
                        {
                            //Ulaz na masinu
                            prijavaPoluproizvoda(Int64.Parse(inputString));
                            bool prijavljeni_poluproizvodi = true;
                            foreach (Boolean b in listaFound)
                            {
                                if (b == false)
                                    prijavljeni_poluproizvodi = false;
                            }
                            if (prijavljeni_poluproizvodi && stanje == State.PRIJAVLJEN_PROPIS)
                            {
                                stanje = State.PRIJAVLJEN_POLUPROIZVOD;
                                if (terminal.Uloga == TerminalRole.INPUT)
                                {
                                    localhost.Service1 service = new localhost.Service1();
                                    service.UpdateMasinaStanje(masina.ID, State.PRIJAVLJEN_POLUPROIZVOD);
                                    service.Dispose();
                                }
                            }
                            else
                                if (stanje == State.PRIJAVLJEN_PROPIS)
                                    return State.PRIJAVLJEN_NOVI_POLUPROIZVOD;
                                else
                                    return State.PRIJAVLJEN_NOVI_POLUPROIZVOD_WORK;
                        }
                        else
                        {
                            if (masina.IzlazniBarKod > proizvod_IzlazniBarKod && stanje >= State.PRIJAVLJEN_PROPIS
                                && terminal.Uloga == TerminalRole.OUTPUT)
                                throw new BussinesLogicLayer.defaultException("Nije moguce ocitavati ulazne proizvode sa izlaznog terminala");
                            if (masina.IzlazniBarKod > proizvod_IzlazniBarKod && stanje < State.PRIJAVLJEN_PROPIS)
                                throw new BussinesLogicLayer.defaultException
                                    ("Niste prijavili propis i zato je nemoguce prijavljivanje poluproizvoda");
                        }

                        if (masina.IzlazniBarKod == proizvod_IzlazniBarKod && stanje >= State.PRIJAVLJEN_IZLAZNI_REGAL
                            && terminal.Uloga != TerminalRole.INPUT)
                        {
                            //TODO Izlazni proizvod sa masine
                            //Ukoliko se desi greska treba logovati propis koji se trenutno nalazi na masini

                            insertProizvod(Int64.Parse(inputString));
                            stanje = State.NOVI_PROIZVOD;
                        }
                        else
                        {
                            if (masina.IzlazniBarKod == proizvod_IzlazniBarKod && stanje >= State.PRIJAVLJEN_IZLAZNI_REGAL
                                && terminal.Uloga == TerminalRole.INPUT)
                            {
                                throw new BussinesLogicLayer.defaultException
                                    ("Nije moguce ocitavati izlazni proizvod sa regala koji je ulazni");
                            }
                            if (masina.IzlazniBarKod == proizvod_IzlazniBarKod && stanje < State.PRIJAVLJEN_IZLAZNI_REGAL)
                            {
                                if (stanje == State.PRIJAVLJEN_POLUPROIZVOD)
                                    throw new BussinesLogicLayer.defaultException("Nije izvrsena prijava izlaznog regala.");
                                else if (stanje == State.PRIJAVLJEN_PROPIS)
                                    throw new BussinesLogicLayer.PropisNotFoundException("Nije izvrsena prijava poluproizvoda");
                                else if (stanje == State.LOGGED)
                                    throw new BussinesLogicLayer.PropisNotFoundException("Nije izvrsena prijava propisa");
                                else if (stanje == State.NOT_LOGGED)
                                    throw new BussinesLogicLayer.PropisNotFoundException("Nije izvrsena prijava Radnika");
                            }
                        }

                        if (masina.IzlazniBarKod <= proizvod_IzlazniBarKod - 1)
                        {
                            throw new BussinesLogicLayer.PropisNotFoundException("Bar kod koji je otkucan ovde ne odgovara datoj masini");
                        }
                    }
                    else
                    {
                        if (masina.TipMasine == "T")
                        {
                            //TODO dadati proveru propisa za gume
                            //trenutno gume ne postoje u bazi, jer nije poovezana konfekcija
                            if (stanje >= State.PRIJAVLJEN_IZLAZNI_REGAL)
                            {
                                insertProizvod(Int64.Parse(inputString));
                                brojacProizvoda++;
                                //regalPopunjen = proveraPopunjenostiRegala(brojacProizvoda);
                                //TODO UKOLIKO JE REGAL POPUNJEN NE DOZVOLITI DA SE DALJE POPUNJAVA, ZAKLJUCATI TERMINAL
                                if (brojacProizvoda == regalOut.Kapacitet)
                                {
                                    stanje = State.PRIJAVLJEN_PROPIS;
                                    throw new BussinesLogicLayer.RackException
                                        ("Dodali ste novi proizvod sa barkodom " + Int64.Parse(inputString) +
                                        " Regal je popunjen, nije moguce dalje popunjavati ovaj regal. Molimo prijavite naredni izlazni regal");
                                }
                                else
                                    stanje = State.NOVI_PROIZVOD;
                            }
                            else
                                throw new BussinesLogicLayer.TyreNotFoundException
                                    ("Nije moguce dodati proizvod ukoliko nije prijavljen izlazni regal. Prijavite izlazni regal");
                        }
                    }
                    //
                }
                else if (substring == "RA")
                {
                    int rackID;
                    rackID = Int32.Parse(barcode);
                    localhost.Regal regalTmp = proveraPrijaveRegala(rackID);

                    if (!regalTmp.isEmpty)
                    {
                        throw new BussinesLogicLayer.RackException("Nije moguce prijaviti regal koji je popunjen");
                    }

                    //TODO proveriti sta se desava sa ovim
                    if (stanje >= State.PRIJAVLJEN_PROPIS && (masina.RJ == RadnaJedinica.VULKANIZACIJA || masina.TipMasine == "T"))
                    {
                        if (masina.RJ == regalTmp.RJ && masina.RJ != RadnaJedinica.VULKANIZACIJA)
                        {
                            if (masina.RJ == 3)
                            {
                                //TODO resiti situaciju sa univerzalnim regalima
                                char colazaPropis = propis.Sifra[propis.Sifra.Length - 1];
                                if (regalTmp.Colaza % 10 == Int32.Parse(colazaPropis.ToString()))
                                {
                                    if (masina.TipMasine.Trim() == "T" && regalTmp.Opis.Trim() == "nepokretni regal")
                                    {
                                        if (regalOut != null)
                                        {
                                            regalOut.BrojProizvoda = brojacProizvoda;
                                            regalOut.isEmpty = false;
                                        }
                                        PrijavaIzlaznogRegala(rackID);
                                        stanje = State.PRIJAVLJEN_IZLAZNI_REGAL;
                                        brojacProizvoda = 0;
                                    }
                                    else
                                    {
                                        if (regalTmp.Opis != "nepokretni regal")
                                        {
                                            stanje = State.PRIJAVLJEN_PROPIS;
                                            throw new BussinesLogicLayer.RackException("Regal nije odgovarajuci za zadatu masinu");
                                        }
                                    }
                                }
                                else
                                {
                                    stanje = State.PRIJAVLJEN_PROPIS;
                                    throw new BussinesLogicLayer.RackException
                                        ("Velicina regala se ne slaze sa propisom koji se koristi na masini");
                                }
                            }
                        }
                        else
                        {
                            stanje = State.PRIJAVLJEN_PROPIS;
                            throw new BussinesLogicLayer.RackException
                                ("Regal se ne moze smatrati izlanim regalom za datu masinu");
                        }
                    }
                    else
                    {
                        if (stanje == State.NOT_LOGGED && (masina.RJ == RadnaJedinica.VULKANIZACIJA || masina.TipMasine == "T"))
                            throw new BussinesLogicLayer.RadnikException("Nije Prijavljen radnik, morate najpre prijaviti radnika");
                        else if (stanje == State.LOGGED && (masina.RJ == RadnaJedinica.VULKANIZACIJA || masina.TipMasine == "T"))
                            throw new BussinesLogicLayer.PropisNotFoundException
                                ("Nije moguce prijaviti izlazni regal ukoliko nije prijavljen propis");
                    }

                    if (!(masina.RJ == RadnaJedinica.VULKANIZACIJA || masina.TipMasine == "T") &&
                        (stanje >= State.PRIJAVLJEN_POLUPROIZVOD))
                    {
                        if (masina.RJ == RadnaJedinica.KONFEKCIJA)
                        {
                            char colazaPropis = propis.Sifra[propis.Sifra.Length - 1];
                            if (regalTmp.Colaza % 10 == Int32.Parse(colazaPropis.ToString()))
                            {
                                if (regalTmp.Opis.Trim() == "pokretni regal")
                                {
                                    if (regalOut != null)
                                    {
                                        regalOut.BrojProizvoda = brojacProizvoda;
                                        regalOut.isEmpty = false;
                                    }
                                    PrijavaIzlaznogRegala(rackID);
                                    stanje = State.PRIJAVLJEN_IZLAZNI_REGAL;
                                    brojacProizvoda = 0;
                                }
                                else
                                    throw new BussinesLogicLayer.RackException("Regal nije odgovarajuci za zadatu masinu");
                            }
                        }
                    }
                    else
                    {
                        if (stanje < State.PRIJAVLJEN_POLUPROIZVOD)
                        {
                            if (stanje == State.PRIJAVLJEN_PROPIS)
                                throw new BussinesLogicLayer.PropisNotFoundException
                                    ("Nije moguce prijaviti izlazni regal ukoliko nisu prijavljeni svi poluproizvodi");
                            if (stanje == State.LOGGED)
                                throw new BussinesLogicLayer.PropisNotFoundException
                                    ("Nije moguce prijaviti izlazni regal ukoliko nije prijavljen propis");
                            if (stanje == State.NOT_LOGGED)
                                throw new BussinesLogicLayer.defaultException
                                    ("Nije Prijavljen radnik, morate najpre prijaviti radnika");
                        }
                    }
                }
                else
                    throw new BussinesLogicLayer.defaultException("Zadati bar-kod nije adekvatan");

            }
            return stanje;
        }

        public void insertGreska(localhost.Greska greska)
        {
            localhost.Service1 service1 = new localhost.Service1();
            service1.insertGreska(greska);
            
        }
    }
}
