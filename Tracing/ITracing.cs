using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracing
{
    public interface ITracing
    {
        void LogStartup();
        void LogFailure(string message);
        void LogFileOpened(string filePath);
        void LogFileClosed(string filePath);
        void LogDatabaseConnectionEstablished(string databaseName);
        void LogDatabaseConnectionClosed(string databaseName);
        void LogLoadingModel(string loadedAssemblyName);
        void LogModelLoaded(string loadedAssemblyName);
        void LogSavingModel(string savedAssemblyName);
        void LogModelSaved(string savedAssemblyName);
    }
}
