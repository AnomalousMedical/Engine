using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;
using Logging;

namespace MyGUIPlugin
{
    internal delegate void MouseEvent(int x, int y, MouseButtonCode button);

    class MyGUIUpdate : UpdateListener
    {
        static MyGUIUpdate instance;

        static public MyGUIUpdate Instance
        {
            get
            {
                return instance;
            }
        }

        Gui gui;
        EventManager eventManager;
        bool[] mouseButtonsDown = new bool[(int)MouseButtonCode.NUM_BUTTONS];

        internal event MouseEvent MouseButtonPressed;
        internal event MouseEvent MouseButtonReleased;

        public MyGUIUpdate(Gui system, EventManager eventManager)
        {
            if (instance == null)
            {
                instance = this;
                this.gui = system;
                this.eventManager = eventManager;
                Keyboard keyboard = eventManager.Keyboard;
                keyboard.KeyPressed += new KeyEvent(keyboard_KeyPressed);
                keyboard.KeyReleased += new KeyEvent(keyboard_KeyReleased);
            }
            else
            {
                throw new Exception("Can only have one instance of the MyGUIUpdate class");
            }
        }

        public void sendUpdate(Clock clock)
        {
            float time = clock.fSeconds;

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
                        if (MouseButtonPressed != null)
                        {
                            MouseButtonPressed.Invoke((int)mousePos.x, (int)mousePos.y, (MouseButtonCode)i);
                        }
                    }
                    else
                    {
                        if (gui.injectMouseRelease((int)mousePos.x, (int)mousePos.y, (MouseButtonCode)i))
                        {
                            gui.HandledMouseButtons = true;
                        }
                        if (MouseButtonReleased != null)
                        {
                            MouseButtonReleased.Invoke((int)mousePos.x, (int)mousePos.y, (MouseButtonCode)i);
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
