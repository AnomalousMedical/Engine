using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class TableRowCollection : TableElementCollection<TableRow>
    {
        public override void add(TableRow item)
        {
            base.add(item);
            item.Cells.createMissingCells();
        }

        public override void remove(TableRow item)
        {
            base.remove(item);
            Table.checkLastEditedRow();
        }
    }
}
