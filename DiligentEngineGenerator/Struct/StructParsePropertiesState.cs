using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructParsePropertiesState : ICodeStructParserState
    {
        public ICodeStructParserState Parse(string line, StringBuilder commentBuilder, ref CodeStruct code)
        {
            if (!String.IsNullOrWhiteSpace(line))
            {
                var propertyParse = line.Trim().Replace(",", "").Replace("{", "").Replace("}", "");
                if (!String.IsNullOrWhiteSpace(propertyParse))
                {
                    if (propertyParse.Contains("DEFAULT_INITIALIZER"))
                    {
                        propertyParse = propertyParse.Substring(0, propertyParse.IndexOf("DEFAULT_INITIALIZER"));
                    }

                    propertyParse = propertyParse.Replace("const", "").Trim();

                    var typeAndName = propertyParse.Split(null); //Split on whitespace

                    code.Properties.Add(new StructProperty()
                    {
                        Comment = commentBuilder.ToString(),
                        Type = typeAndName[0],
                        Name = typeAndName[1],
                        IsConst = line.Contains("const")
                    });
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
