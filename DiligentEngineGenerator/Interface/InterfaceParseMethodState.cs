using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class InterfaceParseMethodState : ICodeInterfaceParserState
    {
        private readonly InterfaceMethod method;
        private bool firstLine = true;

        public InterfaceParseMethodState(InterfaceMethod method)
        {
            this.method = method;
        }

        public ICodeInterfaceParserState Parse(string line, List<String> comments, CodeInterface code)
        {
            var wasFirstLine = firstLine;
            firstLine = false;
            if (!String.IsNullOrWhiteSpace(line))
            {
                var propertyParse = line.Trim().Replace(",", "").Replace("{", "").Replace("}", "");
                var parenIndex = propertyParse.IndexOf(")");
                if(parenIndex != -1)
                {
                    propertyParse = propertyParse.Substring(0, parenIndex);
                }
                if (!String.IsNullOrWhiteSpace(propertyParse))
                {
                    propertyParse = propertyParse.Replace("const", "");
                    propertyParse = propertyParse.Replace("REF", "");
                    propertyParse = propertyParse.Replace("VIRTUAL", "");
                    propertyParse = propertyParse.Replace("struct", "");
                    propertyParse = propertyParse.Trim();

                    var typeAndName = propertyParse.Split(null).Where(i => !String.IsNullOrWhiteSpace(i)); //Split on whitespace

                    method.Args.Add(new InterfaceMethodArgument()
                    {
                        Type = typeAndName.First(),
                        Name = typeAndName.Skip(1).First(),
                        IsConst = line.Contains("const"),
                        IsRef = line.Contains("REF"),
                        IsPtr = line.Contains("*"),
                        IsPtrToPtr = line.Contains("**"),
                    });
                }
            }

            //Just checking for a ) worked for a long time to detect function end.
            //Now this is better, count () on the line with a special case for the
            //first line
            int parenCount = 0;
            bool hasParen = false;
            foreach (var parenMatch in line)
            {
                switch (parenMatch)
                {
                    case '(':
                        hasParen = true;
                        ++parenCount;
                        break;
                    case ')':
                        hasParen = true;
                        --parenCount;
                        break;
                }
            }

            if(hasParen && wasFirstLine)
            {
                //Penalize paren count on the first line to make this pass if everything is 1 line
                --parenCount;
            }

            if (parenCount < 0)
            {
                return new InterfaceParseAllMethodsState();
            }

            if (line.Contains("}"))
            {
                return null;
            }

            return this;
        }
    }
}
