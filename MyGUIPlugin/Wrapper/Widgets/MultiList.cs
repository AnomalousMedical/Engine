using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{

    public class MultiList : Widget
    {
        private List<Object> columnData = new List<Object>();
        private List<Object> rowData = new List<Object>();
        //private List<List<Object>> itemData = new List<List<Object>>();

        internal MultiList(IntPtr multiList)
            :base(multiList)
        {

        }

        //------------------------------------------------------------------------------//
        // Methods for work with columns
        //------------------------------------------------------------------------------//
        // Column crud

        public uint getColumnCount()
        {
            return MultiList_getColumnCount(widget).ToUInt32();
        }

        public void insertColumnAt(uint column, String name, int width)
        {
            insertColumnAt(column, name, width, null);
        }

        public void insertColumnAt(uint column, String name, int width, Object data)
        {
            MultiList_insertColumnAt(widget, new UIntPtr(column), name, width);
            columnData.Insert((int)column, data);
        }

        public void addColumn(String name, int width)
        {
            addColumn(name, width, null);
        }

        public void addColumn(String name, int width, Object data)
        {
            MultiList_addColumn(widget, name, width);
            columnData.Add(data);
        }

        public void removeColumnAt(uint column)
        {
            MultiList_removeColumnAt(widget, new UIntPtr(column));
            columnData.RemoveAt((int)column);
        }

        public void removeAllColumns()
        {
            MultiList_removeAllColumns(widget);
            columnData.Clear();
        }


        //------------------------------------------------------------------------------//
        // Column name

        public void setColumnNameAt(uint column, String name)
        {
            MultiList_setColumnNameAt(widget, new UIntPtr(column), name);
        }

        public void setColumnWidthAt(uint column, int width)
        {
            MultiList_setColumnWidthAt(widget, new UIntPtr(column), width);
        }

        public String getColumnNameAt(uint column)
        {
            return Marshal.PtrToStringUni(MultiList_getColumnNameAt(widget, new UIntPtr(column)));
        }

        public int getColumnWidthAt(uint column)
        {
            return MultiList_getColumnWidthAt(widget, new UIntPtr(column));
        }

        public void sortByColumn(uint column)
        {
            MultiList_sortByColumn(widget, new UIntPtr(column));
        }

        public void sortByColumn(uint column, bool backward)
        {
            MultiList_sortByColumn2(widget, new UIntPtr(column), backward);
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
            return MultiList_getItemCount(widget).ToUInt32();
        }

        public void insertItemAt(uint index, String name)
        {
            insertItemAt(index, name, null);
        }

        public void insertItemAt(uint index, String name, Object data)
        {
            MultiList_insertItemAt(widget, new UIntPtr(index), name);
            rowData.Insert((int)index, data);
        }

        public void addItem(String name)
        {
            addItem(name, null);
        }

        public void addItem(String name, Object data)
        {
            MultiList_addItem(widget, name);
            rowData.Add(data);
        }

        public void removeItemAt(uint index)
        {
            MultiList_removeItemAt(widget, new UIntPtr(index));
            rowData.RemoveAt((int)index);
        }

        public void removeAllItems()
        {
            MultiList_removeAllItems(widget);
            rowData.Clear();
        }

        public void swapItemsAt(uint index1, uint index2)
        {
            MultiList_swapItemsAt(widget, new UIntPtr(index1), new UIntPtr(index2));
        }


        //------------------------------------------------------------------------------//
        // ItemName

        public void setItemNameAt(uint index, String name)
        {
            MultiList_setItemNameAt(widget, new UIntPtr(index), name);
        }

        public String getItemNameAt(uint index)
        {
            return Marshal.PtrToStringUni(MultiList_getItemNameAt(widget, new UIntPtr(index)));
        }


        //------------------------------------------------------------------------------//
        // Selection

        public uint getIndexSelected()
        {
            return MultiList_getIndexSelected(widget).ToUInt32();
        }

        public void setIndexSelected(uint index)
        {
            MultiList_setIndexSelected(widget, new UIntPtr(index));
        }

        public void clearIndexSelected()
        {
            MultiList_clearIndexSelected(widget);
        }

        public bool hasItemSelected()
        {
            UIntPtr result = MultiList_getIndexSelected(widget);
            //Check for max values depending on current runtime (32 or 64 bit).
            return UIntPtr.Size == 4 ? result.ToUInt32() != UInt32.MaxValue : result.ToUInt64() != UInt64.MaxValue;
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


        //------------------------------------------------------------------------------//
        // Methods for work with sub lines
        //------------------------------------------------------------------------------//
        // SubItem Name

        public void setSubItemNameAt(uint column, uint index, String name)
        {
            MultiList_setSubItemNameAt(widget, new UIntPtr(column), new UIntPtr(index), name);
        }


        public String getSubItemNameAt(uint column, uint index)
        {
            return Marshal.PtrToStringUni(MultiList_getSubItemNameAt(widget, new UIntPtr(column), new UIntPtr(index)));
        }

        public uint findSubItemWith(uint column, String name)
        {
            return MultiList_findSubItemWith(widget, new UIntPtr(column), name).ToUInt32();
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

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr MultiList_getColumnCount(IntPtr multiList);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_insertColumnAt(IntPtr multiList, UIntPtr column, [MarshalAs(UnmanagedType.LPWStr)] String name, int width);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_addColumn(IntPtr multiList, [MarshalAs(UnmanagedType.LPWStr)]  String name, int width);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_removeColumnAt(IntPtr multiList, UIntPtr column);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_removeAllColumns(IntPtr multiList);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_setColumnNameAt(IntPtr multiList, UIntPtr column, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_setColumnWidthAt(IntPtr multiList, UIntPtr column, int width);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MultiList_getColumnNameAt(IntPtr multiList, UIntPtr column);

        [DllImport("MyGUIWrapper")]
        private static extern int MultiList_getColumnWidthAt(IntPtr multiList, UIntPtr column);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_sortByColumn(IntPtr multiList, UIntPtr column);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_sortByColumn2(IntPtr multiList, UIntPtr column, bool backward);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr MultiList_getItemCount(IntPtr multiList);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_insertItemAt(IntPtr multiList, UIntPtr index, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_addItem(IntPtr multiList, [MarshalAs(UnmanagedType.LPWStr)]  String name);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_removeItemAt(IntPtr multiList, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_removeAllItems(IntPtr multiList);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_swapItemsAt(IntPtr multiList, UIntPtr index1, UIntPtr index2);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_setItemNameAt(IntPtr multiList, UIntPtr index, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MultiList_getItemNameAt(IntPtr multiList, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr MultiList_getIndexSelected(IntPtr multiList);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_setIndexSelected(IntPtr multiList, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_clearIndexSelected(IntPtr multiList);

        [DllImport("MyGUIWrapper")]
        private static extern void MultiList_setSubItemNameAt(IntPtr multiList, UIntPtr column, UIntPtr index, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr MultiList_getSubItemNameAt(IntPtr multiList, UIntPtr column, UIntPtr index);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr MultiList_findSubItemWith(IntPtr multiList, UIntPtr column, [MarshalAs(UnmanagedType.LPWStr)] String name);

#endregion
    }
}
