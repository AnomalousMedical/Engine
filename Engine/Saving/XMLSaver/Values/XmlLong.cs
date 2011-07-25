using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlLong : XmlValue<long>
    {
        public XmlLong(XmlSaver xmlWriter)
            : base(xmlWriter, "Long")
        {

        }

        public override string valueToString(long value)
        {
            return NumberParser.ToString(value);
        }

        public override long parseValue(XmlReader xmlReader)
        {
            return xmlReader.ReadElementContentAsLong();
        }
    }
}
