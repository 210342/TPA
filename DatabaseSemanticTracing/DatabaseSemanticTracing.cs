﻿using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Tracing;
using System.Xml;
using Tracing;

namespace DatabaseSemanticTracing
{
    [Export(typeof(ITracing))]
    public class DatabaseSemanticTracing : ITracing, IDisposable
    {
        private EventListener _listener;

        public string ConnectionString { get; set; }

        public DatabaseSemanticTracing()
        {
            using (XmlReader reader = 
                XmlReader.Create($"{System.IO.Path.GetDirectoryName(GetType().Assembly.Location)}/DatabaseConfiguration.xml"))
            {
                while (reader.Read() && string.IsNullOrEmpty(ConnectionString))
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "ConnectionString")
                        {
                            if (reader.Read())
                            {
                                ConnectionString = reader.Value;
                            }
                        }
                    }
                }
            }
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
            _listener.Dispose();
            _listener = SqlDatabaseLog.CreateListener("DatabaseSemanticTracing", ConnectionString);
            _listener.EnableEvents(SemanticLoggingEventSource.Log, EventLevel.LogAlways, Keywords.All);
        }
    }
}
