using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlQuaternion : XmlValue<Quaternion>
    {
        public XmlQuaternion(XmlSaver xmlWriter)
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
