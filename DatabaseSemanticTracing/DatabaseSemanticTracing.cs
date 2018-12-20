using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using System;
using System.Data.SqlClient;
using Tracing;

namespace DatabaseSemanticTracing
{
    public class DatabaseSemanticTracing : ITracing, IDisposable
    {
        SqlConnection connection;
        private readonly SinkSubscription<SqlDatabaseSink> _subscription;
        string createTracingTableQuery =
            "CREATE TABLE[dbo].[Traces]" +
           "([id][bigint] IDENTITY(1,1) NOT NULL,"+
          "[InstanceName] [nvarchar] (1000) NOT NULL,"+
           "[ProviderId] [uniqueidentifier] NOT NULL," +
           "[ProviderName] [nvarchar] (500) NOT NULL, "+
            "[EventId] [int] NOT NULL,"+
            "[EventKeywords] [bigint] NOT NULL,"+
            "[Level] [int] NOT NULL,"+
           " [Opcode] [int] NOT NULL,"+
             "[Task] [int] NOT NULL,"+
             "[Timestamp] [datetimeoffset] (7) NOT NULL,"+
            " [Version] [int] NOT NULL,"+
             "[FormattedMessage] [nvarchar] (4000) NULL,"+
	       " [Payload] [nvarchar] (4000) NULL,"+
	       " [ActivityId] [uniqueidentifier],"+
	       " [RelatedActivityId] [uniqueidentifier],"+
	       " [ProcessId] [int],"+
	        "[ThreadId] [int],"+
            "CONSTRAINT[PK_Traces] PRIMARY KEY CLUSTERED"+
            "("+
            "     [id] ASC"+
            " ) WITH(STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)"+
            ")";

        public DatabaseSemanticTracing()
        {
            connection = new SqlConnection(@"Data Source=SCRIPT-PC\SQLEXPRESS;Initial Catalog=AppTracing;User ID=SCRIPT-PC\Script;Integrated Security=True");
            connection.Open();
            int quantity = 0;
            using (SqlCommand checkIfExists = new SqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' " +
                "AND  TABLE_NAME = 'Traces'", connection))
            {
                quantity = (int)checkIfExists.ExecuteScalar();
            }
            if(quantity == 0)
            {
                using (SqlCommand createTable = new SqlCommand(createTracingTableQuery, connection))
                {
                    createTable.ExecuteNonQuery();
                }
            }
                ObservableEventListener listener = new ObservableEventListener();
            
            _subscription = listener.LogToSqlDatabase("DatabaseSemanticTracing",
                @"Data Source=SCRIPT-PC\SQLEXsssPRESS;Initial Catalog=AppTracing;User ID=SCRIPT-PC\Script;Integrated Security=True");
            listener.EnableEvents(SemanticLoggingEventSource.Log, System.Diagnostics.Tracing.EventLevel.LogAlways, Keywords.All);
            //_subscription = listener.LogToSqlDatabase("DatabaseSemanticTracing", 
            //    @"Data Source=SCRIPT-PC\SQLEXPRESS;Initial Catalog=AppTracing;User ID=SCRIPT-PC\Script;Integrated Security=True");
           
        }

        ~DatabaseSemanticTracing()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            connection.Dispose();
            if(_subscription != null)
            {
                _subscription.Dispose();
            }
        }
        public void LogFailure(string message)
        {
            SemanticLoggingEventSource.Log.Failure(message);
        }
        public void LogSuccess(string message)
        {
            SemanticLoggingEventSource.Log.Success(message);
        }
        public void LogLoadingModel(string loadedAssemblyName)
        {
            SemanticLoggingEventSource.Log.LoadingModel(loadedAssemblyName);
        }

        public void LogModelLoaded(string loadedAssemblyName)
        {
            SemanticLoggingEventSource.Log.ModelLoaded(loadedAssemblyName);
        }

        public void LogModelSaved(string savedAssemblyName)
        {
            SemanticLoggingEventSource.Log.ModelSaved(savedAssemblyName);
        }

        public void LogSavingModel(string savedAssemblyName)
        {
            SemanticLoggingEventSource.Log.SavingModel(savedAssemblyName);
        }

        public void LogStartup()
        {
            SemanticLoggingEventSource.Log.Startup();
        }

        public void Flush()
        {
            _subscription.Sink.FlushAsync();
        }
    }
}
