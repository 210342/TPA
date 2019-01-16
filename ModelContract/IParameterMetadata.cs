namespace ModelContract
{
    public interface IParameterMetadata : IMetadata
    {
        ITypeMetadata MyType { get; }

        void MapTypes();
    }
}