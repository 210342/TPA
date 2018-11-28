using Library.Data.Model;
using System;
using System.Reflection;

namespace Library.Data
{
    public class Reflector
    {
        public Reflector(string assemblyFile)
        {
            if (string.IsNullOrEmpty(assemblyFile))
                throw new ArgumentNullException("Assembly path can't be null or empty");

            Assembly assembly = Assembly.UnsafeLoadFrom(assemblyFile);
            m_AssemblyModel = new AssemblyMetadata(assembly);
        }
        public Reflector(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("Assembly path can't be null or empty");
            m_AssemblyModel = new AssemblyMetadata(assembly);
        }
        internal AssemblyMetadata m_AssemblyModel { get; private set; }

    }
}
