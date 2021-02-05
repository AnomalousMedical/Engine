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
            var codeTypeInfo = new CodeTypeInfo();
            var codeWriter = new CodeWriter();

            //////////// Enums

            var baseEnumDir = Path.Combine(baseCSharpOutDir, "Enums");

            {
                var BUFFER_MODE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Buffer.h", 46, 71);
                codeTypeInfo.Enums[nameof(BUFFER_MODE)] = BUFFER_MODE;
                EnumWriter.Write(BUFFER_MODE, Path.Combine(baseEnumDir, $"{nameof(BUFFER_MODE)}.cs"));
            }

            {
                var BIND_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 64, 90);
                codeTypeInfo.Enums[nameof(BIND_FLAGS)] = BIND_FLAGS;
                foreach (var val in BIND_FLAGS.Properties)
                {
                    val.Value = val.Value.TrimEnd('L');
                }
                EnumWriter.Write(BIND_FLAGS, Path.Combine(baseEnumDir, $"{nameof(BIND_FLAGS)}.cs"));
            }

            {
                var USAGE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 93, 140);
                codeTypeInfo.Enums[nameof(USAGE)] = USAGE;
                EnumWriter.Write(USAGE, Path.Combine(baseEnumDir, $"{nameof(USAGE)}.cs"));
            }

            {
                var CPU_ACCESS_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 142, 153);
                codeTypeInfo.Enums[nameof(CPU_ACCESS_FLAGS)] = CPU_ACCESS_FLAGS;
                EnumWriter.Write(CPU_ACCESS_FLAGS, Path.Combine(baseEnumDir, $"{nameof(CPU_ACCESS_FLAGS)}.cs"));
            }

            {
                var SURFACE_TRANSFORM = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 1269, 1300);
                codeTypeInfo.Enums[nameof(SURFACE_TRANSFORM)] = SURFACE_TRANSFORM;
                EnumWriter.Write(SURFACE_TRANSFORM, Path.Combine(baseEnumDir, $"{nameof(SURFACE_TRANSFORM)}.cs"));
            }

            {
                var RESOURCE_STATE_TRANSITION_MODE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceContext.h", 133, 164);
                codeTypeInfo.Enums[nameof(RESOURCE_STATE_TRANSITION_MODE)] = RESOURCE_STATE_TRANSITION_MODE;
                EnumWriter.Write(RESOURCE_STATE_TRANSITION_MODE, Path.Combine(baseEnumDir, $"{nameof(RESOURCE_STATE_TRANSITION_MODE)}.cs"));
            }

            {
                var CLEAR_DEPTH_STENCIL_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceContext.h", 438, 450);
                codeTypeInfo.Enums[nameof(CLEAR_DEPTH_STENCIL_FLAGS)] = CLEAR_DEPTH_STENCIL_FLAGS;
                EnumWriter.Write(CLEAR_DEPTH_STENCIL_FLAGS, Path.Combine(baseEnumDir, $"{nameof(CLEAR_DEPTH_STENCIL_FLAGS)}.cs"));
            }

            {
                var SHADER_TYPE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Shader.h", 44, 64);
                codeTypeInfo.Enums[nameof(SHADER_TYPE)] = SHADER_TYPE;
                EnumWriter.Write(SHADER_TYPE, Path.Combine(baseEnumDir, $"{nameof(SHADER_TYPE)}.cs"));
            }

            {
                var SHADER_SOURCE_LANGUAGE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Shader.h", 67, 92);
                codeTypeInfo.Enums[nameof(SHADER_SOURCE_LANGUAGE)] = SHADER_SOURCE_LANGUAGE;
                EnumWriter.Write(SHADER_SOURCE_LANGUAGE, Path.Combine(baseEnumDir, $"{nameof(SHADER_SOURCE_LANGUAGE)}.cs"));
            }

            {
                var PRIMITIVE_TOPOLOGY = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 943, 1100);
                codeTypeInfo.Enums[nameof(PRIMITIVE_TOPOLOGY)] = PRIMITIVE_TOPOLOGY;
                EnumWriter.Write(PRIMITIVE_TOPOLOGY, Path.Combine(baseEnumDir, $"{nameof(PRIMITIVE_TOPOLOGY)}.cs"));
            }

            {
                var TEXTURE_FORMAT = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 277, 814);
                codeTypeInfo.Enums[nameof(TEXTURE_FORMAT)] = TEXTURE_FORMAT;
                EnumWriter.Write(TEXTURE_FORMAT, Path.Combine(baseEnumDir, $"{nameof(TEXTURE_FORMAT)}.cs"));
            }

            {
                var BLEND_FACTOR = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/BlendState.h", 42, 126);
                codeTypeInfo.Enums[nameof(BLEND_FACTOR)] = BLEND_FACTOR;
                EnumWriter.Write(BLEND_FACTOR, Path.Combine(baseEnumDir, $"{nameof(BLEND_FACTOR)}.cs"));
            }

            {
                var BLEND_OPERATION = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/BlendState.h", 130, 165);
                codeTypeInfo.Enums[nameof(BLEND_OPERATION)] = BLEND_OPERATION;
                EnumWriter.Write(BLEND_OPERATION, Path.Combine(baseEnumDir, $"{nameof(BLEND_OPERATION)}.cs"));
            }

            {
                var COLOR_MASK = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/BlendState.h", 169, 191);
                codeTypeInfo.Enums[nameof(COLOR_MASK)] = COLOR_MASK;
                EnumWriter.Write(COLOR_MASK, Path.Combine(baseEnumDir, $"{nameof(COLOR_MASK)}.cs"));
            }

            {
                var LOGIC_OPERATION = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/BlendState.h", 195, 269);
                codeTypeInfo.Enums[nameof(LOGIC_OPERATION)] = LOGIC_OPERATION;
                EnumWriter.Write(LOGIC_OPERATION, Path.Combine(baseEnumDir, $"{nameof(LOGIC_OPERATION)}.cs"));
            }

            {
                var COMPARISON_FUNCTION = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 880, 926);
                codeTypeInfo.Enums[nameof(COMPARISON_FUNCTION)] = COMPARISON_FUNCTION;
                EnumWriter.Write(COMPARISON_FUNCTION, Path.Combine(baseEnumDir, $"{nameof(COMPARISON_FUNCTION)}.cs"));
            }

            {
                var STENCIL_OP = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DepthStencilState.h", 41, 89);
                codeTypeInfo.Enums[nameof(STENCIL_OP)] = STENCIL_OP;
                EnumWriter.Write(STENCIL_OP, Path.Combine(baseEnumDir, $"{nameof(STENCIL_OP)}.cs"));
            }

            {
                var PSO_CREATE_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/PipelineState.h", 365, 388);
                codeTypeInfo.Enums[nameof(PSO_CREATE_FLAGS)] = PSO_CREATE_FLAGS;
                EnumWriter.Write(PSO_CREATE_FLAGS, Path.Combine(baseEnumDir, $"{nameof(PSO_CREATE_FLAGS)}.cs"));
            }

            {
                var FILL_MODE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/RasterizerState.h", 41, 62);
                codeTypeInfo.Enums[nameof(FILL_MODE)] = FILL_MODE;
                EnumWriter.Write(FILL_MODE, Path.Combine(baseEnumDir, $"{nameof(FILL_MODE)}.cs"));
            }

            {
                var CULL_MODE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/RasterizerState.h", 64, 90);
                codeTypeInfo.Enums[nameof(CULL_MODE)] = CULL_MODE;
                EnumWriter.Write(CULL_MODE, Path.Combine(baseEnumDir, $"{nameof(CULL_MODE)}.cs"));
            }

            {
                var PIPELINE_TYPE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/PipelineState.h", 318, 336);
                codeTypeInfo.Enums[nameof(PIPELINE_TYPE)] = PIPELINE_TYPE;
                EnumWriter.Write(PIPELINE_TYPE, Path.Combine(baseEnumDir, $"{nameof(PIPELINE_TYPE)}.cs"));
            }

            {
                var SHADER_RESOURCE_VARIABLE_TYPE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/ShaderResourceVariable.h", 46, 66);
                codeTypeInfo.Enums[nameof(SHADER_RESOURCE_VARIABLE_TYPE)] = SHADER_RESOURCE_VARIABLE_TYPE;
                EnumWriter.Write(SHADER_RESOURCE_VARIABLE_TYPE, Path.Combine(baseEnumDir, $"{nameof(SHADER_RESOURCE_VARIABLE_TYPE)}.cs"));
            }

            {
                var DRAW_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceContext.h", 66, 128);
                codeTypeInfo.Enums[nameof(DRAW_FLAGS)] = DRAW_FLAGS;
                EnumWriter.Write(DRAW_FLAGS, Path.Combine(baseEnumDir, $"{nameof(DRAW_FLAGS)}.cs"));
            }

            {
                var SWAP_CHAIN_USAGE_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 1249, 1266);
                codeTypeInfo.Enums[nameof(SWAP_CHAIN_USAGE_FLAGS)] = SWAP_CHAIN_USAGE_FLAGS;

                foreach (var prop in SWAP_CHAIN_USAGE_FLAGS.Properties)
                {
                    if (prop.Value?.EndsWith("L") == true)
                    {
                        prop.Value = prop.Value.Substring(0, prop.Value.Length - 1);
                    }
                }

                EnumWriter.Write(SWAP_CHAIN_USAGE_FLAGS, Path.Combine(baseEnumDir, $"{nameof(SWAP_CHAIN_USAGE_FLAGS)}.cs"));
            }

            {
                var VALUE_TYPE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 45, 62);
                codeTypeInfo.Enums[nameof(VALUE_TYPE)] = VALUE_TYPE;
                EnumWriter.Write(VALUE_TYPE, Path.Combine(baseEnumDir, $"{nameof(VALUE_TYPE)}.cs"));
            }

            {
                var INPUT_ELEMENT_FREQUENCY = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/InputLayout.h", 45, 60);
                codeTypeInfo.Enums[nameof(INPUT_ELEMENT_FREQUENCY)] = INPUT_ELEMENT_FREQUENCY;
                EnumWriter.Write(INPUT_ELEMENT_FREQUENCY, Path.Combine(baseEnumDir, $"{nameof(INPUT_ELEMENT_FREQUENCY)}.cs"));
            }

            {
                var MAP_TYPE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 156, 175);
                codeTypeInfo.Enums[nameof(MAP_TYPE)] = MAP_TYPE;
                EnumWriter.Write(MAP_TYPE, Path.Combine(baseEnumDir, $"{nameof(MAP_TYPE)}.cs"));
            }

            {
                var MAP_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 178, 204);
                codeTypeInfo.Enums[nameof(MAP_FLAGS)] = MAP_FLAGS;
                EnumWriter.Write(MAP_FLAGS, Path.Combine(baseEnumDir, $"{nameof(MAP_FLAGS)}.cs"));
            }

            {
                var SET_VERTEX_BUFFERS_FLAGS = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceContext.h", 533, 544);
                codeTypeInfo.Enums[nameof(SET_VERTEX_BUFFERS_FLAGS)] = SET_VERTEX_BUFFERS_FLAGS;
                EnumWriter.Write(SET_VERTEX_BUFFERS_FLAGS, Path.Combine(baseEnumDir, $"{nameof(SET_VERTEX_BUFFERS_FLAGS)}.cs"));
            }

            {
                var FILTER_TYPE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 816, 836);
                codeTypeInfo.Enums[nameof(FILTER_TYPE)] = FILTER_TYPE;
                EnumWriter.Write(FILTER_TYPE, Path.Combine(baseEnumDir, $"{nameof(FILTER_TYPE)}.cs"));
            }

            {
                var TEXTURE_ADDRESS_MODE = CodeEnum.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 839, 877);
                codeTypeInfo.Enums[nameof(TEXTURE_ADDRESS_MODE)] = TEXTURE_ADDRESS_MODE;
                EnumWriter.Write(TEXTURE_ADDRESS_MODE, Path.Combine(baseEnumDir, $"{nameof(TEXTURE_ADDRESS_MODE)}.cs"));
            }

            //////////// Structs

            var baseStructDir = Path.Combine(baseCSharpOutDir, "Structs");
            {
                var ShaderResourceVariableDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/PipelineState.h", 73, 85);
                codeTypeInfo.Structs[nameof(ShaderResourceVariableDesc)] = ShaderResourceVariableDesc;
                codeWriter.AddWriter(new StructCsWriter(ShaderResourceVariableDesc), Path.Combine(baseStructDir, $"{nameof(ShaderResourceVariableDesc)}.cs"));
                codeWriter.AddWriter(new StructCsPassStructWriter(ShaderResourceVariableDesc), Path.Combine(baseStructDir, $"{nameof(ShaderResourceVariableDesc)}.PassStruct.cs"));
                codeWriter.AddWriter(new StructCppPassStructWriter(ShaderResourceVariableDesc), Path.Combine(baseCPlusPlusOutDir, $"{nameof(ShaderResourceVariableDesc)}.PassStruct.h"));
            }

            {
                var DeviceObjectAttribs = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 1150, 1159);
                codeTypeInfo.Structs[nameof(DeviceObjectAttribs)] = DeviceObjectAttribs;
                codeWriter.AddWriter(new StructCsWriter(DeviceObjectAttribs), Path.Combine(baseStructDir, $"{nameof(DeviceObjectAttribs)}.cs"));
            }

            {
                var BufferDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Buffer.h", 72, 108);
                codeTypeInfo.Structs[nameof(BufferDesc)] = BufferDesc;
                codeWriter.AddWriter(new StructCsWriter(BufferDesc), Path.Combine(baseStructDir, $"{nameof(BufferDesc)}.cs"));
            }

            {
                var SwapChainDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/GraphicsTypes.h", 1301, 1345);
                codeTypeInfo.Structs[nameof(SwapChainDesc)] = SwapChainDesc;

                SwapChainDesc.Properties.First(i => i.Name == "DefaultDepthValue").DefaultValue = SwapChainDesc.Properties.First(i => i.Name == "DefaultDepthValue").DefaultValue.Replace(".f", "f");

                codeWriter.AddWriter(new StructCsWriter(SwapChainDesc), Path.Combine(baseStructDir, $"{nameof(SwapChainDesc)}.cs"));
            }

            {
                var BufferDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Buffer.h", 72, 108);
                codeTypeInfo.Structs[nameof(BufferDesc)] = BufferDesc;
                codeWriter.AddWriter(new StructCsWriter(BufferDesc), Path.Combine(baseStructDir, $"{nameof(BufferDesc)}.cs"));
            }

            {
                var ShaderDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Shader.h", 131, 138);
                codeTypeInfo.Structs[nameof(ShaderDesc)] = ShaderDesc;
                codeWriter.AddWriter(new StructCsWriter(ShaderDesc), Path.Combine(baseStructDir, $"{nameof(ShaderDesc)}.cs"));
            }

            {
                var ImmutableSamplerDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/PipelineState.h", 100, 115);
                codeTypeInfo.Structs[nameof(ImmutableSamplerDesc)] = ImmutableSamplerDesc;
                codeWriter.AddWriter(new StructCsWriter(ImmutableSamplerDesc), Path.Combine(baseStructDir, $"{nameof(ImmutableSamplerDesc)}.cs"));
                codeWriter.AddWriter(new StructCsPassStructWriter(ImmutableSamplerDesc), Path.Combine(baseStructDir, $"{nameof(ImmutableSamplerDesc)}.PassStruct.cs"));
                codeWriter.AddWriter(new StructCppPassStructWriter(ImmutableSamplerDesc), Path.Combine(baseCPlusPlusOutDir, $"{nameof(ImmutableSamplerDesc)}.PassStruct.h"));
            }

            {
                var SamplerDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Sampler.h", 46, 112);
                codeTypeInfo.Structs[nameof(SamplerDesc)] = SamplerDesc;
                codeWriter.AddWriter(new StructCsWriter(SamplerDesc), Path.Combine(baseStructDir, $"{nameof(SamplerDesc)}.cs"));
            }

            {
                var LayoutElement = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/InputLayout.h", 61, 109);
                codeTypeInfo.Structs[nameof(LayoutElement)] = LayoutElement;
                codeWriter.AddWriter(new StructCsWriter(LayoutElement), Path.Combine(baseStructDir, $"{nameof(LayoutElement)}.cs"));
                codeWriter.AddWriter(new StructCsPassStructWriter(LayoutElement), Path.Combine(baseStructDir, $"{nameof(LayoutElement)}.PassStruct.cs"));
                codeWriter.AddWriter(new StructCppPassStructWriter(LayoutElement), Path.Combine(baseCPlusPlusOutDir, $"{nameof(LayoutElement)}.PassStruct.h"));
            }

            {
                var PipelineResourceLayoutDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/PipelineState.h", 129, 149);
                codeTypeInfo.Structs[nameof(PipelineResourceLayoutDesc)] = PipelineResourceLayoutDesc;

                {
                    var Variables = PipelineResourceLayoutDesc.Properties.First(i => i.Name == "Variables");
                    Variables.IsArray = true;
                    Variables.PutAutoSize = "NumVariables";
                }
                {
                    var Variables = PipelineResourceLayoutDesc.Properties.First(i => i.Name == "NumVariables");
                    Variables.TakeAutoSize = "Variables";
                }

                {
                    var ImmutableSamplers = PipelineResourceLayoutDesc.Properties.First(i => i.Name == "ImmutableSamplers");
                    ImmutableSamplers.IsArray = true;
                    ImmutableSamplers.PutAutoSize = "NumImmutableSamplers";
                }
                {
                    var NumImmutableSamplers = PipelineResourceLayoutDesc.Properties.First(i => i.Name == "NumImmutableSamplers");
                    NumImmutableSamplers.TakeAutoSize = "ImmutableSamplers";
                }

                codeWriter.AddWriter(new StructCsWriter(PipelineResourceLayoutDesc), Path.Combine(baseStructDir, $"{nameof(PipelineResourceLayoutDesc)}.cs"));
            }


            {
                var BlendStateDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/BlendState.h", 372, 387);
                codeTypeInfo.Structs[nameof(BlendStateDesc)] = BlendStateDesc;
                var remove = new List<String> { "RenderTargets[DILIGENT_MAX_RENDER_TARGETS]", "RenderTargets" };
                BlendStateDesc.Properties = BlendStateDesc.Properties.Where(i => !remove.Contains(i.Name)).ToList();
                codeWriter.AddWriter(new StructCsWriter(BlendStateDesc), Path.Combine(baseStructDir, $"{nameof(BlendStateDesc)}.cs"));
            }

            {
                var RasterizerStateDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/RasterizerState.h", 94, 138);
                codeTypeInfo.Structs[nameof(RasterizerStateDesc)] = RasterizerStateDesc;

                RasterizerStateDesc.Properties.First(i => i.Name == "DepthBiasClamp").DefaultValue = RasterizerStateDesc.Properties.First(i => i.Name == "DepthBiasClamp").DefaultValue.Replace(".f", "f");
                RasterizerStateDesc.Properties.First(i => i.Name == "SlopeScaledDepthBias").DefaultValue = RasterizerStateDesc.Properties.First(i => i.Name == "SlopeScaledDepthBias").DefaultValue.Replace(".f", "f");

                codeWriter.AddWriter(new StructCsWriter(RasterizerStateDesc), Path.Combine(baseStructDir, $"{nameof(RasterizerStateDesc)}.cs"));
            }

            {
                var DepthStencilStateDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DepthStencilState.h", 151, 188);
                codeTypeInfo.Structs[nameof(DepthStencilStateDesc)] = DepthStencilStateDesc;
                codeWriter.AddWriter(new StructCsWriter(DepthStencilStateDesc), Path.Combine(baseStructDir, $"{nameof(DepthStencilStateDesc)}.cs"));
            }

            {
                var DrawAttribs = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceContext.h", 167, 188);
                codeTypeInfo.Structs[nameof(DrawAttribs)] = DrawAttribs;
                codeWriter.AddWriter(new StructCsWriter(DrawAttribs), Path.Combine(baseStructDir, $"{nameof(DrawAttribs)}.cs"));
            }

            {
                var InputLayoutDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/InputLayout.h", 201, 209);
                codeTypeInfo.Structs[nameof(InputLayoutDesc)] = InputLayoutDesc;

                {
                    var LayoutElements = InputLayoutDesc.Properties.First(i => i.Name == "LayoutElements");
                    LayoutElements.IsArray = true;
                    LayoutElements.PutAutoSize = "NumElements";
                }

                {
                    var NumElements = InputLayoutDesc.Properties.First(i => i.Name == "NumElements");
                    NumElements.TakeAutoSize = "LayoutElements";
                }

                codeWriter.AddWriter(new StructCsWriter(InputLayoutDesc), Path.Combine(baseStructDir, $"{nameof(InputLayoutDesc)}.cs"));
            }

            {
                var RenderTargetBlendDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/BlendState.h", 273, 319);
                codeTypeInfo.Structs[nameof(RenderTargetBlendDesc)] = RenderTargetBlendDesc;
                var RenderTargetWriteMask = RenderTargetBlendDesc.Properties.First(i => i.Name == "RenderTargetWriteMask");
                RenderTargetWriteMask.DefaultValue = "(Uint8)COLOR_MASK.COLOR_MASK_ALL";

                codeWriter.AddWriter(new StructCsWriter(RenderTargetBlendDesc), Path.Combine(baseStructDir, $"{nameof(RenderTargetBlendDesc)}.cs"));
            }


            {
                var ShaderCreateInfo = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Shader.h", 223, 331);
                codeTypeInfo.Structs[nameof(ShaderCreateInfo)] = ShaderCreateInfo;
                var skip = new List<String> { "pShaderSourceStreamFactory", "ppConversionStream", "ByteCode", "ByteCodeSize", "Macros", "HLSLVersion", "ShaderCompiler", "GLSLVersion", "GLESSLVersion", "ppCompilerOutput" };
                ShaderCreateInfo.Properties = ShaderCreateInfo.Properties
                    .Where(i => !skip.Contains(i.Name)).ToList();
                codeWriter.AddWriter(new StructCsWriter(ShaderCreateInfo), Path.Combine(baseStructDir, $"{nameof(ShaderCreateInfo)}.cs"));
            }

            {
                var PipelineStateCreateInfo = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/PipelineState.h", 390, 400);
                codeTypeInfo.Structs[nameof(PipelineStateCreateInfo)] = PipelineStateCreateInfo;
                codeWriter.AddWriter(new StructCsWriter(PipelineStateCreateInfo), Path.Combine(baseStructDir, $"{nameof(PipelineStateCreateInfo)}.cs"));
            }

            {
                var GraphicsPipelineDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/PipelineState.h", 154, 214);
                codeTypeInfo.Structs[nameof(GraphicsPipelineDesc)] = GraphicsPipelineDesc;

                var remove = new List<String>() { "pRenderPass" };
                GraphicsPipelineDesc.Properties = GraphicsPipelineDesc.Properties.Where(i => !remove.Contains(i.Name)).ToList();

                codeWriter.AddWriter(new StructCsWriter(GraphicsPipelineDesc), Path.Combine(baseStructDir, $"{nameof(GraphicsPipelineDesc)}.cs"));
            }

            {
                var GraphicsPipelineStateCreateInfo = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/PipelineState.h", 402, 430);
                codeTypeInfo.Structs[nameof(GraphicsPipelineStateCreateInfo)] = GraphicsPipelineStateCreateInfo;
                codeWriter.AddWriter(new StructCsWriter(GraphicsPipelineStateCreateInfo), Path.Combine(baseStructDir, $"{nameof(GraphicsPipelineStateCreateInfo)}.cs"));
            }

            {
                var StencilOpDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DepthStencilState.h", 92, 115);
                codeTypeInfo.Structs[nameof(StencilOpDesc)] = StencilOpDesc;
                codeWriter.AddWriter(new StructCsWriter(StencilOpDesc), Path.Combine(baseStructDir, $"{nameof(StencilOpDesc)}.cs"));
            }

            {
                var PipelineStateDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/PipelineState.h", 338, 356);
                codeTypeInfo.Structs[nameof(PipelineStateDesc)] = PipelineStateDesc;
                codeWriter.AddWriter(new StructCsWriter(PipelineStateDesc), Path.Combine(baseStructDir, $"{nameof(PipelineStateDesc)}.cs"));
            }

            {
                var SampleDesc = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/PipelineState.h", 52, 61);
                codeTypeInfo.Structs[nameof(SampleDesc)] = SampleDesc;
                codeWriter.AddWriter(new StructCsWriter(SampleDesc), Path.Combine(baseStructDir, $"{nameof(SampleDesc)}.cs"));
            }

            {
                var BufferData = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Buffer.h", 153, 162);
                codeTypeInfo.Structs[nameof(BufferData)] = BufferData;
                codeWriter.AddWriter(new StructCsWriter(BufferData), Path.Combine(baseStructDir, $"{nameof(BufferData)}.cs"));
            }

            {
                var DrawIndexedAttribs = CodeStruct.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceContext.h", 222, 250);
                codeTypeInfo.Structs[nameof(DrawIndexedAttribs)] = DrawIndexedAttribs;
                codeWriter.AddWriter(new StructCsWriter(DrawIndexedAttribs), Path.Combine(baseStructDir, $"{nameof(DrawIndexedAttribs)}.cs"));
            }

            

            //////////// Interfaces
            var baseCSharpInterfaceDir = Path.Combine(baseCSharpOutDir, "Interfaces");

            {
                var IRenderDevice = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/RenderDevice.h", 72, 330);
                codeTypeInfo.Interfaces[nameof(IRenderDevice)] = IRenderDevice;

                {
                    var CreateShader = IRenderDevice.Methods.First(i => i.Name == "CreateShader");
                    CreateShader.ReturnType = "IShader*";
                    var ppShader = CreateShader.Args.First(i => i.Name == "ppShader");
                    ppShader.MakeReturnVal = true;
                    ppShader.Type = "IShader*";
                }

                {
                    var CreateGraphicsPipelineState = IRenderDevice.Methods.First(i => i.Name == "CreateGraphicsPipelineState");
                    CreateGraphicsPipelineState.ReturnType = "IPipelineState*";
                    {
                        var ppPipelineState = CreateGraphicsPipelineState.Args.First(i => i.Name == "ppPipelineState");
                        ppPipelineState.MakeReturnVal = true;
                        ppPipelineState.Type = "IPipelineState*";
                    }
                }

                {
                    var CreateBuffer = IRenderDevice.Methods.First(i => i.Name == "CreateBuffer");
                    CreateBuffer.ReturnType = "IBuffer*";
                    {
                        var ppBuffer = CreateBuffer.Args.First(i => i.Name == "ppBuffer");
                        ppBuffer.MakeReturnVal = true;
                        ppBuffer.Type = "IBuffer*";
                    }

                    {
                        var pBuffData = CreateBuffer.Args.First(i => i.Name == "pBuffData");
                        pBuffData.CppPrefix = "&";
                    }
                }

                var allowedMethods = new List<String> { "CreateShader", "CreateGraphicsPipelineState", "CreateBuffer" };
                IRenderDevice.Methods = IRenderDevice.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                codeWriter.AddWriter(new InterfaceCsWriter(IRenderDevice), Path.Combine(baseCSharpInterfaceDir, $"{nameof(IRenderDevice)}.cs"));
                var cppWriter = new InterfaceCppWriter(IRenderDevice, new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/RenderDevice.h",
                    "Color.h",
                    "LayoutElement.PassStruct.h",
                    "ShaderResourceVariableDesc.PassStruct.h",
                    "ImmutableSamplerDesc.PassStruct.h"
                });
                codeWriter.AddWriter(cppWriter, Path.Combine(baseCPlusPlusOutDir, $"{nameof(IRenderDevice)}.cpp"));
            }

            {
                var IDeviceContext = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceContext.h", 1366, 2203);
                codeTypeInfo.Interfaces[nameof(IDeviceContext)] = IDeviceContext;

                {
                    var MapBuffer = IDeviceContext.Methods.First(i => i.Name == "MapBuffer");
                    MapBuffer.ReturnType = "PVoid";
                    var pMappedData = MapBuffer.Args.First(i => i.Name == "pMappedData");
                    pMappedData.MakeReturnVal = true;
                    pMappedData.Type = "PVoid";
                }

                {
                    var SetVertexBuffers = IDeviceContext.Methods.First(i => i.Name == "SetVertexBuffers");
                    {
                        var ppBuffers = SetVertexBuffers.Args.First(i => i.Name == "ppBuffers");
                        ppBuffers.IsArray = true;
                    }
                    {
                        var pOffsets = SetVertexBuffers.Args.First(i => i.Name == "pOffsets");
                        pOffsets.IsArray = true;
                    }
                }

                var allowedMethods = new List<String> { /*"SetRenderTargets", */"DrawIndexed", "CommitShaderResources", "SetIndexBuffer", "Flush", "ClearRenderTarget", "ClearDepthStencil", "Draw", "SetPipelineState", "MapBuffer", "UnmapBuffer", "SetVertexBuffers" };
                IDeviceContext.Methods = IDeviceContext.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                var rgbaArgs = IDeviceContext.Methods.First(i => i.Name == "ClearRenderTarget")
                    .Args.First(i => i.Name == "RGBA");
                rgbaArgs.Type = "Color";
                rgbaArgs.CppPrefix = "(float*)&";
                codeWriter.AddWriter(new InterfaceCsWriter(IDeviceContext), Path.Combine(baseCSharpInterfaceDir, $"{nameof(IDeviceContext)}.cs"));
                var cppWriter = new InterfaceCppWriter(IDeviceContext, new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/DeviceContext.h",
                    "Color.h"
                });
                codeWriter.AddWriter(cppWriter, Path.Combine(baseCPlusPlusOutDir, $"{nameof(IDeviceContext)}.cpp"));
            }

            {
                var IDeviceObject = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/DeviceObject.h", 50, 96);
                codeTypeInfo.Interfaces[nameof(IDeviceObject)] = IDeviceObject;
                var allowedMethods = new List<String> { "Resize" };
                IDeviceObject.Methods = IDeviceObject.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                codeWriter.AddWriter(new InterfaceCsWriter(IDeviceObject), Path.Combine(baseCSharpInterfaceDir, $"{nameof(IDeviceObject)}.cs"));
                var cppWriter = new InterfaceCppWriter(IDeviceObject, new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/DeviceObject.h"
                });
                codeWriter.AddWriter(cppWriter, Path.Combine(baseCPlusPlusOutDir, $"{nameof(IDeviceObject)}.cpp"));
            }

            {
                var ISwapChain = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/SwapChain.h", 54, 119);
                codeTypeInfo.Interfaces[nameof(ISwapChain)] = ISwapChain;
                var allowedMethods = new List<String> { "Resize", "GetCurrentBackBufferRTV", "GetDepthBufferDSV", "Present" };
                ISwapChain.Methods = ISwapChain.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                codeWriter.AddWriter(new InterfaceCsWriter(ISwapChain), Path.Combine(baseCSharpInterfaceDir, $"{nameof(ISwapChain)}.cs"));
                var cppWriter = new InterfaceCppWriter(ISwapChain, new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/SwapChain.h"
                });
                codeWriter.AddWriter(cppWriter, Path.Combine(baseCPlusPlusOutDir, $"{nameof(ISwapChain)}.cpp"));
            }

            {
                var ITextureView = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/TextureView.h", 195, 227);
                codeTypeInfo.Interfaces[nameof(ITextureView)] = ITextureView;
                var allowedMethods = new List<String> { };
                ITextureView.Methods = ITextureView.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                codeWriter.AddWriter(new InterfaceCsWriter(ITextureView), Path.Combine(baseCSharpInterfaceDir, $"{nameof(ITextureView)}.cs"));
                var cppWriter = new InterfaceCppWriter(ITextureView, new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/TextureView.h"
                });
                codeWriter.AddWriter(cppWriter, Path.Combine(baseCPlusPlusOutDir, $"{nameof(ITextureView)}.cpp"));
            }

            {
                var IShader = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Shader.h", 406, 423);
                codeTypeInfo.Interfaces[nameof(IShader)] = IShader;
                var allowedMethods = new List<String> { };
                IShader.Methods = IShader.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                codeWriter.AddWriter(new InterfaceCsWriter(IShader), Path.Combine(baseCSharpInterfaceDir, $"{nameof(IShader)}.cs"));
                var cppWriter = new InterfaceCppWriter(IShader, new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/Shader.h"
                });
                codeWriter.AddWriter(cppWriter, Path.Combine(baseCPlusPlusOutDir, $"{nameof(IShader)}.cpp"));
            }

            {
                var IPipelineState = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/PipelineState.h", 510, 604);
                codeTypeInfo.Interfaces[nameof(IPipelineState)] = IPipelineState;

                {
                    var CreateGraphicsPipelineState = IPipelineState.Methods.First(i => i.Name == "CreateShaderResourceBinding");
                    CreateGraphicsPipelineState.ReturnType = "IShaderResourceBinding*";
                    var ppShaderResourceBinding = CreateGraphicsPipelineState.Args.First(i => i.Name == "ppShaderResourceBinding");
                    ppShaderResourceBinding.MakeReturnVal = true;
                    ppShaderResourceBinding.Type = "IShaderResourceBinding*";
                }

                var allowedMethods = new List<String> { "GetStaticVariableByName", "CreateShaderResourceBinding" };
                IPipelineState.Methods = IPipelineState.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                codeWriter.AddWriter(new InterfaceCsWriter(IPipelineState), Path.Combine(baseCSharpInterfaceDir, $"{nameof(IPipelineState)}.cs"));
                var cppWriter = new InterfaceCppWriter(IPipelineState, new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/PipelineState.h"
                });
                codeWriter.AddWriter(cppWriter, Path.Combine(baseCPlusPlusOutDir, $"{nameof(IPipelineState)}.cpp"));
            }

            {
                var IBuffer = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/Buffer.h", 185, 241);
                codeTypeInfo.Interfaces[nameof(IBuffer)] = IBuffer;
                var allowedMethods = new List<String> { };
                IBuffer.Methods = IBuffer.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                codeWriter.AddWriter(new InterfaceCsWriter(IBuffer), Path.Combine(baseCSharpInterfaceDir, $"{nameof(IBuffer)}.cs"));
                var cppWriter = new InterfaceCppWriter(IBuffer, new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/Buffer.h"
                });
                codeWriter.AddWriter(cppWriter, Path.Combine(baseCPlusPlusOutDir, $"{nameof(IBuffer)}.cpp"));
            }

            {
                var IShaderResourceVariable = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/ShaderResourceVariable.h", 115, 158);
                codeTypeInfo.Interfaces[nameof(IShaderResourceVariable)] = IShaderResourceVariable;
                var allowedMethods = new List<String> { "Set" };
                IShaderResourceVariable.Methods = IShaderResourceVariable.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                codeWriter.AddWriter(new InterfaceCsWriter(IShaderResourceVariable), Path.Combine(baseCSharpInterfaceDir, $"{nameof(IShaderResourceVariable)}.cs"));
                var cppWriter = new InterfaceCppWriter(IShaderResourceVariable, new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/ShaderResourceVariable.h"
                });
                codeWriter.AddWriter(cppWriter, Path.Combine(baseCPlusPlusOutDir, $"{nameof(IShaderResourceVariable)}.cpp"));
            }

            {
                var IShaderResourceBinding = CodeInterface.Find(baseDir + "/DiligentCore/Graphics/GraphicsEngine/interface/ShaderResourceBinding.h", 55, 132);
                codeTypeInfo.Interfaces[nameof(IShaderResourceBinding)] = IShaderResourceBinding;
                var allowedMethods = new List<String> {  };
                IShaderResourceBinding.Methods = IShaderResourceBinding.Methods
                    .Where(i => allowedMethods.Contains(i.Name)).ToList();
                codeWriter.AddWriter(new InterfaceCsWriter(IShaderResourceBinding), Path.Combine(baseCSharpInterfaceDir, $"{nameof(IShaderResourceBinding)}.cs"));
                var cppWriter = new InterfaceCppWriter(IShaderResourceBinding, new List<String>()
                {
                    "Graphics/GraphicsEngine/interface/ShaderResourceBinding.h"
                });
                codeWriter.AddWriter(cppWriter, Path.Combine(baseCPlusPlusOutDir, $"{nameof(IShaderResourceBinding)}.cpp"));
            }

            



            codeWriter.WriteFiles(new CodeRendererContext(codeTypeInfo));
        }
    }
}
