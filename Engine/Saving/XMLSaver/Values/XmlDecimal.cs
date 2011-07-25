using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlDecimal : XmlValue<decimal>
    {
        public XmlDecimal(XmlSaver xmlWriter)
            : base(xmlWriter, "Decimal")
        {

        }

        public override string valueToString(decimal value)
        {
            return NumberParser.ToString(value);
        }

        public override decimal parseValue(XmlReader xmlReader)
        {
            return xmlReader.ReadElementContentAsDecimal();
        }
    }
}
