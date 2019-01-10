﻿using System;

namespace Tracing
{
    public interface ITracing : IDisposable
    {
        void LogStartup();
        void LogFailure(string message);
        void LogSuccess(string message);
        void LogLoadingModel(string loadedAssemblyName);
        void LogModelLoaded(string loadedAssemblyName);
        void LogSavingModel(string savedAssemblyName);
        void LogModelSaved(string savedAssemblyName);
        void Flush();
    }
}