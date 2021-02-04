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

    class StructProperty
    {
        public String Comment { get; set; }

        public String Name { get; set; }

        public bool IsConst { get; set; }

        public string Type { get; set; }

        public string LookupType => Type.Replace("*", "").Trim();

        public string DefaultValue { get; set; }

        public bool IsArray { get; set; }

        public String ArrayLen { get; set; }

        public bool IsUnknownSizeArray => String.IsNullOrWhiteSpace(ArrayLen);

        public int ArrayLenInt
        {
            get
            {
                if (int.TryParse(ArrayLen, out int len))
                {
                    return len;
                }
                return 0;
            }

        }

        /// <summary>
        /// The property that this property stores its size on for auto size. Should define TakeAutoSize on that property.
        /// </summary>
        public string PutAutoSize { get; set; }

        /// <summary>
        /// The name of the property to read the size from. Should also set PutAutoSize on that property.
        /// </summary>
        public string TakeAutoSize { get; set; }
    }
}
