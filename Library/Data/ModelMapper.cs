using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    internal class ModelMapper
    {
        public IAssemblyMetadata Map(IAssemblyMetadata root, Assembly model)
        {
            MappingDictionary.AlreadyMapped.Clear();
            Type rootType = (from type in model.GetTypes()
                             where typeof(IAssemblyMetadata).IsAssignableFrom(type) && !type.IsInterface
                             select type).First();
            ConstructorInfo ctor = rootType.GetConstructor(new Type[] { typeof(IAssemblyMetadata) });
            return ctor.Invoke(new[] { root }) as IAssemblyMetadata;
        }
    }
}
