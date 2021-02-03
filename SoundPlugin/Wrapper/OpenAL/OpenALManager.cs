using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace SoundPlugin
{
    public enum BufferFormat
    {
        Mono8 = 0,
        Mono16 = 1,
        Stereo8 = 2,
        Stereo16 = 3,
    }

    public class OpenALManager : SoundPluginObject, IDisposable
    {
        private SourceManager sourceManager = new SourceManager();
        //private ManagedLogListener managedLogListener = new ManagedLogListener();
        private Listener listener;
        private AudioCodecManager codecManager = new AudioCodecManager();
        private CaptureDeviceManager captureDeviceManager = new CaptureDeviceManager();

        public OpenALManager(SoundState soundState)
            :base(OpenALManager_create())
        {
            listener = new Listener(OpenALManager_getListener(Pointer));
            soundState.MasterVolumeChanged += SoundState_MasterVolumeChanged;
            SoundState_MasterVolumeChanged(soundState);
        }

        private void SoundState_MasterVolumeChanged(SoundState sender)
        {
            listener.Gain = sender.MasterVolume;
        }

        public void Dispose()
        {
            sourceManager.Dispose();
            OpenALManager_destroy(Pointer);
            delete();
            //managedLogListener.Dispose();
        }

        public Listener GetListener()
        {
            return listener;
        }

        public CaptureDevice CreateCaptureDevice(BufferFormat format = BufferFormat.Stereo16, int bufferSeconds = 5, int rate = 44100)
        {
            return captureDeviceManager.get(OpenALManager_createCaptureDevice(Pointer, format, bufferSeconds, rate), this);
        }

        public void DestroyCaptureDevice(CaptureDevice captureDevice)
        {
            IntPtr ptr = captureDevice.Pointer;
            captureDeviceManager.deleteWrapper(ptr);
            OpenALManager_destroyCaptureDevice(Pointer, ptr);
        }

        public AudioCodec CreateAudioCodec(Stream stream)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return codecManager.getCodec(OpenALManager_createAudioCodec(Pointer, managedStream.Pointer), this);
        }

        public void DestroyAudioCodec(AudioCodec codec)
        {
            IntPtr codecPointer = codec.Pointer;
            codecManager.deleteWrapper(codec.Pointer);
            OpenALManager_destroyAudioCodec(Pointer, codecPointer);
        }

        public Sound CreateMemorySound(Stream stream)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createMemorySound(Pointer, managedStream.Pointer));
        }

        public Sound CreateMemorySound(AudioCodec codec)
        {
            return new Sound(OpenALManager_createMemorySoundCodec(Pointer, codec.Pointer));
        }

        public Sound CreateStreamingSound(Stream stream)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createStreamingSound(Pointer, managedStream.Pointer));
        }

        public Sound CreateStreamingSound(AudioCodec codec)
        {
            return new Sound(OpenALManager_createStreamingSoundCodec(Pointer, codec.Pointer));
        }

        public Sound CreateStreamingSound(Stream stream, int bufferSize, int numBuffers)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createStreamingSound2(Pointer, managedStream.Pointer, bufferSize, numBuffers));
        }

        public Sound CreateStreamingSound(AudioCodec codec, int bufferSize, int numBuffers)
        {
            return new Sound(OpenALManager_createStreamingSound2Codec(Pointer, codec.Pointer, bufferSize, numBuffers));
        }

        public void DestroySound(Sound sound)
        {
            OpenALManager_destroySound(Pointer, sound.Pointer);
            sound.delete();
        }

        public Source GetSource()
        {
            return sourceManager.getSource(OpenALManager_getSource(Pointer));
        }

        public void Update()
        {
            OpenALManager_update(Pointer);
        }

        /// <summary>
        /// Resume app wide audio playback. Called when internal resources need to be recreated.
        /// </summary>
        internal void ResumeAudio()
        {
            OpenALManager_resumeAudio(Pointer);
        }

        /// <summary>
        /// Suspend app wide audio playback, called when internal resources need to be destroyed.
        /// </summary>
        internal void SuspendAudio()
        {
            OpenALManager_suspendAudio(Pointer);
        }

        #region PInvoke

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_create();

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OpenALManager_destroy(IntPtr openALManager);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createCaptureDevice(IntPtr openALManager, BufferFormat format, int bufferSeconds, int rate);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OpenALManager_destroyCaptureDevice(IntPtr openALManager, IntPtr captureDevice);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createAudioCodec(IntPtr openALManager, IntPtr stream);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OpenALManager_destroyAudioCodec(IntPtr openALManager, IntPtr codec);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createMemorySound(IntPtr openALManager, IntPtr stream);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createMemorySoundCodec(IntPtr openALManager, IntPtr codec);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createStreamingSound(IntPtr openALManager, IntPtr stream);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createStreamingSoundCodec(IntPtr openALManager, IntPtr codec);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createStreamingSound2(IntPtr openALManager, IntPtr stream, int bufferSize, int numBuffers);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_createStreamingSound2Codec(IntPtr openALManager, IntPtr codec, int bufferSize, int numBuffers);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OpenALManager_destroySound(IntPtr openALManager, IntPtr sound);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_getSource(IntPtr openALManager);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OpenALManager_update(IntPtr openALManager);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OpenALManager_getListener(IntPtr openALManager);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OpenALManager_resumeAudio(IntPtr openALManager);

        [DllImport(SoundPluginInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OpenALManager_suspendAudio(IntPtr openALManager);

        #endregion
    }
}
