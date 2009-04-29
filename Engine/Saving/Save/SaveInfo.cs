﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace Engine.Saving
{
    /// <summary>
    /// This class is used to save the contents of an object to a stream to be
    /// loaded later.
    /// </summary>
    public class SaveInfo
    {
        private Dictionary<String, SaveEntry> entries = new Dictionary<String, SaveEntry>();
        private SaveControl writer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="writer">The SaveControl to call back to.</param>
        public SaveInfo(SaveControl writer)
        {
            this.writer = writer;
        }

        public void AddValue(string name, bool value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(bool)));
        }

        public void AddValue(string name, byte value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(byte)));
        }

        public void AddValue(string name, char value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(char)));
        }

        public void AddValue(string name, decimal value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(decimal)));
        }

        public void AddValue(string name, double value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(double)));
        }

        public void AddValue(string name, float value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(float)));
        }

        public void AddValue(string name, int value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(int)));
        }

        public void AddValue(string name, long value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(long)));
        }

        public void AddValue(string name, Saveable value)
        {
            if (value != null)
            {
                validate(name, value);
                long objectID = writer.saveObject(value);
                entries.Add(name, new SaveEntry(name, value, value.GetType(), objectID));
            }
        }

        public void AddValue(string name, Enum value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, value.GetType()));
        }

        public void AddValue(string name, sbyte value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(sbyte)));
        }

        public void AddValue(string name, short value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(short)));
        }

        public void AddValue(string name, uint value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(uint)));
        }

        public void AddValue(string name, ulong value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(ulong)));
        }

        public void AddValue(string name, ushort value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(ushort)));
        }

        public void AddValue(String name, String value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(String)));
        }

        public void AddValue(string name, Vector3 value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(Vector3)));
        }

        public void AddValue(string name, Quaternion value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(Quaternion)));
        }

        public void AddValue(string name, Ray3 value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(Ray3)));
        }

        public void AddValue(string name, Identifier value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(Identifier)));
        }

        public void AddValue(string name, Color value)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, typeof(Color)));
        }

        /// <summary>
        /// Internal function to be used by the ReflectedSaver to add a type.
        /// This is not public because the interface is designed to only accept
        /// valid classes.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        /// <param name="objectType">The type of the variable</param>
        internal void AddReflectedValue(string name, Object value, Type objectType)
        {
            validate(name, value);
            entries.Add(name, new SaveEntry(name, value, objectType));
        }

        /// <summary>
        /// Get an iterator over all values.
        /// </summary>
        /// <returns>An iterator over all values.</returns>
        internal IEnumerable<SaveEntry> saveEntryIterator()
        {
            return entries.Values;
        }

        /// <summary>
        /// Clear the entries in this SaveInfo. Used so these can be recycled.
        /// </summary>
        internal void clear()
        {
            entries.Clear();
        }

        /// <summary>
        /// Validate helper function.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void validate(String name, object value)
        {
            if (entries.ContainsKey(name))
            {
                throw new SaveException(String.Format("Attempted to add a duplicate value {0}", name));
            }
        }
    }
}
