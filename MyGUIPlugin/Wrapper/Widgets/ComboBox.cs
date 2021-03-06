﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class ComboBox : EditBox
    {
        public const uint Invalid = uint.MaxValue;

        private List<Object> objectsList = new List<object>();

        public ComboBox(IntPtr comboBox)
            : base(comboBox)
        {

        }

        public void insertItemAt(uint index, String name)
        {
            insertItemAt(index, name, null);
        }

        public void insertItemAt(uint index, String name, Object data)
        {
            ComboBox_insertItemAt(widget, new UIntPtr(index), name);
            objectsList.Insert((int)index, data);
        }

        public void addItem(String name)
        {
            addItem(name, null);
        }

        public void addItem(String name, Object data)
        {
            ComboBox_addItem(widget, name);
            objectsList.Add(data);
        }

        public void removeItemAt(uint index)
        {
            ComboBox_removeItemAt(widget, new UIntPtr(index));
            objectsList.RemoveAt((int)index);
        }

        public void removeAllItems()
        {
            ComboBox_removeAllItems(widget);
            objectsList.Clear();
        }

        public uint findItemIndexWith(String name)
        {
            return ComboBox_findItemIndexWith(widget, name).horriblyUnsafeToUInt32();
        }

        public uint findItemIndexWithData(Object data)
        {
            int dataIndex = objectsList.IndexOf(data);
            if (dataIndex == -1)
            {
                return Invalid;
            }
            return (uint)dataIndex;
        }

        public void clearIndexSelected()
        {
            ComboBox_clearIndexSelected(widget);
        }

        public void setItemDataAt(uint index, Object data)
        {
            objectsList[(int)index] = data;
        }

        public void clearItemDataAt(uint index)
        {
            objectsList[(int)index] = null;
        }

        public Object getItemDataAt(uint index)
        {
            return objectsList[(int)index];
        }

        public void setItemNameAt(uint index, String name)
        {
            ComboBox_setItemNameAt(widget, new UIntPtr(index), name);
        }

        public String getItemNameAt(uint index)
        {
            return Marshal.PtrToStringUni(ComboBox_getItemNameAt(widget, new UIntPtr(index)));
        }

        public void beginToItemAt(uint index)
        {
            ComboBox_beginToItemAt(widget, new UIntPtr(index));
        }

        public void beginToItemFirst()
        {
            ComboBox_beginToItemFirst(widget);
        }

        public void beginToItemLast()
        {
            ComboBox_beginToItemLast(widget);
        }

        public void beginToItemSelected()
        {
            ComboBox_beginToItemSelected(widget);
        }

        public uint ItemCount
        {
            get
            {
                return ComboBox_getItemCount(widget).horriblyUnsafeToUInt32();
            }
        }

        public uint SelectedIndex
        {
            get
            {
                return ComboBox_getIndexSelected(widget).horriblyUnsafeToUInt32();
            }
            set
            {
                ComboBox_setIndexSelected(widget, new UIntPtr(value));
            }
        }

        public bool ComboModeDrop
        {
            get
            {
                return ComboBox_getComboModeDrop(widget);
            }
            set
            {
                ComboBox_setComboModeDrop(widget, value);
            }
        }

        public bool SmoothShow
        {
            get
            {
                return ComboBox_getSmoothShow(widget);
            }
            set
            {
                ComboBox_setSmoothShow(widget, value);
            }
        }

        public int MaxListLength
        {
            get
            {
                return ComboBox_getMaxListLength(widget);
            }
            set
            {
                ComboBox_setMaxListLength(widget, value);
            }
        }

        public String SelectedItemName
        {
            get
            {
                uint selectedIndex = SelectedIndex;
                return selectedIndex == uint.MaxValue ? null : getItemNameAt(selectedIndex);
            }
        }

        public Object SelectedItemData
        {
            get
            {
                uint selectedIndex = SelectedIndex;
                return selectedIndex == uint.MaxValue ? null : getItemDataAt(selectedIndex);
            }
        }

        #region Events

        public event MyGUIEvent EventComboAccept
        {
            add
            {
                eventManager.addDelegate<EventComboAcceptTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventComboAcceptTranslator>(value);
            }
        }

        public event MyGUIEvent EventComboChangePosition
        {
            add
            {
                eventManager.addDelegate<EventComboChangePositionTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventComboChangePositionTranslator>(value);
            }
        }

        #endregion

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr ComboBox_getItemCount(IntPtr comboBox);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_insertItemAt(IntPtr comboBox, UIntPtr index, String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_addItem(IntPtr comboBox, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_removeItemAt(IntPtr comboBox, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_removeAllItems(IntPtr comboBox);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr ComboBox_findItemIndexWith(IntPtr comboBox, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr ComboBox_getIndexSelected(IntPtr comboBox);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_setIndexSelected(IntPtr comboBox, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_clearIndexSelected(IntPtr comboBox);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_setItemNameAt(IntPtr comboBox, UIntPtr index, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ComboBox_getItemNameAt(IntPtr comboBox, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_beginToItemAt(IntPtr comboBox, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_beginToItemFirst(IntPtr comboBox);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_beginToItemLast(IntPtr comboBox);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_beginToItemSelected(IntPtr comboBox);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_setComboModeDrop(IntPtr comboBox, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ComboBox_getComboModeDrop(IntPtr comboBox);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_setSmoothShow(IntPtr comboBox, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ComboBox_getSmoothShow(IntPtr comboBox);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ComboBox_setMaxListLength(IntPtr comboBox, int value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern int ComboBox_getMaxListLength(IntPtr comboBox);

        #endregion
    }
}
