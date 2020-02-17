using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Logging;

namespace SoundPlugin
{
    public class SoundManager
    {
        private Dictionary<Source, Sound> oneTimeSounds = new Dictionary<Source, Sound>();
        private OpenALManager openALManager;

        internal SoundManager(OpenALManager openALManager)
        {
            this.openALManager = openALManager;
        }

        public Source streamPlayAndForgetSound(Stream soundStream)
        {
            Source source = openALManager.getSource();
            if (source != null)
            {
                Sound sound = openALManager.createStreamingSound(soundStream);
                oneTimeSounds.Add(source, sound);
                source.PlaybackFinished += source_PlaybackFinished;
                source.playSound(sound);
                return source;
            }
            else
            {
                Log.Error("Ran out of sources trying to play sound.");
            }
            return null;
        }

        public double getDuration(Stream soundStream)
        {
            AudioCodec codec = openALManager.createAudioCodec(soundStream);
            double duration = codec.Duration;
            openALManager.destroyAudioCodec(codec);
            return duration;
        }

        /// <summary>
        /// Open a capture device, you must dispose it when you are done.
        /// </summary>
        /// <returns>A new open capture device.</returns>
        public CaptureDevice openCaptureDevice(BufferFormat format = BufferFormat.Stereo16, int bufferSeconds = 5, int rate = 44100)
        {
            return openALManager.createCaptureDevice(format, bufferSeconds, rate);
        }

        void source_PlaybackFinished(Source source)
        {
            source.PlaybackFinished -= source_PlaybackFinished;
            Sound sound = oneTimeSounds[source];
            openALManager.destroySound(sound);
            oneTimeSounds.Remove(source);
        }
    }
}
