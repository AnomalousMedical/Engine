using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Engine.Attributes;
using Engine;

namespace Editor
{
    public partial class MultiEnumEditor : UserControl
    {
        private struct EnumFieldInfo
        {
            public EnumFieldInfo(ulong value, int index)
            {
                this.value = value;
                this.index = index;
            }

            public ulong value;
            public int index;
        }

        private Type enumType;
        private List<EnumFieldInfo> inclusiveFields = new List<EnumFieldInfo>();
        private List<EnumFieldInfo> independentFields = new List<EnumFieldInfo>();

        public MultiEnumEditor()
        {
            InitializeComponent();
        }

        public void populateList(Type enumType)
        {
            multiEnumListBox.Items.Clear();
            inclusiveFields.Clear();
            independentFields.Clear();
            this.enumType = enumType;
            int i = 0;
            foreach (FieldInfo fieldInfo in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                ulong value = NumberParser.ParseUlong(fieldInfo.GetRawConstantValue().ToString());
                multiEnumListBox.Items.Add(new MultiEnumItem(fieldInfo.Name, value));
                if (fieldInfo.GetCustomAttributes(typeof(InclusiveEnumFieldAttribute), true).Length > 0)
                {
                    inclusiveFields.Add(new EnumFieldInfo(value, i));
                }
                else
                {
                    independentFields.Add(new EnumFieldInfo(value, i));
                }
                i++;
            }
        }


        public T getValue<T>()
        {
            Type enumType = typeof(T);
            if (enumType.IsEnum)
            {
                return (T)Value;
            }
            return default(T);
        }

        public object Value
        {
            get
            {
                ulong value = 0;
                foreach (object item in multiEnumListBox.CheckedItems)
                {
                    MultiEnumItem multiItem = (MultiEnumItem)item;
                    value |= multiItem.Value;
                }
                if (enumType != null)
                {
                    return Enum.Parse(enumType, value.ToString());
                }
                else
                {
                    return value;
                }
            }
            set
            {
                //Zero out the editor
                for (int i = 0; i < multiEnumListBox.Items.Count; i++)
                {
                    multiEnumListBox.SetItemChecked(i, false);
                }
                //Make sure the value given is the appropriate type.
                if (value != null && (value.GetType() == typeof(String) || value.GetType().IsEnum || value is byte || value is char || value is ushort || value is short || value is uint || value is int || value is ulong || value is long))
                {
                    ulong check;
                    //If we cannot parse the value correctly out of the value try to get it another way
                    if (!NumberParser.TryParse(value.ToString(), out check))
                    {
                        Type valueType = value.GetType();
                        //If it is an enum then we got the string for the value but not the value.
                        if (valueType.IsEnum || valueType == typeof(String))
                        {
                            FieldInfo field = enumType.GetField(value.ToString());
                            check = NumberParser.ParseUlong(field.GetRawConstantValue().ToString());
                        }
                        //If it isn't an enum or a string it is a signed number
                        else
                        {

                        }
                    }
                    //Check to see if we match any inclusive fields
                    EnumFieldInfo biggestInclusive = new EnumFieldInfo(0, 0);
                    foreach (EnumFieldInfo inclusive in inclusiveFields)
                    {
                        if ((check ^ inclusive.value) == 0 && inclusive.value > biggestInclusive.value)
                        {
                            biggestInclusive = inclusive;
                        }
                    }
                    //If we did not match any inclusive fields do a normal scan.
                    if (biggestInclusive.value == 0)
                    {
                        foreach (EnumFieldInfo independent in independentFields)
                        {
                            multiEnumListBox.SetItemChecked(independent.index, (independent.value & check) != 0);
                        }
                    }
                    //We matched an inclusive value so mark it.
                    else
                    {
                        for (int i = 0; i < multiEnumListBox.Items.Count; i++)
                        {
                            multiEnumListBox.SetItemChecked(i, false);
                        }
                        multiEnumListBox.SetItemChecked(biggestInclusive.index, true);
                    }
                }
            }
        }
    }
}
