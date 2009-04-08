using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public class EditablePropertyInfo
    {
        private List<EditablePropertyColumn> columns = new List<EditablePropertyColumn>();

        public EditablePropertyInfo()
        {

        }

        public void addColumn(EditablePropertyColumn column)
        {
            columns.Add(column);
        }

        public void removeColumn(EditablePropertyColumn column)
        {
            columns.Remove(column);
        }

        public IEnumerable<EditablePropertyColumn> getColumns()
        {
            return columns;
        }

        public EditablePropertyColumn getColumn(int index)
        {
            return columns[index];
        }

        public int getNumColumns()
        {
            return columns.Count;
        }
    }
}
