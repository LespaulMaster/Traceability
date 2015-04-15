using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Linq;
using System.IO;
using System.Web.Services.Protocols;
using System.Runtime.InteropServices;
using System.Data;
using System.Transactions;
namespace DataAccessWebService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Service1 : System.Web.Services.WebService
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private extern static void GetSystemTime(ref SYSTEMTIME st);

        [WebMethod]
        bool isAlive()
        {
            return true;
        }

        #region Radnik
        //*****************************************************************************************
        //*****************************************************************************************
        //                          RADNIK FUNKCIJE
        //*****************************************************************************************
        //*****************************************************************************************

        //set funkcija za prijavu radnika
        //*****************************************************************************************
        [WebMethod]
        public Radnik PrijaviRadnika(int QV, string IPAddressTerminal)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Radnik radnik = new Radnik();

            radnik = SelectRadnik(QV);

            if (radnik != null)
            {
                ARadnik ar = new ARadnik();
                ar.RadnikID = radnik.ID;
                ar.TerminalID = updateTerminalRadnik(radnik, IPAddressTerminal).ID;
                ar.VremePrijave = DateTime.Now;

                dbase.ARadniks.AddObject(ar);
                dbase.SaveChanges();
            }
            return radnik;
        }

        [WebMethod]
        public Radnik SelectRadnik(int QV)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Radnik radnik = new Radnik();

            var query = from rad in dbase.Radniks
                        where rad.MBR == QV
                        select rad;

            radnik = query.SingleOrDefault();

            return radnik;
        }

        [WebMethod]
        public Radnik selectRadnikLogin(string username, string password)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Radnik radnik = new Radnik();

            var query = from rad in dbase.Radniks
                        where rad.UserName == username && rad.Pass == password
                        select rad;

            radnik = query.SingleOrDefault();

            return radnik;
        }

        [WebMethod]
        public void UpdateRadnik(int ID, int QV, string Ime, string Prezime, int OJ, string nazivRM, int sifraRM, bool pol)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Radnik radnik = new Radnik();

            var query = from rad in dbase.Radniks
                        where rad.ID == ID
                        select rad;

            radnik = query.SingleOrDefault<Radnik>();

            if (radnik != null)
            {
                radnik.MBR = QV;
                radnik.Ime = Ime;
                radnik.Prezime = Prezime;
                radnik.OJ = OJ;
                radnik.Sifra_rad_mesta = sifraRM;
                radnik.Naziv_rad_mesta = nazivRM;

                if (pol)
                    radnik.Pol = "m";
                else
                    radnik.Pol = "z";

                dbase.SaveChanges();
            }
        }

        [WebMethod]
        public void addNewRadnik(int QV, string Ime, string Prezime, int OJ, string nazivRM, int sifraRM, bool pol)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Radnik radnik = new Radnik();

            radnik.MBR = QV;
            radnik.Ime = Ime;
            radnik.Prezime = Prezime;
            radnik.OJ = OJ;
            radnik.Sifra_rad_mesta = sifraRM;
            radnik.Naziv_rad_mesta = nazivRM;

            if (pol)
                radnik.Pol = "m";
            else
                radnik.Pol = "z";

            dbase.Radniks.AddObject(radnik);
            dbase.SaveChanges();
        }

        [WebMethod]
        public void deleteRadnik(int QV)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Radnik radnik = new Radnik();

            var query = from rad in dbase.Radniks
                        where rad.MBR == QV
                        select rad;

            radnik = query.SingleOrDefault<Radnik>();

            if (radnik != null)
            {
                dbase.Radniks.DeleteObject(radnik);
                dbase.SaveChanges();
            }
        }

        [WebMethod]
        public Terminal updateTerminalRadnik(Radnik radnik, string IPAddressTerminal)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Terminal t = new Terminal();

            var query = from term in dbase.Terminals
                        where term.IPAdresa == IPAddressTerminal
                        select term;

            t = query.Single();

            if (radnik == null)
            {
                t.RadnikID = null;
            }
            else
                t.RadnikID = radnik.ID;

            dbase.SaveChanges();

            return t;
        }

        //set funkcija za odjavu radnika
        //*****************************************************************************************
        [WebMethod]
        public bool OdjaviRadnika(Radnik radnik, Terminal terminal)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            ARadnik ar = new ARadnik();

            var query = from rad in dbase.ARadniks
                        where rad.RadnikID == radnik.ID && rad.TerminalID == terminal.ID
                        orderby rad.VremePrijave descending
                        select rad;

            ar = query.First();

            ar.VremeOdjave = DateTime.Now;

            dbase.SaveChanges();

            //funkcija koja odjavluje radnika sa terminala
            Terminal t = updateTerminalRadnik(null, terminal.IPAdresa);

            return true;
        }
        //*****************************************************************************************
        #endregion

        #region Terminal
        //*****************************************************************************************
        //*****************************************************************************************
        //                          TERMINAL FUNKCIJE
        //*****************************************************************************************
        //*****************************************************************************************

        [WebMethod]
        public Masina getLastState(String IPAddress, out Propis propis, out Propis[] propisi, out Terminal terminal, out Radnik radnik)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            //Terminal terminal;
            //ukoliko nema prijavljenih propisa
            propis = null;
            propisi = null;
            radnik = null;

            var termQuery = from term in dbase.Terminals
                            where term.IPAdresa == IPAddress
                            select term;

            terminal = termQuery.Single();

            if (terminal.Radnik != null)
            {
                Terminal terminalTmp = terminal;
                var radnikQuery = from rad in dbase.Radniks
                                  where rad.ID == terminalTmp.RadnikID
                                  select rad;
                radnik = radnikQuery.Single();
            }

            Masina masina;

            masina = terminal.Masina;

            if (masina != null && masina.PropisID != null)
            {
                Terminal terminalTmp = terminal;
                var propisQuery = from prop in dbase.Propis1
                                  where prop.ID == terminalTmp.Masina.PropisID
                                  select prop;
                propis = propisQuery.Single();

                var propisiQuery = from polupr in dbase.Poluproizvods
                                   where polupr.PropisID == terminalTmp.Masina.PropisID
                                   select polupr.Propi;
                propisi = propisiQuery.ToArray<Propis>();

            }
            return masina;
        }

        #endregion

        #region Propis
        //*****************************************************************************************
        //*****************************************************************************************
        //                          PROPIS FUNKCIJE
        //*****************************************************************************************
        //*****************************************************************************************

        //selektovanje propisa
        //*****************************************************************************************
        [WebMethod]
        public Propis selectPropisID(int propisID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Propis propis = new Propis();

            var query = from prop in dbase.Propis1
                        where prop.ID == propisID
                        select prop;

            propis = query.FirstOrDefault();

            return propis;
        }

        [WebMethod]
        public Propis selectPropis(string propisSifra, int RJ)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Propis propis = new Propis();

            var query = from prop in dbase.Propis1
                        where prop.Sifra == propisSifra && prop.RJ == RJ
                        orderby prop.DatumKreiranja descending
                        select prop;

            propis = query.FirstOrDefault();

            return propis;
        }

        [WebMethod]
        public bool selectPropisPrijava(int propisID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Propis propis = new Propis();

            var query = from prop in dbase.Propis1
                        where prop.ID == propisID
                        select prop;

            propis = query.SingleOrDefault();

            Propis propisReturn = new Propis();

            var query1 = from p in dbase.Propis1
                         where p.Sifra == propis.Sifra && p.RJ == propis.RJ
                         orderby p.DatumKreiranja descending
                         select p;
            propisReturn = query1.FirstOrDefault();

            if (propis.ID == propisReturn.ID)
                return true;
            else return false;
        }

        [WebMethod]
        public List<Propis> selectPropisVerzijaList(string propisSifra, int RJ)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Propis> propisList = new List<Propis>();

            var query = from prop in dbase.Propis1
                        where prop.Sifra == propisSifra && prop.RJ == RJ
                        orderby prop.DatumKreiranja descending
                        select prop;

            propisList = query.ToList();

            return propisList;
        }

        [WebMethod]
        public void deletePropis(int propisID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Poluproizvod> poluproizvodList = SelectPoluproizvodList(propisID);
            foreach (Poluproizvod p in poluproizvodList)
            {
                dbase.DetectChanges();
                dbase.Poluproizvods.Attach(p);
                dbase.DeleteObject(p);
                dbase.SaveChanges();
            }

            Propis propis = new Propis();

            var query = from prop in dbase.Propis1
                        where prop.ID == propisID
                        select prop;

            propis = query.SingleOrDefault<Propis>();

            dbase.DeleteObject(propis);
            dbase.SaveChanges();
        }

        [WebMethod]
        public List<Propis> selectPropisPoluproizvodList(int propisID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Propis> PropisList = new List<Propis>();

            var query = from polu in dbase.Poluproizvods
                        where polu.PropisID == propisID
                        select polu.Propi;
            PropisList = query.ToList<Propis>();

            return PropisList;

        }

        /// <summary>
        /// Return propis List , RJ=RadnaJedinica
        /// </summary>
        /// <param name="RJ"></param>
        /// <returns></returns>
        [WebMethod]
        public List<Propis> selectPropisList(int RJ)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Propis> propisList = new List<Propis>();

            var query = from pr in dbase.Propis1
                        where pr.RJ == RJ
                        select pr;
            propisList = query.ToList<Propis>();

            return propisList;
        }

        //TODO dodavanje poluproizvoda
        [WebMethod]
        public void insertPropis
            (String sifraPropisa, String opis, String tipMasine, int radnaJedinica, List<Propis> poluproizvodi, int Verzija)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Propis propis = new Propis();

            propis.Sifra = sifraPropisa;
            propis.Opis = opis;
            propis.RJ = radnaJedinica;
            propis.TipMasine = tipMasine;
            propis.Verzija = Verzija;
            propis.DatumKreiranja = DateTime.Now;

            dbase.Propis1.AddObject(propis);
            dbase.SaveChanges();

            foreach (Propis p in poluproizvodi)
            {
                Poluproizvod poluproizvod = new Poluproizvod();
                poluproizvod.PropisID = propis.ID;
                poluproizvod.PoluproizvodID = p.ID;

                dbase.Poluproizvods.AddObject(poluproizvod);
                dbase.SaveChanges();
            }
        }

        //Set funkcija koje se izvrsavaju priprijavi propisa
        //*****************************************************************************************
        [WebMethod]
        public Propis PrijavaPropisa(int PropisID, int MasinaID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            var query = from prop in dbase.Propis1
                        where prop.ID == PropisID
                        select prop;

            Propis propis = query.SingleOrDefault();

            //insert apropis pri prijavi propisa
            if (propis != null)
            {
                InsertAPropis(PropisID, MasinaID);
                UpdateMasinaPropis(MasinaID, PropisID);
                DeleteTrenutniPoluproizvodi(MasinaID);
            }

            return propis;
        }

        [WebMethod]
        public void InsertAPropis(int PropisID, int MasinaID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            APropis ap = new APropis();

            ap.MasinaID = MasinaID;
            ap.PropisID = PropisID;
            ap.Vreme = DateTime.Now;

            dbase.APropis1.AddObject(ap);
            dbase.SaveChanges();
        }

        [WebMethod]
        public List<Poluproizvod> SelectPoluproizvodList(int PropisID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Poluproizvod> poluproizvodList = new List<Poluproizvod>();

            var query = from pp in dbase.Poluproizvods
                        where pp.PropisID == PropisID
                        select pp;

            poluproizvodList = query.ToList<Poluproizvod>();

            dbase.Dispose();

            return poluproizvodList;
        }

        [WebMethod]
        public Propis selectPropisProizvod(Int64 barcode)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Propis propis = new Propis();

            var query = from pr in dbase.Proizvods
                        where pr.BarKod == barcode
                        select pr.Propi;

            propis = query.Single<Propis>();

            return propis;
        }
        //*****************************************************************************************
        #endregion

        #region UpravljanjeRegalima
        //*****************************************************************************************
        //*****************************************************************************************
        //                          REGALI FUNKCIJE
        //*****************************************************************************************
        //*****************************************************************************************

        //funkcija za izbor ID regala
        //*****************************************************************************************
        [WebMethod]
        public int findAvailableRegalID(int regalID)
        {
            int j = regalID;
            TraceabilityEntities dbase = new TraceabilityEntities();
            Regal regal = new Regal();
            var query = from reg in dbase.Regals
                        where reg.ID == regalID
                        select reg;

            regal = query.SingleOrDefault<Regal>();

            while (regal != null)
            {
                j++;
                var query1 = from reg in dbase.Regals
                             where reg.ID == j
                             select reg;
                regal = query1.SingleOrDefault<Regal>();
            }

            return j;
        }
        //*****************************************************************************************

        //funkcija za dodavanje novog regala
        //*****************************************************************************************
        [WebMethod]
        public void insertRegal(int regalID, int Colaza, int Kapacitet, int RJ, string opis)
        {
            Regal regal = new Regal();

            regal.ID = regalID;
            regal.Colaza = Colaza;
            regal.RJ = RJ;
            regal.Opis = opis;
            regal.Kapacitet = Kapacitet;
            regal.VremePunjenje = DateTime.Now;

            TraceabilityEntities dbase = new TraceabilityEntities();
            dbase.Regals.AddObject(regal);
            dbase.SaveChanges();

        }
        //*****************************************************************************************
        /// <summary>
        /// nalazi prazni regal
        /// </summary>
        /// <param name="RJ"></param>
        /// <returns></returns>
        public Regal findPrazanRegal(TraceabilityEntities dbase, int RJ, String tipRegala, int? colaza, String tipPozicije)
        {

            var query = (from reg in dbase.Regals
                         where reg.Ostecen != 2 && reg.Zakljucan == 0
                                && reg.StatusIzvrsavanja == false
                                && reg.Pozicija != null
                                && reg.Pozicija.RJ == RJ
                                && reg.Pozicija.TipPozicije == "I"
                                && reg.Opis == tipRegala
                                && reg.Colaza == colaza
                                && reg.Pozicija.TipPozicije == tipPozicije
                                && reg.BrojProizvoda == 0
                         orderby reg.VremePunjenje ascending
                         select reg).Take(1);

            return query.FirstOrDefault<Regal>();
        }
        /// <summary>
        /// nalazi pun regal
        /// </summary>
        /// <param name="RJ"></param>
        /// <param name="propisID"></param>
        /// <param name="tipPozicije"><i>I</i> - za internu /n <i>M</i> - pozicija uz masinu</param>
        /// <returns></returns>
        public Regal findPunRegal(int RJ, int propisID, String tipPozicije, String tipRegala, TraceabilityEntities dbase)
        {

            var query = (from reg in dbase.Regals
                         where reg.Ostecen != 2 && reg.Zakljucan == 0
                                && reg.StatusIzvrsavanja == false
                                && reg.Pozicija.TipPozicije == tipPozicije
                                && reg.Pozicija.RJ == RJ
                                && reg.Opis == tipRegala
                                && reg.PropisID == propisID
                                && reg.BrojProizvoda > 0
                         orderby reg.VremePunjenje ascending
                         select reg).Take(1);

            return query.FirstOrDefault<Regal>();
        }


        //funkcija za prijavu regala
        //*****************************************************************************************
        [WebMethod]
        public Regal SelectRegal(int RegalID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = new Regal();

            var query = from reg in dbase.Regals
                        where reg.ID == RegalID
                        select reg;

            regal = query.SingleOrDefault<Regal>();

            return regal;
        }
        //*****************************************************************************************

        //funkcija za vezivanje propisa za regal
        //*****************************************************************************************
        [WebMethod]
        public void updateRegalPropis(int RegalID, int PropisID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = new Regal();

            var query = from reg in dbase.Regals
                        where reg.ID == RegalID
                        select reg;

            regal = query.SingleOrDefault<Regal>();

            regal.PropisID = PropisID;
            dbase.SaveChanges();
        }

        /// <summary>
        /// metoda za Update broja proizvoda na regalu, koristi se u slucaju pomeranja nepraznog regala
        /// </summary>
        /// <param name="regal"></param>
        /// <param name="brojProizvoda"></param>
        /// <returns></returns>
        [WebMethod]
        public bool updateRegalBrojProizvoda(int regalID, int brojProizvoda)
        {
            bool result = false;
            TraceabilityEntities dbase = new TraceabilityEntities();

            var query = from regal in dbase.Regals
                        where regal.ID == regalID
                        select regal;

            Regal regtmp = query.FirstOrDefault();

            if (regtmp != null)
            {
                regtmp.BrojProizvoda = brojProizvoda;
                if (brojProizvoda == 0)
                {
                    regtmp.Propi = null;
                    regtmp.isEmpty = true;
                }
                if (dbase.SaveChanges() != 0)
                    result = true;
            }
            return result;
        }
        //*****************************************************************************************
        [WebMethod]
        public void updateRegal(Regal rack)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = new Regal();

            var query = from reg in dbase.Regals
                        where reg.ID == rack.ID
                        select reg;

            regal = query.SingleOrDefault<Regal>();

            regal.Colaza = rack.Colaza;
            regal.BrojProizvoda = rack.BrojProizvoda;
            regal.Opis = rack.Opis;

            dbase.SaveChanges();
        }
        //*****************************************************************************************

        //funkcija za cekiranje punog regala
        //TODO set funkcija koji se desi kada se napuni regal
        //*****************************************************************************************
        [WebMethod]
        public void updateRegalPopunjen(Regal regal, int masinaID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regTmp = new Regal();

            var query = from reg in dbase.Regals
                        where reg.ID == regal.ID
                        select reg;

            var queryMasina = from mas in dbase.Masinas
                              where mas.ID == masinaID
                              select mas.Propi;

            Propis propis = queryMasina.SingleOrDefault<Propis>();

            regTmp = query.Single<Regal>();

            regTmp.isEmpty = false;
            regTmp.BrojProizvoda = regal.BrojProizvoda;
            regTmp.PropisID = propis.ID;
            regTmp.VremePunjenje = DateTime.Now;

            dbase.SaveChanges();
        }

        //funkcija za cekiranje praznog regala
        [WebMethod]
        public void updateRegalPrazan(int RegalID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = new Regal();

            var query = from reg in dbase.Regals
                        where reg.ID == RegalID
                        select reg;

            regal = query.SingleOrDefault<Regal>();

            regal.isEmpty = true;
            regal.BrojProizvoda = 0;
            //regal.VremePunjenje = DateTime.Now;

            dbase.Attach(regal);
            dbase.SaveChanges();
        }

        [WebMethod]
        public bool updateRegalPozicija(int regalID, int posFrom, int posTo)
        {
            bool result = false;
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = new Regal();
            Pozicija positionFrom = new Pozicija();
            Pozicija positionTo = new Pozicija();

            var query = from reg in dbase.Regals
                        where reg.ID == regalID
                        select reg;
            regal = query.Single<Regal>();

            var queryPosFrom = from poz1 in dbase.Pozicijas
                               where poz1.ID == posFrom
                               select poz1;
            positionFrom = queryPosFrom.Single<Pozicija>();

            var queryPosTo = from poz2 in dbase.Pozicijas
                             where poz2.ID == posTo
                             select poz2;

            positionTo = queryPosTo.Single<Pozicija>();

            if (positionTo.KapacitetPreostalo != 0 && positionTo.Zakljucana == 0)
            {
                regal.PozicijaID = positionTo.ID;
                positionTo.KapacitetPreostalo--;
                positionFrom.KapacitetPreostalo++;
                regal.StatusIzvrsavanja = false;

                //nadji zahteve za odvozenje do masine i ispuni jedan
                var queryZahtevs = from zht in dbase.Zahtevs
                                   where zht.Pozicija_DO == posTo
                                   orderby zht.VremeKreiranja ascending
                                   select zht;
                Zahtev tmp = queryZahtevs.FirstOrDefault();
                if (tmp != null)
                    dbase.DeleteObject(tmp);

                //nadji zahteve za dovozenje do masine i ispuni prvi
                queryZahtevs = from zht in dbase.Zahtevs
                               where zht.Pozicija_OD == posFrom
                               orderby zht.VremeKreiranja ascending
                               select zht;

                Zahtev tmp2 = queryZahtevs.FirstOrDefault();
                if (tmp2 != null)
                    dbase.DeleteObject(tmp2);


                if (dbase.SaveChanges() != 0)
                    result = true;
            }
            return result;
        }

        [WebMethod]
        public bool regalZakljucan(int regalID)
        {
            bool result = false;
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = (from reg in dbase.Regals
                           where reg.ID == regalID
                           select reg).FirstOrDefault();

            if (regal != null)
                result = !(regal.Ostecen == 2 || regal.Zakljucan == 1);

            return result;


        }

        [WebMethod]
        public void deleteRegal(Regal rack)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = new Regal();

            var query = from reg in dbase.Regals
                        where reg.ID == rack.ID
                        select reg;

            regal = query.SingleOrDefault<Regal>();

            dbase.Regals.Attach(regal);
            dbase.DeleteObject(regal);
            dbase.SaveChanges();
        }

        [WebMethod]
        public List<Regal> selectRegalPropisList(string tipRegala, out List<Propis> propisList)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Regal> regalList = new List<Regal>();

            var query = from reg in dbase.Regals
                        where reg.Opis == tipRegala && reg.PropisID != null
                        select reg;
            regalList = query.ToList<Regal>();


            propisList = new List<Propis>();

            var queryPropis = from regalTmp in dbase.Regals
                              where regalTmp.Opis == tipRegala && regalTmp.PropisID != null
                              select regalTmp.Propi;
            propisList = queryPropis.ToList<Propis>();

            return regalList;
        }


        #endregion

        #region Zahtev
        //*****************************************************************************************
        //*****************************************************************************************
        //                          ZAHTEV FUNKCIJE
        //*****************************************************************************************
        //*****************************************************************************************

        //funkcija za kreiranje zahteva na osnovu punog regala
        [WebMethod]
        public void insertZahtevPunRegal(Regal punRegal, int masinaID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            var queryMasina = from mas in dbase.Masinas
                              where mas.ID == masinaID
                              select mas.Propi;
            
            Propis propis = queryMasina.SingleOrDefault<Propis>();

            Zahtev z = new Zahtev();

            z.Pozicija_OD = punRegal.PozicijaID; //pozicija sa koje se regal uzima
            z.RegalID = punRegal.ID; //ID regala koji se pomera
            z.PropisID = propis.ID;
            z.StatusIzvrsavanja = 0;
            z.VremeKreiranja = DateTime.Now;

            dbase.Zahtevs.AddObject(z);
            dbase.SaveChanges();

            z.updateText();

            dbase.SaveChanges();
        }
        //*****************************************************************************************

        /// <summary>
        /// funkcija koja selektuje zahtev iz liste zahteva i menja status zahteva u status <i>SELEKTOVAN</i>
        /// funkcija vraca skup zahteva koji su selektovani jer se moze desiti da jednim zahtevom selektujemo
        /// jos neki povezani zahtev
        /// </summary>
        /// <param name="selectedZahtev"></param>
        /// <returns></returns>
        [WebMethod]
        public Zahtev selectZahtev(long zahtevID, int terminaID, out Zahtev[] povezaniZahtevi)
        {
            Zahtev result = null;

            List<Zahtev> povezani = new List<Zahtev>();

            Zahtev selectedZahtev;
            using (TraceabilityEntities dbase = new TraceabilityEntities())
            {

                var queryZahtev = from zht in dbase.Zahtevs
                                  where zht.ID == zahtevID
                                  select zht;
                selectedZahtev = queryZahtev.SingleOrDefault();


                //ukoliko je zahtev za odvozenje regala od masine
                if (selectedZahtev.Pozicija_OD != null && selectedZahtev.Pozicija_DO == null)
                {

                    selectedZahtev.StatusIzvrsavanja = 1;

                    selectedZahtev.Regal.StatusIzvrsavanja = true;
                    result = selectedZahtev;
                }
                //u suprotnom je zahtev za dovozenje do masine
                //tada racunamo lokaciju sa koje treba da dovezemo regal
                else
                {
                    //ako imamo selektovan propis onda dovozimo pun regal
                    if (selectedZahtev.Propi != null)
                    {
                        Regal regal = new Regal();
                        //ako je pozicija ulazna
                        if (selectedZahtev.Pozicija1.Ulazna != null && (bool)selectedZahtev.Pozicija1.Ulazna)
                        {
                            regal = findPunRegal(selectedZahtev.Pozicija1.RJ - 1, (int)selectedZahtev.PropisID,
                                                "I", selectedZahtev.Pozicija1.TipRegala.Trim(), dbase);


                            if (regal != null)
                            {
                                selectedZahtev.Pozicija = regal.Pozicija;
                                selectedZahtev.Regal = regal;

                                InternalFunctions.prebaciUStanje1(selectedZahtev);

                                result = selectedZahtev;
                            }
                            //ako nije pronadjen regal u skladistu, gleda se da li postoji neki regal na nekoj od masina
                            //koji ceka da se pomeri
                            else
                            {
                                Zahtev zahtevZaOdvozenje = postojiRegalNaMasini(selectedZahtev.Pozicija1.TipRegala.Trim(),
                                                                                Convert.ToInt32(selectedZahtev.PropisID),
                                                                                terminaID,
                                                                                dbase);

                                if (zahtevZaOdvozenje != null)
                                {
                                    selectedZahtev.Pozicija = zahtevZaOdvozenje.Pozicija;
                                    selectedZahtev.Regal = zahtevZaOdvozenje.Regal;

                                    InternalFunctions.prebaciUStanje1(selectedZahtev);

                                    zahtevZaOdvozenje.StatusIzvrsavanja = 3;
                                    result = selectedZahtev;
                                    povezani.Add(zahtevZaOdvozenje);

                                }

                            }

                        }
                        // ako je pozicija izlazna
                        else
                        {
                            regal = findPrazanRegal(dbase, selectedZahtev.Pozicija1.RJ, selectedZahtev.Pozicija1.TipRegala,
                                                    selectedZahtev.Colaza, "I");
                        }
                        if (regal != null)
                        {
                            selectedZahtev.Regal = regal;
                            selectedZahtev.Pozicija = regal.Pozicija;
                            selectedZahtev.Propi = regal.Propi;
                            InternalFunctions.prebaciUStanje1(selectedZahtev);

                            result = selectedZahtev;

                        }
                        else
                            result = null;
                    }//u suprotnom nalazimo prazan regal
                    else
                    {
                        Regal regal = findPrazanRegal(dbase, selectedZahtev.Pozicija1.RJ, selectedZahtev.Pozicija1.TipRegala,
                                                selectedZahtev.Colaza, "I");
                        if (regal != null)
                        {
                            selectedZahtev.RegalID = regal.ID;

                            selectedZahtev.Pozicija = regal.Pozicija;

                            selectedZahtev.Propi = regal.Propi;

                            InternalFunctions.prebaciUStanje1(selectedZahtev);

                            result = selectedZahtev;

                        }
                    }
                }


                if (dbase.SaveChanges() == 0)
                {
                    result = null;
                }

            }


            povezaniZahtevi = povezani.ToArray();
            if (result != null)
                return result;
            else
                return null;
        }



        [WebMethod]
        public bool potrebnoIzabratiTipRegala(int? pozicijaID)
        {
            TraceabilityEntities db = new TraceabilityEntities();
            Pozicija pozicija = (from poz in db.Pozicijas
                                 where poz.ID == pozicijaID
                                 select poz).FirstOrDefault();

            if (pozicija != null && pozicija.Masina != null && pozicija.Masina.TipMasine == "T")
                return true;
            else
                return false;
        }



        [WebMethod]
        public Regal getRackByType(int colaza, int RJ, String tipPozicije)
        {
            TraceabilityEntities db = new TraceabilityEntities();

            Regal rack = null;

            if (colaza == 13 || colaza == 14)
            {
                var query = from rck in db.Regals
                            where (rck.Colaza == colaza || rck.Colaza == 34)
                            && rck.RJ == RJ
                            && rck.BrojProizvoda == 0
                            && rck.Pozicija.TipPozicije == tipPozicije.Trim()
                            && rck.StatusIzvrsavanja == false
                            orderby rck.VremePunjenje ascending
                            select rck;

                rack = query.FirstOrDefault();
            }
            else
            {
                var query = from rck in db.Regals
                            where rck.Colaza == colaza
                            && rck.RJ == RJ
                            && rck.BrojProizvoda == 0
                            && rck.Pozicija.TipPozicije == tipPozicije.Trim()
                            && rck.StatusIzvrsavanja == false
                            orderby rck.VremePunjenje ascending
                            select rck;

                rack = query.FirstOrDefault();
            }


            if (rack != null)
            {
                rack.StatusIzvrsavanja = true;
                db.SaveChanges();
            }
            return rack;
        }
        [WebMethod]
        public void releaseRegal(int rackID)
        {
            TraceabilityEntities db = new TraceabilityEntities();

            (from rck in db.Regals
             where rck.ID == rackID
             select rck).First().StatusIzvrsavanja = false;

            db.SaveChanges();
        }

        [WebMethod]
        public String getPositionName(int posID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();
            var query = from pos in dbase.Pozicijas
                        where pos.ID == posID
                        select pos.KodPozicije;

            return query.Single();
        }

        [WebMethod]
        public bool posIsUlazna(int posID)
        {
            TraceabilityEntities db = new TraceabilityEntities();

            var query = from pos in db.Pozicijas
                        where pos.ID == posID
                        select pos;

            return query.First().Ulazna == true;

        }

        /// <summary>
        /// Otkazuje selektovan zahtev
        /// </summary>
        /// <param name="canceledZahtev"></param>
        /// <returns></returns>
        [WebMethod]
        public bool cancelZahtev(Zahtev initialZahtev, Zahtev[] povezaniZahtevi)
        {


            bool result = false;
            if (initialZahtev != null)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (TraceabilityEntities dbase = new TraceabilityEntities())
                    {

                        var query = from zahtev in dbase.Zahtevs
                                    where zahtev.ID == initialZahtev.ID
                                    select zahtev;
                        Zahtev tmpZahtev = query.First();

                        tmpZahtev.StatusIzvrsavanja = 0;
                        if (tmpZahtev.Regal != null)
                            tmpZahtev.Regal.StatusIzvrsavanja = false;
                        //ukoliko je proces kreiran u svrhu dovozenja do masine
                        tmpZahtev.Colaza = initialZahtev.Colaza;
                        tmpZahtev.Pozicija_OD = initialZahtev.Pozicija_OD;
                        tmpZahtev.Pozicija_DO = initialZahtev.Pozicija_DO;
                        tmpZahtev.PropisID = initialZahtev.PropisID;
                        tmpZahtev.RegalID = initialZahtev.RegalID;

                        dbase.SaveChanges();


                        if (tmpZahtev.Pozicija1 != null)
                            tmpZahtev.Pozicija1.Zakljucana = 0;
                        if (tmpZahtev.Pozicija != null)
                            tmpZahtev.Pozicija.Zakljucana = 0;

                        if (povezaniZahtevi != null)
                        {
                            foreach (Zahtev povezaniZahtev in povezaniZahtevi)
                            {
                                (from zht in dbase.Zahtevs
                                 where zht.ID == povezaniZahtev.ID
                                 select zht).First().StatusIzvrsavanja = 0;
                            }
                        }

                        dbase.SaveChanges();


                        scope.Complete();

                    }
                }

            }
            else
                result = true;
            return result;
        }


        //funkcija koja stavlja zahtev za odvozenje regala u status izvrsenja
        //takodje nalazi povezani zahtev ako postoji zahtev za odvozenje regala direktno na masinu
        [WebMethod]
        public Zahtev updateZahtevOdvezi(long zahtevID, int terminalID)
        {
            Zahtev zahtevOdvezi = null;
            using (TraceabilityEntities dbase = new TraceabilityEntities())
            {
                //najblizu praznu poziciju
                zahtevOdvezi = (from zht in dbase.Zahtevs
                                where zht.ID == zahtevID
                                select zht).FirstOrDefault();
                Zahtev povezanZahtev;//ukoliko izvrsavanje zahteva izvrsava jos jedan zahtev, selektujemo i taj zahtev
                Pozicija pozicijaDO = NajblizaPraznaPozicija(zahtevOdvezi.Pozicija, zahtevOdvezi.Regal.Propi, terminalID, zahtevOdvezi.Regal.Opis.Trim(), out povezanZahtev, dbase);
                if (pozicijaDO != null)
                {

                    zahtevOdvezi.Pozicija.Zakljucana = 0;
                    zahtevOdvezi.Pozicija1 = pozicijaDO;
                    zahtevOdvezi.Pozicija1.Zakljucana = 1;
                    zahtevOdvezi.StatusIzvrsavanja = 2;
                    zahtevOdvezi.Pozicija.KapacitetPreostalo++;

                    zahtevOdvezi.Regal.PozicijaID = null;
                    zahtevOdvezi.Regal.StatusIzvrsavanja = true;

                    if (povezanZahtev != null)
                        povezanZahtev.StatusIzvrsavanja = 3;
                    dbase.SaveChanges();

                    //vrsi update teksta zahteva
                    zahtevOdvezi.updateText();
                    zahtevOdvezi.updateOcekivaniBarKod();

                    dbase.SaveChanges();
                    //uklanja sve ostale zahteve iz liste zahteva koji su zahtevali dovozenje do date pozicije
                    removeZahteviByPosDo(pozicijaDO.ID);


                    //kad pomerimo regal, automatski se kreira zahtev za novim regalom
                    if (zahtevOdvezi.Pozicija.Masina.TipMasine == "T")
                        insertZahtevPraznaPozicija(zahtevOdvezi.Pozicija, -1);
                    else
                        insertZahtevPraznaPozicija(zahtevOdvezi.Pozicija, (int)zahtevOdvezi.PropisID);
                }
                else
                    zahtevOdvezi = null;
            }


            //zakljucavanje pozicija
            return zahtevOdvezi;
        }

        //funkcija koja zakljucava pozicije sa kojih se vozi regal
        [WebMethod]
        public bool zakljucajPozicije(Pozicija pozicijaOD, Pozicija pozicijaDO)
        {

            bool result = false;
            try
            {
                using (TraceabilityEntities dbase = new TraceabilityEntities())
                {
                    dbase.Pozicijas.Attach(pozicijaOD);
                    dbase.Pozicijas.Attach(pozicijaDO);

                    pozicijaDO.Zakljucana = 1;
                    pozicijaOD.Zakljucana = 1;

                    dbase.SaveChanges();
                    result = true;
                }
            }
            catch (Exception e)
            {
                Log(e.Message);
            }

            return result;
        }

        /// <summary>
        /// stavlja zahtev u status izvrsen. Ako je odrediste u medjuvremenu zakljucano vraca azuriran zahtev sa novom pozicijom
        /// </summary>
        /// <param name="zahtevID"></param>
        /// <returns></returns>
        [WebMethod]
        public Zahtev updateZahtevIzvrsen(long zahtevID)
        {

            TraceabilityEntities dbase = new TraceabilityEntities();
            Zahtev zahtev = (from zht in dbase.Zahtevs
                             where zht.ID == zahtevID
                             select zht).FirstOrDefault(); ;

            if (zahtev != null)
            {
                if (zahtev.Pozicija1.Zakljucana == 2)
                {

                    Pozicija novaPozicija = NajblizaInternaPraznaPozicija(zahtev.Pozicija1, zahtev.Regal.Opis, dbase);
                    zahtev.Pozicija1 = novaPozicija;
                    zahtev.Pozicija1.Zakljucana = 1;

                }
                else
                {
                    zahtev.Regal.PozicijaID = (int)zahtev.Pozicija_DO;
                    zahtev.Regal.StatusIzvrsavanja = false;
                    zahtev.Regal.VremePoslednjegPomeraja = DateTime.Now;
                    zahtev.Pozicija.Zakljucana = 0;
                    zahtev.Pozicija1.Zakljucana = 0;
                    zahtev.Pozicija1.KapacitetPreostalo--;

                    //otkljucavamo regal
                    //otkljucavamo pozicije za koriscenje
                    zahtev.StatusIzvrsavanja = 3; //status postavljamo na zavrsen.
                }
                if (dbase.SaveChanges() == 0)
                    zahtev = null;
            }


            return zahtev;
        }


        [WebMethod]
        public bool izvrsiPovezaneZahteve(Regal regal)
        {
            bool result = false;

            TraceabilityEntities db = new TraceabilityEntities();
            var queryZahtevi = from zahtev in db.Zahtevs
                               where zahtev.Pozicija_DO == regal.PozicijaID &&
                               zahtev.StatusIzvrsavanja == 0 &&
                               zahtev.PropisID == regal.PropisID
                               select zahtev;

            Zahtev povezaniZahtev = queryZahtevi.FirstOrDefault();

            if (povezaniZahtev != null)
            {
                povezaniZahtev.StatusIzvrsavanja = 3;


            }
            if (db.SaveChanges() >= 0) result = true;
            return result;
        }
        //funkcija za kreiranje zahteva za dovozenje regala na praznu poziciju pored masine
        //*****************************************************************************************
        [WebMethod]
        public bool insertZahtevPraznaPozicija(Pozicija pozicijaDovezi, int idPropis)
        {
            bool result = false;
            TraceabilityEntities dbase = new TraceabilityEntities();

            Pozicija odrediste = (from poz in dbase.Pozicijas
                                  where poz.ID == pozicijaDovezi.ID
                                  select poz).FirstOrDefault();

            Zahtev z = new Zahtev();

            z.Pozicija_DO = pozicijaDovezi.ID;
            z.StatusIzvrsavanja = 0;
            if (idPropis != -1)
                z.PropisID = idPropis;
            z.VremeKreiranja = DateTime.Now;

            z.NazivMasine = odrediste.Masina.SifraMasine;
            

            dbase.Zahtevs.AddObject(z);
            dbase.SaveChanges();

            z.updateText();

            dbase.SaveChanges();

            result = true;

            return result;
        }

        /// /// <summary>
        /// Vrsi update zahteva koji dovozi regal do masine postavljajuci status izvrsavanja
        /// </summary>
        /// <param name="zahtev"></param>
        /// <returns></returns>
        [WebMethod]
        public Zahtev updateZahtevDovezi(long zahtevID)
        {
            Zahtev tmpZahtev = null;
            
            using (TraceabilityEntities dbase = new TraceabilityEntities())
            {
                var query = from zht in dbase.Zahtevs
                            where zht.ID == zahtevID
                            select zht;

                tmpZahtev = query.First();
                tmpZahtev.StatusIzvrsavanja = 2;
                
                tmpZahtev.Pozicija.KapacitetPreostalo++;

                tmpZahtev.updateOcekivaniBarKod();
                tmpZahtev.updateText();
                dbase.SaveChanges();
            }

            return tmpZahtev;
        }
        
              
        

        [WebMethod]
        public bool updateZahtev(Zahtev zahtev)
        {
            bool result = false;
            using (TraceabilityEntities dbase = new TraceabilityEntities())
            {
                Zahtev updatingZahtev = (from zht in dbase.Zahtevs
                                         where zht.ID == zahtev.ID
                                         select zht).FirstOrDefault();
                
                if (updatingZahtev != null)
                {
                    updatingZahtev.Pozicija_DO = zahtev.Pozicija_DO;
                    updatingZahtev.Pozicija_OD = zahtev.Pozicija_OD;
                    updatingZahtev.PropisID = zahtev.PropisID;
                    updatingZahtev.RegalID = zahtev.RegalID;
                    updatingZahtev.Colaza = zahtev.Colaza;

                    result = dbase.SaveChanges() >= 0;
                }

            }

            return result;
        }
        [WebMethod]
        public String getZahtevStrings(long idZahteva, out String pozicijaOD, out String pozicijaDO)
        {

            TraceabilityEntities db = new TraceabilityEntities();
            Zahtev zahtev = (from zht in db.Zahtevs
                             where zht.ID == idZahteva
                             select zht).First();

            pozicijaOD = null;
            pozicijaDO = null;

            if (zahtev.Pozicija != null)
                pozicijaOD = zahtev.Pozicija.KodPozicije;
            if (zahtev.Pozicija1 != null)
                pozicijaDO = zahtev.Pozicija1.KodPozicije;

            String nazivRegala = zahtev.Regal == null ? null : zahtev.Regal.NazivRegala;
            return nazivRegala;

        }


        /// <summary>
        /// Vraca listu tekstova zahteva koji se prikazuju kod klijenta, i vraca listu zahteva
        /// a kao ulazne parametre prihvata listu zahteva u koje ce izvrsiti upis, kao i listu masina
        /// koje opsluzuje viljuskar, a zatim u tu listu upisuje naziv masine za koju je odgovarajuci zahtev u povratnoj listi
        /// </summary>
        /// <param name="zahtevi"> ulazno izlazni parametar u kome se prosledjuje prazna lista a vraca lista zahteva</param>
        /// <param name="masine">kao ulazni parametar zahteva listu masina koje opsluzuje viljuskar </param>
        /// <param name="masineZaOpsluzivanje">a kao izlazni vraca niz masina koji odgovara listi zahteva</param>
        /// <returns> vraca listu tekstova zahteva</returns>
        [WebMethod]
        public Zahtev[] getAllZahtevi(long terminalID)
        {

            Zahtev[] zahtevi = null;


            using (TraceabilityEntities dbase = new TraceabilityEntities())
            {

                //preuzima sve zahteve koji su u statusu izvrsavanja i cije pozicije Od ili pozicije Do pripadaju
                //masinama koje viljuskar opsluzuje

                var query = from zahtev in dbase.Zahtevs
                            from viljuskarLinija in dbase.ViljuskarLinija
                            from linijaPozicija in dbase.LinijaPozicija
                            where viljuskarLinija.TerminalID == terminalID
                            && ((
                                    (zahtev.Pozicija == linijaPozicija.Pozicija && linijaPozicija.Linija == viljuskarLinija.Linija)
                                    ||
                                    (zahtev.Pozicija1 == linijaPozicija.Pozicija && linijaPozicija.Linija == viljuskarLinija.Linija))
                                )
                            && zahtev.StatusIzvrsavanja == 0
                            && (zahtev.Pozicija1.Zakljucana == 0 ||
                                zahtev.Pozicija.Zakljucana == 0)
                            orderby zahtev.VremeKreiranja ascending
                            select zahtev;




                zahtevi = query.ToArray<Zahtev>();




            }



            return zahtevi;
        }

        /// /// <summary>
        /// prikuplja zahteve za prevoz u zutu zonu
        /// </summary>
        /// <param name="zahtevi"></param>
        /// <returns></returns>
        [WebMethod]
        public Zahtev[] getYellowZoned(int terminalID)
        {

            Zahtev[] zahtevi = null;
            try
            {
                TraceabilityEntities dbase = new TraceabilityEntities();

                var query = from zht in dbase.Zahtevs
                            where zht.StatusIzvrsavanja == 0 && zht.Regal.Zakljucan == 1 && zht.Regal.Ostecen == 0
                            select zht;

                zahtevi = query.ToArray<Zahtev>();

            }
            catch (Exception e)
            {
                Log(e.Message);

            }
            return zahtevi;
        }



        #endregion

        #region UpravljanjeOstecenimRegalima
        //*****************************************************************************************
        //*****************************************************************************************
        //                          UPRAVLJANJE OSTECENIM REGALIMA FUNKCIJE
        //*****************************************************************************************
        //*****************************************************************************************

        //funkcija za zakljucavanje ostecenih regala
        //*****************************************************************************************
        [WebMethod]
        public void updateRegalPrijavljen(Regal ostecenRegal)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            dbase.Regals.Attach(ostecenRegal);

            ostecenRegal.Ostecen = 1;

            dbase.SaveChanges();
        }

        //funkcija za proveru regala koji su oznaceni kao osteceni
        //*****************************************************************************************
        [WebMethod]
        public List<Regal> proveriPrijavljeneRegale()
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Regal> regalList = new List<Regal>();

            var query = from reg in dbase.Regals
                        where reg.Ostecen == 1
                        select reg;

            regalList = query.ToList<Regal>();

            return regalList;
        }

        //funkcija koja se poziva kada sef odluci da treba da se zakljuca osteceni regal 
        //*****************************************************************************************
        [WebMethod]
        public void blokirajOstecenRegal(int RegalID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = new Regal();

            var query = from reg in dbase.Regals
                        where reg.ID == RegalID
                        select reg;

            regal = query.SingleOrDefault<Regal>();

            regal.Ostecen = 2;
            regal.Zakljucan = 1;

            //dbase.Regals.Attach(regal);
            dbase.SaveChanges();
        }
        //*****************************************************************************************

        //funkcija koja vraca sve regale koji se vode kao osteceni
        //*****************************************************************************************
        [WebMethod]
        public List<Regal> proveriOsteceneRegale()
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Regal> regalList = new List<Regal>();

            var query = from reg in dbase.Regals
                        where reg.Ostecen == 2
                        select reg;

            regalList = query.ToList<Regal>();

            return regalList;
        }
        //*****************************************************************************************

        //funkija kojom se potvrdjuje da je regal koji je ostecen sada u redu
        //*****************************************************************************************
        [WebMethod]
        public void otkljucavanjeRegala(int RegalID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = new Regal();

            var query = from reg in dbase.Regals
                        where reg.ID == RegalID
                        select reg;

            regal = query.SingleOrDefault<Regal>();

            regal.Ostecen = 0;
            regal.Zakljucan = 0;

            dbase.SaveChanges();
        }
        //*****************************************************************************************
        #endregion

        #region UpravljanjeProizvodima
        //funkcija za dodavanje novih proizvoda
        //*****************************************************************************************
        //*****************************************************************************************
        //TODO treba dodati listu poluproizvoda koja se vezuje za proizvod
        [WebMethod]
        public Proizvod InsertProizvod(int RadnikID, int MasinaID, int PropisID, Int64 BarKod, out int tipGreske)
        {
            tipGreske = 0;

            TraceabilityEntities dbase = new TraceabilityEntities();
            Proizvod p = new Proizvod();
            Masina m = new Masina();

            var terminalQuery = from mas in dbase.Masinas
                                where mas.ID == MasinaID
                                select mas;
            m = terminalQuery.SingleOrDefault<Masina>();

            int stanje_prijavljenPropis = 2;

            if (m.Stanje == stanje_prijavljenPropis)
            {
                p = null;
                tipGreske = 1;
            }
            else
            {
                //provera da li postoji prizvod koji je vec otkucan sa ove masine
                var proizvodQuery = from proizvod in dbase.Proizvods
                                    where proizvod.MasinaID == MasinaID && proizvod.BarKod == BarKod
                                    select proizvod;

                p = proizvodQuery.SingleOrDefault<Proizvod>();

                if (p == null)
                {
                    p = new Proizvod();
                    p.BarKod = BarKod;
                    p.MasinaID = MasinaID;
                    p.PropisID = (int)m.PropisID;
                    p.RadnikID = RadnikID;
                    p.Vreme = DateTime.Now;

                    dbase.Proizvods.AddObject(p);
                    dbase.SaveChanges();
                }
                else
                {
                    p = null;
                    tipGreske = 2;
                }
            }

            return p;
        }

        [WebMethod]
        public void insertProizvodIngredients(Int64 proizvodID, int masinaID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<MasinaPoluproizvod> mpList = new List<MasinaPoluproizvod>();

            var query = from mpol in dbase.MasinaPoluproizvods
                        where mpol.MasinaID == masinaID
                        select mpol;

            mpList = query.ToList<MasinaPoluproizvod>();

            foreach (MasinaPoluproizvod mp in mpList)
            {
                TraceabilityEntities dbase1 = new TraceabilityEntities();

                Ingredient ing = new Ingredient();
                ing.ProizvodID = proizvodID;
                ing.PoluproizvodID = mp.PoluproizvodID;

                dbase1.Ingredients.AddObject(ing);
                dbase1.SaveChanges();
            }
        }
        //*****************************************************************************************
        //*****************************************************************************************
        [WebMethod]
        public Proizvod selectProizvod(Int64 barcode)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Proizvod p = new Proizvod();

            var query = from pro in dbase.Proizvods
                        where pro.BarKod == barcode
                        select pro;

            p = query.Single<Proizvod>();

            return p;
        }
        //funkcija za vezivanje novog proizvoda na regal
        //*****************************************************************************************
        //*****************************************************************************************
        //TODO treba dodati listu poluproizvoda koja se vezuje za proizvod
        [WebMethod]
        public void InsertRegalProizvod(Proizvod proizvod, Regal regal)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            RegalProizvod rpDelete = new RegalProizvod();

            var query = from regpro in dbase.RegalProizvods
                        where regpro.ProizvodID == proizvod.ID
                        select regpro;

            rpDelete = query.SingleOrDefault<RegalProizvod>();

            if (rpDelete != null)
            {
                dbase.RegalProizvods.Attach(rpDelete);

                dbase.RegalProizvods.DeleteObject(rpDelete);

                dbase.SaveChanges();
            }

            RegalProizvod rp = new RegalProizvod();

            rp.ProizvodID = proizvod.ID;
            rp.RegalID = regal.ID;

            dbase.RegalProizvods.AddObject(rp);
            dbase.SaveChanges();
        }
        //*****************************************************************************************
        //*****************************************************************************************

        //funkcija za proveru propisa poluproizvoda(select poluproizvod)
        //*****************************************************************************************
        //*****************************************************************************************
        [WebMethod]
        public List<Propis> proveraPoluproizvoda(int propisID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Propis> poluproizvodList = new List<Propis>();

            var query = from polupr in dbase.Poluproizvods
                        where polupr.PropisID == propisID
                        select polupr.Propi;

            poluproizvodList = query.ToList<Propis>();

            return poluproizvodList;
        }
        //*****************************************************************************************
        //*****************************************************************************************

        //funkcija za dodavanje novih poluproizvoda pored masine (insert apoluproizvod)
        //vrsi se i vezivanje trenutnih poluproiuzvoda za masinu
        //*****************************************************************************************
        //*****************************************************************************************
        [WebMethod]
        public void InsertAPoluproizvod(int radnikID, int terminalID, Int64 proizvodID, int masinaID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            APoluproizvod ap = new APoluproizvod();

            ap.ProizvodID = proizvodID;
            ap.TerminalID = terminalID;
            ap.RadnikID = radnikID;
            ap.Vreme = DateTime.Now;

            dbase.APoluproizvods.AddObject(ap);
            dbase.SaveChanges();
        }
        //*****************************************************************************************
        //*****************************************************************************************

        //funkcija za dodavanje propisa u okviru tabele MasinaPoluproizvod
        //TODO brisanje poluproizvoda kad se promeni propis
        //*****************************************************************************************
        //*****************************************************************************************
        [WebMethod]
        public void UpdateTrenutniPoluproizvodi(Proizvod otkucaniProizvod, int masinaID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            var query = from mpol in dbase.MasinaPoluproizvods
                        where mpol.MasinaID == masinaID && mpol.Proizvod.PropisID == otkucaniProizvod.PropisID
                        select mpol;

            MasinaPoluproizvod mp = query.SingleOrDefault<MasinaPoluproizvod>();
            bool mp_null = false;

            if (mp == null)
            {
                mp_null = true;
                mp = new MasinaPoluproizvod();
            }

            mp.PoluproizvodID = otkucaniProizvod.ID;
            mp.MasinaID = masinaID;

            if (mp_null)
                dbase.MasinaPoluproizvods.AddObject(mp);
            dbase.SaveChanges();
        }

        [WebMethod]
        public List<Propis> SelectTrenutniPoluproizvodi(int masinaID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            var query = from mpol in dbase.MasinaPoluproizvods
                        where mpol.MasinaID == masinaID
                        select mpol.Proizvod.Propi;

            List<Propis> propisList = new List<Propis>();
            propisList = query.ToList();

            return propisList;
        }

        [WebMethod]
        public void DeleteTrenutniPoluproizvodi(int masinaID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            var query = from mpol in dbase.MasinaPoluproizvods
                        where mpol.MasinaID == masinaID
                        select mpol;

            List<MasinaPoluproizvod> mpList = new List<MasinaPoluproizvod>();
            mpList = query.ToList<MasinaPoluproizvod>();

            foreach (MasinaPoluproizvod mp in mpList)
                dbase.MasinaPoluproizvods.DeleteObject(mp);
            dbase.SaveChanges();
        }

        //*****************************************************************************************
        //*****************************************************************************************
        #endregion

        #region KontrolaSistema

        [WebMethod]
        public void blokirajRegal(int RegalID, bool ostecen)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = new Regal();

            var query = from reg in dbase.Regals
                        where reg.ID == RegalID
                        select reg;

            regal = query.SingleOrDefault<Regal>();

            regal.Zakljucan = 1;

            if (ostecen)
                regal.Ostecen = 2;

            dbase.SaveChanges();
        }

        [WebMethod]
        public List<Regal> otkljucajListaRegala(bool ostecen, string tipRegala)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Regal> regalList = new List<Regal>();

            if (ostecen)
            {
                var query = from reg in dbase.Regals
                            where (reg.Ostecen == 1 || reg.Ostecen == 2) && reg.Zakljucan == 1 && reg.Opis == tipRegala
                            select reg;
                regalList = query.ToList<Regal>();
            }
            else
            {
                var query = from reg in dbase.Regals
                            where reg.Zakljucan == 1 && reg.Ostecen == 0 && reg.Opis == tipRegala
                            select reg;
                regalList = query.ToList<Regal>();
            }

            return regalList;
        }


        [WebMethod]
        public List<String> SelectTipRegala(int radnaJedinica)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<String> regalTipList = new List<String>();

            var query = (from reg in dbase.Regals
                         where reg.RJ == radnaJedinica
                         select reg.Opis).Distinct();

            regalTipList = query.ToList<String>();

            return regalTipList;
        }

        [WebMethod]
        public Regal FindRegalBarKod(Int64 barKodGume)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = new Regal();

            var query = from rp in dbase.RegalProizvods
                        where rp.Proizvod.BarKod == barKodGume
                        select rp.Regal;

            regal = query.SingleOrDefault<Regal>();

            return regal;
        }


        [WebMethod]
        public List<Regal> FindRegalPropis(String propis)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Regal> regalList = new List<Regal>();

            var query = from reg in dbase.Regals
                        where reg.isEmpty == false
                        && reg.Propi.Sifra == propis
                        && reg.Zakljucan == 0
                        && reg.Ostecen == 0
                        select reg;

            regalList = query.ToList<Regal>();

            return regalList;
        }



        [WebMethod]
        public List<Pozicija> FindPozicijaTipRegala(String tipRegala, int RJ)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Pozicija> pozList = new List<Pozicija>();

            if (tipRegala != "")
            {
                var query = from poz in dbase.Pozicijas
                            where poz.TipRegala == tipRegala && poz.RJ == RJ
                            select poz;

                pozList = query.ToList<Pozicija>();
            }
            else
            {
                var query = from poz in dbase.Pozicijas
                            where poz.RJ == RJ
                            select poz;

                pozList = query.ToList<Pozicija>();
            }

            return pozList;
        }

        [WebMethod]
        public int[] selectAllMachines()
        {

            TraceabilityEntities dbase = new TraceabilityEntities();
            var query = from mch in dbase.Masinas
                        select mch.ID;
            return query.ToArray();
        }

        [WebMethod]
        public List<Regal> FindRegalPozicija(String tipRegala, int RJ)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();
            List<Regal> regList = new List<Regal>();

            if (tipRegala != "")
            {
                var query = from reg in dbase.Regals
                            where reg.Opis == tipRegala
                            select reg;

                regList = query.ToList<Regal>();
            }
            else
            {
                var query = from reg in dbase.Regals
                            where reg.RJ == RJ && reg.ID == 1
                            select reg;

                regList = query.ToList<Regal>();
            }

            return regList;
        }


        [WebMethod]
        public List<Pozicija> SelectPozicijaPhase(int radnaJedinica)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Pozicija> pozicijaList = new List<Pozicija>();

            var query = from poz in dbase.Pozicijas
                        where poz.RJ == radnaJedinica || poz.RJ == radnaJedinica + 1
                        select poz;

            pozicijaList = query.ToList<Pozicija>();

            return pozicijaList;
        }

        [WebMethod]
        public Regal Zakljucaj_Pozicija_Regal(string tipRegala, int RJ, bool zakljucajRegal, string sifraRegala, string Colaza, string brojPozicija)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Regal regal = new Regal();

            if (zakljucajRegal)
            {
                Int32 regalSifra = Int32.Parse(sifraRegala);
                string nazivRegalaTmp = sifraRegala + " " + Colaza + " " + brojPozicija;

                var query = from reg in dbase.Regals
                            where reg.RJ == RJ && reg.Opis == tipRegala && reg.NazivRegala == nazivRegalaTmp
                            select reg;

                regal = query.SingleOrDefault<Regal>();
            }
            else
            {
                //situacija kada je izabrana pozicija za zakljucavanje, ne regal
                string kodPozicije = sifraRegala;

                var query = from reg in dbase.Regals
                            where reg.RJ == RJ && reg.Pozicija.KodPozicije == kodPozicije
                            select reg;

                regal = query.SingleOrDefault<Regal>();
            }

            return regal;
        }
        [WebMethod]
        public List<Regal> PomeranjeRegalaList(bool statusRegala_Izvrsavanje, string tipRegala, bool transportTip, bool export)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Regal> regalList = new List<Regal>();

            if (transportTip || (!transportTip && export))
            {
                var query = from regali in dbase.Regals
                            where regali.StatusIzvrsavanja == statusRegala_Izvrsavanje && regali.Opis == tipRegala &&
                            (regali.Pozicija.TipPozicije != "E" || regali.PozicijaID == null)
                            select regali;

                regalList = query.ToList<Regal>();
            }
            else
            {
                var query = from regali in dbase.Regals
                            where regali.StatusIzvrsavanja == statusRegala_Izvrsavanje && regali.Opis == tipRegala
                            && (regali.Pozicija.TipPozicije == "E" || regali.PozicijaID == null)
                            select regali;

                regalList = query.ToList<Regal>();
            }

            return regalList;
        }
        [WebMethod]
        public List<Pozicija> PomeranjeRegalaPozicijaList(Regal[] regalList)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();
            List<Pozicija> pozList = new List<Pozicija>();
            Pozicija poz = new Pozicija();

            for (int i = 0; i < regalList.Length; i++)
            {
                int regalID = regalList[i].ID;
                var query = from reg in dbase.Regals
                            where reg.ID == regalID
                            select reg.Pozicija;

                poz = query.Single<Pozicija>();
                pozList.Add(poz);
            }

            return pozList;
        }


        #endregion

        #region Pozicije

        [WebMethod]
        public Pozicija selectPozicija(int RJ, string kod_pozicije)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Pozicija pozicija = new Pozicija();

            var query = from poz in dbase.Pozicijas
                        where poz.RJ == RJ && poz.KodPozicije == kod_pozicije
                        select poz;

            pozicija = query.SingleOrDefault<Pozicija>();

            return pozicija;
        }

        [WebMethod]
        public List<Pozicija> pozicijaPraznaList(string tipRegala, bool tipPozicije)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Pozicija> pozList = new List<Pozicija>();

            if (!tipPozicije)
            {
                var query = from poz in dbase.Pozicijas
                            where poz.KapacitetPreostalo != 0 && poz.TipRegala == tipRegala
                            select poz;

                pozList = query.ToList<Pozicija>();
            }
            else
            {
                var query = from poz in dbase.Pozicijas
                            where poz.TipPozicije == "E" && poz.TipRegala == tipRegala
                            select poz;

                pozList = query.ToList<Pozicija>();
            }

            return pozList;
        }

        [WebMethod]
        public bool pozicijaSlobodna(int pozicijaID)
        {
            Boolean result = false;
            TraceabilityEntities db = new TraceabilityEntities();

            Pozicija pozicija = (from poz in db.Pozicijas
                                 where poz.ID == pozicijaID
                                 select poz).FirstOrDefault();

            if (pozicija != null)
                result = pozicija.KapacitetPreostalo > 0;

            return result;

        }

        [WebMethod]
        public void insertPozicija(Pozicija pozicija)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();
            dbase.Pozicijas.AddObject(pozicija);
            dbase.SaveChanges();
        }

        [WebMethod]
        public bool slobodnaPozicija(int pozicijaID)
        {
            TraceabilityEntities db = new TraceabilityEntities();

            Pozicija tmpPoz = (from poz in db.Pozicijas
                               where poz.ID == pozicijaID
                               select poz).FirstOrDefault();

            if (tmpPoz != null)
                return tmpPoz.KapacitetPreostalo > 0;
            else
                return false;

        }

        [WebMethod]
        public void deletePozicija(Pozicija pozicija)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Pozicija p = new Pozicija();

            var query = from poz in dbase.Pozicijas
                        where poz.ID == pozicija.ID
                        select poz;

            p = query.SingleOrDefault<Pozicija>();

            dbase.Pozicijas.DeleteObject(p);
            dbase.SaveChanges();
        }

        [WebMethod]
        public void updatePozicija(Pozicija pozicija)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Pozicija p = new Pozicija();

            var query = from poz in dbase.Pozicijas
                        where poz.ID == pozicija.ID
                        select poz;
            p = query.SingleOrDefault<Pozicija>();


            p.Gornja = pozicija.Gornja;
            p.Leva = pozicija.Leva;
            p.KapacitetPreostalo = pozicija.KapacitetPreostalo;
            p.KodPozicije = pozicija.KodPozicije;
            p.PozicijaInfoID = pozicija.PozicijaInfoID;
            p.TipRegala = pozicija.TipRegala;

            dbase.SaveChanges();
        }

        [WebMethod]
        public void updatePozicijaZakljucana(List<Pozicija> positionsBlockedList, List<Pozicija> positionsEnabledList)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            for (int i = 0; i < positionsBlockedList.Count; i++)
            {
                Pozicija pozicijaTmp = positionsBlockedList[i];

                var query = from position in dbase.Pozicijas
                            where position.ID == pozicijaTmp.ID
                            select position;

                Pozicija pos = query.Single<Pozicija>();

                pos.Zakljucana = 2;
                dbase.SaveChanges();
            }
            for (int i = 0; i < positionsEnabledList.Count; i++)
            {
                Pozicija pozicijaTmp = positionsEnabledList[i];

                var query = from position in dbase.Pozicijas
                            where position.ID == pozicijaTmp.ID
                            select position;

                Pozicija pos = query.Single<Pozicija>();

                pos.Zakljucana = 0;
                dbase.SaveChanges();
            }
        }


        #endregion

        #region Masina

        [WebMethod]
        public List<String> selectTipMasina(int RJ)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<String> masinaTip = new List<String>();

            var query = (from masina in dbase.Masinas
                         where masina.RJ == RJ
                         select masina.TipMasine).Distinct();

            masinaTip = query.ToList<String>();

            return masinaTip;
        }



        [WebMethod]
        public Masina selectMasina(int ID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Masina masina = new Masina();


            var query = from m in dbase.Masinas
                        where m.ID == ID
                        select m;

            masina = query.SingleOrDefault<Masina>();

            return masina;
        }
        [WebMethod]
        public void UpdateMasinaStanje(int masinaID, int stanje)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Masina masina = new Masina();

            var query = from m in dbase.Masinas
                        where m.ID == masinaID
                        select m;

            masina = query.SingleOrDefault<Masina>();

            masina.Stanje = stanje;

            dbase.SaveChanges();
        }

        [WebMethod]
        public void UpdateMasinaPropis(int masinaID, int propisID)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            Masina masina = new Masina();

            var query = from m in dbase.Masinas
                        where m.ID == masinaID
                        select m;

            masina = query.SingleOrDefault<Masina>();

            masina.PropisID = propisID;

            dbase.SaveChanges();
        }

        [WebMethod]
        public List<Masina> selectMasinaList(int RJ)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Masina> masinaList = new List<Masina>();

            var query = from masina in dbase.Masinas
                        where masina.RJ == RJ
                        select masina;
            masinaList = query.ToList<Masina>();

            return masinaList;
        }

        #endregion

        #region PozicijaInfo

        [WebMethod]
        public List<PozicijaInfo> selectPozicijaInfoList()
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<PozicijaInfo> pozicijaInfoList = new List<PozicijaInfo>();

            var query = from poz in dbase.PozicijaInfo
                        select poz;

            pozicijaInfoList = query.ToList<PozicijaInfo>();

            return pozicijaInfoList;
        }


        #endregion

        #region Greska
        [WebMethod]
        public void insertGreska(Greska error)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();
            error.Vreme = DateTime.Now;

            dbase.Greskas.AddObject(error);
            dbase.SaveChanges();
        }
        #endregion


        #region linijaTransporta

        [WebMethod]
        public void insertViljuskarLinija(Linija linija, Terminal terminal)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            var query = from vl in dbase.ViljuskarLinija
                        where vl.LinijaID == linija.ID && vl.TerminalID == terminal.ID
                        select vl;

            ViljuskarLinija viljuskar_linija = query.SingleOrDefault<ViljuskarLinija>();

            if (viljuskar_linija == null)
            {
                viljuskar_linija.LinijaID = linija.ID;
                viljuskar_linija.TerminalID = terminal.ID;

                dbase.SaveChanges();
            }
        }

        [WebMethod]
        public List<Pozicija> selectLinijaPozicijaList(int RJ, string LinijaName, out List<Pozicija> PozicijaListNotMapped)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Pozicija> linijaPozicijaList = new List<Pozicija>();

            var query = from lpoz in dbase.LinijaPozicija
                        where lpoz.Linija.NazivLinije == LinijaName && lpoz.Linija.RJ == RJ
                        select lpoz.Pozicija;

            linijaPozicijaList = query.ToList<Pozicija>();

            var query1 = from poz in dbase.Pozicijas
                         where poz.TipPozicije == "M"
                         select poz;

            //var queryRes = query1.Except<Pozicija>(query);

            PozicijaListNotMapped = query1.ToList<Pozicija>();

            return linijaPozicijaList;
        }

        [WebMethod]
        public List<Linija> selectLinija(int RJ)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            List<Linija> linijaList = new List<Linija>();

            var query = from lin in dbase.Linija
                        where lin.RJ == RJ
                        select lin;

            linijaList = query.ToList<Linija>();
            return linijaList;
        }

        [WebMethod]
        public void updateLinijaPozicija(List<Pozicija> pozicijaAddList, List<Pozicija> pozicijaDeleteList, Linija linija)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            for (int i = 0; i < pozicijaDeleteList.Count; i++)
            {
                LinijaPozicija linPoz = new LinijaPozicija();
                Pozicija pozicijaTmp = pozicijaDeleteList[i];

                var query = from lpoz in dbase.LinijaPozicija
                            where lpoz.PozicijaID == pozicijaTmp.ID && lpoz.LinijaID == linija.ID
                            select lpoz;

                linPoz = query.Single<LinijaPozicija>();

                if (linPoz != null)
                {
                    dbase.LinijaPozicija.DeleteObject(linPoz);
                    dbase.SaveChanges();
                }
            }

            for (int i = 0; i < pozicijaAddList.Count; i++)
            {
                LinijaPozicija lp = new LinijaPozicija();

                lp.LinijaID = linija.ID;
                lp.PozicijaID = pozicijaAddList[i].ID;

                dbase.LinijaPozicija.AddObject(lp);
                dbase.SaveChanges();
            }
        }

        [WebMethod]
        public List<Terminal> selectViljuskarList()
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            var query = from term in dbase.Terminals
                        where term.OpisTerminala.Contains("Viljuskar")
                        select term;

            List<Terminal> terminalList = query.ToList<Terminal>();

            return terminalList;
        }

        #endregion




        [WebMethod]
        public List<sp_AlarmMinMax_Result> SelectAlarmMinMax()
        {
            TraceabilityEntities dbase = new TraceabilityEntities();
            List<sp_AlarmMinMax_Result> alarmMinMaxList = new List<sp_AlarmMinMax_Result>();


            alarmMinMaxList = dbase.sp_AlarmMinMax().ToList<sp_AlarmMinMax_Result>();

            return alarmMinMaxList;
        }


        public Pozicija NajblizaInternaPraznaPozicija(Pozicija startnaPozicija, String tipRegala, TraceabilityEntities dbase)
        {
            Pozicija result = null;
            Pozicija najblizaPozicija = new Pozicija();

            var query = from poz in dbase.Pozicijas
                        where poz.KapacitetPreostalo != 0 && poz.TipPozicije == "I" && poz.RJ == startnaPozicija.RJ
                        && poz.TipRegala == tipRegala && poz.Zakljucana == 0
                        select poz;

            List<Pozicija> pozicijaList = new List<Pozicija>();
            pozicijaList = query.ToList<Pozicija>();

            if (pozicijaList.Count != 0)
                najblizaPozicija = pozicijaList[0];



            if (najblizaPozicija != null)
            {
                double najblizaRazdaljina = System.Math.Sqrt(System.Math.Pow((double)(startnaPozicija.Leva - najblizaPozicija.Leva), 2) +
                    System.Math.Pow((double)(startnaPozicija.Gornja - najblizaPozicija.Gornja), 2));

                int najblizaI = 0;
                for (int i = 0; i < pozicijaList.Count; i++)
                {
                    try
                    {
                        double razdaljinaTrenutna = System.Math.Sqrt(System.Math.Pow((double)(startnaPozicija.Leva - pozicijaList[i].Leva), 2) +
                        System.Math.Pow((double)(startnaPozicija.Gornja - pozicijaList[i].Gornja), 2));

                        if (razdaljinaTrenutna < najblizaRazdaljina)
                        {
                            najblizaI = i;
                            najblizaPozicija = pozicijaList[i];
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
                result = najblizaPozicija;

            }
            else
            {
                result = null;
            }


            //ako postoji zahtev na nekoj masini, vracamo i taj zahtev


            return result;
        }

        /// <summary>
        /// racunanje najblize pozicije na osnovu polazne pozicije
        /// </summary>
        /// <param name="startnaPozicija">startna pozicija od koje se nalazi najbliza pozicija</param>
        /// <param name="propis">ako je prosledjen propis, proverava da li postoji</param>
        /// <returns></returns>
        public Pozicija NajblizaPraznaPozicija(Pozicija startnaPozicija, Propis propis, int? terminalID, String tipRegala,
                                                out Zahtev povezanZahtev, TraceabilityEntities dbase)
        {
            Pozicija result = null;
            Pozicija najblizaPozicija = new Pozicija();
            List<Pozicija> pozicijaList = new List<Pozicija>();
            List<Zahtev> listaZahtevaNaMasinama = new List<Zahtev>();

            povezanZahtev = null;

            try
            {

                if (propis != null && terminalID != null)
                {

                    Regal regal = findPunRegal(startnaPozicija.RJ, propis.ID, "I", tipRegala, dbase);



                    povezanZahtev = potrebanRegalNaMasini(tipRegala.Trim(), propis.ID, Convert.ToInt32(terminalID), dbase);


                    //ukoliko nema regala u magacinima i postoji zahtev na nekoj masini
                    if (regal == null && povezanZahtev != null)
                    {

                        pozicijaList.Add(povezanZahtev.Pozicija1);
                    }
                    //u suprotnom odvezi na magacin
                    else
                    {
                        var query = from poz in dbase.Pozicijas
                                    where poz.KapacitetPreostalo != 0 && poz.TipPozicije == "I" && poz.RJ == startnaPozicija.RJ
                                    && poz.TipRegala == tipRegala && poz.Zakljucana==0
                                    select poz;

                        pozicijaList = query.ToList<Pozicija>();
                    }

                }
                else
                {
                    var query = from poz in dbase.Pozicijas
                                where poz.KapacitetPreostalo != 0 && poz.TipPozicije == "I" && poz.RJ == startnaPozicija.RJ
                                && poz.TipRegala == tipRegala && poz.Zakljucana == 0
                                select poz;

                    pozicijaList = query.ToList<Pozicija>();
                }

                if (pozicijaList.Count != 0)
                    najblizaPozicija = pozicijaList[0];



                if (najblizaPozicija != null)
                {
                    double najblizaRazdaljina = System.Math.Sqrt(System.Math.Pow((double)(startnaPozicija.Leva - najblizaPozicija.Leva), 2) +
                        System.Math.Pow((double)(startnaPozicija.Gornja - najblizaPozicija.Gornja), 2));

                    int najblizaI = 0;
                    for (int i = 0; i < pozicijaList.Count; i++)
                    {
                        try
                        {
                            double razdaljinaTrenutna = System.Math.Sqrt(System.Math.Pow((double)(startnaPozicija.Leva - pozicijaList[i].Leva), 2) +
                            System.Math.Pow((double)(startnaPozicija.Gornja - pozicijaList[i].Gornja), 2));

                            if (razdaljinaTrenutna < najblizaRazdaljina)
                            {
                                najblizaI = i;
                                najblizaPozicija = pozicijaList[i];
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    result = najblizaPozicija;

                }
                else
                {
                    result = null;
                }
            }
            catch (Exception e)
            {
                Log(e.Message);

            }

            //ako postoji zahtev na nekoj masini, vracamo i taj zahtev


            return result;
        }


        private Zahtev potrebanRegalNaMasini(String tipRegala, int propisID, int terminalID, TraceabilityEntities db)
        {
            Zahtev result = null;



            var queryZahtevi = (from zahtev in db.Zahtevs
                                from viljuskarLinija in db.ViljuskarLinija
                                from linijaPozicija in db.LinijaPozicija
                                where viljuskarLinija.TerminalID == terminalID
                                && (zahtev.Pozicija1 == linijaPozicija.Pozicija && linijaPozicija.Linija == viljuskarLinija.Linija)
                                && zahtev.PropisID == propisID && zahtev.Pozicija_OD == null

                                && zahtev.Pozicija1.TipRegala == tipRegala
                                && zahtev.StatusIzvrsavanja == 0
                                && zahtev.Pozicija1.Zakljucana == 0
                                orderby zahtev.VremeKreiranja ascending
                                select zahtev);

            result = queryZahtevi.FirstOrDefault();

            return result;
        }

        private Zahtev postojiRegalNaMasini(String tipRegala, int propisID, int terminalID, TraceabilityEntities db)
        {
            Zahtev result = null;


            var queryZahtevi = (from zahtev in db.Zahtevs
                                from viljuskarLinija in db.ViljuskarLinija
                                from linijaPozicija in db.LinijaPozicija
                                where viljuskarLinija.TerminalID == terminalID
                                && zahtev.PropisID == propisID
                                && zahtev.Pozicija_DO == null
                                && (zahtev.Pozicija == linijaPozicija.Pozicija && linijaPozicija.Linija == viljuskarLinija.Linija)
                                && zahtev.Pozicija.TipRegala == tipRegala
                                && zahtev.StatusIzvrsavanja == 0
                                && zahtev.Regal.Zakljucan == 0
                                orderby zahtev.VremeKreiranja ascending
                                select zahtev);

            return result;
        }




        //funkcija za logovanje
        private void Log(string logMessage)
        {

            using (StreamWriter w = File.AppendText(@"C:\log.txt"))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine("  :");
                w.WriteLine("  :{0}", logMessage);
                w.WriteLine("---d----------------------------");
            }


        }

        private void removeZahteviByPosDo(int IDpozicijaDo)
        {
            TraceabilityEntities dbase = new TraceabilityEntities();
            var query = from zahtev in dbase.Zahtevs
                        where zahtev.Pozicija_DO == IDpozicijaDo && zahtev.Pozicija_OD == null
                        && zahtev.StatusIzvrsavanja == 0
                        select zahtev;
            foreach (Zahtev tmpZht in query)
            {
                dbase.DeleteObject(tmpZht);
            }
        }

        #region sinhronizacija

        [WebMethod]
        public DateTime synchronizeTime()
        {
            SYSTEMTIME systime = new SYSTEMTIME();
            GetSystemTime(ref systime);

            DateTime dateTime = new DateTime(systime.wYear, systime.wMonth, systime.wDay, systime.wHour, systime.wMinute, systime.wSecond, systime.wMinute);

            return dateTime;
        }


        [WebMethod]
        public bool synchronizeTable(DataTable table, String IPadress)
        {
            Boolean result = false;
            TraceabilityEntities db = new TraceabilityEntities();


            foreach (DataRow row in table.Rows)
            {
                try
                {
                    int regalID;
                    int pocetnaPozicija;
                    int krajnjaPozicija;
                    DateTime vreme;
                    int radnikMBR;

                    Int32.TryParse(row["Regal"].ToString(), out regalID);
                    Int32.TryParse(row["Krajnja pozicija"].ToString(), out krajnjaPozicija);
                    Int32.TryParse(row["Pocetna pozicija"].ToString(), out pocetnaPozicija);
                    Int32.TryParse(row["Radnik"].ToString(), out radnikMBR);
                    vreme = (DateTime)row["Vreme"];


                    Terminal terminal = (from term in db.Terminals
                                         where term.IPAdresa == IPadress
                                         select term).FirstOrDefault();

                    var queryPozicija = from poz in db.Pozicijas
                                        where poz.ID == krajnjaPozicija
                                        select poz;
                    Pozicija pozicija2 = queryPozicija.FirstOrDefault();
                    Pozicija pozicija1 = null;

                    var queryRadnik = from rad in db.Radniks
                                      where rad.MBR == radnikMBR
                                      select rad;
                    Radnik radnik = queryRadnik.FirstOrDefault();
                    if (radnik != null)
                    {
                        Movement move = new Movement();
                        if (pocetnaPozicija != 0)
                        {
                            var queryPozicija1 = from poz in db.Pozicijas
                                                 where poz.ID == pocetnaPozicija
                                                 select poz;
                            move.Pozicija_OD = pocetnaPozicija;
                            pozicija1 = queryPozicija.FirstOrDefault();

                        }
                        else

                            move.Pozicija_DO = krajnjaPozicija;
                        move.RadnikID = radnik.ID;
                        move.RegalID = regalID;
                        move.Vreme = vreme;
                        move.Terminal = terminal;
                        db.Movements.AddObject(move);
                    }

                    var query = from reg in db.Regals
                                where reg.ID == regalID
                                select reg;

                    Regal regal = query.SingleOrDefault();
                    Pozicija tmpPoz = regal.Pozicija;
                    tmpPoz.KapacitetPreostalo--;
                    if (((DateTime)row["Vreme"]).CompareTo(regal.VremePoslednjegPomeraja) > 0)
                    {
                        regal.PozicijaID = (int)row["Krajnja pozicija"];
                        regal.VremePoslednjegPomeraja = (DateTime)row["Vreme"];
                        regal.StatusIzvrsavanja = false;
                        if (pozicija1 != null)
                            pozicija1.Zakljucana = 0;
                        pozicija2.Zakljucana = 0;
                    }
                    List<Zahtev> zahtevi;
                    if (pozicija1 != null)
                    {
                        pozicija1.KapacitetPreostalo--;
                        pozicija2.KapacitetPreostalo++;
                        zahtevi = (from zht in db.Zahtevs
                                   where ((zht.Pozicija_DO == pozicija2.ID && zht.Pozicija1.MasinaID != null)
                                         || (zht.Pozicija_OD == pozicija1.ID && zht.Pozicija.Ulazna == true && zht.RegalID == regalID)
                                         || (zht.RegalID == regalID)) && zht.VremeKreiranja < vreme
                                   select zht).ToList<Zahtev>();
                    }
                    else
                    {
                        pozicija2.KapacitetPreostalo++;
                        zahtevi = (from zht in db.Zahtevs
                                   where (zht.RegalID == regalID) && zht.VremeKreiranja < vreme
                                   select zht).ToList();
                    }

                    foreach (Zahtev zht in zahtevi)
                        db.DeleteObject(zht);

                    db.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    Log(e.Message);

                }
            }

            return result;
        }

        private class SYSTEMTIME
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
        #endregion

        /*
        #region shortestPath
        [WebMethod]
        public List<GraphTest> ListGraphTets()
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            var query = from graph in dbase.GraphTests
                        select graph;

            List<GraphTest> graphList = query.ToList<GraphTest>();

            return graphList;
        }
        [WebMethod]
        public List<String> ListGraphNodes()
        {
            TraceabilityEntities dbase = new TraceabilityEntities();

            var query = (from graph in dbase.GraphTests
                        select graph.PointStart).Distinct();

            List<String> graphNodesList = query.ToList<String>();

            return graphNodesList;
        }

        #endregion
        */

    }






}
