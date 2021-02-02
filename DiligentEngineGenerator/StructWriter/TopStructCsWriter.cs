using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiligentEngineGenerator
{
    class TopStructCsWriter : StructCsWriter
    {
        protected override void WriteClassNameAndConstructor(CodeStruct code, StreamWriter writer)
        {
            if (code.BaseType != null)
            {
                writer.WriteLine($"    public partial class {code.Name} : {code.BaseType}");

                writer.WriteLine(
$@"    {{
        public {code.Name}(IntPtr objPtr)
            : base(objPtr)
        {{

        }}");
            }
            else
            {
                writer.WriteLine($"    public partial class {code.Name} : IDisposable");

                writer.WriteLine(
$@"    {{
        internal protected IntPtr objPtr;

        public IntPtr ObjPtr => objPtr;

        public {code.Name}()
        {{
            this.objPtr = {code.Name}_Create();
        }}

        public void Dispose()
        {{
            {code.Name}_Delete(this.objPtr);
        }}");
            }
        }

        protected override void CustomPInvoke(CodeStruct code, StreamWriter writer)
        {
            base.CustomPInvoke(code, writer);

            writer.WriteLine(
$@"        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr {code.Name}_Create();

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void {code.Name}_Delete(IntPtr obj);");
        }
    }
}
