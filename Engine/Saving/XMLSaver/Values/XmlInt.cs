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

        public override string valueToString(int value)
        {
            return NumberParser.ToString(value);
        }

        public override int parseValue(XmlReader xmlReader)
        {
            return xmlReader.ReadElementContentAsInt();
        }
    }
}
