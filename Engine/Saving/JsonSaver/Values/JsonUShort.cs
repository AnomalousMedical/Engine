using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.Json
{
    class JsonUShort : JsonValue<ushort>
    {
        public JsonUShort(JsonSaver xmlWriter)
            : base(xmlWriter, "UShort")
        {

        }

        public override void writeValue(ushort value, JsonWriter writer)
        {
            writer.WriteValue(value);
        }

        public override ushort parseValue(JsonReader xmlReader)
        {
            return (ushort)xmlReader.ReadAsInt32();
        }
    }
}
