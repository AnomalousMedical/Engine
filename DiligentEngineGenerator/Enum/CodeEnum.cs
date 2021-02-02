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

        public String Comment { get; set; }

        public List<EnumProperty> Properties { get; set; } = new List<EnumProperty>(); 
        
        public static CodeEnum Find(String file, int startLine, int endLine)
        {
            var commentBuilder = new StringBuilder();
            using var reader = new StreamReader(File.OpenRead(file));
            var lines = reader.ReadLines().Skip(startLine).Take(endLine - startLine);
            CodeEnum codeEnum = new CodeEnum();
            ICodeEnumParserState currentState = new StartEnumParseState();

            foreach (var line in lines.Select(l => l.Replace(";", "")))
            {
                Console.WriteLine(line);
                if (!String.IsNullOrWhiteSpace(line) && !CommentParser.Find(line, commentBuilder))
                {
                    var parsed = CommentParser.RemoveComments(line);
                    currentState = currentState.Parse(parsed, commentBuilder, ref codeEnum);
                    commentBuilder.Clear();
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
        public String Comment { get; set; }

        public String Name { get; set; }

        public String Value { get; set; }
    }
}
