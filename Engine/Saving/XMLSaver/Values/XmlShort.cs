using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlShort : XmlValue<short>
    {
        public XmlShort(XmlSaver xmlWriter)
            : base(xmlWriter, "Short")
        {

        }

        public override short parseValue(XmlReader xmlReader)
        {
            return (short)xmlReader.ReadElementContentAsInt();
        }
    }
}
