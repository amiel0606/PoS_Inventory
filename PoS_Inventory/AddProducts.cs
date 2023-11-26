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
    public partial class AddProducts : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        Products products;
        public AddProducts(Products prods)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            products = prods;
        }

        private void Clear()
        {
            txtBarcode.Clear();
            txtBrandName.Clear();
            txtDescription.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();
            txtBarcode.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBarcode.Text != "" && txtBrandName.Text != "" && txtDescription.Text != "" && txtPrice.Text != "" && txtQuantity.Text != "")
                {
                    if (MessageBox.Show("Add Product?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cn.Open();
                        cm = new SqlCommand("INSERT INTO tblBarcode (barcode, quantity, brandName, description, price) VALUES (@barcode, @quantity, @brandName, @description, @price)", cn);
                        cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                        cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                        cm.Parameters.AddWithValue("@brandName", txtBrandName.Text);
                        cm.Parameters.AddWithValue("@description", txtDescription.Text);
                        cm.Parameters.AddWithValue("@price", txtPrice.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Product Added Successfully");
                        btnUpdate.Enabled = false;
                        Clear();
                        products.LoadRecords();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter the given details above");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBarcode.Text != "" && txtBrandName.Text != "" && txtDescription.Text != "" && txtPrice.Text != "" && txtQuantity.Text != "")
                {
                    if (MessageBox.Show("Are you sure you want to update this product?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cn.Open();
                        cm = new SqlCommand("UPDATE tblBarcode SET barcode = @barcode, quantity = @quantity, brandName = @brandName, description = @description, price = @price WHERE barcode like '" + lblD.Text + "'", cn);
                        cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                        cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                        cm.Parameters.AddWithValue("@brandName", txtBrandName.Text);
                        cm.Parameters.AddWithValue("@description", txtDescription.Text);
                        cm.Parameters.AddWithValue("@price", txtPrice.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Product Updated Successfully");
                        btnSave.Enabled = false;
                        Clear();
                        products.LoadRecords();
                        this.Dispose();
                    }
                } else
                {
                    MessageBox.Show("Please enter the given details above");
                }
                
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46)
            {
                //para tumanggap ng tuldok .
            }
            else if (e.KeyChar == 8)
            {
                //para tumanggap ng backspace
            }
            else if ((e.KeyChar < 48) || (e.KeyChar > 57))
            {
                e.Handled = true;
            }
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46)
            {
                //para tumanggap ng tuldok .
            }
            else if (e.KeyChar == 8)
            {
                //para tumanggap ng backspace
            }
            else if ((e.KeyChar < 48) || (e.KeyChar > 57))
            {
                e.Handled = true;
            }
        }

        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46)
            {
                //para tumanggap ng tuldok .
            }
            else if (e.KeyChar == 8)
            {
                //para tumanggap ng backspace
            }
            else if ((e.KeyChar < 48) || (e.KeyChar > 57))
            {
                e.Handled = true;
            }
        }
    }
}
