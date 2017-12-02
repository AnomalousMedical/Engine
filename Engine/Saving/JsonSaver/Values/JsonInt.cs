using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonInt : JsonValue<int>
    {
        public JsonInt(JsonSaver xmlWriter)
            : base(xmlWriter, "Int")
        {

        }

        public override void writeValue(int value, JsonWriter writer)
        {
            writer.WriteValue(value);
        }

        public override int parseValue(JsonReader xmlReader)
        {
            return xmlReader.ReadAsInt32().Value;
        }
    }
}
