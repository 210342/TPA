using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracing;

namespace FileSemanticTracing
{
    public class FileSemanticTracing : ITracing, IDisposable
    {
        private readonly SinkSubscription<FlatFileSink> _subscription;

        public string FilePath { get; }

        public FileSemanticTracing()
        {
            FilePath = $"{nameof(FileSemanticTracing)}.log";
            ObservableEventListener listener = new ObservableEventListener();
            listener.EnableEvents(SemanticLoggingEventSource.Log, System.Diagnostics.Tracing.EventLevel.LogAlways, Keywords.All);
            _subscription = listener.LogToFlatFile(FilePath);
        }

        ~FileSemanticTracing()
        {
            if (_subscription != null)
            {
                _subscription.Dispose();
            }
        }

        public void Dispose()
        {
            if(_subscription != null)
            {
                _subscription.Dispose();
            }
        }

        public void LogDatabaseConnectionClosed(string databaseName)
        {
            SemanticLoggingEventSource.Log.DatabaseConnectionClosed(databaseName);
        }

        public void LogDatabaseConnectionEstablished(string databaseName)
        {
            SemanticLoggingEventSource.Log.DatabaseConnectionEstablished(databaseName);
        }

        public void LogFailure(string message)
        {
            SemanticLoggingEventSource.Log.Failure(message);
        }

        public void LogFileClosed(string filePath)
        {
            SemanticLoggingEventSource.Log.FileClosed(filePath);
        }

        public void LogFileOpened(string filePath)
        {
            SemanticLoggingEventSource.Log.FileOpened(filePath);
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
