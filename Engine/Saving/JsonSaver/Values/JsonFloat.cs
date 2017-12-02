using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonFloat : JsonValue<float>
    {
        public JsonFloat(JsonSaver xmlWriter)
            : base(xmlWriter, "Float")
        {

        }

        public override string valueToString(float value)
        {
            return NumberParser.ToString(value);
        }

        public override float parseValue(JsonReader xmlReader)
        {
            return (float)xmlReader.ReadAsDouble().Value;
        }
    }
}
