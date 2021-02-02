using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    class EnumParsePropertiesState : ICodeEnumParserState
    {
        public ICodeEnumParserState Parse(string line, StringBuilder commentBuilder, ref CodeEnum codeEnum)
        {
            if (!String.IsNullOrWhiteSpace(line))
            {
                var propertyParse = line.Trim().Replace(",", "").Replace("{", "").Replace("}", "");
                if (!String.IsNullOrWhiteSpace(propertyParse))
                {
                    if (line.Contains("="))
                    {
                        var split = propertyParse.Split("=");
                        codeEnum.Properties.Add(new EnumProperty()
                        {
                            Comment = commentBuilder.ToString(),
                            Name = split[0].Trim(),
                            Value = split[1].Trim()
                        });
                    }
                    else
                    {
                        codeEnum.Properties.Add(new EnumProperty()
                        {
                            Comment = commentBuilder.ToString(),
                            Name = propertyParse.Trim(),
                        });
                    }
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
