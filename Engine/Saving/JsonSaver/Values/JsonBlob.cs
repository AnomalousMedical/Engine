﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.JsonSaver
{
    class JsonBlob : JsonValue<byte[]>
    {
        private const string BYTE_SIZE_ENTRY = "NumBytes";

        public JsonBlob(JsonSaver xmlWriter)
            : base(xmlWriter, "Blob")
        {

        }

        public override void writeValue(SaveEntry entry)
        {
            XmlWriter xmlWriter = xmlSaver.XmlWriter;
            xmlWriter.WriteStartElement(elementName);
            xmlWriter.WriteAttributeString(NAME_ENTRY, entry.Name);
            if (entry.Value != null)
            {
                byte[] blobArray = (byte[])entry.Value;
                xmlWriter.WriteAttributeString(BYTE_SIZE_ENTRY, blobArray.Length.ToString());
                xmlWriter.WriteBinHex(blobArray, 0, blobArray.Length);
            }
            xmlWriter.WriteEndElement();
        }

        public override string valueToString(byte[] value)
        {
            return null;
        }

        public override byte[] parseValue(JsonReader xmlReader)
        {
            return xmlReader.ReadAsBytes();
        }
    }
}
