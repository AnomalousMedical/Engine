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

        public EnvironmentMapBuilder(TextureLoader textureLoader)
        {
            this.textureLoader = textureLoader;
        }

        public unsafe AutoPtr<ITextureView> BuildEnvMapView(IRenderDevice m_pDevice, IDeviceContext m_pImmediateContext)
        {
            //Freeimage
            //Cubemap array layout, what it should be
            //The papermill is different, still figuring this out
            //The faces are rotated diffrerently either because of gltf or maybe its wrong
            //https://docs.unrealengine.com/en-US/RenderingAndGraphics/Textures/Cubemaps/CreatingCubemaps/index.html
            //positive x, negative x, positive y, negative y, positive z, negative z
            //Or if lying on back looking at sky
            //left, right, head back (up), head forward (tword feet), forward, backward
            //But these need additional fixes to images when standing, to make them like your lying down
            //Positive X - Rotated 90 degrees CCW
            //Negative X -Rotated 90 degrees CW
            //Positive Y -Rotated 180 degrees
            //Negative Y - No Rotation
            //Positive Z - The side that must align with positive Y should be at the top
            //Negative Z -The side that must align with positive Y should be at the top

            var InitData = new TextureData
            {
                pSubResources = new List<TextureSubResData>()
            };

            using var positiveXBmp = FreeImageBitmap.FromFile("papermill/PositiveX.png");
            textureLoader.FixBitmap(positiveXBmp);
            using var negativeXBmp = FreeImageBitmap.FromFile("papermill/NegativeX.png");
            textureLoader.FixBitmap(negativeXBmp);
            using var positiveYBmp = FreeImageBitmap.FromFile("papermill/PositiveY.png");
            textureLoader.FixBitmap(positiveYBmp);
            using var negativeYBmp = FreeImageBitmap.FromFile("papermill/NegativeY.png");
            textureLoader.FixBitmap(negativeYBmp);
            using var positiveZBmp = FreeImageBitmap.FromFile("papermill/PositiveZ.png");
            textureLoader.FixBitmap(positiveZBmp);
            using var negativeZBmp = FreeImageBitmap.FromFile("papermill/NegativeZ.png");
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
