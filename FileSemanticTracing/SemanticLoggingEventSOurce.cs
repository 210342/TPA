using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticTracing
{
    [EventSource(Name = "FileSemanticLogging")]
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
            public const EventKeywords Diagnostic = (EventKeywords)2;
            public const EventKeywords Performance = (EventKeywords)4;
        }

        public enum EventID
        {
            Startup = 1,
            Failure = 2,
            FileOpened = 3,
            FileClosed = 4,
            LoadingModel = 5,
            ModelLoaded = 6,
            SavingModel = 7,
            ModelSaved = 8,
            Success = 9
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

        [Event((int)EventID.Success, Message = "OperationSuccess: {0}", Keywords = Keywords.Diagnostic, Level = EventLevel.Verbose)]
        public void Success(string message)
        {
            WriteEvent((int)EventID.Success, message);
        }

        [Event((int)EventID.LoadingModel, Message = "{0}", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic, Opcode = EventOpcode.Start)]
        public void LoadingModel(string loadedAssemblyName)
        {
            if (IsEnabled())
            {
                WriteEvent((int)EventID.LoadingModel, loadedAssemblyName);
            }
        }

        [Event((int)EventID.ModelLoaded, Message = "{0}", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic, Opcode = EventOpcode.Stop)]
        public void ModelLoaded(string loadedAssemblyName)
        {
            if (IsEnabled())
            {
                WriteEvent((int)EventID.ModelLoaded, loadedAssemblyName);
            }
        }

        [Event((int)EventID.SavingModel, Message = "{0}", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic, Opcode = EventOpcode.Start)]
        public void SavingModel(string savedAssemblyName)
        {
            if (IsEnabled())
            {
                WriteEvent((int)EventID.SavingModel, savedAssemblyName);
            }
        }

        [Event((int)EventID.ModelSaved, Message = "{0}", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic, Opcode = EventOpcode.Stop)]
        public void ModelSaved(string savedAssemblyName)
        {
            if (IsEnabled())
            {
                WriteEvent((int)EventID.ModelSaved, savedAssemblyName);
            }
        }
    }
}
