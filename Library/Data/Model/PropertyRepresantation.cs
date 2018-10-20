

namespace Library.Data.Model
{
    internal class PropertyRepresantation : IRepresantation
    {
        #region properties
        public string Name { get; private set; }
        public TypeRepresantation TypeMetadata { get; private set; }
        #endregion

        #region constructor
        internal PropertyRepresantation(string propertyName, TypeRepresantation propertyType)
        {
            Name = propertyName;
            TypeMetadata = propertyType;
        }
        #endregion

        public string Print()
        {
            throw new System.NotImplementedException();
        }
    }
}
