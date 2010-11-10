using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace SoundPlugin
{
    public class Source : SoundPluginObject
    {
        internal Source(IntPtr source)
            : base(source)
        {

        }

        public bool playSound(Sound sound)
        {
            return Source_playSound(Pointer, sound.Pointer);
        }

        public void stop()
        {
            Source_stop(Pointer);
        }

        public void pause()
        {
            Source_pause(Pointer);
        }

        public bool resume()
        {
            return Source_resume(Pointer);
        }

        public bool Playing
        {
            get
            {
                return Source_playing(Pointer);
            }
        }

        public bool Looping
        {
            get
            {
                return Source_getLooping(Pointer);
            }
        }

        #region PInvoke

        [DllImport("SoundWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Source_playSound(IntPtr source, IntPtr sound);

        [DllImport("SoundWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Source_playing(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_stop(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_pause(IntPtr source);

        [DllImport("SoundWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Source_resume(IntPtr source);

        [DllImport("SoundWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Source_getLooping(IntPtr source);

        #endregion
    }
}
