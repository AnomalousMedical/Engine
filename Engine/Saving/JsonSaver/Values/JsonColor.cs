using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonColor : JsonValue<Color>
    {
        public JsonColor(JsonSaver xmlWriter)
            : base(xmlWriter, "Color")
        {

        }

        public override string valueToString(Color value)
        {
            return value.ToString();
        }

        public override Color parseValue(XmlReader xmlReader)
        {
            return new Color(xmlReader.ReadElementContentAsString());
        }
    }
}
