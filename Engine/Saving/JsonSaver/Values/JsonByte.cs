
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonByte : JsonValue<byte>
    {
        public JsonByte(JsonSaver xmlWriter)
            : base(xmlWriter, "Byte")
        {

        }

        public override string valueToString(byte value)
        {
            return NumberParser.ToString(value);
        }

        public override byte parseValue(JsonReader xmlReader)
        {
            return (byte)xmlReader.ReadAsInt32();
        }
    }
}
