using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SoundPlugin
{
    public unsafe class AudioCodec : SoundPluginObject
    {
        private OpenALManager manager;

        internal AudioCodec(IntPtr codec, OpenALManager manager)
            : base(codec)
        {
            this.manager = manager;
        }

        public int NumChannels
        {
            get
            {
                return AudioCodec_getNumChannels(Pointer);
            }
        }

        public int SamplingFrequency
        {
            get
            {
                return AudioCodec_getSamplingFrequency(Pointer);
            }
        }

        public double Duration
        {
            get
            {
                return AudioCodec_getDuration(Pointer);
            }
        }

        #region PInvoke

        [DllImport("SoundWrapper")]
        private static extern int AudioCodec_getNumChannels(IntPtr codec);

        [DllImport("SoundWrapper")]
        private static extern int AudioCodec_getSamplingFrequency(IntPtr codec);

        [DllImport("SoundWrapper")]
        private static extern UIntPtr AudioCodec_read(IntPtr codec, char* buffer, int length);

        [DllImport("SoundWrapper")]
        private static extern void AudioCodec_close(IntPtr codec);

        [DllImport("SoundWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool AudioCodec_eof(IntPtr codec);

        [DllImport("SoundWrapper")]
        private static extern void AudioCodec_seekToStart(IntPtr codec);

        [DllImport("SoundWrapper")]
        private static extern double AudioCodec_getDuration(IntPtr codec);

        #endregion 
    }
}
