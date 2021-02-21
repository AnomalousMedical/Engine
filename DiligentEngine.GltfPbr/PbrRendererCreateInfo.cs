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
    public class PbrRendererCreateInfo
    {
        /// Render target format.
        public TEXTURE_FORMAT RTVFmt = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;

        /// Depth-buffer format.

        /// \note   If both RTV and DSV formats are TEX_FORMAT_UNKNOWN,
        ///         the renderer will not initialize PSO, uniform buffers and other
        ///         resources. It is expected that an application will use custom
        ///         render callback function.
        public TEXTURE_FORMAT DSVFmt = TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;

        /// Indicates if front face is CCW.
        public bool FrontCCW = false;

        /// Indicates if the renderer should allow debug views.
        /// Rendering with debug views disabled is more efficient.
        public bool AllowDebugView = false;

        /// Indicates whether to use IBL. Default: true.
        public bool UseIBL = true;

        /// Whether to use ambient occlusion texture.
        public bool UseAO = true;

        /// Whether to use emissive texture.
        public bool UseEmissive = true;

        /// When set to true, pipeline state will be compiled with immutable samplers.
        /// When set to false, samplers from the texture views will be used.
        public bool UseImmutableSamplers = true;

        /// Whether to use texture atlas (e.g. apply UV transforms when sampling textures).
        public bool UseTextureAtlas = false;

        /// Immutable sampler for color map texture.
        public SamplerDesc ColorMapImmutableSampler = new SamplerDesc();

        public SamplerDesc ColorMapImmutableSamplerSprite = new SamplerDesc()
        {
            MinFilter = FILTER_TYPE.FILTER_TYPE_POINT,
            MagFilter = FILTER_TYPE.FILTER_TYPE_POINT,
            MipFilter = FILTER_TYPE.FILTER_TYPE_POINT,
        };

        /// Immutable sampler for physical description map texture.
        public SamplerDesc PhysDescMapImmutableSampler = new SamplerDesc();

        /// Immutable sampler for normal map texture.
        public SamplerDesc NormalMapImmutableSampler = new SamplerDesc();

        /// Immutable sampler for AO texture.
        public SamplerDesc AOMapImmutableSampler = new SamplerDesc();

        /// Immutable sampler for emissive map texture.
        public SamplerDesc EmissiveMapImmutableSampler = new SamplerDesc();

        /// Maximum number of joints
        public Uint32 MaxJointCount = 64;
    }
}
