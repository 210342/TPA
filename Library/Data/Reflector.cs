using System;
using System.Reflection;
using TPA.Reflection.Model;

namespace TPA.Reflection
{
    //TODO add UT - testing data is required
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
