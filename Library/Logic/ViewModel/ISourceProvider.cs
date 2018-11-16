namespace Library.Logic.ViewModel
{
    public interface ISourceProvider
    {
        bool GetAccess();
        string GetFilePath();
    }
}
