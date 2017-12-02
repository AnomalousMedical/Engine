﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonChar : JsonValue<char>
    {
        public JsonChar(JsonSaver xmlWriter)
            : base(xmlWriter, "Char")
        {

        }

        public override string valueToString(char value)
        {
            return NumberParser.ToString(value);
        }

        public override char parseValue(JsonReader xmlReader)
        {
            return (char)xmlReader.ReadAsInt32();
        }
    }
}
