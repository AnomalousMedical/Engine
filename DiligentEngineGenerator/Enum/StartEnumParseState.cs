using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    class StartEnumParseState : ICodeEnumParserState
    {
        public ICodeEnumParserState Parse(string line, StringBuilder comment, ref CodeEnum codeEnum)
        {
            if (line.Contains("DILIGENT_TYPED_ENUM"))
            {
                var nameParse = line.Replace("DILIGENT_TYPED_ENUM", "");
                nameParse = nameParse.Replace("(", "");
                nameParse = nameParse.Replace(")", "");
                nameParse = nameParse.Trim();
                var splitName = nameParse.Split(',');
                codeEnum.Comment = comment.ToString();
                codeEnum.Name = splitName[0];
                codeEnum.BaseType = splitName[1];
            }

            if (line.Contains("{"))
            {
                return new EnumParsePropertiesState();
            }

            return this;
        }
    }
}
