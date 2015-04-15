using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;


namespace BussinesLogicLayer
{
    public class ControlClass
    {
        private static readonly ControlClass instance = new ControlClass();

        public ControlClass()
        {
        }

        public static ControlClass Instance
        {
            get { return instance; }
        }

        public localhost.Pozicija[] pozicijaList;
        public localhost.Regal[] regalTipList;
        public List<localhost.Regal> regalList;

        public localhost.Regal[] listaRegalaOtkljucaj;

        public localhost.Regal[] regalPomeranjeList;
        public localhost.Pozicija[] regalPomeranjePozicijaList;
        public localhost.Pozicija[] regalPomeranjePozicijaPraznaList;

        public String[] GetRegalTip(int radnaJedinica)
        {
            localhost.Service1 service1 = new localhost.Service1();

            String[] TipRegalaList = service1.SelectTipRegala(radnaJedinica);

            return TipRegalaList;
        }

        public localhost.Regal[] findRegal(string PropisRegala)
        {
            localhost.Service1 service1 = new localhost.Service1();

            return service1.FindRegalPropis(PropisRegala);
        }


        public localhost.Regal[] findRegal(Int64 BarKodGume)
        {
            localhost.Service1 service1 = new localhost.Service1();

            localhost.Regal[] regList = new localhost.Regal[1];
            localhost.Regal rTmp = service1.FindRegalBarKod(BarKodGume);

            regList[0] = rTmp;

            return regList;
        }

        public void findPozicijaTipRegala(string tipRegala, int RJ)
        {
            localhost.Service1 service1 = new localhost.Service1();

            pozicijaList = service1.FindPozicijaTipRegala(tipRegala, RJ);
        }

        public localhost.Pozicija[] findPozicijaList(int faza)
        {
            localhost.Service1 service1 = new localhost.Service1();

            return service1.SelectPozicijaPhase(faza);
        }

        public localhost.Regal[] findRegalPozicija(string tipRegala, int RJ)
        {
            localhost.Service1 service1 = new localhost.Service1();

            return service1.FindRegalPozicija(tipRegala, RJ);
        }


        public localhost.Propis findPropis(string propis, int faza)
        {
            localhost.Service1 service1 = new localhost.Service1();

            localhost.Propis p = service1.selectPropis(propis, faza);

            if (p == null)
                throw new BussinesLogicLayer.PropisNotFoundException("Ovaj propis nije pronadjen za zadatu radnu jedinicu");

            return p;
        }

        public localhost.Regal ZakljucajRegal(string tipRegala, int RJ, bool regalZakljucaj, string sifraRegala, string colaza, string brojPozicija)
        {
            localhost.Service1 service1 = new localhost.Service1();

            localhost.Regal regal = service1.Zakljucaj_Pozicija_Regal(tipRegala, RJ, regalZakljucaj, sifraRegala, colaza, brojPozicija);

            if (regal == null && regalZakljucaj)
                throw new BussinesLogicLayer.RackException("Ovaj regal nije pronadjen");
            else if (regal == null)
                throw new BussinesLogicLayer.RackException("Nije pronadjen ni jedan regal na zadatoj poziciji");

            return regal;
        }

        public void ZakljucajRegal(int RegalID, bool ostecen)
        {
            localhost.Service1 service1 = new localhost.Service1();

            service1.blokirajRegal(RegalID, ostecen);
        }

        public void OtkljucajListaRegala(bool ostecen, string tipRegala)
        {
            localhost.Service1 service1 = new localhost.Service1();

            listaRegalaOtkljucaj = service1.otkljucajListaRegala(ostecen, tipRegala);
        }

        public void OtkljucajRegal(int regalID)
        {
            localhost.Service1 service1 = new localhost.Service1();

            service1.otkljucavanjeRegala(regalID);
        }

        public void Pomeranje_StatusRegala_List(bool statusIzvrasavanje, string tipRegala, bool transportTip, bool import_export)
        {
            localhost.Service1 service1 = new localhost.Service1();

            regalPomeranjeList = service1.PomeranjeRegalaList(statusIzvrasavanje, tipRegala, transportTip, import_export);
            regalPomeranjePozicijaList = service1.PomeranjeRegalaPozicijaList(regalPomeranjeList);


            if (!transportTip && import_export == true)
                regalPomeranjePozicijaPraznaList = service1.pozicijaPraznaList(tipRegala, import_export);
            else
                regalPomeranjePozicijaPraznaList = service1.pozicijaPraznaList(tipRegala, false);

        }

        public void PomeranjeRegalaClick
            (localhost.Regal rackToMove, localhost.Pozicija posFrom, localhost.Pozicija posTo, bool transportTip_internal, bool export)
        {
            localhost.Service1 service1 = new localhost.Service1();

            if (!service1.updateRegalPozicija(rackToMove.ID, posFrom.ID, posTo.ID))
                throw new BussinesLogicLayer.PozicijaException("Pozicija na koju poskusavate pomeranje regala " + posTo.KodPozicije +
                    " je u medjuvremenu zauzeta");
        }

        public localhost.Radnik selectRadnikLogin(string username, string password)
        {
            localhost.Service1 service1 = new localhost.Service1();
            return service1.selectRadnikLogin(username, password);
        }

        public localhost.Regal[] selectRegalPropisList(string tipRegala, out localhost.Propis[] propisList)
        {
            localhost.Service1 service = new localhost.Service1();
            localhost.Regal[] regalList = service.selectRegalPropisList(tipRegala, out propisList);
            return regalList;
        }

        public void selectGraphList()
        {
            //localhost.Service1 service = new localhost.Service1();
            //localhost.GraphTest[] graphList = service.ListGraphTets();
            //String[] graphNodesList = service.ListGraphNodes();

            //shortestPath(graphList, graphNodesList);
        }

        public localhost.Pozicija[] selectLinijaPozicijaList(int RJ, string linijaNaziv, out localhost.Pozicija[] pozicijaAllList)
        {
            localhost.Service1 service = new localhost.Service1();
            return service.selectLinijaPozicijaList(RJ, linijaNaziv, out pozicijaAllList);
        }

        public localhost.Terminal[] selectViljuskar()
        {
            localhost.Service1 service = new localhost.Service1();
            return service.selectViljuskarList();
        }

        /*private void shortestPath(localhost.GraphTest[] graphList, String[] graphNodesList)
        {
            int[] dist = new int[graphNodesList.Length];
            string[] prev = new string[graphNodesList.Length];
            List<String> unvisitedNodes = new List<String>();

            #region PopunjavanjeDistance
            for (int i = 0; i < graphNodesList.Length; i++)
            {
                //definise prethodnike , za pocetak su svi null
                //definise distancu, distanca do neobidjenih cvorova je infinity
                if (i > 0)
                {
                    dist[i] = 1000000;
                    prev[i] = null;
                }
                else
                {
                    prev[0] = null;
                    dist[0] = 0;
                }
                unvisitedNodes.Add(graphNodesList[i]);
            }
            #endregion

            string visitedNode = graphNodesList[0];
            unvisitedNodes.RemoveAt(0);

            while (unvisitedNodes.Count != 0)
            {
                for (int j = 0; j < graphList.Length; j++)
                {
                    if (visitedNode == graphList[j].PointStart)
                    {
                        int startPointNode = -1;
                        int endPointNode = -1;
                        int brojac1 = 0;
                        int brojac2 = 0;
                        while (startPointNode == -1 || endPointNode == -1)
                        {
                            if (graphList[j].PointEnd == graphNodesList[brojac1])
                                endPointNode = brojac1;
                            else
                                brojac1++;

                            if (graphList[j].PointStart == graphNodesList[brojac2])
                                startPointNode = brojac2;
                            else
                                brojac2++;
                        }

                        int temporaryDistance = dist[startPointNode] + (int)graphList[j].Distance;
                        if (temporaryDistance < dist[endPointNode])
                        {
                            dist[endPointNode] = temporaryDistance;
                            prev[endPointNode] = graphNodesList[startPointNode];
                        }
                    }
                  
                }

                visitedNode = unvisitedNodes[0];
                unvisitedNodes.RemoveAt(0);

            }

        }*/

        public localhost.sp_AlarmMinMax_Result[] SelectStockMinMaxList()
        {
            localhost.Service1 service = new localhost.Service1();

            return service.SelectAlarmMinMax();
        }

        public localhost.Linija[] selectLinijaList(int RJ)
        {
            
            localhost.Service1 service = new localhost.Service1();
            return service.selectLinija(RJ);
        }

        public void updateLinijaPozicija
            (localhost.Pozicija[] pozicijaAddList, localhost.Pozicija[] pozicijaDeleteList, localhost.Linija linija)
        {
            localhost.Service1 service = new localhost.Service1();
            service.updateLinijaPozicija(pozicijaAddList, pozicijaDeleteList, linija);
        }

        public void updatePozicijaZakljucana(List<localhost.Pozicija> blockedLit, List<localhost.Pozicija> enabledList)
        {
            localhost.Service1 service = new localhost.Service1();
            service.updatePozicijaZakljucana(blockedLit.ToArray(), enabledList.ToArray());
        }

        public void insertViljuskarLinija(localhost.Terminal viljuskar, localhost.Linija linija)
        {
            localhost.Service1 service = new localhost.Service1();
            service.insertViljuskarLinija(linija, viljuskar);
        }
    }
}
