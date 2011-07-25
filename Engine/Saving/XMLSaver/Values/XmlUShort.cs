using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlUShort : XmlValue<ushort>
    {
        public XmlUShort(XmlSaver xmlWriter)
            : base(xmlWriter, "UShort")
        {

        }

        public override ushort parseValue(XmlReader xmlReader)
        {
            return NumberParser.ParseUshort(xmlReader.ReadElementContentAsString());
        }
    }
}
