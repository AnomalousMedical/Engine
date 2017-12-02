using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonULong : JsonValue<ulong>
    {
        public JsonULong(JsonSaver xmlWriter)
            : base(xmlWriter, "ULong")
        {

        }

        public override string valueToString(ulong value)
        {
            return NumberParser.ToString(value);
        }

        public override ulong parseValue(JsonReader xmlReader)
        {
            return NumberParser.ParseUlong(xmlReader.ReadAsString());
        }
    }
}
