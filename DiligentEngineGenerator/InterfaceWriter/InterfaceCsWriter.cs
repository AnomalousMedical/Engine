using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class InterfaceCsWriter : ICodeRenderer
    {
        private readonly CodeInterface code;

        public InterfaceCsWriter(CodeInterface code)
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
using System.Linq;
using Engine;

{DiligentTypeMapper.Usings}

namespace DiligentEngine
{{");
            WriteClassDefinitionAndConstructors(code, writer);

            //Public interface
            foreach (var item in code.Methods)
            {
                var cSharpReturnType = GetCSharpType(item.ReturnType, context);
                var isReturnTypeInterface = context.CodeTypeInfo.Interfaces.ContainsKey(item.LookupReturnType);
                var isReturnTypePooledInterface = isReturnTypeInterface && item.PoolManagedObject;

                if (item.Comment != null && item.Comment.Any())
                {
                    writer.WriteLine("        /// <summary>");
                    foreach(var comment in item.Comment)
                    {
                        writer.WriteLine($"        /// {comment}");
                    }
                    writer.WriteLine("        /// </summary>");
                }
                
                if (isReturnTypePooledInterface)
                {
                    writer.WriteLine(
@$"        private {cSharpReturnType} _{item.Name};");
                }

                if (item.ReturnAsAutoPtr)
                {
                    writer.Write($"        public AutoPtr<{cSharpReturnType}> {item.Name}(");
                }
                else
                {
                    writer.Write($"        public {cSharpReturnType} {item.Name}(");
                }

                var sep = "";

                foreach (var arg in item.Args.Where(i => !i.MakeReturnVal))
                {
                    Func<String, String> enumerableWrapper = s => s;
                    if (arg.IsArray)
                    {
                        enumerableWrapper = s => $"{s}[]";
                    }

                    writer.Write($"{sep}{enumerableWrapper(GetCSharpType(arg.Type, context))} {arg.Name}");
                    sep = ", ";
                }

                writer.WriteLine(")");
                writer.WriteLine("        {");

                if (isReturnTypePooledInterface)
                {
                    writer.WriteLine(
@$"            // Only create a new instance of the return type if this really changed.");
                }
                
                if(isReturnTypeInterface)
                {
                    writer.WriteLine(
@$"            var theReturnValue = ");
                }
                else if(item.LookupReturnType == "PVoid")
                {
                    writer.WriteLine(
@$"            return");
                }

                writer.WriteLine(
@$"            {code.Name}_{item.Name}(
                this.objPtr");

                foreach (var arg in item.Args.Where(i => !i.MakeReturnVal))
                {
                    if (context.CodeTypeInfo.Structs.TryGetValue(arg.LookupType, out var structInfo))
                    {
                        var argWriter = new StructCsFunctionCallArgsWriter(arg.Name, structInfo, "                ");
                        argWriter.Render(writer, context);
                    }
                    else if (context.CodeTypeInfo.Interfaces.TryGetValue(arg.LookupType, out var ifaceInfo))
                    {
                        if (arg.IsArray)
                        {
                            writer.WriteLine($"                , {arg.Name}.Select(i => i.objPtr).ToArray()");
                        }
                        else
                        {
                            writer.WriteLine($"                , {arg.Name}.objPtr");
                        }
                    }
                    else
                    {
                        writer.WriteLine($"                , {arg.Name}");
                    }
                }

                writer.WriteLine(
@$"            );");

                if (isReturnTypePooledInterface)
                {
                    writer.WriteLine(
@$"            if(_{item.Name} == null || theReturnValue != _{item.Name}.objPtr)
            {{
                _{item.Name} = theReturnValue == null ? null : new {cSharpReturnType}(theReturnValue);
            }}
            return _{item.Name};");
                }
                else if (isReturnTypeInterface)
                {
                    if (item.ReturnAsAutoPtr)
                    {
                        writer.WriteLine($"            return theReturnValue != IntPtr.Zero ? new AutoPtr<{cSharpReturnType}>(new {cSharpReturnType}(theReturnValue), {item.AddRefToAutoPtr.ToString().ToLowerInvariant()}) : null;");
                    }
                    else
                    {
                        writer.WriteLine($"            return theReturnValue != IntPtr.Zero ? new {cSharpReturnType}(theReturnValue) : null;");
                    }
                }

                writer.WriteLine("        }");
            }

            writer.WriteLine();
            writer.WriteLine();

            //PInvoke
            foreach (var item in code.Methods)
            {
                writer.WriteLine("        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]");
                if (TypeDetector.IsBool(item.LookupReturnType))
                {
                    writer.WriteLine("[return: MarshalAs(UnmanagedType.I1)]");
                }
                writer.WriteLine(
@$"        private static extern {GetPInvokeType(item, context)} {code.Name}_{item.Name}(
            IntPtr objPtr");

                foreach (var arg in item.Args.Where(i => !i.MakeReturnVal))
                {
                    if (context.CodeTypeInfo.Structs.TryGetValue(arg.LookupType, out var structInfo))
                    {
                        var argWriter = new StructCsFunctionSignatureArgsWriter(arg.Name, structInfo, "            ");
                        argWriter.Render(writer, context);
                    }
                    else
                    {
                        var attrs = "";
                        if (TypeDetector.IsBool(arg.LookupType))
                        {
                            attrs = "[MarshalAs(UnmanagedType.I1)]";
                        }

                        writer.WriteLine($"            , {attrs}{GetPInvokeType(arg, context)} {arg.Name}");
                    }
                }

                writer.WriteLine($"        );");
            }

            writer.WriteLine("    }");

            writer.WriteLine("}");
        }

        private void WriteClassDefinitionAndConstructors(CodeInterface code, TextWriter writer)
        {
            if (code.Comment != null && code.Comment.Any())
            {
                writer.WriteLine("    /// <summary>");
                foreach (var comment in code.Comment)
                {
                    writer.WriteLine($"    /// {comment}");
                }
                writer.WriteLine("    /// </summary>");
            }

            if (code.BaseType != null)
            {
                writer.WriteLine($"    public partial class {code.Name} : {code.BaseType}");

                writer.WriteLine(
$@"    {{
        public {code.Name}(IntPtr objPtr)
            : base(objPtr)
        {{
            this._ConstructorCalled();
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
            this._ConstructorCalled();
        }}");
            }

            writer.WriteLine("        partial void _ConstructorCalled();");
        }

        private static String GetCSharpType(String type, CodeRendererContext context)
        {
            switch (type)
            {
                case "char*":
                case "Char*":
                    return "String";
                case "void*":
                    return "IntPtr";
                case "size_t":
                    return "UIntPtr";
            }

            return type.Replace("*", "");
        }

        private static String GetPInvokeType(InterfaceMethodArgument arg, CodeRendererContext context)
        {
            if (context.CodeTypeInfo.Interfaces.ContainsKey(arg.LookupType))
            {
                var array = "";
                if (arg.IsArray)
                {
                    array = "[]";
                }

                return $"IntPtr{array}";
            }

            switch (arg.Type)
            {
                case "char*":
                case "Char*":
                    return "String";
                case "Color":
                    return "Color";
                case "void*":
                    return "IntPtr";
                case "size_t":
                    return "UIntPtr";
            }

            if (arg.IsArray)
            {
                return $"{arg.LookupType}[]";
            }

            return arg.Type;
        }

        private static String GetPInvokeType(InterfaceMethod method, CodeRendererContext context)
        {
            if (context.CodeTypeInfo.Interfaces.ContainsKey(method.LookupReturnType))
            {
                return "IntPtr";
            }

            switch (method.ReturnType)
            {
                case "char*":
                case "Char*":
                    return "String";
            }

            if (method.IsPtr)
            {
                return "IntPtr";
            }

            return method.ReturnType;
        }
    }
}
