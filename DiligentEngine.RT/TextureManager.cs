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
    public class TextureManager : IDisposable
    {
        List<String> textureFiles = new List<String>
        {
            "ChristmasTreeOrnament007",
            "MetalPlates001",
            "Fabric021",
            "Wood049",
            "Ground042"
        };
        private readonly TextureLoader textureLoader;
        private readonly GraphicsEngine graphicsEngine;
        private readonly VirtualFileSystem virtualFileSystem;
        private readonly List<AutoPtr<ITexture>> pTex;

        public int NumTextures => textureFiles.Count;

        public List<IDeviceObject> TexSRVs { get; private set; }

        public List<IDeviceObject> TexNormalSRVs { get; private set; }

        public TextureManager(TextureLoader textureLoader, GraphicsEngine graphicsEngine, VirtualFileSystem virtualFileSystem)
        {
            this.textureLoader = textureLoader;
            this.graphicsEngine = graphicsEngine;
            this.virtualFileSystem = virtualFileSystem;

            pTex = new List<AutoPtr<ITexture>>(textureFiles.Count);
            LoadTextures();
        }

        private void LoadTextures()
        {
            var m_pImmediateContext = graphicsEngine.ImmediateContext;

            // Load textures

            TexSRVs = new List<IDeviceObject>(textureFiles.Count);
            TexNormalSRVs = new List<IDeviceObject>(textureFiles.Count);
            var Barriers = new List<StateTransitionDesc>(textureFiles.Count);

            for (int tex = 0; tex < textureFiles.Count; ++tex)
            {
                {
                    var textureFile = $"cc0Textures/{textureFiles[tex]}_1K_Color.jpg";

                    using var logoStream = virtualFileSystem.openStream(textureFile, FileMode.Open);
                    var color = textureLoader.LoadTexture(logoStream, $"Color {tex} Texture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, true);
                    pTex.Add(color);

                    // Get shader resource view from the texture
                    var pTextureSRV = color.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
                    TexSRVs.Add(pTextureSRV);
                    Barriers.Add(new StateTransitionDesc { pResource = color.Obj, OldState = RESOURCE_STATE.RESOURCE_STATE_UNKNOWN, NewState = RESOURCE_STATE.RESOURCE_STATE_SHADER_RESOURCE, UpdateResourceState = true });
                }

                {
                    var textureFile = $"cc0Textures/{textureFiles[tex]}_1K_Normal.jpg";

                    using var logoStream = virtualFileSystem.openStream(textureFile, FileMode.Open);
                    using var bmp = FreeImageBitmap.FromStream(logoStream);
                    bmp.ConvertColorDepth(FREE_IMAGE_COLOR_DEPTH.FICD_32_BPP); //Cheat and convert color depth
                    var normal = textureLoader.CreateTextureFromImage(bmp, 0, $"Normal {tex} Texture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, false); //SRGB breaks normal maps
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
            foreach (var i in pTex)
            {
                i.Dispose();
            }
        }
    }
}
