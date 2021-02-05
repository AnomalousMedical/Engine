using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class InterfaceCppWriter : ICodeRenderer
    {
        private readonly CodeInterface code;
        private readonly IEnumerable<string> includeDirs;

        public InterfaceCppWriter(CodeInterface code, IEnumerable<String> includeDirs)
        {
            this.code = code;
            this.includeDirs = includeDirs;
        }

        public void Render(TextWriter writer, CodeRendererContext context)
        {
            writer.WriteLine("#include \"StdAfx.h\"");
            foreach(var inc in includeDirs)
            {
                writer.WriteLine($"#include \"{inc}\"");
            }
            writer.WriteLine("using namespace Diligent;");

            //PInvoke
            foreach (var item in code.Methods)
            {
                writer.WriteLine(
@$"extern ""C"" _AnomalousExport {item.ReturnType} {code.Name}_{item.Name}(
	{code.Name}* objPtr");

                foreach (var arg in item.Args.Where(i => !i.MakeReturnVal))
                {
                    if (context.CodeTypeInfo.Structs.TryGetValue(arg.LookupType, out var structInfo))
                    {
                        var argWriter = new StructCppFunctionSignatureArgsWriter(arg.Name, structInfo, "	");
                        argWriter.Render(writer, context);
                    }
                    else
                    {
                        writer.Write($", {arg.Type} {arg.Name}");
                    }
                }

                var makeReturnArg = item.Args.Where(i => i.MakeReturnVal).FirstOrDefault();

                writer.WriteLine($")");
                writer.WriteLine("{");

                var structCppWriterContext = new StructCppWriterContext();

                //Rebuild structs
                foreach (var arg in item.Args.Where(i => !i.MakeReturnVal))
                {
                    if (context.CodeTypeInfo.Structs.TryGetValue(arg.LookupType, out var structInfo))
                    {
                        var argWriter = new StructCppRebuildWriter(arg.Name, arg.Name, structInfo, "	");
                        writer.WriteLine($"	{structInfo.Name} {arg.Name};");
                        argWriter.Render(writer, context, structCppWriterContext);
                    }
                }

                bool hasReturnValue = false;
                //Write function
                if (item.ReturnType != "void")
                {
                    if(makeReturnArg != null)
                    {
                        if (makeReturnArg.IsRef)
                        {
                            writer.WriteLine($"	{makeReturnArg.Type} theReturnValue;");
                        }
                        else
                        {
                            writer.WriteLine($"	{makeReturnArg.Type} theReturnValue = nullptr;");
                        }
                        writer.WriteLine($"	objPtr->{item.Name}(");
                        hasReturnValue = true;
                    }
                    else
                    {
                        writer.WriteLine($"	{item.ReturnType} theReturnValue = objPtr->{item.Name}(");
                        hasReturnValue = true;
                    }
                }
                else
                {
                    writer.WriteLine($"	objPtr->{item.Name}(");
                }
                var sep = "		";
                foreach (var arg in item.Args)
                {
                    if (arg.MakeReturnVal)
                    {
                        var refStr = "&";
                        if (arg.IsRef)
                        {
                            refStr = ""; //Yes this is removing the deference, this means the func takes a ref not a ptr to ptr
                        }

                        writer.WriteLine($"{sep}{refStr}theReturnValue");
                    }
                    else
                    {
                        writer.WriteLine($"{sep}{arg.CppPrefix}{arg.Name}");
                    }
                    sep = "		, ";
                }
                writer.WriteLine("	);");

                foreach(var del in structCppWriterContext.DeleteStatements)
                {
                    writer.WriteLine($"    {del}");
                }

                //Add a ref to any interface pointers to help simulate the autopointer using dispose
                if (makeReturnArg != null)
                {
                    writer.WriteLine("    theReturnValue->AddRef();");
                }

                if (hasReturnValue)
                {
                    writer.WriteLine($"    return theReturnValue;");
                }

                writer.WriteLine("}");
            }
        }
    }
}
