using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace Engine.Saving
{
    /// <summary>
    /// This class contains the serialized info for a Saveable implementation.
    /// It will be provided to the constructor.
    /// </summary>
    public class LoadInfo
    {
        private Dictionary<String, SaveEntry> entries = new Dictionary<String, SaveEntry>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public LoadInfo()
        {

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

        public byte GetByte(string name)
        {
            return (byte)entries[name].Value;
        }

        public char GetChar(string name)
        {
            return (char)entries[name].Value;
        }

        public decimal GetDecimal(string name)
        {
            return (decimal)entries[name].Value;
        }

        public float GetFloat(string name)
        {
            return (float)entries[name].Value;
        }

        public double GetDouble(string name)
        {
            return (double)entries[name].Value;
        }

        public short GetInt16(string name)
        {
            return (short)entries[name].Value;
        }

        public int GetInt32(string name)
        {
            return (int)entries[name].Value;
        }

        public long GetInt64(string name)
        {
            return (long)entries[name].Value;
        }

        public sbyte GetSByte(string name)
        {
            return (sbyte)entries[name].Value;
        }

        public float GetSingle(string name)
        {
            return (float)entries[name].Value;
        }

        public string GetString(string name)
        {
            return (string)entries[name].Value;
        }

        public ushort GetUInt16(string name)
        {
            return (ushort)entries[name].Value;
        }

        public uint GetUInt32(string name)
        {
            return (uint)entries[name].Value;
        }

        public ulong GetUInt64(string name)
        {
            return (ulong)entries[name].Value;
        }

        public Quaternion GetQuaternion(string name)
        {
            return (Quaternion)entries[name].Value;
        }

        public Vector3 GetVector3(string name)
        {
            return (Vector3)entries[name].Value;
        }

        public Color GetColor(string name)
        {
            return (Color)entries[name].Value;
        }

        public Saveable GetValue(string name, Type type)
        {
            return (Saveable)entries[name].Value;
        }

        public T GetValue<T>(string name)
        {
            return (T)entries[name].Value;
        }

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
