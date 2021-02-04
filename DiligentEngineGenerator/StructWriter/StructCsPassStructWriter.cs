using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCsPassStructWriter : ICodeRenderer
    {
        private CodeStruct code;

        public StructCsPassStructWriter(CodeStruct code)
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

{DiligentTypeMapper.Usings}

namespace DiligentEngine
{{");

                writer.WriteLine(
@$"    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct {code.Name}PassStruct
    {{");

            foreach (var item in code.Properties.Where(i => i.TakeAutoSize == null))
            {
                WriteItem(writer, context, item);
            }

            writer.WriteLine(
$@"        public static {code.Name}PassStruct[] ToStruct(IEnumerable<{code.Name}> vals)
        {{
            if(vals == null)
            {{
                return null;
            }}

            return vals.Select(i => new {code.Name}PassStruct
            {{");

            foreach (var item in code.Properties.Where(i => i.TakeAutoSize == null))
            {
                WriteSet(writer, context, item);
            }

            writer.WriteLine(
$@"            }}).ToArray();
        }}");

            writer.WriteLine("    }");

            writer.WriteLine("}");
        }

        private static void WriteItem(TextWriter writer, CodeRendererContext context, StructProperty item, Func<String> arrayStringCb = null, Func<String, String> customizeTypeCb = null)
        {
            var cSharpType = GetCSharpType(item, context);
            cSharpType = customizeTypeCb?.Invoke(cSharpType) ?? cSharpType;

            writer.Write($"        public {cSharpType} {item.Name}{arrayStringCb?.Invoke()};");

            writer.WriteLine();
        }

        private static void WriteSet(TextWriter writer, CodeRendererContext context, StructProperty item, Func<String> arrayStringCb = null)
        {
            var data = ConvertCSharpData(item, context, $"i.{item.Name}{arrayStringCb?.Invoke()}");

            writer.Write($"                {item.Name}{arrayStringCb?.Invoke()} = {data},");

            writer.WriteLine();
        }

        private static String ConvertCSharpData(StructProperty item, CodeRendererContext context, String data)
        {
            switch (item.Type)
            {
                case "bool":
                case "Bool":
                    return $"Convert.ToUInt32({data})";
            }

            return data;
        }

        private static String GetCSharpType(StructProperty item, CodeRendererContext context)
        {
            if(context.CodeTypeInfo.Interfaces.ContainsKey(item.LookupType) || context.CodeTypeInfo.Structs.ContainsKey(item.LookupType))
            {
                return item.LookupType;
            }

            switch (item.Type)
            {
                case "char*":
                case "Char*":
                    return "String";
                case "void*":
                    return "IntPtr";
                case "bool":
                case "Bool":
                    return "Uint32";
            }

            return item.Type;
        }
    }
}
