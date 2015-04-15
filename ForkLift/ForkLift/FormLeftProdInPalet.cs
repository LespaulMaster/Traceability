using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BussinesLogicLayer;

namespace ForkLift
{
    public partial class FormLeftProdInPalet : Form
    {
        int NoProducts;
        public FormLeftProdInPalet()
        {
            InitializeComponent();
            this.Width = Screen.FromControl(this).Bounds.Width;      
            NoProducts = 0;
        }

        private void FormLeftProdInPalet_Load(object sender, EventArgs e)
        {
            
            this.textBox1.Text = NoProducts.ToString();
        }

        private void AddProduct(object sender, EventArgs e)
        {
            NoProducts++;
            this.textBox1.Text = NoProducts.ToString();
        }

        private void RemoveProduct(object sender, EventArgs e)
        {
            NoProducts--;
            this.textBox1.Text = NoProducts.ToString();
        }

        private void xButtonMetro3_Click(object sender, EventArgs e)
        {
            int noProds;
            Int32.TryParse(this.textBox1.Text, out noProds);

            if (TerminalForkLift.Instance.updateRegalBrojProizvoda(noProds))
                Close();
        }

        
    }
}
