using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.Json
{
    class JsonRay3 : JsonValue<Ray3>
    {
        public JsonRay3(JsonSaver xmlWriter)
            : base(xmlWriter, "Ray3")
        {

        }

        public override void writeValue(Ray3 value, JsonWriter writer)
        {
            writer.WriteStartArray();

            writer.WriteStartArray();
            writer.WriteValue(value.Origin.x);
            writer.WriteValue(value.Origin.y);
            writer.WriteValue(value.Origin.z);
            writer.WriteEndArray();

            writer.WriteStartArray();
            writer.WriteValue(value.Direction.x);
            writer.WriteValue(value.Direction.y);
            writer.WriteValue(value.Direction.z);
            writer.WriteEndArray();

            writer.WriteEndArray();
        }

        // { "Ray3" : [ [x, y, z], [x, y, z] ] }

        public override Ray3 parseValue(JsonReader reader)
        {
            reader.Read();
            if(reader.TokenType != JsonToken.StartArray)
            {
                throwReadError();
            }

            var o = reader.ReadArray<float>(3, Convert.ToSingle);
            var d = reader.ReadArray<float>(3, Convert.ToSingle);

            reader.Read();
            if(reader.TokenType != JsonToken.EndArray)
            {
                throwReadError();
            }

            return new Ray3(new Vector3(o[0], o[1], o[2]), new Vector3(d[0], d[1], d[2]));
        }

        private static void throwReadError()
        {
            throw new InvalidOperationException(@"A Ray3 must be in the format { ""Ray3"" : [ [x, y, z], [x, y, z] ] where the first array is the origin vector and the 2nd is the direction }.");
        }
    }
}
