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
            var baseCSharpOutDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory() + "../../../../../DiligentEngine"));
            var baseCPlusPlusOutDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory() + "../../../../../DiligentEngineWrapper"));

            //////////// Enums

            var baseEnumDir = Path.Combine(baseCSharpOutDir, "Enums");

            {
                var BUFFER_MODE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Buffer.h", 46, 71);
                EnumWriter.Write(BUFFER_MODE, Path.Combine(baseEnumDir, $"{nameof(BUFFER_MODE)}.cs"));
            }

            {
                var BIND_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 64, 90);
                foreach (var val in BIND_FLAGS.Properties)
                {
                    val.Value = val.Value.TrimEnd('L');
                }
                EnumWriter.Write(BIND_FLAGS, Path.Combine(baseEnumDir, $"{nameof(BIND_FLAGS)}.cs"));
            }

            {
                var USAGE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 93, 140);
                EnumWriter.Write(USAGE, Path.Combine(baseEnumDir, $"{nameof(USAGE)}.cs"));
            }

            {
                var CPU_ACCESS_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 142, 153);
                EnumWriter.Write(CPU_ACCESS_FLAGS, Path.Combine(baseEnumDir, $"{nameof(CPU_ACCESS_FLAGS)}.cs"));
            }

            {
                var SURFACE_TRANSFORM = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 1269, 1300);
                EnumWriter.Write(SURFACE_TRANSFORM, Path.Combine(baseEnumDir, $"{nameof(SURFACE_TRANSFORM)}.cs"));
            }

            //////////// Structs

            var baseStructDir = Path.Combine(baseCSharpOutDir, "Structs");
            var DeviceObjectAttribs = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 1150, 1159);
            StructCsWriter.Write(DeviceObjectAttribs, Path.Combine(baseStructDir, $"{nameof(DeviceObjectAttribs)}.cs"));

            var BufferDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Buffer.h", 72, 108);
            StructCsWriter.Write(BufferDesc, Path.Combine(baseStructDir, $"{nameof(BufferDesc)}.cs"));

            //////////// Interfaces
            var baseCSharpInterfaceDir = Path.Combine(baseCSharpOutDir, "Interfaces");

            {
                var IRenderDevice = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/RenderDevice.h", 72, 330);
                var allowedMethods = new List<String> { /*"CreateShader" */};
                IRenderDevice.Methods = IRenderDevice.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                InterfaceCsWriter.Write(IRenderDevice, Path.Combine(baseCSharpInterfaceDir, $"{nameof(IRenderDevice)}.cs"));
            }

            {
                var IDeviceContext = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceContext.h", 1366, 2203);
                var allowedMethods = new List<String> { "Flush" };
                IDeviceContext.Methods = IDeviceContext.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                InterfaceCsWriter.Write(IDeviceContext, Path.Combine(baseCSharpInterfaceDir, $"{nameof(IDeviceContext)}.cs"));
                InterfaceCppWriter.Write(IDeviceContext, Path.Combine(baseCPlusPlusOutDir, $"{nameof(IDeviceContext)}.cpp"), new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/DeviceContext.h",
                    "Color.h"
                });
            }

            {
                var ISwapChain = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/SwapChain.h", 54, 119);
                var allowedMethods = new List<String> { "Resize" };
                ISwapChain.Methods = ISwapChain.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                InterfaceCsWriter.Write(ISwapChain, Path.Combine(baseCSharpInterfaceDir, $"{nameof(ISwapChain)}.cs"));
                InterfaceCppWriter.Write(ISwapChain, Path.Combine(baseCPlusPlusOutDir, $"{nameof(ISwapChain)}.cpp"), new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/SwapChain.h"
                });
            }
        }
    }
}
