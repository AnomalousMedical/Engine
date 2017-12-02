using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonColor : JsonValue<Color>
    {
        public JsonColor(JsonSaver xmlWriter)
            : base(xmlWriter, "Color")
        {

        }

        public override string valueToString(Color value)
        {
            return value.ToString();
        }

        public override Color parseValue(JsonReader xmlReader)
        {
            float[] rgba = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
            xmlReader.ReadArray(rgba, r => (float)r.ReadAsDouble().Value);
            return new Color(rgba[0], rgba[1], rgba[2], rgba[3]);
        }
    }
}
