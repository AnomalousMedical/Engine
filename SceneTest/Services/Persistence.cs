﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SceneTest.Services
{
    class PersistenceEntry<T>
            where T : struct
    {
        private Dictionary<int, Dictionary<int, T>> entryDictionary = new Dictionary<int, Dictionary<int, T>>();

        public T GetData(int level, int key)
        {
            if (entryDictionary.TryGetValue(level, out var levelData))
            {
                if (levelData.TryGetValue(key, out var val))
                {
                    return (T)val;
                }
            }

            return default(T);
        }

        public void SetData(int level, int key, T value)
        {
            Dictionary<int, T> levelData;
            if (!entryDictionary.TryGetValue(level, out levelData))
            {
                levelData = new Dictionary<int, T>();
                entryDictionary[level] = levelData;
            }
            levelData[key] = value;
        }
    }

    class LevelStatus
    {
        public int CurrentLevelIndex { get; set; }
    }

    class Persistence
    {
        public PersistenceEntry<BattleTrigger.PersistenceData> BattleTriggers { get; } = new PersistenceEntry<BattleTrigger.PersistenceData>();

        public PersistenceEntry<TreasureTrigger.PersistenceData> TreasureTriggers { get; } = new PersistenceEntry<TreasureTrigger.PersistenceData>();

        public LevelStatus LevelStatus { get; set; } = new LevelStatus();
    }
}
