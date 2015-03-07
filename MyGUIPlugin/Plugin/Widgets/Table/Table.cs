using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class Table : IDisposable
    {
        public event EventHandler CellValueChanged;
        public event EventHandler<TableCellValidationEventArgs> CellValidating;
        private TableCellValidationEventArgs validationEventArgs = new TableCellValidationEventArgs();

        private Widget tableWidget;
        private TableCell editingCell;

        public Table(Widget tableWidget)
        {
            this.tableWidget = tableWidget;

            Columns = new TableColumnCollection();
            Columns.Table = this;
            Rows = new TableRowCollection();
            Rows.Table = this;
            Rows.Cleared += new Action(Rows_Cleared);
            RowHeight = ScaleHelper.Scaled(20);
            HeaderHeight = ScaleHelper.Scaled(20);
            CurrentEditRow = -1;
        }

        public void Dispose()
        {
            Rows.Dispose();
            Columns.Dispose();
        }

        public virtual void layout()
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

        public int Width
        {
            get
            {
                int width = 0;
                foreach (TableColumn column in Columns)
                {
                    width += column.Width;
                }
                return width;
            }
        }

        public int Height
        {
            get
            {
                return HeaderHeight + RowHeight * Rows.Count;
            }
        }

        private int _currentEditRow;
        public int CurrentEditRow
        {
            get
            {
                return _currentEditRow;
            }
            private set
            {
                if (_currentEditRow != value)
                {
                    if(_currentEditRow != -1 && _currentEditRow < Rows.Count)
                    {
                        Rows[_currentEditRow].setAppearSelected(false);
                    }

                    _currentEditRow = value;

                    if (_currentEditRow != -1 && _currentEditRow < Rows.Count)
                    {
                        Rows[_currentEditRow].setAppearSelected(true);
                    }
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return tableWidget.Enabled;
            }
            set
            {
                tableWidget.Enabled = value;
            }
        }

        /// <summary>
        /// Internal function to get the widget for this table. Cells can get
        /// this info through their protected TableWidget property.
        /// </summary>
        internal Widget TableWidget
        {
            get
            {
                return tableWidget;
            }
        }

        /// <summary>
        /// Request that a given cell be put into edit mode.
        /// </summary>
        /// <param name="cell"></param>
        internal void requestCellEdit(TableCell cell)
        {
            bool allowCellChange = true;

            if (editingCell != null)
            {
                validationEventArgs.reset();
                validationEventArgs.EditValue = editingCell.EditValue;
                if (CellValidating != null)
                {
                    CellValidating.Invoke(editingCell, validationEventArgs);
                }
                if (validationEventArgs.Cancel)
                {
                    allowCellChange = false;
                }
                else
                {
                    editingCell.commitEditValueToValue();
                    if (editingCell != null)
                    {
                        editingCell.setStaticMode();
                    }
                }
            }

            if (allowCellChange)
            {
                editingCell = cell;
                if (editingCell != null)
                {
                    editingCell.setEditMode();
                    CurrentEditRow = editingCell.RowIndex;
                }
            }
        }

        internal void fireCellValueChanged(TableCell cell)
        {
            if (CellValueChanged != null)
            {
                CellValueChanged.Invoke(cell, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Call when a row is going to be removed, should only need to be called from TableRowCollection.
        /// </summary>
        internal void goingToRemoveRow(int newRowCount)
        {
            if (editingCell != null)
            {
                if(editingCell.RowIndex == CurrentEditRow)
                {
                    editingCell = null;
                }
            }

            if (CurrentEditRow >= newRowCount) //Removed last row
            {
                CurrentEditRow = newRowCount - 1;
            }
            else if(CurrentEditRow != -1)
            {
                //Make the newly selected row appear selected
                Rows[CurrentEditRow + 1].setAppearSelected(true);
            }
        }

        void Rows_Cleared()
        {
            editingCell = null;
        }
    }
}
