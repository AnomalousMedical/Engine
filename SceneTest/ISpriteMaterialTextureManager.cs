using FreeImageAPI;
using System.Threading.Tasks;

namespace SceneTest
{
    interface ISpriteMaterialTextureManager
    {
        Task<SpriteMaterialTextures> Checkout(FreeImageBitmap image, SpriteMaterialTextureDescription desc);
        void Return(SpriteMaterialTextures binding);
    }
}