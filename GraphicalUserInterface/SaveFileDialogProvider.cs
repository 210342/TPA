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
        //Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
        System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
        internal SaveFileDialogProvider()
        {
            dialog.DefaultExt = "xml";
            dialog.Filter = "Dynamically linked library (*xml)|*xml|All Files(*.*)|*.*";
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
