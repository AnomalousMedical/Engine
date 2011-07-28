using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;

namespace MyGUIPlugin
{
    public class NumericEdit
    {
        public event MyGUIEvent ValueChanged;

        private Edit edit;
        private String lastCaption;
        private bool allowFloat;
        private float minValue;
        private float maxValue;
        private float keyFocusValue = float.NaN;

        public NumericEdit(Edit edit)
        {
            this.edit = edit;
            lastCaption = edit.Caption;
            edit.EventEditTextChange += new MyGUIEvent(edit_EventEditTextChange);
            edit.MouseWheel += new MyGUIEvent(edit_MouseWheel);
            edit.KeyLostFocus += new MyGUIEvent(edit_KeyLostFocus);
            edit.KeySetFocus += new MyGUIEvent(edit_KeySetFocus);
            edit.EventEditSelectAccept += new MyGUIEvent(edit_EventEditSelectAccept);
            edit.AllowMouseScroll = false;
            Increment = 1;
            FireValueChangedOnType = false;

            //AllowFloat
            String userString = edit.getUserString("AllowFloat");
            if (userString == null || !Boolean.TryParse(userString, out allowFloat))
            {
                allowFloat = true;
            }

            //MinValue
            userString = edit.getUserString("MinValue");
            if (userString == null || !NumberParser.TryParse(userString, out minValue))
            {
                minValue = 0;
            }
            
            //MaxValue
            userString = edit.getUserString("MaxValue");
            if (userString == null || !NumberParser.TryParse(userString, out maxValue))
            {
                maxValue = 10;
            }
        }

        public NumericEdit(Edit edit, Button upButton, Button downButton)
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
                if (AllowFloat)
                {
                    float value = 0;
                    if (Single.TryParse(currentCaption, out value))
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
                else
                {
                    int value = 0;
                    if (Int32.TryParse(currentCaption, out value))
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
            }
            lastCaption = edit.Caption;
        }

        public bool AllowFloat
        {
            get
            {
                return allowFloat;
            }
            set
            {
                allowFloat = value;
            }
        }

        public int IntValue
        {
            get
            {
                int value = 0;
                Int32.TryParse(edit.Caption, out value);
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

        public float FloatValue
        {
            get
            {
                float value = 0;
                Single.TryParse(edit.Caption, out value);
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

        public decimal DecimalValue
        {
            get
            {
                decimal value = 0;
                decimal.TryParse(edit.Caption, out value);
                return value;
            }
            set
            {
                if ((float)value >= minValue && (float)value <= maxValue)
                {
                    edit.Caption = value.ToString();
                    lastCaption = edit.Caption;
                }
            }
        }

        public float MinValue
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

        public float MaxValue
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

        public float Increment { get; set; }

        public Edit Edit
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
            float newVal = FloatValue - Increment;
            if (newVal < minValue)
            {
                newVal = minValue;
            }
            FloatValue = newVal;
            fireValueChanged();
        }

        void upButton_MouseButtonClick(Widget source, EventArgs e)
        {
            float newVal = FloatValue + Increment;
            if (newVal > maxValue)
            {
                newVal = maxValue;
            }
            FloatValue = newVal;
            fireValueChanged();
        }

        void edit_MouseWheel(Widget source, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            int wheelDelta = me.RelativeWheelPosition > 0 ? 1 : -1;
            float newVal = FloatValue + Increment * wheelDelta;
            if (newVal > maxValue)
            {
                newVal = maxValue;
            }
            else if (newVal < minValue)
            {
                newVal = minValue;
            }
            FloatValue = newVal;
            fireValueChanged();
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
            keyFocusValue = FloatValue;
        }

        void edit_KeyLostFocus(Widget source, EventArgs e)
        {
            if (FloatValue != keyFocusValue)
            {
                fireValueChanged();
            }
        }

        void edit_EventEditSelectAccept(Widget source, EventArgs e)
        {
            if (FloatValue != keyFocusValue)
            {
                fireValueChanged();
                keyFocusValue = FloatValue;
            }
        }
    }
}
