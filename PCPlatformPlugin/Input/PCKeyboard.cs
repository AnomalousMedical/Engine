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
        private delegate void KeyCallback(KeyboardButtonCode key, uint text);

        private IntPtr keyboard;
        private IntPtr keyListener;
        private sbyte[] keys = new sbyte[256];

        private KeyCallback keyPressedCbPtr;
        private KeyCallback keyReleasedCbPtr;

        public PCKeyboard(IntPtr keyboard, EventManager eventManager)
            :base(eventManager)
        {
            this.keyboard = keyboard;
            keyPressedCbPtr = new KeyCallback(fireKeyPressed);
            keyReleasedCbPtr = new KeyCallback(fireKeyReleased);
            keyListener = oisKeyboard_createListener(keyboard, keyPressedCbPtr, keyReleasedCbPtr);
        }

        public void Dispose()
        {
            oisKeyboard_destroyListener(keyboard, keyListener);
        }

        public override bool isKeyDown(KeyboardButtonCode keyCode)
        {
            return keys[(int)keyCode] != 0;
        }

        public override bool isModifierDown(Modifier keyCode)
        {
            return oisKeyboard_isModifierDown(keyboard, keyCode);
        }

        public override string getAsString(KeyboardButtonCode code)
        {
            return Marshal.PtrToStringAnsi(oisKeyboard_getAsString(keyboard, code));
        }

        public unsafe override void capture()
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
        private static extern IntPtr oisKeyboard_createListener(IntPtr keyboard, KeyCallback keyPressedCb, KeyCallback keyReleasedCb);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern void oisKeyboard_destroyListener(IntPtr keyboard, IntPtr listener);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern void oisKeyboard_setTextTranslationMode(IntPtr keyboard, TextTranslationMode mode);

        [DllImport("PCPlatform", CallingConvention=CallingConvention.Cdecl)]
        private static extern TextTranslationMode oisKeyboard_getTextTranslationMode(IntPtr keyboard);
    }
}
