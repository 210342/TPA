using Library.Logic.ViewModel;
using System;

namespace GraphicalUserInterface
{
    internal class OpenFileDialogProvider : ISourceProvider
    {
        //Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
        System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
        internal OpenFileDialogProvider()
        {
            dialog.DefaultExt = "dll";
            dialog.Filter = "Dynamically linked library (*dll)|*dll|All Files(*.*)|*.*";
        }

        public bool GetAccess()
        {
            return dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public string GetFilePath()
        {
            return dialog.FileName;
        }
    }
}
