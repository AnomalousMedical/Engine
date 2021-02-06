using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class CodeEnum
    {
        public String BaseType { get; set; }

        public String Name { get; set; }

        public IEnumerable<String> Comment { get; set; }

        public List<EnumProperty> Properties { get; set; } = new List<EnumProperty>(); 
        
        public static CodeEnum Find(String file, int startLine, int endLine, IEnumerable<int> skipLines = null)
        {
            var comment = new List<String>();
            using var reader = new StreamReader(File.OpenRead(file));
            var lines = LineReader.ReadLines(reader.ReadLines(), startLine, endLine, skipLines);
            CodeEnum codeEnum = new CodeEnum();
            ICodeEnumParserState currentState = new StartEnumParseState();

            foreach (var line in lines.Select(l => l.Replace(";", "")))
            {
                if (!String.IsNullOrWhiteSpace(line) && !CommentParser.Find(line, comment))
                {
                    var parsed = CommentParser.RemoveComments(line);
                    currentState = currentState.Parse(parsed, comment, codeEnum);
                    comment.Clear();
                    if (currentState == null)
                    {
                        break;
                    }
                }
            }

            return codeEnum;
        }
    }

    class EnumProperty
    {
        public IEnumerable<String> Comment { get; set; }

        public String Name { get; set; }

        public String Value { get; set; }
    }
}
