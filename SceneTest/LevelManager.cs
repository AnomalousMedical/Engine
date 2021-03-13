using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class LevelManager : IDisposable
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

        public bool ChangingLevels => changingLevels;

        private List<(String floorTexture, String wallTexture)> biomes = new List<(String floorTexture, String wallTexture)>()
        {
            (
                floorTexture: "cc0Textures/Rocks023_1K",
                wallTexture: "cc0Textures/Ground037_1K"
            ),
            (
                floorTexture: "cc0Textures/Ground025_1K",
                wallTexture: "cc0Textures/Rock029_1K"
            ),
            (
                floorTexture: "cc0Textures/Snow006_1K",
                wallTexture: "cc0Textures/Rock022_1K"
            )
        };

        public LevelManager(Desc description, IObjectResolverFactory objectResolverFactory)
        {
            objectResolver = objectResolverFactory.Create();
            levelRandom = new Random(description.RandomSeed);
            createdLevelSeeds.Add(levelRandom.Next(int.MinValue, int.MaxValue));
            createdLevelSeeds.Add(levelRandom.Next(int.MinValue, int.MaxValue));
        }

        public async Task Initialize()
        {
            currentLevel = CreateLevel(createdLevelSeeds[0], new Vector3(0, 0, 0));
            nextLevel = CreateLevel(createdLevelSeeds[1], new Vector3(150, 0, 0));

            await currentLevel.WaitForLevelGeneration();

            currentLevel.SetupPhysics();

            player = this.objectResolver.Resolve<Player, Player.Description>(c =>
            {
                c.Translation = currentLevel.StartPoint;
            });

            this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeTinyDino(c);
                c.Translation = currentLevel.StartPoint + new Vector3(-4, 0, -1);
            });
            this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeTinyDino(c, skinMaterial: "cc0Textures/Leather011_1K");
                c.Translation = currentLevel.StartPoint + new Vector3(-5, 0, -2);
            });
            this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeSkeleton(c);
                c.Translation = currentLevel.StartPoint + new Vector3(0, 0, -3);
            });
            this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeTinyDino(c);
                c.Translation = currentLevel.StartPoint + new Vector3(-6, 0, -3);
            });

            await nextLevel.WaitForLevelGeneration();
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
            nextLevel = CreateLevel(levelSeed, new Vector3(150, 0, 0));

            //Physics changeover
            previousLevel.DestroyPhysics();
            previousLevel.SetPosition(new Vector3(-150, 0, 0));
            currentLevel.SetPosition(new Vector3(0, 0, 0));
            currentLevel.SetupPhysics();

            player.SetLocation(currentLevel.StartPoint);

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
                var levelSeed = createdLevelSeeds[currentLevelIndex - 1];
                previousLevel = CreateLevel(levelSeed, new Vector3(-150, 0, 0));
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

            player.SetLocation(currentLevel.StartPoint);

            changingLevels = false;
        }

        private Level CreateLevel(int levelSeed, Vector3 translation)
        {
            var random = new Random(levelSeed);
            var biome = biomes[random.Next(0, biomes.Count)];

            return this.objectResolver.Resolve<Level, Level.Description>(o =>
            {
                //o.MapUnitY = 1.0f;
                o.FloorTexture = biome.floorTexture;
                o.WallTexture = biome.wallTexture;

                o.Translation = translation;
                o.RandomSeed = levelSeed;
                o.Width = 50;
                o.Height = 50;
                o.CorridorSpace = 10;
                o.RoomDistance = 3;
                o.RoomMin = new IntSize2(2, 2);
                o.RoomMax = new IntSize2(6, 6); //Between 3-6 is good here, 3 for more cityish with small rooms, 6 for more open with more big rooms, sometimes connected
                o.CorridorMaxLength = 4;
            });
        }
    }
}
