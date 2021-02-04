using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCsFunctionSignatureArgsWriter : ICodeRenderer
    {
        private readonly string argName;
        private CodeStruct code;
        private readonly string tabs;

        public StructCsFunctionSignatureArgsWriter(String argName, CodeStruct code, String tabs)
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
                var nestedWriter = new StructCsFunctionSignatureArgsWriter($"{argName}_{item.Name}", st, tabs);
                nestedWriter.Render(writer, context);
            }
            else
            {
                if (item.IsArray)
                {
                    var len = item.ArrayLenInt;
                    for(int i = 0; i < len; ++i)
                    {
                        WriteBasicItem(writer, context, item, $"_{i}");
                    }
                }
                else
                {
                    WriteBasicItem(writer, context, item, "");
                }
            }
        }

        private void WriteBasicItem(TextWriter writer, CodeRendererContext context, StructProperty item, String arrayIndex)
        {
            var attrs = "";
            if (TypeDetector.IsBool(item.LookupType))
            {
                attrs = "[MarshalAs(UnmanagedType.I1)]";
            }

            writer.WriteLine($"{tabs}, {attrs}{GetCSharpType(item, context)} {argName}_{item.Name}{arrayIndex}");
        }

        private static String GetCSharpType(StructProperty item, CodeRendererContext context)
        {
            if (context.CodeTypeInfo.Interfaces.ContainsKey(item.LookupType))
            {
                return "IntPtr";
            }

            switch (item.Type)
            {
                case "Char*":
                    return "String";
                case "void*":
                    return "IntPtr";
            }

            return item.Type;
        }
    }
}
