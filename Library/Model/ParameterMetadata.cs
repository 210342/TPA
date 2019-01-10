using System;
using System.Collections.Generic;
using ModelContract;

namespace Library.Model
{
    public class ParameterMetadata : AbstractMapper, IParameterMetadata
    {
        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            if (name == null || typeMetadata == null)
                throw new ArgumentNullException("Neither name or TypeMetadata can be null.");
            Name = name;
            TypeMetadata = typeMetadata;
            SavedHash = 17;
            SavedHash *= 31 + Name.GetHashCode();
            SavedHash *= 31 + TypeMetadata.GetHashCode();
        }

        internal ParameterMetadata()
        {
        }

        public ParameterMetadata(IParameterMetadata parameterMetadata)
        {
            Name = parameterMetadata.Name;
            SavedHash = parameterMetadata.SavedHash;
            if (AlreadyMapped.TryGetValue(parameterMetadata.TypeMetadata.SavedHash, out IMetadata item))
            {
                TypeMetadata = item as ITypeMetadata;
            }
            else
            {
                ITypeMetadata newType = new TypeMetadata(parameterMetadata.TypeMetadata);
                TypeMetadata = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
            }
        }

        public string Name { get; }
        public ITypeMetadata TypeMetadata { get; private set; }
        public int SavedHash { get; }

        public IEnumerable<IMetadata> Children
        {
            get => new[] {TypeMetadata};
            set
            {
                foreach (IMetadata elem in value)
                {
                    TypeMetadata = (TypeMetadata) elem;
                    break;
                }
            }
        }

        #region object overrides

        public override int GetHashCode()
        {
            return SavedHash;
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
                return false;
            ParameterMetadata pm = (ParameterMetadata) obj;
            if (Name == pm.Name)
                if (TypeMetadata != pm.TypeMetadata)
                    return false;
            return false;
        }

        public override string ToString()
        {
            return Name + ": " + TypeMetadata.Name;
        }

        #endregion
    }
}