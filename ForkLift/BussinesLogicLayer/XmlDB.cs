using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using System.IO;

namespace BussinesLogicLayer
{
    public class XmlDB
    {
        private static readonly XmlDB instance = new XmlDB();
        DataSet ds;
        DataView dv;
        DataTable dt;

        String dbPath = AppDomain.CurrentDomain.BaseDirectory + "\\XML\\OfflineData.xml";
        public XmlDB()
        {
            
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("Radnik", typeof(int)));
            dt.Columns.Add(new DataColumn("Regal", typeof(int)));
            dt.Columns.Add(new DataColumn("Pocetna pozicija", typeof(int)));
            dt.Columns.Add(new DataColumn("Krajnja pozicija", typeof(int)));
            dt.Columns.Add(new DataColumn("Vreme", typeof(DateTime)));

            dv = new DataView(dt);
            ds = new DataSet();
            ds.Tables.Add(dt);

            if (!File.Exists(dbPath))
            {
                
                save();
            }

            
        }

        public static XmlDB Instance{
            get { return instance; }
        }

        public int Count()
        {
            return ds.Tables[0].Rows.Count;
        }

        public void save()
        {

            ds.WriteXml(dbPath, XmlWriteMode.WriteSchema);

        }

        //Store new data in a dataview
        public void Insert(int radnikMBR, int ID, int posOD, int posDO, DateTime vreme)
        {

            DataRow dr = ds.Tables[0].NewRow();

            dr[0] = radnikMBR;
            dr[1] = ID;

            dr[2] = posOD;
            dr[3] = posDO;
            dr[4] = vreme;

            ds.Tables[0].Rows.Add(dr);
            


            save();

        }

        //Search any data in dataview with specific primary key, and update it's data
        public void Update(int ID, int posDO)
        {

            DataRow dr = Select(ID);

            dr[1] = posDO;

            save();

        }

        //Delete any data with specific key
        public void Delete(int ID)
        {

            dv.RowFilter = "Regal='" + ID + "'";

            dv.Sort = "Vreme desc";

            for (int i = 0; i < dv.Count; i++)
                dv.Delete(i);

            dv.RowFilter = "";

            save();

        }

        //Search any data in dataview with specific primary key
        public DataRow Select(int ID)
        {

            dv.RowFilter = "Regal='" + ID + "'";

            dv.Sort = "Vreme desc";

            DataRow dr = null;

            if (dv.Count > 0)
            {

                dr = dv[0].Row;

            }

            dv.RowFilter = "";

            return dr;

        }

        //Load all data to dataset
        public DataView SelectAll()
        {

            ds.Clear();

            ds.ReadXml(dbPath, XmlReadMode.ReadSchema);

            dv = ds.Tables[0].DefaultView;

            return dv;

        }

        public DataTable GetDataTable()
        {
            return this.ds.Tables[0];
        }

        public void DeleteAll()
        {
            ds.Clear();

            save();
        }


    }
}