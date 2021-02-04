using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCppRebuildWriter
    {
        private readonly string argName;
        private CodeStruct code;
        private readonly string tabs;

        public StructCppRebuildWriter(String argName, CodeStruct code, String tabs)
        {
            this.argName = argName;
            this.code = code;
            this.tabs = tabs;
        }

        public void Render(TextWriter writer, CodeRendererContext context, StructCppWriterContext structCppWriterContext)
        {
            writer.WriteLine($"{tabs}{code.Name} {argName};");

            foreach (var item in code.Properties)
            {
                HandleItem(writer, context, item, structCppWriterContext);
            }

            var current = this.code;
            while (current.BaseType != null && context.CodeTypeInfo.Structs.TryGetValue(current.BaseType, out current))
            {
                foreach (var item in current.Properties)
                {
                    HandleItem(writer, context, item, structCppWriterContext);
                }
            }
        }

        private void HandleItem(TextWriter writer, CodeRendererContext context, StructProperty item, StructCppWriterContext structCppWriterContext)
        {
            if (context.CodeTypeInfo.Structs.TryGetValue(item.LookupType, out var st))
            {
                var nestedWriter = new StructCppRebuildNestedWriter($"{argName}.{item.Name}", $"{argName}_{item.Name}", st, tabs);
                nestedWriter.Render(writer, context, structCppWriterContext);
            }
            else
            {
                if (item.IsArray)
                {
                    var len = item.ArrayLenInt;
                    for (var i = 0; i < len; ++i)
                    {
                        WriteSimple(writer, item, $"[{i}]", $"_{i}");
                    }
                }
                else
                {
                    WriteSimple(writer, item, "", "");
                }
            }
        }

        private void WriteSimple(TextWriter writer, StructProperty item, String arrayIndex, String arrayItem)
        {
            writer.WriteLine($"{tabs}{argName}.{item.Name}{arrayIndex} = {argName}_{item.Name}{arrayItem};");
        }
    }
}
