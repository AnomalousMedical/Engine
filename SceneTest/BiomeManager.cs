using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Biome
    {
        public Biome(String floorTexture, String wallTexture)
        {
            FloorTexture = floorTexture;
            WallTexture = wallTexture;
        }

        public string FloorTexture { get; }
        public string WallTexture { get; }
    }

    class BiomeManager : IBiomeManager
    {
        private List<Biome> biomes = new List<Biome>()
        {
            new Biome(
                floorTexture: "cc0Textures/Rocks023_1K",
                wallTexture: "cc0Textures/Ground037_1K"
            ),
            new Biome(
                floorTexture: "cc0Textures/Ground025_1K",
                wallTexture: "cc0Textures/Rock029_1K"
            ),
            new Biome(
                floorTexture: "cc0Textures/Snow006_1K",
                wallTexture: "cc0Textures/Rock022_1K"
            )
        };

        public Biome GetBiome(int index)
        {
            return biomes[index];
        }

        public int Count => biomes.Count;
    }
}
