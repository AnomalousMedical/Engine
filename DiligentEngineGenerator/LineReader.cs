using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class LineReader
    {
        public static IEnumerable<String> ReadLines(IEnumerable<String> inputLines, int startLine, int endLine, IEnumerable<int> skipLines = null)
        {
            if (skipLines == null)
            {
                skipLines = new List<int>(0);
            }

            var commentBuilder = new StringBuilder();
            var beforeStart = startLine - 1;
            var lines = inputLines.Skip(startLine - 1).Take(endLine - beforeStart);
            CodeStruct code = new CodeStruct();
            ICodeStructParserState currentState = new StartStructParseState();

            var lineNumber = startLine;
            foreach (var line in lines.Select(l => l.Replace(";", "")))
            {
                try
                {
                    if (!skipLines.Contains(lineNumber))
                    {
                        Console.WriteLine($"{lineNumber}: {line}");
                        yield return line;
                    }

                }
                finally
                {
                    ++lineNumber;
                }
            }
        }
    }
}
