using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlFloat : XmlValue<float>
    {
        public XmlFloat(XmlSaver xmlWriter)
            : base(xmlWriter, "Float")
        {

        }

        public override float parseValue(XmlReader xmlReader)
        {
            return xmlReader.ReadElementContentAsFloat();
        }
    }
}
