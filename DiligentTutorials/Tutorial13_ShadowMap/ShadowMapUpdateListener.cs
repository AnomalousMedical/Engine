using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;
using PVoid = System.IntPtr;
using float4 = Engine.Vector4;
using float3 = Engine.Vector3;
using float2 = Engine.Vector2;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;
using System.IO;

namespace Tutorial13_ShadowMap
{
    class TextureUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        private readonly TextureLoader textureLoader;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext m_pImmediateContext;

        private AutoPtr<IBuffer> m_VSConstants;

        public unsafe TextureUpdateListener(GraphicsEngine graphicsEngine, NativeOSWindow window, TextureLoader textureLoader)
        {
            this.graphicsEngine = graphicsEngine;
            this.window = window;
            this.textureLoader = textureLoader;
            this.swapChain = graphicsEngine.SwapChain;
            this.m_pImmediateContext = graphicsEngine.ImmediateContext;
            var pDevice = graphicsEngine.RenderDevice;

            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pSwapChain = graphicsEngine.SwapChain;

            var Barriers = new List<StateTransitionDesc>();
            // Create dynamic uniform buffer that will store our transformation matrices
            // Dynamic buffers can be frequently updated by the CPU

            {
                BufferDesc CBDesc = new BufferDesc();
                CBDesc.Name = "VS constants CB";
                CBDesc.uiSizeInBytes = (uint)(sizeof(float4x4) * 2 + sizeof(float4));
                CBDesc.Usage = USAGE.USAGE_DYNAMIC;
                CBDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                CBDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;

                m_VSConstants = pDevice.CreateBuffer(CBDesc);
                Barriers.Add(new StateTransitionDesc()
                {
                    pResource = m_VSConstants.Obj,
                    OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN,
                    NewState = RESOURCE_STATE.RESOURCE_STATE_CONSTANT_BUFFER,
                    UpdateResourceState = true
                });
            }

            //CreateCubePSO();
            //CreatePlanePSO();
            //CreateShadowMapVisPSO();

            //// Load cube

            //// In this tutorial we need vertices with normals
            //CreateVertexBuffer();
            //// Load index buffer
            //m_CubeIndexBuffer = TexturedCube::CreateIndexBuffer(m_pDevice);
            //// Explicitly transition vertex and index buffers to required states
            //Barriers.emplace_back(m_CubeVertexBuffer, RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_VERTEX_BUFFER, true);
            //Barriers.emplace_back(m_CubeIndexBuffer, RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_INDEX_BUFFER, true);
            //// Load texture
            //auto CubeTexture = TexturedCube::LoadTexture(m_pDevice, "DGLogo.png");
            //m_CubeSRB->GetVariableByName(SHADER_TYPE_PIXEL, "g_Texture")->Set(CubeTexture->GetDefaultView(TEXTURE_VIEW_SHADER_RESOURCE));
            //// Transition the texture to shader resource state
            //Barriers.emplace_back(CubeTexture, RESOURCE_STATE_UNKNOWN, RESOURCE_STATE_SHADER_RESOURCE, true);

            //CreateShadowMap();

            m_pImmediateContext.TransitionResourceStates(Barriers);
        }

        public void Dispose()
        {
            m_VSConstants.Dispose();
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public unsafe void sendUpdate(Clock clock)
        {
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var preTransform = swapChain.GetDesc_PreTransform;
            m_pImmediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            var ClearColor = new Engine.Color(0.350f, 0.350f, 0.350f, 1.0f);

            // Clear the back buffer
            // Let the engine perform required state transitions
            m_pImmediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            m_pImmediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            

            this.swapChain.Present(1);
        }

        const String VSSource =
    @"cbuffer Constants
{
    float4x4 g_WorldViewProj;
};

// Vertex shader takes two inputs: vertex position and uv coordinates.
// By convention, Diligent Engine expects vertex shader inputs to be 
// labeled 'ATTRIBn', where n is the attribute number.
struct VSInput
{
    float3 Pos : ATTRIB0;
    float2 UV  : ATTRIB1;
};

struct PSInput 
{ 
    float4 Pos : SV_POSITION; 
    float2 UV  : TEX_COORD; 
};

// Note that if separate shader objects are not supported (this is only the case for old GLES3.0 devices), vertex
// shader output variable name must match exactly the name of the pixel shader input variable.
// If the variable has structure type (like in this example), the structure declarations must also be indentical.
void main(in  VSInput VSIn,
          out PSInput PSIn) 
{
    PSIn.Pos = mul( float4(VSIn.Pos,1.0), g_WorldViewProj);
    PSIn.UV  = VSIn.UV;
}
";

        const String PSSource =
    @"Texture2D    g_Texture;
SamplerState g_Texture_sampler; // By convention, texture samplers must use the '_sampler' suffix

struct PSInput 
{ 
    float4 Pos : SV_POSITION; 
    float2 UV : TEX_COORD; 
};

struct PSOutput
{
    float4 Color : SV_TARGET;
};

void main(in  PSInput  PSIn,
          out PSOutput PSOut)
{
    PSOut.Color = g_Texture.Sample(g_Texture_sampler, PSIn.UV); 
}
";
    }
}
