using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonString : JsonValue<String>
    {
        public JsonString(JsonSaver xmlWriter)
            : base(xmlWriter, "String")
        {

        }

        public override string valueToString(String value)
        {
            return value;
        }

        public override String parseValue(XmlReader xmlReader)
        {
            return xmlReader.ReadElementContentAsString();
        }
    }
}
