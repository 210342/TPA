using Library.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.GraphicalData.TreeView;

namespace Library.Logic.TreeView
{
    public static  class DataLoadedDictionary
    {
        public static readonly Dictionary<int, TreeViewItem> Items = 
            new Dictionary<int, TreeViewItem>();
    }
}
