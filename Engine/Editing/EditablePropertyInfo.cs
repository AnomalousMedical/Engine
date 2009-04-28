using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace Engine.Editing
{
    /// <summary>
    /// This class contains information about the EditableProperties to be
    /// displayed on a UI.
    /// </summary>
    [DoNotCopy]
    public class EditablePropertyInfo
    {
        private List<EditablePropertyColumn> columns = new List<EditablePropertyColumn>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public EditablePropertyInfo()
        {

        }

        /// <summary>
        /// Add a column.
        /// </summary>
        /// <param name="column">The column to add.</param>
        public void addColumn(EditablePropertyColumn column)
        {
            columns.Add(column);
        }

        /// <summary>
        /// Remove a column.
        /// </summary>
        /// <param name="column">The column to remove.</param>
        public void removeColumn(EditablePropertyColumn column)
        {
            columns.Remove(column);
        }

        /// <summary>
        /// Get an enumerator over all columns.
        /// </summary>
        /// <returns>An enumerator over all columns.</returns>
        public IEnumerable<EditablePropertyColumn> getColumns()
        {
            return columns;
        }

        /// <summary>
        /// Get the column at the specified index.
        /// </summary>
        /// <param name="index">The index of the column.</param>
        /// <returns>The column at the specified index.</returns>
        public EditablePropertyColumn getColumn(int index)
        {
            return columns[index];
        }

        /// <summary>
        /// Get the number of columns.
        /// </summary>
        /// <returns>The number of columns.</returns>
        public int getNumColumns()
        {
            return columns.Count;
        }
    }
}
