using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    class InterfaceParseAllMethodsState : ICodeInterfaceParserState
    {
        public ICodeInterfaceParserState Parse(string line, StringBuilder commentBuilder, ref CodeInterface code)
        {
            if (!String.IsNullOrWhiteSpace(line))
            {
                var parsed = line.Trim().Replace(",", "").Replace("{", "").Replace("}", "");
                if (!String.IsNullOrWhiteSpace(parsed) && parsed.Contains("METHOD"))
                {
                    parsed = parsed.Replace("const", "");
                    parsed = parsed.Replace("VIRTUAL", "");
                    parsed = parsed.Trim();

                    var typeAndName = parsed.Split("METHOD("); //Split on whitespace

                    var method = new InterfaceMethod()
                    {
                        Comment = commentBuilder.ToString(),
                        ReturnType = typeAndName[0].Replace("REF", "").Trim(),
                        Name = typeAndName[1].Substring(0, typeAndName[1].IndexOf(")")).Trim(),
                        IsConst = line.Contains("const"),
                        IsRef = line.Contains("REF"),
                        IsPtr = line.Contains("*"),
                        IsPtrToPtr = line.Contains("**"),
                    };
                    code.Methods.Add(method);
                    if (!line.Contains("THIS)"))
                    {
                        return new InterfaceParseMethodState(method);
                    }
                }
            }

            if (line.Contains("}"))
            {
                return null;
            }

            return this;
        }
    }
}
