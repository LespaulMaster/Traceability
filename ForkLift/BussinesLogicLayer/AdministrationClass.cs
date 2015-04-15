using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;

namespace BussinesLogicLayer
{
    public class AdministrationClass
    {
        public String[] MasinaTipList;
        public List<localhost.Propis> PoluproizvodList = new List<localhost.Propis>();
        public localhost.Propis[] pList;

        public localhost.Propis propisFound;

        public int availableRegalID;

        private static readonly AdministrationClass instance = new AdministrationClass();

        public AdministrationClass()
        {
        }

        public static AdministrationClass Instance
        {
            get { return instance; }
        }

        public localhost.Radnik selectRadnik(int QV)
        {
            localhost.Service1 service1 = new localhost.Service1();

            localhost.Radnik radnik = service1.SelectRadnik(QV);

            //if (radnik == null)
            //    throw new BussinesLogicLayer.RadnikException("Radnik sa zadatim maticnim brojem nije pronadjen");

            return radnik;
        }

        public void updateRadnik(int ID, int QV, string Ime, string Prezime, int OJ, string NazivRM, int SifraRM, bool Pol)
        {
            localhost.Service1 service1 = new localhost.Service1();

            service1.UpdateRadnik(ID, QV, Ime, Prezime, OJ, NazivRM, SifraRM, Pol);
        }

        public void addRadnik(int QV, string Ime, string Prezime, int OJ, string NazivRM, int SifraRM, bool Pol)
        {
            localhost.Service1 service1 = new localhost.Service1();

            service1.addNewRadnik(QV, Ime, Prezime, OJ, NazivRM, SifraRM, Pol);
        }

        public void deleteRadnik(int QV)
        {
            localhost.Service1 service1 = new localhost.Service1();

            service1.deleteRadnik(QV);
        }

        public void ChangeRadnaJedinica(int RJ)
        {
            localhost.Service1 service1 = new localhost.Service1();

            MasinaTipList = service1.selectTipMasina(RJ);

            localhost.Propis[] prList;
            PoluproizvodList.Clear();

            if (RJ == RadnaJedinica.VALJARA)
            {
                prList = service1.selectPropisList(RadnaJedinica.VALJARA);
                createPoluproizvodList(prList);
            }
            else if (RJ == RadnaJedinica.PRERADA)
            {
                prList = service1.selectPropisList(RadnaJedinica.PRERADA);
                createPoluproizvodList(prList);
                prList = service1.selectPropisList(RadnaJedinica.VALJARA);
                createPoluproizvodList(prList);
            }
            else if (RJ == RadnaJedinica.KONFEKCIJA)
            {
                prList = service1.selectPropisList(RadnaJedinica.PRERADA);
                createPoluproizvodList(prList);
                prList = service1.selectPropisList(RadnaJedinica.KONFEKCIJA);
                createPoluproizvodList(prList);
            }
            else if (RJ == RadnaJedinica.VULKANIZACIJA)
            {
                prList = service1.selectPropisList(RadnaJedinica.KONFEKCIJA);
                createPoluproizvodList(prList);
            }
        }

        private void createPoluproizvodList(localhost.Propis[] propisList)
        {
            foreach (localhost.Propis p in propisList)
            {
                PoluproizvodList.Add(p);
            }
        }

        public String addPropis(String sifraPropisa, String opis, String tipMasine, int RJ, List<localhost.Propis> poluproizvodi)
        {
            localhost.Service1 service1 = new localhost.Service1();

            localhost.Propis propisTmp = service1.selectPropis(sifraPropisa, RJ);
            string successString;
            int VerzijaPropisa = 0;

            if (propisTmp == null)
                successString = "Uspešno ste dodali novi propis";
            else
            {
                successString = "Propis " + sifraPropisa + " je uspešno ažuriran";
                VerzijaPropisa = (int)propisTmp.Verzija + 1;
            }

            service1.insertPropis(sifraPropisa, opis, tipMasine, RJ, poluproizvodi.ToArray(), VerzijaPropisa);

            return successString;
        }

        public void findPropis(String sifraPropisa, Int32 RJ)
        {
            localhost.Service1 service1 = new localhost.Service1();

            propisFound = service1.selectPropis(sifraPropisa, RJ);
            if (propisFound != null)
            {
                selectPropisPoluproizvodList(propisFound.ID);
            }
            else
                throw new PropisNotFoundException("Trazeni Propis nije pronadjen u bazi podataka");
        }

        public localhost.Propis[] findPropisList(String sifraPropisa, Int32 RJ)
        {
            localhost.Service1 service1 = new localhost.Service1();

            localhost.Propis[] propisList = service1.selectPropisVerzijaList(sifraPropisa, RJ);

            if (propisList.Length != 0)
                return propisList;
            else
                throw new PropisNotFoundException("Trazeni Propis nije pronadjen u bazi podataka");
        }

        public void selectPropisPoluproizvodList(int propisID)
        {
            localhost.Service1 service1 = new localhost.Service1();
            pList = service1.selectPropisPoluproizvodList(propisID);
        }

        public void deletePropis(int PropisID)
        {
            localhost.Service1 service1 = new localhost.Service1();

            service1.deletePropis(PropisID);
        }

        public void insertRegal(int regalID, int colaza, int kapacitet, int radnaJedinica, string opis)
        {
            localhost.Service1 service1 = new localhost.Service1();

            localhost.Regal regal = service1.SelectRegal(regalID);

            if (regal != null)
            {
                availableRegalID = service1.findAvailableRegalID(regalID);
                throw new BussinesLogicLayer.RackException("ID regala koji ste zadali vec postoji, probajte " + availableRegalID);
            }
            else
            {
                service1.insertRegal(regalID, colaza, kapacitet, radnaJedinica, opis);
            }
        }

        public localhost.Regal selectRegal(int regalID)
        {
            localhost.Service1 service1 = new localhost.Service1();

            localhost.Regal regal = service1.SelectRegal(regalID);

            if (regal == null)
            {
                throw new BussinesLogicLayer.RackException("Regal sa zadatim identifikacionim brojem ne postoji");
            }

            return regal;
        }

        public void updateRegal(localhost.Regal regal)
        {
            localhost.Service1 service1 = new localhost.Service1();

            service1.updateRegal(regal);
        }

        public void deleteRegal(localhost.Regal regal)
        {
            localhost.Service1 service1 = new localhost.Service1();
            service1.deleteRegal(regal);
        }

        public localhost.Masina[] selectMasinaList(int RJ)
        {
            localhost.Service1 service1 = new localhost.Service1();
            localhost.Masina[] masinaList = service1.selectMasinaList(RJ);

            return masinaList;
        }

        public string[] selectTipRegala(int rj)
        {
            localhost.Service1 service1 = new localhost.Service1();
            String[] tipRegalaList = service1.SelectTipRegala(rj);
            return tipRegalaList;
        }

        public void insertPozicija(localhost.Pozicija pozicija)
        {
            localhost.Service1 service1 = new localhost.Service1();
            localhost.Pozicija poz = service1.selectPozicija(pozicija.RJ, pozicija.KodPozicije);

            if (poz != null)
                throw new BussinesLogicLayer.PozicijaException("pozicija sa zadatim kodom pozicije vec postoji u ovoj radnoj jedinici");

            service1.insertPozicija(pozicija);
        }

        public localhost.PozicijaInfo[] selectPozicijaInfoList()
        {
            localhost.Service1 service1 = new localhost.Service1();

            return service1.selectPozicijaInfoList();
        }

        public localhost.Pozicija selectPozicija(int RJ, string KodPozicije)
        {
            localhost.Service1 service1 = new localhost.Service1();

            localhost.Pozicija pozicija = service1.selectPozicija(RJ, KodPozicije);

            if (pozicija == null)
                throw new BussinesLogicLayer.PozicijaException("Zadata pozicija nije pronadjena u bazi podataka");

            return pozicija;
        }

        public void deletePozicija(localhost.Pozicija pozicija)
        {
            localhost.Service1 service1 = new localhost.Service1();
            service1.deletePozicija(pozicija);
        }

        public void updatePozicija(localhost.Pozicija pozicija)
        {
            localhost.Service1 service1 = new localhost.Service1();
            service1.updatePozicija(pozicija);
        }



        public localhost.Masina selectMasina(int masinaID)
        {
            localhost.Service1 service1 = new localhost.Service1();
            return service1.selectMasina(masinaID);
        }

    }
}
