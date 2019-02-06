using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace bestilvasketid.dk.Controllers
{
    public class SQLClass
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public void WriteSQLDatatable(string query, DataTable dataTable, string dataTableType)
        {
            SqlCommand command = new SqlCommand(query, con);
            con.Open();
            var param = command.Parameters.AddWithValue("@DataTable", dataTable);
            param.TypeName = dataTableType;
            command.ExecuteNonQuery();
            con.Close();
        }

        public DataTable ReadSQLDatatable(string query)
        {
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            con.Close();
            da.Dispose();

            return dataTable;
        }

        public List<string> ReadSQL(string query)
        {
            List<string> SQLData = new List<string>();
            con.Open();
            using (SqlCommand command = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            SQLData.Add(reader.GetValue(i).ToString());
                        }
                    }
                }
            }
            con.Close();
            return SQLData;
        }
    }
}