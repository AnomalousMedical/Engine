using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlULong : XmlValue<ulong>
    {
        public XmlULong(XmlSaver xmlWriter)
            : base(xmlWriter, "ULong")
        {

        }

        public override ulong parseValue(XmlReader xmlReader)
        {
            return ulong.Parse(xmlReader.ReadElementContentAsString());
        }
    }
}
