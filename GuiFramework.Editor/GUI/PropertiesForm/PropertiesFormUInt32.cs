﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine.Editing;
using Engine;

namespace Anomalous.GuiFramework.Editor
{
    class PropertiesFormUInt32 : ConstrainableFormComponent
    {
        private UInt32NumericEdit num;
        private bool allowValueChanges = true;

        public PropertiesFormUInt32(EditableProperty property, Widget parent)
            : base(property, parent, "Anomalous.GuiFramework.Editor.GUI.PropertiesForm.PropertiesFormTextBox.layout")
        {
            widget.ForwardMouseWheelToParent = true;

            TextBox textBox = (TextBox)widget.findWidget("TextBox");
            textBox.Caption = property.getValue(0);
            textBox.ForwardMouseWheelToParent = true;

            num = new UInt32NumericEdit((EditBox)widget.findWidget("EditBox"));
            num.Value = (UInt32)property.getRealValue(1);
            num.ValueChanged += new MyGUIEvent(editBox_ValueChanged);
        }

        public override void refreshData()
        {
            num.Value = (UInt32)Property.getRealValue(1);
        }

        public override void setConstraints(ReflectedMinMaxEditableProperty minMaxProp)
        {
            num.MinValue = minMaxProp.MinValue.AsUInt32;
            num.MaxValue = minMaxProp.MaxValue.AsUInt32;
            num.Increment = minMaxProp.Increment.AsUInt32;
        }

        void editBox_ValueChanged(Widget source, EventArgs e)
        {
            setValue();
        }

        private void setValue()
        {
            if (allowValueChanges)
            {
                allowValueChanges = false;

                Property.setValue(1, num.Value);
                allowValueChanges = true;
            }
        }
    }
}