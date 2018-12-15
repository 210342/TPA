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
        Ookii.Dialogs.Wpf.VistaSaveFileDialog dialog = new Ookii.Dialogs.Wpf.VistaSaveFileDialog();
        internal SaveFileDialogProvider()
        {
            dialog.DefaultExt = "xml";
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
