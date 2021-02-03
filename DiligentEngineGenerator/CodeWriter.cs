using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class CodeWriter
    {
        private List<(ICodeRenderer renderer, String file)> codeRenderers = new List<(ICodeRenderer, String)>();

        public CodeWriter()
        {

        }

        public void AddWriter(ICodeRenderer renderer, String file)
        {
            this.codeRenderers.Add((renderer, file));
        }

        public void WriteFiles(CodeRendererContext context)
        {
            foreach (var i in codeRenderers)
            {
                using (var writer = new StreamWriter(File.Open(i.file, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)))
                {
                    i.renderer.Render(writer, context);
                }
            }
        }
    }
}
