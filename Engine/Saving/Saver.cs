using Engine.Saving.Json;
using Engine.Saving.XMLSaver;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Engine.Saving
{
    public enum SaverOutputType
    {
        Json,
        Bson,
        Xml
    }

    public class Saver
    {
        public static SaverOutputType TypeFromFile(String file, bool throwOnError = false, SaverOutputType fallback = SaverOutputType.Json)
        {
            switch (Path.GetExtension(file).ToLowerInvariant())
            {
                case ".json":
                    return SaverOutputType.Json;
                case ".bson":
                    return SaverOutputType.Bson;
                case ".xml":
                    return SaverOutputType.Xml;
            }
            if (throwOnError)
            {
                throw new InvalidOperationException($"{file} is not a recognized output type for saver");
            }
            return fallback;
        }

        /// <summary>
        /// Try to detect the type of the data in a stream. The stream will be read and left
        /// in position at the end of the header detection unless you pass setToBegin = true (default)
        /// </summary>
        /// <param name="stream">The stream to detect.</param>
        /// <param name="setToBegin">Pass true to reset stream to the beginning after detecting the type.</param>
        /// <returns></returns>
        public static SaverOutputType TypeFromStream(Stream stream, bool setToBegin = true)
        {
            try
            {
                byte[] header = new byte[30];
                int remaining = header.Length < stream.Length ? header.Length : (int)stream.Length;
                while (remaining > 0)
                {
                    remaining -= stream.Read(header, 0, header.Length);
                }

                var headerStr = Encoding.Unicode.GetString(header);

                if (headerStr.StartsWith("<Save>") || headerStr.StartsWith("<?xml"))
                {
                    return SaverOutputType.Xml;
                }

                if (headerStr.StartsWith("["))
                {
                    return SaverOutputType.Json;
                }

                return SaverOutputType.Bson;
            }
            finally
            {
                if (setToBegin)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }
            }
        }

        private Lazy<JsonSaver> jsonSaver = new Lazy<JsonSaver>(() => new JsonSaver());
        private Lazy<XmlSaver> xmlSaver = new Lazy<XmlSaver>(() => new XmlSaver());

        public void saveObject(Saveable save, Stream stream, SaverOutputType format = SaverOutputType.Json)
        {
            switch (format)
            {
                case SaverOutputType.Json:
                    jsonSaver.Value.saveObject(save, new JsonTextWriter(new StreamWriter(stream)));
                    break;
                case SaverOutputType.Bson:
                    jsonSaver.Value.saveObject(save, new BsonDataWriter(stream));
                    break;
                case SaverOutputType.Xml:
                    xmlSaver.Value.saveObject(save, new XmlTextWriter(stream, Encoding.Unicode)
                    {
                        Formatting = System.Xml.Formatting.Indented
                    });
                    break;
            }
        }

        public T restoreObject<T>(Stream stream, SaverOutputType format)
        {
            switch (format)
            {
                case SaverOutputType.Json:
                    return jsonSaver.Value.restoreObject<T>(new JsonTextReader(new StreamReader(stream)));
                case SaverOutputType.Bson:
                    return jsonSaver.Value.restoreObject<T>(new BsonDataReader(stream));
                case SaverOutputType.Xml:
                    return (T)xmlSaver.Value.restoreObject(new XmlTextReader(stream));
                default:
                    throw new NotImplementedException($"{nameof(SaverOutputType)} format {format} not supported.");
            }
        }
    }
}
