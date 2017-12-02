using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonUShort : JsonValue<ushort>
    {
        public JsonUShort(JsonSaver xmlWriter)
            : base(xmlWriter, "UShort")
        {

        }

        public override string valueToString(ushort value)
        {
            return NumberParser.ToString(value);
        }

        public override ushort parseValue(XmlReader xmlReader)
        {
            return NumberParser.ParseUshort(xmlReader.ReadElementContentAsString());
        }
    }
}
