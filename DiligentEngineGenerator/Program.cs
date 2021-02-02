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

            {
                var RESOURCE_STATE_TRANSITION_MODE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceContext.h", 133, 164);
                EnumWriter.Write(RESOURCE_STATE_TRANSITION_MODE, Path.Combine(baseEnumDir, $"{nameof(RESOURCE_STATE_TRANSITION_MODE)}.cs"));
            }

            {
                var CLEAR_DEPTH_STENCIL_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceContext.h", 438, 450);
                EnumWriter.Write(CLEAR_DEPTH_STENCIL_FLAGS, Path.Combine(baseEnumDir, $"{nameof(CLEAR_DEPTH_STENCIL_FLAGS)}.cs"));
            }




            //////////// Structs

            var baseStructDir = Path.Combine(baseCSharpOutDir, "Structs");
            {
                var DeviceObjectAttribs = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 1150, 1159);
                StructCsWriter.Write(DeviceObjectAttribs, Path.Combine(baseStructDir, $"{nameof(DeviceObjectAttribs)}.cs"));
            }

            {
                var BufferDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Buffer.h", 72, 108);
                StructCsWriter.Write(BufferDesc, Path.Combine(baseStructDir, $"{nameof(BufferDesc)}.cs"));
            }

            {
                var ShaderCreateInfo = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Shader.h", 223, 331);
                var allowed = new List<String> { /*"CreateShader" */};
                ShaderCreateInfo.Properties = ShaderCreateInfo.Properties
                    .Where(i => allowed.Contains(i.Name)).ToList();
                StructCsWriter.Write(ShaderCreateInfo, Path.Combine(baseStructDir, $"{nameof(ShaderCreateInfo)}.cs"), new TopStructCsWriter());
                StructCppWriter.Write(ShaderCreateInfo, Path.Combine(baseCPlusPlusOutDir, $"{nameof(ShaderCreateInfo)}.cpp"), new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/DeviceContext.h",
                    "Color.h"
                }, new TopStructCppWriter());
            }

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
                var allowedMethods = new List<String> { "Flush", /*"SetRenderTargets", */ "ClearRenderTarget", "ClearDepthStencil" };
                IDeviceContext.Methods = IDeviceContext.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                var rgbaArgs = IDeviceContext.Methods.First(i => i.Name == "ClearRenderTarget")
                    .Args.First(i => i.Name == "RGBA");
                rgbaArgs.Type = "Color";
                rgbaArgs.CppPrefix = "(float*)&";
                InterfaceCsWriter.Write(IDeviceContext, Path.Combine(baseCSharpInterfaceDir, $"{nameof(IDeviceContext)}.cs"));
                InterfaceCppWriter.Write(IDeviceContext, Path.Combine(baseCPlusPlusOutDir, $"{nameof(IDeviceContext)}.cpp"), new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/DeviceContext.h",
                    "Color.h"
                });
            }

            {
                var IDeviceObject = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceObject.h", 50, 96);
                var allowedMethods = new List<String> { "Resize" };
                IDeviceObject.Methods = IDeviceObject.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                InterfaceCsWriter.Write(IDeviceObject, Path.Combine(baseCSharpInterfaceDir, $"{nameof(IDeviceObject)}.cs"));
                InterfaceCppWriter.Write(IDeviceObject, Path.Combine(baseCPlusPlusOutDir, $"{nameof(IDeviceObject)}.cpp"), new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/SwapChain.h"
                });
            }

            {
                var ISwapChain = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/SwapChain.h", 54, 119);
                var allowedMethods = new List<String> { "Resize", "GetCurrentBackBufferRTV", "GetDepthBufferDSV", "Present" };
                ISwapChain.Methods = ISwapChain.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                InterfaceCsWriter.Write(ISwapChain, Path.Combine(baseCSharpInterfaceDir, $"{nameof(ISwapChain)}.cs"));
                InterfaceCppWriter.Write(ISwapChain, Path.Combine(baseCPlusPlusOutDir, $"{nameof(ISwapChain)}.cpp"), new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/SwapChain.h"
                });
            }

            {
                var ITextureView = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/TextureView.h", 195, 227);
                var allowedMethods = new List<String> { };
                ITextureView.Methods = ITextureView.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                InterfaceCsWriter.Write(ITextureView, Path.Combine(baseCSharpInterfaceDir, $"{nameof(ITextureView)}.cs"));
                InterfaceCppWriter.Write(ITextureView, Path.Combine(baseCPlusPlusOutDir, $"{nameof(ITextureView)}.cpp"), new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/SwapChain.h"
                });
            }
        }
    }
}
