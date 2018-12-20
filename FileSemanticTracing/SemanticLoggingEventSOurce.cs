using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSemanticTracing
{
    [EventSource(Name = "SemanticLogging")]
    public class SemanticLoggingEventSource : EventSource
    {
        public static SemanticLoggingEventSource Log { get; } = new SemanticLoggingEventSource();
        private SemanticLoggingEventSource() { }

        public class Tasks
        {
            public const EventTask Page = (EventTask)1;
            public const EventTask DatabaseConnection = (EventTask)2;
            public const EventTask File = (EventTask)16;
        }

        public class Keywords
        {
            public const EventKeywords Page = (EventKeywords)1;
            public const EventKeywords DataBase = (EventKeywords)2;
            public const EventKeywords Diagnostic = (EventKeywords)4;
            public const EventKeywords Performance = (EventKeywords)8;
            public const EventKeywords File = (EventKeywords)16;
        }

        public enum EventID
        {
            Startup = 1,
            Failure = 2,
            FileOpened = 3,
            FileClosed = 4,
            DatabaseConnectionEstablished = 5,
            DatabaseConnectionClosed = 6,
            LoadingModel = 7,
            ModelLoaded = 8,
            SavingModel = 9,
            ModelSaved = 10
        }

        [Event((int)EventID.Startup, Message = "Starting up", Keywords = Keywords.Performance, Level = EventLevel.Informational)]
        public void Startup()
        {
            WriteEvent((int)EventID.Startup);
        }
        [Event((int)EventID.Failure, Message = "ApplicationFailure: {0}", Keywords = Keywords.Diagnostic, Level = EventLevel.Error)]
        public void Failure(string message)
        {
            WriteEvent((int)EventID.Failure, message);
        }

        [Event((int)EventID.FileOpened, Message = "Opening file {0}", Opcode = EventOpcode.Start,
            Task = Tasks.File, Keywords = Keywords.File, Level = EventLevel.Informational)]
        public void FileOpened(string filePath)
        {
            if (this.IsEnabled())
            {
                WriteEvent((int)EventID.FileOpened, filePath);
            }
        }

        [Event((int)EventID.FileClosed, Message = "Closing file {0}", Opcode = EventOpcode.Stop, Task = Tasks.File, Keywords = Keywords.File, Level = EventLevel.Informational)]
        public void FileClosed(string filePath)
        {
            if (this.IsEnabled())
            {
                WriteEvent((int)EventID.FileClosed, filePath);
            }
        }

        [Event((int)EventID.DatabaseConnectionEstablished, Opcode = EventOpcode.Start, Task = Tasks.DatabaseConnection, Keywords = Keywords.DataBase, Level = EventLevel.Informational)]
        public void DatabaseConnectionEstablished(string databaseName)
        {
            WriteEvent((int)EventID.DatabaseConnectionEstablished, databaseName);
        }

        [Event((int)EventID.DatabaseConnectionClosed, Opcode = EventOpcode.Stop, Task = Tasks.DatabaseConnection, Keywords = Keywords.DataBase, Level = EventLevel.Informational)]
        public void DatabaseConnectionClosed(string databaseName)
        {
            WriteEvent((int)EventID.DatabaseConnectionClosed, databaseName);
        }

        [Event((int)EventID.LoadingModel, Level = EventLevel.Informational, Keywords = Keywords.Diagnostic, Opcode = EventOpcode.Start)]
        public void LoadingModel(string loadedAssemblyName)
        {
            if (IsEnabled())
            {
                WriteEvent((int)EventID.LoadingModel, loadedAssemblyName);
            }
        }

        [Event((int)EventID.ModelLoaded, Level = EventLevel.Informational, Keywords = Keywords.Diagnostic, Opcode = EventOpcode.Stop)]
        public void ModelLoaded(string loadedAssemblyName)
        {
            if (IsEnabled())
            {
                WriteEvent((int)EventID.ModelLoaded, loadedAssemblyName);
            }
        }

        [Event((int)EventID.SavingModel, Level = EventLevel.Informational, Keywords = Keywords.Diagnostic, Opcode = EventOpcode.Start)]
        public void SavingModel(string savedAssemblyName)
        {
            if (IsEnabled())
            {
                WriteEvent((int)EventID.SavingModel, savedAssemblyName);
            }
        }

        [Event((int)EventID.ModelSaved, Level = EventLevel.Informational, Keywords = Keywords.Diagnostic, Opcode = EventOpcode.Stop)]
        public void ModelSaved(string savedAssemblyName)
        {
            if (IsEnabled())
            {
                WriteEvent((int)EventID.ModelSaved, savedAssemblyName);
            }
        }
    }
}
