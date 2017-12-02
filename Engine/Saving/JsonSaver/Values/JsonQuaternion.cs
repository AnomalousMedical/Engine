using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonQuaternion : JsonValue<Quaternion>
    {
        public JsonQuaternion(JsonSaver xmlWriter)
            : base(xmlWriter, "Quaternion")
        {

        }

        public override string valueToString(Quaternion value)
        {
            return value.ToString();
        }

        public override Quaternion parseValue(XmlReader xmlReader)
        {
            return new Quaternion(xmlReader.ReadElementContentAsString());
        }
    }
}
