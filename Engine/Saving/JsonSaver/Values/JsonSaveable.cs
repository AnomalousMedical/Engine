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
            //JSON_XML_ISSUE
            //There is a discrepency here between xml and json, this always writes the fact that the value existed.
            //This could change how hasValue works since it won't always be written the same way, this might not matter, but need to investigate
            //Stuff that is using hasValue, likely it can be updated as well
            //That or fix this to write out something
            var writer = xmlSaver.Writer;
            if (entry.Value != null)
            {
                writer.WritePropertyName(entry.Name);
                writer.WriteValue(NumberParser.ToString(entry.ObjectID));
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
