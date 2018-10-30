

using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Data.Model
{
    internal class PropertyRepresentation : IRepresentation
    {
        #region properties
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public TypeRepresentation Type { get; private set; }
        public IEnumerable<IRepresentation> Children
        {
            get
            {
                yield return Type;
            }
        }
        public string ToStringProperty
        {
            get
            {
                return ToString();
            }
        }
        #endregion

        #region constructor
        internal PropertyRepresentation(string propertyName, TypeRepresentation propertyType, string className)
        {
            Name = propertyName;
            FullName = $"{className}.{propertyName}";
            Type = propertyType;
        }
        #endregion

        public IEnumerable<string> Print()
        {
            yield return $"NAME: {Name}";
            yield return $"Type: {Type.Name}";
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Print());
        }
    }
}
