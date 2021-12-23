using System.Threading.Tasks;

namespace DiligentEngine.RT.Sprites
{
    interface ISpriteMaterialManager
    {
        Task<ISpriteMaterial> Checkout(SpriteMaterialDescription desc);
        public void TryReturn(ISpriteMaterial item);
        void Return(ISpriteMaterial item);
    }
}