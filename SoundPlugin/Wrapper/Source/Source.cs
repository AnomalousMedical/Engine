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

        public float Pitch
        {
            get
            {
                return Source_getPitch(Pointer);
            }
            set
            {
                Source_setPitch(Pointer, value);
            }
        }

        public float Gain
        {
            get
            {
                return Source_getGain(Pointer);
            }
            set
            {
                Source_setGain(Pointer, value);
            }
        }

        public float MinGain
        {
            get
            {
                return Source_getMinGain(Pointer);
            }
            set
            {
                Source_setMinGain(Pointer, value);
            }
        }

        public float MaxGain
        {
            get
            {
                return Source_getMaxGain(Pointer);
            }
            set
            {
                Source_setMaxGain(Pointer, value);
            }
        }

        public float MaxDistance
        {
            get
            {
                return Source_getMaxDistance(Pointer);
            }
            set
            {
                Source_setMaxDistance(Pointer, value);
            }
        }

        public float RolloffFactor
        {
            get
            {
                return Source_getRolloffFactor(Pointer);
            }
            set
            {
                Source_setRolloffFactor(Pointer, value);
            }
        }

        public float ConeOuterGain
        {
            get
            {
                return Source_getConeOuterGain(Pointer);
            }
            set
            {
                Source_setConeOuterGain(Pointer, value);
            }
        }

        public float ConeInnerAngle
        {
            get
            {
                return Source_getConeInnerAngle(Pointer);
            }
            set
            {
                Source_setConeInnerAngle(Pointer, value);
            }
        }

        public float ConeOuterAngle
        {
            get
            {
                return Source_getConeOuterAngle(Pointer);
            }
            set
            {
                Source_setConeOuterAngle(Pointer, value);
            }
        }

        public float ReferenceDistance
        {
            get
            {
                return Source_getReferenceDistance(Pointer);
            }
            set
            {
                Source_setReferenceDistance(Pointer, value);
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

        [DllImport("SoundWrapper")]
        private static extern void Source_setPitch(IntPtr source, float value);

        [DllImport("SoundWrapper")]
        private static extern float Source_getPitch(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_setGain(IntPtr source, float value);

        [DllImport("SoundWrapper")]
        private static extern float Source_getGain(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_setMinGain(IntPtr source, float value);

        [DllImport("SoundWrapper")]
        private static extern float Source_getMinGain(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_setMaxGain(IntPtr source, float value);

        [DllImport("SoundWrapper")]
        private static extern float Source_getMaxGain(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_setMaxDistance(IntPtr source, float value);

        [DllImport("SoundWrapper")]
        private static extern float Source_getMaxDistance(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_setRolloffFactor(IntPtr source, float value);

        [DllImport("SoundWrapper")]
        private static extern float Source_getRolloffFactor(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_setConeOuterGain(IntPtr source, float value);

        [DllImport("SoundWrapper")]
        private static extern float Source_getConeOuterGain(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_setConeInnerAngle(IntPtr source, float value);

        [DllImport("SoundWrapper")]
        private static extern float Source_getConeInnerAngle(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_setConeOuterAngle(IntPtr source, float value);

        [DllImport("SoundWrapper")]
        private static extern float Source_getConeOuterAngle(IntPtr source);

        [DllImport("SoundWrapper")]
        private static extern void Source_setReferenceDistance(IntPtr source, float value);

        [DllImport("SoundWrapper")]
        private static extern float Source_getReferenceDistance(IntPtr source);

        #endregion
    }
}
