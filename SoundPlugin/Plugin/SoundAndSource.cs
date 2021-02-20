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
            Source.PlaybackFinished += Source_PlaybackFinished;
        }

        public Sound Sound { get; private set; }
        public Source Source { get; private set; }

        public void Dispose()
        {
            Source?.stop();
        }

        private void Source_PlaybackFinished(Source source)
        {
            openALManager.DestroySound(Sound);
            Source.PlaybackFinished -= Source_PlaybackFinished;
            openALManager = null;
            Sound = null;
            Source = null;
        }
    }
}
