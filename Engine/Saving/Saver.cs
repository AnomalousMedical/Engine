using Engine.Saving.Json;
using Engine.Saving.XMLSaver;
using Engine.Utility;
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
        private Lazy<JsonSaver> jsonSaver;
        private Lazy<XmlSaver> xmlSaver;

        public Saver()
            : this(new DefaultTypeFinder())
        {

        }

        public Saver(TypeFinder typeFinder)
        {
            jsonSaver = new Lazy<JsonSaver>(() => new JsonSaver(typeFinder));
            xmlSaver = new Lazy<XmlSaver>(() => new XmlSaver(typeFinder));
        }

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

                if (CheckForEncodedString(header, "["))
                {
                    return SaverOutputType.Json;
                }

                if (CheckForEncodedString(header, "<Save>") || CheckForEncodedString(header, "<?xml"))
                {
                    return SaverOutputType.Xml;
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

        private static bool CheckForEncodedString(byte[] bytes, String check)
        {
            return Encoding.UTF8.GetString(bytes).StartsWith(check) ||
                Encoding.Unicode.GetString(bytes).StartsWith(check) ||
                Encoding.ASCII.GetString(bytes).StartsWith(check);
        }

        public void saveObject(Saveable save, Stream stream, SaverOutputType format = SaverOutputType.Json)
        {
            stream = new NoCloseStream(stream);
            switch (format)
            {
                case SaverOutputType.Json:
                    using (var writer = new JsonTextWriter(new StreamWriter(stream)))
                    {
                        if (WritePretty)
                        {
                            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                        }
                        jsonSaver.Value.saveObject(save, writer);
                    }
                    break;
                case SaverOutputType.Bson:
                    using (var writer = new BsonDataWriter(stream))
                    {
                        jsonSaver.Value.saveObject(save, writer);
                    }
                    break;
                case SaverOutputType.Xml:
                    using (var writer = new XmlTextWriter(stream, Encoding.Unicode))
                    {
                        if (WritePretty)
                        {
                            writer.Formatting = System.Xml.Formatting.Indented;
                        }
                        xmlSaver.Value.saveObject(save, writer);
                    }
                    break;
            }
        }

        public T restoreObject<T>(Stream stream)
        {
            return restoreObject<T>(stream, TypeFromStream(stream));
        }

        public T restoreObject<T>(Stream stream, SaverOutputType format)
        {
            stream = new NoCloseStream(stream);
            switch (format)
            {
                case SaverOutputType.Json:
                    using (var reader = new JsonTextReader(new StreamReader(stream)))
                    {
                        return jsonSaver.Value.restoreObject<T>(reader);
                    }
                case SaverOutputType.Bson:
                    using (var reader = new BsonDataReader(stream)
                    {
                        ReadRootValueAsArray = true
                    })
                    {
                        return jsonSaver.Value.restoreObject<T>(reader);
                    }
                case SaverOutputType.Xml:
                    using (var reader = new XmlTextReader(stream))
                    {
                        return (T)xmlSaver.Value.restoreObject(reader);
                    }
                default:
                    throw new NotImplementedException($"{nameof(SaverOutputType)} format {format} not supported.");
            }
        }

        /// <summary>
        /// Set this to false to write the output as small as possible instead of in a human readable way.
        /// </summary>
        public bool WritePretty { get; set; } = true;
    }
}
