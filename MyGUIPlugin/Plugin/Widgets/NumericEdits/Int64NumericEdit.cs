﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;

namespace MyGUIPlugin
{
    public class Int64NumericEdit
    {
        public event MyGUIEvent ValueChanged;

        private EditBox edit;
        private String lastCaption;
        private Int64 minValue;
        private Int64 maxValue;
        private Int64 keyFocusValue = 0;
        private bool hasKeyFocus = false;

        public Int64NumericEdit(EditBox edit)
        {
            this.edit = edit;
            lastCaption = edit.Caption;
            edit.EventEditTextChange += new MyGUIEvent(edit_EventEditTextChange);
            edit.MouseWheel += new MyGUIEvent(edit_MouseWheel);
            edit.ClientWidget.MouseWheel += new MyGUIEvent(edit_MouseWheel);
            edit.KeyLostFocus += new MyGUIEvent(edit_KeyLostFocus);
            edit.KeySetFocus += new MyGUIEvent(edit_KeySetFocus);
            edit.EventEditSelectAccept += new MyGUIEvent(edit_EventEditSelectAccept);
            edit.AllowMouseScroll = false;
            Increment = 1;
            FireValueChangedOnType = false;

            //MinValue
            String userString = edit.getUserString("MinValue");
            if (userString == null || !NumberParser.TryParse(userString, out minValue))
            {
                minValue = Int64.MinValue;
            }
            
            //MaxValue
            userString = edit.getUserString("MaxValue");
            if (userString == null || !NumberParser.TryParse(userString, out maxValue))
            {
                maxValue = Int64.MaxValue;
            }
        }

        public Int64NumericEdit(EditBox edit, Button upButton, Button downButton)
            :this(edit)
        {
            upButton.MouseButtonClick += new MyGUIEvent(upButton_MouseButtonClick);
            downButton.MouseButtonClick += new MyGUIEvent(downButton_MouseButtonClick);
        }

        void edit_EventEditTextChange(Widget source, EventArgs e)
        {
            String currentCaption = edit.Caption;
            if (currentCaption != String.Empty)
            {
                if (currentCaption == "-")
                {
                    if (minValue >= 0)
                    {
                        edit.Caption = lastCaption;
                    }
                }
                Int64 value = 0;
                if (NumberParser.TryParse(currentCaption, out value))
                {
                    if (value >= minValue && value <= maxValue)
                    {
                        if (FireValueChangedOnType)
                        {
                            fireValueChanged();
                        }
                    }
                    else
                    {
                        edit.Caption = lastCaption;
                    }
                }
                else
                {
                    edit.Caption = lastCaption;
                }
            }
            lastCaption = edit.Caption;
        }

        public Int64 Value
        {
            get
            {
                Int64 value = 0;
                NumberParser.TryParse(edit.Caption, out value);
                return value;
            }
            set
            {
                if (value >= minValue && value <= maxValue)
                {
                    edit.Caption = value.ToString();
                    lastCaption = edit.Caption;
                }
            }
        }

        public Int64 MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
            }
        }

        public Int64 MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
            }
        }

        public Int64 Increment { get; set; }

        public EditBox Edit
        {
            get
            {
                return edit;
            }
        }

        /// <summary>
        /// Determines if the ValueChanged event is fired as the user types. Default: false.
        /// </summary>
        public bool FireValueChangedOnType { get; set; }

        void downButton_MouseButtonClick(Widget source, EventArgs e)
        {
            Int64 newVal = Value - Increment;
            if (newVal < minValue)
            {
                newVal = minValue;
            }
            Value = newVal;
            fireValueChanged();
        }

        void upButton_MouseButtonClick(Widget source, EventArgs e)
        {
            Int64 newVal = Value + Increment;
            if (newVal > maxValue)
            {
                newVal = maxValue;
            }
            Value = newVal;
            fireValueChanged();
        }

        void edit_MouseWheel(Widget source, EventArgs e)
        {
            if (hasKeyFocus)
            {
                MouseEventArgs me = (MouseEventArgs)e;
                int wheelDelta = me.RelativeWheelPosition > 0 ? 1 : -1;
                Int64 newVal = Value + Increment * wheelDelta;
                if (newVal > maxValue)
                {
                    newVal = maxValue;
                }
                else if (newVal < minValue)
                {
                    newVal = minValue;
                }
                Value = newVal;
                fireValueChanged();
            }
        }

        private void fireValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged.Invoke(edit, EventArgs.Empty);
            }
        }

        void edit_KeySetFocus(Widget source, EventArgs e)
        {
            keyFocusValue = Value;
            hasKeyFocus = true;
        }

        void edit_KeyLostFocus(Widget source, EventArgs e)
        {
            if (Value != keyFocusValue)
            {
                fireValueChanged();
            }
            hasKeyFocus = false;
        }

        void edit_EventEditSelectAccept(Widget source, EventArgs e)
        {
            if (Value != keyFocusValue)
            {
                fireValueChanged();
                keyFocusValue = Value;
            }
        }
    }
}
