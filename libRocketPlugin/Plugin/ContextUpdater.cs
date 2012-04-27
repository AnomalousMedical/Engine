using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;

namespace libRocketPlugin
{
    public class ContextUpdater : UpdateListener, IDisposable
    {
        private const float REPEAT_INTERVAL = 0.05f;
        private const float REPEAT_INTERVAL_START = 0.4f;

        private Context context;
        private EventManager eventManager;
        private float lastMouseWheel = 0;
        private KeyIdentifier holdKey = KeyIdentifier.KI_UNKNOWN;
        private ushort holdChar;
        private float repeatTimeout = REPEAT_INTERVAL;

        public ContextUpdater(Context context, EventManager eventManager)
        {
            this.context = context;
            this.eventManager = eventManager;
            eventManager.Mouse.ButtonDown += Mouse_ButtonDown;
            eventManager.Mouse.ButtonUp += Mouse_ButtonUp;
            eventManager.Mouse.Moved += Mouse_Moved;
            eventManager.Keyboard.KeyPressed += new KeyEvent(Keyboard_KeyPressed);
            eventManager.Keyboard.KeyReleased += new KeyEvent(Keyboard_KeyReleased);
        }

        public void Dispose()
        {
            eventManager.Mouse.ButtonDown -= Mouse_ButtonDown;
            eventManager.Mouse.ButtonUp -= Mouse_ButtonUp;
            eventManager.Mouse.Moved -= Mouse_Moved;
        }

        public void sendUpdate(Clock clock)
        {
            if (holdKey != KeyIdentifier.KI_UNKNOWN)
            {
                repeatTimeout -= clock.fSeconds;
                if (repeatTimeout < 0.0f)
                {
                    repeatTimeout = REPEAT_INTERVAL;
                    context.ProcessKeyUp(holdKey, buildModifier());
                    if (holdChar > 0)
                    {
                        context.ProcessTextInput(holdChar);
                    }
                    context.ProcessKeyDown(holdKey, buildModifier());
                }
            }
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {

        }

        void Mouse_Moved(Mouse mouse, MouseButtonCode buttonCode)
        {
            Vector3 absMouse = mouse.getAbsMouse();
            context.ProcessMouseMove((int)absMouse.x, (int)absMouse.y, 0);
            int wheel = (int)(lastMouseWheel - absMouse.z);
            if (wheel != 0)
            {
                context.ProcessMouseWheel(wheel / 120, 0);
            }
            lastMouseWheel = absMouse.z;
        }

        void Mouse_ButtonUp(Mouse mouse, MouseButtonCode buttonCode)
        {
            context.ProcessMouseButtonUp((int)buttonCode, 0);
        }

        void Mouse_ButtonDown(Mouse mouse, MouseButtonCode buttonCode)
        {
            context.ProcessMouseButtonDown((int)buttonCode, 0);
        }

        void Keyboard_KeyReleased(KeyboardButtonCode keyCode, uint keyChar)
        {
            KeyIdentifier key = InputMap.GetKey(keyCode);
            context.ProcessKeyUp(key, buildModifier());
            if (holdKey == key)
            {
                holdKey = KeyIdentifier.KI_UNKNOWN;
            }
        }

        void Keyboard_KeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            KeyIdentifier key = InputMap.GetKey(keyCode);
            repeatTimeout = REPEAT_INTERVAL_START;
            context.ProcessKeyDown(key, buildModifier());
            if (key == KeyIdentifier.KI_BACK || key == KeyIdentifier.KI_DELETE)
            {
                holdChar = 0;
                holdKey = key;
            }
            else if (keyChar >= 32)
            {
                holdKey = key;
                holdChar = (ushort)keyChar;
                context.ProcessTextInput(holdChar);
            }
            else if (key == KeyIdentifier.KI_RETURN)
            {
                holdKey = key;
                holdChar = (ushort)'\n';
                context.ProcessTextInput(holdChar);
            }
            else if (key == KeyIdentifier.KI_LEFT || key == KeyIdentifier.KI_RIGHT || key == KeyIdentifier.KI_UP || key == KeyIdentifier.KI_DOWN)
            {
                holdKey = key;
                holdChar = 0;
            }
            else
            {
                holdChar = 0;
                holdKey = KeyIdentifier.KI_UNKNOWN;
            }
        }

        int buildModifier()
        {
            Keyboard keyboard = eventManager.Keyboard;
            int value = 0;
            if (keyboard.isModifierDown(Modifier.Alt))
            {
                value += (int)KeyModifier.KM_ALT;
            }
            if (keyboard.isModifierDown(Modifier.Shift))
            {
                value += (int)KeyModifier.KM_SHIFT;
            }
            if (keyboard.isModifierDown(Modifier.Ctrl))
            {
                value += (int)KeyModifier.KM_CTRL;
            }
            return value;
        }
    }
}
