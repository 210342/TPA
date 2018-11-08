using System;
using System.Data;
using System.Data.SqlClient;

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
        public bool TableExists(string SQLTableName)
        {
            int result = 0;
            using (connection = new SqlConnection(connectionString))
            {
                using (sqlCommand = new SqlCommand("select COUNT(*) from INFORMATION_SCHEMA.TABLES where " +
                    "TABLE_SCHEMA = 'dbo' AND TABLE_NAME = @tableName;", connection))
                {
                    sqlCommand.Parameters.Add("@tableName", SqlDbType.VarChar);
                    sqlCommand.Parameters["@tableName"].Value = SQLTableName;
                    try
                    {
                        connection.Open();
                        result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return (result > 0 ? true : false);
        }
        public bool ColumnExists(string dbName, string SQLTableName, string SQLColumn)
        {
            string sqlQuery = $"use {dbName}; select COL_LENGTH('{SQLTableName}', '{SQLColumn}');";
            int result = 0;
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (sqlCommand = new SqlCommand($"use {dbName}; select COL_LENGTH(@tableName, " +
                    "@columnName);", connection))
                {
                    sqlCommand.Parameters.Add("@tableName", SqlDbType.VarChar);
                    sqlCommand.Parameters.Add("@columnName", SqlDbType.VarChar);
                    sqlCommand.Parameters["@tableName"].Value = SQLTableName;
                    sqlCommand.Parameters["@columnName"].Value = SQLColumn;

                    result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                }
            }
            return (result > 0 ? true : false);
        }
    }
}
