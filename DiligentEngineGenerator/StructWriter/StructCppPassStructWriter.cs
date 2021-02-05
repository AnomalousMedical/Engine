using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCppPassStructWriter : ICodeRenderer
    {
        private CodeStruct code;

        public StructCppPassStructWriter(CodeStruct code)
        {
            this.code = code;
        }

        public void Render(TextWriter writer, CodeRendererContext context)
        {
            writer.WriteLine(
$@"#pragma once
#include ""Primitives/interface/BasicTypes.h""
#include ""Graphics/GraphicsEngine/interface/GraphicsTypes.h""
");

                writer.WriteLine(
@$"namespace Diligent 
{{
struct {code.Name}PassStruct
{{");

            foreach (var item in code.Properties.Where(i => i.TakeAutoSize == null))
            {
                WriteItem(writer, context, item);
            }
            writer.WriteLine("};");
            writer.Write("}");
        }

        private static void WriteItem(TextWriter writer, CodeRendererContext context, StructProperty item, Func<String> arrayStringCb = null)
        {
            var type = GetNativeType(item, context);

            var typePtr = "";
            if (context.CodeTypeInfo.Interfaces.ContainsKey(item.LookupType))
            {
                typePtr = "*";
            }

            writer.Write($"        {type}{typePtr} {item.Name}{arrayStringCb?.Invoke()};");

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

        private static String GetNativeType(StructProperty item, CodeRendererContext context)
        {
            if(context.CodeTypeInfo.Interfaces.ContainsKey(item.LookupType) || context.CodeTypeInfo.Structs.ContainsKey(item.LookupType))
            {
                return item.LookupType;
            }

            switch (item.Type)
            {
                case "bool":
                case "Bool":
                    return "Uint32";
            }

            return item.Type;
        }
    }
}
