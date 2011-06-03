using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class TableCellCollection : TableElementCollection<TableCell>
    {
        private TableRow row;

        public TableCellCollection(TableRow row)
        {
            this.row = row;
        }

        public override void add(TableCell item)
        {
            base.add(item);
            item.Row = row;
        }

        public override void remove(TableCell item)
        {
            base.remove(item);
            item.Row = null;
        }

        internal void createMissingCells()
        {
            TableColumnCollection columns = Table.Columns;
            int numColumns = columns.Count;
            for (int i = 0; i < numColumns; ++i)
            {
                TableCell cell = null;
                if (i < Count)
                {
                    cell = this[i];
                    if (cell == null)
                    {
                        this[i] = cell = columns[i].createCell();
                    }
                }
                else
                {
                    cell = columns[i].createCell();
                    add(cell);
                }
                cell.setStaticMode();
            }
        }
    }
}
