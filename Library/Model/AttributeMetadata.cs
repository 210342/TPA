using ModelContract;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Library.Model
{
    public class AttributeMetadata : IAttributeMetadata
    {
        public string Name { get; }
        public IEnumerable<IMetadata> Children => null;
        public int SavedHash { get; }

        internal AttributeMetadata(Attribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("Attribute can't be null.");
            Name = attribute.GetType().Name;
            SavedHash = attribute.GetHashCode();
        }
        internal AttributeMetadata() { }

        public override int GetHashCode()
        {
            return SavedHash;
        }

        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;
            if (this.Name == ((AttributeMetadata)obj).Name)
                return true;
            else
                return false;
        }
        public override string ToString()
        {
            return "[" + Name + "]";
        }
    }
}
