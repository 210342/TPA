using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace Tracing
{
    public class DbTraceListener : TraceListener
    {
        private string connectionString;
        private readonly StringBuilder queriesBuilder = new StringBuilder();
        DatabaseHandling.IDatabaseWriter databaseWriter;

        private readonly Dictionary<string, string> acceptedFields = 
            new Dictionary<string, string>() { { "messageField", "log_message" }, { "time", "log_time" } };
        private readonly string tableName = "tracing_logs";
        private readonly string dbName = "AppTracing";

        public DbTraceListener() { }
        public DbTraceListener(string xmlDocPath)
        {
            int readCounter = 0;
            using (XmlReader reader = XmlReader.Create(xmlDocPath))
            {
                while (reader.Read())
                    if (reader.IsStartElement())
                        if (reader.Name == "ConnectionString")
                                if (reader.Read())
                                {
                                    connectionString = reader.Value;
                                    readCounter++;
                                }
            }
            if (readCounter < 1)
                throw new XmlException("Could not read xml file");
            databaseWriter = new DatabaseHandling.DatabaseWriter(connectionString);
            if (!databaseWriter.TableExists(tableName))
            {
                throw new NotSupportedException($"No table {tableName} in database.");
            }
                
            foreach(var kv in acceptedFields)
            {
                if(!databaseWriter.ColumnExists(dbName, tableName, kv.Value))
                {
                    throw new NotSupportedException($"No column {kv.Value} in database.");
                }
                    
            }
        }
        public override void Write(string message)
        {
            WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            string callerMethod = new StackTrace().GetFrame(4).GetMethod().Name;

            queriesBuilder.Append($"Insert into {tableName}({acceptedFields["messageField"]}, " +
                $"{acceptedFields["time"]}) " +
                $"values('{message}', '{DateTime.Now}');");
        }
        public override void Flush()
        {
            if(queriesBuilder.Length > 0)
            {
                databaseWriter.Write(queriesBuilder.ToString());
            }
        }
    }
}
