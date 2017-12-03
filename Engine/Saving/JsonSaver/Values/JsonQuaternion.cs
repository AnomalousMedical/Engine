using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonQuaternion : JsonValue<Quaternion>
    {
        public JsonQuaternion(JsonSaver xmlWriter)
            : base(xmlWriter, "Quaternion")
        {

        }

        public override void writeValue(Quaternion value, JsonWriter writer)
        {
            writer.WriteStartArray();

            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteValue(value.z);
            writer.WriteValue(value.w);

            writer.WriteEndArray();
        }

        public override Quaternion parseValue(JsonReader xmlReader)
        {
            var quat = xmlReader.ReadArray<float>(4, Convert.ToSingle);
            return new Quaternion(quat[0], quat[1], quat[2], quat[3]);
        }
    }
}
