using System;

namespace Persistance
{
    public interface IPersister : IDisposable
    {
        FileSystemDependency FileSystemDependency { get; }
        string Target { get; set; }
        void Save(object obj);
        object Load();
    }
}