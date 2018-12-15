﻿using Library.Model;

namespace Library.Logic.ViewModel
{
    public class AttributeItem : TreeViewItem
    {
        public AttributeItem(AttributeMetadata source) : base(source)
        {

        }

        protected override TreeViewItem GetChildOfType(IMetadata metadata)
        {
            switch (metadata)
            {
                case AttributeMetadata attribute:
                    return new AttributeItem(attribute);
                case ParameterMetadata param:
                    return new ParameterItem(param);
                case TypeMetadata type:
                    return new TypeItem(type);
            }
            return null;
        }
    }
}