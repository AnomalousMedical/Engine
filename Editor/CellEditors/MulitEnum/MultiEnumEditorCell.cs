using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Editor
{
    public class MultiEnumEditorCell : DataGridViewTextBoxCell
    {
        public Type EnumType { get; set; }

        public MultiEnumEditorCell()
        {
           
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            MultiEnumCellEditControl ctrl = DataGridView.EditingControl as MultiEnumCellEditControl;
            ctrl.EnumType = EnumType;
            ctrl.Value = this.Value;
        }

        public override Type EditType
        {
            get
            {
                return typeof(MultiEnumCellEditControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(object);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return "";
            }
        }
    }
}
