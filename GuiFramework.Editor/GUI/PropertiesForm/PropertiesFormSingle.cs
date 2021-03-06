﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine.Editing;
using Engine;

namespace Anomalous.GuiFramework.Editor
{
    class PropertiesFormSingle : ConstrainableFormComponent
    {
        private SingleNumericEdit num;
        private bool allowValueChanges = true;

        public PropertiesFormSingle(EditableProperty property, Widget parent)
            : base(property, parent, "Anomalous.GuiFramework.Editor.GUI.PropertiesForm.PropertiesFormTextBox.layout")
        {
            widget.ForwardMouseWheelToParent = true;

            TextBox textBox = (TextBox)widget.findWidget("TextBox");
            textBox.Caption = property.getValue(0);
            textBox.ForwardMouseWheelToParent = true;

            num = new SingleNumericEdit((EditBox)widget.findWidget("EditBox"));
            num.Value = (Single)property.getRealValue(1);
            num.ValueChanged += new MyGUIEvent(editBox_ValueChanged);
        }

        public override void refreshData()
        {
            num.Value = (Single)Property.getRealValue(1);
        }

        public override void setConstraints(ReflectedMinMaxEditableProperty minMaxProp)
        {
            num.MinValue = minMaxProp.MinValue.AsSingle;
            num.MaxValue = minMaxProp.MaxValue.AsSingle;
            num.Increment = minMaxProp.Increment.AsSingle;
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
