using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracing.DatabaseHandling
{
    public class DatabaseWriter : IDatabaseWriter
    {
        SqlConnection connection;
        SqlCommand sqlCommand;
        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        string connectionString;

        public DatabaseWriter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Write(string data)
        {
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (sqlCommand = new SqlCommand(data, connection))
                {
                    sqlAdapter.InsertCommand = sqlCommand;
                    sqlAdapter.InsertCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
