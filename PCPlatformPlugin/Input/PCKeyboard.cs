using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Logging;

namespace PCPlatform
{
    class PCKeyboard : Keyboard, IDisposable
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void KeyCallback(KeyboardButtonCode key, uint text);

        /// <summary>
        /// This event is fired when a key is pressed. Will only be fired if the
        /// keyboard is created with buffered=true.
        /// </summary>
        public override event KeyEvent KeyPressed;

        /// <summary>
        /// This event is fired when a key is released. Will only be fired if the
        /// keyboard is created with buffered=true.
        /// </summary>
        public override event KeyEvent KeyReleased;

        private IntPtr keyboard;
        private IntPtr keyListener;
        private sbyte[] keys = new sbyte[256];

        private KeyCallback keyPressedCbPtr;
        private KeyCallback keyReleasedCbPtr;

        public PCKeyboard(IntPtr keyboard)
        {
            this.keyboard = keyboard;
            keyPressedCbPtr = new KeyCallback(keyPressedCb);
            keyReleasedCbPtr = new KeyCallback(keyReleasedCb);
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

        public override int translateText(KeyboardButtonCode code)
        {
            return oisKeyboard_translateText(code);
        }

        public unsafe override void capture()
        {
            fixed (sbyte* keyBuf = &keys[0])
            {
                oisKeyboard_capture(keyboard, keyBuf);
            }
        }

        internal IntPtr KeyboardHandle
        {
            get
            {
                return keyboard;
            }
        }

        private void keyPressedCb(KeyboardButtonCode key, uint text)
        {
            if (KeyPressed != null)
            {
                KeyPressed.Invoke(key, text);
            }
        }

        private void keyReleasedCb(KeyboardButtonCode key, uint text)
        {
            if (KeyReleased != null)
            {
                KeyReleased.Invoke(key, text);
            }
        }

        [DllImport("PCPlatform")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool oisKeyboard_isModifierDown(IntPtr keyboard, Modifier keyCode);

        [DllImport("PCPlatform")]
        private static extern IntPtr oisKeyboard_getAsString(IntPtr keyboard, KeyboardButtonCode code);

        [DllImport("PCPlatform")]
        private static extern int oisKeyboard_translateText(KeyboardButtonCode code);

        [DllImport("PCPlatform")]
        private static unsafe extern void oisKeyboard_capture(IntPtr keyboard, sbyte* keys);

        [DllImport("PCPlatform")]
        private static extern IntPtr oisKeyboard_createListener(IntPtr keyboard, KeyCallback keyPressedCb, KeyCallback keyReleasedCb);

        [DllImport("PCPlatform")]
        private static extern void oisKeyboard_destroyListener(IntPtr keyboard, IntPtr listener);
    }
}
