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
                if (item.IsArray)
                {
                    if (item.IsUnknownSizeArray)
                    {
                        writer.WriteLine($"{tabs}, {item.LookupType}PassStruct.ToStruct({argName}?.{item.Name})");
                    }
                    else
                    {
                        writer.WriteLine($"{tabs}, {item.LookupType}PassStruct.ToStruct({argName}?.{item.Name})");
                    }
                }
                else
                {
                    var nestedWriter = new StructCsFunctionCallArgsWriter($"{argName}.{item.Name}", st, tabs);
                    nestedWriter.Render(writer, context);
                }
            }
            else
            {
                if (item.IsArray && !item.IsUnknownSizeArray) //Yes, pass through is wanted here for unknown size since those can just pass as is here
                {
                    var max = item.ArrayLenInt;
                    for (var i = 0; i < max; ++i)
                    {
                        WriteSimple(writer, context, item, array:() => $"_{i}");
                    }
                }
                else
                {
                    WriteSimple(writer, context, item);
                }
            }
        }

        private void WriteSimple(TextWriter writer, CodeRendererContext context, StructProperty item, Func<String> array = null)
        {

            if (context.CodeTypeInfo.Interfaces.TryGetValue(item.LookupType, out var iface))
            {
                writer.WriteLine($"{tabs}, {argName}.{item.Name}{array?.Invoke()}?.objPtr ?? IntPtr.Zero");
            }
            else if (!String.IsNullOrEmpty(item.TakeAutoSize))
            {
                writer.WriteLine($"{tabs}, {argName}?.{item.TakeAutoSize} != null ? (Uint32){argName}.{item.TakeAutoSize}.Count : 0");
            }
            else
            {
                writer.WriteLine($"{tabs}, {argName}.{item.Name}{array?.Invoke()}");
            }
        }
    }
}
