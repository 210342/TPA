using Library.Logic.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicalUserInterface
{
    internal class SaveFileDialogProvider : ISourceProvider
    {
        Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
        internal SaveFileDialogProvider()
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
