using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    interface ICodeEnumParserState
    {
        ICodeEnumParserState Parse(String line, StringBuilder commentBuilder, ref CodeEnum codeEnum);
    }
}
