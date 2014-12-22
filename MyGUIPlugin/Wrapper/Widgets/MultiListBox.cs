using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class MultiListBox : Widget
    {
        private List<Object> columnData = new List<Object>();
        private List<Object> rowData = new List<Object>();
        //private List<List<Object>> itemData = new List<List<Object>>();

        internal MultiListBox(IntPtr multiList)
            :base(multiList)
        {

        }

        //------------------------------------------------------------------------------//
        // Methods for work with columns
        //------------------------------------------------------------------------------//
        // Column crud

        public uint getColumnCount()
        {
            return MultiListBox_getColumnCount(widget).horriblyUnsafeToUInt32();
        }

        public void insertColumnAt(uint column, String name, int width)
        {
            insertColumnAt(column, name, width, null);
        }

        public void insertColumnAt(uint column, String name, int width, Object data)
        {
            MultiListBox_insertColumnAt(widget, new UIntPtr(column), name, width);
            columnData.Insert((int)column, data);
        }

        public void addColumn(String name, int width)
        {
            addColumn(name, width, null);
        }

        public void addColumn(String name, int width, Object data)
        {
            MultiListBox_addColumn(widget, name, width);
            columnData.Add(data);
        }

        public void removeColumnAt(uint column)
        {
            MultiListBox_removeColumnAt(widget, new UIntPtr(column));
            columnData.RemoveAt((int)column);
        }

        public void removeAllColumns()
        {
            MultiListBox_removeAllColumns(widget);
            columnData.Clear();
        }


        //------------------------------------------------------------------------------//
        // Column name

        public void setColumnNameAt(uint column, String name)
        {
            MultiListBox_setColumnNameAt(widget, new UIntPtr(column), name);
        }

        public void setColumnWidthAt(uint column, int width)
        {
            MultiListBox_setColumnWidthAt(widget, new UIntPtr(column), width);
        }

        public String getColumnNameAt(uint column)
        {
            return Marshal.PtrToStringUni(MultiListBox_getColumnNameAt(widget, new UIntPtr(column)));
        }

        public int getColumnWidthAt(uint column)
        {
            return MultiListBox_getColumnWidthAt(widget, new UIntPtr(column));
        }

        public void sortByColumn(uint column)
        {
            MultiListBox_sortByColumn(widget, new UIntPtr(column));
        }

        public void sortByColumn(uint column, bool backward)
        {
            MultiListBox_sortByColumn2(widget, new UIntPtr(column), backward);
        }

        public void setColumnResizingPolicyAt(uint index, ResizingPolicy value)
        {
            MultiListBox_setColumnResizingPolicyAt(widget, new UIntPtr(index), value);
        }

        //------------------------------------------------------------------------------//
        // Column data

        public void setColumnDataAt(uint index, Object data)
        {
            columnData[(int)index] = data;
        }

        public void clearColumnDataAt(uint index)
        {
            columnData[(int)index] = null;
        }

        public Object getColumnDataAt(uint index)
        {
            return columnData[(int)index];
        }

        //------------------------------------------------------------------------------//
        // Methods for work with lines
        //------------------------------------------------------------------------------//
        // Item crud

        public uint getItemCount()
        {
            return MultiListBox_getItemCount(widget).horriblyUnsafeToUInt32();
        }

        public void insertItemAt(uint index, String name)
        {
            insertItemAt(index, name, null);
        }

        public void insertItemAt(uint index, String name, Object data)
        {
            MultiListBox_insertItemAt(widget, new UIntPtr(index), name);
            rowData.Insert((int)index, data);
        }

        public void addItem(String name)
        {
            addItem(name, null);
        }

        public void addItem(String name, Object data)
        {
            MultiListBox_addItem(widget, name);
            rowData.Add(data);
        }

        public void removeItemAt(uint index)
        {
            MultiListBox_removeItemAt(widget, new UIntPtr(index));
            rowData.RemoveAt((int)index);
        }

        public void removeAllItems()
        {
            MultiListBox_removeAllItems(widget);
            rowData.Clear();
        }

        public void swapItemsAt(uint index1, uint index2)
        {
            MultiListBox_swapItemsAt(widget, new UIntPtr(index1), new UIntPtr(index2));
        }


        //------------------------------------------------------------------------------//
        // ItemName

        public void setItemNameAt(uint index, String name)
        {
            MultiListBox_setItemNameAt(widget, new UIntPtr(index), name);
        }

        public String getItemNameAt(uint index)
        {
            return Marshal.PtrToStringUni(MultiListBox_getItemNameAt(widget, new UIntPtr(index)));
        }


        //------------------------------------------------------------------------------//
        // Selection

        public uint getIndexSelected()
        {
            return MultiListBox_getIndexSelected(widget).horriblyUnsafeToUInt32();
        }

        public void setIndexSelected(uint index)
        {
            MultiListBox_setIndexSelected(widget, new UIntPtr(index));
        }

        public void clearIndexSelected()
        {
            MultiListBox_clearIndexSelected(widget);
        }

        public bool hasItemSelected()
        {
            UIntPtr result = MultiListBox_getIndexSelected(widget);
            //Check for max values depending on current runtime (32 or 64 bit).
            return UIntPtr.Size == 4 ? result.horriblyUnsafeToUInt32() != UInt32.MaxValue : result.ToUInt64() != UInt64.MaxValue;
        }


        //------------------------------------------------------------------------------//
        // Item Data
        public void setItemDataAt(uint index, Object data)
        {
            rowData[(int)index] = data;
        }

        public void clearItemDataAt(uint index)
        {
            rowData[(int)index] = null;
        }

        public Object getItemDataAt(uint index)
        {
            return rowData[(int)index];
        }

        public uint findItemWithData(Object data)
        {
            uint index = 0;
            foreach (Object obj in rowData)
            {
                if (obj == data)
                {
                    return index;
                }
                ++index;
            }
            return uint.MaxValue;
        }

        //------------------------------------------------------------------------------//
        // Methods for work with sub lines
        //------------------------------------------------------------------------------//
        // SubItem Name

        public void setSubItemNameAt(uint column, uint index, String name)
        {
            MultiListBox_setSubItemNameAt(widget, new UIntPtr(column), new UIntPtr(index), name);
        }


        public String getSubItemNameAt(uint column, uint index)
        {
            return Marshal.PtrToStringUni(MultiListBox_getSubItemNameAt(widget, new UIntPtr(column), new UIntPtr(index)));
        }

        public bool findSubItemWith(uint column, String name, out uint index)
        {
            UIntPtr result = MultiListBox_findSubItemWith(widget, new UIntPtr(column), name);
            index = result.horriblyUnsafeToUInt32();
            return UIntPtr.Size == 4 ? result.horriblyUnsafeToUInt32() != UInt32.MaxValue : result.ToUInt64() != UInt64.MaxValue;
        }

        //Set SubItem data

        //unsupported for now
        //public void setSubItemDataAt(uint column, uint index, Object data)
        //{
        //    itemData[(int)index][(int)column] = data;
        //}

        //public void clearSubItemDataAt(uint column, uint index)
        //{
        //    itemData[(int)index][(int)column] = null;
        //}

        //public Object getSubItemDataAt(uint column, uint index)
        //{
        //    return itemData[(int)index][(int)column];
        //}

        public bool SortOnChanges
        {
            get
            {
                return MultiListBox_getSortOnChanges(widget);
            }
            set
            {
                MultiListBox_setSortOnChanges(widget, value);
            }
        }

        #region Events

        public event MyGUIEvent ListSelectAccept
        {
            add
            {
                eventManager.addDelegate<EventListSelectAcceptTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventListSelectAcceptTranslator>(value);
            }
        }

        public event MyGUIEvent ListChangePosition
        {
            add
            {
                eventManager.addDelegate<EventListChangePositionTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventListChangePositionTranslator>(value);
            }
        }

        #endregion

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr MultiListBox_getColumnCount(IntPtr multiList);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_insertColumnAt(IntPtr multiList, UIntPtr column, [MarshalAs(UnmanagedType.LPWStr)] String name, int width);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_addColumn(IntPtr multiList, [MarshalAs(UnmanagedType.LPWStr)]  String name, int width);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_removeColumnAt(IntPtr multiList, UIntPtr column);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_removeAllColumns(IntPtr multiList);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_setColumnNameAt(IntPtr multiList, UIntPtr column, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_setColumnWidthAt(IntPtr multiList, UIntPtr column, int width);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MultiListBox_getColumnNameAt(IntPtr multiList, UIntPtr column);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern int MultiListBox_getColumnWidthAt(IntPtr multiList, UIntPtr column);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_sortByColumn(IntPtr multiList, UIntPtr column);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_sortByColumn2(IntPtr multiList, UIntPtr column, bool backward);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr MultiListBox_getItemCount(IntPtr multiList);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_insertItemAt(IntPtr multiList, UIntPtr index, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_addItem(IntPtr multiList, [MarshalAs(UnmanagedType.LPWStr)]  String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_removeItemAt(IntPtr multiList, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_removeAllItems(IntPtr multiList);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_swapItemsAt(IntPtr multiList, UIntPtr index1, UIntPtr index2);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_setItemNameAt(IntPtr multiList, UIntPtr index, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MultiListBox_getItemNameAt(IntPtr multiList, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr MultiListBox_getIndexSelected(IntPtr multiList);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_setIndexSelected(IntPtr multiList, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_clearIndexSelected(IntPtr multiList);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_setSubItemNameAt(IntPtr multiList, UIntPtr column, UIntPtr index, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MultiListBox_getSubItemNameAt(IntPtr multiList, UIntPtr column, UIntPtr index);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern UIntPtr MultiListBox_findSubItemWith(IntPtr multiList, UIntPtr column, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MultiListBox_setSortOnChanges(IntPtr multiList, bool value);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MultiListBox_getSortOnChanges(IntPtr multiList);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void MultiListBox_setColumnResizingPolicyAt(IntPtr multiList, UIntPtr _index, ResizingPolicy _value);

#endregion
    }
}
