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
    public partial class PriceCheck : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        //System.Timers.Timer t;
        string stitle = "PoS";
        public PriceCheck()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchRecords();

        }

        public void SearchRecords()
        {
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tblBarcode WHERE barcode LIKE @barcode", cn);
            cm.Parameters.AddWithValue("@barcode", txtSearch.Text + "%");
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                lblBarcode.Text = dr["barcode"].ToString();
                lblDescription.Text = dr["description"].ToString();
                lblBrand.Text = dr["brandName"].ToString();
                lblPrice.Text = dr["price"].ToString();
            }
            dr.Close();
            cn.Close();
        }
        private void Clear()
        {
            lblBarcode.Text = "";
            lblDescription.Text = "";
            lblBrand.Text = "";
            lblPrice.Text = "";
            txtSearch.Focus();
            txtSearch.Clear();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
