﻿using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace Tracing
{
    public class DbTraceListener : TraceListener
    {
        private string connectionString;
        private string tableName;
        private string recordName;
        private readonly StringBuilder messageBuilder = new StringBuilder();
        private readonly StringBuilder queriesBuilder = new StringBuilder();
        DatabaseHandling.IDatabaseWriter databaseWriter;

        public DbTraceListener() { }
        public DbTraceListener(string xmlDocPath)
        {
            int readCounter = 0;
            using (XmlReader reader = XmlReader.Create(xmlDocPath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "ConnectionString":
                                if (reader.Read())
                                {
                                    connectionString = reader.Value;
                                    readCounter++;
                                }
                                break;
                            case "TableName":
                                if(reader.Read())
                                {
                                    tableName = reader.Value;
                                    readCounter++;
                                }
                                break;
                            case "RecordName":
                                if (reader.Read())
                                {
                                    recordName = reader.Value;
                                    readCounter++;
                                }
                                break;
                        }
                    }
                }
            }
            if (readCounter < 3)
                throw new XmlException("Could not read xml file");
            databaseWriter = new DatabaseHandling.DatabaseWriter(connectionString);
        }
        public override void Write(string message)
        {
            messageBuilder.Append(message);
        }

        public override void WriteLine(string message)
        {
            queriesBuilder.Append($"Insert into {tableName}({recordName}) values('{message}'); ");
        }
        public override void Flush()
        {
            if(messageBuilder.Length > 0)
            {
                string sqlQuery = $"Insert into {tableName}({recordName}) values('{messageBuilder.ToString()}');";
                databaseWriter.Write(sqlQuery);
            }
            if(queriesBuilder.Length > 0)
            {
                databaseWriter.Write(queriesBuilder.ToString());
            }
        }
    }
}
