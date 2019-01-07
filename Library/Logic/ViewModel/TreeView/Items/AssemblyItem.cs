﻿using Library.Model;
using ModelContract;
using System.Linq;

namespace Library.Logic.ViewModel
{
    public class AssemblyItem : TreeViewItem
    {
        private readonly string _details;
        public override string Details {
            get
            {
                return _details;
            }
        }

        public AssemblyItem(AssemblyMetadata source) : base(source)
        {
            _details =  $"Assembly name: {source.Name}, " +
                $"has " + (source.Children == null ? "0" : source.Children.Count().ToString()) + " namespaces.";
        }

        protected override TreeViewItem GetChildOfType(IMetadata metadata)
        {
            switch (metadata)
            {
                case NamespaceMetadata namesp:
                    return new NamespaceItem(namesp);
            }
            return null;
        }
    }
}