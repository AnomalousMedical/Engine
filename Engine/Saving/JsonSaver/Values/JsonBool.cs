using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonBool : JsonValue<bool>
    {
        public JsonBool(JsonSaver xmlWriter)
            : base(xmlWriter, "Bool")
        {

        }

        public override string valueToString(bool value)
        {
            return value.ToString();
        }

        public override bool parseValue(JsonReader xmlReader)
        {
            return xmlReader.ReadAsBoolean().Value;
        }
    }
}
