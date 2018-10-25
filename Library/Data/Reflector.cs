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
        internal AssemblyRepresantation AssemblyModel { get; private set; }

        public Reflector(string assemblyFile)
        {
            if (string.IsNullOrEmpty(assemblyFile))
                throw new System.ArgumentNullException();
            Assembly assembly = Assembly.LoadFrom(assemblyFile);
            AssemblyModel = new AssemblyRepresantation(assembly);
        }

        public Reflector(Assembly assembly)
        {
            AssemblyModel = new AssemblyRepresantation(assembly);
        }
    }
}
