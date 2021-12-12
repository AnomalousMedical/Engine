using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCppRebuildWriter
    {
        private readonly string setName;
        private readonly string argName;
        private CodeStruct code;
        private readonly string tabs;

        public StructCppRebuildWriter(String setName, String argName, CodeStruct code, String tabs)
        {
            this.setName = setName;
            this.argName = argName;
            this.code = code;
            this.tabs = tabs;
        }

        public void Render(TextWriter writer, CodeRendererContext context, StructCppWriterContext structCppWriterContext)
        {
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
                if (item.IsArray)
                {
                    if (item.IsUnknownSizeArray)
                    {
                        var nativeArrayName = $"{argName}_{item.Name}_Native_Array";

                        structCppWriterContext.DeleteStatements.Add($"delete[] {nativeArrayName};");

                        writer.WriteLine(
@$"{tabs}{item.LookupType}* {nativeArrayName} = new {item.LookupType}[{argName}_{item.PutAutoSize}];
{tabs}if({argName}_{item.PutAutoSize} > 0)
{tabs}{{
{tabs}{tabs}for (Uint32 i = 0; i < {argName}_{item.PutAutoSize}; ++i)
{tabs}{tabs}{{");

                        foreach(var nestedProp in st.Properties)
                        {
                            WriteNestedProperty(writer, context, nestedProp, item, nativeArrayName);
                        }

                        writer.WriteLine(
@$"{tabs}{tabs}}}
{tabs}{tabs}{setName}.{item.Name} = {nativeArrayName};  
{tabs}}}");
                    }
                    else
                    {
                        var nativeArrayName = $"{setName}.{item.Name}";

                        writer.WriteLine(
@$"{tabs}for (Uint32 i = 0; i < {item.ArrayLenInt}; ++i)
{tabs}{{");

                        foreach (var nestedProp in st.Properties)
                        {
                            WriteNestedProperty(writer, context, nestedProp, item, nativeArrayName);
                        }

                        writer.WriteLine(
@$"{tabs}}}");
                    }
                }
                else
                {
                    var nestedWriter = new StructCppRebuildWriter($"{setName}.{item.Name}", $"{argName}_{item.Name}", st, tabs);
                    nestedWriter.Render(writer, context, structCppWriterContext);
                }
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

        private void WriteNestedProperty(TextWriter writer, CodeRendererContext context, StructProperty nestedProp, StructProperty item, string nativeArrayName,
            Func<String, String> customizeName = null,
            Func<String, String> customizeNestedPropName = null,
            Func<String, String> customizeNestedGetPropName = null)
        {
            var name = customizeName?.Invoke(item.Name) ?? item.Name;
            var nestedPropSetName = customizeNestedPropName?.Invoke(nestedProp.Name) ?? nestedProp.Name;
            var nestedPropGetName = customizeNestedGetPropName?.Invoke(nestedProp.Name) ?? nestedPropSetName;

            if (nestedProp.PullPropertiesIntoStruct && context.CodeTypeInfo.Structs.TryGetValue(nestedProp.LookupType, out var stlookup))
            {
                foreach (var pullItem in stlookup.Properties)
                {
                    WriteNestedProperty(writer, context, pullItem, item, nativeArrayName, 
                        customizeName: s => s, 
                        customizeNestedPropName: s => $"{nestedPropSetName}.{s}",
                        customizeNestedGetPropName: s => $"{nestedProp.Name}_{s}");
                }
            }
            else
            {
                if (nestedProp.IsArray && !nestedProp.IsUnknownSizeArray)
                {
                    for (var i = 0; i < nestedProp.ArrayLenInt; ++i)
                    {
                        writer.WriteLine($"{tabs}    {nativeArrayName}[i].{nestedPropSetName}[{i}] = {argName}_{name}[i].{nestedPropGetName}[{i}];");
                    }
                }
                else
                {
                    writer.WriteLine($"{tabs}    {nativeArrayName}[i].{nestedPropSetName} = {argName}_{name}[i].{nestedPropGetName};");
                }
            }
        }

        private void WriteSimple(TextWriter writer, StructProperty item, String arrayIndex, String arrayItem)
        {
            writer.WriteLine($"{tabs}{setName}.{item.Name}{arrayIndex} = {argName}_{item.Name}{arrayItem};");
        }
    }
}
