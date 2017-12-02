using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonGuid : JsonValue<Guid>
    {
        public JsonGuid(JsonSaver xmlWriter)
            : base(xmlWriter, "Guid")
        {

        }

        public override void writeValue(Guid value, JsonWriter writer)
        {
            writer.WriteValue(value.ToString());
        }

        public override Guid parseValue(JsonReader xmlReader)
        {
            return new Guid(xmlReader.ReadAsString());
        }
    }
}
