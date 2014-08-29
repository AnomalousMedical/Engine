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
            HandledEvents = false;
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

        /// <summary>
        /// Update this layer, allowProcessing determines if the 
        /// </summary>
        /// <param name="allowProcessing"></param>
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

        /// <summary>
        /// The EventManager this layer is a part of.
        /// </summary>
        public EventManager EventManager { get; private set; }

        /// <summary>
        /// Access to the Mouse for this layer, the events on that object will respect the event layer stack so if a higher
        /// layer says that it has already used the mouse subscribers to this layer's mouse will not get any events.
        /// 
        /// This is provided more for convenience, events subscribed here are not guarenteed to get the full down / pressed / up
        /// lifecycle that MessageEvents get, this is more to hook up to a subsystem that might need direct mouse events, like a gui.
        /// 
        /// Ideally only one EventLayer would subscribe to these events and it would be the topmost one (e.g. a gui library)
        /// </summary>
        public Mouse Mouse { get; private set; }

        /// <summary>
        /// Access to Keyboard Mouse for this layer, the events on that object will respect the event layer stack so if a higher
        /// layer says that it has already used the keyboard subscribers to this layer's keyboard will not get any events.
        /// 
        /// This is provided more for convenience, events subscribed here are not guarenteed to get the full down / pressed / up
        /// lifecycle that MessageEvents get, this is more to hook up to a subsystem that might need direct mouse events, like a gui.
        /// 
        /// Ideally only one EventLayer would subscribe to these events and it would be the topmost one (e.g. a gui library)
        /// </summary>
        public Keyboard Keyboard { get; private set; }

        /// <summary>
        /// An internal property to track if this layer has handled its events. Do not make this public it has no real meaning
        /// beyond the EventManager's processing loop.
        /// </summary>
        internal bool HandledEvents { get; set; }
    }
}
