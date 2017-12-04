using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.Json
{
    abstract class JsonValue<T> : ValueWriterEntry, JsonValueReader
    {
        protected const String NAME_ENTRY = "name";
        protected String elementName;

        protected JsonSaver xmlSaver;

        public JsonValue(JsonSaver xmlSaver, String elementName)
        {
            this.xmlSaver = xmlSaver;
            this.elementName = elementName;
        }

        public virtual void writeValue(SaveEntry entry)
        {
            var writer = xmlSaver.Writer;
            writer.WritePropertyName(entry.Name);
            writer.WriteStartObject();
            writer.WritePropertyName(elementName);
            if (entry.Value != null)
            {
                writeValue((T)entry.Value, writer);
            }
            else
            {
                writer.WriteNull();
            }
            writer.WriteEndObject();
        }

        public abstract void writeValue(T value, JsonWriter writer);

        public virtual void readValue(LoadControl loadControl, String name, JsonReader xmlReader)
        {
            loadControl.addValue(name, xmlReader.Value != null ? parseValue(xmlReader) : default(T), typeof(T));
        }

        public abstract T parseValue(JsonReader xmlReader);

        public String ElementName
        {
            get
            {
                return elementName;
            }
        }

        public Type getWriteType()
        {
            return typeof(T);
        }
    }
}
