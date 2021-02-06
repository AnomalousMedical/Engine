using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class InterfaceParseMethodState : ICodeInterfaceParserState
    {
        private readonly InterfaceMethod method;

        public InterfaceParseMethodState(InterfaceMethod method)
        {
            this.method = method;
        }

        public ICodeInterfaceParserState Parse(string line, List<String> comments, CodeInterface code)
        {
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

            if (line.Contains(")"))
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
