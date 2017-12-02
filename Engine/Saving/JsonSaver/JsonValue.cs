using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
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
            //JSON_XML_ISSUE
            //There is a discrepency here between xml and json, this always writes the fact that the value existed.
            //This could change how hasValue works since it won't always be written the same way, this might not matter, but need to investigate
            //Stuff that is using hasValue, likely it can be updated as well
            //That or fix this to write out something
            var writer = xmlSaver.Writer;
            if(entry.Value != null)
            {
                writer.WritePropertyName(entry.Name);
                writeValue((T)entry.Value, writer);
            }
        }

        public abstract void writeValue(T value, JsonWriter writer);

        public virtual void readValue(LoadControl loadControl, String name, JsonReader xmlReader)
        {
            loadControl.addValue(name, parseValue(xmlReader), typeof(T));
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
