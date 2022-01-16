using RpgMath;

namespace SceneTest
{
    interface IBiome
    {
        string FloorTexture { get; set; }
        string WallTexture { get; set; }
        bool ReflectFloor { get; }
        bool ReflectWall { get; }
        BiomeTreasure Treasure { get; set; }

        BiomeEnemy GetEnemy(EnemyType type);
    }
}