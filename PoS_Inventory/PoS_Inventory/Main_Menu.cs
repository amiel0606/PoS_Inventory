using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace PoS_Inventory
{
    public partial class Main_Menu : Form
    {
        Products inv = new Products();
        PoS point_of_sales = new PoS();
        addEmployee add = new addEmployee();
        public static string role;

        public Main_Menu(string role)
        {
            InitializeComponent();
            if (role == "Admin")
            {
                lblRole.Text = "Administrator";
                btnInv.Visible = true;
                btnPos.Visible = true;
                btnAdd.Visible = true;
            }
            else if (role == "Employee")
            {
                lblRole.Text = "Employee";
                btnInv.Visible = false;
                btnPos.Visible = true;
                btnAdd.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            point_of_sales.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            inv.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            add.Show();
        }
    }
}
