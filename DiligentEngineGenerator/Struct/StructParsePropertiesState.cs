using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructParsePropertiesState : ICodeStructParserState
    {
        private const string DEFAULT_INITIALIZER = "DEFAULT_INITIALIZER";

        public ICodeStructParserState Parse(string line, List<String> comment, CodeStruct code)
        {
            if (!String.IsNullOrWhiteSpace(line))
            {
                var propertyParse = line.Trim().Replace(",", "").Replace("{", "").Replace("}", "").Replace("enum", "");
                if (!String.IsNullOrWhiteSpace(propertyParse))
                {
                    propertyParse = propertyParse.Replace("const", "").Replace("struct", "").Trim();

                    var typeAndName = propertyParse.Split(null).Where(i => !String.IsNullOrWhiteSpace(i)); //Split on whitespace

                    var property = new StructProperty()
                    {
                        Comment = comment,
                        Type = typeAndName.First(),
                        Name = typeAndName.Skip(1).First(),
                        IsConst = line.Contains("const")
                    };

                    if (propertyParse.Contains(DEFAULT_INITIALIZER))
                    {
                        property.DefaultValue = propertyParse.Substring(propertyParse.IndexOf(DEFAULT_INITIALIZER) + DEFAULT_INITIALIZER.Length)
                            .Replace("(", "")
                            .Replace(")", "")
                            .Trim();
                    }
                    else if (propertyParse.Contains("="))
                    {
                        property.DefaultValue = propertyParse.Substring(propertyParse.IndexOf("=") + 1)
                           .Trim();
                    }

                    //Check name to see if its an array
                    var name = property.Name;
                    var bracketIndex = name.IndexOf("[");
                    if (bracketIndex != -1)
                    {
                        property.IsArray = true;
                        property.ArrayLen = name.Substring(bracketIndex + 1).Replace("]", "").Trim();
                        property.Name = name.Substring(0, bracketIndex);
                    }

                    code.Properties.Add(property);
                }
            }

            if (line.Contains("}") && !line.Contains(DEFAULT_INITIALIZER))
            {
                return null;
            }

            return this;
        }
    }
}
