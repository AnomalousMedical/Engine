using System;
using System.Collections.Generic;
using System.Text;

namespace SoundPlugin
{
    public class SoundAndSource : IDisposable
    {
        private OpenALManager openALManager;

        public SoundAndSource(Sound sound, Source source, OpenALManager openALManager)
        {
            Sound = sound;
            Source = source;
            this.openALManager = openALManager;
        }

        public Sound Sound { get; private set; }
        public Source Source { get; private set; }

        public void Dispose()
        {
            if (Source?.Playing == true)
            {
                Source.stop();
            }
            openALManager?.DestroySound(Sound);
            openALManager = null;
            Sound = null;
            Source = null;
        }
    }
}
