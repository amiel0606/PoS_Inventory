using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoS_Inventory
{
    public partial class Quantity : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        private double price;
        private String transno, pcode;
        string title = "Point of Sale";
        PoS frmpos;
        public Quantity(PoS pos)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            frmpos = pos;
        }

        public void ProductInfo(String pcode, double price, String transno)
        {
            this.pcode = pcode;
            this.price = price;
            this.transno = transno;
        }
        private void Quantity_Load(object sender, EventArgs e)
        {
            txtQuantity.Focus();
            txtQuantity.Text = "0";
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar==13)&&(txtQuantity.Text != String.Empty))
            {
                cn.Open();
                cm = new SqlCommand("INSERT INTO tblCarts (transNo, pcode, price, qty, sdate) VALUES (@transNo, @pcode, @price, @qty, @sdate)", cn);
                cm.Parameters.AddWithValue("@transNo", transno);
                cm.Parameters.AddWithValue("@pcode", pcode);
                cm.Parameters.AddWithValue("@price", price);
                cm.Parameters.AddWithValue("@qty", int.Parse(txtQuantity.Text));
                cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                cm.ExecuteNonQuery();
                cn.Close();
                frmpos.txtSearch.Focus();
                frmpos.txtSearch.Clear();
                frmpos.LoadCart();
                this.Dispose();
            }
        }
    }
}
