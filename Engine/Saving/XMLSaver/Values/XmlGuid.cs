using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlGuid : XmlValue<Guid>
    {
        public XmlGuid(XmlSaver xmlWriter)
            : base(xmlWriter, "Guid")
        {

        }

        public override string valueToString(Guid value)
        {
            return value.ToString();
        }

        public override Guid parseValue(XmlReader xmlReader)
        {
            return new Guid(xmlReader.ReadElementContentAsString());
        }
    }
}
