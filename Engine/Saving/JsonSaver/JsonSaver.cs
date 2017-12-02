using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    public class JsonSaver : HeaderWriter, FooterWriter
    {
        private const string SAVEABLE_ELEMENT = "Saveable";
        private const string TYPE_ATTRIBUTE = "type";
        private const string ID_ATTIBUTE = "id";
        private const string VERSION_ATTIBUTE = "version";
        private const string DOCUMENT = "Save";

        private ValueWriterCollection valueWriters;
        private SaveControl saveControl;
        private JsonSaveable saveableValue;
        private JsonEnum enumValue;
        private XmlWriter xmlWriter;
        private LoadControl loadControl;
        private Dictionary<String, JsonValueReader> valueReaders = new Dictionary<string,JsonValueReader>();
        private TypeFinder typeFinder;

        public JsonSaver()
            :this(new DefaultTypeFinder())
        {
            
        }

        public JsonSaver(TypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
            loadControl = new LoadControl(typeFinder);
            saveableValue = new JsonSaveable(this);
            valueReaders.Add(saveableValue.ElementName, saveableValue);
            enumValue = new JsonEnum(this);
            valueReaders.Add(enumValue.ElementName, enumValue);
            valueWriters = new ValueWriterCollection(saveableValue, enumValue);
            addXmlValue<bool>(new JsonBool(this));
            addXmlValue<byte>(new JsonByte(this));
            addXmlValue<char>(new JsonChar(this));
            addXmlValue<decimal>(new JsonDecimal(this));
            addXmlValue<double>(new JsonDouble(this));
            addXmlValue<float>(new JsonFloat(this));
            addXmlValue<int>(new JsonInt(this));
            addXmlValue<long>(new JsonLong(this));
            addXmlValue<Quaternion>(new JsonQuaternion(this));
            addXmlValue<Ray3>(new JsonRay3(this));
            addXmlValue<sbyte>(new JsonSByte(this));
            addXmlValue<short>(new JsonShort(this));
            addXmlValue<String>(new JsonString(this));
            addXmlValue<uint>(new JsonUInt(this));
            addXmlValue<ulong>(new JsonULong(this));
            addXmlValue<ushort>(new JsonUShort(this));
            addXmlValue<Vector3>(new JsonVector3(this));
            addXmlValue<Color>(new JsonColor(this));
            addXmlValue<byte[]>(new JsonBlob(this));
            addXmlValue<Guid>(new JsonGuid(this));
            saveControl = new SaveControl(this, valueWriters, this);
        }

        private void addXmlValue<T>(JsonValue<T> xmlValue)
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
            xmlWriter.WriteAttributeString(TYPE_ATTRIBUTE, DefaultTypeFinder.CreateShortTypeString(objectId.ObjectType));
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
    }
}
