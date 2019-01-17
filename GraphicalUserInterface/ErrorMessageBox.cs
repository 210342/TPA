using System.Windows;
using Library.Logic.ViewModel;

namespace GraphicalUserInterface
{
    public class ErrorMessageBox : Window, IErrorFlushTarget
    {
        public void SendMessage(string title, string message)
        {
            MessageBox.Show(this, message, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel);
        }

        public void CloseApp()
        {
            Application.Current?.Shutdown();
        }
    }
}