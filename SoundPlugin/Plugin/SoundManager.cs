using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Extensions.Logging;

namespace SoundPlugin
{
    public class SoundManager
    {
        private Dictionary<Source, Sound> oneTimeSounds = new Dictionary<Source, Sound>();
        private OpenALManager openALManager;
        private readonly ILogger<SoundManager> logger;

        public SoundManager(OpenALManager openALManager, ILogger<SoundManager> logger)
        {
            this.openALManager = openALManager;
            this.logger = logger;
        }

        public Source StreamPlayAndForgetSound(Stream soundStream)
        {
            Source source = openALManager.GetSource();
            if (source != null)
            {
                Sound sound = openALManager.CreateStreamingSound(soundStream);
                oneTimeSounds.Add(source, sound);
                source.PlaybackFinished += source_PlaybackFinished;
                source.playSound(sound);
                return source;
            }
            else
            {
                logger.LogError("Ran out of sources trying to play sound.");
            }
            return null;
        }

        /// <summary>
        /// Play a sound, the returned item must be disposed when the caller is done with it.
        /// </summary>
        /// <param name="soundStream"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public SoundAndSource StreamPlaySound(Stream soundStream)
        {
            Source source = openALManager.GetSource();
            if (source != null)
            {
                Sound sound = openALManager.CreateStreamingSound(soundStream);
                source.playSound(sound);
                return new SoundAndSource(sound, source, openALManager);
            }
            else
            {
                logger.LogError("Ran out of sources trying to play sound.");
            }
            return null;
        }

        public double GetDuration(Stream soundStream)
        {
            AudioCodec codec = openALManager.CreateAudioCodec(soundStream);
            double duration = codec.Duration;
            openALManager.DestroyAudioCodec(codec);
            return duration;
        }

        /// <summary>
        /// Open a capture device, you must dispose it when you are done.
        /// </summary>
        /// <returns>A new open capture device.</returns>
        public CaptureDevice OpenCaptureDevice(BufferFormat format = BufferFormat.Stereo16, int bufferSeconds = 5, int rate = 44100)
        {
            return openALManager.CreateCaptureDevice(format, bufferSeconds, rate);
        }

        void source_PlaybackFinished(Source source)
        {
            source.PlaybackFinished -= source_PlaybackFinished;
            Sound sound = oneTimeSounds[source];
            openALManager.DestroySound(sound);
            oneTimeSounds.Remove(source);
        }
    }
}
