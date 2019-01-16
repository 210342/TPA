using ModelContract;
using System;

namespace Persistance
{
    public interface IPersister : IDisposable
    {
        FileSystemDependency FileSystemDependency { get; }
        void Access(string target);
        void Save(IAssemblyMetadata obj);
        IAssemblyMetadata Load();
    }
}