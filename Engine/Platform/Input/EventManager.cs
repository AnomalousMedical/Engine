using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.Collections;

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
        /// <summary>
        /// Unproject the x, y position of the mouse.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The mouse position in world coords.</returns>
        public delegate Vector3 Unproject(int x, int y);

        private InputHandler inputHandler;
        private KeyboardHardware keyboardHardware = null;
        private Keyboard keyboard;
        private MouseHardware mouseHardware = null;
        private Mouse mouse;
        private Touches touches;
        private TouchHardware touchHardware;
        private Unproject unprojectFunc;

        private EventLayer focusLayer;
        private FastIteratorMap<Object, EventLayer> eventLayers = new FastIteratorMap<object, EventLayer>();

        /// <summary>
        /// Constructor takes the input handler to use and an enumearble over the layer keys in order that they should be processed
        /// </summary>
        /// <param name="inputHandler">The input handler to use.</param>
        public EventManager(InputHandler inputHandler, IEnumerable layerKeys)
        {
            this.inputHandler = inputHandler;
            keyboard = new Keyboard();
            keyboardHardware = inputHandler.createKeyboard(keyboard);
            mouse = new Mouse(this);
            mouseHardware = inputHandler.createMouse(mouse);
            touches = new Touches();
            touchHardware = inputHandler.createTouchHardware(touches);

            foreach (object key in layerKeys)
            {
                eventLayers.Add(key, new EventLayer(this));
            }

            DefaultEvents.registerEventManager(this);
        }

        /// <summary>
        /// Destroyes the created keyboard and mouse.
        /// </summary>
        public void Dispose()
        {
            inputHandler.destroyTouchHardware(touchHardware);
            inputHandler.destroyKeyboard(keyboardHardware);
            inputHandler.destroyMouse(mouseHardware);
            DefaultEvents.unregisterEventManager(this);
        }

        /// <summary>
        /// Called to capture input and manage events. All layers will get events, they must determine if they want
        /// to work based on the EventProcessingAllowed property of their event layer.
        /// </summary>
        /// <param name="time">The clock with info about this frame.</param>
        public void updateEvents(Clock clock)
        {
            //Process the whole update with the same focus layer
            EventLayer currentFocusLayer = focusLayer;

            mouse.capture();
            bool allowEventProcessing = true; //The first layer always gets all events

            //If there is a focus layer, process it first
            if (currentFocusLayer != null)
            {
                allowEventProcessing = processLayer(clock, allowEventProcessing, currentFocusLayer);
            }

            //Process all other layers skipping the focus layer
            foreach (var layer in eventLayers)
            {
                if (layer != currentFocusLayer)
                {
                    allowEventProcessing = processLayer(clock, allowEventProcessing, layer);
                }
            }
            mouse.postUpdate();
            touches.tick();
        }

        /// <summary>
        /// Get the EventLayer specified by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public EventLayer this[Object key]
        {
            get
            {
                return eventLayers[key];
            }
        }

        /// <summary>
        /// Add an event to this EventManager.
        /// </summary>
        /// <param name="messageEvent"></param>
        public void addEvent(MessageEvent messageEvent)
        {
            eventLayers[messageEvent.EventLayerKey].addEvent(messageEvent);
        }

        /// <summary>
        /// Remove a MessageEvent from this event manager.
        /// </summary>
        /// <param name="messageEvent"></param>
        public void removeEvent(MessageEvent messageEvent)
        {
            eventLayers[messageEvent.EventLayerKey].removeEvent(messageEvent);
        }

        /// <summary>
        /// Set the focus layer overriding any existing focus layer.
        /// </summary>
        internal void setFocusLayer(EventLayer layer)
        {
            focusLayer = layer;
        }

        /// <summary>
        /// Clear the focus layer if the passed layer is currently the focus layer, otherwise does nothing.
        /// </summary>
        internal void clearFocusLayer(EventLayer layer)
        {
            if(focusLayer == layer)
            {
                focusLayer = null;
            }
        }

        /// <summary>
        /// The touches tracked by the system.
        /// </summary>
        public Touches Touches
        {
            get
            {
                return touches;
            }
        }

        /// <summary>
        /// The mouse.
        /// </summary>
        public Mouse Mouse
        {
            get
            {
                return mouse;
            }
        }

        /// <summary>
        /// The keyboard.
        /// </summary>
        public Keyboard Keyboard
        {
            get
            {
                return keyboard;
            }
        }

        public Vector3 unproject(int x, int y)
        {
            return unprojectFunc(x, y);
        }

        /// <summary>
        /// Set the unproject function for this event manager.
        /// This should be done by a camera manager.
        /// </summary>
        /// <param name="unprojectFunc"></param>
        public void setUnprojectFunction(Unproject unprojectFunc)
        {
            this.unprojectFunc = unprojectFunc;
        }

        /// <summary>
        /// Process a layer.
        /// </summary>
        private static bool processLayer(Clock clock, bool allowEventProcessing, EventLayer layer)
        {
            //First try to update the layer with the current allowEventProcessing
            layer.update(allowEventProcessing, clock);

            //Modify allowEventProcessing as needed, if we have already set allowEventProcessing to false it should stay false
            allowEventProcessing = allowEventProcessing && !layer.SkipNextLayer;

            //Reset the layer's HandledEvents for this frame, this is done last to reset without iterating again.
            //Doing this last also ensures that if we stopped early on one of the fire events below that that layer
            //keeps its HandledEvents property set where it should be.
            layer.HandledEvents = false;
            return allowEventProcessing;
        }
    }
}
