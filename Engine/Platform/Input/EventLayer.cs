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
        private List<MessageEvent> eventList = new List<MessageEvent>();

        /// <summary>
        /// This event is fired when the EventLayer is updated. This will happen every frame
        /// that the eventManager is processed.
        /// </summary>
        public event MessageEventCallback OnUpdate;

        internal EventLayer(EventManager eventManager, KeyboardHardware keyboardHardware, MouseHardware mouseHardware)
        {
            this.EventManager = eventManager;
            Keyboard = new Keyboard(keyboardHardware);
            HandledEvents = false;
            Locked = false;
        }

        /// <summary>
        /// Add an event to this event layer.
        /// </summary>
        /// <param name="evt">The event to add.</param>
        public void addEvent(MessageEvent evt)
        {
            eventList.Add(evt);
        }

        /// <summary>
        /// Remove an event from this event layer.
        /// </summary>
        /// <param name="evt">The event to remove.</param>
        public void removeEvent(MessageEvent evt)
        {
            eventList.Remove(evt);
        }

        /// <summary>
        /// Call this function to alert the Layer that one of its events has been handled. This will prevent
        /// lower layers from having events with down status and will force them to up.
        /// </summary>
        /// <remarks>
        /// This will only cause the event layers to skip the remaining elements in the stack if it is called during a MessageEvent handler
        /// like FirstFrameDownEvent, FirstFrameUpEvent, OnHeldDown or OnDown or during the OnUpdate call from
        /// this layer, otherwise it is likely that the event stack will be processed completely before you are able
        /// to call this function.
        /// </remarks>
        public void alertEventsHandled()
        {
            HandledEvents = true;
        }

        /// <summary>
        /// Update this layer, allowProcessing will be true if events should process.
        /// </summary>
        /// <param name="allowProcessing"></param>
        internal void update(bool allowProcessing, Clock clock)
        {
            EventProcessingAllowed = allowProcessing;
            foreach (MessageEvent evt in eventList)
            {
                evt.update(this, allowProcessing, clock);
            }
            if(OnUpdate != null)
            {
                OnUpdate.Invoke(this);
            }
        }

        /// <summary>
        /// Returns the number of events in this container.
        /// </summary>
        public int Count
        {
            get
            {
                return eventList.Count;
            }
        }

        /// <summary>
        /// The EventManager this layer is a part of.
        /// </summary>
        public EventManager EventManager { get; private set; }

        /// <summary>
        /// This will be true if the events fired by this layer have not already had their input handled by a higher layer. This is
        /// really important to check if you want the stack to actually work since all layers will process events and fire them no matter
        /// if event processing is allowed or not (this makes tracking down up etc consistant otherwise there are lots of problems).
        /// To determine if you should actually do something with the event you are getting make sure this is true.
        /// </summary>
        public bool EventProcessingAllowed { get; set; }

        /// <summary>
        /// Access the Mouse. The events fired by the mouse class are not layered so anything subscribed will get them, however,
        /// you can call the alertEventsProcessed function when handling the mouse, which will cause that layer to block lower layers
        /// during the next update.
        /// </summary>
        public Mouse Mouse
        {
            get
            {
                return EventManager.Mouse;
            }
        }

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
        /// Set this to true to lock input to not go past this layer. Set this to false to allow input past this layer again.
        /// This is likely not to be used commonly since the stack will take care of these issues mostly, however, for layers
        /// that deal directly with mouse and keyboard input (like a gui) this will be useful since HandledEvents is reset every
        /// frame, but a Mouse clicked or moved event may not fire each frame.
        /// </summary>
        /// <remarks>
        /// This will only cause the event layers to skip the remaining elements in the stack if it is set during a MessageEvent handler
        /// like FirstFrameDownEvent, FirstFrameUpEvent, OnHeldDown or OnDown or during the OnUpdate call from
        /// this layer, otherwise it is likely that the event stack will be processed completely before you are able
        /// to call this function.
        /// </remarks>
        public bool Locked { get; set; }

        /// <summary>
        /// An internal property to track if this layer has handled its events. Do not make this public it has no real meaning
        /// beyond the EventManager's processing loop.
        /// </summary>
        internal bool HandledEvents { get; set; }

        /// <summary>
        /// This property determines if the next event layer should be processed. It will be true if Locked or HandledEvents are true.
        /// </summary>
        internal bool SkipNextLayer
        {
            get
            {
                return HandledEvents || Locked;
            }
        }
    }
}
