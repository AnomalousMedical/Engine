using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonDouble : JsonValue<double>
    {
        public JsonDouble(JsonSaver xmlWriter)
            : base(xmlWriter, "Double")
        {

        }

        public override string valueToString(double value)
        {
            return NumberParser.ToString(value);
        }

        public override double parseValue(XmlReader xmlReader)
        {
            return xmlReader.ReadElementContentAsDouble();
        }
    }
}
