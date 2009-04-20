using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;

namespace Engine.Saving
{
    /// <summary>
    /// This is a collection of ValueWriters for all supported types that can be
    /// written to a given stream type.
    /// </summary>
    public class ValueWriterCollection
    {
        private Dictionary<Type, ValueWriter> valueWriters = new Dictionary<Type, ValueWriter>();
        private ValueWriter saveableWriter;
        private ValueWriter enumWriter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="saveableWriter">The writer to use when a Saveable is encountered.</param>
        public ValueWriterCollection(ValueWriter saveableWriter, ValueWriter enumWriter)
        {
            this.saveableWriter = saveableWriter;
            this.enumWriter = enumWriter;
        }

        /// <summary>
        /// Add a ValueWriter for a new type.
        /// </summary>
        /// <param name="writer">The writer to register with a given type.</param>
        public void addValueWriter(ValueWriter writer)
        {
            valueWriters.Add(writer.getWriteType(), writer);
        }

        /// <summary>
        /// Remove a ValueWriter.
        /// </summary>
        /// <param name="writer">The ValueWriter to remove.</param>
        public void removeValueWriter(ValueWriter writer)
        {
            valueWriters.Remove(writer.getWriteType());
        }

        /// <summary>
        /// Write a given value to the stream using the correct writer.
        /// </summary>
        /// <param name="entry"></param>
        public void writeValue(SaveEntry entry)
        {
            Type objType = entry.ObjectType;
            if (valueWriters.ContainsKey(objType))
            {
                valueWriters[objType].writeValue(entry);
            }
            else if (objType.IsEnum)
            {
                enumWriter.writeValue(entry);
            }
            else if (objType == typeof(Saveable) || objType.GetInterface(typeof(Saveable).Name) != null)
            {
                saveableWriter.writeValue(entry);
            }
            else
            {
                Log.Default.sendMessage("Attempted to save a variable named {0} of type {1} that has no value writer. Object not written.", LogLevel.Warning, "Engine", entry.Name, entry.ObjectType.Name);
            }
        }
    }
}
