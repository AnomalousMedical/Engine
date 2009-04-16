﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    interface XmlValueReader
    {
        void readValue(LoadControl loadControl, XmlReader xmlReader);

        String ElementName { get; }
    }
}
