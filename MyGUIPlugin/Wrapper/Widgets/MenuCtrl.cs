using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class MenuCtrl : Widget
    {
        public MenuCtrl(IntPtr menuCtrl)
            :base(menuCtrl)
        {
            
        }

        public void setVisibleSmooth(bool value)
        {
            MenuCtrl_setVisibleSmooth(widget, value);
        }

        public uint getItemCount()
        {
            return MenuCtrl_getItemCount(widget).ToUInt32();
        }

        public MenuItem insertItemAt(uint index, String name)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_insertItemAt(widget, new UIntPtr(index), name));
        }

        public MenuItem insertItemAt(uint index, String name, MenuItemType type)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_insertItemAt2(widget, new UIntPtr(index), name, type));
        }

        public MenuItem insertItemAt(uint index, String name, MenuItemType type, String id)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_insertItemAt3(widget, new UIntPtr(index), name, type, id));
        }

        public MenuItem insertItem(MenuItem to, String name)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_insertItem(widget, to.WidgetPtr, name));
        }

        public MenuItem insertItem(MenuItem to, String name, MenuItemType type)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_insertItem2(widget, to.WidgetPtr, name, type));
        }

        public MenuItem insertItem(MenuItem to, String name, MenuItemType type, String id)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_insertItem3(widget, to.WidgetPtr, name, type, id));
        }

        public MenuItem addItem(String name)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_addItem(widget, name));
        }

        public MenuItem addItem(String name, MenuItemType type)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_addItem2(widget, name, type));
        }

        public MenuItem addItem(String name, MenuItemType type, String id)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_addItem3(widget, name, type, id));
        }

        public void removeItemAt(uint index)
        {
            MenuCtrl_removeItemAt(widget, new UIntPtr(index));
        }

        public void removeItem(MenuItem item)
        {
            MenuCtrl_removeItem(widget, item.WidgetPtr);
        }

        public void removeAllItems()
        {
            MenuCtrl_removeAllItems(widget);
        }

        public MenuItem getItemAt(uint index)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_getItemAt(widget, new UIntPtr(index)));
        }

        public uint getItemIndex(MenuItem item)
        {
            return MenuCtrl_getItemIndex(widget, item.WidgetPtr).ToUInt32();
        }

        public uint findItemIndex(MenuItem item)
        {
            return MenuCtrl_findItemIndex(widget, item.WidgetPtr).ToUInt32();
        }

        public MenuItem findItemWith(String name)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_findItemWith(widget, name));
        }

        public void setItemIdAt(uint index, String id)
        {
            MenuCtrl_setItemIdAt(widget, new UIntPtr(index), id);
        }

        public void setItemId(MenuItem item, String id)
        {
            MenuCtrl_setItemId(widget, item.WidgetPtr, id);
        }

        public String getItemIdAt(uint index)
        {
            return Marshal.PtrToStringAnsi(MenuCtrl_getItemIdAt(widget, new UIntPtr(index)));
        }

        public String getItemId(MenuItem item)
        {
            return Marshal.PtrToStringAnsi(MenuCtrl_getItemId(widget, item.WidgetPtr));
        }

        public MenuItem getItemById(String id)
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_getItemById(widget, id));
        }

        public uint getItemIndexById(String id)
        {
            return MenuCtrl_getItemIndexById(widget, id).ToUInt32();
        }

        public void setItemNameAt(uint index, String name)
        {
            MenuCtrl_setItemNameAt(widget, new UIntPtr(index), name);
        }

        public void setItemName(MenuItem item, String name)
        {
            MenuCtrl_setItemName(widget, item.WidgetPtr, name);
        }

        public String getItemNameAt(uint index)
        {
            return Marshal.PtrToStringUni(MenuCtrl_getItemNameAt(widget, new UIntPtr(index)));
        }

        public String getItemName(MenuItem item)
        {
            return Marshal.PtrToStringUni(MenuCtrl_getItemName(widget, item.WidgetPtr));
        }

        public uint findItemIndexWith(String name)
        {
            return MenuCtrl_findItemIndexWith(widget, name).ToUInt32();
        }

        public void setItemChildVisibleAt(uint index, bool visible)
        {
            MenuCtrl_setItemChildVisibleAt(widget, new UIntPtr(index), visible);
        }

        public void setItemChildVisible(MenuItem item, bool visible)
        {
            MenuCtrl_setItemChildVisible(widget, item.WidgetPtr, visible);
        }

        public MenuCtrl getItemChildAt(uint index)
        {
            return (MenuCtrl)WidgetManager.getWidget(MenuCtrl_getItemChildAt(widget, new UIntPtr(index)));
        }

        public MenuCtrl getItemChild(MenuItem item)
        {
            return (MenuCtrl)WidgetManager.getWidget(MenuCtrl_getItemChild(widget, item.WidgetPtr));
        }

        public MenuCtrl createItemChildAt(uint index)
        {
            return (MenuCtrl)WidgetManager.getWidget(MenuCtrl_createItemChildAt(widget, new UIntPtr(index)));
        }

        public MenuCtrl createItemChild(MenuItem item)
        {
            return (MenuCtrl)WidgetManager.getWidget(MenuCtrl_createItemChild(widget, item.WidgetPtr));
        }

        public void removeItemChildAt(uint index)
        {
            MenuCtrl_removeItemChildAt(widget, new UIntPtr(index));
        }

        public void removeItemChild(MenuItem item)
        {
            MenuCtrl_removeItemChild(widget, item.WidgetPtr);
        }

        public MenuItemType getItemTypeAt(uint index)
        {
            return MenuCtrl_getItemTypeAt(widget, new UIntPtr(index));
        }

        public MenuItemType getItemType(MenuItem item)
        {
            return MenuCtrl_getItemType(widget, item.WidgetPtr);
        }

        public void setItemTypeAt(uint index, MenuItemType type)
        {
            MenuCtrl_setItemTypeAt(widget, new UIntPtr(index), type);
        }

        public void setItemType(MenuItem item, MenuItemType type)
        {
            MenuCtrl_setItemType(widget, item.WidgetPtr, type);
        }

        public MenuItem getMenuItemParent()
        {
            return (MenuItem)WidgetManager.getWidget(MenuCtrl_getMenuItemParent(widget));
        }

        public bool PopupAccept
        {
            get
            {
                return MenuCtrl_getPopupAccept(widget);
            }
            set
            {
                MenuCtrl_setPopupAccept(widget, value);
            }
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_setVisibleSmooth(IntPtr widget, bool value);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr MenuCtrl_getItemCount(IntPtr menuCtrl);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_insertItemAt(IntPtr menuCtrl, UIntPtr index, String name);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_insertItemAt2(IntPtr menuCtrl, UIntPtr index, String name, MenuItemType type);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_insertItemAt3(IntPtr menuCtrl, UIntPtr index, String name, MenuItemType type, String id);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_insertItem(IntPtr menuCtrl, IntPtr to, String name);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_insertItem2(IntPtr menuCtrl, IntPtr to, String name, MenuItemType type);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_insertItem3(IntPtr menuCtrl, IntPtr to, String name, MenuItemType type, String id);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_addItem(IntPtr menuCtrl, String name);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_addItem2(IntPtr menuCtrl, String name, MenuItemType type);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_addItem3(IntPtr menuCtrl, String name, MenuItemType type, String id);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_removeItemAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_removeItem(IntPtr menuCtrl, IntPtr item);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_removeAllItems(IntPtr menuCtrl);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_getItemAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr MenuCtrl_getItemIndex(IntPtr menuCtrl, IntPtr item);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr MenuCtrl_findItemIndex(IntPtr menuCtrl, IntPtr item);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_findItemWith(IntPtr menuCtrl, String name);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_setItemIdAt(IntPtr menuCtrl, UIntPtr index, String id);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_setItemId(IntPtr menuCtrl, IntPtr item, String id);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_getItemIdAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_getItemId(IntPtr menuCtrl, IntPtr item);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_getItemById(IntPtr menuCtrl, String id);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr MenuCtrl_getItemIndexById(IntPtr menuCtrl, String id);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_setItemNameAt(IntPtr menuCtrl, UIntPtr index, String name);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_setItemName(IntPtr menuCtrl, IntPtr item, String name);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_getItemNameAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_getItemName(IntPtr menuCtrl, IntPtr item);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr MenuCtrl_findItemIndexWith(IntPtr menuCtrl, String name);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_setItemChildVisibleAt(IntPtr menuCtrl, UIntPtr index, bool visible);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_setItemChildVisible(IntPtr menuCtrl, IntPtr item, bool visible);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_getItemChildAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_getItemChild(IntPtr menuCtrl, IntPtr item);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_createItemChildAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_createItemChild(IntPtr menuCtrl, IntPtr item);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_removeItemChildAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_removeItemChild(IntPtr menuCtrl, IntPtr item);

        [DllImport("MyGUIWrapper")]
        private static extern MenuItemType MenuCtrl_getItemTypeAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern MenuItemType MenuCtrl_getItemType(IntPtr menuCtrl, IntPtr item);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_setItemTypeAt(IntPtr menuCtrl, UIntPtr index, MenuItemType type);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_setItemType(IntPtr menuCtrl, IntPtr item, MenuItemType type);

        [DllImport("MyGUIWrapper")]
        private static extern void MenuCtrl_setPopupAccept(IntPtr menuCtrl, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MenuCtrl_getPopupAccept(IntPtr menuCtrl);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MenuCtrl_getMenuItemParent(IntPtr menuCtrl);

#endregion
    }
}
