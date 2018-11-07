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
        #region private fields
        private readonly Dictionary<string, string> acceptedFields = 
            new Dictionary<string, string>() { { "messageField", "log_message" }, { "time", "log_time" } };
        private string connectionString;
        private readonly StringBuilder queriesBuilder = new StringBuilder();
        private DatabaseHandling.IDatabaseWriter databaseWriter;
        #endregion
        #region properties
        public string MessageField
        {
            get
            {
                return acceptedFields["messageField"];
            }
            set
            {
                acceptedFields["messageField"] = value;
            }
        }
        public string TimeField
        {
            get
            {
                return acceptedFields["time"];
            }
            set
            {
                acceptedFields["time"] = value;
            }
        }
        public string DatabaseName { get; set; } = "AppTracing";

        public string TableName { get; set; } = "tracing_logs";

        #endregion
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
            if (!databaseWriter.TableExists(TableName))
            {
                throw new NotSupportedException($"No table {TableName} in database.");
            }
                
            foreach(var kv in acceptedFields)
            {
                if(!databaseWriter.ColumnExists(DatabaseName, TableName, kv.Value))
                {
                    Console.WriteLine(kv.Value);
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
            queriesBuilder.Append($"Insert into {TableName}({acceptedFields["messageField"]}, " +
                $"{acceptedFields["time"]}) " +
                $"values('{message}', '{DateTime.Now}');");
            Console.WriteLine(queriesBuilder.ToString());
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
