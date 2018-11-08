using Library.Data.Model;
using System.Collections.Generic;

namespace Library.Logic.TreeView
{
    public static  class DataLoadedDictionary
    {
       /* public static readonly Dictionary<int, TreeViewItem> Items =
            new Dictionary<int, TreeViewItem>();*/

        public static readonly Dictionary<int, IMetadata> Items = 
            new Dictionary<int, IMetadata>();
    }
}
