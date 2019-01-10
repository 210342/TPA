namespace Library.Logic.ViewModel
{
    public interface IErrorMessageBox
    {
        void ShowMessage(string title, string message);
        void CloseApp();
    }
}