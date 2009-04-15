using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    public class LoadInfo
    {
        private Dictionary<String, SaveEntry> entries = new Dictionary<String, SaveEntry>();

        public LoadInfo()
        {

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

        public Saveable GetValue(string name, Type type)
        {
            return (Saveable)entries[name].Value;
        }

        public T GetValue<T>(string name)
        {
            return (T)entries[name].Value;
        }

        internal void reset()
        {
            entries.Clear();
        }

        internal void addValue(string name, object value, Type objectType)
        {
            entries.Add(name, new SaveEntry(name, value, objectType));
        }
    }
}
