using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.Json
{
    class JsonDouble : JsonValue<double>
    {
        public JsonDouble(JsonSaver xmlWriter)
            : base(xmlWriter, "Double")
        {

        }

        public override void writeValue(double value, JsonWriter writer)
        {
            writer.WriteValue(value);
        }

        public override double parseValue(JsonReader xmlReader)
        {
            return xmlReader.ReadAsDouble().Value;
        }
    }
}
