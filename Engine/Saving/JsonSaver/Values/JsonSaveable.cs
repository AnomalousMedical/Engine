using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonSaveable : JsonValue<Saveable>
    {
        private const String OBJECT_ID = "id";

        public JsonSaveable(JsonSaver xmlWriter)
            :base(xmlWriter, "Saveable")
        {

        }

        public override void writeValue(SaveEntry entry)
        {
            XmlWriter xmlWriter = xmlSaver.XmlWriter;
            xmlWriter.WriteStartElement(elementName);
            xmlWriter.WriteAttributeString(NAME_ENTRY, entry.Name);
            xmlWriter.WriteAttributeString(OBJECT_ID, entry.ObjectID.ToString());
            xmlWriter.WriteEndElement();
        }

        public override string valueToString(Saveable value)
        {
            return value.ToString();
        }

        //Format { "Saveable": "Id" }

        public override void readValue(LoadControl loadControl, String name, JsonReader xmlReader)
        {
            loadControl.addObjectValue(name, NumberParser.ParseLong(xmlReader.ReadAsString()));
        }

        public override Saveable parseValue(JsonReader xmlReader)
        {
            throw new SaveException("This parseValue function should never be called");
        }
    }
}
