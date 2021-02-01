using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Engine.Platform.Input;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<EventManager> logger;
        private KeyboardHardware keyboardHardware = null;
        private Keyboard keyboard;
        private MouseHardware mouseHardware = null;
        private Mouse mouse;
        private Touches touches;
        private TouchHardware touchHardware;
        private Unproject unprojectFunc;

        private EventLayer focusLayer;
        private FastIteratorMap<Object, EventLayer> eventLayers = new FastIteratorMap<object, EventLayer>();

        private Gamepad pad1;
        private Gamepad pad2;
        private Gamepad pad3;
        private Gamepad pad4;
        private GamepadHardware pad1Hardware;
        private GamepadHardware pad2Hardware;
        private GamepadHardware pad3Hardware;
        private GamepadHardware pad4Hardware;

        /// <summary>
        /// Constructor takes the input handler to use and an enumearble over the layer keys in order that they should be processed
        /// </summary>
        /// <param name="inputHandler">The input handler to use.</param>
        public EventManager(InputHandler inputHandler, IEnumerable layerKeys, ILogger<EventManager> logger)
        {
            this.inputHandler = inputHandler;
            this.logger = logger;
            keyboard = new Keyboard();
            keyboardHardware = inputHandler.createKeyboard(keyboard);
            mouse = new Mouse(this);
            mouseHardware = inputHandler.createMouse(mouse);
            touches = new Touches();
            touchHardware = inputHandler.createTouchHardware(touches);

            pad1 = new Gamepad(this, GamepadId.Pad1);
            pad2 = new Gamepad(this, GamepadId.Pad2);
            pad3 = new Gamepad(this, GamepadId.Pad3);
            pad4 = new Gamepad(this, GamepadId.Pad4);
            pad1Hardware = inputHandler.createGamepad(pad1);
            pad2Hardware = inputHandler.createGamepad(pad2);
            pad3Hardware = inputHandler.createGamepad(pad3);
            pad4Hardware = inputHandler.createGamepad(pad4);

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
            inputHandler.destroyGamepad(pad1Hardware);
            inputHandler.destroyGamepad(pad2Hardware);
            inputHandler.destroyGamepad(pad3Hardware);
            inputHandler.destroyGamepad(pad4Hardware);
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

            pad1Hardware.Update();
            pad2Hardware.Update();
            pad3Hardware.Update();
            pad4Hardware.Update();
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
            EventLayer layer;
            if (eventLayers.TryGetValue(messageEvent.EventLayerKey, out layer))
            {
                layer.addEvent(messageEvent);
            }
            else
            {
                logger.LogWarning($"Could not bind message event to layer {messageEvent.EventLayerKey}");
            }
        }

        /// <summary>
        /// Remove a MessageEvent from this event manager.
        /// </summary>
        /// <param name="messageEvent"></param>
        public void removeEvent(MessageEvent messageEvent)
        {
            EventLayer layer;
            if (eventLayers.TryGetValue(messageEvent.EventLayerKey, out layer))
            {
                layer.removeEvent(messageEvent);
            }
            else
            {
                logger.LogWarning($"Could not unbind message event to layer {messageEvent.EventLayerKey}");
            }
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
            if (focusLayer == layer)
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

        public Gamepad Pad1
        {
            get
            {
                return pad1;
            }
        }

        public Gamepad Pad2
        {
            get
            {
                return pad2;
            }
        }

        public Gamepad Pad3
        {
            get
            {
                return pad3;
            }
        }

        public Gamepad Pad4
        {
            get
            {
                return pad4;
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
