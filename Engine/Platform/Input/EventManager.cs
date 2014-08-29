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
        private KeyboardHardware keyboard = null;
        private MouseHardware mouse = null;
        
        private List<EventLayer> eventLayers = new List<EventLayer>();
        //Temporary
        EventLayer defaultLayer;

        internal NewEventDetected EventDetected { get; private set; }

        /// <summary>
        /// Constructor takes the input handler to use.
        /// </summary>
        /// <param name="inputHandler">The input handler to use.</param>
        public EventManager(InputHandler inputHandler)
        {
            EventDetected = new NewEventDetected(addEvent);
            this.inputHandler = inputHandler;
            keyboard = inputHandler.createKeyboard(true, this);
            mouse = inputHandler.createMouse(false, this);
            DefaultEvents.registerEventManager(this);

            //Temp
            defaultLayer = new EventLayer(this, keyboard, mouse);
            eventLayers.Add(defaultLayer);
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
            keyboard = inputHandler.createKeyboard(true, this);
            mouse = inputHandler.createMouse(false, this);
        }

        public EventLayer addEventLayer(int index = int.MaxValue)
        {
            EventLayer eventLayer = new EventLayer(this, keyboard, mouse);
            if(index < eventLayers.Count)
            {
                eventLayers.Insert(index, eventLayer);
            }
            else
            {
                eventLayers.Add(eventLayer);
            }
            return eventLayer;
        }

        public void removeEventLayer(EventLayer eventLayer)
        {
            eventLayers.Remove(eventLayer);
        }

        /// <summary>
        /// Add an event to this event manager.
        /// </summary>
        /// <param name="evt">The event to add.</param>
        public void addEvent(MessageEvent evt)
        {
            defaultLayer.addEvent(evt);
        }

        /// <summary>
        /// Remove an event from this event manager.
        /// </summary>
        /// <param name="evt">The event to remove.</param>
        public void removeEvent(MessageEvent evt)
        {
            defaultLayer.removeEvent(evt);
        }

        /// <summary>
        /// Determine if the container already identifies the given event.
        /// </summary>
        /// <param name="evt">The event to check for.</param>
        /// <returns>True if the container contains the event.  False if it does not.</returns>
        public bool containsEvent(object evt)
        {
            return defaultLayer.containsEvent(evt);
        }

        /// <summary>
        /// Called to capture input and manage events.
        /// </summary>
        /// <param name="time">The clock with info about this frame.</param>
        public void updateEvents(Clock clock)
        {
            if (mouse != null)
            {
                mouse.capture();
            }
            if (keyboard != null)
            {
                keyboard.capture();
            }
            bool allowEventProcessing = true; //The first layer always gets all events
            foreach (var layer in eventLayers)
            {
                //First try to update the layer with the current allowEventProcessing
                layer.update(allowEventProcessing);

                //Modify allowEventProcessing as needed, if we have already set allowEventProcessing to false it should stay false
                allowEventProcessing = allowEventProcessing && !layer.HandledEvents;

                //Reset the layer's HandledEvents for this frame, this is done last to reset without iterating again.
                //Doing this last also ensures that if we stopped early on one of the fire events below that that layer
                //keeps its HandledEvents property set where it should be.
                layer.HandledEvents = false;
            }
        }

        /// <summary>
        /// Fire a key pressed event.
        /// </summary>
        internal void fireKeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            //See UpdateEvents for how HandledEvents is processed
            foreach (var layer in eventLayers)
            {
                layer.Keyboard.fireKeyPressed(keyCode, keyChar);
                if(layer.HandledEvents)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Fire a key released event.
        /// </summary>
        internal void fireKeyReleased(KeyboardButtonCode keyCode, uint keyChar)
        {
            //See UpdateEvents for how HandledEvents is processed
            foreach (var layer in eventLayers)
            {
                layer.Keyboard.fireKeyReleased(keyCode, keyChar);
                if (layer.HandledEvents)
                {
                    break;
                }
            }
        }

        internal void fireButtonDown(MouseButtonCode button)
        {
            //See UpdateEvents for how HandledEvents is processed
            foreach (var layer in eventLayers)
            {
                layer.Mouse.fireButtonDown(button);
                if (layer.HandledEvents)
                {
                    break;
                }
            }
        }

        internal void fireButtonUp(MouseButtonCode button)
        {
            //See UpdateEvents for how HandledEvents is processed
            foreach (var layer in eventLayers)
            {
                layer.Mouse.fireButtonUp(button);
                if (layer.HandledEvents)
                {
                    break;
                }
            }
        }

        internal void fireMoved()
        {
            //See UpdateEvents for how HandledEvents is processed
            foreach (var layer in eventLayers)
            {
                layer.Mouse.fireMoved();
                if (layer.HandledEvents)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Returns the number of events in this container.
        /// </summary>
        public int Count
        {
            get
            {
                return defaultLayer.Count;
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
                return defaultLayer[index];
            }
        }

        //TEMPORARY
        public EventLayer DefaultEventLayer
        {
            get
            {
                return defaultLayer;
            }
        }
    }
}
