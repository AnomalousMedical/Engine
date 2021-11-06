namespace SceneTest
{
    interface IBiomeManager
    {
        int Count { get; }

        IBiome GetBiome(int index);
    }
}