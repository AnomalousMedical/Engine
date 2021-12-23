using FreeImageAPI;
using System.Threading.Tasks;

namespace DiligentEngine.RT.Sprites
{
    public interface ISpriteMaterialTextureManager
    {
        Task<SpriteMaterialTextures> Checkout(FreeImageBitmap image, SpriteMaterialTextureDescription desc);
        void Return(SpriteMaterialTextures binding);
    }
}