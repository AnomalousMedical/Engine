using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlSaveable : XmlValue<Saveable>
    {
        private const String OBJECT_ID = "id";

        public XmlSaveable(XmlSaver xmlWriter)
            :base(xmlWriter, "Saveable")
        {

        }

        public override void writeValue(SaveEntry entry)
        {
            XmlWriter xmlWriter = xmlSaver.XmlWriter;
            xmlWriter.WriteStartElement(elementName);
            xmlWriter.WriteAttributeString(NAME_ENTRY, entry.Name);
            xmlWriter.WriteAttributeString(OBJECT_ID, entry.ObjectID.ToString());
            xmlWriter.WriteEndElement();
        }

        public override void readValue(LoadControl loadControl, XmlReader xmlReader)
        {
            loadControl.addObjectValue(xmlReader.GetAttribute(NAME_ENTRY), NumberParser.ParseLong(xmlReader.GetAttribute(OBJECT_ID)));
        }

        public override Saveable parseValue(XmlReader xmlReader)
        {
            throw new SaveException("This parseValue function should never be called");
        }
    }
}
