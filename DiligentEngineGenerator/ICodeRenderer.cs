using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class CodeRendererContext
    {
        public CodeRendererContext(CodeTypeInfo codeTypeInfo)
        {
            this.CodeTypeInfo = codeTypeInfo;
        }

        public CodeTypeInfo CodeTypeInfo { get; private set; }
    }

    interface ICodeRenderer
    {
        public void Render(TextWriter writer, CodeRendererContext context);
    }
}
