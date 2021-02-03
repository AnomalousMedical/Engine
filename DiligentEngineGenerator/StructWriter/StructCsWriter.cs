using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCsWriter
    {
        public static void Write(CodeStruct code, String file, StructCsWriter fileWriter = null)
        {
            if (fileWriter == null)
            {
                fileWriter = new StructCsWriter();
            }

            var dir = Path.GetDirectoryName(file);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using var writer = new StreamWriter(File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None));
            fileWriter.Write(code, writer);
        }

        public void Write(CodeStruct code, StreamWriter writer)
        {
            writer.WriteLine(
$@"using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

{DiligentTypeMapper.Usings}

namespace DiligentEngine
{{");

            WriteClassNameAndConstructor(code, writer);

            foreach (var item in code.Properties)
            {
                writer.WriteLine($"        public {GetCSharpType(item.Type)} {item.Name} {{ get; set; }}");
            }

            //PInvoke
            writer.WriteLine();
            writer.WriteLine();

            writer.WriteLine("    }");

            writer.WriteLine("}");
        }

        protected virtual void WriteClassNameAndConstructor(CodeStruct code, StreamWriter writer)
        {
            if (code.BaseType != null)
            {
                writer.WriteLine($"    public partial class {code.Name} : {code.BaseType}");

                writer.WriteLine(
$@"    {{
        public {code.Name}()
        {{

        }}");
            }
            else
            {
                writer.WriteLine($"    public partial class {code.Name}");

                writer.WriteLine(
$@"    {{

        public {code.Name}()
        {{
            
        }}");
            }
        }

        private static String GetCSharpType(String type)
        {
            switch (type)
            {
                case "Char*":
                    return "String";
            }

            return type;
        }

        private static String GetPInvokeType(StructProperty arg)
        {
            switch (arg.Type)
            {
                case "Char*":
                    return "String";
            }

            //if (arg.IsPtr)
            //{
            //    return "IntPtr";
            //}

            return arg.Type;
        }

        protected virtual void CustomPInvoke(CodeStruct code, StreamWriter writer)
        {
        }
    }
}
