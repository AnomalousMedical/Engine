using DiligentEngine.RT.ShaderSets;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    public class RayTracingRenderer : IDisposable
    {
        //Camera Settings
        float YFov = MathFloat.PI / 4.0f;
        float ZNear = 0.1f;
        float ZFar = 100f;

        private readonly GraphicsEngine graphicsEngine;
        private readonly RTImageBlitter imageBlitter;
        private readonly RTCameraAndLight cameraAndLight;
        private UInt32 maxRecursionDepth = 8;

        private AutoPtr<IBuffer> m_ConstantsCB;
        private Constants m_Constants;
        private AutoPtr<IPipelineState> m_pRayTracingPSO;
        private AutoPtr<IShaderResourceBinding> m_pRayTracingSRB;
        private AutoPtr<IShaderBindingTable> m_pSBT;
        AutoPtr<IBuffer> m_ScratchBuffer;
        AutoPtr<IBuffer> m_InstanceBuffer;
        uint lastNumInstances = 0;
        bool rebuildPipeline = true;

        public RayTracingPipelineStateCreateInfo PSOCreateInfo { get; private set; } = CreatePSOCreateInfo();

        List<TLASBuildInstanceData> instances = new List<TLASBuildInstanceData>();
        List<ShaderTableBinder> shaderTableBinders = new List<ShaderTableBinder>();
        List<ShaderResourceBinder> shaderResourceBinders = new List<ShaderResourceBinder>();

        public delegate void ShaderResourceBinder(IShaderResourceBinding rayTracingSRB);

        public delegate void ShaderTableBinder(IShaderBindingTable sbt, ITopLevelAS tlas);

        public unsafe RayTracingRenderer
        (
            GraphicsEngine graphicsEngine,
            RTImageBlitter imageBlitter,
            RTCameraAndLight cameraAndLight,
            GeneralShaders generalShaders
        )
        {
            this.graphicsEngine = graphicsEngine;
            this.imageBlitter = imageBlitter;
            this.cameraAndLight = cameraAndLight;

            maxRecursionDepth = Math.Min(maxRecursionDepth, graphicsEngine.RenderDevice.DeviceProperties_MaxRayTracingRecursionDepth);
            m_Constants = Constants.CreateDefault(maxRecursionDepth);

            // Specify the maximum ray recursion depth.
            // WARNING: the driver does not track the recursion depth and it is the
            //          application's responsibility to not exceed the specified limit.
            //          The value is used to reserve the necessary stack size and
            //          exceeding it will likely result in driver crash.
            PSOCreateInfo.RayTracingPipeline.MaxRecursionDepth = (byte)maxRecursionDepth;

            generalShaders.Setup(PSOCreateInfo);
        }

        public void Dispose()
        {
            DestroyPSO();
        }

        private void DestroyPSO()
        {
            m_pSBT?.Dispose();
            m_InstanceBuffer?.Dispose();
            m_ScratchBuffer?.Dispose();
            m_pRayTracingSRB?.Dispose();
            m_pRayTracingPSO?.Dispose();
            m_ConstantsCB?.Dispose();
        }

        private unsafe static RayTracingPipelineStateCreateInfo CreatePSOCreateInfo()
        {
            // Prepare ray tracing pipeline description.
            var PSOCreateInfo = new RayTracingPipelineStateCreateInfo();

            PSOCreateInfo.PSODesc.Name = "Ray tracing PSO";
            PSOCreateInfo.PSODesc.PipelineType = PIPELINE_TYPE.PIPELINE_TYPE_RAY_TRACING;

            // Setup shader groups
            PSOCreateInfo.pGeneralShaders = new List<RayTracingGeneralShaderGroup>();

            PSOCreateInfo.pTriangleHitShaders = new List<RayTracingTriangleHitShaderGroup>();

            PSOCreateInfo.pProceduralHitShaders = new List<RayTracingProceduralHitShaderGroup>();

            //Set this to the size of the largest root shader record size
            PSOCreateInfo.RayTracingPipeline.ShaderRecordSize = (ushort)sizeof(HLSL.SpriteFrame);

            // DirectX 12 only: set attribute and payload size. Values should be as small as possible to minimize the memory usage.
            PSOCreateInfo.MaxAttributeSize = (uint)sizeof(/*BuiltInTriangleIntersectionAttributes*/ Vector2);
            PSOCreateInfo.MaxPayloadSize = (uint)Math.Max(sizeof(DiligentEngine.RT.HLSL.PrimaryRayPayload), sizeof(DiligentEngine.RT.HLSL.ShadowRayPayload));


            // Define immutable sampler for g_Texture and g_GroundTexture. Immutable samplers should be used whenever possible
            var SamLinearWrapDesc = new SamplerDesc
            {
                MinFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                MagFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                MipFilter = FILTER_TYPE.FILTER_TYPE_LINEAR,
                AddressU = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_WRAP,
                AddressV = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_WRAP,
                AddressW = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_WRAP
            };

            var SamPointWrapDesc = new SamplerDesc
            {
                MinFilter = FILTER_TYPE.FILTER_TYPE_POINT,
                MagFilter = FILTER_TYPE.FILTER_TYPE_POINT,
                MipFilter = FILTER_TYPE.FILTER_TYPE_POINT,
                AddressU = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_WRAP,
                AddressV = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_WRAP,
                AddressW = TEXTURE_ADDRESS_MODE.TEXTURE_ADDRESS_WRAP
            };

            var ImmutableSamplers = new List<ImmutableSamplerDesc>
            {
                new ImmutableSamplerDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, SamplerOrTextureName = "g_SamLinearWrap", Desc = SamLinearWrapDesc},
                new ImmutableSamplerDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_ANY_HIT, SamplerOrTextureName = "g_SamLinearWrap", Desc = SamLinearWrapDesc},

                new ImmutableSamplerDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, SamplerOrTextureName = "g_SamPointWrap", Desc = SamPointWrapDesc},
                new ImmutableSamplerDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_ANY_HIT, SamplerOrTextureName = "g_SamPointWrap", Desc = SamPointWrapDesc}
            };

            var Variables = new List<ShaderResourceVariableDesc> //
            {
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_GEN | SHADER_TYPE.SHADER_TYPE_RAY_MISS | SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, Name = "g_ConstantsCB", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_STATIC},
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_GEN | SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, Name = "g_TLAS", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC},
                new ShaderResourceVariableDesc{ShaderStages = SHADER_TYPE.SHADER_TYPE_RAY_GEN, Name = "g_ColorBuffer", Type = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_DYNAMIC} //This is the buffer where the rays are written
            };

            PSOCreateInfo.PSODesc.ResourceLayout.Variables = Variables;
            PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers = ImmutableSamplers;
            PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = SHADER_RESOURCE_VARIABLE_TYPE.SHADER_RESOURCE_VARIABLE_TYPE_MUTABLE;

            return PSOCreateInfo;
        }

        unsafe void CreateRayTracingPSO()
        {
            var m_pDevice = graphicsEngine.RenderDevice;

            // Create a buffer with shared constants.
            BufferDesc BuffDesc = new BufferDesc();
            BuffDesc.Name = "Constant buffer";
            BuffDesc.uiSizeInBytes = (uint)sizeof(Constants);
            BuffDesc.Usage = USAGE.USAGE_DEFAULT;
            BuffDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;

            m_ConstantsCB = m_pDevice.CreateBuffer(BuffDesc);
            //VERIFY_EXPR(m_ConstantsCB != nullptr);

            this.m_pRayTracingPSO = m_pDevice.CreateRayTracingPipelineState(PSOCreateInfo);
            //VERIFY_EXPR(m_pRayTracingPSO != nullptr);

            m_pRayTracingPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_GEN, "g_ConstantsCB").Set(m_ConstantsCB.Obj);
            m_pRayTracingPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_MISS, "g_ConstantsCB").Set(m_ConstantsCB.Obj);
            m_pRayTracingPSO.Obj.GetStaticVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_ConstantsCB").Set(m_ConstantsCB.Obj);

            m_pRayTracingSRB = m_pRayTracingPSO.Obj.CreateShaderResourceBinding(true);
            //VERIFY_EXPR(m_pRayTracingSRB != nullptr);

            BindShaderResources(m_pRayTracingSRB.Obj);
        }

        void CreateSBT()
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            // Create shader binding table.

            var SBTDesc = new ShaderBindingTableDesc();
            SBTDesc.Name = "SBT";
            SBTDesc.pPSO = m_pRayTracingPSO.Obj;

            m_pSBT = m_pDevice.CreateSBT(SBTDesc);
            //VERIFY_EXPR(m_pSBT != nullptr);

            m_pSBT.Obj.BindRayGenShader("Main", IntPtr.Zero, 0);

            m_pSBT.Obj.BindMissShader("PrimaryMiss", RtStructures.PRIMARY_RAY_INDEX, IntPtr.Zero, 0);
            m_pSBT.Obj.BindMissShader("ShadowMiss", RtStructures.SHADOW_RAY_INDEX, IntPtr.Zero, 0);
        }

        AutoPtr<ITopLevelAS> UpdateTLAS()
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;
            // Create or update top-level acceleration structure

            uint numInstances = (uint)instances.Count;
            if (numInstances == 0)
            {
                return null;
            }

            if (rebuildPipeline)
            {
                DestroyPSO();
                //These are disposed now, set them to null so they are recreated below
                m_ScratchBuffer = null;
                m_InstanceBuffer = null;

                CreateRayTracingPSO();
                CreateSBT();
                rebuildPipeline = false;
            }

            if (numInstances != lastNumInstances) //If instance count changes invalidate buffers
            {
                m_ScratchBuffer?.Dispose();
                m_ScratchBuffer = null;
                m_InstanceBuffer?.Dispose();
                m_InstanceBuffer = null;
                lastNumInstances = numInstances;
            }

            // Create TLAS
            var TLASDesc = new TopLevelASDesc();
            TLASDesc.Name = "TLAS";
            TLASDesc.MaxInstanceCount = numInstances;
            TLASDesc.Flags = RAYTRACING_BUILD_AS_FLAGS.RAYTRACING_BUILD_AS_ALLOW_UPDATE | RAYTRACING_BUILD_AS_FLAGS.RAYTRACING_BUILD_AS_PREFER_FAST_TRACE;

            var m_pTLAS = m_pDevice.CreateTLAS(TLASDesc);
            //VERIFY_EXPR(m_pTLAS != nullptr);

            m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_GEN, "g_TLAS").Set(m_pTLAS.Obj);
            m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_CLOSEST_HIT, "g_TLAS").Set(m_pTLAS.Obj);

            // Create scratch buffer
            if (m_ScratchBuffer == null)
            {
                m_ScratchBuffer = m_pDevice.CreateBuffer(new BufferDesc()
                {
                    Name = "TLAS Scratch Buffer",
                    Usage = USAGE.USAGE_DEFAULT,
                    BindFlags = BIND_FLAGS.BIND_RAY_TRACING,
                    uiSizeInBytes = Math.Max(m_pTLAS.Obj.ScratchBufferSizes_Build, m_pTLAS.Obj.ScratchBufferSizes_Update)
                }, new BufferData());
            }

            // Create instance buffer
            if (m_InstanceBuffer == null)
            {
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "TLAS Instance Buffer";
                BuffDesc.Usage = USAGE.USAGE_DEFAULT;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_RAY_TRACING;
                BuffDesc.uiSizeInBytes = ITopLevelAS.TLAS_INSTANCE_DATA_SIZE * numInstances;

                m_InstanceBuffer = m_pDevice.CreateBuffer(BuffDesc, new BufferData());
                //VERIFY_EXPR(m_InstanceBuffer != nullptr);
            }

            // Build or update TLAS
            var Attribs = new BuildTLASAttribs();
            Attribs.pTLAS = m_pTLAS.Obj;
            Attribs.Update = false;

            // Scratch buffer will be used to store temporary data during TLAS build or update.
            // Previous content in the scratch buffer will be discarded.
            Attribs.pScratchBuffer = m_ScratchBuffer.Obj;

            // Instance buffer will store instance data during TLAS build or update.
            // Previous content in the instance buffer will be discarded.
            Attribs.pInstanceBuffer = m_InstanceBuffer.Obj;

            // Instances will be converted to the format that is required by the graphics driver and copied to the instance buffer.
            Attribs.pInstances = instances;

            // Bind hit shaders per instance, it allows you to change the number of geometries in BLAS without invalidating the shader binding table.
            Attribs.BindingMode = HIT_GROUP_BINDING_MODE.HIT_GROUP_BINDING_MODE_PER_INSTANCE;
            Attribs.HitGroupStride = RtStructures.HIT_GROUP_STRIDE;

            // Allow engine to change resource states.
            Attribs.TLASTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
            Attribs.BLASTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
            Attribs.InstanceBufferTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
            Attribs.ScratchBufferTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;

            m_pImmediateContext.BuildTLAS(Attribs);

            // Hit groups for primary ray
            BindShaders(m_pSBT.Obj, m_pTLAS.Obj);

            // Hit groups for shadow ray.
            // null means no shaders are bound and hit shader invocation will be skipped.
            m_pSBT.Obj.BindHitGroupForTLAS(m_pTLAS.Obj, RtStructures.SHADOW_RAY_INDEX, null, IntPtr.Zero, 0);

            return m_pTLAS;
        }

        public unsafe void Render(Vector3 cameraPos, Quaternion cameraRot, Vector4 light1Pos, Vector4 ligth2Pos)
        {
            var swapChain = graphicsEngine.SwapChain;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;

            //TODO: Might be able to avoid recreating this like the other buffers, this also seems ok
            //So really need more info about behavior to decide here
            using var tlas = UpdateTLAS();
            if (tlas == null)
            {
                return;
            }

            // Update constants
            {
                var pDSV = swapChain.GetDepthBufferDSV();
                var preTransform = swapChain.GetDesc_PreTransform;

                //= new Vector3(0f, 0f, -15f);
                var preTransformMatrix = CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), preTransform);
                var cameraProj = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, imageBlitter.Width, imageBlitter.Height, preTransform);
                cameraAndLight.GetCameraPosition(cameraPos, cameraRot, preTransformMatrix, cameraProj, out var CameraWorldPos, out var CameraViewProj);

                var Frustum = new ViewFrustum();
                cameraAndLight.ExtractViewFrustumPlanesFromMatrix(CameraViewProj, Frustum, false);

                // Normalize frustum planes.
                for (ViewFrustum.PLANE_IDX i = 0; i < ViewFrustum.PLANE_IDX.NUM_PLANES; ++i)
                {
                    Plane3D plane = Frustum.GetPlane(i);
                    float invlen = 1.0f / plane.Normal.length();
                    plane.Normal *= invlen;
                    plane.Distance *= invlen;
                }

                // Calculate ray formed by the intersection two planes.
                void GetPlaneIntersection(ViewFrustum.PLANE_IDX lhs, ViewFrustum.PLANE_IDX rhs, out Vector4 result)
                {
                    var lp = Frustum.GetPlane(lhs);
                    var rp = Frustum.GetPlane(rhs);

                    Vector3 dir = lp.Normal.cross(rp.Normal);
                    float len = dir.length2();

                    //VERIFY_EXPR(len > 1.0e-5);

                    var v3result = dir * (1.0f / MathF.Sqrt(len));
                    result = new Vector4(v3result.x, v3result.y, v3result.z, 0);
                };

                GetPlaneIntersection(ViewFrustum.PLANE_IDX.BOTTOM_PLANE_IDX, ViewFrustum.PLANE_IDX.LEFT_PLANE_IDX, out m_Constants.FrustumRayLB);
                GetPlaneIntersection(ViewFrustum.PLANE_IDX.LEFT_PLANE_IDX, ViewFrustum.PLANE_IDX.TOP_PLANE_IDX, out m_Constants.FrustumRayLT);
                GetPlaneIntersection(ViewFrustum.PLANE_IDX.RIGHT_PLANE_IDX, ViewFrustum.PLANE_IDX.BOTTOM_PLANE_IDX, out m_Constants.FrustumRayRB);
                GetPlaneIntersection(ViewFrustum.PLANE_IDX.TOP_PLANE_IDX, ViewFrustum.PLANE_IDX.RIGHT_PLANE_IDX, out m_Constants.FrustumRayRT);

                m_Constants.CameraPos = new Vector4(CameraWorldPos.x, CameraWorldPos.y, CameraWorldPos.z, 1.0f);
                m_Constants.LightPos_0 = light1Pos * -1; //Need to invert going into the shader
                m_Constants.LightPos_1 = ligth2Pos * -1; //Need to invert going into the shader

                fixed (Constants* constantsPtr = &m_Constants)
                {
                    m_pImmediateContext.UpdateBuffer(m_ConstantsCB.Obj, 0, (uint)sizeof(Constants), new IntPtr(constantsPtr), RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
                }
            }

            //Trace rays
            {
                m_pRayTracingSRB.Obj.GetVariableByName(SHADER_TYPE.SHADER_TYPE_RAY_GEN, "g_ColorBuffer").Set(imageBlitter.TextureView);

                m_pImmediateContext.SetPipelineState(m_pRayTracingPSO.Obj);
                m_pImmediateContext.CommitShaderResources(m_pRayTracingSRB.Obj, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

                var Attribs = new TraceRaysAttribs();
                Attribs.DimensionX = imageBlitter.Width;
                Attribs.DimensionY = imageBlitter.Height;
                Attribs.pSBT = m_pSBT.Obj;

                m_pImmediateContext.TraceRays(Attribs);
            }

            // Blit to swapchain image
            imageBlitter.Blit();
        }

        public void AddTlasBuild(TLASBuildInstanceData instance)
        {
            instances.Add(instance);
        }

        public void RemoveTlasBuild(TLASBuildInstanceData instance)
        {
            instances.Remove(instance);
        }

        public void AddShaderTableBinder(ShaderTableBinder binder)
        {
            shaderTableBinders.Add(binder);
        }

        public void RemoveShaderTableBinder(ShaderTableBinder binder)
        {
            shaderTableBinders.Remove(binder);
        }

        public void AddShaderResourceBinder(ShaderResourceBinder binder)
        {
            rebuildPipeline = true;
            shaderResourceBinders.Add(binder);
        }

        public void RemoveShaderResourceBinder(ShaderResourceBinder binder)
        {
            shaderResourceBinders.Remove(binder);
        }

        private void BindShaders(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            foreach (var i in shaderTableBinders)
            {
                i(sbt, tlas);
            }
        }

        private void BindShaderResources(IShaderResourceBinding rayTracingSRB)
        {
            foreach (var i in shaderResourceBinders)
            {
                i(rayTracingSRB);
            }
        }
    }
}
