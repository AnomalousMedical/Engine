using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonFloat : JsonValue<float>
    {
        public JsonFloat(JsonSaver xmlWriter)
            : base(xmlWriter, "Float")
        {

        }

        public override void writeValue(float value, JsonWriter writer)
        {
            writer.WriteValue(value);
        }

        public override float parseValue(JsonReader xmlReader)
        {
            return (float)xmlReader.ReadAsDouble().Value;
        }
    }
}
