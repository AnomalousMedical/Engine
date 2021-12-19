using DiligentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSandbox
{
    class ProceduralBLAS : IDisposable
    {
        AutoPtr<IBuffer> m_BoxAttribsCB;
        AutoPtr<IBottomLevelAS> m_pProceduralBLAS;

        public unsafe ProceduralBLAS(GraphicsEngine graphicsEngine)
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;

            //static_assert(sizeof(HLSL::BoxAttribs) % 16 == 0, "BoxAttribs must be aligned by 16 bytes");

            var Boxes = new BoxAttribs() { minX = -2.5f, minY = -2.5f, minZ = -2.5f, maxX = 2.5f, maxY = 2.5f, maxZ = 2.5f };

            // Create box buffer
            {
                var BufData = new BufferData { pData = new IntPtr(&Boxes), DataSize = (uint)sizeof(BoxAttribs) };
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "AABB Buffer";
                BuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_RAY_TRACING | BIND_FLAGS.BIND_SHADER_RESOURCE;
                BuffDesc.uiSizeInBytes = BufData.DataSize;
                BuffDesc.ElementByteStride = (uint)sizeof(BoxAttribs);
                BuffDesc.Mode = BUFFER_MODE.BUFFER_MODE_STRUCTURED;

                m_BoxAttribsCB = m_pDevice.CreateBuffer(BuffDesc, BufData);
                //VERIFY_EXPR(m_BoxAttribsCB != nullptr);
            }

            // Create & build bottom level acceleration structure
            {
                // Create BLAS
                var BoxInfo = new BLASBoundingBoxDesc();
                {
                    BoxInfo.GeometryName = "Box";
                    BoxInfo.MaxBoxCount = 1;

                    var ASDesc = new BottomLevelASDesc();
                    ASDesc.Name = "Procedural BLAS";
                    ASDesc.Flags = RAYTRACING_BUILD_AS_FLAGS.RAYTRACING_BUILD_AS_PREFER_FAST_TRACE;
                    ASDesc.pBoxes = new List<BLASBoundingBoxDesc>() { BoxInfo };

                    m_pProceduralBLAS = m_pDevice.CreateBLAS(ASDesc);
                    //VERIFY_EXPR(m_pProceduralBLAS != nullptr);
                }

                // Create scratch buffer
                using var pScratchBuffer = m_pDevice.CreateBuffer(new BufferDesc()
                {
                    Name = "BLAS Scratch Buffer",
                    Usage = USAGE.USAGE_DEFAULT,
                    BindFlags = BIND_FLAGS.BIND_RAY_TRACING,
                    uiSizeInBytes = m_pProceduralBLAS.Obj.ScratchBufferSizes_Build,
                }, new BufferData());

                // Build BLAS
                var BoxData = new BLASBuildBoundingBoxData();
                BoxData.GeometryName = BoxInfo.GeometryName;
                BoxData.BoxCount = 1;
                BoxData.BoxStride = (uint)sizeof(BoxAttribs);
                BoxData.pBoxBuffer = m_BoxAttribsCB.Obj;

                var Attribs = new BuildBLASAttribs();
                Attribs.pBLAS = m_pProceduralBLAS.Obj;
                Attribs.pBoxData = new List<BLASBuildBoundingBoxData> { BoxData };

                // Scratch buffer will be used to store temporary data during BLAS build.
                // Previous content in the scratch buffer will be discarded.
                Attribs.pScratchBuffer = pScratchBuffer.Obj;

                // Allow engine to change resource states.
                Attribs.BLASTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
                Attribs.GeometryTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
                Attribs.ScratchBufferTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;

                m_pImmediateContext.BuildBLAS(Attribs);
            }
        }

        public void Dispose()
        {
            m_pProceduralBLAS.Dispose();
            m_BoxAttribsCB.Dispose();
        }

        public IBuffer Attribs => m_BoxAttribsCB.Obj;

        public IBottomLevelAS BLAS => m_pProceduralBLAS.Obj;
    }
}
