using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlString : XmlValue<String>
    {
        public XmlString(XmlSaver xmlWriter)
            : base(xmlWriter, "String")
        {

        }

        public override String parseValue(XmlReader xmlReader)
        {
            return xmlReader.ReadElementContentAsString();
        }
    }
}
