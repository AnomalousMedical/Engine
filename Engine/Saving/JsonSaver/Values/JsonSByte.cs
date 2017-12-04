using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.Json
{
    class JsonSByte : JsonValue<sbyte>
    {
        public JsonSByte(JsonSaver xmlWriter)
            : base(xmlWriter, "SByte")
        {

        }

        public override void writeValue(sbyte value, JsonWriter writer)
        {
            writer.WriteValue(value);
        }

        public override sbyte parseValue(JsonReader xmlReader)
        {
            return (sbyte)xmlReader.ReadAsInt32();
        }
    }
}
