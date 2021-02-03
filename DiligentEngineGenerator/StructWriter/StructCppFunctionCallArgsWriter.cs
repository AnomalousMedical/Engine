using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCppFunctionCallArgsWriter : ICodeRenderer
    {
        private readonly string argName;
        private CodeStruct code;
        private readonly string tabs;

        public StructCppFunctionCallArgsWriter(String argName, CodeStruct code, String tabs)
        {
            this.argName = argName;
            this.code = code;
            this.tabs = tabs;
        }

        public void Render(TextWriter writer, CodeRendererContext context)
        {
            foreach (var item in code.Properties)
            {
                writer.WriteLine($"{tabs}, {argName}_{item.Name}");
            }
        }
    }
}
