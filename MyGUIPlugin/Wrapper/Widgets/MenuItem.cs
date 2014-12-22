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
            return MenuItem_getItemIndex(widget).horriblyUnsafeToUInt32();
        }

        public MenuControl createItemChild()
        {
            return (MenuControl)WidgetManager.getWidget(MenuItem_createItemChild(widget));
        }

        public void setItemChildVisible(bool value)
        {
            MenuItem_setItemChildVisible(widget, value);
        }

        public MenuControl getMenuCtrlParent()
        {
            return (MenuControl)WidgetManager.getWidget(MenuItem_getMenuCtrlParent(widget));
        }

        public MenuControl getItemChild()
        {
            return (MenuControl)WidgetManager.getWidget(MenuItem_getItemChild(widget));
        }

#region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuItem_setItemName(IntPtr menuItem, [MarshalAs(UnmanagedType.LPWStr)] String value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuItem_getItemName(IntPtr menuItem);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuItem_removeItem(IntPtr menuItem);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuItem_setItemId(IntPtr menuItem, String value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuItem_getItemId(IntPtr menuItem);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr MenuItem_getItemIndex(IntPtr menuItem);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuItem_createItemChild(IntPtr menuItem);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuItem_setItemType(IntPtr menuItem, MenuItemType value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern MenuItemType MenuItem_getItemType(IntPtr menuItem);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuItem_setItemChildVisible(IntPtr menuItem, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuItem_getMenuCtrlParent(IntPtr menuItem);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuItem_getItemChild(IntPtr menuItem);

#endregion

    }
}
