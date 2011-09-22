using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace SoundPlugin
{
    public class OpenALManager : SoundPluginObject, IDisposable
    {
        private SourceManager sourceManager = new SourceManager();
        private ManagedLogListener managedLogListener = new ManagedLogListener();
        private Listener listener;
        private AudioCodecManager codecManager = new AudioCodecManager();

        public OpenALManager()
            :base(OpenALManager_create())
        {
            listener = new Listener(OpenALManager_getListener(Pointer));
            SoundConfig.MasterVolumeChanged += new EventHandler(SoundConfig_MasterVolumeChanged);
        }

        void SoundConfig_MasterVolumeChanged(object sender, EventArgs e)
        {
            listener.Gain = SoundConfig.MasterVolume;
        }

        public void Dispose()
        {
            sourceManager.Dispose();
            OpenALManager_destroy(Pointer);
            delete();
            managedLogListener.Dispose();
        }

        public Listener getListener()
        {
            return listener;
        }

        public AudioCodec createAudioCodec(Stream stream)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return codecManager.getCodec(OpenALManager_createAudioCodec(Pointer, managedStream.Pointer), this);
        }

        public void destroyAudioCodec(AudioCodec codec)
        {
            IntPtr codecPointer = codec.Pointer;
            codecManager.deleteWrapper(codec.Pointer);
            OpenALManager_destroyAudioCodec(Pointer, codecPointer);
        }

        public Sound createMemorySound(Stream stream)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createMemorySound(Pointer, managedStream.Pointer));
        }

        public Sound createMemorySound(AudioCodec codec)
        {
            return new Sound(OpenALManager_createMemorySoundCodec(Pointer, codec.Pointer));
        }

        public Sound createStreamingSound(Stream stream)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createStreamingSound(Pointer, managedStream.Pointer));
        }

        public Sound createStreamingSound(AudioCodec codec)
        {
            return new Sound(OpenALManager_createStreamingSoundCodec(Pointer, codec.Pointer));
        }

        public Sound createStreamingSound(Stream stream, int bufferSize, int numBuffers)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createStreamingSound2(Pointer, managedStream.Pointer, bufferSize, numBuffers));
        }

        public Sound createStreamingSound(AudioCodec codec, int bufferSize, int numBuffers)
        {
            return new Sound(OpenALManager_createStreamingSound2Codec(Pointer, codec.Pointer, bufferSize, numBuffers));
        }

        public void destroySound(Sound sound)
        {
            OpenALManager_destroySound(Pointer, sound.Pointer);
            sound.delete();
        }

        public Source getSource()
        {
            return sourceManager.getSource(OpenALManager_getSource(Pointer));
        }

        public void update()
        {
            OpenALManager_update(Pointer);
        }

        #region PInvoke

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_create();

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OpenALManager_destroy(IntPtr openALManager);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createAudioCodec(IntPtr openALManager, IntPtr stream);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OpenALManager_destroyAudioCodec(IntPtr openALManager, IntPtr codec);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createMemorySound(IntPtr openALManager, IntPtr stream);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createMemorySoundCodec(IntPtr openALManager, IntPtr codec);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createStreamingSound(IntPtr openALManager, IntPtr stream);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createStreamingSoundCodec(IntPtr openALManager, IntPtr codec);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createStreamingSound2(IntPtr openALManager, IntPtr stream, int bufferSize, int numBuffers);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createStreamingSound2Codec(IntPtr openALManager, IntPtr codec, int bufferSize, int numBuffers);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OpenALManager_destroySound(IntPtr openALManager, IntPtr sound);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_getSource(IntPtr openALManager);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void OpenALManager_update(IntPtr openALManager);

        [DllImport("SoundWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_getListener(IntPtr openALManager);

        #endregion
    }
}
