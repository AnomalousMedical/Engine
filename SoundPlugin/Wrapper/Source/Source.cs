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

        /// <summary>
        /// pitch multiplier
        /// always positive
        /// </summary>
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

        /// <summary>
        /// source gain
        /// value should be positive
        /// </summary>
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

        /// <summary>
        /// the minimum gain for this source
        /// </summary>
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

        /// <summary>
        /// the maximum gain for this source
        /// </summary>
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

        /// <summary>
        /// used with the Inverse Clamped Distance Model to set the distance
        /// where there will no longer be any attenuation of the source.
        /// </summary>
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

        /// <summary>
        /// the rolloff rate for the source
        /// default is 1.0
        /// </summary>
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

        /// <summary>
        /// the gain when outside the oriented cone
        /// </summary>
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

        /// <summary>
        /// the gain when inside the oriented cone
        /// </summary>
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

        /// <summary>
        /// outer angle of the sound cone, in degrees
        /// default is 360
        /// </summary>
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

        /// <summary>
        /// the distance under which the volume for the source would normally
        /// drop by half (before being influenced by rolloff factor or
        /// MaxDistance)
        /// </summary>
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

        /// <summary>
        /// The position of the source.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return Source_getPosition(Pointer);
            }
            set
            {
                Source_setPosition(Pointer, value);
            }
        }

        /// <summary>
        /// The velocity of the source.
        /// </summary>
        public Vector3 Velocity
        {
            get
            {
                return Source_getVelocity(Pointer);
            }
            set
            {
                Source_setVelocity(Pointer, value);
            }
        }

        /// <summary>
        /// The direction of the source.
        /// </summary>
        public Vector3 Direction
        {
            get
            {
                return Source_getDirection(Pointer);
            }
            set
            {
                Source_setDirection(Pointer, value);
            }
        }

        /// <summary>
        /// determines if the positions are relative to the listener
        /// default is AL_FALSE
        /// </summary>
        public bool SourceRelative
        {
            get
            {
                return Source_getSourceRelative(Pointer);
            }
            set
            {
                Source_setSourceRelative(Pointer, value);
            }
        }

        public float PlaybackPosition
        {
            get
            {
                return Source_getPlaybackPosition(Pointer);
            }
            set
            {
                Source_setPlaybackPosition(Pointer, value);
            }
        }

        /// <summary>
        /// Private callback for when the Source is finished playing and will be returned to the pool.
        /// </summary>
        /// <param name="source">The source that triggered the callback.</param>
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

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Source_playSound(IntPtr source, IntPtr sound);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Source_playing(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_stop(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_pause(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Source_resume(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Source_getLooping(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_rewind(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setPlaybackPosition(IntPtr source, float time);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Source_getPlaybackPosition(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setFinishedCallback(IntPtr source, SourceFinishedCallback callback);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setPitch(IntPtr source, float value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Source_getPitch(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setGain(IntPtr source, float value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Source_getGain(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setMinGain(IntPtr source, float value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Source_getMinGain(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setMaxGain(IntPtr source, float value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Source_getMaxGain(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setMaxDistance(IntPtr source, float value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Source_getMaxDistance(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setRolloffFactor(IntPtr source, float value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Source_getRolloffFactor(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setConeOuterGain(IntPtr source, float value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Source_getConeOuterGain(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setConeInnerAngle(IntPtr source, float value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Source_getConeInnerAngle(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setConeOuterAngle(IntPtr source, float value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Source_getConeOuterAngle(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setReferenceDistance(IntPtr source, float value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern float Source_getReferenceDistance(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setPosition(IntPtr source, Vector3 value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 Source_getPosition(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setVelocity(IntPtr source, Vector3 value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 Source_getVelocity(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setDirection(IntPtr source, Vector3 value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern Vector3 Source_getDirection(IntPtr source);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Source_setSourceRelative(IntPtr source, bool value);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Source_getSourceRelative(IntPtr source);

        #endregion
    }
}
