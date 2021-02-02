#include "StdAfx.h"

#include "Graphics/GraphicsEngine/interface/Shader.h"
#include "Graphics/GraphicsEngine/interface/PipelineState.h"
#include "Graphics/GraphicsEngine/interface/SwapChain.h"
#include "Graphics/GraphicsEngine/interface/RenderDevice.h"

#include "Common/interface/RefCntAutoPtr.hpp"

using namespace Diligent;

extern "C" _AnomalousExport ShaderCreateInfo * ShaderCreateInfo_Create()
{
    return new ShaderCreateInfo;
}

extern "C" _AnomalousExport void ShaderCreateInfo_Delete(ShaderCreateInfo * obj)
{
    delete obj;
}

static const char* VSSource = R"(
struct PSInput 
{ 
    float4 Pos   : SV_POSITION; 
    float3 Color : COLOR; 
};

void main(in  uint    VertId : SV_VertexID,
          out PSInput PSIn) 
{
    float4 Pos[3];
    Pos[0] = float4(-0.5, -0.5, 0.0, 1.0);
    Pos[1] = float4( 0.0, +0.5, 0.0, 1.0);
    Pos[2] = float4(+0.5, -0.5, 0.0, 1.0);

    float3 Col[3];
    Col[0] = float3(1.0, 0.0, 0.0); // red
    Col[1] = float3(0.0, 1.0, 0.0); // green
    Col[2] = float3(0.0, 0.0, 1.0); // blue

    PSIn.Pos   = Pos[VertId];
    PSIn.Color = Col[VertId];
}
)";

// Pixel shader simply outputs interpolated vertex color
static const char* PSSource = R"(
struct PSInput 
{ 
    float4 Pos   : SV_POSITION; 
    float3 Color : COLOR; 
};

struct PSOutput
{ 
    float4 Color : SV_TARGET; 
};

void main(in  PSInput  PSIn,
          out PSOutput PSOut)
{
    PSOut.Color = float4(PSIn.Color.rgb, 1.0);
}
)";

extern "C" _AnomalousExport void Lazy_VS(ShaderCreateInfo * ShaderCI)
{
    // Tell the system that the shader source code is in HLSL.
    // For OpenGL, the engine will convert this into GLSL under the hood
    ShaderCI->SourceLanguage = SHADER_SOURCE_LANGUAGE_HLSL;
    // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
    ShaderCI->UseCombinedTextureSamplers = true;
    ShaderCI->Desc.ShaderType = SHADER_TYPE_VERTEX;
    ShaderCI->EntryPoint = "main";
    ShaderCI->Desc.Name = "Triangle vertex shader";
    ShaderCI->Source = VSSource;
}

extern "C" _AnomalousExport void Lazy_PS(ShaderCreateInfo * ShaderCI)
{
    // Tell the system that the shader source code is in HLSL.
    // For OpenGL, the engine will convert this into GLSL under the hood
    ShaderCI->SourceLanguage = SHADER_SOURCE_LANGUAGE_HLSL;
    // OpenGL backend requires emulated combined HLSL texture samplers (g_Texture + g_Texture_sampler combination)
    ShaderCI->UseCombinedTextureSamplers = true;
    ShaderCI->Desc.ShaderType = SHADER_TYPE_PIXEL;
    ShaderCI->EntryPoint = "main";
    ShaderCI->Desc.Name = "Triangle pixel shader";
    ShaderCI->Source = PSSource;
}