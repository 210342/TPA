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
            if (DataContext is ViewModel)
            {
                ((ViewModel)DataContext).OpenFileSourceProvider = new OpenFileDialogProvider();
                ((ViewModel)DataContext).SaveFileSourceProvider = new SaveFileDialogProvider();
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItemHelper.Content = e.NewValue;
        }
    }
}
