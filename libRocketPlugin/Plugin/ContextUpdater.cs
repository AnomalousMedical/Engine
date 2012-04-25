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
        private Context context;
        private EventManager eventManager;
        private float lastMouseWheel = 0;

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
            //context.Update();
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
            context.ProcessKeyUp(keyMap[keyCode], buildModifier());
        }

        void Keyboard_KeyPressed(KeyboardButtonCode keyCode, uint keyChar)
        {
            context.ProcessKeyDown(keyMap[keyCode], buildModifier());
            if (keyChar >= 32)
            {
                context.ProcessTextInput((ushort)keyChar);
            }
            else if (keyCode == KeyboardButtonCode.KC_RETURN)
            {
                context.ProcessTextInput((ushort)'\n');
            }
        }

        int buildModifier()
        {
            Keyboard keyboard = eventManager.Keyboard;
            int value = 0;
            if(keyboard.isModifierDown(Modifier.Alt))
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

        private static Dictionary<KeyboardButtonCode, KeyIdentifier> keyMap = new Dictionary<KeyboardButtonCode, KeyIdentifier>();

        static ContextUpdater()
        {
            keyMap[KeyboardButtonCode.KC_UNASSIGNED] = KeyIdentifier.KI_UNKNOWN;
	        keyMap[KeyboardButtonCode.KC_ESCAPE] = KeyIdentifier.KI_ESCAPE;
	        keyMap[KeyboardButtonCode.KC_1] = KeyIdentifier.KI_1;
	        keyMap[KeyboardButtonCode.KC_2] = KeyIdentifier.KI_2;
	        keyMap[KeyboardButtonCode.KC_3] = KeyIdentifier.KI_3;
	        keyMap[KeyboardButtonCode.KC_4] = KeyIdentifier.KI_4;
	        keyMap[KeyboardButtonCode.KC_5] = KeyIdentifier.KI_5;
	        keyMap[KeyboardButtonCode.KC_6] = KeyIdentifier.KI_6;
	        keyMap[KeyboardButtonCode.KC_7] = KeyIdentifier.KI_7;
	        keyMap[KeyboardButtonCode.KC_8] = KeyIdentifier.KI_8;
	        keyMap[KeyboardButtonCode.KC_9] = KeyIdentifier.KI_9;
	        keyMap[KeyboardButtonCode.KC_0] = KeyIdentifier.KI_0;
	        keyMap[KeyboardButtonCode.KC_MINUS] = KeyIdentifier.KI_OEM_MINUS;
	        keyMap[KeyboardButtonCode.KC_EQUALS] = KeyIdentifier.KI_OEM_PLUS;
	        keyMap[KeyboardButtonCode.KC_BACK] = KeyIdentifier.KI_BACK;
	        keyMap[KeyboardButtonCode.KC_TAB] = KeyIdentifier.KI_TAB;
	        keyMap[KeyboardButtonCode.KC_Q] = KeyIdentifier.KI_Q;
	        keyMap[KeyboardButtonCode.KC_W] = KeyIdentifier.KI_W;
	        keyMap[KeyboardButtonCode.KC_E] = KeyIdentifier.KI_E;
	        keyMap[KeyboardButtonCode.KC_R] = KeyIdentifier.KI_R;
	        keyMap[KeyboardButtonCode.KC_T] = KeyIdentifier.KI_T;
	        keyMap[KeyboardButtonCode.KC_Y] = KeyIdentifier.KI_Y;
	        keyMap[KeyboardButtonCode.KC_U] = KeyIdentifier.KI_U;
	        keyMap[KeyboardButtonCode.KC_I] = KeyIdentifier.KI_I;
	        keyMap[KeyboardButtonCode.KC_O] = KeyIdentifier.KI_O;
	        keyMap[KeyboardButtonCode.KC_P] = KeyIdentifier.KI_P;
	        keyMap[KeyboardButtonCode.KC_LBRACKET] = KeyIdentifier.KI_OEM_4;
	        keyMap[KeyboardButtonCode.KC_RBRACKET] = KeyIdentifier.KI_OEM_6;
	        keyMap[KeyboardButtonCode.KC_RETURN] = KeyIdentifier.KI_RETURN;
	        keyMap[KeyboardButtonCode.KC_LCONTROL] = KeyIdentifier.KI_LCONTROL;
	        keyMap[KeyboardButtonCode.KC_A] = KeyIdentifier.KI_A;
	        keyMap[KeyboardButtonCode.KC_S] = KeyIdentifier.KI_S;
	        keyMap[KeyboardButtonCode.KC_D] = KeyIdentifier.KI_D;
	        keyMap[KeyboardButtonCode.KC_F] = KeyIdentifier.KI_F;
	        keyMap[KeyboardButtonCode.KC_G] = KeyIdentifier.KI_G;
	        keyMap[KeyboardButtonCode.KC_H] = KeyIdentifier.KI_H;
	        keyMap[KeyboardButtonCode.KC_J] = KeyIdentifier.KI_J;
	        keyMap[KeyboardButtonCode.KC_K] = KeyIdentifier.KI_K;
	        keyMap[KeyboardButtonCode.KC_L] = KeyIdentifier.KI_L;
	        keyMap[KeyboardButtonCode.KC_SEMICOLON] = KeyIdentifier.KI_OEM_1;
	        keyMap[KeyboardButtonCode.KC_APOSTROPHE] = KeyIdentifier.KI_OEM_7;
	        keyMap[KeyboardButtonCode.KC_GRAVE] = KeyIdentifier.KI_OEM_3;
	        keyMap[KeyboardButtonCode.KC_LSHIFT] = KeyIdentifier.KI_LSHIFT;
	        keyMap[KeyboardButtonCode.KC_BACKSLASH] = KeyIdentifier.KI_OEM_5;
	        keyMap[KeyboardButtonCode.KC_Z] = KeyIdentifier.KI_Z;
	        keyMap[KeyboardButtonCode.KC_X] = KeyIdentifier.KI_X;
	        keyMap[KeyboardButtonCode.KC_C] = KeyIdentifier.KI_C;
	        keyMap[KeyboardButtonCode.KC_V] = KeyIdentifier.KI_V;
	        keyMap[KeyboardButtonCode.KC_B] = KeyIdentifier.KI_B;
	        keyMap[KeyboardButtonCode.KC_N] = KeyIdentifier.KI_N;
	        keyMap[KeyboardButtonCode.KC_M] = KeyIdentifier.KI_M;
	        keyMap[KeyboardButtonCode.KC_COMMA] = KeyIdentifier.KI_OEM_COMMA;
	        keyMap[KeyboardButtonCode.KC_PERIOD] = KeyIdentifier.KI_OEM_PERIOD;
	        keyMap[KeyboardButtonCode.KC_SLASH] = KeyIdentifier.KI_OEM_2;
	        keyMap[KeyboardButtonCode.KC_RSHIFT] = KeyIdentifier.KI_RSHIFT;
	        keyMap[KeyboardButtonCode.KC_MULTIPLY] = KeyIdentifier.KI_MULTIPLY;
	        keyMap[KeyboardButtonCode.KC_LMENU] = KeyIdentifier.KI_LMENU;
	        keyMap[KeyboardButtonCode.KC_SPACE] = KeyIdentifier.KI_SPACE;
	        keyMap[KeyboardButtonCode.KC_CAPITAL] = KeyIdentifier.KI_CAPITAL;
	        keyMap[KeyboardButtonCode.KC_F1] = KeyIdentifier.KI_F1;
	        keyMap[KeyboardButtonCode.KC_F2] = KeyIdentifier.KI_F2;
	        keyMap[KeyboardButtonCode.KC_F3] = KeyIdentifier.KI_F3;
	        keyMap[KeyboardButtonCode.KC_F4] = KeyIdentifier.KI_F4;
	        keyMap[KeyboardButtonCode.KC_F5] = KeyIdentifier.KI_F5;
	        keyMap[KeyboardButtonCode.KC_F6] = KeyIdentifier.KI_F6;
	        keyMap[KeyboardButtonCode.KC_F7] = KeyIdentifier.KI_F7;
	        keyMap[KeyboardButtonCode.KC_F8] = KeyIdentifier.KI_F8;
	        keyMap[KeyboardButtonCode.KC_F9] = KeyIdentifier.KI_F9;
	        keyMap[KeyboardButtonCode.KC_F10] = KeyIdentifier.KI_F10;
	        keyMap[KeyboardButtonCode.KC_NUMLOCK] = KeyIdentifier.KI_NUMLOCK;
	        keyMap[KeyboardButtonCode.KC_SCROLL] = KeyIdentifier.KI_SCROLL;
	        keyMap[KeyboardButtonCode.KC_NUMPAD7] = KeyIdentifier.KI_7;
	        keyMap[KeyboardButtonCode.KC_NUMPAD8] = KeyIdentifier.KI_8;
	        keyMap[KeyboardButtonCode.KC_NUMPAD9] = KeyIdentifier.KI_9;
	        keyMap[KeyboardButtonCode.KC_SUBTRACT] = KeyIdentifier.KI_SUBTRACT;
	        keyMap[KeyboardButtonCode.KC_NUMPAD4] = KeyIdentifier.KI_4;
	        keyMap[KeyboardButtonCode.KC_NUMPAD5] = KeyIdentifier.KI_5;
	        keyMap[KeyboardButtonCode.KC_NUMPAD6] = KeyIdentifier.KI_6;
	        keyMap[KeyboardButtonCode.KC_ADD] = KeyIdentifier.KI_ADD;
	        keyMap[KeyboardButtonCode.KC_NUMPAD1] = KeyIdentifier.KI_1;
	        keyMap[KeyboardButtonCode.KC_NUMPAD2] = KeyIdentifier.KI_2;
	        keyMap[KeyboardButtonCode.KC_NUMPAD3] = KeyIdentifier.KI_3;
	        keyMap[KeyboardButtonCode.KC_NUMPAD0] = KeyIdentifier.KI_0;
	        keyMap[KeyboardButtonCode.KC_DECIMAL] = KeyIdentifier.KI_DECIMAL;
	        keyMap[KeyboardButtonCode.KC_OEM_102] = KeyIdentifier.KI_OEM_102;
	        keyMap[KeyboardButtonCode.KC_F11] = KeyIdentifier.KI_F11;
	        keyMap[KeyboardButtonCode.KC_F12] = KeyIdentifier.KI_F12;
	        keyMap[KeyboardButtonCode.KC_F13] = KeyIdentifier.KI_F13;
	        keyMap[KeyboardButtonCode.KC_F14] = KeyIdentifier.KI_F14;
	        keyMap[KeyboardButtonCode.KC_F15] = KeyIdentifier.KI_F15;
	        keyMap[KeyboardButtonCode.KC_KANA] = KeyIdentifier.KI_KANA;
	        keyMap[KeyboardButtonCode.KC_ABNT_C1] = KeyIdentifier.KI_UNKNOWN;
	        keyMap[KeyboardButtonCode.KC_CONVERT] = KeyIdentifier.KI_CONVERT;
	        keyMap[KeyboardButtonCode.KC_NOCONVERT] = KeyIdentifier.KI_NONCONVERT;
	        keyMap[KeyboardButtonCode.KC_YEN] = KeyIdentifier.KI_UNKNOWN;
	        keyMap[KeyboardButtonCode.KC_ABNT_C2] = KeyIdentifier.KI_UNKNOWN;
	        keyMap[KeyboardButtonCode.KC_NUMPADEQUALS] = KeyIdentifier.KI_OEM_NEC_EQUAL;
	        keyMap[KeyboardButtonCode.KC_PREVTRACK] = KeyIdentifier.KI_MEDIA_PREV_TRACK;
	        keyMap[KeyboardButtonCode.KC_AT] = KeyIdentifier.KI_UNKNOWN;
	        keyMap[KeyboardButtonCode.KC_COLON] = KeyIdentifier.KI_OEM_1;
	        keyMap[KeyboardButtonCode.KC_UNDERLINE] = KeyIdentifier.KI_OEM_MINUS;
	        keyMap[KeyboardButtonCode.KC_KANJI] = KeyIdentifier.KI_KANJI;
	        keyMap[KeyboardButtonCode.KC_STOP] = KeyIdentifier.KI_UNKNOWN;
	        keyMap[KeyboardButtonCode.KC_AX] = KeyIdentifier.KI_OEM_AX;
	        keyMap[KeyboardButtonCode.KC_UNLABELED] = KeyIdentifier.KI_UNKNOWN;
	        keyMap[KeyboardButtonCode.KC_NEXTTRACK] = KeyIdentifier.KI_MEDIA_NEXT_TRACK;
	        keyMap[KeyboardButtonCode.KC_NUMPADENTER] = KeyIdentifier.KI_NUMPADENTER;
	        keyMap[KeyboardButtonCode.KC_RCONTROL] = KeyIdentifier.KI_RCONTROL;
	        keyMap[KeyboardButtonCode.KC_MUTE] = KeyIdentifier.KI_VOLUME_MUTE;
	        keyMap[KeyboardButtonCode.KC_CALCULATOR] = KeyIdentifier.KI_UNKNOWN;
	        keyMap[KeyboardButtonCode.KC_PLAYPAUSE] = KeyIdentifier.KI_MEDIA_PLAY_PAUSE;
	        keyMap[KeyboardButtonCode.KC_MEDIASTOP] = KeyIdentifier.KI_MEDIA_STOP;
	        keyMap[KeyboardButtonCode.KC_VOLUMEDOWN] = KeyIdentifier.KI_VOLUME_DOWN;
	        keyMap[KeyboardButtonCode.KC_VOLUMEUP] = KeyIdentifier.KI_VOLUME_UP;
	        keyMap[KeyboardButtonCode.KC_WEBHOME] = KeyIdentifier.KI_BROWSER_HOME;
	        keyMap[KeyboardButtonCode.KC_NUMPADCOMMA] = KeyIdentifier.KI_SEPARATOR;
	        keyMap[KeyboardButtonCode.KC_DIVIDE] = KeyIdentifier.KI_DIVIDE;
	        keyMap[KeyboardButtonCode.KC_SYSRQ] = KeyIdentifier.KI_SNAPSHOT;
	        keyMap[KeyboardButtonCode.KC_RMENU] = KeyIdentifier.KI_RMENU;
	        keyMap[KeyboardButtonCode.KC_PAUSE] = KeyIdentifier.KI_PAUSE;
	        keyMap[KeyboardButtonCode.KC_HOME] = KeyIdentifier.KI_HOME;
	        keyMap[KeyboardButtonCode.KC_UP] = KeyIdentifier.KI_UP;
	        keyMap[KeyboardButtonCode.KC_PGUP] = KeyIdentifier.KI_PRIOR;
	        keyMap[KeyboardButtonCode.KC_LEFT] = KeyIdentifier.KI_LEFT;
	        keyMap[KeyboardButtonCode.KC_RIGHT] = KeyIdentifier.KI_RIGHT;
	        keyMap[KeyboardButtonCode.KC_END] = KeyIdentifier.KI_END;
	        keyMap[KeyboardButtonCode.KC_DOWN] = KeyIdentifier.KI_DOWN;
	        keyMap[KeyboardButtonCode.KC_PGDOWN] = KeyIdentifier.KI_NEXT;
	        keyMap[KeyboardButtonCode.KC_INSERT] = KeyIdentifier.KI_INSERT;
	        keyMap[KeyboardButtonCode.KC_DELETE] = KeyIdentifier.KI_DELETE;
	        keyMap[KeyboardButtonCode.KC_LWIN] = KeyIdentifier.KI_LWIN;
	        keyMap[KeyboardButtonCode.KC_RWIN] = KeyIdentifier.KI_RWIN;
	        keyMap[KeyboardButtonCode.KC_APPS] = KeyIdentifier.KI_APPS;
	        keyMap[KeyboardButtonCode.KC_POWER] = KeyIdentifier.KI_POWER;
	        keyMap[KeyboardButtonCode.KC_SLEEP] = KeyIdentifier.KI_SLEEP;
	        keyMap[KeyboardButtonCode.KC_WAKE] = KeyIdentifier.KI_WAKE;
	        keyMap[KeyboardButtonCode.KC_WEBSEARCH] = KeyIdentifier.KI_BROWSER_SEARCH;
	        keyMap[KeyboardButtonCode.KC_WEBFAVORITES] = KeyIdentifier.KI_BROWSER_FAVORITES;
	        keyMap[KeyboardButtonCode.KC_WEBREFRESH] = KeyIdentifier.KI_BROWSER_REFRESH;
	        keyMap[KeyboardButtonCode.KC_WEBSTOP] = KeyIdentifier.KI_BROWSER_STOP;
	        keyMap[KeyboardButtonCode.KC_WEBFORWARD] = KeyIdentifier.KI_BROWSER_FORWARD;
	        keyMap[KeyboardButtonCode.KC_WEBBACK] = KeyIdentifier.KI_BROWSER_BACK;
	        keyMap[KeyboardButtonCode.KC_MYCOMPUTER] = KeyIdentifier.KI_UNKNOWN;
	        keyMap[KeyboardButtonCode.KC_MAIL] = KeyIdentifier.KI_LAUNCH_MAIL;
	        keyMap[KeyboardButtonCode.KC_MEDIASELECT] = KeyIdentifier.KI_LAUNCH_MEDIA_SELECT;
        }
    }
}
