using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public enum MenuItemType { Normal, Popup, Separator, MAX }

    public class MenuItem : Button
    {
        public MenuItem(IntPtr menuItem)
            :base(menuItem)
        {

        }

        public String ItemName
        {
            get
            {
                return Marshal.PtrToStringUni(MenuItem_getItemName(widget));
            }
            set
            {
                MenuItem_setItemName(widget, value);
            }
        }

        public String ItemId
        {
            get
            {
                return Marshal.PtrToStringAnsi(MenuItem_getItemId(widget));
            }
            set
            {
                MenuItem_setItemId(widget, value);
            }
        }

        public MenuItemType ItemType
        {
            get
            {
                return MenuItem_getItemType(widget);
            }
            set
            {
                MenuItem_setItemType(widget, value);
            }
        }

        public void removeItem()
        {
            MenuItem_removeItem(widget);
        }

        public uint getItemIndex()
        {
            return MenuItem_getItemIndex(widget).ToUInt32();
        }

        public MenuCtrl createItemChild()
        {
            return (MenuCtrl)WidgetManager.getWidget(MenuItem_createItemChild(widget));
        }

        public void setItemChildVisible(bool value)
        {
            MenuItem_setItemChildVisible(widget, value);
        }

        public MenuCtrl getMenuCtrlParent()
        {
            return (MenuCtrl)WidgetManager.getWidget(MenuItem_getMenuCtrlParent(widget));
        }

        public MenuCtrl getItemChild()
        {
            return (MenuCtrl)WidgetManager.getWidget(MenuItem_getItemChild(widget));
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern void MenuItem_setItemName(IntPtr menuItem, [MarshalAs(UnmanagedType.LPWStr)] String value);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuItem_getItemName(IntPtr menuItem);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuItem_removeItem(IntPtr menuItem);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuItem_setItemId(IntPtr menuItem, String value);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuItem_getItemId(IntPtr menuItem);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr MenuItem_getItemIndex(IntPtr menuItem);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuItem_createItemChild(IntPtr menuItem);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuItem_setItemType(IntPtr menuItem, MenuItemType value);

        [DllImport("MyGUIWrapper")]
        private static extern MenuItemType MenuItem_getItemType(IntPtr menuItem);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuItem_setItemChildVisible(IntPtr menuItem, bool value);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuItem_getMenuCtrlParent(IntPtr menuItem);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuItem_getItemChild(IntPtr menuItem);

#endregion

    }
}
