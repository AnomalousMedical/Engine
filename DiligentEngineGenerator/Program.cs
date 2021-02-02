using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiligentEngineGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseDir = "C:/Anomalous/DiligentEngine";
            var baseOutDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory() + "../../../../../DiligentEngine"));

            //////////// Enums

            var baseEnumDir = Path.Combine(baseOutDir, "Enums");
            var BUFFER_MODE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Buffer.h", 46, 71);
            EnumWriter.Write(BUFFER_MODE, Path.Combine(baseEnumDir, $"{nameof(BUFFER_MODE)}.cs"));

            var BIND_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 64, 90);
            foreach(var val in BIND_FLAGS.Properties)
            {
                val.Value = val.Value.TrimEnd('L');
            }
            EnumWriter.Write(BIND_FLAGS, Path.Combine(baseEnumDir, $"{nameof(BIND_FLAGS)}.cs"));

            var USAGE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 93, 140);
            EnumWriter.Write(USAGE, Path.Combine(baseEnumDir, $"{nameof(USAGE)}.cs"));

            var CPU_ACCESS_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 142, 153);
            EnumWriter.Write(CPU_ACCESS_FLAGS, Path.Combine(baseEnumDir, $"{nameof(CPU_ACCESS_FLAGS)}.cs"));

            //////////// Structs

            var baseStructDir = Path.Combine(baseOutDir, "Structs");
            var DeviceObjectAttribs = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 1150, 1159);
            StructCsWriter.Write(DeviceObjectAttribs, Path.Combine(baseStructDir, $"{nameof(DeviceObjectAttribs)}.cs"));

            var BufferDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Buffer.h", 72, 108);
            StructCsWriter.Write(BufferDesc, Path.Combine(baseStructDir, $"{nameof(BufferDesc)}.cs"));

            //////////// Interfaces
            var baseInterfaceDir = Path.Combine(baseOutDir, "Interfaces");

            var IRenderDevice = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/RenderDevice.h", 72, 330);
            var allowedMethods = new List<String> { "CreateShader" };
            IRenderDevice.Methods = IRenderDevice.Methods
                .Where(i => false)
                .Where(i => allowedMethods.Contains(i.Name)).ToList();
            InterfaceCsWriter.Write(IRenderDevice, Path.Combine(baseInterfaceDir, $"{nameof(IRenderDevice)}.cs"));
        }
    }
}
