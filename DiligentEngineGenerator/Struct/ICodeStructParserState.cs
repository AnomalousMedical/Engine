using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    interface ICodeStructParserState
    {
        ICodeStructParserState Parse(String line, List<String> comment, CodeStruct code);
    }
}
