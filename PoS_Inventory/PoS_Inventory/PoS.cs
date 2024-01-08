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
using System.Reflection;
using System.IO;
using System.Security.Policy;

namespace PoS_Inventory
{
    public partial class PoS : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        string stitle = "PoS";
        public string role;
        private int transactionSuffix = 1001;
        private string transactionFile = "transaction.txt";
        public PoS()
        {
            InitializeComponent();
            lblDate.Text = DateTime.Now.ToLongDateString();
            cn = new SqlConnection(dbcon.MyConnection());
            this.KeyPreview = true;
            try
            {
                if (File.Exists(transactionFile))
                {
                    string lastTransactionNumber = File.ReadAllText(transactionFile);
                    transactionSuffix = int.Parse(lastTransactionNumber);
                    Console.WriteLine(transactionSuffix);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public string GetTransNum()
        {
            string datePrefix = DateTime.Today.ToString("yyyyMMdd");
            string transactionNumber = datePrefix + transactionSuffix.ToString();
            transactionSuffix++;

            try
            {
                File.WriteAllText(transactionFile, transactionSuffix.ToString());
                Console.WriteLine(File.ReadAllText(transactionFile));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            lbltransNum.Text = transactionNumber;
            return transactionNumber;
        }
        public void createTableCart()
        {
            try
            {
                cn.Open();
                string transNum = lbltransNum.Text;
                string tableName = $"tblCarts_for_{transNum}";
                string sql = $"CREATE TABLE {tableName}(transNum varchar(50) NOT NULL, sdate date ,pcode int, price decimal(18,2), qty int, disc int, total decimal(18,2), status varchar(50) DEFAULT 'Pending')";
                cm = new SqlCommand(sql, cn);
                cm.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cn.Close();
            }
            cn.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            GetTransNum();
            createTableCart();
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
                    cn = new SqlConnection(dbcon.MyConnection());
                    cn.Open();
                    cm = new SqlCommand("SELECT * FROM tblBarcode WHERE barcode LIKE @barcode", cn);
                    cm.Parameters.AddWithValue("@barcode", txtSearch.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Quantity qty = new Quantity(this);
                        qty.ProductInfo(dr["pcode"].ToString(), double.Parse(dr["price"].ToString()), lbltransNum.Text);
                        qty.ShowDialog();
                        dr.Close();
                        cn.Close();
                    }
                    else
                    {
                        dr.Close();
                        cn.Close();
                    }
                    dr.Close();
                    cn.Close();
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

                string transNum = lbltransNum.Text;
                cn = new SqlConnection(dbcon.MyConnection());
                dataGridView1.Rows.Clear();
                int i = 0;
                double total = 0;
                cn.Open();
                cm = new SqlCommand($"SELECT c.pcode, p.brandName, p.description, c.price, c.qty, c.disc, c.total FROM tblCarts_for_{transNum} AS c INNER JOIN tblBarcode AS p ON c.pcode = p.pcode WHERE transNum LIKE '" + lbltransNum.Text + "'", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    total += Double.Parse(dr["total"].ToString());
                    dataGridView1.Rows.Add(i,dr["pcode"].ToString(), dr["description"].ToString() + dr["brandName"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString());
                }
                dr.Close();
                cn.Close();
                lblTotalPrice.Text = total.ToString("#,##0.00");
                GetTotal();

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
                    string transNum = lbltransNum.Text;
                    cn.Open();
                    cm = new SqlCommand($"DELETE FROM tblCarts_for_{transNum} WHERE pcode like '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item has been removed", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCart();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            role = frmLogin.role;
            Main_Menu main = new Main_Menu(role);
            main.Show();
            this.Dispose();
        }

        private void checkout_Click(object sender, EventArgs e)
        {
            string transNum = lbltransNum.Text;
            double vat = double.Parse(lblVAT.Text); // get VAT from lblVAT
            double vatable = double.Parse(lblVatable.Text); // get Vatable from lblVatable
            Payment payment = new Payment(double.Parse(lblTotalPrice.Text), transNum, vat, vatable);
            payment.ShowDialog();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to cancel this transaction?", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string transNum = lbltransNum.Text;
                cn.Open();
                cm = new SqlCommand($"UPDATE tblCarts_for_{transNum} SET status = 'Canceled' WHERE transNum like '" + lbltransNum.Text + "'", cn);
                cm.ExecuteNonQuery();
                MessageBox.Show("Transaction has been cancelled", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cm = new SqlCommand($"DROP TABLE  tblCarts_for_{transNum}", cn);
                cm.ExecuteNonQuery();
                cn.Close();
                dataGridView1.Rows.Clear();
                lblTotalPrice.Text = "0.00";
                lblVAT.Text = "0.00";
                lblVatable.Text = "0.00";
                txtSearch.Enabled = false;
                txtSearch.Clear();
                lbltransNum.Text = "000000000000";
            }
        }
    }
}
