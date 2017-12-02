using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using Logging;
using Newtonsoft.Json;

namespace Engine.Saving.JsonSaver
{
    class JsonEnum : JsonValue<Enum>
    {
        private const String TYPE = "type";

        public JsonEnum(JsonSaver xmlWriter)
            : base(xmlWriter, "Enum")
        {

        }

        public override void writeValue(SaveEntry entry)
        {
            XmlWriter xmlWriter = xmlSaver.XmlWriter;
            xmlWriter.WriteStartElement(elementName);
            xmlWriter.WriteAttributeString(NAME_ENTRY, entry.Name);
            xmlWriter.WriteAttributeString(TYPE, DefaultTypeFinder.CreateShortTypeString(entry.ObjectType));
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

        /**
         * Format
         * { "Enum": { "ClrEnumType" : "Value" } }
         */

        private void throwReadFormatError()
        {
            throw new InvalidOperationException(@"The format of an Enum must be { ""Enum:"": { ""ClrEnumType"" : ""Value"" } }");
        }

        public override void readValue(LoadControl loadControl, String name, JsonReader xmlReader)
        {
            if(xmlReader.TokenType != JsonToken.StartObject)
            {
                throwReadFormatError();
            }

            xmlReader.Read();
            var clrType = xmlReader.ReadAsString();
            var enumType = loadControl.TypeFinder.findType(clrType);

            xmlReader.Read();
            var valueStr = xmlReader.ReadAsString();

            //Read to end of object
            while(xmlReader.TokenType != JsonToken.EndObject && xmlReader.Read()) { }

            if (enumType != null)
            {
                Object value = Enum.Parse(enumType, valueStr);
                loadControl.addValue(name, value, enumType);
            }
            else
            {
                Log.Default.sendMessage("Could not find enum type {0}. Value not loaded.", LogLevel.Warning, "Saving", clrType);
            }
        }

        public override Enum parseValue(JsonReader xmlReader)
        {
            throw new SaveException("This ParseValue function should never be called");
        }
    }
}
