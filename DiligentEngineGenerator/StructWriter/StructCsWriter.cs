using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    static class StructCsWriter
    {
        public static void Write(CodeStruct code, String file)
        {
            var dir = Path.GetDirectoryName(file);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using var writer = new StreamWriter(File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None));
            Write(code, writer);
        }

        public static void Write(CodeStruct code, StreamWriter writer)
        {
            writer.WriteLine(
$@"using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

{DiligentTypeMapper.Usings}

namespace DiligentEngine
{{");


            if (code.BaseType != null)
            {
                writer.WriteLine($"    public partial class {code.Name} : {code.BaseType}");

                writer.WriteLine(
$@"    {{
            public {code.Name}(IntPtr objPtr)
                : base(objPtr)
            {{

            }}");
            }
            else
            {
                writer.WriteLine($"    public partial class {code.Name}");

                writer.WriteLine(
$@"    {{
        internal protected IntPtr objPtr;

        public IntPtr ObjPtr => objPtr;

        public {code.Name}(IntPtr objPtr)
        {{
            this.objPtr = objPtr;
        }}");
            }

            foreach (var item in code.Properties)
            {
                writer.WriteLine($"        public {GetCSharpType(item.Type)} {item.Name} {{get; set;}}");
            }

            writer.WriteLine("    }");

            writer.WriteLine("}");
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
    }
}
