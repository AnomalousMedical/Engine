using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlChar : XmlValue<char>
    {
        public XmlChar(XmlSaver xmlWriter)
            : base(xmlWriter, "Char")
        {

        }

        public override char parseValue(XmlReader xmlReader)
        {
            return char.Parse(xmlReader.ReadElementContentAsString());
        }
    }
}
