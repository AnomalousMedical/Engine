using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    /// <summary>
    /// This is a column in the EditableProperty table. It holds various
    /// information about a column.
    /// </summary>
    public class EditablePropertyColumn
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="header">The header text of the column.</param>
        /// <param name="readOnly">True if the column is read only.</param>
        public EditablePropertyColumn(String header, bool readOnly)
        {
            this.Header = header;
            this.ReadOnly = readOnly;
        }

        /// <summary>
        /// The header text.
        /// </summary>
        public String Header { get; set; }

        /// <summary>
        /// True if the column should be read only.
        /// </summary>
        public bool ReadOnly { get; set; }
    }
}
