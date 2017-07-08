using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    /// <summary>
    /// This class handles the default events for the system.  All classes that create events
    /// can register them with this DefaultEvents class.  Since static variables in some classes
    /// may not be registered until the event managers have been created this class includes
    /// two events that alert any subscribers that new events have been detected and should be
    /// processed.
    /// 
    /// All classes that use a certain event should add it to the DefaultEvents in a static 
    /// constructor.  If an event is added more than once it will be ignored so it is not a problem
    /// to register an event multiple times.
    /// </summary>
    public class DefaultEvents
    {
        /// <summary>
        /// This delegate is used to fire new event detected events.  This is called whenever
        /// a new event is registered in the DefaultEvents class.
        /// </summary>
        /// <param name="evt"></param>
        public delegate void NewEventDetected(MessageEvent evt);

        /// <summary>
        /// This is fired when new events have been detected.
        /// </summary>
        static event NewEventDetected OnNewEventDetected;

        static HashSet<MessageEvent> events = new HashSet<MessageEvent>();

        /// <summary>
        /// Add a default event to the list of default events.  If the user has defined
        /// key settings that are different from the default provided they will be overwritten
        /// with the user settings in this function.
        /// </summary>
        /// <param name="evt">The event to register.</param>
        [Obsolete("Don't use DefaultEvents anymore, inject the EventManager when building your controls.")]
        public static void registerDefaultEvent(MessageEvent evt)
        {
            events.Add(evt);
            if (OnNewEventDetected != null)
            {
                OnNewEventDetected.Invoke(evt);
            }
        }

        /// <summary>
        /// Setup an EventManager with all identified default events and subscribe it
        /// to the new events found callbacks to make sure it identifies any new events
        /// found.
        /// </summary>
        /// <param name="eventManager"></param>
        public static void registerEventManager(EventManager eventManager)
        {
            foreach (MessageEvent evt in events)
            {
                eventManager.addEvent(evt);
            }

            DefaultEvents.OnNewEventDetected += eventManager.addEvent;
        }

        /// <summary>
        /// Cleanup the callbacks to the event manager because it is no longer needed.
        /// </summary>
        /// <param name="eventManager">The event manager to unregister.</param>
        public static void unregisterEventManager(EventManager eventManager)
        {
            DefaultEvents.OnNewEventDetected -= eventManager.addEvent;
        }
    }
}
