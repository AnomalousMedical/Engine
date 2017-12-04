using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Saving.Json
{
    static class JsonReaderExtensions
    {
        public static T[] ReadArray<T>(this JsonReader xmlReader, int length, Func<Object, T> convert, bool throwOnLengthMismatch = true)
        {
            T[] arr = new T[length];
            int index = ReadArray(xmlReader, arr, convert);

            if (throwOnLengthMismatch && index != arr.Length)
            {
                throw new InvalidOperationException("Read array length does not match passed in length.");
            }

            return arr;
        }

        public static int ReadArray<T>(this JsonReader xmlReader, T[] arr, Func<Object, T> convert)
        {
            xmlReader.Read();
            if (xmlReader.TokenType != JsonToken.StartArray)
            {
                throw new InvalidOperationException("Array reader is not on a StartArray token.");
            }
            
            int index = 0;
            bool keepReading = true;
            while (keepReading)
            {
                xmlReader.Read();
                if (keepReading = xmlReader.TokenType != JsonToken.EndArray)
                {
                    arr[index++] = convert(xmlReader.Value);
                }
            }

            return index;
        }
    }
}
