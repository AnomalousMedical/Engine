using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;

namespace libRocketPlugin
{
    /// <summary>
    /// A ContextUpdated for librocket, note that it does not alert its event layer if input is handled
    /// because librocket does not provide that information. This class isn't really used anyway, but something
    /// to be aware of.
    /// </summary>
    public class ContextUpdater : UpdateListener, IDisposable
    {
        private const Int64 REPEAT_INTERVAL = 50000; //in microseconds
        private const Int64 REPEAT_INTERVAL_START = 400000; //in microseconds

        private Context context;
        private EventLayer  eventLayer;
        private float lastMouseWheel = 0;
        private KeyIdentifier holdKey = KeyIdentifier.KI_UNKNOWN;
        private ushort holdChar;
        private Int64 repeatTimeout = REPEAT_INTERVAL;

        public ContextUpdater(Context context, EventLayer eventLayer)
        {
            this.context = context;
            this.eventLayer = eventLayer;

            eventLayer.Mouse.ButtonDown += Mouse_ButtonDown;
            eventLayer.Mouse.ButtonUp += Mouse_ButtonUp;
            eventLayer.Mouse.Moved += Mouse_Moved;

            eventLayer.Keyboard.KeyPressed += Keyboard_KeyPressed;
            eventLayer.Keyboard.KeyReleased += Keyboard_KeyReleased;
        }

        public void Dispose()
        {
            eventLayer.Keyboard.KeyPressed -= Keyboard_KeyPressed;
            eventLayer.Keyboard.KeyReleased -= Keyboard_KeyReleased;

            eventLayer.Mouse.ButtonDown -= Mouse_ButtonDown;
            eventLayer.Mouse.ButtonUp -= Mouse_ButtonUp;
            eventLayer.Mouse.Moved -= Mouse_Moved;
        }

        public void sendUpdate(Clock clock)
        {
            if (holdKey != KeyIdentifier.KI_UNKNOWN)
            {
                repeatTimeout -= clock.DeltaTimeMicro;
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

        void Mouse_Moved(Mouse mouse)
        {
            IntVector3 absMouse = mouse.AbsolutePosition;
            context.ProcessMouseMove(absMouse.x, absMouse.y, 0);
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
            var keyboard = eventLayer.Keyboard;
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
