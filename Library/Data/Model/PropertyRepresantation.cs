

using System.Collections.Generic;

namespace Library.Data.Model
{
    internal class PropertyRepresantation : IRepresantation
    {
        #region properties
        public string Name { get; private set; }
        public TypeRepresantation Type { get; private set; }
        #endregion

        #region constructor
        internal PropertyRepresantation(string propertyName, TypeRepresantation propertyType)
        {
            Name = propertyName;
            Type = propertyType;
        }
        #endregion

        IEnumerable<string> IRepresantation.Print()
        {
            yield return $"NAME: {Name}";
            yield return $"Type: {Type.Name}";
        }
    }
}
