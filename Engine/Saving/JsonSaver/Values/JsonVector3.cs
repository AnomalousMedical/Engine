using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonVector3 : JsonValue<Vector3>
    {
        public JsonVector3(JsonSaver xmlWriter)
            : base(xmlWriter, "Vector3")
        {

        }

        public override void writeValue(Vector3 value, JsonWriter writer)
        {
            writer.WriteStartArray();

            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteValue(value.z);

            writer.WriteEndArray();
        }

        public override Vector3 parseValue(JsonReader xmlReader)
        {
            float[] vector3 = xmlReader.ReadArray<float>(3, Convert.ToSingle);
            return new Vector3(vector3[0], vector3[1], vector3[2]);
        }
    }
}
