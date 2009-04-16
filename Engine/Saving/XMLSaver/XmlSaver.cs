using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Engine.ObjectManagement;
using EngineMath;

namespace Engine.Saving.XMLSaver
{
    public class XmlSaver : HeaderWriter, FooterWriter
    {
        private const string SAVEABLE_ELEMENT = "Saveable";
        private const string TYPE_ATTRIBUTE = "type";
        private const string ID_ATTIBUTE = "id";
        private const string DOCUMENT = "Save";

        private ValueWriterCollection valueWriters;
        private SaveControl saveWriter;
        private XmlSaveable saveableValue;
        private XmlEnum enumValue;
        private XmlWriter xmlWriter;
        private LoadControl loadControl = new LoadControl();
        private Dictionary<String, XmlValueReader> valueReaders = new Dictionary<string,XmlValueReader>();

        public XmlSaver()
        {
            saveableValue = new XmlSaveable(this);
            valueReaders.Add(saveableValue.ElementName, saveableValue);
            enumValue = new XmlEnum(this);
            valueReaders.Add(enumValue.ElementName, enumValue);
            valueWriters = new ValueWriterCollection(saveableValue, enumValue);
            addXmlValue<bool>(new XmlBool(this));
            addXmlValue<byte>(new XmlByte(this));
            addXmlValue<char>(new XmlChar(this));
            addXmlValue<decimal>(new XmlDecimal(this));
            addXmlValue<double>(new XmlDouble(this));
            addXmlValue<float>(new XmlFloat(this));
            addXmlValue<Identifier>(new XmlIdentifier(this));
            addXmlValue<int>(new XmlInt(this));
            addXmlValue<long>(new XmlLong(this));
            addXmlValue<Quaternion>(new XmlQuaternion(this));
            addXmlValue<Ray3>(new XmlRay3(this));
            addXmlValue<sbyte>(new XmlSByte(this));
            addXmlValue<short>(new XmlShort(this));
            addXmlValue<String>(new XmlString(this));
            addXmlValue<uint>(new XmlUInt(this));
            addXmlValue<ulong>(new XmlULong(this));
            addXmlValue<ushort>(new XmlUShort(this));
            addXmlValue<Vector3>(new XmlVector3(this));
            saveWriter = new SaveControl(this, valueWriters, this);
        }

        private void addXmlValue<T>(XmlValue<T> xmlValue)
        {
            valueWriters.addValueWriter(xmlValue);
            valueReaders.Add(xmlValue.ElementName, xmlValue);
        }

        public void saveObject(Saveable save, XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;
            xmlWriter.WriteStartElement(DOCUMENT);
            saveWriter.saveObject(save);
            xmlWriter.WriteEndElement();
        }

        public void writeHeader(ObjectIdentifier objectId)
        {
            xmlWriter.WriteStartElement(SAVEABLE_ELEMENT);
            xmlWriter.WriteAttributeString(TYPE_ATTRIBUTE, String.Format("{0}, {1}", objectId.ObjectType.FullName, objectId.ObjectType.Assembly.FullName));
            xmlWriter.WriteAttributeString(ID_ATTIBUTE, objectId.ObjectID.ToString());
        }

        public object restoreObject(XmlReader xmlReader)
        {
            Object lastReadObject = null;
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name.Equals(SAVEABLE_ELEMENT))
                    {
                        ObjectIdentifier objectId = new ObjectIdentifier(long.Parse(xmlReader.GetAttribute(ID_ATTIBUTE)), null, Type.GetType(xmlReader.GetAttribute(TYPE_ATTRIBUTE)));
                        loadControl.startDefiningObject(objectId);
                        while (!(xmlReader.Name == SAVEABLE_ELEMENT && xmlReader.NodeType == XmlNodeType.EndElement) && xmlReader.Read())
                        {
                            if (xmlReader.NodeType == XmlNodeType.Element)
                            {
                                valueReaders[xmlReader.Name].readValue(loadControl, xmlReader);
                            }
                        }
                        lastReadObject = loadControl.createCurrentObject();
                    }
                }
            }
            return lastReadObject;
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
