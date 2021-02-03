using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCsFunctionCallArgsWriter : ICodeRenderer
    {
        private readonly string argName;
        private CodeStruct code;
        private readonly string tabs;

        public StructCsFunctionCallArgsWriter(String argName, CodeStruct code, String tabs)
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
                var nestedWriter = new StructCsFunctionCallArgsWriter($"{argName}.{item.Name}", st, tabs);
                nestedWriter.Render(writer, context);
            }
            else
            {
                if (item.IsArray)
                {
                    var max = item.ArrayLenInt;
                    for (var i = 0; i < max; ++i)
                    {
                        WriteSimple(writer, context, item, $"_{i}");
                    }
                }
                else
                {
                    WriteSimple(writer, context, item, "");
                }
            }
        }

        private void WriteSimple(TextWriter writer, CodeRendererContext context, StructProperty item, String array)
        {

            if (context.CodeTypeInfo.Interfaces.TryGetValue(item.LookupType, out var iface))
            {
                writer.WriteLine($"{tabs}, {argName}.{item.Name}{array}?.objPtr ?? IntPtr.Zero");
            }
            else
            {
                writer.WriteLine($"{tabs}, {argName}.{item.Name}{array}");
            }
        }
    }
}
