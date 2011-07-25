using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlSByte : XmlValue<sbyte>
    {
        public XmlSByte(XmlSaver xmlWriter)
            : base(xmlWriter, "SByte")
        {

        }

        public override string valueToString(sbyte value)
        {
            return NumberParser.ToString(value);
        }

        public override sbyte parseValue(XmlReader xmlReader)
        {
            return NumberParser.ParseSbyte(xmlReader.ReadElementContentAsString());
        }
    }
}
