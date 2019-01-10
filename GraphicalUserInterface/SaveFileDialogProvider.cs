using Library.Logic.ViewModel;
using Ookii.Dialogs.Wpf;

namespace GraphicalUserInterface
{
    internal class SaveFileDialogProvider : ISourceProvider
    {
        private readonly VistaSaveFileDialog dialog = new VistaSaveFileDialog();

        internal SaveFileDialogProvider()
        {
            dialog.DefaultExt = "xml";
            dialog.Filter = "XML file (*.xml)|*.xml|All Files(*.*)|*.*";
        }

        public bool GetAccess()
        {
            return dialog.ShowDialog() == true;
        }

        public string GetFilePath()
        {
            return dialog.FileName;
        }
    }
}