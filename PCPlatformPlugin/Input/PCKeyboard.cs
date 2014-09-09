using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Logging;

namespace PCPlatform
{
    class PCKeyboard : KeyboardHardware, IDisposable
    {
        public enum TextTranslationMode
        {
            Off,
            Unicode,
            Ascii
        };

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void KeyDownCallback(KeyboardButtonCode key, uint text);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void KeyUpCallback(KeyboardButtonCode key);

        private IntPtr keyboard;
        private IntPtr keyListener;

        private KeyDownCallback keyPressedCbPtr;
        private KeyUpCallback keyReleasedCbPtr;

        private sbyte[] keys = new sbyte[256];

        public PCKeyboard(IntPtr keyboardPtr, Keyboard keyboard)
            : base(keyboard)
        {
            this.keyboard = keyboardPtr;
            keyPressedCbPtr = new KeyDownCallback(fireKeyPressed);
            keyReleasedCbPtr = new KeyUpCallback(fireKeyReleased);
            keyListener = oisKeyboard_createListener(this.keyboard, keyPressedCbPtr, keyReleasedCbPtr);
        }

        public void Dispose()
        {
            oisKeyboard_destroyListener(keyboard, keyListener);
        }

        public unsafe void capture()
        {
            fixed (sbyte* keyBuf = &keys[0])
            {
                oisKeyboard_capture(keyboard, keyBuf);
            }
        }

        public TextTranslationMode TranslationMode
        {
            get
            {
                return oisKeyboard_getTextTranslationMode(keyboard);
            }
            set
            {
                oisKeyboard_setTextTranslationMode(keyboard, value);
            }
        }

        internal IntPtr KeyboardHandle
        {
            get
            {
                return keyboard;
            }
        }

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool oisKeyboard_isModifierDown(IntPtr keyboard, Modifier keyCode);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr oisKeyboard_getAsString(IntPtr keyboard, KeyboardButtonCode code);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static unsafe extern void oisKeyboard_capture(IntPtr keyboard, sbyte* keys);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr oisKeyboard_createListener(IntPtr keyboard, KeyDownCallback keyPressedCb, KeyUpCallback keyReleasedCb);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern void oisKeyboard_destroyListener(IntPtr keyboard, IntPtr listener);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern void oisKeyboard_setTextTranslationMode(IntPtr keyboard, TextTranslationMode mode);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern TextTranslationMode oisKeyboard_getTextTranslationMode(IntPtr keyboard);
    }
}
