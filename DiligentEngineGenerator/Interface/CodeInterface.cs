using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class CodeInterface
    {
        public String BaseType { get; set; }

        public String Name { get; set; }

        public String Comment { get; set; }

        public List<InterfaceMethod> Properties { get; set; } = new List<InterfaceMethod>(); 
        
        public static CodeInterface Find(String file, int startLine, int endLine)
        {
            var commentBuilder = new StringBuilder();
            using var reader = new StreamReader(File.OpenRead(file));
            var lines = reader.ReadLines().Skip(startLine).Take(endLine - startLine);
            CodeInterface code = new CodeInterface();
            ICodeInterfaceParserState currentState = new StartInterfaceParseState();

            foreach (var line in lines.Select(l => l.Replace(";", "")))
            {
                Console.WriteLine(line);
                if (!String.IsNullOrWhiteSpace(line) && !CommentParser.Find(line, commentBuilder))
                {
                    var parsed = CommentParser.RemoveComments(line);
                    currentState = currentState.Parse(parsed, commentBuilder, ref code);
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

    class InterfaceMethod
    {
        public String Comment { get; set; }

        public String Name { get; set; }

        public string ReturnType { get; set; }

        public List<InterfaceMethodArgument> Args { get; set; } = new List<InterfaceMethodArgument>();
    }

    class InterfaceMethodArgument
    {
        public String Name { get; set; }

        public string Type { get; set; }

        public bool IsRef { get; set; }

        public bool IsConst { get; set; }

        public bool IsPtr { get; set; }

        public bool IsPtrToPtr { get; set; }
    }
}
