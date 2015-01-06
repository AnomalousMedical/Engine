using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class MenuControl : Widget
    {
        bool autoAcceptRunAction = false;

        public MenuControl(IntPtr menuCtrl)
            :base(menuCtrl)
        {
            
        }

        public void setVisibleSmooth(bool value)
        {
            MenuControl_setVisibleSmooth(widget, value);
        }

        public uint getItemCount()
        {
            return MenuControl_getItemCount(widget).horriblyUnsafeToUInt32();
        }

        public MenuItem insertItemAt(uint index, String name)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_insertItemAt(widget, new UIntPtr(index), name));
        }

        public MenuItem insertItemAt(uint index, String name, MenuItemType type)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_insertItemAt2(widget, new UIntPtr(index), name, type));
        }

        public MenuItem insertItemAt(uint index, String name, MenuItemType type, String id)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_insertItemAt3(widget, new UIntPtr(index), name, type, id));
        }

        public MenuItem insertItem(MenuItem to, String name)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_insertItem(widget, to.WidgetPtr, name));
        }

        public MenuItem insertItem(MenuItem to, String name, MenuItemType type)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_insertItem2(widget, to.WidgetPtr, name, type));
        }

        public MenuItem insertItem(MenuItem to, String name, MenuItemType type, String id)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_insertItem3(widget, to.WidgetPtr, name, type, id));
        }

        public MenuItem addItem(String name)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_addItem(widget, name));
        }

        public MenuItem addItem(String name, MenuItemType type)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_addItem2(widget, name, type));
        }

        public MenuItem addItem(String name, MenuItemType type, String id)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_addItem3(widget, name, type, id));
        }

        /// <summary>
        /// Add an item with a specific action to execute. Note that this consumes the UserObject
        /// for the item added. This can be used with AutoAcceptRunAction set to true to make the
        /// menu automatically run the actions it is given.
        /// </summary>
        /// <param name="name">The name of the item, this will appear in the menu.</param>
        /// <param name="action">The Action to run when clicked if AutoAcceptRunAction is true. This will consume the UserObject for the menu item.</param>
        /// <returns>A new MenuItem.</returns>
        public MenuItem addItemAction(String name, Action action)
        {
            MenuItem item = addItem(name);
            item.UserObject = action;
            return item;
        }

        /// <summary>
        /// Add an item with a specific action to execute. Note that this consumes the UserObject
        /// for the item added. This can be used with AutoAcceptRunAction set to true to make the
        /// menu automatically run the actions it is given.
        /// </summary>
        /// <param name="name">The name of the item, this will appear in the menu.</param>
        /// <param name="type">The type of item to add.</param>
        /// <param name="action">The Action to run when clicked if AutoAcceptRunAction is true. This will consume the UserObject for the menu item.</param>
        /// <returns>A new MenuItem.</returns>
        public MenuItem addItemAction(String name, MenuItemType type, Action action)
        {
            MenuItem item = addItem(name, type);
            item.UserObject = action;
            return item;
        }

        /// <summary>
        /// Add an item with a specific action to execute. Note that this consumes the UserObject
        /// for the item added. This can be used with AutoAcceptRunAction set to true to make the
        /// menu automatically run the actions it is given.
        /// </summary>
        /// <param name="name">The name of the item, this will appear in the menu.</param>
        /// <param name="type">The type of item to add.</param>
        /// <param name="id">The id of the item.</param>
        /// <param name="action">The Action to run when clicked if AutoAcceptRunAction is true. This will consume the UserObject for the menu item.</param>
        /// <returns>A new MenuItem.</returns>
        public MenuItem addItemAction(String name, MenuItemType type, String id, Action action)
        {
            MenuItem item = addItem(name, type, id);
            item.UserObject = action;
            return item;
        }

        public void removeItemAt(uint index)
        {
            MenuControl_removeItemAt(widget, new UIntPtr(index));
        }

        public void removeItem(MenuItem item)
        {
            MenuControl_removeItem(widget, item.WidgetPtr);
        }

        public void removeAllItems()
        {
            MenuControl_removeAllItems(widget);
        }

        public MenuItem getItemAt(uint index)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_getItemAt(widget, new UIntPtr(index)));
        }

        public uint getItemIndex(MenuItem item)
        {
            return MenuControl_getItemIndex(widget, item.WidgetPtr).horriblyUnsafeToUInt32();
        }

        public uint findItemIndex(MenuItem item)
        {
            return MenuControl_findItemIndex(widget, item.WidgetPtr).horriblyUnsafeToUInt32();
        }

        public MenuItem findItemWith(String name)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_findItemWith(widget, name));
        }

        public void setItemIdAt(uint index, String id)
        {
            MenuControl_setItemIdAt(widget, new UIntPtr(index), id);
        }

        public void setItemId(MenuItem item, String id)
        {
            MenuControl_setItemId(widget, item.WidgetPtr, id);
        }

        public String getItemIdAt(uint index)
        {
            return Marshal.PtrToStringAnsi(MenuControl_getItemIdAt(widget, new UIntPtr(index)));
        }

        public String getItemId(MenuItem item)
        {
            return Marshal.PtrToStringAnsi(MenuControl_getItemId(widget, item.WidgetPtr));
        }

        public MenuItem getItemById(String id)
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_getItemById(widget, id));
        }

        public uint getItemIndexById(String id)
        {
            return MenuControl_getItemIndexById(widget, id).horriblyUnsafeToUInt32();
        }

        public void setItemNameAt(uint index, String name)
        {
            MenuControl_setItemNameAt(widget, new UIntPtr(index), name);
        }

        public void setItemName(MenuItem item, String name)
        {
            MenuControl_setItemName(widget, item.WidgetPtr, name);
        }

        public String getItemNameAt(uint index)
        {
            return Marshal.PtrToStringUni(MenuControl_getItemNameAt(widget, new UIntPtr(index)));
        }

        public String getItemName(MenuItem item)
        {
            return Marshal.PtrToStringUni(MenuControl_getItemName(widget, item.WidgetPtr));
        }

        public uint findItemIndexWith(String name)
        {
            return MenuControl_findItemIndexWith(widget, name).horriblyUnsafeToUInt32();
        }

        public void setItemChildVisibleAt(uint index, bool visible)
        {
            MenuControl_setItemChildVisibleAt(widget, new UIntPtr(index), visible);
        }

        public void setItemChildVisible(MenuItem item, bool visible)
        {
            MenuControl_setItemChildVisible(widget, item.WidgetPtr, visible);
        }

        public MenuControl getItemChildAt(uint index)
        {
            return (MenuControl)WidgetManager.getWidget(MenuControl_getItemChildAt(widget, new UIntPtr(index)));
        }

        public MenuControl getItemChild(MenuItem item)
        {
            return (MenuControl)WidgetManager.getWidget(MenuControl_getItemChild(widget, item.WidgetPtr));
        }

        public MenuControl createItemChildAt(uint index)
        {
            return (MenuControl)WidgetManager.getWidget(MenuControl_createItemChildAt(widget, new UIntPtr(index)));
        }

        public MenuControl createItemChild(MenuItem item)
        {
            return (MenuControl)WidgetManager.getWidget(MenuControl_createItemChild(widget, item.WidgetPtr));
        }

        public PopupMenu createItemPopupMenuChild(MenuItem item)
        {
            return (PopupMenu)WidgetManager.getWidget(MenuControl_createItemPopupMenuChild(widget, item.WidgetPtr));
        }

        public PopupMenu createItemPopupMenuChild(uint index)
        {
            return (PopupMenu)WidgetManager.getWidget(MenuControl_createItemPopupMenuChildAt(widget, new UIntPtr(index)));
        }

        public void removeItemChildAt(uint index)
        {
            MenuControl_removeItemChildAt(widget, new UIntPtr(index));
        }

        public void removeItemChild(MenuItem item)
        {
            MenuControl_removeItemChild(widget, item.WidgetPtr);
        }

        public MenuItemType getItemTypeAt(uint index)
        {
            return MenuControl_getItemTypeAt(widget, new UIntPtr(index));
        }

        public MenuItemType getItemType(MenuItem item)
        {
            return MenuControl_getItemType(widget, item.WidgetPtr);
        }

        public void setItemTypeAt(uint index, MenuItemType type)
        {
            MenuControl_setItemTypeAt(widget, new UIntPtr(index), type);
        }

        public void setItemType(MenuItem item, MenuItemType type)
        {
            MenuControl_setItemType(widget, item.WidgetPtr, type);
        }

        public MenuItem getMenuItemParent()
        {
            return (MenuItem)WidgetManager.getWidget(MenuControl_getMenuItemParent(widget));
        }

        public bool PopupAccept
        {
            get
            {
                return MenuControl_getPopupAccept(widget);
            }
            set
            {
                MenuControl_setPopupAccept(widget, value);
            }
        }

        public event MyGUIEvent ItemAccept
        {
            add
            {
                eventManager.addDelegate<EventMenuCtrlAcceptTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventMenuCtrlAcceptTranslator>(value);
            }
        }

        public event MyGUIEvent Closed
        {
            add
            {
                eventManager.addDelegate<EventMenuCtrlCloseTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventMenuCtrlCloseTranslator>(value);
            }
        }

        /// <summary>
        /// Set this to true to automatically run the UserObject of all items
        /// added to any menu or submenu's UserObject as an action, use this
        /// with the addItemAction functions.
        /// </summary>
        public bool AutoAcceptRunAction
        {
            get
            {
                return autoAcceptRunAction;
            }
            set
            {
                if (value != autoAcceptRunAction)
                {
                    autoAcceptRunAction = value;
                    if (value)
                    {
                        this.ItemAccept += MenuControl_ItemAccept;
                    }
                    else
                    {
                        this.ItemAccept -= MenuControl_ItemAccept;
                    }
                }
            }
        }

        void MenuControl_ItemAccept(Widget source, EventArgs e)
        {
            MenuCtrlAcceptEventArgs mcae = e as MenuCtrlAcceptEventArgs;
            Action action = mcae.Item.UserObject as Action;
            if (action != null)
            {
                action.Invoke();
            }
        }

#region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_setVisibleSmooth(IntPtr widget, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr MenuControl_getItemCount(IntPtr menuCtrl);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_insertItemAt(IntPtr menuCtrl, UIntPtr index, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_insertItemAt2(IntPtr menuCtrl, UIntPtr index, [MarshalAs(UnmanagedType.LPWStr)] String name, MenuItemType type);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_insertItemAt3(IntPtr menuCtrl, UIntPtr index, [MarshalAs(UnmanagedType.LPWStr)] String name, MenuItemType type, String id);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_insertItem(IntPtr menuCtrl, IntPtr to, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_insertItem2(IntPtr menuCtrl, IntPtr to, [MarshalAs(UnmanagedType.LPWStr)] String name, MenuItemType type);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_insertItem3(IntPtr menuCtrl, IntPtr to, [MarshalAs(UnmanagedType.LPWStr)] String name, MenuItemType type, String id);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_addItem(IntPtr menuCtrl, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_addItem2(IntPtr menuCtrl, [MarshalAs(UnmanagedType.LPWStr)] String name, MenuItemType type);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_addItem3(IntPtr menuCtrl, [MarshalAs(UnmanagedType.LPWStr)] String name, MenuItemType type, String id);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_removeItemAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_removeItem(IntPtr menuCtrl, IntPtr item);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_removeAllItems(IntPtr menuCtrl);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_getItemAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr MenuControl_getItemIndex(IntPtr menuCtrl, IntPtr item);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr MenuControl_findItemIndex(IntPtr menuCtrl, IntPtr item);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_findItemWith(IntPtr menuCtrl, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_setItemIdAt(IntPtr menuCtrl, UIntPtr index, String id);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_setItemId(IntPtr menuCtrl, IntPtr item, String id);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_getItemIdAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_getItemId(IntPtr menuCtrl, IntPtr item);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_getItemById(IntPtr menuCtrl, String id);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr MenuControl_getItemIndexById(IntPtr menuCtrl, String id);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_setItemNameAt(IntPtr menuCtrl, UIntPtr index, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_setItemName(IntPtr menuCtrl, IntPtr item, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_getItemNameAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_getItemName(IntPtr menuCtrl, IntPtr item);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr MenuControl_findItemIndexWith(IntPtr menuCtrl, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_setItemChildVisibleAt(IntPtr menuCtrl, UIntPtr index, bool visible);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_setItemChildVisible(IntPtr menuCtrl, IntPtr item, bool visible);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_getItemChildAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_getItemChild(IntPtr menuCtrl, IntPtr item);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_createItemChildAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_createItemChild(IntPtr menuCtrl, IntPtr item);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_createItemPopupMenuChildAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_createItemPopupMenuChild(IntPtr menuCtrl, IntPtr item);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_removeItemChildAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_removeItemChild(IntPtr menuCtrl, IntPtr item);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern MenuItemType MenuControl_getItemTypeAt(IntPtr menuCtrl, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern MenuItemType MenuControl_getItemType(IntPtr menuCtrl, IntPtr item);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_setItemTypeAt(IntPtr menuCtrl, UIntPtr index, MenuItemType type);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_setItemType(IntPtr menuCtrl, IntPtr item, MenuItemType type);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MenuControl_setPopupAccept(IntPtr menuCtrl, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MenuControl_getPopupAccept(IntPtr menuCtrl);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MenuControl_getMenuItemParent(IntPtr menuCtrl);

#endregion
    }
}
