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
            button.Selected = !button.Selected;
            if (CheckedChanged != null)
            {
                CheckedChanged.Invoke(button, EventArgs.Empty);
            }
        }

        public bool Checked
        {
            get
            {
                return button.Selected;
            }
            set
            {
                if (Checked != value)
                {
                    button.Selected = value;
                    if (CheckedChanged != null)
                    {
                        CheckedChanged.Invoke(button, EventArgs.Empty);
                    }
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return button.Enabled;
            }
            set
            {
                button.Enabled = value;
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
