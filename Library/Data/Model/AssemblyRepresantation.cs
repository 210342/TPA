using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Model
{
    internal class AssemblyRepresantation : IRepresantation
    {
        public string Name { get; private set; }
        public IEnumerable<NamespaceRepresantation> Namespaces { get; private set; }

        public AssemblyRepresantation(Assembly assembly)
        {
            Name = assembly.ManifestModule.Name;

        }

        public string Print()
        {
            throw new NotImplementedException();
        }
    }
}
