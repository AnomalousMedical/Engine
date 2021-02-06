using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    class SkipCppInterface : ICodeInterfaceParserState
    {
        public ICodeInterfaceParserState Parse(string line, List<String> comment, CodeInterface code)
        {
            if (line.Contains("#endif"))
            {
                return new InterfaceParseAllMethodsState();
            }

            return this;
        }
    }
}
