using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    class StartEnumParseState : ICodeEnumParserState
    {
        public ICodeEnumParserState Parse(string line, List<String> comment, CodeEnum code)
        {
            if (line.Contains("DILIGENT_TYPED_ENUM"))
            {
                var nameParse = line.Replace("DILIGENT_TYPED_ENUM", "");
                nameParse = nameParse.Replace("(", "");
                nameParse = nameParse.Replace(")", "");
                nameParse = nameParse.Trim();
                var splitName = nameParse.Split(',');
                code.Comment = comment;
                code.Name = splitName[0];
                code.BaseType = splitName[1];
            }
            else if (line.Contains("enum"))
            {
                code.Comment = comment;

                var nameParse = line.Replace("enum", "");
                code.Name = nameParse;
            }

            if (line.Contains("{"))
            {
                return new EnumParsePropertiesState();
            }

            return this;
        }
    }
}
