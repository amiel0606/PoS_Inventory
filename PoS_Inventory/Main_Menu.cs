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

        public Main_Menu()
        {
            InitializeComponent();
            Text = "";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            inv.Show();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            inv.Show();
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            inv.Show();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            point_of_sales.Show();
        }

        private void pos_Click(object sender, EventArgs e)
        {
            this.Hide();
            point_of_sales.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
