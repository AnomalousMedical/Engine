using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class InterfaceCppWriter : ICodeRenderer
    {
        private readonly CodeInterface code;
        private readonly IEnumerable<string> includeDirs;

        public InterfaceCppWriter(CodeInterface code, IEnumerable<String> includeDirs)
        {
            this.code = code;
            this.includeDirs = includeDirs;
        }

        public void Render(TextWriter writer, CodeRendererContext context)
        {
            writer.WriteLine("#include \"StdAfx.h\"");
            foreach(var inc in includeDirs)
            {
                writer.WriteLine($"#include \"{inc}\"");
            }
            writer.WriteLine("using namespace Diligent;");

            //PInvoke
            foreach (var item in code.Methods)
            {
                writer.Write($"extern \"C\" _AnomalousExport {item.ReturnType} {code.Name}_{item.Name}({code.Name}* objPtr");

                foreach (var arg in item.Args)
                {
                    writer.Write($", {arg.Type} {arg.Name}");
                }

                writer.WriteLine($")");
                writer.WriteLine("{");
                if(item.ReturnType != "void")
                {
                    writer.Write($"	return objPtr->{item.Name}(");
                }
                else
                {
                    writer.Write($"	objPtr->{item.Name}(");
                }
                var sep = "";
                foreach (var arg in item.Args)
                {
                    writer.Write($"{sep}{arg.CppPrefix}{arg.Name}");
                    sep = ", ";
                }
                writer.WriteLine(");");
                writer.WriteLine("}");
            }
        }
    }
}
