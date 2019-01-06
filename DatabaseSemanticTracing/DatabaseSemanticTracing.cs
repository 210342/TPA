using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using System;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Diagnostics.Tracing;
using Tracing;

namespace DatabaseSemanticTracing
{
    [Export(typeof(ITracing))]
    public class DatabaseSemanticTracing : ITracing, IDisposable
    {
        private readonly EventListener _listener;

        public string ConnectionString { get; set; } = @"Data Source=DESKTOP-MPF14F3\SQLEXPRESS;Initial Catalog=Logging;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public DatabaseSemanticTracing()
        {
            _listener = SqlDatabaseLog.CreateListener("DatabaseSemanticTracing", ConnectionString);
            _listener.EnableEvents(SemanticLoggingEventSource.Log, EventLevel.LogAlways, Keywords.All);
        }

        ~DatabaseSemanticTracing()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if(_listener != null)
            {
                _listener.Dispose();
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
            // nothing to do
        }
    }
}
