using System.Linq;
using System.Reflection;
using ModelContract;

namespace Library.Data
{
    internal class ModelMapper
    {
        public IAssemblyMetadata Map(IAssemblyMetadata root, Assembly model)
        {
            var rootType = (from type in model.GetTypes()
                where typeof(IAssemblyMetadata).IsAssignableFrom(type) && !type.IsInterface
                select type).First();
            var ctor = rootType.GetConstructor(new[] {typeof(IAssemblyMetadata)});
            return ctor.Invoke(new[] {root}) as IAssemblyMetadata;
        }
    }
}