using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonSByte : JsonValue<sbyte>
    {
        public JsonSByte(JsonSaver xmlWriter)
            : base(xmlWriter, "SByte")
        {

        }

        public override string valueToString(sbyte value)
        {
            return NumberParser.ToString(value);
        }

        public override sbyte parseValue(JsonReader xmlReader)
        {
            return (sbyte)xmlReader.ReadAsInt32();
        }
    }
}
