using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCsWriter : ICodeRenderer
    {
        private CodeStruct code;

        public StructCsWriter(CodeStruct code)
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

{DiligentTypeMapper.Usings}

namespace DiligentEngine
{{");

            if (code.BaseType != null)
            {
                writer.WriteLine($"    public partial class {code.Name} : {code.BaseType}");

                writer.WriteLine(
$@"    {{
        public {code.Name}()
        {{

        }}");
            }
            else
            {
                writer.WriteLine($"    public partial class {code.Name}");

                writer.WriteLine(
$@"    {{

        public {code.Name}()
        {{
            
        }}");
            }

            foreach (var item in code.Properties.Where(i => i.TakeAutoSize == null))
            {
                if (item.IsArray)
                {
                    if (item.IsUnknownSizeArray)
                    {
                        WriteItem(writer, context, item, writeDefaultValue: false, customizeTypeCb: s=> $"List<{s}>");
                    }
                    else
                    {
                        var len = item.ArrayLenInt;
                        for (var i = 0; i < len; ++i)
                        {
                            WriteItem(writer, context, item, arrayStringCb: () => $"_{i}");
                        }
                    }
                }
                else
                {
                    WriteItem(writer, context, item);
                }
            }

            //PInvoke
            writer.WriteLine();
            writer.WriteLine();

            writer.WriteLine("    }");

            writer.WriteLine("}");
        }

        private static void WriteItem(TextWriter writer, CodeRendererContext context, StructProperty item, Func<String> arrayStringCb = null, bool writeDefaultValue = true, Func<String, String> customizeTypeCb = null)
        {
            var cSharpType = GetCSharpType(item, context);
            cSharpType = customizeTypeCb?.Invoke(cSharpType) ?? cSharpType;

            writer.Write($"        public {cSharpType} {item.Name}{arrayStringCb?.Invoke()} {{ get; set; }}");

            if (writeDefaultValue)
            {
                if (context.CodeTypeInfo.Structs.ContainsKey(item.LookupType))
                {
                    if (item.DefaultValue != "nullptr") 
                    { 
                        writer.Write($" = new {item.Type}();");
                    }
                }
                else if (!String.IsNullOrWhiteSpace(item.DefaultValue))
                {
                    var value = GetCSharpValue(item, context);
                    if (value != null)
                    {
                        writer.Write($" = {value};");
                    }
                }
            }

            writer.WriteLine();
        }

        /// <summary>
        /// Get the c# value. Returns a string with the value or null to write nothing.
        /// </summary>
        /// <param name="structProperty"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static String GetCSharpValue(StructProperty structProperty, CodeRendererContext context)
        {
            switch (structProperty.DefaultValue)
            {
                case "nullptr":
                    return null; //Let the language implied null work
                case "True":
                    return "true";
                case "False":
                    return "false";
            }

            if (TypeDetector.IsBool(structProperty.Type))
            {
                if(long.TryParse(structProperty.DefaultValue, out long value))
                {
                    if(value > 0)
                    {
                        return "true";
                    }
                    else
                    {
                        return "false";
                    }
                }
            }

            if (context.CodeTypeInfo.Enums.ContainsKey(structProperty.LookupType))
            {
                return $"{structProperty.LookupType}.{structProperty.DefaultValue}";
            }

            return structProperty.DefaultValue;
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
            }

            return item.Type;
        }
    }
}
