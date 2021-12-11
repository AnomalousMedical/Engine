using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngineRayTracing
{
    class RayTracingUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;

        UInt32 m_MaxRecursionDepth = 8;

        private AutoPtr<IBuffer> m_ConstantsCB;
        private Constants m_Constants;

        public unsafe RayTracingUpdateListener(GraphicsEngine graphicsEngine)
        {
            this.graphicsEngine = graphicsEngine;
            this.swapChain = graphicsEngine.SwapChain;
            this.immediateContext = graphicsEngine.ImmediateContext;

            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pSwapChain = graphicsEngine.SwapChain;

            // Create a buffer with shared constants.
            BufferDesc BuffDesc = new BufferDesc();
            BuffDesc.Name = "Constant buffer";
            BuffDesc.uiSizeInBytes = (uint)sizeof(Constants);
            BuffDesc.Usage = USAGE.USAGE_DEFAULT;
            BuffDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;

            m_ConstantsCB = m_pDevice.CreateBuffer(BuffDesc);
            //VERIFY_EXPR(m_ConstantsCB != nullptr);

            //CreateGraphicsPSO();
            //CreateRayTracingPSO();
            //LoadTextures();
            //CreateCubeBLAS();
            //CreateProceduralBLAS();
            //UpdateTLAS();
            //CreateSBT();

            // Setup camera.
            //m_Camera.SetPos(float3(-7.f, -0.5f, 16.5f));
            //m_Camera.SetRotation(-2.67f, -0.145f);
            //m_Camera.SetRotationSpeed(0.005f);
            //m_Camera.SetMoveSpeed(5.f);
            //m_Camera.SetSpeedUpScales(5.f, 10.f);

            // Initialize constants.
            m_Constants = new Constants
            {
                ClipPlanes = new Vector2(0.1f, 100.0f),
                ShadowPCF = 1,
                MaxRecursion = Math.Min(6, m_MaxRecursionDepth),

                // Sphere constants.
                SphereReflectionColorMask = new Vector3(0.81f, 1.0f, 0.45f),
                SphereReflectionBlur = 1,

                // Glass cube constants.
                GlassReflectionColorMask = new Vector3(0.22f, 0.83f, 0.93f),
                GlassAbsorption = 0.5f,
                GlassMaterialColor = new Vector4(0.33f, 0.93f, 0.29f, 0f),
                GlassIndexOfRefraction = new Vector2(1.5f, 1.02f),
                GlassEnableDispersion = 0,

                // Wavelength to RGB and index of refraction interpolation factor.
                DispersionSamples_0 = new Vector4(0.140000f, 0.000000f, 0.266667f, 0.53f),
                DispersionSamples_1 = new Vector4(0.130031f, 0.037556f, 0.612267f, 0.25f),
                DispersionSamples_2 = new Vector4(0.100123f, 0.213556f, 0.785067f, 0.16f),
                DispersionSamples_3 = new Vector4(0.050277f, 0.533556f, 0.785067f, 0.00f),
                DispersionSamples_4 = new Vector4(0.000000f, 0.843297f, 0.619682f, 0.13f),
                DispersionSamples_5 = new Vector4(0.000000f, 0.927410f, 0.431834f, 0.38f),
                DispersionSamples_6 = new Vector4(0.000000f, 0.972325f, 0.270893f, 0.27f),
                DispersionSamples_7 = new Vector4(0.000000f, 0.978042f, 0.136858f, 0.19f),
                DispersionSamples_8 = new Vector4(0.324000f, 0.944560f, 0.029730f, 0.47f),
                DispersionSamples_9 = new Vector4(0.777600f, 0.871879f, 0.000000f, 0.64f),
                DispersionSamples_10 = new Vector4(0.972000f, 0.762222f, 0.000000f, 0.77f),
                DispersionSamples_11 = new Vector4(0.971835f, 0.482222f, 0.000000f, 0.62f),
                DispersionSamples_12 = new Vector4(0.886744f, 0.202222f, 0.000000f, 0.73f),
                DispersionSamples_13 = new Vector4(0.715967f, 0.000000f, 0.000000f, 0.68f),
                DispersionSamples_14 = new Vector4(0.459920f, 0.000000f, 0.000000f, 0.91f),
                DispersionSamples_15 = new Vector4(0.218000f, 0.000000f, 0.000000f, 0.99f),
                DispersionSampleCount = 4,

                AmbientColor = new Vector4(1f, 1f, 1f, 0f) * 0.015f,
                LightPos_0 = new Vector4(8.00f, -8.0f, +0.00f, 0f),
                LightColor_0 = new Vector4(1.00f, +0.8f, +0.80f, 0f),
                LightPos_1 = new Vector4(0.00f, -4.0f, -5.00f, 0f),
                LightColor_1 = new Vector4(0.85f, +1.0f, +0.85f, 0f),

                // Random points on disc.
                DiscPoints_0 = new Vector4(+0.0f, +0.0f, +0.9f, -0.9f),
                DiscPoints_1 = new Vector4(-0.8f, +1.0f, -1.1f, -0.8f),
                DiscPoints_2 = new Vector4(+1.5f, +1.2f, -2.1f, +0.7f),
                DiscPoints_3 = new Vector4(+0.1f, -2.2f, -0.2f, +2.4f),
                DiscPoints_4 = new Vector4(+2.4f, -0.3f, -3.0f, +2.8f),
                DiscPoints_5 = new Vector4(+2.0f, -2.6f, +0.7f, +3.5f),
                DiscPoints_6 = new Vector4(-3.2f, -1.6f, +3.4f, +2.2f),
                DiscPoints_7 = new Vector4(-1.8f, -3.2f, -1.1f, +3.6f),
            };
        }

        public void Dispose()
        {
            m_ConstantsCB.Dispose();
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public void sendUpdate(Clock clock)
        {
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var ClearColor = new Color(0.350f, 0.350f, 0.350f, 1.0f);

            // Clear the back buffer
            // Let the engine perform required state transitions
            immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            this.swapChain.Present(1);
        }
    }
}
