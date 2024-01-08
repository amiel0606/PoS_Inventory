using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoS_Inventory
{
    public partial class Payment : Form
    {
        public Payment(double total, string transNumber, double vat, double vatable)
        {
            InitializeComponent();
            lblTotalPayment.Text = total.ToString();
            lblTrans.Text = transNumber;
            lblVAT.Text = vat.ToString();
            lblVatable.Text = vatable.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tn = lblTrans.Text;
            double total = double.Parse(lblTotalPayment.Text);
            double cash = double.Parse(txtPayment.Text);
            if (cash >= total)
            {
                double vat = double.Parse(lblVAT.Text);
                double vatable = double.Parse(lblVatable.Text);
                double change = cash - total;
                Receipt receipt = new Receipt(tn, cash, change, vat, vatable, total);
                receipt.Show();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Insufficient Payment");
            }
        }

        private void txtPayment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string tn = lblTrans.Text;
                double total = double.Parse(lblTotalPayment.Text);
                double cash = double.Parse(txtPayment.Text);
                if (cash >= total)
                {
                    double vat = double.Parse(lblVAT.Text);
                    double vatable = double.Parse(lblVatable.Text);
                    double change = cash - total;
                    Receipt receipt = new Receipt(tn, cash, change, vat, vatable, total);
                    receipt.Show();
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Insufficient Payment");
                }
            }
        }
    }
}
