using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlVector3 : XmlValue<Vector3>
    {
        public XmlVector3(XmlSaver xmlWriter)
            : base(xmlWriter, "Vector3")
        {

        }

        public override Vector3 parseValue(XmlReader xmlReader)
        {
            return new Vector3(xmlReader.ReadElementContentAsString());
        }
    }
}
