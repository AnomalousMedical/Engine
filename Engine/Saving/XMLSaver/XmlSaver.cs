using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    public class XmlSaver : HeaderWriter, FooterWriter
    {
        private const string SAVEABLE_ELEMENT = "Saveable";
        private const string TYPE_ATTRIBUTE = "type";
        private const string ID_ATTIBUTE = "id";

        private ValueWriterCollection valueWriters;
        private SaveControl saveWriter;
        private XmlSaveable saveableValue;
        private XmlEnum enumValue;
        private XmlWriter xmlWriter;


        public XmlSaver()
        {
            saveableValue = new XmlSaveable(this);
            enumValue = new XmlEnum(this);
            valueWriters = new ValueWriterCollection(saveableValue, enumValue);
            valueWriters.addValueWriter(new XmlBool(this));
            valueWriters.addValueWriter(new XmlByte(this));
            valueWriters.addValueWriter(new XmlChar(this));
            valueWriters.addValueWriter(new XmlDecimal(this));
            valueWriters.addValueWriter(new XmlDouble(this));
            valueWriters.addValueWriter(new XmlFloat(this));
            valueWriters.addValueWriter(new XmlIdentifier(this));
            valueWriters.addValueWriter(new XmlInt(this));
            valueWriters.addValueWriter(new XmlLong(this));
            valueWriters.addValueWriter(new XmlQuaternion(this));
            valueWriters.addValueWriter(new XmlRay3(this));
            valueWriters.addValueWriter(new XmlSByte(this));
            valueWriters.addValueWriter(new XmlShort(this));
            valueWriters.addValueWriter(new XmlString(this));
            valueWriters.addValueWriter(new XmlUInt(this));
            valueWriters.addValueWriter(new XmlULong(this));
            valueWriters.addValueWriter(new XmlUShort(this));
            valueWriters.addValueWriter(new XmlVector3(this));
            saveWriter = new SaveControl(this, valueWriters, this);
        }

        public void saveObject(Saveable save, XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;
            saveWriter.saveObject(save);
        }

        public void writeHeader(ObjectIdentifier objectId)
        {
            xmlWriter.WriteStartElement(SAVEABLE_ELEMENT);
            xmlWriter.WriteAttributeString(TYPE_ATTRIBUTE, String.Format("{0}, {1}", objectId.ObjectType.FullName, objectId.ObjectType.Assembly.FullName));
            xmlWriter.WriteAttributeString(ID_ATTIBUTE, objectId.ObjectID.ToString());
        }

        public void writeFooter(ObjectIdentifier objectId)
        {
            xmlWriter.WriteEndElement();
        }

        internal XmlWriter XmlWriter
        {
            get
            {
                return xmlWriter;
            }
        }
    }
}
