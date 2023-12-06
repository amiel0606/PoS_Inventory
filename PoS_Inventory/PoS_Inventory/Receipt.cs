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
    public partial class Receipt : Form
    {
        SqlConnection con = new SqlConnection();
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        string transno;


        public Receipt(string transNum)
        {
            InitializeComponent();
            con = new SqlConnection(dbcon.MyConnection());
            transno = transNum;
            DisplayReceipt(transNum);
            cn.Open();
            string update = $"UPDATE tblCarts_for_{transNum} SET status = 'Complete' WHERE status = 'Pending'";
            cm = new SqlCommand(update, cn);
            cm.ExecuteNonQuery();
            cn.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thank you for your Purchase");
            Application.Exit();
        }
        public void DisplayReceipt(string num)
        {
            listBox1.Items.Clear();
            cn = new SqlConnection(dbcon.MyConnection());
            cn.Open();
            string query = $"SELECT c.pcode, p.brandName, p.description, c.price, c.qty, c.disc, c.total FROM tblCarts_for_{num} AS c INNER JOIN tblBarcode AS p ON c.pcode = p.pcode WHERE transNum LIKE {num}";
            cm = new SqlCommand(query, cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                string itemName = dr["description"].ToString()+" " + dr["brandName"].ToString();
                string qty = dr["qty"].ToString();
                string price = dr["price"].ToString();
                string total = dr["total"].ToString();
                string add = string.Format("Item:{0, -20} Quantity:{1, -30} Price:{2, -30} Total:{3, -30}", itemName, qty, price, total);
                listBox1.Items.Add(add);
            }
            dr.Close();
            cn.Close();
        }
    }
}
