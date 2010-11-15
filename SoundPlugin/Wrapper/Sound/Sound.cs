using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SoundPlugin
{
    public class Sound : SoundPluginObject
    {
        internal Sound(IntPtr sound)
            :base(sound)
        {
            
        }

        public bool Repeat
        {
            get
            {
                return Sound_getRepeat(Pointer);
            }
            set
            {
                Sound_setRepeat(Pointer, value);
            }
        }

        public double Duration
        {
            get
            {
                return Sound_getDuration(Pointer);
            }
        }

        #region PInvoke

        [DllImport("SoundWrapper")]
        private static extern void Sound_setRepeat(IntPtr sound, bool value);

        [DllImport("SoundWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Sound_getRepeat(IntPtr sound);

        [DllImport("SoundWrapper")]
        private static extern double Sound_getDuration(IntPtr sound);

        #endregion
    }
}
