using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class CodeStruct
    {
        public String BaseType { get; set; }

        public String Name { get; set; }

        public String Comment { get; set; }

        public List<StructProperty> Properties { get; set; } = new List<StructProperty>(); 
        
        public static CodeStruct Find(String file, int startLine, int endLine)
        {
            var commentBuilder = new StringBuilder();
            using var reader = new StreamReader(File.OpenRead(file));
            var lines = reader.ReadLines().Skip(startLine).Take(endLine - startLine);
            CodeStruct code = new CodeStruct();
            ICodeStructParserState currentState = new StartStructParseState();

            foreach (var line in lines.Select(l => l.Replace(";", "")))
            {
                Console.WriteLine(line);
                if (!String.IsNullOrWhiteSpace(line) && !CommentParser.Find(line, commentBuilder))
                {
                    currentState = currentState.Parse(line, commentBuilder, ref code);
                    commentBuilder.Clear();
                    if (currentState == null)
                    {
                        break;
                    }
                }
            }

            return code;
        }
    }

    class StructProperty
    {
        public String Comment { get; set; }

        public String Name { get; set; }

        public bool IsConst { get; set; }

        public string Type { get; set; }
    }
}
