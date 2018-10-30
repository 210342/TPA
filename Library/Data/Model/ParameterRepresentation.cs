using System.Collections.Generic;

namespace Library.Data.Model
{
    internal class ParameterRepresentation : IRepresentation
    {
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public TypeRepresentation Type { get; private set; }
        public IEnumerable<string> Children
        {
            get
            {
                return Print();
            }
        }

        public ParameterRepresentation(string name, TypeRepresentation type, string methodName)
        {
            Name = name;
            FullName = $"{methodName}.{name}";
            Type = type;
        }

        public IEnumerable<string> Print()
        {
            yield return $"NAME: {Name}";
            yield return $"Type: {Type.Name}";
        }
    }
}