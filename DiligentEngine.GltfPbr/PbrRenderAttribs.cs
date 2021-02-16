using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PbrRenderAttribs
    {
        public static PbrRenderAttribs CreateDefault()
        {
            return new PbrRenderAttribs
            {
                BaseColorFactor = new float4(1, 1, 1, 1),
                EmissiveFactor = new float4(1, 1, 1, 1),
                SpecularFactor = new float4(1, 1, 1, 1),

                Workflow = PbrWorkflow.PBR_WORKFLOW_METALL_ROUGH,
                BaseColorTextureUVSelector = 0,
                PhysicalDescriptorTextureUVSelector = 0,
                NormalTextureUVSelector = 0,

                OcclusionTextureUVSelector = 0,
                EmissiveTextureUVSelector = 0,
                BaseColorSlice = 0,
                PhysicalDescriptorSlice = 0,

                NormalSlice = 0,
                OcclusionSlice = 0,
                EmissiveSlice = 0,
                MetallicFactor = 1,

                RoughnessFactor = 1,
                AlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                AlphaMaskCutoff = 0.5f,
                Dummy0 = -107374176f, //Not sure what this value is, but its what was in the c++ version, might just be garbage

                // When texture atlas is used, UV scale and bias applied to
                // each texture coordinate set
                BaseColorUVScaleBias = new float4(1, 1, 0, 0),
                PhysicalDescriptorUVScaleBias = new float4(1, 1, 0, 0),
                NormalMapUVScaleBias = new float4(1, 1, 0, 0),
                OcclusionUVScaleBias = new float4(1, 1, 0, 0),
                EmissiveUVScaleBias = new float4(1, 1, 0, 0),


                //Material info

                AverageLogLum = 0.300000012f,
                MiddleGray = 0.180000007f,
                WhitePoint = 3.00000000f,
                IBLScale = 1.00000000f,
                DebugViewType = DebugViewType.None,
                OcclusionStrength = 1.00000000f,
                EmissionScale = 1.00000000f,
        };
        }

        //--Material Info

        public float4 BaseColorFactor;
        public float4 EmissiveFactor;
        public float4 SpecularFactor;

        public PbrWorkflow Workflow;
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
        public PbrAlphaMode AlphaMode;
        public float AlphaMaskCutoff;
        public float Dummy0;

        // When texture atlas is used, UV scale and bias applied to
        // each texture coordinate set
        public float4 BaseColorUVScaleBias;
        public float4 PhysicalDescriptorUVScaleBias;
        public float4 NormalMapUVScaleBias;
        public float4 OcclusionUVScaleBias;
        public float4 EmissiveUVScaleBias;

        //--RenderParameters

        public float AverageLogLum;
        public float MiddleGray;
        public float WhitePoint;

        public float IBLScale;
        public DebugViewType DebugViewType;
        public float OcclusionStrength;
        public float EmissionScale;

        public bool DoubleSided;
        public bool GetShadows;
    }
}
