using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.Json
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
        private JsonWriter jsonWriter;
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
            addParser<bool>(new JsonBool(this));
            addParser<byte>(new JsonByte(this));
            addParser<char>(new JsonChar(this));
            addParser<decimal>(new JsonDecimal(this));
            addParser<double>(new JsonDouble(this));
            addParser<float>(new JsonFloat(this));
            addParser<int>(new JsonInt(this));
            addParser<long>(new JsonLong(this));
            addParser<Quaternion>(new JsonQuaternion(this));
            addParser<Ray3>(new JsonRay3(this));
            addParser<sbyte>(new JsonSByte(this));
            addParser<short>(new JsonShort(this));
            addParser<String>(new JsonString(this));
            addParser<uint>(new JsonUInt(this));
            addParser<ulong>(new JsonULong(this));
            addParser<ushort>(new JsonUShort(this));
            addParser<Vector3>(new JsonVector3(this));
            addParser<Color>(new JsonColor(this));
            addParser<byte[]>(new JsonBlob(this));
            addParser<Guid>(new JsonGuid(this));
            saveControl = new SaveControl(this, valueWriters, this);
        }

        private void addParser<T>(JsonValue<T> xmlValue)
        {
            valueWriters.addValueWriter(xmlValue);
            valueReaders.Add(xmlValue.ElementName, xmlValue);
        }

        public void saveObject(Saveable save, JsonWriter jsonWriter)
        {
            this.jsonWriter = jsonWriter;
            try
            {
                jsonWriter.WriteStartArray();
                saveControl.saveObject(save);
                jsonWriter.WriteEndArray();
                jsonWriter.Flush();
            }
            finally
            {
                saveControl.reset();
            }
        }

        public void writeHeader(ObjectIdentifier objectId, int version)
        {
            jsonWriter.WriteStartObject();

            //Write saveable header
            jsonWriter.WritePropertyName(DefaultTypeFinder.CreateShortTypeString(objectId.ObjectType));
            jsonWriter.WriteStartObject();

            jsonWriter.WritePropertyName("id");
            jsonWriter.WriteValue(NumberParser.ToString(objectId.ObjectID));

            if (version != 0)
            {
                jsonWriter.WritePropertyName("version");
                jsonWriter.WriteValue(version);
            }

            jsonWriter.WriteEndObject();
        }

        public void writeFooter(ObjectIdentifier objectId)
        {
            jsonWriter.WriteEndObject();
        }

        /// <summary>
        /// Read the json header object, it must come first in any saveable object. The reader should
        /// be on the _saveable PropertyName. It will advance the reader to the node for the end of the
        /// header object.
        /// </summary>
        /// <param name="reader">This must currently be on the _saveable PropertyName</param>
        /// <param name="version">The version number of the object.</param>
        /// <returns></returns>
        private ObjectIdentifier ParseHeaderObject(JsonReader reader, ref int version)
        {
            if(reader.TokenType != JsonToken.PropertyName)
            {
                throw new InvalidOperationException($"Saveable Json Objects must start with a header object named with the type to load.");
            }

            version = 0;
            long id = 0;
            String type = reader.Value.ToString();
            bool inHeaderObject = true;

            while (inHeaderObject && reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        var propName = reader.Value.ToString();
                        switch (propName)
                        {
                            case "version":
                                version = reader.ReadAsInt32().Value;
                                break;
                            case "id":
                                id = NumberParser.ParseLong(reader.ReadAsString());
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

        private void readProp(JsonReader reader)
        {
            var name = reader.Value.ToString();
            //Load value object
            reader.Read();
            if(reader.TokenType != JsonToken.StartObject)
            {
                readPropError(name);
            }
            reader.Read();
            if(reader.TokenType != JsonToken.PropertyName)
            {
                readPropError(name);
            }
            String type = reader.Value.ToString();
            valueReaders[type].readValue(loadControl, name, reader);
            reader.Read();
            if(reader.TokenType != JsonToken.EndObject)
            {
                readPropError(name);
            }
        }

        private static void readPropError(string name)
        {
            throw new InvalidOperationException($@"Property {name} must have a value in the format {{ ""type"" : ""value"" }}");
        }

        private Object readSaveable(JsonReader reader)
        {
            reader.Read();
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

        public T restoreObject<T>(JsonReader reader)
        {
            return (T)restoreObject(reader);
        }

        public object restoreObject(JsonReader reader)
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

        internal JsonWriter Writer
        {
            get
            {
                return jsonWriter;
            }
        }
    }
}
