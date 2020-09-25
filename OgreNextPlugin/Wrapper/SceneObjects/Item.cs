using Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OgreNextPlugin
{
    [NativeSubsystemType]
    public class Item : MovableObject
    {
        internal static Item createWrapper(IntPtr item, object[] args)
        {
            return new Item(item);
        }

        private Item(IntPtr item)
            : base(item)
        {

        }
    }
}
