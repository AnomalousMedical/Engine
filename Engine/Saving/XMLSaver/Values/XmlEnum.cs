using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;

namespace Engine.Saving.XMLSaver
{
    class XmlEnum : XmlValue<Enum>
    {
        private const String TYPE = "type";

        public XmlEnum(XmlSaver xmlWriter)
            : base(xmlWriter, "Enum")
        {

        }

        public override void writeValue(SaveEntry entry)
        {
            XmlWriter xmlWriter = xmlSaver.XmlWriter;
            xmlWriter.WriteStartElement(elementName);
            xmlWriter.WriteAttributeString(NAME_ENTRY, entry.Name);
            xmlWriter.WriteAttributeString(TYPE, String.Format("{0}, {1}", entry.ObjectType.FullName, XmlSaver.createShortTypeString(entry.ObjectType)));
            if (entry.Value != null)
            {
                xmlWriter.WriteString(entry.Value.ToString());
            }
            xmlWriter.WriteEndElement();
        }

        public override void readValue(LoadControl loadControl, XmlReader xmlReader)
        {
            Type enumType = Type.GetType(xmlReader.GetAttribute(TYPE), true);
            String name = xmlReader.GetAttribute(NAME_ENTRY);
            Object value = Enum.Parse(enumType, xmlReader.ReadElementContentAsString());
            loadControl.addValue(name, value, enumType);
        }

        public override Enum parseValue(XmlReader xmlReader)
        {
            throw new SaveException("This ParseValue function should never be called");
        }
    }
}
