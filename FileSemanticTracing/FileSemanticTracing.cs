using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracing;

namespace FileSemanticTracing
{
    public class FileSemanticTracing : ITracing
    {
        private readonly string filepath = $"{nameof(FileSemanticTracing)}.log";

        public void LogDatabaseConnectionClosed(string databaseName)
        {
            SemanticLoggingEventSource.Log.DatabaseConnectionClosed(databaseName);
        }

        public void LogDatabaseConnectionEstablished(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void LogFailure(string message)
        {
            throw new NotImplementedException();
        }

        public void LogFileClosed(string filePath)
        {
            throw new NotImplementedException();
        }

        public void LogFileOpened(string filePath)
        {
            throw new NotImplementedException();
        }

        public void LogLoadingModel(string loadedAssemblyName)
        {
            throw new NotImplementedException();
        }

        public void LogModelLoaded(string loadedAssemblyName)
        {
            throw new NotImplementedException();
        }

        public void LogModelSaved(string savedAssemblyName)
        {
            throw new NotImplementedException();
        }

        public void LogSavingModel(string savedAssemblyName)
        {
            throw new NotImplementedException();
        }

        public void LogStartup()
        {
            throw new NotImplementedException();
        }
    }
}
