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
        float[] keysDownTime = new float[256]; //used for auto repeat
        bool[] keyQuickRepeat = new bool[256];
        private const float START_REPEAT_TIME = 0.5f;
        private const float QUICK_REPEAT_TIME = 0.03f;

        public CEGUIUpdate(CEGUISystem system, EventManager eventManager)
        {
            this.system = system;
            this.eventManager = eventManager;
        }

        public void sendUpdate(Clock clock)
        {
            float time = clock.fSeconds;
            system.injectTimePulse(time);

            //Mouse
            Mouse mouse = eventManager.Mouse;
            Vector3 mousePos = mouse.getAbsMouse();
            system.injectMousePosition(mousePos.x, mousePos.y);
            Vector3 mouseRel = mouse.getRelMouse();
            system.injectMouseWheelChange(mouseRel.z / 120.0f);
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

            //Keyboard
            Keyboard keyboard = eventManager.Keyboard;
            for (uint i = 0; i < keysDown.Length; i++)
            {
                bool down = keyboard.isKeyDown((KeyboardButtonCode)i);

                //Check auto repeat
                if(down && keysDown[i])
                {
                    keysDownTime[i] += time;
                    if (keyQuickRepeat[i])
                    {
                        if (keysDownTime[i] > QUICK_REPEAT_TIME)
                        {
                            keysDownTime[i] = 0.0f;
                            keysDown[i] = false;
                        }
                    }
                    else
                    {
                        if (keysDownTime[i] > START_REPEAT_TIME)
                        {
                            keysDownTime[i] = 0.0f;
                            keysDown[i] = false;
                            keyQuickRepeat[i] = true;
                        }
                    }
                }

                //Alert CEGUI
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
                        keyQuickRepeat[i] = false;
                        keysDownTime[i] = 0.0f;
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
