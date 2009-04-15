using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Editor
{
    public partial class SingleEnumEditor : UserControl
    {
        public SingleEnumEditor()
        {
            InitializeComponent();
        }

        public void populateCombo(Type enumType)
        {
            foreach (FieldInfo fieldInfo in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                valueBox.Items.Add(fieldInfo.Name);
            }
        }

        public String Value
        {
            get
            {
                return valueBox.SelectedText;
            }
            set
            {
                valueBox.SelectedText = value;
            }
        }
    }
}
