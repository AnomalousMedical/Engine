using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    abstract class XmlValue<T> : ValueWriterEntry, XmlValueReader
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
            //JSON_XML_ISSUE
            //There is a discrepency here between xml and json, this always writes the fact that the value existed.
            //This could change how hasValue works since it won't always be written the same way, this might not matter, but need to investigate
            //Stuff that is using hasValue, likely it can be updated as well
            XmlWriter xmlWriter = xmlSaver.XmlWriter;
            xmlWriter.WriteStartElement(elementName);
            xmlWriter.WriteAttributeString(NAME_ENTRY, entry.Name);
            if (entry.Value != null)
            {
                xmlWriter.WriteString(valueToString((T)entry.Value));
            }
            xmlWriter.WriteEndElement();
        }

        public abstract String valueToString(T value);

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
