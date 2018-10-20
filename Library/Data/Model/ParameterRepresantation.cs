namespace Library.Data.Model
{
    internal class ParameterRepresantation : IRepresantation
    {
        public string Name { get; private set; }
        public TypeRepresantation Type { get; private set; }

        public ParameterRepresantation(string name, TypeRepresantation type)
        {
            Name = name;
            Type = type;
        }

        public string Print()
        {
            throw new System.NotImplementedException();
        }
    }
}