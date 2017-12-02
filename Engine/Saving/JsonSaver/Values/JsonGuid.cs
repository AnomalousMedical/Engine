using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonGuid : JsonValue<Guid>
    {
        public JsonGuid(JsonSaver xmlWriter)
            : base(xmlWriter, "Guid")
        {

        }

        public override string valueToString(Guid value)
        {
            return value.ToString();
        }

        public override Guid parseValue(XmlReader xmlReader)
        {
            return new Guid(xmlReader.ReadElementContentAsString());
        }
    }
}
