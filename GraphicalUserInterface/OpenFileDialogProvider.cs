using Library.Logic.ViewModel;
using Ookii.Dialogs.Wpf;

namespace GraphicalUserInterface
{
    internal class OpenFileDialogProvider : ISourceProvider
    {
        private readonly VistaOpenFileDialog dialog = new VistaOpenFileDialog();

        internal OpenFileDialogProvider()
        {
            dialog.DefaultExt = "dll";
            dialog.Filter = "Dynamically linked library (*.dll)|*.dll|XML file (*.xml)|*.xml|All Files(*.*)|*.*";
        }

        public bool GetAccess()
        {
            return dialog.ShowDialog() == true;
        }

        public string GetPath()
        {
            return dialog.FileName;
        }
    }
}