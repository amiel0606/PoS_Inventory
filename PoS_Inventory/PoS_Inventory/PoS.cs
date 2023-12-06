using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PoS_Inventory
{
    public partial class PoS : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        string stitle = "PoS";
        public PoS()
        {
            InitializeComponent();
            lblDate.Text = DateTime.Now.ToLongDateString();
            cn = new SqlConnection(dbcon.MyConnection());
            this.KeyPreview = true;
        }

        public void GetTransNum()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                string transnum;
                int count;
                cn.Open();
                cm = new SqlCommand("SELECT TOP 1 transNo FROM tblCarts WHERE transNo LIKE '" + sdate + "%' ORDER BY transNo DESC", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    transnum = dr[0].ToString();
                    count = int.Parse(transnum.Substring(8, 4));
                    lbltransNum.Text = sdate + (count + 1);
                }
                else
                {
                    transnum = sdate + "1001";
                    lbltransNum.Text = transnum;
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                dr.Close();
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetTransNum();
            txtSearch.Enabled = true;
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text == String.Empty) { return; }
                else
                {
                    cn.Open();
                    cm = new SqlCommand("SELECT * FROM tblBarcode WHERE barcode like '" + txtSearch.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Quantity qty = new Quantity(this);
                        qty.ProductInfo(dr["pcode"].ToString(), double.Parse(dr["price"].ToString()), lbltransNum.Text);
                        dr.Close();
                        cn.Close();
                        qty.ShowDialog();
                    }
                    else
                    {
                        dr.Close();
                        cn.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                dr.Close();
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadCart()
        {
            try
            {
                dataGridView1.Rows.Clear();
                int i = 0;
                double total = 0;
                cn.Open();
                cm = new SqlCommand("SELECT c.id, c.pcode, p.brandName , p.description, c.price, c.qty, c.disc, c.total FROM tblCarts AS c INNER JOIN tblBarcode AS p ON c.pcode = p.pcode WHERE transNo LIKE '" + lbltransNum.Text + "'", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    total += Double.Parse(dr["total"].ToString());
                    dataGridView1.Rows.Add(i, dr["id"].ToString(), dr["brandName"].ToString() + " " + dr["description"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString());
                }
                dr.Close();
                cn.Close();
                lblTotalPrice.Text = total.ToString("#,##0.00");
                GetTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cn.Close();
            }
        }
        public void GetTotal()
        {
            double subtotal = Double.Parse(lblTotalPrice.Text);
            double vat = subtotal * .12;
            double discount = 0;
            double vatable = subtotal - vat;
            lblVAT.Text = vat.ToString("#,##0.00");
            lblVatable.Text = vatable.ToString("#,##0.00");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            PriceCheck priceCheck = new PriceCheck();
            priceCheck.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "delete")
            {
                if (MessageBox.Show("Remove this item from the cart?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tblCarts WHERE id like '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item has been removed", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCart();
                }
            }
        }
    }
}
