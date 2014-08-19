using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// This class contains the serialized info for a Saveable implementation.
    /// It will be provided to the constructor.
    /// </summary>
    public class LoadInfo
    {
        private Dictionary<String, SaveEntry> entries = new Dictionary<String, SaveEntry>();
        private TypeFinder typeFinder;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LoadInfo(TypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
        }

        /// <summary>
        /// Determine if a value exists in this load info by name.
        /// </summary>
        /// <param name="name">The name of the value to check for.</param>
        /// <returns>True if the value exists. False if it does not.</returns>
        public bool hasValue(String name)
        {
            return entries.ContainsKey(name);
        }

        public bool GetBoolean(string name)
        {
            return (bool)entries[name].Value;
        }

        public bool GetBoolean(string name, bool defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (bool)retVal.Value;
            }
            return defaultValue;
        }

        public byte GetByte(string name)
        {
            return (byte)entries[name].Value;
        }

        public byte GetByte(string name, byte defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (byte)retVal.Value;
            }
            return defaultValue;
        }

        public char GetChar(string name)
        {
            return (char)entries[name].Value;
        }

        public char GetChar(string name, char defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (char)retVal.Value;
            }
            return defaultValue;
        }

        public decimal GetDecimal(string name)
        {
            return (decimal)entries[name].Value;
        }

        public decimal GetDecimal(string name, decimal defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (decimal)retVal.Value;
            }
            return defaultValue;
        }

        public float GetFloat(string name)
        {
            return (float)entries[name].Value;
        }

        public float GetFloat(string name, float defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (float)retVal.Value;
            }
            return defaultValue;
        }

        public double GetDouble(string name)
        {
            return (double)entries[name].Value;
        }

        public double GetDouble(string name, double defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (double)retVal.Value;
            }
            return defaultValue;
        }

        public short GetInt16(string name)
        {
            return (short)entries[name].Value;
        }

        public short GetInt16(string name, short defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (short)retVal.Value;
            }
            return defaultValue;
        }

        public int GetInt32(string name)
        {
            return (int)entries[name].Value;
        }

        public int GetInt32(string name, int defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (int)retVal.Value;
            }
            return defaultValue;
        }

        public long GetInt64(string name)
        {
            return (long)entries[name].Value;
        }

        public long GetInt64(string name, long defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (long)retVal.Value;
            }
            return defaultValue;
        }

        public sbyte GetSByte(string name)
        {
            return (sbyte)entries[name].Value;
        }

        public sbyte GetSByte(string name, sbyte defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (sbyte)retVal.Value;
            }
            return defaultValue;
        }

        public float GetSingle(string name)
        {
            return (float)entries[name].Value;
        }

        public float GetSingle(string name, float defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (float)retVal.Value;
            }
            return defaultValue;
        }

        public string GetString(string name)
        {
            return (string)entries[name].Value;
        }

        public string GetString(string name, string defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (string)retVal.Value;
            }
            return defaultValue;
        }

        public ushort GetUInt16(string name)
        {
            return (ushort)entries[name].Value;
        }

        public ushort GetUInt16(string name, ushort defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (ushort)retVal.Value;
            }
            return defaultValue;
        }

        public uint GetUInt32(string name)
        {
            return (uint)entries[name].Value;
        }

        public uint GetUInt32(string name, uint defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (uint)retVal.Value;
            }
            return defaultValue;
        }

        public ulong GetUInt64(string name)
        {
            return (ulong)entries[name].Value;
        }

        public ulong GetUInt64(string name, ulong defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (ulong)retVal.Value;
            }
            return defaultValue;
        }

        public Quaternion GetQuaternion(string name)
        {
            return (Quaternion)entries[name].Value;
        }

        public Quaternion GetQuaternion(string name, Quaternion defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (Quaternion)retVal.Value;
            }
            return defaultValue;
        }

        public Vector3 GetVector3(string name)
        {
            return (Vector3)entries[name].Value;
        }

        public Vector3 GetVector3(string name, Vector3 defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (Vector3)retVal.Value;
            }
            return defaultValue;
        }

        public Color GetColor(string name)
        {
            return (Color)entries[name].Value;
        }

        public Color GetColor(string name, Color defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (Color)retVal.Value;
            }
            return defaultValue;
        }

        public byte[] GetBlob(String name)
        {
            return (byte[])entries[name].Value;
        }

        public byte[] GetBlob(string name, byte[] defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (byte[])retVal.Value;
            }
            return defaultValue;
        }

        public Guid GetGuid(string name)
        {
            return (Guid)entries[name].Value;
        }

        public Guid GetGuid(string name, Guid defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (Guid)retVal.Value;
            }
            return defaultValue;
        }

        public Saveable GetValue(string name, Type type)
        {
            return (Saveable)entries[name].Value;
        }

        public Saveable GetTYPE(string name, Saveable defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (Saveable)retVal.Value;
            }
            return defaultValue;
        }

        public T GetValue<T>(string name)
        {
            return (T)entries[name].Value;
        }

        public T GetValue<T>(string name, T defaultValue)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (T)retVal.Value;
            }
            return defaultValue;
        }

        /// <summary>
        /// This function allows the caller to defer creating a default value unless it is really needed. You can also do anything in your callback, the system does not further process
        /// the data if the callback is used.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="name">The name of the object to retrieve.</param>
        /// <param name="defaultValueRetriver">A function to call to provide the default value.</param>
        /// <returns></returns>
        public T GetValueCb<T>(String name, Func<T> defaultValueRetriver)
        {
            SaveEntry retVal;
            if (entries.TryGetValue(name, out retVal))
            {
                return (T)retVal.Value;
            }
            return defaultValueRetriver();
        }

        public void RebuildDictionary<KeyType, ValueType>(String baseName, Dictionary<KeyType, ValueType> dictionary)
        {
            String keyBase = baseName + "Key";
            String valueBase = baseName + "Value";
            for (int i = 0; entries.ContainsKey(keyBase + i); ++i)
            {
                dictionary.Add((KeyType)entries[keyBase + i].Value, (ValueType)entries[valueBase + i].Value);
            }
        }

        public void RebuildList<ValueType>(String baseName, List<ValueType> values)
        {
            for (int i = 0; entries.ContainsKey(baseName + i); ++i)
            {
                values.Add((ValueType)entries[baseName + i].Value);
            }
        }

        public void RebuildLinkedList<ValueType>(String baseName, LinkedList<ValueType> values)
        {
            for (int i = 0; entries.ContainsKey(baseName + i); ++i)
            {
                values.AddLast((ValueType)entries[baseName + i].Value);
            }
        }

        public TypeFinder TypeFinder
        {
            get
            {
                return typeFinder;
            }
        }

        /// <summary>
        /// The version of the object's LoadInfo. This has no effect within the save system, but you can check
        /// it in your own constructor to take action on old versions.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Internal function to just get the value directly as an object. Used
        /// by the ReflectedSaver.
        /// </summary>
        /// <param name="name">The name of the object to get.</param>
        /// <returns>The object matching name.</returns>
        internal Object getValueObject(string name)
        {
            return entries[name].Value;
        }

        /// <summary>
        /// Internal function to reset this object so it can be pooled.
        /// </summary>
        internal void reset()
        {
            entries.Clear();
            Version = 0;
        }

        /// <summary>
        /// Internal function to add a value. This keeps the LoadInfos read only.
        /// </summary>
        /// <param name="name">The name of the value to add.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="objectType">The object type of the value.</param>
        internal void addValue(string name, object value, Type objectType)
        {
            entries.Add(name, new SaveEntry(name, value, objectType));
        }
    }
}
