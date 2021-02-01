using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using Newtonsoft.Json;

namespace Engine.Saving.Json
{
    class JsonEnum : JsonValue<Enum>
    {
        private const String TYPE = "type";

        public JsonEnum(JsonSaver xmlWriter)
            : base(xmlWriter, "Enum")
        {

        }

        public override void writeValue(Enum value, JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(value.GetType().FullName);
            writer.WriteValue(value.ToString());

            writer.WriteEndObject();
        }

        /**
         * Format
         * { "Enum": { "ClrEnumType" : "Value" } }
         */

        private void throwReadFormatError()
        {
            throw new InvalidOperationException(@"The format of an Enum must be { ""Enum:"": { ""ClrEnumType"" : ""Value"" } }");
        }

        public override void readValue(LoadControl loadControl, String name, JsonReader reader)
        {
            reader.Read();
            if(reader.TokenType != JsonToken.StartObject)
            {
                throwReadFormatError();
            }

            reader.Read();
            if(reader.TokenType != JsonToken.PropertyName)
            {
                throwReadFormatError();
            }
            var clrType = reader.Value.ToString();
            var enumType = loadControl.TypeFinder.findType(clrType);
            var valueStr = reader.ReadAsString();

            reader.Read();
            if(reader.TokenType != JsonToken.EndObject)
            {
                throwReadFormatError();
            }

            if (enumType != null)
            {
                Object value = Enum.Parse(enumType, valueStr);
                loadControl.addValue(name, value, enumType);
            }
            else
            {
                //Log.Default.sendMessage("Could not find enum type {0}. Value not loaded.", LogLevel.Warning, "Saving", clrType);
            }
        }

        public override Enum parseValue(JsonReader xmlReader)
        {
            throw new SaveException("This ParseValue function should never be called");
        }
    }
}
