using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonUInt : JsonValue<uint>
    {
        public JsonUInt(JsonSaver xmlWriter)
            : base(xmlWriter, "UInt")
        {

        }

        public override void writeValue(uint value, JsonWriter writer)
        {
            writer.WriteValue((int)value);
        }

        public override uint parseValue(JsonReader xmlReader)
        {
            return (uint)xmlReader.ReadAsInt32(); //This *should* be ok, still a 32 bit number.
        }
    }
}
