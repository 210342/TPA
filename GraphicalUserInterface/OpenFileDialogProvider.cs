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
            dialog.Filter = "Dynamically linked library (*.dll)|*.dll|XML file (*.xml)|*.xml|All Files(*.*)|*.*";
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
