using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Engine.ObjectManagement;

namespace Engine.Saving.XMLSaver
{
    public class XmlSaver : HeaderWriter, FooterWriter
    {
        private const string SAVEABLE_ELEMENT = "Saveable";
        private const string TYPE_ATTRIBUTE = "type";
        private const string ID_ATTIBUTE = "id";
        private const string VERSION_ATTIBUTE = "version";
        private const string DOCUMENT = "Save";

        private ValueWriterCollection valueWriters;
        private SaveControl saveControl;
        private XmlSaveable saveableValue;
        private XmlEnum enumValue;
        private XmlWriter xmlWriter;
        private LoadControl loadControl;
        private Dictionary<String, XmlValueReader> valueReaders = new Dictionary<string,XmlValueReader>();
        private TypeFinder typeFinder;

        public XmlSaver()
            :this(new DefaultTypeFinder())
        {
            
        }

        public XmlSaver(TypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
            loadControl = new LoadControl(typeFinder);
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
            addXmlValue<Color>(new XmlColor(this));
            addXmlValue<byte[]>(new XmlBlob(this));
            saveControl = new SaveControl(this, valueWriters, this);
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
            saveControl.saveObject(save);
            xmlWriter.WriteEndElement();
            saveControl.reset();
        }

        public void writeHeader(ObjectIdentifier objectId, int version)
        {
            xmlWriter.WriteStartElement(SAVEABLE_ELEMENT);
            xmlWriter.WriteAttributeString(TYPE_ATTRIBUTE, String.Format("{0}, {1}", objectId.ObjectType.FullName, createShortTypeString(objectId.ObjectType)));
            xmlWriter.WriteAttributeString(ID_ATTIBUTE, objectId.ObjectID.ToString());
            if (version > 0)
            {
                xmlWriter.WriteAttributeString(VERSION_ATTIBUTE, version.ToString());
            }
        }

        public object restoreObject(XmlReader xmlReader)
        {
            Object lastReadObject = null;
            try
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name.Equals(SAVEABLE_ELEMENT))
                        {
                            int version = 0;
                            String versionStr = xmlReader.GetAttribute(VERSION_ATTIBUTE);
                            if (versionStr != null)
                            {
                                version = NumberParser.ParseInt(versionStr);
                            }
                            ObjectIdentifier objectId = ObjectIdentifierFactory.CreateObjectIdentifier(NumberParser.ParseLong(xmlReader.GetAttribute(ID_ATTIBUTE)), xmlReader.GetAttribute(TYPE_ATTRIBUTE), typeFinder);
                            loadControl.startDefiningObject(objectId, version);
                            //If the element is empty do not bother to loop looking for elements.
                            if (!xmlReader.IsEmptyElement)
                            {
                                while (!(xmlReader.Name == SAVEABLE_ELEMENT && xmlReader.NodeType == XmlNodeType.EndElement) && xmlReader.Read())
                                {
                                    if (xmlReader.NodeType == XmlNodeType.Element)
                                    {
                                        valueReaders[xmlReader.Name].readValue(loadControl, xmlReader);
                                    }
                                }
                            }
                            lastReadObject = loadControl.createCurrentObject();
                        }
                    }
                }
            }
            finally
            {
                loadControl.reset();
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

        internal static String createShortTypeString(Type type)
        {
            String shortAssemblyName = type.Assembly.FullName;
            return shortAssemblyName.Remove(shortAssemblyName.IndexOf(','));
        }
    }
}
