using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonDecimal : JsonValue<decimal>
    {
        public JsonDecimal(JsonSaver xmlWriter)
            : base(xmlWriter, "Decimal")
        {

        }

        public override void writeValue(decimal value, JsonWriter writer)
        {
            writer.WriteValue(value);
        }

        public override decimal parseValue(JsonReader xmlReader)
        {
            return xmlReader.ReadAsDecimal().Value;
        }
    }
}
