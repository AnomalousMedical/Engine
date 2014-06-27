using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Engine;

namespace Editor
{
    public class SingleEnumEditorCell : DataGridViewComboBoxCell
    {
        public void populateCombo(Type theEnum)
        {
            foreach(var name in EnumUtil.Elements(theEnum))
            {
                this.Items.Add(name);
            }
        }        
    }
}
