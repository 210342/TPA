using System;
using System.Collections.Generic;
using ModelContract;

namespace Library.Model
{
    public class AttributeMetadata : AbstractMapper, IAttributeMetadata
    {
        internal AttributeMetadata(Attribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("Attribute can't be null.");
            Name = attribute.GetType().Name;
            SavedHash = attribute.GetHashCode();
        }

        internal AttributeMetadata()
        {
        }

        public AttributeMetadata(IAttributeMetadata attributeMetadata)
        {
            Name = attributeMetadata.Name;
            SavedHash = attributeMetadata.SavedHash;
        }

        public string Name { get; }
        public IEnumerable<IMetadata> Children => null;
        public int SavedHash { get; }

        public override int GetHashCode()
        {
            return SavedHash;
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
                return false;
            if (Name == ((AttributeMetadata) obj).Name)
                return true;
            return false;
        }

        public override string ToString()
        {
            return "[" + Name + "]";
        }
    }
}