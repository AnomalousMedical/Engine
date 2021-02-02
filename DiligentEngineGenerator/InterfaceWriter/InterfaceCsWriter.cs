using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    static class InterfaceCsWriter
    {
        public static void Write(CodeInterface code, String file)
        {
            var dir = Path.GetDirectoryName(file);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using var writer = new StreamWriter(File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None));
            Write(code, writer);
        }

        public static void Write(CodeInterface code, StreamWriter writer)
        {
            writer.WriteLine(
$@"using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Engine;

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

            //Public interface
            foreach (var item in code.Methods)
            {
                writer.Write($"        public {GetCSharpType(item.ReturnType)} {item.Name}(");

                var sep = "";

                foreach(var arg in item.Args)
                {
                    writer.Write($"{sep}{GetCSharpType(arg.Type)} {arg.Name}");
                    sep = ", ";
                }

                writer.WriteLine(")");
                writer.WriteLine("        {");

                if (item.ReturnType != "void")
                {
                    writer.Write($"            return new {GetCSharpType(item.ReturnType)}({code.Name}_{item.Name}(this.objPtr");
                }
                else
                {
                    writer.Write($"            {code.Name}_{item.Name}(this.objPtr");
                }

                foreach (var arg in item.Args)
                {
                    writer.Write($", {arg.Name}");
                    var pInvokeType = GetPInvokeType(arg);
                    if(pInvokeType == "IntPtr")
                    {
                        writer.Write(".objPtr");
                    }
                }

                if (item.ReturnType != "void")
                {
                    writer.WriteLine($"));");
                }
                else
                {
                    writer.WriteLine($");");
                }

                writer.WriteLine("        }");
            }

            writer.WriteLine();
            writer.WriteLine();

            //PInvoke
            foreach (var item in code.Methods)
            {
                writer.WriteLine("        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]");
                writer.Write($"        private static extern {GetPInvokeType(item)} {code.Name}_{item.Name}(IntPtr objPtr");

                foreach (var arg in item.Args)
                {
                    writer.Write($", {GetPInvokeType(arg)} {arg.Name}");
                }

                writer.WriteLine($");");
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

            return type.Replace("*", "");
        }

        private static String GetPInvokeType(InterfaceMethodArgument arg)
        {
            switch (arg.Type)
            {
                case "Char*":
                    return "String";
                case "Color":
                    return "Color";
            }

            if (arg.IsPtr)
            {
                return "IntPtr";
            }

            return arg.Type;
        }

        private static String GetPInvokeType(InterfaceMethod arg)
        {
            switch (arg.ReturnType)
            {
                case "Char*":
                    return "String";
            }

            if (arg.IsPtr)
            {
                return "IntPtr";
            }

            return arg.ReturnType;
        }
    }
}
