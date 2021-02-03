using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCsFunctionSignatureArgsWriter : ICodeRenderer
    {
        private CodeStruct code;

        public StructCsFunctionSignatureArgsWriter(CodeStruct code)
        {
            this.code = code;
        }

        public void Render(TextWriter writer, CodeRendererContext context)
        {
            foreach (var item in code.Properties)
            {
                writer.WriteLine($", {GetCSharpType(item.Type)} {code.Name}_{item.Name}");
            }
        }

        private static String GetCSharpType(String type)
        {
            switch (type)
            {
                case "Char*":
                    return "String";
            }

            return type;
        }

        private static String GetPInvokeType(StructProperty arg)
        {
            switch (arg.Type)
            {
                case "Char*":
                    return "String";
            }

            //if (arg.IsPtr)
            //{
            //    return "IntPtr";
            //}

            return arg.Type;
        }
    }
}
