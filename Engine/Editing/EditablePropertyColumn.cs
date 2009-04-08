using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public class EditablePropertyColumn
    {
        public EditablePropertyColumn(String header, bool readOnly)
        {
            this.Header = header;
            this.ReadOnly = readOnly;
        }

        public String Header { get; set; }

        public bool ReadOnly { get; set; }
    }
}
