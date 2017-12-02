using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonShort : JsonValue<short>
    {
        public JsonShort(JsonSaver xmlWriter)
            : base(xmlWriter, "Short")
        {

        }

        public override void writeValue(short value, JsonWriter writer)
        {
            writer.WriteValue(value);
        }

        public override short parseValue(JsonReader xmlReader)
        {
            return (short)xmlReader.ReadAsInt32();
        }
    }
}
