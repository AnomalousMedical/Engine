using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlIdentifier : XmlValue<Identifier>
    {
        public XmlIdentifier(XmlSaver xmlWriter)
            : base(xmlWriter, "Identifier")
        {

        }

        public override Identifier parseValue(XmlReader xmlReader)
        {
            return new Identifier(xmlReader.ReadElementContentAsString());
        }
    }
}
