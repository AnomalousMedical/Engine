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

namespace DiligentEngine.GltfPbr
{
    [StructLayout(LayoutKind.Sequential)]
    struct GLTFShaderAttribs
    {
        /// <summary>
        /// These don't actually work as is, but they are the defaults from the c++ version.
        /// </summary>
        /// <returns></returns>
        public static GLTFShaderAttribs CreateDefault()
        {
            return new GLTFShaderAttribs()
            {
                BaseColorFactor = new float4(1, 1, 1, 1),
                EmissiveFactor = new float4(1, 1, 1, 1),
                SpecularFactor = new float4(1, 1, 1, 1),

                Workflow = (int)PbrWorkflow.PBR_WORKFLOW_METALL_ROUGH,
                BaseColorTextureUVSelector = -1,
                PhysicalDescriptorTextureUVSelector = -1,
                NormalTextureUVSelector = -1,

                OcclusionTextureUVSelector = -1,
                EmissiveTextureUVSelector = -1,
                BaseColorSlice = 0,
                PhysicalDescriptorSlice = 0,

                NormalSlice = 0,
                OcclusionSlice = 0,
                EmissiveSlice = 0,
                MetallicFactor = 1,

                RoughnessFactor = 1,
                AlphaMode = (int)GltfPbr.PbrAlphaMode.ALPHA_MODE_OPAQUE,
                AlphaMaskCutoff = 0.5f,
                //Dummy0,

                // When texture atlas is used, UV scale and bias applied to
                // each texture coordinate set
                BaseColorUVScaleBias = new float4(1, 1, 0, 0),
                PhysicalDescriptorUVScaleBias = new float4(1, 1, 0, 0),
                NormalMapUVScaleBias = new float4(1, 1, 0, 0),
                OcclusionUVScaleBias = new float4(1, 1, 0, 0),
                EmissiveUVScaleBias = new float4(1, 1, 0, 0),
            };
        }

        public float4 BaseColorFactor;
        public float4 EmissiveFactor;
        public float4 SpecularFactor;

        public int Workflow;
        public float BaseColorTextureUVSelector;
        public float PhysicalDescriptorTextureUVSelector;
        public float NormalTextureUVSelector;

        public float OcclusionTextureUVSelector;
        public float EmissiveTextureUVSelector;
        public float BaseColorSlice;
        public float PhysicalDescriptorSlice;

        public float NormalSlice;
        public float OcclusionSlice;
        public float EmissiveSlice;
        public float MetallicFactor;

        public float RoughnessFactor;
        public int AlphaMode;
        public float AlphaMaskCutoff;
        public float Dummy0;

        // When texture atlas is used, UV scale and bias applied to
        // each texture coordinate set
        public float4 BaseColorUVScaleBias;
        public float4 PhysicalDescriptorUVScaleBias;
        public float4 NormalMapUVScaleBias;
        public float4 OcclusionUVScaleBias;
        public float4 EmissiveUVScaleBias;
    }
}
