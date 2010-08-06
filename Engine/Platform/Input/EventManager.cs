using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;

namespace Engine.Platform
{
    /// <summary>
    /// The event manager tracks input from the keyboard and mouse and tracks this
    /// information in Events that abstract the physical button press from the event
    /// being fired.  Events can be named with any object.  Ideally this will be one or
    /// more enums that can be easily reproduced.
    /// </summary>
    public class EventManager : IDisposable
    {
        private InputHandler inputHandler;
        private Keyboard keyboard = null;
        private Mouse mouse = null;
        //Events stored in a dictionary for fast lookup.
        private Dictionary<object, MessageEvent> events = new Dictionary<object, MessageEvent>();
        //Events stored in a list for fast iteration.
        private List<MessageEvent> eventList = new List<MessageEvent>();

        internal NewEventDetected EventDetected { get; private set; }

        /// <summary>
        /// Constructor takes the input handler to use.
        /// </summary>
        /// <param name="inputHandler">The input handler to use.</param>
        public EventManager(InputHandler inputHandler)
        {
            EventDetected = new NewEventDetected(addEvent);
            this.inputHandler = inputHandler;
            keyboard = inputHandler.createKeyboard(true);
            mouse = inputHandler.createMouse(false);
            DefaultEvents.registerEventManager(this);
        }

        /// <summary>
        /// Destroyes the created keyboard and mouse.
        /// </summary>
        public void Dispose()
        {
            destroyInputObjects();
            DefaultEvents.unregisterEventManager(this);
        }

        public void destroyInputObjects()
        {
            if (keyboard != null)
            {
                inputHandler.destroyKeyboard(keyboard);
            }
            if (mouse != null)
            {
                inputHandler.destroyMouse(mouse);
            }
        }

        public void changeInputHandler(InputHandler newHandler)
        {
            this.inputHandler = newHandler;
            keyboard = inputHandler.createKeyboard(true);
            mouse = inputHandler.createMouse(false);
        }

        /// <summary>
        /// Add an event to this event manager.
        /// </summary>
        /// <param name="evt">The event to add.</param>
        public void addEvent(MessageEvent evt)
        {
            if (!events.ContainsKey(evt.Name))
            {
                events.Add(evt.Name, evt);
                eventList.Add(evt);
            }
            else
            {
                Log.Default.sendMessage("Attempted to add a duplicate event {0}.  Duplicate ignored.", LogLevel.Warning, "Input", evt.Name.ToString());
            }
        }

        /// <summary>
        /// Remove an event from this event manager.
        /// </summary>
        /// <param name="evt">The event to remove.</param>
        public void removeEvent(MessageEvent evt)
        {
            if (events.ContainsKey(evt.Name))
            {
                events.Remove(evt.Name);
                eventList.Remove(evt);
            }
            else
            {
                Log.Default.sendMessage("Attempted to remove an event {0} that does not exist.  No changes made.", LogLevel.Warning, "Input", evt.Name.ToString());
            }
        }

        /// <summary>
        /// Determine if the container already identifies the given event.
        /// </summary>
        /// <param name="evt">The event to check for.</param>
        /// <returns>True if the container contains the event.  False if it does not.</returns>
        public bool containsEvent(object evt)
        {
            return events.ContainsKey(evt);
        }

        /// <summary>
        /// Returns the number of events in this container.
        /// </summary>
        public int Count
        {
            get
            {
                return events.Count;
            }
        }

        /// <summary>
        /// Index for easy access to events.
        /// </summary>
        /// <param name="index">The event to index.</param>
        /// <returns>The event identified by index.</returns>
        public MessageEvent this[object index]
        {
            get
            {
                return events[index];
            }
        }

        /// <summary>
        /// Called to capture input and manage events.
        /// </summary>
        /// <param name="time">The time since the last call to this function.</param>
        public void updateEvents(double time)
        {
            if (mouse != null)
            {
                mouse.capture();
            }
            if (keyboard != null)
            {
                keyboard.capture();
            }
            foreach (MessageEvent evt in eventList)
            {
                evt.update(this);
            }
        }

        /// <summary>
        /// The keyboard being used for input.
        /// </summary>
        public Keyboard Keyboard
        {
            get { return keyboard; }
        }

        /// <summary>
        /// The mouse being used for input.
        /// </summary>
        public Mouse Mouse
        {
            get { return mouse; }
        }
    }
}
