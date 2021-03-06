﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine.Editing;
using Engine;

namespace Anomalous.GuiFramework.Editor
{
    class PropertiesFormVector2 : ConstrainableFormComponent
    {
        private SingleNumericEdit x;
        private SingleNumericEdit y;
        private bool allowValueChanges = true;

        public PropertiesFormVector2(EditableProperty property, Widget parent)
            : base(property, parent, "Anomalous.GuiFramework.Editor.GUI.PropertiesForm.PropertiesFormXY.layout")
        {
            widget.ForwardMouseWheelToParent = true;

            TextBox textBox = (TextBox)widget.findWidget("TextBox");
            textBox.Caption = property.getValue(0);
            textBox.ForwardMouseWheelToParent = true;

            Vector2 value = (Vector2)property.getRealValue(1);

            x = new SingleNumericEdit((EditBox)widget.findWidget("X"));
            x.Value = value.x;
            x.ValueChanged += new MyGUIEvent(editBox_ValueChanged);

            y = new SingleNumericEdit((EditBox)widget.findWidget("Y"));
            y.Value = value.y;
            y.ValueChanged += new MyGUIEvent(editBox_ValueChanged);
        }

        public override void refreshData()
        {
            Vector2 value = (Vector2)Property.getRealValue(1);
            x.Value = value.x;
            y.Value = value.y;
        }

        public override void setConstraints(ReflectedMinMaxEditableProperty minMaxProp)
        {
            x.MinValue = minMaxProp.MinValue.AsSingle;
            x.MaxValue = minMaxProp.MaxValue.AsSingle;
            x.Increment = minMaxProp.Increment.AsSingle;

            y.MinValue = minMaxProp.MinValue.AsSingle;
            y.MaxValue = minMaxProp.MaxValue.AsSingle;
            y.Increment = minMaxProp.Increment.AsSingle;
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

                Property.setValue(1, new Vector2(x.Value, y.Value));
                allowValueChanges = true;
            }
        }
    }
}
