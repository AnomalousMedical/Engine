using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class TableCellCollection : TableElementCollection<TableCell>
    {
        internal void createMissingCells()
        {
            TableColumnCollection columns = Table.Columns;
            int numColumns = columns.Count;
            for (int i = 0; i < numColumns; ++i)
            {
                TableCell cell = null;
                if (i + 1 < Count)
                {
                    if (this[i] == null)
                    {
                        this[i] = cell = columns[i].createCell();
                    }
                }
                else
                {
                    cell = columns[i].createCell();
                    add(cell);
                }
                cell.setStaticMode(Table.TableWidget);
            }
        }
    }
}
