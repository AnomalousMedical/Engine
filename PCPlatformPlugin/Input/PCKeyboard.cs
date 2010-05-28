using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;

namespace PCPlatform
{
    class PCKeyboard : Keyboard
    {
        private IntPtr keyboard;
        private sbyte[] keys = new sbyte[256];

        public PCKeyboard(IntPtr keyboard)
        {
            this.keyboard = keyboard;
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

        internal IntPtr KeyboardHandle
        {
            get
            {
                return keyboard;
            }
        }

        [DllImport("PCPlatform")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool oisKeyboard_isModifierDown(IntPtr keyboard, Modifier keyCode);

        [DllImport("PCPlatform")]
        private static extern IntPtr oisKeyboard_getAsString(IntPtr keyboard, KeyboardButtonCode code);

        [DllImport("PCPlatform")]
        private static unsafe extern void oisKeyboard_capture(IntPtr keyboard, sbyte* keys);
    }
}
