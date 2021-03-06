﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine.Editing;
using Engine;

namespace Anomalous.GuiFramework.Editor
{
    class PropertiesFormDouble : ConstrainableFormComponent
    {
        private DoubleNumericEdit num;
        private bool allowValueChanges = true;

        public PropertiesFormDouble(EditableProperty property, Widget parent)
            : base(property, parent, "Anomalous.GuiFramework.Editor.GUI.PropertiesForm.PropertiesFormTextBox.layout")
        {
            widget.ForwardMouseWheelToParent = true;

            TextBox textBox = (TextBox)widget.findWidget("TextBox");
            textBox.Caption = property.getValue(0);
            textBox.ForwardMouseWheelToParent = true;

            num = new DoubleNumericEdit((EditBox)widget.findWidget("EditBox"));
            num.Value = (Double)property.getRealValue(1);
            num.ValueChanged += new MyGUIEvent(editBox_ValueChanged);
        }

        public override void refreshData()
        {
            num.Value = (Double)Property.getRealValue(1);
        }

        public override void setConstraints(ReflectedMinMaxEditableProperty minMaxProp)
        {
            num.MinValue = minMaxProp.MinValue.AsDouble;
            num.MaxValue = minMaxProp.MaxValue.AsDouble;
            num.Increment = minMaxProp.Increment.AsDouble;
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