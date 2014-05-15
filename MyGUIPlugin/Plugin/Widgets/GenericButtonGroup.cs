using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class provides synchronization between a group of buttons for the
    /// CheckState property. It also has an event for when the selecte button
    /// changes.
    /// 
    /// This class will use the UserObject property of the buttons placed into it.
    /// </summary>
    public class ButtonGroup<T>
    {
        private Dictionary<T, Button> buttons = new Dictionary<T, Button>();
        private Button selectedButton;
        private bool enabled = true;

        /// <summary>
        /// Called when the selected button changes.
        /// </summary>
        public event EventHandler SelectedButtonChanged;

        public ButtonGroup()
        {

        }

        public void addButton(T key, Button button)
        {
            button.Enabled = enabled;
            buttons.Add(key, button);
            button.MouseButtonClick += button_MouseButtonClick;
            button.UserObject = key;
            if (selectedButton == null)
            {
                SelectedButton = button;
            }
        }

        public void removeButton(T key)
        {
            Button button;
            if(buttons.TryGetValue(key, out button))
            {
                button.MouseButtonClick -= button_MouseButtonClick;
                buttons.Remove(key);
                if (selectedButton == button)
                {
                    SelectedButton = buttons.Values.FirstOrDefault();
                }
            }
        }

        public Button this[T key]
        {
            get
            {
                Button button;
                buttons.TryGetValue(key, out button);
                return button;
            }
        }

        public Button SelectedButton
        {
            get
            {
                return selectedButton;
            }
            set
            {
                selectedButton = value;
                foreach (Button button in buttons.Values)
                {
                    button.Selected = button == value;
                }
                if (SelectedButtonChanged != null)
                {
                    SelectedButtonChanged.Invoke(value, EventArgs.Empty);
                }
            }
        }

        public T Selection
        {
            get
            {
                return (T)selectedButton.UserObject;
            }
            set
            {
                SelectedButton = this[value];
            }
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                foreach (Button button in buttons.Values)
                {
                    button.Enabled = value;
                }
            }
        }

        void button_MouseButtonClick(Widget source, EventArgs e)
        {
            SelectedButton = source as Button;
        }
    }
}
