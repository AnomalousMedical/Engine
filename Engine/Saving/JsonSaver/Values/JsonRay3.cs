using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonRay3 : JsonValue<Ray3>
    {
        public JsonRay3(JsonSaver xmlWriter)
            : base(xmlWriter, "Ray3")
        {

        }

        public override string valueToString(Ray3 value)
        {
            return value.ToString();
        }

        public override Ray3 parseValue(XmlReader xmlReader)
        {
            return new Ray3(xmlReader.ReadElementContentAsString());
        }
    }
}
