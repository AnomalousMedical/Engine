using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    /// <summary>
    /// This is an event.  It can tell the client if it is down or up and if this
    /// was the first frame the event was down or up.  Events can be thought of as
    /// buttons.
    /// </summary>
    public class MessageEvent
    {
        private HashSet<KeyboardButtonCode> keyboardButtons = new HashSet<KeyboardButtonCode>();
        private HashSet<MouseButtonCode> mouseButtons = new HashSet<MouseButtonCode>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The int representing the name of the event.</param>
        public MessageEvent(object name)
        {
            Name = name;
            Down = false;
            Up = true;
            FirstFrameDown = false;
            FirstFrameUp = false;
        }

        /// <summary>
        /// The name of the event.
        /// </summary>
        public object Name { get; private set; }

        /// <summary>
        /// Returns true if this is the first frame the event has been active.
        /// </summary>
        public bool FirstFrameDown { get; private set; }

        /// <summary>
        /// Returns true if this is the first frame the event went inactive.
        /// </summary>
        public bool FirstFrameUp { get; private set; }

        /// <summary>
        /// Returns true if the event has been held down more than 1 frame.
        /// </summary>
        public bool Down { get; private set; }

        /// <summary>
        /// Returns true if the event has been released more thean 1 frame.
        /// </summary>
        public bool Up { get; private set; }

        /// <summary>
        /// Add a keyboard button binding to the event.
        /// </summary>
        /// <param name="button">The button to add.</param>
        public void addButton(KeyboardButtonCode button)
        {
            keyboardButtons.Add(button);
        }

        /// <summary>
        /// Remove a keyboard button binding from the event.
        /// </summary>
        /// <param name="button">The button to remove.</param>
        public void removeButton(KeyboardButtonCode button)
        {
            keyboardButtons.Remove(button);
        }

        /// <summary>
        /// Add a mouse button binding to the event.
        /// </summary>
        /// <param name="button">The button to add.</param>
        public void addButton(MouseButtonCode button)
        {
            mouseButtons.Add(button);
        }

        /// <summary>
        /// Remove a mouse button binding from the event.
        /// </summary>
        /// <param name="button">The button to remove.</param>
        public void removeButton(MouseButtonCode button)
        {
            mouseButtons.Remove(button);
        }

        /// <summary>
        /// Clear all bindings from this event.  Warning this will make it always inactive.
        /// </summary>
        public void clearButtons()
        {
            keyboardButtons.Clear();
            mouseButtons.Clear();
        }

        /// <summary>
        /// Called internally to manage status of the event.
        /// </summary>
        internal void update(EventManager eventManager)
        {
            if (scanButtons(eventManager))
            {
                if (FirstFrameDown)
                {
                    Down = true;
                    FirstFrameDown = false;
                }
                else if (!Down)
                {
                    FirstFrameDown = true;
                }
                FirstFrameUp = false;
                Up = false;
            }
            else
            {
                if (FirstFrameUp)
                {
                    Up = true;
                    FirstFrameUp = false;
                }
                else if (!Up)
                {
                    FirstFrameUp = true;
                }
                FirstFrameDown = false;
                Down = false;
            }
        }

        /// <summary>
        /// Internal method to make sure all the buttons for the event are pressed.
        /// </summary>
        /// <param name="eventManager">The event manager with the keyboard and mouse being used.</param>
        /// <returns>True if all the required buttons are pressed.</returns>
        private bool scanButtons(EventManager eventManager)
        {
            Mouse mouse = eventManager.Mouse;
            Keyboard keyboard = eventManager.Keyboard;
            bool active = keyboardButtons.Count + mouseButtons.Count > 0;
            if (active)
            {
                foreach (KeyboardButtonCode keyCode in keyboardButtons)
                {
                    active &= keyboard.isKeyDown(keyCode);
                    if (!active)
                    {
                        break;
                    }
                }
                if (active)
                {
                    foreach (MouseButtonCode mouseCode in mouseButtons)
                    {
                        active &= mouse.buttonDown(mouseCode);
                        if (!active)
                        {
                            break;
                        }
                    }
                }
            }
            return active;
        }
    }
}
