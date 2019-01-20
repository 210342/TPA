using ModelContract;
using System;
using System.Threading.Tasks;

namespace Persistence
{
    public interface IPersister : IDisposable
    {
        FileSystemDependency FileSystemDependency { get; }
        void Access(string target);
        Task Save(IAssemblyMetadata obj);
        Task<IAssemblyMetadata> Load();
    }
}