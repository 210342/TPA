using Library.Logic.ViewModel;
using System.Windows;

namespace GraphicalUserInterface
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public override void EndInit()
        {
            base.EndInit();
            ((ViewModel)DataContext).OpenFileSourceProvider = new OpenFileDialogProvider();
            ((ViewModel)DataContext).SaveFileSourceProvider = new SaveFileDialogProvider();
            ((ViewModel)DataContext).ErrorMessageBox = new ErrorMessageBox();
            ((ViewModel)DataContext).EndInit();
        }
    }
}
