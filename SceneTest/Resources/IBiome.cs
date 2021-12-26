using RpgMath;

namespace SceneTest
{
    interface IBiome
    {
        string FloorTexture { get; set; }
        string WallTexture { get; set; }
        bool ReflectFloor { get; }

        BiomeEnemy GetEnemy(EnemyType type);
    }
}