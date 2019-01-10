using System;
using System.Collections.Generic;
using System.Reflection;
using ModelContract;

namespace Library.Data
{
    public static class DataLoadedDictionary
    {
        public static Dictionary<int, IMetadata> Items =
            new Dictionary<int, IMetadata>();

        public static readonly Dictionary<Type, IMetadata> Types =
            new Dictionary<Type, IMetadata>();

        public static IEnumerable<Type> GetKnownMetadata(object obj)
        {
            Type[] types = Assembly.GetAssembly(obj.GetType()).GetTypes();
            foreach (Type type in types)
                if (type.GetInterface("IMetadata") != null)
                    yield return type;
        }
    }
}