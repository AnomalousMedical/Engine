using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;

namespace Engine.Saving
{
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

        public void addValueWriter(ValueWriter writer)
        {
            valueWriters.Add(writer.getWriteType(), writer);
        }

        public void removeValueWriter(ValueWriter writer)
        {
            valueWriters.Remove(writer.getWriteType());
        }

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
            else if (objType.GetInterface(typeof(Saveable).Name) != null)
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
