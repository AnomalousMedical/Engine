using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    abstract class XmlValue<T> : ValueWriter, XmlValueReader
    {
        protected const String NAME_ENTRY = "name";
        protected String elementName;

        protected XmlSaver xmlSaver;

        public XmlValue(XmlSaver xmlSaver, String elementName)
        {
            this.xmlSaver = xmlSaver;
            this.elementName = elementName;
        }

        public virtual void writeValue(SaveEntry entry)
        {
            XmlWriter xmlWriter = xmlSaver.XmlWriter;
            xmlWriter.WriteStartElement(elementName);
            xmlWriter.WriteAttributeString(NAME_ENTRY, entry.Name);
            if (entry.Value != null)
            {
                xmlWriter.WriteString(entry.Value.ToString());
            }
            xmlWriter.WriteEndElement();
        }

        public virtual void readValue(LoadControl loadControl, XmlReader xmlReader)
        {
            loadControl.addValue(xmlReader.GetAttribute(NAME_ENTRY), parseValue(xmlReader), typeof(T));
        }

        public abstract T parseValue(XmlReader xmlReader);

        public String ElementName
        {
            get
            {
                return elementName;
            }
        }

        public Type getWriteType()
        {
            return typeof(T);
        }
    }
}
