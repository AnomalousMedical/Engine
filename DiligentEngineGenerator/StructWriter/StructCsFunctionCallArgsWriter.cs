using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCsFunctionCallArgsWriter : ICodeRenderer
    {
        private CodeStruct code;

        public StructCsFunctionCallArgsWriter(CodeStruct code)
        {
            this.code = code;
        }

        public void Render(TextWriter writer, CodeRendererContext context)
        {
            foreach (var item in code.Properties)
            {
                writer.WriteLine($", {code.Name}_{item.Name}");
            }
        }
    }
}
