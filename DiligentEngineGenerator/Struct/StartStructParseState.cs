using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    class StartStructParseState : ICodeStructParserState
    {
        public ICodeStructParserState Parse(string line, List<String> comment, CodeStruct codeStruct)
        {
            if (line.Contains("struct"))
            {
                codeStruct.Comment = comment;

                var nameParse = line.Replace("struct", "");
                var split = nameParse.Split("DILIGENT_DERIVE(");
                if(split.Length > 0)
                {
                    codeStruct.Name = split[0].Trim();
                }
                if(split.Length > 1)
                {
                    codeStruct.BaseType = split[1].Replace(")", "").Trim();
                }
            }

            if (line.Contains("{") || line.Contains("DILIGENT_DERIVE")) //If there is a DILIGENT_DERIVE there will be no opening bracket
            {
                return new StructParsePropertiesState();
            }

            return this;
        }
    }
}
