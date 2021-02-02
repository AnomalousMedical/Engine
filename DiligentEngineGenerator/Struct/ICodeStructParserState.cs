using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    interface ICodeStructParserState
    {
        ICodeStructParserState Parse(String line, StringBuilder commentBuilder, ref CodeStruct code);
    }
}
