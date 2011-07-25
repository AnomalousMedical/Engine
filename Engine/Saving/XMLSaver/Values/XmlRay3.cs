using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlRay3 : XmlValue<Ray3>
    {
        public XmlRay3(XmlSaver xmlWriter)
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
