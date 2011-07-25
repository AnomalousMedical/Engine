using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlUInt : XmlValue<uint>
    {
        public XmlUInt(XmlSaver xmlWriter)
            : base(xmlWriter, "UInt")
        {

        }

        public override uint parseValue(XmlReader xmlReader)
        {
            return NumberParser.ParseUint(xmlReader.ReadElementContentAsString());
        }
    }
}
