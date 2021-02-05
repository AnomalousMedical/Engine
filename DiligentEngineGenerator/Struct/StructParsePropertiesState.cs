using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructParsePropertiesState : ICodeStructParserState
    {
        private const string DEFAULT_INITIALIZER = "DEFAULT_INITIALIZER";

        public ICodeStructParserState Parse(string line, StringBuilder commentBuilder, ref CodeStruct code)
        {
            if (!String.IsNullOrWhiteSpace(line))
            {
                var propertyParse = line.Trim().Replace(",", "").Replace("{", "").Replace("}", "").Replace("enum", "");
                if (!String.IsNullOrWhiteSpace(propertyParse))
                {
                    propertyParse = propertyParse.Replace("const", "").Replace("struct", "").Trim();

                    var withInitialize = propertyParse;
                    bool hasDefault = false;
                    if (propertyParse.Contains(DEFAULT_INITIALIZER))
                    {
                        hasDefault = true;
                        propertyParse = propertyParse.Substring(0, propertyParse.IndexOf(DEFAULT_INITIALIZER));
                    }

                    var typeAndName = propertyParse.Split(null).Where(i => !String.IsNullOrWhiteSpace(i)); //Split on whitespace

                    var property = new StructProperty()
                    {
                        Comment = commentBuilder.ToString(),
                        Type = typeAndName.First(),
                        Name = typeAndName.Skip(1).First(),
                        IsConst = line.Contains("const")
                    };

                    //Check name to see if its an array
                    var name = property.Name;
                    var bracketIndex = name.IndexOf("[");
                    if (bracketIndex != -1)
                    {
                        property.IsArray = true;
                        property.ArrayLen = name.Substring(bracketIndex + 1).Replace("]", "").Trim();
                        property.Name = name.Substring(0, bracketIndex);
                    }

                    if (hasDefault)
                    {
                        property.DefaultValue = withInitialize.Substring(withInitialize.IndexOf(DEFAULT_INITIALIZER) + DEFAULT_INITIALIZER.Length)
                            .Replace("(", "")
                            .Replace(")", "")
                            .Trim();
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
