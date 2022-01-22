using BepuPlugin;
using Engine;
using SceneTest.Exploration;
using SceneTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class LevelManager : IDisposable, ILevelManager
    {
        private bool changingLevels = false;
        private Level currentLevel;
        private Level nextLevel;
        private Level previousLevel;

        private Player player;
        private IObjectResolver objectResolver;
        private readonly Party party;
        private readonly IWorldManager worldManager;
        private readonly Persistence persistence;

        public event Action<ILevelManager> LevelChanged;

        public bool ChangingLevels => changingLevels;

        public Level CurrentLevel => currentLevel;

        public bool IsPlayerMoving => player?.IsMoving == true;

        public LevelManager(
            Party party,
            IWorldManager worldManager,
            IObjectResolverFactory objectResolverFactory,
            IBackgroundMusicManager backgroundMusicManager,
            Persistence persistence,
            IBepuScene bepuScene //Inject this so it is created earlier and destroyed later
        )
        {
            objectResolver = objectResolverFactory.Create();

            backgroundMusicManager.SetBackgroundSong("freepd/Rafael Krux - Black Knight.ogg");
            this.party = party;
            this.worldManager = worldManager;
            this.persistence = persistence;
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

            var currentLevelIndex = persistence.Level.CurrentLevelIndex;
            currentLevel = CreateLevel(worldManager.GetLevelSeed(currentLevelIndex), new Vector3(0, 0, 0), currentLevelIndex);
            nextLevel = CreateLevel(worldManager.GetLevelSeed(currentLevelIndex + 1), new Vector3(150, 0, 0), currentLevelIndex + 1);
            if(currentLevelIndex - 1 >= 0)
            {
                previousLevel = CreateLevel(worldManager.GetLevelSeed(currentLevelIndex - 1), new Vector3(-150, 0, 0), currentLevelIndex - 1);
            }

            await currentLevel.WaitForLevelGeneration();

            currentLevel.SetupPhysics();

            if (player == null)
            {
                player = this.objectResolver.Resolve<Player, Player.Description>(c =>
                {
                    c.Translation = currentLevel.StartPoint;
                    var leader = party.ActiveCharacters.First();
                    c.PlayerSpriteInfo = leader.PlayerSprite;
                    c.PrimaryHandItem = leader.PrimaryHandAsset;
                    c.SecondaryHandItem = leader.SecondaryHandAsset;
                });
            }
            else
            {
                player.SetLocation(currentLevel.StartPoint);
            }

            LevelChanged?.Invoke(this);

            await nextLevel.WaitForLevelGeneration();
            var nextOffset = currentLevel.LocalEndPoint - nextLevel.LocalStartPoint;
            nextLevel.SetPosition(new Vector3(150, nextOffset.y, nextOffset.z));
            if (previousLevel != null)
            {
                await previousLevel.WaitForLevelGeneration();
                var previousOffset = currentLevel.LocalStartPoint - previousLevel.LocalEndPoint;
                previousLevel.SetPosition(new Vector3(-150, previousOffset.y, previousOffset.z));
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
            ++persistence.Level.CurrentLevelIndex;
            var nextLevelIndex = persistence.Level.CurrentLevelIndex + 1;
            var levelSeed = worldManager.GetLevelSeed(nextLevelIndex);

            //Create new level
            nextLevel = CreateLevel(levelSeed, new Vector3(150, 0, 0), nextLevelIndex);

            //Physics changeover
            previousLevel.DestroyPhysics();
            var previousOffset = currentLevel.LocalStartPoint - previousLevel.LocalEndPoint;
            previousLevel.SetPosition(new Vector3(-150, previousOffset.y, previousOffset.z));
            currentLevel.SetPosition(new Vector3(0, 0, 0));
            currentLevel.SetupPhysics();

            var playerLoc = player.GetLocation();
            playerLoc += new Vector3(-150f, previousOffset.y, previousOffset.z);
            player.SetLocation(playerLoc);

            LevelChanged?.Invoke(this);

            changingLevels = false;

            //Keep this last after setting changingLevels
            await nextLevel.WaitForLevelGeneration();
            var nextOffset = currentLevel.LocalEndPoint - nextLevel.LocalStartPoint;
            nextLevel.SetPosition(new Vector3(150, nextOffset.y, nextOffset.z));
        }

        public async Task GoPreviousLevel()
        {
            if (changingLevels)
            {
                return;
            }

            //Change level index
            --persistence.Level.CurrentLevelIndex;
            if (persistence.Level.CurrentLevelIndex < 0)
            {
                //Below 0, do nothing
                persistence.Level.CurrentLevelIndex = 0;
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

            if (persistence.Level.CurrentLevelIndex > 0)
            {
                var previousLevelIndex = persistence.Level.CurrentLevelIndex - 1;
                var levelSeed = worldManager.GetLevelSeed(previousLevelIndex);
                previousLevel = CreateLevel(levelSeed, new Vector3(-150, 0, 0), previousLevelIndex);
            }
            else
            {
                previousLevel = null;
            }

            //Physics changeover
            nextLevel.DestroyPhysics();
            var nextOffset = currentLevel.LocalEndPoint - nextLevel.LocalStartPoint;
            nextLevel.SetPosition(new Vector3(150, nextOffset.y, nextOffset.z));
            currentLevel.SetPosition(new Vector3(0, 0, 0));
            currentLevel.SetupPhysics();

            var playerLoc = player.GetLocation();
            playerLoc += new Vector3(150f, nextOffset.y, nextOffset.z);
            player.SetLocation(playerLoc);

            LevelChanged?.Invoke(this);

            changingLevels = false;

            //Keep this last after the changingLevels = false;
            if(previousLevel != null)
            {
                await previousLevel.WaitForLevelGeneration();
                var previousOffset = currentLevel.LocalStartPoint - previousLevel.LocalEndPoint;
                previousLevel.SetPosition(new Vector3(-150, previousOffset.y, previousOffset.z));
            }
        }

        private Level CreateLevel(int levelSeed, Vector3 translation, int levelIndex)
        {
            return this.objectResolver.Resolve<Level, Level.Description>(o =>
            {
                o.Index = levelIndex;
                o.Translation = translation;
                o.RandomSeed = levelSeed;
                o.Width = 50;
                o.Height = 50;
                o.CorridorSpace = 10;
                o.RoomDistance = 3;
                o.RoomMin = new IntSize2(2, 2);
                o.RoomMax = new IntSize2(6, 6); //Between 3-6 is good here, 3 for more cityish with small rooms, 6 for more open with more big rooms, sometimes connected
                o.CorridorMaxLength = 4;
                o.GoPrevious = levelIndex != 0;
            });
        }

        public void GoStartPoint()
        {
            player.SetLocation(currentLevel.StartPoint);
        }

        public void GoEndPoint()
        {
            player.SetLocation(currentLevel.EndPoint);
        }

        public void StopPlayer()
        {
            player.StopMovement();
        }

        public void Rest()
        {
            currentLevel.DestroyPhysics();
            currentLevel.SetupPhysics();
        }
    }
}
