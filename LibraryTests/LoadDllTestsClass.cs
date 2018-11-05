using Library.Data;
using Library.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library.Logic
{
    public static class LoadDllTestsClass
    {
        private static Assembly currentAssembly;

        public static void LoadAssembly(string path)
        {
            currentAssembly = Assembly.LoadFrom(path);
        }

        internal static TypeRepresentation[] MemberTypes()
        {
            IEnumerable<Type> types = currentAssembly.GetTypes();
            IEnumerable<TypeRepresentation> tmp = ReadMetadata.ReadTypes(types);
            return tmp.ToArray();
        }

        internal static int DictSize()
        {
            return ReadMetadata.AlreadyRead.Count;
        }
    }
}
