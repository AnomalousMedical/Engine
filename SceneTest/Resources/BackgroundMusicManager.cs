using Engine;
using SoundPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class BackgroundMusicManager : IDisposable, IBackgroundMusicManager
    {
        private readonly VirtualFileSystem virtualFileSystem;
        private readonly SoundManager soundManager;
        private readonly ICoroutineRunner coroutineRunner;
        private SoundAndSource bgMusicSound;
        private SoundAndSource battleMusicSound;
        private bool bgMusicFinished = false;
        private String currentBackgroundSong;

        public BackgroundMusicManager(
            VirtualFileSystem virtualFileSystem, 
            SoundManager soundManager,
            ICoroutineRunner coroutineRunner)
        {
            this.virtualFileSystem = virtualFileSystem;
            this.soundManager = soundManager;
            this.coroutineRunner = coroutineRunner;
        }

        public void Dispose()
        {
            DisposeBgSound();
            battleMusicSound?.Dispose();
        }

        private void DisposeBgSound()
        {
            if (bgMusicSound != null)
            {
                bgMusicSound.Source.PlaybackFinished -= BgMusic_PlaybackFinished;
                bgMusicSound?.Dispose();
                bgMusicSound = null;
            }
        }

        public void SetBackgroundSong(String songFile)
        {
            DisposeBgSound();
            if (battleMusicSound == null && songFile != null)
            {
                var stream = virtualFileSystem.openStream(songFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                bgMusicSound = soundManager.StreamPlaySound(stream);
                bgMusicSound.Sound.Repeat = true;
                bgMusicSound.Source.PlaybackFinished += BgMusic_PlaybackFinished;
                bgMusicFinished = false;
            }
            currentBackgroundSong = songFile;
        }

        private void BgMusic_PlaybackFinished(Source source)
        {
            bgMusicFinished = true;
            IEnumerator<YieldAction> co()
            {
                yield return coroutineRunner.WaitSeconds(0);
                if (bgMusicFinished) //Double check that the song was not changed.
                {
                    SetBackgroundSong(currentBackgroundSong);
                }
            }
            coroutineRunner.Run(co());
        }

        public void SetBattleTrack(String songFile)
        {
            battleMusicSound?.Dispose();
            battleMusicSound = null;

            if (songFile == null)
            {
                if (bgMusicSound != null && !bgMusicFinished && !bgMusicSound.Source.Playing)
                {
                    bgMusicSound.Source.resume();
                }
                else if (bgMusicSound == null && currentBackgroundSong != null)
                {
                    //If we should play a song, but it hasn't started yet, this would happen if the bg music changes during a battle.
                    SetBackgroundSong(currentBackgroundSong);
                }
            }
            else
            {
                var stream = virtualFileSystem.openStream(songFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                battleMusicSound = soundManager.StreamPlaySound(stream);
                battleMusicSound.Sound.Repeat = true;

                if (bgMusicSound != null)
                {
                    bgMusicSound.Source.pause();
                }
            }
        }
    }
}
