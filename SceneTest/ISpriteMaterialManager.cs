using System.Threading.Tasks;

namespace SceneTest
{
    interface ISpriteMaterialManager
    {
        Task<ISpriteMaterial> Checkout(MaterialSpriteBindingDescription desc);
        void Return(ISpriteMaterial item);
    }
}