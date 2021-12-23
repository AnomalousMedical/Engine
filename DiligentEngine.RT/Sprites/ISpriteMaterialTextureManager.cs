using FreeImageAPI;
using System.Threading.Tasks;

namespace DiligentEngine.RT.Sprites
{
    interface ISpriteMaterialTextureManager
    {
        Task<SpriteMaterialTextures> Checkout(FreeImageBitmap image, SpriteMaterialTextureDescription desc);
        void Return(SpriteMaterialTextures binding);
    }
}