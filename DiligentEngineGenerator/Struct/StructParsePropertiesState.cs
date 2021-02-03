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
                var propertyParse = line.Trim().Replace(",", "").Replace("{", "").Replace("}", "");
                if (!String.IsNullOrWhiteSpace(propertyParse))
                {
                    propertyParse = propertyParse.Replace("const", "").Trim();

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
