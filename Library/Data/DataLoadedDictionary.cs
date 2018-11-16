using Library.Data.Model;
using System.Collections.Generic;

namespace Library.Data
{
    public static class DataLoadedDictionary
    {
        public static readonly Dictionary<int, IMetadata> Items = 
            new Dictionary<int, IMetadata>();
        public static readonly Dictionary<System.Type, IMetadata> Types =
            new Dictionary<System.Type, IMetadata>();
    }
}
