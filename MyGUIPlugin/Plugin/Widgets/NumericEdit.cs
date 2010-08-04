using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class NumericEdit
    {
        private Edit edit;
        private String lastCaption;
        private bool allowFloat;
        private float minValue;
        private float maxValue;

        public NumericEdit(Edit edit)
        {
            this.edit = edit;
            lastCaption = edit.Caption;
            edit.EventEditTextChange += new MyGUIEvent(edit_EventEditTextChange);

            //AllowFloat
            String userString = edit.getUserString("AllowFloat");
            if (userString == null || !Boolean.TryParse(userString, out allowFloat))
            {
                allowFloat = true;
            }

            //MinValue
            userString = edit.getUserString("MinValue");
            if (userString == null || !Single.TryParse(userString, out minValue))
            {
                minValue = 0;
            }
            
            //MaxValue
            userString = edit.getUserString("MaxValue");
            if (userString == null || !Single.TryParse(userString, out maxValue))
            {
                maxValue = 10;
            }
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
                            edit.Caption = value.ToString();
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
                            edit.Caption = value.ToString();
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

        public Edit Edit
        {
            get
            {
                return edit;
            }
        }
    }
}
