using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace CEGUIPlugin
{
    public class ItemListBase : Window
    {
        public enum SortMode
        {
            Ascending,
            Descending,
            UserSort
        };

        internal ItemListBase(IntPtr itemList)
            :base(itemList)
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public uint getItemCount()
        {
            return ItemListBase_getItemCount(window);
        }

        public ItemEntry getItemFromIndex(uint index)
        {
            return WindowManager.Instance.getWindow(ItemListBase_getItemFromIndex(window, index)) as ItemEntry;
        }

        public uint getItemIndex(ItemEntry item)
        {
            return ItemListBase_getItemIndex(window, item.CEGUIWindow);
        }

        public ItemEntry findItemWithText(String text, ItemEntry start_item)
        {
            return WindowManager.Instance.getWindow(ItemListBase_findItemWithText(window, text, start_item.CEGUIWindow)) as ItemEntry;
        }

        public bool isItemInList(ItemEntry item)
        {
            return ItemListBase_isItemInList(window, item.CEGUIWindow);
        }

        public bool isAutoResizeEnabled()
        {
            return ItemListBase_isAutoResizeEnabled(window);
        }

        public bool isSortEnabled()
        {
            return ItemListBase_isSortEnabled(window);
        }

        public SortMode getSortMode()
        {
            return ItemListBase_getSortMode(window);
        }

        public void initialiseComponents()
        {
            ItemListBase_initialiseComponents(window);
        }

        public void resetList()
        {
            ItemListBase_resetList(window);
        }

        public void addItem(ItemEntry item)
        {
            ItemListBase_addItem(window, item.CEGUIWindow);
        }

        public void insertItem(ItemEntry item, ItemEntry position)
        {
            ItemListBase_insertItem(window, item.CEGUIWindow, position.CEGUIWindow);
        }

        public void removeItem(ItemEntry item)
        {
            ItemListBase_removeItem(window, item.CEGUIWindow);
        }

        public void handleUpdatedItemData()
        {
            ItemListBase_handleUpdatedItemData(window);
        }

        public void handleUpdatedItemData(bool resort)
        {
            ItemListBase_handleUpdatedItemData2(window, resort);
        }

        public void setAutoResizeEnabled(bool setting)
        {
            ItemListBase_setAutoResizeEnabled(window, setting);
        }

        public void sizeToContent()
        {
            ItemListBase_sizeToContent(window);
        }

        public void endInitialisation()
        {
            ItemListBase_endInitialisation(window);
        }

        public Rect getItemRenderArea()
        {
            return ItemListBase_getItemRenderArea(window);
        }

        public Window getContentPane()
        {
            return WindowManager.Instance.getWindow(ItemListBase_getContentPane(window));
        }

        public void notifyItemClicked(ItemEntry item)
        {
            ItemListBase_notifyItemClicked(window, item.CEGUIWindow);
        }

        public void notifyItemSelectState(ItemEntry item, bool select)
        {
            ItemListBase_notifyItemSelectState(window, item.CEGUIWindow, select);
        }

        public void setSortEnabled(bool setting)
        {
            ItemListBase_setSortEnabled(window, setting);
        }

        public void setSortMode(SortMode mode)
        {
            ItemListBase_setSortMode(window, mode);
        }

        public void sortList()
        {
            ItemListBase_sortList(window);
        }

        public void sortList(bool relayout)
        {
            ItemListBase_sortList2(window, relayout);
        }

#region  Events

        const String EventListContentsChanged = "ListItemsChanged";
        const String EventSortEnabledChanged = "SortEnabledChanged";
        const String EventSortModeChanged = "SortModeChanged";

        protected override void disposeEvents()
        {
            if (ListContentsChangedTranslator != null) ListContentsChangedTranslator.Dispose();
            if (SortEnabledChangedTranslator != null) SortEnabledChangedTranslator.Dispose();
            if (SortModeChangedTranslator != null) SortModeChangedTranslator.Dispose();
            base.disposeEvents();
        }

        private CEGUIEventTranslator ListContentsChangedTranslator;

        public event CEGUIEvent ListContentsChanged
        {
            add
            {
                if(ListContentsChangedTranslator == null)
                {
                    ListContentsChangedTranslator = new WindowEventTranslator(EventListContentsChanged, this);
                }
                ListContentsChangedTranslator.BoundEvent += value;
            }
            remove
            {
                ListContentsChangedTranslator.BoundEvent -= value;
            }
        }

        private CEGUIEventTranslator SortEnabledChangedTranslator;

        public event CEGUIEvent SortEnabledChanged
        {
            add
            {
                if(SortEnabledChangedTranslator == null)
                {
                    SortEnabledChangedTranslator = new WindowEventTranslator(EventSortEnabledChanged, this);
                }
                SortEnabledChangedTranslator.BoundEvent += value;
            }
            remove
            {
                SortEnabledChangedTranslator.BoundEvent -= value;
            }
        }

        private CEGUIEventTranslator SortModeChangedTranslator;

        public event CEGUIEvent SortModeChanged
        {
            add
            {
                if(SortModeChangedTranslator == null)
                {
                    SortModeChangedTranslator = new WindowEventTranslator(EventSortModeChanged, this);
                }
                SortModeChangedTranslator.BoundEvent += value;
            }
            remove
            {
                SortModeChangedTranslator.BoundEvent -= value;
            }
        }

#endregion

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern uint ItemListBase_getItemCount(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        private static extern IntPtr ItemListBase_getItemFromIndex(IntPtr itemListBase, uint index);
        
        [DllImport("CEGUIWrapper")]
        private static extern uint ItemListBase_getItemIndex(IntPtr itemListBase, IntPtr item);
        
        [DllImport("CEGUIWrapper")]
        private static extern IntPtr ItemListBase_findItemWithText(IntPtr itemListBase, String text, IntPtr start_item);
        
        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ItemListBase_isItemInList(IntPtr itemListBase, IntPtr item);
        
        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ItemListBase_isAutoResizeEnabled(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ItemListBase_isSortEnabled(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        private static extern SortMode ItemListBase_getSortMode(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_initialiseComponents(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_resetList(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_addItem(IntPtr itemListBase, IntPtr item);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_insertItem(IntPtr itemListBase, IntPtr item, IntPtr position);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_removeItem(IntPtr itemListBase, IntPtr item);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_handleUpdatedItemData(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_handleUpdatedItemData2(IntPtr itemListBase, bool resort);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_setAutoResizeEnabled(IntPtr itemListBase, bool setting);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_sizeToContent(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_endInitialisation(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_performChildWindowLayout(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        private static extern Rect ItemListBase_getItemRenderArea(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        private static extern IntPtr ItemListBase_getContentPane(IntPtr itemListBase);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_notifyItemClicked(IntPtr itemListBase, IntPtr item);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_notifyItemSelectState(IntPtr itemListBase, IntPtr item, bool select);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_setSortEnabled(IntPtr itemListBase, bool setting);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_setSortMode(IntPtr itemListBase, SortMode mode);
        
        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_sortList(IntPtr itemListBase);

        [DllImport("CEGUIWrapper")]
        private static extern void ItemListBase_sortList2(IntPtr itemListBase, bool relayout);

#endregion
    }
}
