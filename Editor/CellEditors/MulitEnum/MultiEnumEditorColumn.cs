using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Editor
{
    public class MultiEnumEditorColumn : DataGridViewColumn
    {
        public MultiEnumEditorColumn()
        {
            this.CellTemplate = new MultiEnumEditorCell();
        }
    }
}
