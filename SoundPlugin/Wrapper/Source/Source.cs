using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace SoundPlugin
{
    public delegate void SourceFinishedDelegate(Source source);

    public class Source : SoundPluginObject
    {
        private SourceFinishedCallback finishedCB;
        public event SourceFinishedDelegate PlaybackFinished;

        internal Source(IntPtr source)
            : base(source)
        {
            finishedCB = finished;
            Source_setFinishedCallback(Pointer, finishedCB);
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

        public void rewind()
        {
            Source_rewind(Pointer);
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

        private void finished(IntPtr source)
        {
            if (PlaybackFinished != null)
            {
                PlaybackFinished.Invoke(this);
            }
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void SourceFinishedCallback(IntPtr source);

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

        [DllImport("SoundWrapper")]
        private static extern void Source_rewind(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_setFinishedCallback(IntPtr source, SourceFinishedCallback callback);

        #endregion
    }
}
