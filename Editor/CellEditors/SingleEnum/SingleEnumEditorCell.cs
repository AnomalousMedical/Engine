using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Editor
{
    public class SingleEnumEditorCell : DataGridViewComboBoxCell
    {
        public void populateCombo(Type theEnum)
        {
            foreach (FieldInfo fieldInfo in theEnum.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                this.Items.Add(fieldInfo.Name);
            }
        }        
    }
}
