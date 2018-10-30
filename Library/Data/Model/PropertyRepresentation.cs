

using System.Collections.Generic;

namespace Library.Data.Model
{
    internal class PropertyRepresentation : IRepresentation
    {
        #region properties
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public TypeRepresentation Type { get; private set; }
        public IEnumerable<string> Children
        {
            get
            {
                return Print();
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
    }
}
