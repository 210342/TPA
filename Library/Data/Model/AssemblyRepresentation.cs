using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Model
{
    internal class AssemblyRepresentation : IRepresentation
    {
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public IEnumerable<NamespaceRepresentation> Namespaces { get; private set; }
        public IEnumerable<string> Children
        {
            get
            {
                return Print();
            }
        }

        public AssemblyRepresentation(Assembly assembly)
        {
            Name = assembly.ManifestModule.Name;
            FullName = Name;
            Namespaces = ReadMetadata.ReadNamespaces(assembly);
        }

        public IEnumerable<string> Print()
        {
            yield return $"NAME: {Name}";
            foreach(NamespaceRepresentation _namespace in Namespaces)
            {
                yield return $"Namespace: {_namespace.Name}";
            }
        }
    }
}
