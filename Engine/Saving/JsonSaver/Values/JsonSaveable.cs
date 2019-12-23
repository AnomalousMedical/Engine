using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.Json
{
    class JsonSaveable : JsonValue<Saveable>
    {
        private const String OBJECT_ID = "id";

        public JsonSaveable(JsonSaver xmlWriter)
            :base(xmlWriter, "Saveable")
        {

        }

        public override void writeValue(SaveEntry entry)
        {
            var writer = xmlSaver.Writer;
            writer.WritePropertyName(entry.Name);
            var format = writer.Formatting;
            writer.Formatting = Newtonsoft.Json.Formatting.None;
            writer.WriteStartObject();
            writer.WritePropertyName(elementName);
            if (entry.Value != null)
            {
                writer.WriteValue(NumberParser.ToString(entry.ObjectID));
            }
            else
            {
                writer.WriteNull();
            }
            writer.WriteEndObject();
            writer.Formatting = format;
        }

        public override void writeValue(Saveable value, JsonWriter writer)
        {
            throw new NotImplementedException("Should not be called overwrote writeValue");
        }

        //Format { "Saveable": "Id" }

        public override void readValue(LoadControl loadControl, String name, JsonReader xmlReader)
        {
            loadControl.addObjectValue(name, NumberParser.ParseLong(xmlReader.ReadAsString()));
        }

        public override Saveable parseValue(JsonReader xmlReader)
        {
            throw new SaveException("This parseValue function should never be called");
        }
    }
}
