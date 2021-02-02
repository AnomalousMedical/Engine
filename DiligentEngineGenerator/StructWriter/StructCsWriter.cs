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
                writer.WriteLine($"        public {GetCSharpType(item.Type)} {item.Name}");
                writer.WriteLine($"        {{");
                writer.WriteLine($"            get");
                writer.WriteLine($"            {{");
                writer.WriteLine($"                return {code.Name}_Get_{item.Name}(this.objPtr);");
                writer.WriteLine($"            }}");
                writer.WriteLine($"            set");
                writer.WriteLine($"            {{");
                writer.WriteLine($"                {code.Name}_Set_{item.Name}(this.objPtr, value);");
                writer.WriteLine($"            }}");
                writer.WriteLine($"        }}");
            }

            //PInvoke
            writer.WriteLine();
            writer.WriteLine();

            CustomPInvoke(code, writer);

            foreach (var item in code.Properties)
            {
                writer.WriteLine("        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]");
                writer.WriteLine($"        private static extern {GetPInvokeType(item)} {code.Name}_Get_{item.Name}(IntPtr objPtr);");

                writer.WriteLine("        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]");
                writer.WriteLine($"        private static extern void {code.Name}_Set_{item.Name}(IntPtr objPtr, {GetPInvokeType(item)} value);");
            }

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
