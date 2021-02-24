using DiligentEngine;
using System.Threading.Tasks;

namespace SceneTest
{
    interface IMaterialSpriteBuilder
    {
        Task<AutoPtr<IShaderResourceBinding>> CreateSpriteAsync(MaterialSpriteBindingDescription desc);
    }
}