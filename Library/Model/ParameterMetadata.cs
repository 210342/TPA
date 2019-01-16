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
                throw new ArgumentNullException("Neither name or MyType can be null.");
            Name = name;
            MyType = typeMetadata;
            SavedHash = 17;
            SavedHash *= 31 + Name.GetHashCode();
            SavedHash *= 31 + MyType.GetHashCode();
        }

        internal ParameterMetadata()
        {
        }

        public ParameterMetadata(IParameterMetadata parameterMetadata)
        {
            Name = parameterMetadata.Name;
            SavedHash = parameterMetadata.SavedHash;
            if (AlreadyMapped.TryGetValue(parameterMetadata.MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
            }
            else
            {
                MyType = new TypeMetadata(parameterMetadata.MyType.SavedHash, parameterMetadata.MyType.Name);
            }
        }

        public string Name { get; }
        public ITypeMetadata MyType { get; private set; }
        public int SavedHash { get; }

        public IEnumerable<IMetadata> Children
        {
            get => new[] {MyType};
            set
            {
                foreach (IMetadata elem in value)
                {
                    MyType = (TypeMetadata) elem;
                    break;
                }
            }
        }

        public void MapTypes()
        {
            if (!MyType.Mapped && AlreadyMapped.TryGetValue(MyType.SavedHash, out IMetadata item))
            {
                MyType = item as ITypeMetadata;
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
                if (MyType != pm.MyType)
                    return false;
            return false;
        }

        public override string ToString()
        {
            return Name + ": " + MyType.Name;
        }

        #endregion
    }
}