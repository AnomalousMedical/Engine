using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MyGUIPlugin
{
    public class TableRow : TableElement
    {
        private TableCellCollection cells;

        public TableRow()
        {
            cells = new TableCellCollection(this);
            cells.Table = Table;
        }

        public override void Dispose()
        {
            cells.Dispose();
        }

        public TableCellCollection Cells
        {
            get
            {
                return cells;
            }
        }

        public int RowIndex
        {
            get
            {
                return Table.Rows.getItemIndex(this);
            }
        }

        public override Table Table
        {
            get
            {
                return base.Table;
            }
            internal set
            {
                base.Table = value;
                cells.Table = value;
            }
        }
    }
}
