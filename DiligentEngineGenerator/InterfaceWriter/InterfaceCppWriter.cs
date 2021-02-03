using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                foreach (var arg in item.Args.Where(i => !i.MakeReturnVal))
                {
                    writer.Write($", {arg.Type} {arg.Name}");
                }

                var makeReturnArg = item.Args.Where(i => i.MakeReturnVal).FirstOrDefault();

                writer.WriteLine($")");
                writer.WriteLine("{");
                if(item.ReturnType != "void")
                {
                    if(makeReturnArg != null)
                    {
                        writer.WriteLine($"	{makeReturnArg.Type} {makeReturnArg.Name} = nullptr;");
                        writer.WriteLine($"	objPtr->{item.Name}(");
                    }
                    else
                    {
                        writer.WriteLine($"	return objPtr->{item.Name}(");
                    }
                }
                else
                {
                    writer.WriteLine($"	objPtr->{item.Name}(");
                }
                var sep = "		";
                foreach (var arg in item.Args)
                {
                    var typePrefix = "";
                    if (arg.MakeReturnVal)
                    {
                        typePrefix = "&";
                    }

                    writer.WriteLine($"{sep}{typePrefix}{arg.CppPrefix}{arg.Name}");
                    sep = "		, ";
                }
                writer.WriteLine("	);");

                if (makeReturnArg != null)
                {
                    writer.WriteLine($"	return {makeReturnArg.Name};");
                }

                writer.WriteLine("}");
            }
        }
    }
}
