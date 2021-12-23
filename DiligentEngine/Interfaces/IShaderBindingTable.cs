using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using Engine;

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

namespace DiligentEngine
{
    /// <summary>
    /// Defines the methods to manipulate an SBT object
    /// </summary>
    public partial class IShaderBindingTable :  IDeviceObject
    {
        public IShaderBindingTable(IntPtr objPtr)
            : base(objPtr)
        {
            this._ConstructorCalled();
        }
        partial void _ConstructorCalled();
        /// <summary>
        /// Binds a ray-generation shader.
        /// \param [in] pShaderGroupName - Ray-generation shader name that was specified in RayTracingGeneralShaderGroup::Name
        /// when the pipeline state was created.
        /// \param [in] pData            - Shader record data, can be null.
        /// \param [in] DataSize         - Shader record data size, should be equal to RayTracingPipelineDesc::ShaderRecordSize.
        /// 
        /// \note Access to the SBT must be externally synchronized.
        /// </summary>
        public void BindRayGenShader(String pShaderGroupName, IntPtr pData, Uint32 DataSize)
        {
            IShaderBindingTable_BindRayGenShader(
                this.objPtr
                , pShaderGroupName
                , pData
                , DataSize
            );
        }
        /// <summary>
        /// Binds a ray-miss shader.
        /// \param [in] pShaderGroupName - Ray-miss shader name that was specified in RayTracingGeneralShaderGroup::Name
        /// when the pipeline state was created. Can be null to make the shader inactive.
        /// \param [in] MissIndex        - Miss shader offset in the shader binding table (aka ray type). This offset will
        /// correspond to 'MissShaderIndex' argument of TraceRay() function in HLSL,
        /// and 'missIndex' argument of traceRay() function in GLSL.
        /// \param [in] pData            - Shader record data, can be null.
        /// \param [in] DataSize         - Shader record data size, should be equal to RayTracingPipelineDesc::ShaderRecordSize.
        /// 
        /// \note Access to the SBT must be externally synchronized.
        /// </summary>
        public void BindMissShader(String pShaderGroupName, Uint32 MissIndex, IntPtr pData, Uint32 DataSize)
        {
            IShaderBindingTable_BindMissShader(
                this.objPtr
                , pShaderGroupName
                , MissIndex
                , pData
                , DataSize
            );
        }
        /// <summary>
        /// Binds a hit group for all geometries in the specified instance.
        /// \param [in] pTLAS                    - Top-level AS that contains the given instance.
        /// \param [in] pInstanceName            - Instance name, for which to bind the hit group. This is the name that was used
        /// when the TLAS was created, see TLASBuildInstanceData::InstanceName.
        /// \param [in] RayOffsetInHitGroupIndex - Ray offset in the shader binding table (aka ray type). This offset will
        /// correspond to 'RayContributionToHitGroupIndex' argument of TraceRay() function
        /// in HLSL, and 'sbtRecordOffset' argument of traceRay() function in GLSL.
        /// Must be less than HitShadersPerInstance.
        /// \param [in] pShaderGroupName         - Hit group name that was specified in RayTracingTriangleHitShaderGroup::Name or
        /// RayTracingProceduralHitShaderGroup::Name when the pipeline state was created.
        /// Can be null to make the shader group inactive.
        /// \param [in] pData                    - Shader record data, can be null.
        /// \param [in] DataSize                 - Shader record data size, should be equal to RayTracingPipelineDesc::ShaderRecordSize.
        /// 
        /// \note Access to the SBT must be externally synchronized.
        /// Access to the TLAS must be externally synchronized.
        /// </summary>
        public void BindHitGroupForInstance(ITopLevelAS pTLAS, String pInstanceName, Uint32 RayOffsetInHitGroupIndex, String pShaderGroupName, IntPtr pData, Uint32 DataSize)
        {
            IShaderBindingTable_BindHitGroupForInstance(
                this.objPtr
                , pTLAS.objPtr
                , pInstanceName
                , RayOffsetInHitGroupIndex
                , pShaderGroupName
                , pData
                , DataSize
            );
        }
        /// <summary>
        /// Binds a hit group for all instances in the given top-level AS.
        /// \param [in] pTLAS                    - Top-level AS, for which to bind the hit group.
        /// \param [in] RayOffsetInHitGroupIndex - Ray offset in the shader binding table (aka ray type). This offset will
        /// correspond to 'RayContributionToHitGroupIndex' argument of TraceRay()
        /// function in HLSL, and 'sbtRecordOffset' argument of traceRay() function in GLSL.
        /// Must be less than HitShadersPerInstance.
        /// \param [in] pShaderGroupName         - Hit group name that was specified in RayTracingTriangleHitShaderGroup::Name or
        /// RayTracingProceduralHitShaderGroup::Name when the pipeline state was created.
        /// Can be null to make the shader group inactive.
        /// \param [in] pData                    - Shader record data, can be null.
        /// \param [in] DataSize                 - Shader record data size, should be equal to RayTracingPipelineDesc::ShaderRecordSize.
        /// 
        /// \note Access to the SBT must be externally synchronized.
        /// Access to the TLAS must be externally synchronized.
        /// </summary>
        public void BindHitGroupForTLAS(ITopLevelAS pTLAS, Uint32 RayOffsetInHitGroupIndex, String pShaderGroupName, IntPtr pData, Uint32 DataSize)
        {
            IShaderBindingTable_BindHitGroupForTLAS(
                this.objPtr
                , pTLAS.objPtr
                , RayOffsetInHitGroupIndex
                , pShaderGroupName
                , pData
                , DataSize
            );
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IShaderBindingTable_BindRayGenShader(
            IntPtr objPtr
            , String pShaderGroupName
            , IntPtr pData
            , Uint32 DataSize
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IShaderBindingTable_BindMissShader(
            IntPtr objPtr
            , String pShaderGroupName
            , Uint32 MissIndex
            , IntPtr pData
            , Uint32 DataSize
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IShaderBindingTable_BindHitGroupForInstance(
            IntPtr objPtr
            , IntPtr pTLAS
            , String pInstanceName
            , Uint32 RayOffsetInHitGroupIndex
            , String pShaderGroupName
            , IntPtr pData
            , Uint32 DataSize
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IShaderBindingTable_BindHitGroupForTLAS(
            IntPtr objPtr
            , IntPtr pTLAS
            , Uint32 RayOffsetInHitGroupIndex
            , String pShaderGroupName
            , IntPtr pData
            , Uint32 DataSize
        );
    }
}
