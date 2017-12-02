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

        public override string valueToString(Ray3 value)
        {
            return value.ToString();
        }

        // { "Ray3" : { [ox, oy, oz], [dx, dy, dz] } }

        public override Ray3 parseValue(JsonReader xmlReader)
        {
            if(xmlReader.TokenType != JsonToken.StartObject)
            {
                throwReadError();
            }
            var o = xmlReader.ReadArray<float>(3, r => (float)r.ReadAsDouble());
            var d = xmlReader.ReadArray<float>(3, r => (float)r.ReadAsDouble());

            return new Ray3(new Vector3(o[0], o[1], o[2]), new Vector3(d[0], d[1], d[2]));
        }

        private static void throwReadError()
        {
            throw new InvalidOperationException(@"A Ray3 must be in the format { ""Ray3"" : { [ox, oy, oz], [dx, dy, dz] } } where x is the origin and d is the direction.");
        }
    }
}
