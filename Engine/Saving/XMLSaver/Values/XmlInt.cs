using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlInt : XmlValue<int>
    {
        public XmlInt(XmlSaver xmlWriter)
            : base(xmlWriter, "Int")
        {

        }

        public override int parseValue(XmlReader xmlReader)
        {
            return xmlReader.ReadElementContentAsInt();
        }
    }
}
