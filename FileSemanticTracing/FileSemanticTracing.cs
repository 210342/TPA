﻿using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using System;
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
