using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;
using Logging;

namespace CEGUIPlugin
{
    class CEGUIUpdate : UpdateListener
    {
        CEGUISystem system;
        EventManager eventManager;
        bool[] mouseButtonsDown = new bool[(int)MouseButton.MouseButtonCount];
        bool[] keysDown = new bool[256];

        public CEGUIUpdate(CEGUISystem system, EventManager eventManager)
        {
            this.system = system;
            this.eventManager = eventManager;
        }

        public void sendUpdate(Clock clock)
        {
            system.injectTimePulse(clock.fSeconds);
            Mouse mouse = eventManager.Mouse;
            Vector3 mousePos = mouse.getAbsMouse();
            system.injectMousePosition(mousePos.x, mousePos.y);
            for(int i = 0; i < mouseButtonsDown.Length; i++)
            {
                bool down = mouse.buttonDown((MouseButtonCode)i);
                if(down != mouseButtonsDown[i])
                {
                    if(down)
                    {
                        system.injectMouseButtonDown((MouseButton)i);
                    }
                    else
                    {
                        system.injectMouseButtonUp((MouseButton)i);
                    }
                    mouseButtonsDown[i] = down;
                }
            }

            Keyboard keyboard = eventManager.Keyboard;
            for (uint i = 0; i < keysDown.Length; i++)
            {
                bool down = keyboard.isKeyDown((KeyboardButtonCode)i);
                if (down != keysDown[i])
                {
                    if (down)
                    {
                        system.injectKeyDown(i);
                        uint key = (uint)keyboard.translateText((KeyboardButtonCode)i);
                        if(key != 0)
                        {
                            system.injectChar(key);
                        }
                    }
                    else
                    {
                        system.injectKeyUp(i);
                    }
                    keysDown[i] = down;
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
