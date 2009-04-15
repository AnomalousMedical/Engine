using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlDouble : XmlValue<double>
    {
        public XmlDouble(XmlSaver xmlWriter)
            : base(xmlWriter, "Double")
        {

        }

        public override double parseValue(XmlReader xmlReader)
        {
            return xmlReader.ReadElementContentAsDouble();
        }
    }
}
