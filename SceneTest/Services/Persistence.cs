using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataDictionary = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<object, object>>>;

namespace SceneTest.Services
{
    public class Persistence
    {
        private DataDictionary data = new DataDictionary();

        public T GetData<T>(int level, String type, object key)
            where T : struct
        {
            if (data.TryGetValue(level, out var levelData))
            {
                if (levelData.TryGetValue(type, out var typeData))
                {
                    if (typeData.TryGetValue(key, out var val))
                    {
                        return (T)val;
                    }
                }
            }

            return default(T);
        }

        public void SetData<T>(int level, String type, object key, T value)
        {
            Dictionary<string, Dictionary<object, object>> levelData;
            if (!data.TryGetValue(level, out levelData))
            {
                levelData = new Dictionary<string, Dictionary<object, object>>();
                data[level] = levelData;
            }
            Dictionary<object, object> typeData;
            if (!levelData.TryGetValue(type, out typeData))
            {
                typeData = new Dictionary<object, object>();
                levelData[type] = typeData;
            }
            typeData[key] = value;
        }

        public void DeleteData(int level, String type, object key)
        {
            if (data.TryGetValue(level, out var levelData))
            {
                if (levelData.TryGetValue(type, out var typeData))
                {
                    if (typeData.Remove(key))
                    {
                        if (typeData.Count == 0)
                        {
                            if (levelData.Remove(type))
                            {
                                if (levelData.Count == 0)
                                {
                                    data.Remove(level);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SaveData(Stream stream)
        {
            JsonSerializer.Serialize(stream, data, new JsonSerializerOptions()
            {
                WriteIndented = true,
            });
        }

        public void LoadData(Stream stream)
        {
            data = JsonSerializer.Deserialize<DataDictionary>(stream);
        }
    }
}
