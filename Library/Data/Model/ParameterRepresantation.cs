using System.Collections.Generic;

namespace Library.Data.Model
{
    internal class ParameterRepresantation : IRepresantation
    {
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public TypeRepresantation Type { get; private set; }

        public ParameterRepresantation(string name, TypeRepresantation type, string methodName)
        {
            Name = name;
            FullName = $"{methodName}.{name}";
            Type = type;
        }

        IEnumerable<string> IRepresantation.Print()
        {
            yield return $"NAME: {Name}";
            yield return $"Type: {Type.Name}";
        }
    }
}