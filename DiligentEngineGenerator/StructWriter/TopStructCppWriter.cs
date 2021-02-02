using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class TopStructCppWriter : StructCppWriter
    {
        protected override void WriteCustomPInvoke(CodeStruct code, StreamWriter writer)
        {
            writer.Write(
$@"extern ""C"" _AnomalousExport {code.Name} * {code.Name}_Create()
{{
    return new {code.Name};
}}

extern ""C"" _AnomalousExport void {code.Name}_Delete({code.Name}* obj)
{{
    delete obj;
}}");
        }
    }
}
