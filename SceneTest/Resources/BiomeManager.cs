using RpgMath;
using SceneTest.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Biome : IBiome
    {
        public string FloorTexture { get; set; }

        public string WallTexture { get; set; }

        public bool ReflectFloor { get; set; }

        public bool ReflectWall { get; set; }

        public BiomeEnemy GetEnemy(EnemyType type)
        {
            BiomeEnemy biomeEnemy;
            switch (type)
            {
                case EnemyType.Badass:
                    biomeEnemy = BadassEnemy;
                    break;
                case EnemyType.Peon:
                    biomeEnemy = PeonEnemy;
                    break;
                default:
                    biomeEnemy = RegularEnemy;
                    break;
            }

            return biomeEnemy ?? RegularEnemy;
        }

        public BiomeEnemy RegularEnemy { get; set; }

        /// <summary>
        /// Set this to control the badass version of the enemy separately. You will get a badass enemy
        /// stat-wise no matter what.
        /// </summary>
        public BiomeEnemy BadassEnemy { get; set; }

        /// <summary>
        /// Set this to control the peon version of the enemy separately. You will get a peon enemy
        /// stat-wise no matter what.
        /// </summary>
        public BiomeEnemy PeonEnemy { get; set; }

        /// <summary>
        /// The treasure to use for the biome.
        /// </summary>
        public BiomeTreasure Treasure { get; set; }
    }

    class BiomeEnemy
    {
        public ISpriteAsset Asset { get; set; }

        public IEnemyCurve EnemyCurve { get; set; }

        public Dictionary<Element, Resistance> Resistances { get; set; }
    }

    class BiomeTreasure
    {
        public ISpriteAsset Asset { get; set; }
    }

    class BiomeManager : IBiomeManager
    {
        private List<IBiome> biomes = new List<IBiome>()
        {
            //Countryside
            new Biome
            {
                FloorTexture = "cc0Textures/Rocks023_1K",
                WallTexture = "cc0Textures/Ground037_1K",
                RegularEnemy = new BiomeEnemy()
                {
                    Asset = new Assets.Original.TinyDino(),
                    EnemyCurve = new StandardEnemyCurve()
                },
                BadassEnemy = new BiomeEnemy()
                {
                    Asset = new Assets.Original.TinyDino(),
                    EnemyCurve = new StandardEnemyCurve()
                },
                PeonEnemy = new BiomeEnemy()
                {
                    Asset =  new Assets.Original.TinyDino(),
                    EnemyCurve = new StandardEnemyCurve()
                },
                Treasure = new BiomeTreasure()
                {
                    Asset = new Assets.Original.TreasureChest(),
                }
            },
            //Desert
            new Biome
            {
                FloorTexture = "cc0Textures/Ground025_1K",
                WallTexture = "cc0Textures/Rock029_1K",
                RegularEnemy = new BiomeEnemy()
                {
                    Asset = new Assets.Original.Skeleton(),
                    EnemyCurve = new StandardEnemyCurve(),
                    Resistances = new Dictionary<Element, Resistance>
                    {
                        { Element.Healing, Resistance.Absorb },
                        { Element.Fire, Resistance.Weak }
                    }
                },
                Treasure = new BiomeTreasure()
                {
                    Asset = new Assets.Original.TreasureChest(),
                }
            },
            //Snowy
            new Biome
            {
                FloorTexture = "cc0Textures/Snow006_1K",
                WallTexture = "cc0Textures/Rock022_1K",
                ReflectFloor = false,
                RegularEnemy = new BiomeEnemy()
                {
                    Asset = new Assets.Original.TinyDino(),
                    EnemyCurve = new StandardEnemyCurve()
                },
                BadassEnemy = new BiomeEnemy()
                {
                    Asset = new Assets.Original.TinyDino(),
                    EnemyCurve = new StandardEnemyCurve()
                },
                PeonEnemy = new BiomeEnemy()
                {
                    Asset =  new Assets.Original.TinyDino(),
                    EnemyCurve = new StandardEnemyCurve()
                },
                Treasure = new BiomeTreasure()
                {
                    Asset = new Assets.Original.TreasureChest(),
                }
            }
        };

        public IBiome GetBiome(int index)
        {
            return biomes[index];
        }

        public int Count => biomes.Count;
    }
}
