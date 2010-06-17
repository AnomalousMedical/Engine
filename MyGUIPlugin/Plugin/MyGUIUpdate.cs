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
        bool[] keysDown = new bool[256];
        float[] keysDownTime = new float[256]; //used for auto repeat
        bool[] keyQuickRepeat = new bool[256];
        private const float START_REPEAT_TIME = 0.5f;
        private const float QUICK_REPEAT_TIME = 0.03f;

        public MyGUIUpdate(Gui system, EventManager eventManager)
        {
            this.gui = system;
            this.eventManager = eventManager;
        }

        public void sendUpdate(Clock clock)
        {
            float time = clock.fSeconds;

            //Mouse
            Mouse mouse = eventManager.Mouse;
            Vector3 mousePos = mouse.getAbsMouse();
            gui.injectMouseMove((int)mousePos.x, (int)mousePos.y, (int)mousePos.z);
            for(int i = 0; i < mouseButtonsDown.Length; i++)
            {
                bool down = mouse.buttonDown((MouseButtonCode)i);
                if(down != mouseButtonsDown[i])
                {
                    if(down)
                    {
                        gui.injectMousePress((int)mousePos.x, (int)mousePos.y, (MouseButtonCode)i);
                    }
                    else
                    {
                        gui.injectMouseRelease((int)mousePos.x, (int)mousePos.y, (MouseButtonCode)i);
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
                        uint key = (uint)keyboard.translateText((KeyboardButtonCode)i);
                        gui.injectKeyPress((KeyboardButtonCode)i, key);
                    }
                    else
                    {
                        gui.injectKeyRelease((KeyboardButtonCode)i);
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
