using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    static class InterfaceCppWriter
    {
        public static void Write(CodeInterface code, String file, IEnumerable<String> includeDirs)
        {
            var dir = Path.GetDirectoryName(file);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using var writer = new StreamWriter(File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None));
            Write(code, writer, includeDirs);
        }

        public static void Write(CodeInterface code, StreamWriter writer, IEnumerable<String> includeDirs)
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
                    writer.Write($"{sep}{arg.Name}");
                    sep = ", ";
                }
                writer.WriteLine(");");
                writer.WriteLine("}");
            }
        }
    }
}
