using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonBlob : JsonValue<byte[]>
    {
        private const string BYTE_SIZE_ENTRY = "NumBytes";

        public JsonBlob(JsonSaver xmlWriter)
            : base(xmlWriter, "Blob")
        {

        }

        public override void writeValue(byte[] value, JsonWriter writer)
        {
            //Assuming we can write an array and read it as bytes
            writer.WriteStartArray();
            foreach(var v in value)
            {
                writer.WriteValue(v);
            }
            writer.WriteEndArray();
        }

        public override byte[] parseValue(JsonReader xmlReader)
        {
            return xmlReader.ReadAsBytes();
        }
    }
}
