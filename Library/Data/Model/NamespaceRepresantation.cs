using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Model
{
    internal class NamespaceRepresantation : IRepresantation
    {
        public string Name { get; private set; }
        public string FullName { get; }
        public IEnumerable<TypeRepresantation> Types { get; private set; }

        internal NamespaceRepresantation(string name, IEnumerable<Type> types)
        {
            Name = name;
            FullName = name;
            Types = ReadMetadata.ReadTypes(types);
        }

        public IEnumerable<string> Print()
        {
            yield return $"NAME: {Name}";
            foreach (TypeRepresantation _type in Types)
            {
                yield return $"Type: {_type.Name}";
            }
        }
    }
}
