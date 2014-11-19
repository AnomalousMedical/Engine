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
    /// </summary>
    public class ButtonGroup
    {
        private List<Button> buttons = new List<Button>();
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
        /// Add a button to the group.
        /// </summary>
        /// <param name="button">The button to add.</param>
        public void addButton(Button button)
        {
            button.Enabled = enabled;
            buttons.Add(button);
            button.MouseButtonClick += button_MouseButtonClick;
            if (selectedButton == null)
            {
                SelectedButton = button;
            }
        }

        /// <summary>
        /// Remove a button from the group.
        /// </summary>
        /// <param name="button">The button to remove.</param>
        public void removeButton(Button button)
        {
            button.MouseButtonClick -= button_MouseButtonClick;
            buttons.Remove(button);
            if (selectedButton == button)
            {
                if (buttons.Count > 0)
                {
                    SelectedButton = buttons[0];
                }
                else
                {
                    SelectedButton = null;
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
        /// Find a button with the given user data.
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public Button findButtonWithUserData(Object userData)
        {
            foreach (Button button in buttons)
            {
                if (button.UserObject.Equals(userData))
                {
                    return button;
                }
            }
            return null;
        }

        /// <summary>
        /// The currently selected button.
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
                foreach (Button button in buttons)
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
        /// True if the group is enabled, false otherwise.
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
                foreach (Button button in buttons)
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
