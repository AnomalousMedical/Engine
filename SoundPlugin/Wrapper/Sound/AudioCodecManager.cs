using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace SoundPlugin
{
    class AudioCodecManager : IDisposable
    {
        private WrapperCollection<AudioCodec> codecs;

        public AudioCodecManager()
        {
            codecs = new WrapperCollection<AudioCodec>(createWrapper, destroyWrapper);
        }

        public AudioCodec getCodec(IntPtr codec, OpenALManager openALManager)
        {
            if (codec != IntPtr.Zero)
            {
                return codecs.getObject(codec, openALManager);
            }
            return null;
        }

        public void Dispose()
        {
            codecs.clearObjects();
        }

        public IntPtr deleteWrapper(IntPtr widget)
        {
            return codecs.destroyObject(widget);
        }

        private AudioCodec createWrapper(IntPtr source, object[] args)
        {
            return new AudioCodec(source, (OpenALManager)args[0]);
        }

        private void destroyWrapper(AudioCodec source)
        {
            source.delete();
        }
    }
}
