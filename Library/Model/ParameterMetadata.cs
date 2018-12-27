using ModelContract;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Library.Model
{
    public class ParameterMetadata : IParameterMetadata
    {
        public string Name { get; }
        public ITypeMetadata TypeMetadata { get; private set; }
        public int SavedHash { get; }
        public IEnumerable<IMetadata> Children
        {
            get
            {
                return new[] { TypeMetadata };
            }
            set
            {
                foreach (var elem in value)
                {
                    this.TypeMetadata = (TypeMetadata)elem;
                    break;
                }
            }
        }

        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            if (name == null || typeMetadata == null)
                throw new ArgumentNullException("Neither name or TypeMetadata can be null.");
            this.Name = name;
            this.TypeMetadata = typeMetadata;
            SavedHash = 17;
            SavedHash *= 31 + Name.GetHashCode();
            SavedHash *= 31 + TypeMetadata.GetHashCode();
        }
        internal ParameterMetadata() { }

        #region object overrides

        public override int GetHashCode()
        {
            return SavedHash;
        }

        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType())
                return false;
            ParameterMetadata pm = ((ParameterMetadata)obj);
            if (this.Name == pm.Name)
            {
                if (TypeMetadata !=pm.TypeMetadata)
                    return false;
            }
            return false;
        }

        public override string ToString()
        {
            return Name + ": " + TypeMetadata.Name;
        }

        #endregion
    }
}