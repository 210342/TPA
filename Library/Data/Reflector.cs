using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Library.Data.Model;

namespace Library.Data
{
    public class Reflector
    {
        internal AssemblyRepresentation AssemblyModel { get; private set; }

        public Reflector(string assemblyFile)
        {
            if (string.IsNullOrEmpty(assemblyFile))
                throw new ArgumentNullException();
            Assembly assembly = Assembly.LoadFrom(assemblyFile);
            AssemblyModel = new AssemblyRepresentation(assembly);
        }

        public Reflector(Assembly assembly)
        {
            AssemblyModel = new AssemblyRepresentation(assembly);
        }
    }
}
