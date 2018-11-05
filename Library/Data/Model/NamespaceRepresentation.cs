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
        public IEnumerable<IRepresentation> Children
        {
            get
            {
                foreach(TypeRepresentation _type in Types)
                {
                    yield return _type;
                }
            }
        }
        public string ToStringProperty
        {
            get
            {
                return ToString();
            }
        }

        internal NamespaceRepresentation(string name, IEnumerable<Type> types)
        {
            Name = name;
            FullName = name;
            Types = ReadMetadata.ReadTypes(types);
        }

        #region Methods

        public IEnumerable<string> Print()
        {
            yield return $"NAME: {Name}";
            foreach (TypeRepresentation _type in Types)
            {
                yield return $"Type: {_type.Name}";
            }
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Print());
        }
        #endregion
    }
}
