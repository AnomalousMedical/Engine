using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCppFunctionSignatureArgsWriter : ICodeRenderer
    {
        private readonly string argName;
        private CodeStruct code;
        private readonly string tabs;

        public StructCppFunctionSignatureArgsWriter(String argName, CodeStruct code, String tabs)
        {
            this.argName = argName;
            this.code = code;
            this.tabs = tabs;
        }

        public void Render(TextWriter writer, CodeRendererContext context)
        {
            foreach (var item in code.Properties)
            {
                HandleItem(writer, context, item);
            }

            var current = this.code;
            while (current.BaseType != null && context.CodeTypeInfo.Structs.TryGetValue(current.BaseType, out current))
            {
                foreach (var item in current.Properties)
                {
                    HandleItem(writer, context, item);
                }
            }
        }

        private void HandleItem(TextWriter writer, CodeRendererContext context, StructProperty item)
        {
            if (context.CodeTypeInfo.Structs.TryGetValue(item.LookupType, out var st))
            {
                if (item.IsArray)
                {
                    if (item.IsUnknownSizeArray)
                    {
                        writer.WriteLine($"{tabs}, {item.LookupType}PassStruct* {argName}_{item.Name}");
                    }
                    else
                    {
                        writer.WriteLine($"{tabs}, {item.LookupType}PassStruct* {argName}_{item.Name}");
                    }
                }
                else
                {
                    var nestedWriter = new StructCppFunctionSignatureArgsWriter($"{argName}_{item.Name}", st, tabs);
                    nestedWriter.Render(writer, context);
                }
            }
            else
            {
                if (item.IsArray)
                {
                    var len = item.ArrayLenInt;
                    for (var i = 0; i < len; ++i)
                    {
                        WriteSimple(writer, item, $"_{i}");
                    }
                }
                else
                {
                    WriteSimple(writer, item, "");
                }
            }
        }

        private void WriteSimple(TextWriter writer, StructProperty item, String array)
        {
            writer.WriteLine($"{tabs}, {item.Type} {argName}_{item.Name}{array}");
        }
    }
}
