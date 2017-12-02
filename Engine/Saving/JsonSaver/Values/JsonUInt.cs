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

        public override string valueToString(uint value)
        {
            return NumberParser.ToString(value);
        }

        public override uint parseValue(XmlReader xmlReader)
        {
            return NumberParser.ParseUint(xmlReader.ReadElementContentAsString());
        }
    }
}
