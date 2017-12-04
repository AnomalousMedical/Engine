using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.Json
{
    interface JsonValueReader
    {
        void readValue(LoadControl loadControl, String name, JsonReader xmlReader);

        String ElementName { get; }
    }
}
