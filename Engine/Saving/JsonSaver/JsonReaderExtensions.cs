using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Saving.JsonSaver
{
    static class JsonReaderExtensions
    {
        public static T[] ReadArray<T>(this JsonReader xmlReader, int length, Func<JsonReader, T> readCb, bool throwOnLengthMismatch = true)
        {
            T[] arr = new T[length];
            int index = ReadArray(xmlReader, arr, readCb);

            if (throwOnLengthMismatch && index != arr.Length)
            {
                throw new InvalidOperationException("Read array length does not match passed in length.");
            }

            return arr;
        }

        public static int ReadArray<T>(this JsonReader xmlReader, T[] arr, Func<JsonReader, T> readCb)
        {
            if (xmlReader.TokenType != JsonToken.StartArray)
            {
                throw new InvalidOperationException("Array reader is not on a StartArray token.");
            }

            int index = 0;
            bool keepReading = true;
            while (keepReading && xmlReader.Read())
            {
                switch (xmlReader.TokenType)
                {
                    default:
                        arr[index++] = readCb(xmlReader);
                        break;
                    case JsonToken.EndArray:
                        keepReading = false;
                        break;
                }
            }

            return index;
        }
    }
}
