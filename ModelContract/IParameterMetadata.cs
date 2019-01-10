namespace ModelContract
{
    public interface IParameterMetadata : IMetadata
    {
        ITypeMetadata TypeMetadata { get; }

        void MapTypes();
    }
}