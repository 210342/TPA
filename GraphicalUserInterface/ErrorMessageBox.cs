using System.Windows;
using Library.Logic.ViewModel;

namespace GraphicalUserInterface
{
    public class ErrorMessageBox : Window, IErrorMessageBox
    {
        public void ShowMessage(string title, string message)
        {
            MessageBox.Show(this, message, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel);
        }

        public void CloseApp()
        {
            Application.Current?.Shutdown();
        }
    }
}