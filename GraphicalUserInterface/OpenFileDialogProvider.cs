using Library.Logic.ViewModel;
using System;

namespace GraphicalUserInterface
{
    internal class OpenFileDialogProvider : ISourceProvider
    {
        Ookii.Dialogs.Wpf.VistaOpenFileDialog dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
        internal OpenFileDialogProvider()
        {
            dialog.DefaultExt = "dll";
            dialog.Filter = "XML file (*xml)|*xml|Dynamically linked library (*dll)|*dll|All Files(*.*)|*.*";
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
