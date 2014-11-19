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

        /// <summary>
        /// Constructor
        /// </summary>
        public ButtonGroup()
        {

        }

        /// <summary>
        /// Add a button with a key.
        /// </summary>
        /// <param name="key">The key for the button.</param>
        /// <param name="button">The button.</param>
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

        /// <summary>
        /// Remove a button by key.
        /// </summary>
        /// <param name="key">The key of the button to remove.</param>
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

        /// <summary>
        /// Clear all buttons from the group.
        /// </summary>
        public void clear()
        {
            buttons.Clear();
            selectedButton = null;
        }

        /// <summary>
        /// Get the button specified by key.
        /// </summary>
        /// <param name="key">The key for the button/</param>
        /// <returns>The button or null if the key does not map to a button.</returns>
        public Button this[T key]
        {
            get
            {
                Button button;
                buttons.TryGetValue(key, out button);
                return button;
            }
        }

        /// <summary>
        /// Get the value specified by Button.
        /// </summary>
        /// <param name="button">The button to lookup.</param>
        /// <returns>The value for the button.</returns>
        public T this[Button button]
        {
            get
            {
                return (T)button.UserObject;
            }
        }

        /// <summary>
        /// Get / Set the currently selected button.
        /// </summary>
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

        /// <summary>
        /// Get the currently selected data.
        /// </summary>
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

        /// <summary>
        /// Enabled status.
        /// </summary>
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
