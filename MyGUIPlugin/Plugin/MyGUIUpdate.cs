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

        

        public MyGUIUpdate(Gui system, EventManager eventManager)
        {
            this.gui = system;
            this.eventManager = eventManager;
            Keyboard keyboard = eventManager.Keyboard;
            keyboard.KeyPressed += new KeyEvent(keyboard_KeyPressed);
            keyboard.KeyReleased += new KeyEvent(keyboard_KeyReleased);
        }

        public void sendUpdate(Clock clock)
        {
            float time = clock.fSeconds;
            Gui.Instance.fireUpdateEvent(time);

            //Mouse
            gui.HandledMouseButtons = false;
            Mouse mouse = eventManager.Mouse;
            Vector3 mousePos = mouse.getAbsMouse();
            gui.injectMouseMove((int)mousePos.x, (int)mousePos.y, (int)mousePos.z);
            for (int i = 0; i < mouseButtonsDown.Length; i++)
            {
                bool down = mouse.buttonDown((MouseButtonCode)i);
                if (down != mouseButtonsDown[i])
                {
                    if (down)
                    {
                        if (gui.injectMousePress((int)mousePos.x, (int)mousePos.y, (MouseButtonCode)i))
                        {
                            gui.HandledMouseButtons = true;
                        }
                    }
                    else
                    {
                        if (gui.injectMouseRelease((int)mousePos.x, (int)mousePos.y, (MouseButtonCode)i))
                        {
                            gui.HandledMouseButtons = true;
                        }
                    }
                    mouseButtonsDown[i] = down;
                }
            }
        }

        void keyboard_KeyReleased(KeyboardButtonCode keyCode, uint keyChar)
        {
            gui.injectKeyRelease(keyCode);
        }

        void keyboard_KeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            gui.injectKeyPress(keyCode, keyChar);
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }
    }
}
