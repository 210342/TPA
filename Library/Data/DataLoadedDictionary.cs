using System;
using System.Collections.Generic;
using System.Reflection;
using ModelContract;

namespace Library.Data
{
    public static class DataLoadedDictionary
    {
        public static Dictionary<int, IMetadata> Items { get; } =
            new Dictionary<int, IMetadata>();
    }
}