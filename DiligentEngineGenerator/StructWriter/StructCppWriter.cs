using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCppWriter
    {
        public static void Write(CodeStruct code, String file, IEnumerable<String> includeDirs, StructCppWriter structCppWriter = null)
        {
            if (structCppWriter == null)
            {
                structCppWriter = new StructCppWriter();
            }

            var dir = Path.GetDirectoryName(file);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using var writer = new StreamWriter(File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None));
            structCppWriter.Write(code, writer, includeDirs);
        }

        public void Write(CodeStruct code, StreamWriter writer, IEnumerable<String> includeDirs)
        {
            writer.WriteLine("#include \"StdAfx.h\"");
            foreach (var inc in includeDirs)
            {
                writer.WriteLine($"#include \"{inc}\"");
            }
            writer.WriteLine("using namespace Diligent;");
            writer.WriteLine();

            WriteCustomPInvoke(code, writer);

            //PInvoke
            foreach (var item in code.Properties)
            {
                var constStr = item.IsConst ? "const " : "";

                writer.WriteLine(
@$"extern ""C"" _AnomalousExport {constStr}{item.Type} {code.Name}_Get_{item.Name}({code.Name}* objPtr)
{{
    return objPtr->{item.Name};
}}
");

                writer.WriteLine(
$@"extern ""C"" _AnomalousExport void {code.Name}_Set_{item.Name}({code.Name}* objPtr, {constStr}{item.Type} value)
{{
    objPtr->{item.Name} = value;
}}
");
            }
        }

        protected virtual void WriteCustomPInvoke(CodeStruct code, StreamWriter writer)
        {

        }
    }
}
