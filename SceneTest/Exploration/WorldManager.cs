using SceneTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Exploration
{
    interface IWorldManager
    {
        int GetLevelSeed(int index);
    }

    class WorldManager : IWorldManager
    {
        private List<int> createdLevelSeeds = new List<int>();
        private Random levelRandom;

        public WorldManager
        (
            Persistence persistence
        )
        {
            levelRandom = new Random(persistence.World.Seed);
        }

        public int GetLevelSeed(int index)
        {
            var end = index + 1;
            for (var i = createdLevelSeeds.Count; i < end; ++i)
            {
                createdLevelSeeds.Add(levelRandom.Next(int.MinValue, int.MaxValue));
            }
            return createdLevelSeeds[index];
        }
    }
}
