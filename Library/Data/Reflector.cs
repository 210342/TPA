using System;
using System.Reflection;
using Library.Model;

namespace Library.Data
{
    public class Reflector
    {
        public Reflector(string assemblyFile)
        {
            if (string.IsNullOrEmpty(assemblyFile))
                throw new ArgumentNullException("Assembly path can't be null or empty");

            Assembly assembly = Assembly.ReflectionOnlyLoadFrom(assemblyFile);
            AssemblyModel = new AssemblyMetadata(assembly);
        }

        public Reflector(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("Assembly path can't be null or empty");
            AssemblyModel = new AssemblyMetadata(assembly);
        }

        internal AssemblyMetadata AssemblyModel { get; private set; }
    }
}