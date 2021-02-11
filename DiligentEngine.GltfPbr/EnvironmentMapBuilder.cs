using Engine.Resources;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{
    public class EnvironmentMapBuilder
    {
        private readonly TextureLoader textureLoader;
        private readonly IResourceProvider<EnvironmentMapBuilder> resourceProvider;

        public EnvironmentMapBuilder(TextureLoader textureLoader, IResourceProvider<EnvironmentMapBuilder> resourceProvider)
        {
            this.textureLoader = textureLoader;
            this.resourceProvider = resourceProvider;
        }

        private FreeImageBitmap OpenFreeImageBmp(String file)
        {
            using var stream = resourceProvider.openFile(file);
            return FreeImageBitmap.FromStream(stream);
        }

        public unsafe AutoPtr<ITextureView> BuildEnvMapView(IRenderDevice m_pDevice, IDeviceContext m_pImmediateContext, String baseName, String ext)
        {
            //For these cubemaps 
            //PosX, NegX, PosZ, NegZ are around the view - all aligned normally, no rotations
            //PosY is above head
            //PosZ is below head
            //Still load in normal order, but addressing must be different in shader.

            var InitData = new TextureData
            {
                pSubResources = new List<TextureSubResData>()
            };

            using var positiveXBmp = OpenFreeImageBmp($"{baseName}PositiveX.{ext}");
            textureLoader.FixBitmap(positiveXBmp);
            using var negativeXBmp = OpenFreeImageBmp($"{baseName}NegativeX.{ext}");
            textureLoader.FixBitmap(negativeXBmp);
            using var positiveYBmp = OpenFreeImageBmp($"{baseName}PositiveY.{ext}");
            textureLoader.FixBitmap(positiveYBmp);
            using var negativeYBmp = OpenFreeImageBmp($"{baseName}NegativeY.{ext}");
            textureLoader.FixBitmap(negativeYBmp);
            using var positiveZBmp = OpenFreeImageBmp($"{baseName}PositiveZ.{ext}");
            textureLoader.FixBitmap(positiveZBmp);
            using var negativeZBmp = OpenFreeImageBmp($"{baseName}NegativeZ.{ext}");
            textureLoader.FixBitmap(negativeZBmp);

            var TexDesc = new TextureDesc();
            TexDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_CUBE;
            TexDesc.Usage = USAGE.USAGE_IMMUTABLE;
            TexDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE;
            TexDesc.Depth = 6;
            TexDesc.Format = TEXTURE_FORMAT.TEX_FORMAT_BGRA8_UNORM;
            TexDesc.MipLevels = 1;
            TexDesc.Format = TEXTURE_FORMAT.TEX_FORMAT_BGRA8_UNORM;
            TexDesc.Width = (uint)positiveXBmp.Width;
            TexDesc.Height = (uint)positiveXBmp.Height;
            Int32 stride = -positiveXBmp.Stride;
            UInt32 uStride = (UInt32)(stride);

            var positiveX = new TextureSubResData { pData = positiveXBmp.Scan0 - (stride * (positiveXBmp.Height - 1)), Stride = uStride };
            InitData.pSubResources.Add(positiveX);

            var negativeX = new TextureSubResData { pData = negativeXBmp.Scan0 - (stride * (negativeXBmp.Height - 1)), Stride = uStride };
            InitData.pSubResources.Add(negativeX);

            var positiveY = new TextureSubResData { pData = positiveYBmp.Scan0 - (stride * (positiveYBmp.Height - 1)), Stride = uStride };
            InitData.pSubResources.Add(positiveY);

            var negativeY = new TextureSubResData { pData = negativeYBmp.Scan0 - (stride * (negativeYBmp.Height - 1)), Stride = uStride };
            InitData.pSubResources.Add(negativeY);

            var positiveZ = new TextureSubResData { pData = positiveZBmp.Scan0 - (stride * (positiveZBmp.Height - 1)), Stride = uStride };
            InitData.pSubResources.Add(positiveZ);

            var negativeZ = new TextureSubResData { pData = negativeZBmp.Scan0 - (stride * (negativeZBmp.Height - 1)), Stride = uStride };
            InitData.pSubResources.Add(negativeZ);

            using var EnvironmentMap = m_pDevice.CreateTexture(TexDesc, InitData);

            var textureViewPtr = new AutoPtr<ITextureView>(EnvironmentMap.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));

            var Barriers = new List<StateTransitionDesc>
            {
                new StateTransitionDesc{pResource = EnvironmentMap.Obj,           OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true}
            };
            m_pImmediateContext.TransitionResourceStates(Barriers);

            return textureViewPtr; //Caller takes ownership
        }
    }
}
