using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCppRebuildWriter : ICodeRenderer
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

        public void Render(TextWriter writer, CodeRendererContext context)
        {
            writer.WriteLine($"{tabs}{code.Name} {argName};");

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
                var nestedWriter = new StructCppRebuildNestedWriter($"{argName}.{item.Name}", $"{argName}_{item.Name}", st, tabs);
                nestedWriter.Render(writer, context);
            }
            else
            {
                writer.WriteLine($"{tabs}{argName}.{item.Name} = {argName}_{item.Name};");
            }
        }
    }
}
