using System.Windows;
using Library.Logic.ViewModel;

namespace GraphicalUserInterface
{
    /// <summary>
    ///     Logika interakcji dla klasy MainWindow.xaml
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
            ((ViewModel) DataContext).OpenFileSourceProvider = new OpenFileDialogProvider();
            ((ViewModel) DataContext).SaveFileSourceProvider = new SaveFileDialogProvider();
            ((ViewModel) DataContext).ErrorMessageTarget = new ErrorMessageBox();
            ((ViewModel) DataContext).InformationMessageTarget = new InformationMessageBox();
            ((ViewModel) DataContext).EndInit();
        }
    }
}