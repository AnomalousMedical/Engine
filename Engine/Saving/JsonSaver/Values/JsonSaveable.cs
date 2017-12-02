using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
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
            if (entry.Value != null)
            {
                writer.WriteValue(NumberParser.ToString(entry.ObjectID));
            }
            else
            {
                writer.WriteNull();
            }
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
