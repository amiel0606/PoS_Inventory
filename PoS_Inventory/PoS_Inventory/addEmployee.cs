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
    public partial class addEmployee : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        public addEmployee()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtpass.Text.Length != 0)
                {
                    cn = new SqlConnection(dbcon.MyConnection());
                    cn.Open();
                    cm = new SqlCommand("INSERT INTO tblusers(password, role)  VALUES (@pasword, @role)", cn);
                    cm.Parameters.AddWithValue("@pasword", txtpass.Text);
                    cm.Parameters.AddWithValue("@role", cmbRole.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("User Added Successfully");
                    txtpass.Clear();
                    cmbRole.Text = "";
                    this.Dispose();
                }
                else if (txtpass.Text.Length == 0)
                {
                    MessageBox.Show("Please enter a password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (cmbRole.Text.Length == 0)
                {
                    MessageBox.Show("Please select a role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtpass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (txtpass.Text.Length != 0)
                    {
                        cn = new SqlConnection(dbcon.MyConnection());
                        cn.Open();
                        cm = new SqlCommand("INSERT INTO tblusers(password, role)  VALUES (@pasword, @role)", cn);
                        cm.Parameters.AddWithValue("@pasword", txtpass.Text);
                        cm.Parameters.AddWithValue("@role", cmbRole.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("User Added Successfully");
                        txtpass.Clear();
                        cmbRole.Text = "";
                        this.Dispose();
                    }
                    else if (txtpass.Text.Length == 0)
                    {
                        MessageBox.Show("Please enter a password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (cmbRole.Text.Length == 0)
                    {
                        MessageBox.Show("Please select a role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
 }