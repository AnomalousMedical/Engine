using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    interface ICodeEnumParserState
    {
        ICodeEnumParserState Parse(String line, List<String> comment, CodeEnum codeEnum);
    }
}
