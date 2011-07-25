using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using Logging;

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

        public override string valueToString(Enum value)
        {
            return value.ToString();
        }

        public override void readValue(LoadControl loadControl, XmlReader xmlReader)
        {
            Type enumType = PluginManager.Instance.getType(xmlReader.GetAttribute(TYPE));
            if (enumType != null)
            {
                String name = xmlReader.GetAttribute(NAME_ENTRY);
                Object value = Enum.Parse(enumType, xmlReader.ReadElementContentAsString());
                loadControl.addValue(name, value, enumType);
            }
            else
            {
                Log.Default.sendMessage("Could not find enum type {0}. Value not loaded.", LogLevel.Warning, "Saving", xmlReader.GetAttribute(TYPE));
            }
        }

        public override Enum parseValue(XmlReader xmlReader)
        {
            throw new SaveException("This ParseValue function should never be called");
        }
    }
}
