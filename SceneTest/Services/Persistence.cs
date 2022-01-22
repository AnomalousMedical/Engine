using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SceneTest.Services
{
    class Persistence
    {
        public event Action<Persistence> DataModified;

        public PersistenceEntry<BattleTrigger.PersistenceData> BattleTriggers { get; }

        public PersistenceEntry<TreasureTrigger.PersistenceData> TreasureTriggers { get; }

        public LevelData Level { get; }

        public Persistence()
        {
            this.BattleTriggers = new PersistenceEntry<BattleTrigger.PersistenceData>(this);
            this.TreasureTriggers = new PersistenceEntry<TreasureTrigger.PersistenceData>(this);
            this.Level = new LevelData(this);
        }

        private void FireDataModified()
        {
            DataModified?.Invoke(this);
        }

        public class PersistenceEntry<T>
                where T : struct
        {
            private Persistence persistence;

            public PersistenceEntry(Persistence persistence)
            {
                this.persistence = persistence;
            }

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
                persistence.FireDataModified();
            }
        }

        public class LevelData
        {
            private Persistence persistence;

            public LevelData(Persistence persistence)
            {
                this.persistence = persistence;
            }

            private int _currentLevelIndex;
            public int CurrentLevelIndex
            {
                get
                {
                    return _currentLevelIndex;
                }
                set
                {
                    _currentLevelIndex = value;
                    persistence.FireDataModified();
                }
            }
        }
    }
}
