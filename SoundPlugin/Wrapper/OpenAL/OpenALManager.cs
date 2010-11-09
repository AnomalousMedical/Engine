using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace SoundPlugin
{
    public class OpenALManager : IDisposable
    {
        private IntPtr openALManager;

        internal IntPtr Pointer
        {
            get
            {
                return openALManager;
            }
        }

        public OpenALManager()
        {
            openALManager = OpenALManager_create();
        }

        public void Dispose()
        {
            OpenALManager_destroy(openALManager);
        }

        public Sound createMemorySound(Stream stream)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createMemorySound(openALManager, managedStream.Pointer));
        }

        public Sound createStreamingSound(Stream stream)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createStreamingSound(openALManager, managedStream.Pointer));
        }

        public Sound createStreamingSound(Stream stream, int bufferSize, int numBuffers)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createStreamingSound2(openALManager, managedStream.Pointer, bufferSize, numBuffers));
        }

        public void destroySound(Sound sound)
        {
            OpenALManager_destroySound(openALManager, sound.Pointer);
        }

        #region PInvoke

        [DllImport("SoundWrapper")]
        private static extern IntPtr OpenALManager_create();

        [DllImport("SoundWrapper")]
        private static extern void OpenALManager_destroy(IntPtr openALManager);

        [DllImport("SoundWrapper")]
        private static extern IntPtr OpenALManager_createMemorySound(IntPtr openALManager, IntPtr stream);

        [DllImport("SoundWrapper")]
        private static extern IntPtr OpenALManager_createStreamingSound(IntPtr openALManager, IntPtr stream);

        [DllImport("SoundWrapper")]
        private static extern IntPtr OpenALManager_createStreamingSound2(IntPtr openALManager, IntPtr stream, int bufferSize, int numBuffers);

        [DllImport("SoundWrapper")]
        private static extern void OpenALManager_destroySound(IntPtr openALManager, IntPtr sound);

        #endregion
    }
}
