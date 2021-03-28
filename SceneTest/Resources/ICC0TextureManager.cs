using DiligentEngine;
using System.Threading.Tasks;

namespace SceneTest
{
    interface ICC0TextureManager
    {
        /// <summary>
        /// Checkout a srb that will work for a given texture. When finished you must
        /// call return or the resource will leak.
        /// </summary>
        /// <param name="baseName"></param>
        /// <returns></returns>
        Task<IShaderResourceBinding> Checkout(CCOTextureBindingDescription desc);

        /// <summary>
        /// Return a srb to the texture manager. When all checked out instances are returned
        /// the srb is destroyed. This can safely be passed null and will check for it.
        /// </summary>
        /// <param name="binding"></param>
        void TryReturn(IShaderResourceBinding binding);

        /// <summary>
        /// Return a srb to the texture manager. When all checked out instances are returned
        /// the srb is destroyed.
        /// </summary>
        /// <param name="binding"></param>
        void Return(IShaderResourceBinding binding);
    }
}