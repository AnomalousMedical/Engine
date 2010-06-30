using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    /// <summary>
    /// Helper class to make a button a check button.
    /// </summary>
    public class CheckButton
    {
        private Button button;
        public event MyGUIEvent CheckedChanged;

        public CheckButton(Button button)
        {
            this.button = button;
            button.MouseButtonClick += new MyGUIEvent(button_MouseButtonClick);
        }

        void button_MouseButtonClick(Widget source, EventArgs e)
        {
            button.StateCheck = !button.StateCheck;
            if (CheckedChanged != null)
            {
                CheckedChanged.Invoke(button, EventArgs.Empty);
            }
        }

        public bool Checked
        {
            get
            {
                return button.StateCheck;
            }
            set
            {
                if (Checked != value)
                {
                    button.StateCheck = value;
                    if (CheckedChanged != null)
                    {
                        CheckedChanged.Invoke(button, EventArgs.Empty);
                    }
                }
            }
        }

        public Button Button
        {
            get
            {
                return button;
            }
        }
    }
}
