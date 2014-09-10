using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public class ButtonEvent : MessageEvent
    {
        public event MessageEventCallback FirstFrameDownEvent;
        public event MessageEventCallback FirstFrameUpEvent;

        /// <summary>
        /// Called the frame after the FirstFrameDownEvent is fired and continues until FirstFrameUpEvent is called.
        /// </summary>
        public event MessageEventCallback OnHeldDown;

        /// <summary>
        /// Called whenever Down is true, which is during FirstFrameDown and Held down, FirstFrameDown will always be called before OnDown on that frame.
        /// </summary>
        public event MessageEventCallback OnDown;

        private HashSet<KeyboardButtonCode> keyboardButtons = new HashSet<KeyboardButtonCode>();
        private HashSet<MouseButtonCode> mouseButtons = new HashSet<MouseButtonCode>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The int representing the name of the event.</param>
        public ButtonEvent(object eventLayerKey)
            :base(eventLayerKey)
        {
            HeldDown = false;
            ReleasedUp = true;
            FirstFrameDown = false;
            FirstFrameUp = false;
        }

        public ButtonEvent(object eventLayerKey, MessageEventCallback frameDown = null, MessageEventCallback frameUp = null, IEnumerable<KeyboardButtonCode> keys = null, IEnumerable<MouseButtonCode> mouseButtons = null)
            :this(eventLayerKey)
        {
            FirstFrameDownEvent += frameDown;
            FirstFrameUpEvent += frameUp;
            if (keys != null)
            {
                foreach (KeyboardButtonCode key in keys)
                {
                    keyboardButtons.Add(key);
                }
            }
            if (mouseButtons != null)
            {
                foreach (MouseButtonCode mouseButton in mouseButtons)
                {
                    this.mouseButtons.Add(mouseButton);
                }
            }
        }

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
        public bool HeldDown { get; private set; }

        /// <summary>
        /// Returns true if the event has been released more thean 1 frame.
        /// </summary>
        public bool ReleasedUp { get; private set; }

        /// <summary>
        /// Returns true if the event is down, first frame or held.
        /// </summary>
        public bool Down
        {
            get
            {
                return HeldDown || FirstFrameDown;
            }
        }

        /// <summary>
        /// Returns true if the event is up, first frame or released.
        /// </summary>
        public bool Up
        {
            get
            {
                return ReleasedUp || FirstFrameUp;
            }
        }

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
            MouseWheelDirection = MouseWheelDirection.None;
        }

        /// <summary>
        /// Determine the direction the mouse wheel should be moving on the frame this event fires. By default it is None,
        /// which means the mouse wheel is not involved in this event.
        /// </summary>
        public MouseWheelDirection MouseWheelDirection { get; set; }

        public String KeyDescription
        {
            get
            {
                String keyFormat = "{0}";
                StringBuilder sb = new StringBuilder(25);
                foreach(var button in keyboardButtons)
                {
                    sb.AppendFormat(keyFormat, Keyboard.PrettyKeyName(button));
                    keyFormat = "+ {0}";
                }
                foreach (var button in mouseButtons)
                {
                    sb.AppendFormat(keyFormat, Mouse.PrettyButtonName(button));
                    keyFormat = "+ {0}";
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Called internally to manage status of the event.
        /// </summary>
        protected internal override void update(EventLayer eventLayer, bool allowProcessing, Clock clock)
        {
            switch (scanButtons(eventLayer))
            {
                case ButtonScanResult.Down:
                    if (FirstFrameDown)
                    {
                        HeldDown = true;
                        FirstFrameDown = false;
                    }
                
                    if(HeldDown)
                    {
                        if (OnHeldDown != null)
                        {
                            OnHeldDown.Invoke(eventLayer);
                        }
                        if (OnDown != null)
                        {
                            OnDown.Invoke(eventLayer);
                        }
                    }
                    else
                    {
                        FirstFrameDown = true;
                        if (FirstFrameDownEvent != null)
                        {
                            FirstFrameDownEvent.Invoke(eventLayer);
                        }
                        if(OnDown != null)
                        {
                            OnDown.Invoke(eventLayer);
                        }
                    }
                    FirstFrameUp = false;
                    ReleasedUp = false;
                    break;

                case ButtonScanResult.Up:
                    if (FirstFrameUp)
                    {
                        ReleasedUp = true;
                        FirstFrameUp = false;
                    }
                    else if (!ReleasedUp)
                    {
                        FirstFrameUp = true;
                        if (FirstFrameUpEvent != null)
                        {
                            FirstFrameUpEvent.Invoke(eventLayer);
                        }
                    }
                    FirstFrameDown = false;
                    HeldDown = false;
                    break;

                case ButtonScanResult.DownAndUpThisFrame:
                    FirstFrameDown = true;
                    if(FirstFrameDownEvent != null)
                    {
                        FirstFrameDownEvent.Invoke(eventLayer);
                    }
                    FirstFrameUp = true;
                    FirstFrameDown = false;
                    if(FirstFrameUpEvent != null)
                    {
                        FirstFrameUpEvent.Invoke(eventLayer);
                    }
                    ReleasedUp = false;
                    HeldDown = false;
                    break;
            }
        }

        enum ButtonScanResult
        {
            Up = 0,
            Down = 1,
            DownAndUpThisFrame = 2,
        }

        /// <summary>
        /// Internal method to make sure all the buttons for the event are pressed.
        /// </summary>
        /// <param name="eventManager">The event manager with the keyboard and mouse being used.</param>
        /// <returns>True if all the required buttons are pressed.</returns>
        private ButtonScanResult scanButtons(EventLayer eventLayer)
        {
            Mouse mouse = eventLayer.Mouse;
            Keyboard keyboard = eventLayer.Keyboard;
            bool allActivated = keyboardButtons.Count + mouseButtons.Count > 0 || MouseWheelDirection > 0;
            bool anyDownUpThisFrame = false;
            bool currentDownUpThisFrame;
            if (allActivated)
            {
                foreach (KeyboardButtonCode keyCode in keyboardButtons)
                {
                    allActivated &= keyboard.isKeyDown(keyCode);
                    if (!allActivated)
                    {
                        break;
                    }
                }
                if (allActivated)
                {
                    foreach (MouseButtonCode mouseCode in mouseButtons)
                    {
                        currentDownUpThisFrame = mouse.buttonDownThisFrame(mouseCode);
                        allActivated &= (mouse.buttonDown(mouseCode) || currentDownUpThisFrame);
                        if (!allActivated)
                        {
                            break;
                        }
                        anyDownUpThisFrame |= currentDownUpThisFrame;
                    }
                }
                if (allActivated)
                {
                    switch(MouseWheelDirection)
                    {
                        case MouseWheelDirection.Up:
                            allActivated &= mouse.RelativePosition.z > 0;
                            break;
                        case MouseWheelDirection.Down:
                            allActivated &= mouse.RelativePosition.z < 0;
                            break;
                    }
                }
            }
            if(allActivated)
            {
                if(anyDownUpThisFrame)
                {
                    return ButtonScanResult.DownAndUpThisFrame;
                }
                return ButtonScanResult.Down;
            }
            else
            {
                return ButtonScanResult.Up;
            }
        }
    }
}
