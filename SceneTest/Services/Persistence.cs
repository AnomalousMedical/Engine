using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataDictionary = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<object, object>>>;

namespace SceneTest.Services
{
    public class Persistence
    {
        private DataDictionary data = new DataDictionary();

        public T GetData<T>(String type, int level, object key)
            where T : struct
        {
            if (data.TryGetValue(type, out var typeData))
            {
                if (typeData.TryGetValue(level, out var levelData))
                {
                    if (levelData.TryGetValue(key, out var val))
                    {
                        return (T)val;
                    }
                }
            }

            return default(T);
        }

        public void SetData<T>(String type, int level, object key, T value)
        {
            Dictionary<int, Dictionary<object, object>> typeData;
            if (!data.TryGetValue(type, out typeData))
            {
                typeData = new Dictionary<int, Dictionary<object, object>>();
                data[type] = typeData;
            }
            Dictionary<object, object> levelData;
            if (!typeData.TryGetValue(level, out levelData))
            {
                levelData = new Dictionary<object, object>();
                typeData[level] = levelData;
            }
            levelData[key] = value;
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
