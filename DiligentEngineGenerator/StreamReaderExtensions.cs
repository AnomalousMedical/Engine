using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    public static class StreamReaderExtensions
    {
        public static IEnumerable<String> ReadLines(this StreamReader reader)
        {
            var line = reader.ReadLine();
            while (line != null)
            {
                yield return line;
                line = reader.ReadLine();
            }
        }
    }
}
