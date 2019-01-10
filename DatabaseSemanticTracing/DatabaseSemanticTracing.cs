using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics.Tracing;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Tracing;

namespace DatabaseSemanticTracing
{
    [Export(typeof(ITracing))]
    public class DatabaseSemanticTracing : ITracing, IDisposable
    {
        private EventListener _listener;

        public DatabaseSemanticTracing()
        {
            ConnectionString =
                ConfigurationManager.ConnectionStrings["DbSource"].ConnectionString;
            _listener = SqlDatabaseLog.CreateListener("DatabaseSemanticTracing", ConnectionString);
            _listener.EnableEvents(SemanticLoggingEventSource.Log, EventLevel.LogAlways, Keywords.All);
        }

        public string ConnectionString { get; set; }

        public void Dispose()
        {
            _listener?.Dispose();
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
            _listener.Dispose();
            _listener = SqlDatabaseLog.CreateListener("DatabaseSemanticTracing", ConnectionString);
            _listener.EnableEvents(SemanticLoggingEventSource.Log, EventLevel.LogAlways, Keywords.All);
        }

        ~DatabaseSemanticTracing()
        {
            Dispose();
        }
    }
}