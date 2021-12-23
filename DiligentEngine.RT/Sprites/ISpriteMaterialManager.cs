using System.Threading.Tasks;

namespace DiligentEngine.RT.Sprites
{
    public interface ISpriteMaterialManager
    {
        Task<SpriteMaterial> Checkout(SpriteMaterialDescription desc);
        public void TryReturn(SpriteMaterial item);
        void Return(SpriteMaterial item);
    }
}