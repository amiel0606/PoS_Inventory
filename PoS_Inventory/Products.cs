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
    public partial class Products : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        public Products()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            LoadRecords();
            txtSearch.Clear();
            txtSearch.Focus();
        }
        public void LoadRecords()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tblBarcode ORDER BY quantity", cn);
            SqlDataReader dr = cm.ExecuteReader(); 
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["barcode"].ToString(), dr["description"].ToString(), dr["brandName"].ToString(), dr["quantity"].ToString(), dr["price"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void SearchRecords()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tblBarcode WHERE barcode like '" + txtSearch.Text + "%' OR description like '" + txtSearch.Text + "%' OR brandName like '" + txtSearch.Text + "%' ORDER BY quantity", cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["barcode"].ToString(), dr["description"].ToString(), dr["brandName"].ToString(), dr["quantity"].ToString(), dr["price"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddProducts addProducts = new AddProducts(this );
            addProducts.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "edit")
            {
                AddProducts prods = new AddProducts(this);
                prods.lblD.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                prods.txtBarcode.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                prods.txtBrandName.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                prods.txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                prods.txtQuantity.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                prods.txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                prods.ShowDialog();
            } else if (colName == "delete")
            {
                if (MessageBox.Show("Are you sure to delete this item?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tblBarcode WHERE barcode like '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item has been successfully deleted", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecords();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchRecords();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Main_Menu main = new Main_Menu();
            main.Show();
            this.Hide();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Main_Menu main = new Main_Menu();
            main.Show();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main_Menu main = new Main_Menu();
            main.Show();
            this.Dispose();
        }
    }
}
