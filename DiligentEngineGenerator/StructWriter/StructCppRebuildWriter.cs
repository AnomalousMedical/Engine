using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCppRebuildWriter : ICodeRenderer
    {
        private readonly string argName;
        private CodeStruct code;
        private readonly string tabs;

        public StructCppRebuildWriter(String argName, CodeStruct code, String tabs)
        {
            this.argName = argName;
            this.code = code;
            this.tabs = tabs;
        }

        public void Render(TextWriter writer, CodeRendererContext context)
        {
            writer.WriteLine($"{tabs}{code.Name} {argName};");

            foreach (var item in code.Properties)
            {
                writer.WriteLine($"{tabs}{argName}.{item.Name} = {argName}_{item.Name};");
            }
        }
    }
}
