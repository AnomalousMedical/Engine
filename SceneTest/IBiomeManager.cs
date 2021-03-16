namespace SceneTest
{
    interface IBiomeManager
    {
        int Count { get; }

        Biome GetBiome(int index);
    }
}