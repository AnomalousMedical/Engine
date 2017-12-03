using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonRay3 : JsonValue<Ray3>
    {
        public JsonRay3(JsonSaver xmlWriter)
            : base(xmlWriter, "Ray3")
        {

        }

        public override void writeValue(Ray3 value, JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WriteEndObject();
        }

        // { "Ray3" : { "origin" : [x, y, z], "dir": [x, y, z] } }

        public override Ray3 parseValue(JsonReader xmlReader)
        {
            if(xmlReader.TokenType != JsonToken.StartObject)
            {
                throwReadError();
            }

            xmlReader.Read();
            if(xmlReader.TokenType != JsonToken.PropertyName && xmlReader.ReadAsString() != "origin")
            {
                throwReadError();
            }
            xmlReader.Read();
            var o = xmlReader.ReadArray<float>(3, Convert.ToSingle);

            xmlReader.Read();
            if (xmlReader.TokenType != JsonToken.PropertyName && xmlReader.ReadAsString() != "dir")
            {
                throwReadError();
            }
            xmlReader.Read();
            var d = xmlReader.ReadArray<float>(3, Convert.ToSingle);

            return new Ray3(new Vector3(o[0], o[1], o[2]), new Vector3(d[0], d[1], d[2]));
        }

        private static void throwReadError()
        {
            throw new InvalidOperationException(@"A Ray3 must be in the format { ""Ray3"" : { ""origin"" : [x, y, z], ""dir"": [x, y, z] } }.");
        }
    }
}
