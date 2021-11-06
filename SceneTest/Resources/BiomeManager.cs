using RpgMath;
using SceneTest.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Biome
    {
        public string FloorTexture { get; set; }

        public string WallTexture { get; set; }

        public BiomeEnemy RegularEnemy { get; set; }

        public BiomeEnemy BadassEnemy { get; set; }

        public BiomeEnemy PeonEnemy { get; set; }
    }

    class BiomeEnemy
    {
        public ISpriteAsset Asset { get; set; }

        public IEnemyCurve EnemyCurve { get; set; }
    }

    class BiomeManager : IBiomeManager
    {
        private List<Biome> biomes = new List<Biome>()
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
                    Asset = new Assets.Original.TinyDino()
                    {
                        SkinMaterial = "cc0Textures/Leather011_1K"
                    },
                    EnemyCurve = new StandardEnemyCurve()
                },
                PeonEnemy = new BiomeEnemy()
                {
                    Asset =  new Assets.Original.TinyDino(),
                    EnemyCurve = new StandardEnemyCurve()
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
                    EnemyCurve = new StandardEnemyCurve()
                },
                BadassEnemy = new BiomeEnemy()
                {
                    Asset = new Assets.Original.Skeleton(),
                    EnemyCurve = new StandardEnemyCurve()
                },
                PeonEnemy = new BiomeEnemy()
                {
                    Asset =  new Assets.Original.Skeleton(),
                    EnemyCurve = new StandardEnemyCurve()
                }
            },
            //Snowy
            new Biome
            {
                FloorTexture = "cc0Textures/Snow006_1K",
                WallTexture = "cc0Textures/Rock022_1K",
                RegularEnemy = new BiomeEnemy()
                {
                    Asset = new Assets.Original.TinyDino(),
                    EnemyCurve = new StandardEnemyCurve()
                },
                BadassEnemy = new BiomeEnemy()
                {
                    Asset = new Assets.Original.TinyDino()
                    {
                        SkinMaterial = "cc0Textures/Leather011_1K"
                    },
                    EnemyCurve = new StandardEnemyCurve()
                },
                PeonEnemy = new BiomeEnemy()
                {
                    Asset =  new Assets.Original.TinyDino(),
                    EnemyCurve = new StandardEnemyCurve()
                }
            }
        };

        public Biome GetBiome(int index)
        {
            return biomes[index];
        }

        public int Count => biomes.Count;
    }
}
