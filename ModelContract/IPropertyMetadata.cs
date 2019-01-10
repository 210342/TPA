namespace ModelContract
{
    public interface IPropertyMetadata : IMetadata
    {
        ITypeMetadata MyType { get; }
    }
}