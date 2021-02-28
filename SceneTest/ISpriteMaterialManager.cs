using System.Threading.Tasks;

namespace SceneTest
{
    interface ISpriteMaterialManager
    {
        Task<ISpriteMaterial> Checkout(SpriteMaterialDescription desc);
        public void TryReturn(ISpriteMaterial item);
        void Return(ISpriteMaterial item);
    }
}