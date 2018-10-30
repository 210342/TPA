using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Model
{
    internal class NamespaceRepresentation : IRepresentation
    {
        public string Name { get; private set; }
        public string FullName { get; }
        public IEnumerable<TypeRepresentation> Types { get; private set; }
        public IEnumerable<string> Children
        {
            get
            {
                return Print();
            }
        }

        internal NamespaceRepresentation(string name, IEnumerable<Type> types)
        {
            Name = name;
            FullName = name;
            Types = ReadMetadata.ReadTypes(types);
        }

        public IEnumerable<string> Print()
        {
            yield return $"NAME: {Name}";
            foreach (TypeRepresentation _type in Types)
            {
                yield return $"Type: {_type.Name}";
            }
        }
    }
}
