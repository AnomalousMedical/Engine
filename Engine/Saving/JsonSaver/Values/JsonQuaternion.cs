﻿using Newtonsoft.Json;
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

        public override string valueToString(Quaternion value)
        {
            return value.ToString();
        }

        public override Quaternion parseValue(JsonReader xmlReader)
        {
            var quat = xmlReader.ReadArray<float>(4, r => (float)r.ReadAsDouble());
            return new Quaternion(quat[0], quat[1], quat[2], quat[3]);
        }
    }
}
