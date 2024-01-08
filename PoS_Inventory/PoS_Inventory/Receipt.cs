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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing.Printing;

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
        public Receipt(string transNum, double cash, double change, double vat, double vatable, double total)
        {
            InitializeComponent();
            con = new SqlConnection(dbcon.MyConnection());
            transno = transNum;
            lbltransNum.Text = transNum;
            lblCash.Text = cash.ToString();
            lblChange.Text = change.ToString();
            lblVAT.Text = vat.ToString();
            lblVatable.Text = vatable.ToString();
            lblTotalPrice.Text = total.ToString();
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
            this.Dispose();
            PoS pos = new PoS();
            pos.Show();
        }
        public void DisplayReceipt(string num)
        {
            dataGridView1.Columns.Clear();
            cn = new SqlConnection(dbcon.MyConnection());
            cn.Open();
            string query = $"SELECT * FROM tblCarts_for_{num}";
            cm = new SqlCommand(query, cn);
            dr = cm.ExecuteReader();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                dataGridView1.Columns.Add(dr.GetName(i), dr.GetName(i));
            }
            while (dr.Read())
            {
                int numColumns = dr.FieldCount;
                object[] rowValues = new object[numColumns];
                for (int i = 0; i < numColumns; i++)
                {
                    if (dr.GetName(i) == "sdate" && dr.GetValue(i) is DateTime)
                    {
                        rowValues[i] = ((DateTime)dr.GetValue(i)).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        rowValues[i] = dr.GetValue(i);
                    }
                }
                dataGridView1.Rows.Add(rowValues);
            }
            dr.Close();
            cn.Close();
        }


        private void SaveReceiptAsPDF()
        {
            string filePath = $"E:\\Amiel\\hehe\\{lbltransNum.Text}.pdf";
            if (File.Exists(filePath))
            {
                MessageBox.Show($"Error: File already exists in {filePath}.");
            }
            else
            {
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                writer.PageEvent = new PdfHeaderFooter();
                doc.Open();
                PdfPTable table = new PdfPTable(dataGridView1.ColumnCount);
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    table.AddCell(new Phrase(dataGridView1.Columns[j].HeaderText));
                }
                table.HeaderRows = 1;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int k = 0; k < dataGridView1.Columns.Count; k++)
                    {
                        if (dataGridView1[k, i].Value != null)
                        {
                            table.AddCell(new Phrase(dataGridView1[k, i].Value.ToString()));
                        }
                    }
                }
                MessageBox.Show($"Receipt saved to {filePath}.");
                doc.Add(table);
                doc.Close();
                writer.Close();
            }
        }

        private void PrintReceipt()
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += printDoc_PrintPage;
            PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();
            printPrvDlg.Document = printDoc;
            if (printPrvDlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
            dataGridView1.DrawToBitmap(bm, new System.Drawing.Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
            e.Graphics.DrawImage(bm, 0, 0);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveReceiptAsPDF();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SaveReceiptAsPDF();
            PrintReceipt();
        }
    }
}