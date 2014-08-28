using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    public class EventLayer
    {
        //Events stored in a dictionary for fast lookup.
        private Dictionary<object, MessageEvent> events = new Dictionary<object, MessageEvent>();
        //Events stored in a list for fast iteration.
        private List<MessageEvent> eventList = new List<MessageEvent>();

        internal EventLayer(EventManager eventManager, KeyboardHardware keyboardHardware, MouseHardware mouseHardware)
        {
            this.EventManager = eventManager;
            Keyboard = new Keyboard(keyboardHardware);
            Mouse = new Mouse(mouseHardware);
        }

        /// <summary>
        /// Add an event to this event layer.
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
        /// Remove an event from this event layer.
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
        /// Determine if the layer already identifies the given event.
        /// </summary>
        /// <param name="evt">The event to check for.</param>
        /// <returns>True if the container contains the event.  False if it does not.</returns>
        public bool containsEvent(object evt)
        {
            return events.ContainsKey(evt);
        }

        /// <summary>
        /// Call this function to alert the Layer that one of its events has been handled. This will prevent
        /// lower layers from having events with down status and will force them to up.
        /// </summary>
        public void alertEventsHandled()
        {
            HandledEvents = true;
        }

        internal void update(bool allowProcessing)
        {
            foreach (MessageEvent evt in eventList)
            {
                evt.update(this, allowProcessing);
            }
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

        public bool HandledEvents { get; internal set; }

        public EventManager EventManager { get; private set; }

        public Mouse Mouse { get; private set; }

        public Keyboard Keyboard { get; private set; }
    }
}
