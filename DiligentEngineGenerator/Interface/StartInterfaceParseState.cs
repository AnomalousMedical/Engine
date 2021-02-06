using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class StartInterfaceParseState : ICodeInterfaceParserState
    {
        public ICodeInterfaceParserState Parse(string line, List<String> comment, CodeInterface code)
        {
            if (line.Contains("DILIGENT_BEGIN_INTERFACE"))
            {
                var nameParse = line.Replace("DILIGENT_BEGIN_INTERFACE", "");
                nameParse = nameParse.Replace("(", "");
                nameParse = nameParse.Replace(")", "");
                nameParse = nameParse.Trim();
                var splitName = nameParse.Split(',');
                code.Comment = comment;
                code.Name = splitName[0];
                code.BaseType = splitName[1];
            }
            else if (line.Contains("class"))
            {
                //Makes sure we have class with whitespace around it.
                var lineSplit = line.Split(null).ToList();
                var doubleCheckClass = lineSplit.IndexOf("class");
                if (doubleCheckClass != -1)
                {
                    var name = lineSplit.Skip(doubleCheckClass + 1).First();
                    code.Comment = comment;
                    code.Name = name;
                    //code.BaseType = splitName[1];
                }
            }

            if (line.Contains("{"))
            {
                return new InterfaceParseAllMethodsState();
            }

            return this;
        }
    }
}
