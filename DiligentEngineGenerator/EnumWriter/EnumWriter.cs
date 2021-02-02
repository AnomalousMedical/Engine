using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator.EnumWriter
{
    static class EnumWriter
    {
        public static void Write(CodeEnum code, String file)
        {
            var dir = Path.GetDirectoryName(file);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using var writer = new StreamWriter(File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None));
            Write(code, writer);
        }

        public static void Write(CodeEnum code, StreamWriter writer)
        {
            writer.WriteLine(
@"using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;

namespace DiligentEngine
{");


            if (code.BaseType != null)
            {
                writer.WriteLine($"    public enum {code.Name} : {code.BaseType}");
            }
            else
            {
                writer.WriteLine($"    public enum {code.Name}");
            }

            writer.WriteLine("    {");

            foreach(var item in code.Properties)
            {
                if (item.Value != null)
                {
                    writer.WriteLine($"        {item.Name} = {item.Value},");
                }
                else
                {
                    writer.WriteLine($"        {item.Name},");
                }
            }

            writer.WriteLine("    }");

            writer.WriteLine("}");
        }
    }
}
