using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Engine;

namespace MyGUIPlugin
{
    public class EnumComboBox<EnumType>
    {
        private ComboBox comboBox;
        private Dictionary<EnumType, uint> enumIndexes = new Dictionary<EnumType,uint>();

        public EnumComboBox(ComboBox comboBox)
        {
            this.comboBox = comboBox;
            uint index = 0;
            Type enumType = typeof(EnumType);
            foreach (FieldInfo fieldInfo in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                PrettyNameAttribute[] prettyName = (PrettyNameAttribute[])fieldInfo.GetCustomAttributes(typeof(PrettyNameAttribute), true);
                if (prettyName.Length > 0)
                {
                    comboBox.addItem(prettyName[0].Name);
                }
                else
                {
                    comboBox.addItem(fieldInfo.Name);
                }
                EnumType value = (EnumType)fieldInfo.GetValue(enumType);
                enumIndexes.Add(value, index);
                comboBox.setItemDataAt(index++, value);
            }
        }

        public EnumType Value
        {
            get
            {
                return (EnumType)comboBox.SelectedItemData;
            }
            set
            {
                comboBox.SelectedIndex = enumIndexes[value];
            }
        }
    }
}
