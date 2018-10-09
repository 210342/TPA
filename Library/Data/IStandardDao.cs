namespace Library.Data
{
    public interface IStandardDao<ElementDataType>
    {
        void Save(ElementDataType elementToSave);
        ElementDataType Read();
    }
}
