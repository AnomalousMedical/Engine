using BepuPlugin;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class LevelManager : IDisposable, ILevelManager
    {
        public class Desc
        {
            public int RandomSeed { get; set; } = 0;
        }

        private bool changingLevels = false;
        private List<int> createdLevelSeeds = new List<int>();
        private int currentLevelIndex = 0;
        private Random levelRandom;
        private Level currentLevel;
        private Level nextLevel;
        private Level previousLevel;

        private Player player;
        private IObjectResolver objectResolver;
        private readonly Party party;
        private readonly IBackgroundMusicManager backgroundMusicManager;
        private readonly IBepuScene bepuScene;

        public event Action<ILevelManager> LevelChanged;

        public bool ChangingLevels => changingLevels;

        public Level CurrentLevel => currentLevel;

        public bool IsPlayerMoving => player?.IsMoving == true;

        public LevelManager(
            Desc description,
            Party party,
            IObjectResolverFactory objectResolverFactory,
            IBackgroundMusicManager backgroundMusicManager,
            IBepuScene bepuScene //Inject this so it is created earlier and destroyed later
        )
        {
            objectResolver = objectResolverFactory.Create();
            levelRandom = new Random(description.RandomSeed);
            createdLevelSeeds.Add(levelRandom.Next(int.MinValue, int.MaxValue));
            createdLevelSeeds.Add(levelRandom.Next(int.MinValue, int.MaxValue));

            backgroundMusicManager.SetBackgroundSong("freepd/Rafael Krux - Black Knight.ogg");
            this.party = party;
            this.backgroundMusicManager = backgroundMusicManager;
            this.bepuScene = bepuScene;
        }

        public async Task Restart()
        {
            if (changingLevels)
            {
                return;
            }

            changingLevels = true;

            if(previousLevel != null)
            {
                await previousLevel.WaitForLevelGeneration();
                previousLevel.RequestDestruction();
            }

            if(currentLevel != null)
            {
                await currentLevel.WaitForLevelGeneration();
                currentLevel.RequestDestruction();
            }

            if(nextLevel != null)
            {
                await nextLevel.WaitForLevelGeneration();
                nextLevel.RequestDestruction();
            }

            currentLevelIndex = 0;
            currentLevel = CreateLevel(createdLevelSeeds[currentLevelIndex], new Vector3(0, 0, 0), false);
            nextLevel = CreateLevel(createdLevelSeeds[currentLevelIndex + 1], new Vector3(150, 0, 0), true);
            if(currentLevelIndex - 1 >= 0)
            {
                previousLevel = CreateLevel(createdLevelSeeds[currentLevelIndex - 1], new Vector3(-150, 0, 0), true);
            }

            await currentLevel.WaitForLevelGeneration();

            currentLevel.SetupPhysics();

            if (player == null)
            {
                player = this.objectResolver.Resolve<Player, Player.Description>(c =>
                {
                    c.Translation = currentLevel.StartPoint;
                    c.PlayerSpriteInfo = party.ActiveCharacters.First().PlayerSprite;
                });
            }
            else
            {
                player.SetLocation(currentLevel.StartPoint);
            }

            LevelChanged?.Invoke(this);

            await nextLevel.WaitForLevelGeneration();
            if(previousLevel != null)
            {
                await previousLevel.WaitForLevelGeneration();
            }

            changingLevels = false;
        }

        public void Dispose()
        {
            objectResolver.Dispose();
        }

        public Task WaitForCurrentLevel()
        {
            return currentLevel?.WaitForLevelGeneration();
        }

        public Task WaitForNextLevel()
        {
            return nextLevel?.WaitForLevelGeneration();
        }

        public Task WaitForPreviousLevel()
        {
            return previousLevel?.WaitForLevelGeneration();
        }

        public async Task GoNextLevel()
        {
            if (changingLevels)
            {
                return;
            }

            changingLevels = true;
            if (previousLevel != null)
            {
                await previousLevel.WaitForLevelGeneration(); //This is pretty unlikely, but have to stop here if level isn't created yet
            }
            await nextLevel.WaitForLevelGeneration(); //Also unlikely, but next level might not be loaded yet

            //Shuffle levels
            previousLevel?.RequestDestruction();
            previousLevel = currentLevel;
            currentLevel = nextLevel;

            //Change level index
            ++currentLevelIndex;
            var nextLevelIndex = currentLevelIndex + 1;
            if (nextLevelIndex == createdLevelSeeds.Count)
            {
                createdLevelSeeds.Add(levelRandom.Next(int.MinValue, int.MaxValue));
            }
            var levelSeed = createdLevelSeeds[nextLevelIndex];

            //Create new level
            nextLevel = CreateLevel(levelSeed, new Vector3(150, 0, 0), true);

            //Physics changeover
            previousLevel.DestroyPhysics();
            previousLevel.SetPosition(new Vector3(-150, 0, 0));
            currentLevel.SetPosition(new Vector3(0, 0, 0));
            currentLevel.SetupPhysics();

            player.SetLocation(currentLevel.StartPoint);

            LevelChanged?.Invoke(this);

            changingLevels = false;
        }

        public async Task GoPreviousLevel()
        {
            if (changingLevels)
            {
                return;
            }

            //Change level index
            --currentLevelIndex;
            if (currentLevelIndex < 0)
            {
                //Below 0, do nothing
                currentLevelIndex = 0;
                return;
            }

            changingLevels = true;
            if (previousLevel != null)
            {
                await previousLevel.WaitForLevelGeneration(); //This is pretty unlikely, but have to stop here if level isn't created yet
            }
            await nextLevel.WaitForLevelGeneration(); //Also unlikely, but next level might not be loaded yet

            //Shuffle levels
            nextLevel?.RequestDestruction();
            nextLevel = currentLevel;
            currentLevel = previousLevel;

            if (currentLevelIndex > 0)
            {
                var previousLevelIndex = currentLevelIndex - 1;
                var levelSeed = createdLevelSeeds[previousLevelIndex];
                previousLevel = CreateLevel(levelSeed, new Vector3(-150, 0, 0), previousLevelIndex > 0);
            }
            else
            {
                previousLevel = null;
            }

            //Physics changeover
            nextLevel.DestroyPhysics();
            nextLevel.SetPosition(new Vector3(150, 0, 0));
            currentLevel.SetPosition(new Vector3(0, 0, 0));
            currentLevel.SetupPhysics();

            player.SetLocation(currentLevel.EndPoint);

            LevelChanged?.Invoke(this);

            changingLevels = false;
        }

        private Level CreateLevel(int levelSeed, Vector3 translation, bool goPrevious)
        {
            return this.objectResolver.Resolve<Level, Level.Description>(o =>
            {
                o.Translation = translation;
                o.RandomSeed = levelSeed;
                o.Width = 50;
                o.Height = 50;
                o.CorridorSpace = 10;
                o.RoomDistance = 3;
                o.RoomMin = new IntSize2(2, 2);
                o.RoomMax = new IntSize2(6, 6); //Between 3-6 is good here, 3 for more cityish with small rooms, 6 for more open with more big rooms, sometimes connected
                o.CorridorMaxLength = 4;
                o.GoPrevious = goPrevious;
            });
        }

        public void StopPlayer()
        {
            player.StopMovement();
        }
    }
}
