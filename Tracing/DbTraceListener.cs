using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics;

namespace Tracing
{
    [Export(typeof(TraceListener))]
    public class DbTraceListener : TraceListener
    {
        #region private fields
        private readonly Dictionary<string, string> acceptedFields = 
            new Dictionary<string, string>() { { "messageField", "log_message" }, { "time", "log_time" } };
        private string connectionString;
        private List<string> queriesBuilt = new List<string>();
        DatabaseHandling.IDatabaseWriter databaseWriter;

        #endregion
        public string LogField
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
        public string TableName { get; set; } = "tracing_logs";
        public string DatabaseName { get; set; } = "AppTracing";

        public DbTraceListener(DatabaseHandling.IDatabaseWriter dbWriter)
        {
            this.databaseWriter = dbWriter;
        }
        public DbTraceListener()
        {
            this.connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            databaseWriter = new DatabaseHandling.DatabaseWriter(connectionString);
            if (!databaseWriter.TableExists(TableName))
            {
                throw new NotSupportedException($"No table {TableName} in database.");
            }
                
            foreach(var kv in acceptedFields)
            {
                if(!databaseWriter.ColumnExists(DatabaseName, TableName, kv.Value))
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

            queriesBuilt.Add($"Insert into {TableName}({acceptedFields["messageField"]}, " +
                $"{acceptedFields["time"]}) " +
                $"values('[{callerMethod}] {message}', '{DateTime.Now}');");
        }
        public override void Flush()
        {
            if(queriesBuilt.Count > 0)
            {
                foreach(string query in queriesBuilt)
                    databaseWriter.WriteQuery(query);
                queriesBuilt.Clear();
            }
        }
    }
}
