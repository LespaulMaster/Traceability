using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BussinesLogicLayer;
using TestApp.rs.nbs.webservices;

namespace TestApp
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
           
            
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            
        }

       

        private void button4_Click(object sender, EventArgs e)
        {
            rs.nbs.webservices.AuthenticationHeader header = new AuthenticationHeader();
            header.LicenceID = new Guid("f7442934-0272-4875-bb52-2eb4057db5ee");
            header.UserName = "adacta";
            header.Password = "adacta123";

            ExchangeRateService service = new ExchangeRateService();
            service.AuthenticationHeaderValue = header;
            try
            {
                DataSet ds = service.GetExchangeRateRsdEurByPeriod("20141231", DateTime.Now.ToString("yyyyMMdd"), null);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

            
        }
    }
}
