using RpgMath;

namespace SceneTest
{
    interface IBiome
    {
        string FloorTexture { get; set; }
        string WallTexture { get; set; }

        BiomeEnemy GetEnemy(EnemyType type);
    }
}