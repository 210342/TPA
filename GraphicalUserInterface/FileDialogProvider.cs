using Library.Logic.ViewModel;
using System;

namespace GraphicalUserInterface
{
    internal class FileDialogProvider : ISourceProvider
    {
        Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
        internal FileDialogProvider()
        {
            dialog.DefaultExt = "dll";
            dialog.Filter = "Dynamically linked library (*dll)|*dll|All Files(*.*)|*.*";
        }

        public bool GetAccess()
        {
            return dialog.ShowDialog().Value;
        }

        public string GetFilePath()
        {
            return dialog.FileName;
        }
    }
}
