using System.Threading.Tasks;

namespace SceneTest
{
    interface IMaterialSpriteBuilder
    {
        Task<ISpriteMaterial> CreateSpriteMaterialAsync(MaterialSpriteBindingDescription desc);
    }
}