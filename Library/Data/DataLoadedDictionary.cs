using Library.Model;
using ModelContract;
using System.Collections.Generic;
using System.Reflection;

namespace Library.Data
{
    public static class DataLoadedDictionary
    {
        public static Dictionary<int, IMetadata> Items = 
            new Dictionary<int, IMetadata>();
        public static readonly Dictionary<System.Type, IMetadata> Types =
            new Dictionary<System.Type, IMetadata>();
        public static IEnumerable<System.Type> GetKnownMetadata(object obj)
        {
            System.Type[] types = Assembly.GetAssembly(obj.GetType()).GetTypes();
            foreach (System.Type type in types)
                if (type.GetInterface("IMetadata") != null)
                    yield return type;
        }
    }
}
