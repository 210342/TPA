using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Tracing;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using Tracing;

namespace SemanticTracing
{
    [Export(typeof(ITracing))]
    public class FileSemanticTracing : ITracing
    {
        private readonly SinkSubscription<FlatFileSink> _subscription;

        public FileSemanticTracing()
        {
            FindViableFilePath();
            ObservableEventListener listener = new ObservableEventListener();
            listener.EnableEvents(SemanticLoggingEventSource.Log, EventLevel.LogAlways, Keywords.All);
            _subscription = listener.LogToFlatFile(FilePath);
        }

        public string FilePath { get; private set; }

        public void Dispose()
        {
            if (_subscription != null) _subscription.Dispose();
            GC.SuppressFinalize(this);
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

        private void FindViableFilePath()
        {
            bool notViable = true;
            while (notViable)
            {
                FilePath = $"{nameof(FileSemanticTracing)} {DateTime.Now.Ticks}.log";
                if (!File.Exists(FilePath)) notViable = false;
            }
        }
    }
}