using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlByte : XmlValue<byte>
    {
        public XmlByte(XmlSaver xmlWriter)
            : base(xmlWriter, "Byte")
        {

        }

        public override byte parseValue(XmlReader xmlReader)
        {
            return NumberParser.ParseByte(xmlReader.ReadElementContentAsString());
        }
    }
}
