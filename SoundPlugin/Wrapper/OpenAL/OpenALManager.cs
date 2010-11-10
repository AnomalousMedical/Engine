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

        public OpenALManager()
            :base(OpenALManager_create())
        {
            
        }

        public void Dispose()
        {
            sourceManager.Dispose();
            OpenALManager_destroy(Pointer);
            delete();
        }

        public Sound createMemorySound(Stream stream)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createMemorySound(Pointer, managedStream.Pointer));
        }

        public Sound createStreamingSound(Stream stream)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createStreamingSound(Pointer, managedStream.Pointer));
        }

        public Sound createStreamingSound(Stream stream, int bufferSize, int numBuffers)
        {
            ManagedStream managedStream = new ManagedStream(stream);
            return new Sound(OpenALManager_createStreamingSound2(Pointer, managedStream.Pointer, bufferSize, numBuffers));
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

        [DllImport("SoundWrapper")]
        private static extern IntPtr OpenALManager_getSource(IntPtr openALManager);

        [DllImport("SoundWrapper")]
        private static extern void OpenALManager_update(IntPtr openALManager);

        #endregion
    }
}
