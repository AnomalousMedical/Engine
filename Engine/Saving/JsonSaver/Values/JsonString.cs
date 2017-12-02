using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonString : JsonValue<String>
    {
        public JsonString(JsonSaver xmlWriter)
            : base(xmlWriter, "String")
        {

        }

        public override void writeValue(String value, JsonWriter writer)
        {
            writer.WriteValue(value);
        }

        public override String parseValue(JsonReader xmlReader)
        {
            return xmlReader.ReadAsString();
        }
    }
}
