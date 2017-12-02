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

        public override string valueToString(Vector3 value)
        {
            return value.ToString();
        }

        public override Vector3 parseValue(JsonReader xmlReader)
        {
            float[] vector3 = xmlReader.ReadArray<float>(3, r => (float)xmlReader.ReadAsDouble());
            return new Vector3(vector3[0], vector3[1], vector3[2]);
        }
    }
}
