using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCsWriter : ICodeRenderer
    {
        private CodeStruct code;

        public StructCsWriter(CodeStruct code)
        {
            this.code = code;
        }

        public void Render(TextWriter writer, CodeRendererContext context)
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
    }
}
