using Engine;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    public class TextureSet : IDisposable
    {
        private List<AutoPtr<ITexture>> pTex;
        private readonly TextureLoader textureLoader;
        private readonly GraphicsEngine graphicsEngine;
        private readonly VirtualFileSystem virtualFileSystem;
        private int numTextures; //The number of textues in the set, multiple texture files may be loaded
        private bool hasOpacity;

        public int NumTextures => numTextures;

        public bool HasOpacity => hasOpacity;

        public List<IDeviceObject> TexSRVs { get; private set; }

        public List<IDeviceObject> TexNormalSRVs { get; private set; }

        public TextureSet(TextureLoader textureLoader, GraphicsEngine graphicsEngine, VirtualFileSystem virtualFileSystem)
        {
            this.textureLoader = textureLoader;
            this.graphicsEngine = graphicsEngine;
            this.virtualFileSystem = virtualFileSystem;
        }

        public void Setup(IEnumerable<String> textureFiles, int numTexturesHint = 1) 
        { 
            if(pTex != null)
            {
                throw new InvalidOperationException("Please call Setup only once per TextureSet.");
            }

            numTextures = 0;
            hasOpacity = false;

            var m_pImmediateContext = graphicsEngine.ImmediateContext;

            pTex = new List<AutoPtr<ITexture>>(numTexturesHint * 2);
            TexSRVs = new List<IDeviceObject>(numTexturesHint);
            TexNormalSRVs = new List<IDeviceObject>(numTexturesHint);
            var Barriers = new List<StateTransitionDesc>(numTexturesHint);

            // Load textures
            foreach (var texture in textureFiles)
            {
                ++numTextures;
                {
                    var textureFile = $"cc0Textures/{texture}_1K_Color.jpg";
                    var opacityFile = $"cc0Textures/{texture}_1K_Opacity.jpg";

                    using var logoStream = virtualFileSystem.openStream(textureFile, FileMode.Open);
                    using var bmp = FreeImageBitmap.FromStream(logoStream);
                    if (virtualFileSystem.exists(opacityFile))
                    {
                        hasOpacity = true;
                        //Jam opacity map into color alpha channel if it exists
                        bmp.ConvertColorDepth(FREE_IMAGE_COLOR_DEPTH.FICD_32_BPP); //Cheat and convert color depth
                        using var opacityStream = virtualFileSystem.openStream(opacityFile, FileMode.Open);
                        using var opacityBmp = FreeImageBitmap.FromStream(opacityStream);
                        opacityBmp.ConvertColorDepth(FREE_IMAGE_COLOR_DEPTH.FICD_08_BPP);
                        bmp.SetChannel(opacityBmp, FREE_IMAGE_COLOR_CHANNEL.FICC_ALPHA);
                    }
                    var color = textureLoader.CreateTextureFromImage(bmp, 0, $"Color {texture} Texture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, true);
                    pTex.Add(color);

                    // Get shader resource view from the texture
                    var pTextureSRV = color.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
                    TexSRVs.Add(pTextureSRV);
                    Barriers.Add(new StateTransitionDesc { pResource = color.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true });
                }

                {
                    var textureFile = $"cc0Textures/{texture}_1K_Normal.jpg";

                    using var logoStream = virtualFileSystem.openStream(textureFile, FileMode.Open);
                    using var bmp = FreeImageBitmap.FromStream(logoStream);
                    var normal = textureLoader.CreateTextureFromImage(bmp, 0, $"Normal {texture} Texture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, false); //SRGB breaks normal maps
                    pTex.Add(normal);

                    // Get shader resource view from the texture
                    var pTextureSRV = normal.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
                    TexNormalSRVs.Add(pTextureSRV);
                    Barriers.Add(new StateTransitionDesc { pResource = normal.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true });
                }
            }

            m_pImmediateContext.TransitionResourceStates(Barriers);
        }

        public void Dispose()
        {
            if (pTex != null)
            {
                foreach (var i in pTex)
                {
                    i.Dispose();
                }
            }
        }
    }
}
