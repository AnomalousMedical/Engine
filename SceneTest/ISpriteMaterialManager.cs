using System.Threading.Tasks;

namespace SceneTest
{
    interface ISpriteMaterialManager
    {
        Task<ISpriteMaterial> Checkout(SpriteMaterialDescription desc);
        void Return(ISpriteMaterial item);
    }
}