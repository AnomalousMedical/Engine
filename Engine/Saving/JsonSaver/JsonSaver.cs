using Newtonsoft.Json;
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
        private Dictionary<String, JsonValueReader> valueReaders = new Dictionary<string, JsonValueReader>();
        private TypeFinder typeFinder;

        public JsonSaver()
            : this(new DefaultTypeFinder())
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

        /// <summary>
        /// Read the json header object, it must come first in any saveable object. The reader should
        /// be on the _saveable PropertyName. It will advance the reader to the node for the end of the
        /// header object.
        /// </summary>
        /// <param name="reader">This must currently be on the _saveable PropertyName</param>
        /// <param name="version">The version number of the object.</param>
        /// <returns></returns>
        private ObjectIdentifier ParseHeaderObject(JsonTextReader reader, ref int version)
        {
            if(reader.TokenType != JsonToken.PropertyName && reader.ReadAsString() != "_saveable")
            {
                throw new InvalidOperationException("Saveable Json Objects must start with a header object named '_saveable'.");
            }

            version = 0;
            long id = 0;
            String type = null;
            bool inHeaderObject = true;

            while (inHeaderObject && reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        var propName = reader.ReadAsString();
                        reader.Read(); //Advance to value
                        switch (propName)
                        {
                            case "version":
                                version = reader.ReadAsInt32().Value;
                                break;
                            case "id":
                                id = NumberParser.ParseLong(reader.ReadAsString());
                                break;
                            case "type":
                                type = reader.ReadAsString();
                                break;
                        }
                        break;
                    case JsonToken.EndObject:
                        inHeaderObject = false;
                        break;
                }
            }

            return ObjectIdentifierFactory.CreateObjectIdentifier(id, type, typeFinder);
        }

        private void readProp(JsonTextReader reader)
        {
            var name = reader.ReadAsString();
            //Load value object
            reader.Read();
            if(reader.TokenType != JsonToken.StartObject)
            {
                throw new InvalidOperationException($@"Property {name} must have a value in the format {{ ""type"" : ""value"" }}");
            }
            reader.Read();
            if(reader.TokenType != JsonToken.PropertyName)
            {
                throw new InvalidOperationException($@"Property {name} must have a value in the format {{ ""type"" : ""value"" }}");
            }
            String type = reader.ReadAsString();
            reader.Read(); //Advance to value
            valueReaders[type].readValue(loadControl, name, reader);
        }

        private Object readSaveable(JsonTextReader reader)
        {
            int version = 0;
            ObjectIdentifier objectId = ParseHeaderObject(reader, ref version);
            loadControl.startDefiningObject(objectId, version);
            bool keepReading = true;
            while(keepReading && reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        readProp(reader);
                        break;
                    case JsonToken.EndObject:
                        keepReading = false;
                        break;
                }
            }
            return loadControl.createCurrentObject();
        }

        public object restoreObject(JsonTextReader reader)
        {
            Object lastReadObject = null;
            try
            {
                //Read array
                reader.Read();
                if(reader.TokenType != JsonToken.StartArray)
                {
                    throw new InvalidOperationException("Saveable Json Files must be an array of objects.");
                }

                bool keepReading = true;
                while (keepReading && reader.Read())
                {
                    Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);

                    switch (reader.TokenType)
                    {
                        case JsonToken.StartObject:
                            lastReadObject = readSaveable(reader);
                            break;
                        case JsonToken.EndArray:
                            keepReading = false;
                            break;
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
