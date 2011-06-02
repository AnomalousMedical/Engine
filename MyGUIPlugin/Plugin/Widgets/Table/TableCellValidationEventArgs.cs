using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class TableCellValidationEventArgs : EventArgs
    {
        public TableCellValidationEventArgs()
        {
            reset();
        }

        public void reset()
        {
            Cancel = false;
            EditValue = null;
        }

        public bool Cancel { get; set; }

        public Object EditValue { get; internal set; }
    }
}
