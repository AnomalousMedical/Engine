using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace libRocketPlugin
{
    public static class InputMap
    {
        static Dictionary<KeyboardButtonCode, KeyIdentifier> keyMap = new Dictionary<KeyboardButtonCode, KeyIdentifier>();

        public static KeyIdentifier GetKey(KeyboardButtonCode buttonCode)
        {
            return keyMap[buttonCode];
        }

        #region Map
        static InputMap()
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
        #endregion
    }

    public enum KeyIdentifier
    {
        KI_UNKNOWN = 0,

        KI_SPACE = 1,

        KI_0 = 2,
        KI_1 = 3,
        KI_2 = 4,
        KI_3 = 5,
        KI_4 = 6,
        KI_5 = 7,
        KI_6 = 8,
        KI_7 = 9,
        KI_8 = 10,
        KI_9 = 11,

        KI_A = 12,
        KI_B = 13,
        KI_C = 14,
        KI_D = 15,
        KI_E = 16,
        KI_F = 17,
        KI_G = 18,
        KI_H = 19,
        KI_I = 20,
        KI_J = 21,
        KI_K = 22,
        KI_L = 23,
        KI_M = 24,
        KI_N = 25,
        KI_O = 26,
        KI_P = 27,
        KI_Q = 28,
        KI_R = 29,
        KI_S = 30,
        KI_T = 31,
        KI_U = 32,
        KI_V = 33,
        KI_W = 34,
        KI_X = 35,
        KI_Y = 36,
        KI_Z = 37,

        KI_OEM_1 = 38,				// US standard keyboard; the ';:' key.
        KI_OEM_PLUS = 39,			// Any region; the '=+' key.
        KI_OEM_COMMA = 40,			// Any region; the ',<' key.
        KI_OEM_MINUS = 41,			// Any region; the '-_' key.
        KI_OEM_PERIOD = 42,			// Any region; the '.>' key.
        KI_OEM_2 = 43,				// Any region; the '/?' key.
        KI_OEM_3 = 44,				// Any region; the '`~' key.

        KI_OEM_4 = 45,				// US standard keyboard; the '[{' key.
        KI_OEM_5 = 46,				// US standard keyboard; the '\|' key.
        KI_OEM_6 = 47,				// US standard keyboard; the ']}' key.
        KI_OEM_7 = 48,				// US standard keyboard; the ''"' key.
        KI_OEM_8 = 49,

        KI_OEM_102 = 50,			// RT 102-key keyboard; the '<>' or '\|' key.

        KI_NUMPAD0 = 51,
        KI_NUMPAD1 = 52,
        KI_NUMPAD2 = 53,
        KI_NUMPAD3 = 54,
        KI_NUMPAD4 = 55,
        KI_NUMPAD5 = 56,
        KI_NUMPAD6 = 57,
        KI_NUMPAD7 = 58,
        KI_NUMPAD8 = 59,
        KI_NUMPAD9 = 60,
        KI_NUMPADENTER = 61,
        KI_MULTIPLY = 62,			// Asterisk on the numeric keypad.
        KI_ADD = 63,				// Plus on the numeric keypad.
        KI_SEPARATOR = 64,
        KI_SUBTRACT = 65,			// Minus on the numeric keypad.
        KI_DECIMAL = 66,			// Period on the numeric keypad.
        KI_DIVIDE = 67,				// Forward Slash on the numeric keypad.

        /*
         * NEC PC-9800 kbd definitions
         */
        KI_OEM_NEC_EQUAL = 68,		// Equals key on the numeric keypad.

        KI_BACK = 69,				// Backspace key.
        KI_TAB = 70,				// Tab key.

        KI_CLEAR = 71,
        KI_RETURN = 72,

        KI_PAUSE = 73,
        KI_CAPITAL = 74,			// Capslock key.

        KI_KANA = 75,				// IME Kana mode.
        KI_HANGUL = 76,				// IME Hangul mode.
        KI_JUNJA = 77,				// IME Junja mode.
        KI_FINAL = 78,				// IME final mode.
        KI_HANJA = 79,				// IME Hanja mode.
        KI_KANJI = 80,				// IME Kanji mode.

        KI_ESCAPE = 81,				// Escape key.

        KI_CONVERT = 82,			// IME convert.
        KI_NONCONVERT = 83,			// IME nonconvert.
        KI_ACCEPT = 84,				// IME accept.
        KI_MODECHANGE = 85,			// IME mode change request.

        KI_PRIOR = 86,				// Page Up key.
        KI_NEXT = 87,				// Page Down key.
        KI_END = 88,
        KI_HOME = 89,
        KI_LEFT = 90,				// Left Arrow key.
        KI_UP = 91,					// Up Arrow key.
        KI_RIGHT = 92,				// Right Arrow key.
        KI_DOWN = 93,				// Down Arrow key.
        KI_SELECT = 94,
        KI_PRINT = 95,
        KI_EXECUTE = 96,
        KI_SNAPSHOT = 97,			// Print Screen key.
        KI_INSERT = 98,
        KI_DELETE = 99,
        KI_HELP = 100,

        KI_LWIN = 101,				// Left Windows key.
        KI_RWIN = 102,				// Right Windows key.
        KI_APPS = 103,				// Applications key.

        KI_POWER = 104,
        KI_SLEEP = 105,
        KI_WAKE = 106,

        KI_F1 = 107,
        KI_F2 = 108,
        KI_F3 = 109,
        KI_F4 = 110,
        KI_F5 = 111,
        KI_F6 = 112,
        KI_F7 = 113,
        KI_F8 = 114,
        KI_F9 = 115,
        KI_F10 = 116,
        KI_F11 = 117,
        KI_F12 = 118,
        KI_F13 = 119,
        KI_F14 = 120,
        KI_F15 = 121,
        KI_F16 = 122,
        KI_F17 = 123,
        KI_F18 = 124,
        KI_F19 = 125,
        KI_F20 = 126,
        KI_F21 = 127,
        KI_F22 = 128,
        KI_F23 = 129,
        KI_F24 = 130,

        KI_NUMLOCK = 131,			// Numlock key.
        KI_SCROLL = 132,			// Scroll Lock key.

        /*
         * Fujitsu/OASYS kbd definitions
         */
        KI_OEM_FJ_JISHO = 133,		// 'Dictionary' key.
        KI_OEM_FJ_MASSHOU = 134,	// 'Unregister word' key.
        KI_OEM_FJ_TOUROKU = 135,	// 'Register word' key.
        KI_OEM_FJ_LOYA = 136,		// 'Left OYAYUBI' key.
        KI_OEM_FJ_ROYA = 137,		// 'Right OYAYUBI' key.

        KI_LSHIFT = 138,
        KI_RSHIFT = 139,
        KI_LCONTROL = 140,
        KI_RCONTROL = 141,
        KI_LMENU = 142,
        KI_RMENU = 143,

        KI_BROWSER_BACK = 144,
        KI_BROWSER_FORWARD = 145,
        KI_BROWSER_REFRESH = 146,
        KI_BROWSER_STOP = 147,
        KI_BROWSER_SEARCH = 148,
        KI_BROWSER_FAVORITES = 149,
        KI_BROWSER_HOME = 150,

        KI_VOLUME_MUTE = 151,
        KI_VOLUME_DOWN = 152,
        KI_VOLUME_UP = 153,
        KI_MEDIA_NEXT_TRACK = 154,
        KI_MEDIA_PREV_TRACK = 155,
        KI_MEDIA_STOP = 156,
        KI_MEDIA_PLAY_PAUSE = 157,
        KI_LAUNCH_MAIL = 158,
        KI_LAUNCH_MEDIA_SELECT = 159,
        KI_LAUNCH_APP1 = 160,
        KI_LAUNCH_APP2 = 161,

        /*
         * Various extended or enhanced keyboards
         */
        KI_OEM_AX = 162,
        KI_ICO_HELP = 163,
        KI_ICO_00 = 164,

        KI_PROCESSKEY = 165,		// IME Process key.

        KI_ICO_CLEAR = 166,

        KI_ATTN = 167,
        KI_CRSEL = 168,
        KI_EXSEL = 169,
        KI_EREOF = 170,
        KI_PLAY = 171,
        KI_ZOOM = 172,
        KI_PA1 = 173,
        KI_OEM_CLEAR = 174,

        KI_LMETA = 175,
        KI_RMETA = 176
    };

    public enum KeyModifier
    {
        KM_CTRL = 1 << 0,		// Set if at least one Ctrl key is depressed.
        KM_SHIFT = 1 << 1,		// Set if at least one Shift key is depressed.
        KM_ALT = 1 << 2,		// Set if at least one Alt key is depressed.
        KM_META = 1 << 3,		// Set if at least one Meta key (the command key) is depressed.
        KM_CAPSLOCK = 1 << 4,	// Set if caps lock is enabled.
        KM_NUMLOCK = 1 << 5,	// Set if num lock is enabled.
        KM_SCROLLLOCK = 1 << 6	// Set if scroll lock is enabled.
    };
}
