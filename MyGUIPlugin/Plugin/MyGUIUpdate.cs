using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;
using Logging;

namespace MyGUIPlugin
{
    class MyGUIUpdate : UpdateListener
    {
        Gui gui;
        bool[] mouseButtonsDown = new bool[(int)MouseButtonCode.NUM_BUTTONS];
        InputManager inputManager;
        EventLayer eventLayer;

        public MyGUIUpdate(Gui system, EventLayer eventLayer)
        {
            this.gui = system;
            this.inputManager = InputManager.Instance;
            this.eventLayer = eventLayer;
            Keyboard keyboard = eventLayer.Keyboard;
            keyboard.KeyPressed += keyboard_KeyPressed;
            keyboard.KeyReleased += keyboard_KeyReleased;

            Mouse mouse = eventLayer.Mouse;
            mouse.ButtonDown += mouse_ButtonDown;
            mouse.ButtonUp += mouse_ButtonUp;
            mouse.Moved += mouse_Moved;
        }

        public void sendUpdate(Clock clock)
        {
            PerformanceMonitor.start("MyGUI");
            float time = clock.DeltaSeconds;
            gui.frameEvent(time);
            PerformanceMonitor.stop("MyGUI");
        }

        void mouse_Moved(Mouse mouse)
        {
            if (eventLayer.EventProcessingAllowed)
            {
                IntVector3 mousePos = mouse.AbsolutePosition;
                gui.HandledMouseMove = inputManager.injectMouseMove(mousePos.x, mousePos.y, mousePos.z);
                if (gui.HandledMouseMove || inputManager.isModalAny())
                {
                    eventLayer.alertEventsHandled();
                }
            }
        }

        void mouse_ButtonUp(Mouse mouse, MouseButtonCode buttonCode)
        {
            if (eventLayer.EventProcessingAllowed)
            {
                IntVector3 mousePos = mouse.AbsolutePosition;
                gui.HandledMouseButtons = inputManager.injectMouseRelease(mousePos.x, mousePos.y, buttonCode);
                if (gui.HandledMouseButtons || inputManager.isModalAny())
                {
                    eventLayer.alertEventsHandled();
                }
                eventLayer.Locked = mouse.AnyButtonsDown;
            }
        }

        void mouse_ButtonDown(Mouse mouse, MouseButtonCode buttonCode)
        {
            if (eventLayer.EventProcessingAllowed)
            {
                IntVector3 mousePos = mouse.AbsolutePosition;
                gui.HandledMouseButtons = inputManager.injectMousePress(mousePos.x, mousePos.y, buttonCode);
                if (gui.HandledMouseButtons || inputManager.isModalAny())
                {
                    eventLayer.alertEventsHandled();
                    eventLayer.Locked = true;
                }
            }
        }

        void keyboard_KeyReleased(KeyboardButtonCode keyCode)
        {
            if (eventLayer.EventProcessingAllowed)
            {
                gui.HandledKeyboardButtons = inputManager.injectKeyRelease(keyCode);
                if (gui.HandledKeyboardButtons || inputManager.isModalAny())
                {
                    eventLayer.alertEventsHandled();
                }
            }
        }

        void keyboard_KeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            if (eventLayer.EventProcessingAllowed)
            {
                gui.HandledKeyboardButtons = inputManager.injectKeyPress(keyCode, keyChar);
                if (gui.HandledKeyboardButtons || inputManager.isModalAny())
                {
                    eventLayer.alertEventsHandled();
                }
            }
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }
    }
}
