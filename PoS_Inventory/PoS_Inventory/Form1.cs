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
    public partial class frmLogin : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        public static string role;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cn = new SqlConnection(dbcon.MyConnection());
                cn.Open();
                cm = new SqlCommand("SELECT * FROM tblusers WHERE role = @role AND password = @password", cn);
                cm.Parameters.AddWithValue("@password", txtpass.Text);
                cm.Parameters.AddWithValue("@role", cmbRole.Text);
                SqlDataReader dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    if (dr["role"].ToString() == "Admin")
                    {
                        role = dr["role"].ToString();
                        MessageBox.Show("Welcome " + role, "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        Main_Menu main = new Main_Menu(role);
                        main.Show();
                    }
                    else if (dr["role"].ToString() == "Employee")
                    {
                        role = dr["role"].ToString();
                        MessageBox.Show("Welcome " + role, "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        Main_Menu main = new Main_Menu(role);
                        main.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (cmbRole.Text.Length == 0)
                {
                    MessageBox.Show("Please select a role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtpass.Text.Length == 0)
                {
                    MessageBox.Show("Please enter a password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
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
