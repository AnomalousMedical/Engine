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
        private SoundAndSource bgMusicSound;
        private SoundAndSource battleMusicSound;

        public BackgroundMusicManager(VirtualFileSystem virtualFileSystem, SoundManager soundManager)
        {
            this.virtualFileSystem = virtualFileSystem;
            this.soundManager = soundManager;
        }

        public void Dispose()
        {
            bgMusicSound?.Dispose();
            battleMusicSound?.Dispose();
        }

        public void SetBackgroundSong(String songFile)
        {
            bgMusicSound?.Dispose();
            var stream = virtualFileSystem.openStream(songFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            bgMusicSound = soundManager.StreamPlaySound(stream);
            bgMusicSound.Sound.Repeat = true;
        }

        public void SetBattleTrack(String songFile)
        {
            battleMusicSound?.Dispose();

            if (songFile == null)
            {
                if (bgMusicSound != null && !bgMusicSound.Source.Playing)
                {
                    bgMusicSound.Source.resume();
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
