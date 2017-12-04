using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.Json
{
    class JsonLong : JsonValue<long>
    {
        public JsonLong(JsonSaver xmlWriter)
            : base(xmlWriter, "Long")
        {

        }

        public override void writeValue(long value, JsonWriter writer)
        {
            writer.WriteValue(NumberParser.ToString(value));
        }

        public override long parseValue(JsonReader xmlReader)
        {
            return NumberParser.ParseLong(xmlReader.ReadAsString());
        }
    }
}
