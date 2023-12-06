using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoS_Inventory
{
    internal class DBConnection
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public string MyConnection()
        {
            string con = @"Data Source=HEART;Initial Catalog=PoS_Demo;Integrated Security=True";
            return con;
        }
        public double GetVal()
        {
            double vat = 0.12;
            return vat;
        }
    }
}
