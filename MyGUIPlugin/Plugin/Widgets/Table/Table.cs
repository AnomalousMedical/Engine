using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class Table
    {
        private Widget tableWidget;

        public Table(Widget tableWidget)
        {
            this.tableWidget = tableWidget;

            Columns = new TableColumnCollection();
            Columns.Table = this;
            Rows = new TableRowCollection();
            Rows.Table = this;
            RowHeight = 20;
            HeaderHeight = 20;
        }

        public void layout()
        {
            int xPosition = 0;
            int yPosition = 0;
            int columnIndex = 0;
            foreach (TableColumn column in Columns)
            {
                int columnWidth = column.Width;
                column.Position = new IntVector2(xPosition, yPosition);
                column.Size = new IntSize2(columnWidth, HeaderHeight);
                xPosition += columnWidth;
            }
            xPosition = 0;
            yPosition += HeaderHeight;

            foreach (TableRow row in Rows)
            {
                columnIndex = -1;
                while(++columnIndex < row.Cells.Count)
                {
                    TableCell cell = row.Cells[columnIndex];
                    int columnWidth = Columns[columnIndex].Width;
                    cell.Position = new IntVector2(xPosition, yPosition);
                    cell.Size = new IntSize2(columnWidth, RowHeight);
                    xPosition += columnWidth;
                }
                xPosition = 0;
                yPosition += RowHeight;
            }
        }

        public int RowHeight { get; set; }

        public int HeaderHeight { get; set; }

        public TableColumnCollection Columns { get; set; }

        public TableRowCollection Rows { get; set; }

        public Widget TableWidget
        {
            get
            {
                return tableWidget;
            }
        }
    }
}
