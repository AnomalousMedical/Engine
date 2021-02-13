using FreeImageAPI;
using System.Collections.Generic;

namespace DiligentEngine.GltfPbr
{
    public interface ICC0MaterialTextureBuilder
    {
        CC0MaterialTextureBuffers CreateMaterialSet(FreeImageBitmap indexImage, int scale, Dictionary<uint, (string basePath, string ext)> materialIds);
    }
}