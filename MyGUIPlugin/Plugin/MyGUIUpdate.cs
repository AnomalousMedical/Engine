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
        EventManager eventManager;
        bool[] mouseButtonsDown = new bool[(int)MouseButtonCode.NUM_BUTTONS];
        InputManager inputManager;

        public MyGUIUpdate(Gui system, EventManager eventManager)
        {
            this.gui = system;
            this.eventManager = eventManager;
            this.inputManager = InputManager.Instance;
            Keyboard keyboard = eventManager.Keyboard;
            keyboard.KeyPressed += new KeyEvent(keyboard_KeyPressed);
            keyboard.KeyReleased += new KeyEvent(keyboard_KeyReleased);

            Mouse mouse = eventManager.Mouse;
            mouse.ButtonDown += new MouseCallback(mouse_ButtonDown);
            mouse.ButtonUp += new MouseCallback(mouse_ButtonUp);
            mouse.Moved += new MouseCallback(mouse_Moved);
        }

        public void sendUpdate(Clock clock)
        {
            PerformanceMonitor.start("MyGUI");
            float time = clock.DeltaSeconds;
            gui.frameEvent(time);
            PerformanceMonitor.stop("MyGUI");
        }

        void mouse_Moved(Mouse mouse, MouseButtonCode buttonCode)
        {
            Vector3 mousePos = mouse.getAbsMouse();
            inputManager.injectMouseMove((int)mousePos.x, (int)mousePos.y, (int)mousePos.z);
        }

        void mouse_ButtonUp(Mouse mouse, MouseButtonCode buttonCode)
        {
            Vector3 mousePos = mouse.getAbsMouse();
            gui.HandledMouseButtons = inputManager.injectMouseRelease((int)mousePos.x, (int)mousePos.y, buttonCode);
        }

        void mouse_ButtonDown(Mouse mouse, MouseButtonCode buttonCode)
        {
            Vector3 mousePos = mouse.getAbsMouse();
            gui.HandledMouseButtons = inputManager.injectMousePress((int)mousePos.x, (int)mousePos.y, buttonCode);
        }

        void keyboard_KeyReleased(KeyboardButtonCode keyCode, uint keyChar)
        {
            gui.HandledKeyboardButtons = inputManager.injectKeyRelease(keyCode);
        }

        void keyboard_KeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            gui.HandledKeyboardButtons = inputManager.injectKeyPress(keyCode, keyChar);
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }
    }
}
