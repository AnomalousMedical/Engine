using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Editor
{
    public class SingleEnumEditorColumn : DataGridViewColumn
    {
        public SingleEnumEditorColumn()
        {
            this.CellTemplate = new SingleEnumEditorCell();
        }
    }
}
