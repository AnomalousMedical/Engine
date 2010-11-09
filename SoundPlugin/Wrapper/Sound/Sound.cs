using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SoundPlugin
{
    public class Sound : IDisposable
    {
        private IntPtr sound;

        public IntPtr Pointer
        {
            get
            {
                return sound;
            }
        }

        internal Sound(IntPtr sound)
        {
            this.sound = sound;
        }

        public void Dispose()
        {
            sound = IntPtr.Zero;
        }

        public bool Repeat
        {
            get
            {
                return Sound_getRepeat(sound);
            }
            set
            {
                Sound_setRepeat(sound, value);
            }
        }

        #region PInvoke

        [DllImport("SoundWrapper")]
        private static extern void Sound_setRepeat(IntPtr sound, bool value);

        [DllImport("SoundWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Sound_getRepeat(IntPtr sound);

        #endregion
    }
}
