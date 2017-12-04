using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.Json
{
    class JsonBool : JsonValue<bool>
    {
        public JsonBool(JsonSaver xmlWriter)
            : base(xmlWriter, "Bool")
        {

        }

        public override void writeValue(bool value, JsonWriter writer)
        {
            writer.WriteValue(value);
        }

        public override bool parseValue(JsonReader xmlReader)
        {
            return xmlReader.ReadAsBoolean().Value;
        }
    }
}
