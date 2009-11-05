using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Engine.Saving.XMLSaver
{
    class XmlBlob : XmlValue<byte[]>
    {
        private const string BYTE_SIZE_ENTRY = "NumBytes";

        public XmlBlob(XmlSaver xmlWriter)
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

        public override byte[] parseValue(XmlReader xmlReader)
        {
            int length = int.Parse(xmlReader.GetAttribute(BYTE_SIZE_ENTRY));
            byte[] blobArray = new byte[length];
            xmlReader.ReadElementContentAsBinHex(blobArray, 0, blobArray.Length);
            return blobArray;
        }
    }
}
