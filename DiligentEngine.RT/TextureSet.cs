using DiligentEngine.RT.Resources;
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
        private List<CC0TextureResult> pTex;
        private readonly CC0TextureLoader textureLoader;
        private int numTextures; //The number of textues in the set, multiple texture files may be loaded
        private bool hasOpacity;

        public int NumTextures => numTextures;

        public bool HasOpacity => hasOpacity;

        public List<IDeviceObject> TexSRVs { get; private set; }

        public List<IDeviceObject> TexNormalSRVs { get; private set; }

        public List<IDeviceObject> TexPhysicalSRVs { get; private set; }

        public List<IDeviceObject> TexEmissiveSRVs { get; private set; }

        public TextureSet(CC0TextureLoader texturelLoader)
        {
            this.textureLoader = texturelLoader;
        }

        public async Task Setup(IEnumerable<String> textureFiles, int numTexturesHint = 1) 
        { 
            if(pTex != null)
            {
                throw new InvalidOperationException("Please call Setup only once per TextureSet.");
            }

            numTextures = 0;
            hasOpacity = false;

            pTex = new List<CC0TextureResult>(numTexturesHint);
            TexSRVs = new List<IDeviceObject>(numTexturesHint);
            TexNormalSRVs = new List<IDeviceObject>(numTexturesHint);
            TexPhysicalSRVs = new List<IDeviceObject>(numTexturesHint);
            TexEmissiveSRVs = new List<IDeviceObject>(numTexturesHint);

            // Load textures
            foreach (var texture in textureFiles)
            {
                ++numTextures;
                //TODO: This isn't very good, we really don't need this layer at all
                var result = await textureLoader.LoadTextureSet(texture);
                pTex.Add(result);
                TexSRVs.AddRange(result.BaseColorSRVs);
                TexNormalSRVs.AddRange(result.NormalMapSRVs);
                TexPhysicalSRVs.AddRange(result.PhysicalDescriptorMapSRVs);
                TexEmissiveSRVs.AddRange(result.EmissiveSRVs);
            }
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
