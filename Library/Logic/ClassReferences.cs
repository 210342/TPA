using System;
using System.Collections.Generic;
namespace Library.Logic
{
    public static class LoadedClassesReferences
    {
        private static readonly Dictionary<Type, IEnumerable<Type>> Dictionary = 
            new Dictionary<Type, IEnumerable<Type>>();

    }
}
