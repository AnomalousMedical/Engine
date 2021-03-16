using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin;
using DiligentEngine;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class BattleArena : IDisposable
    {
        public class Description : SceneObjectDesc
        {
            public String Texture { get; set; } = "cc0Textures/Bricks045_1K";

            public bool RenderShadow { get; set; } = false;

            public bool GetShadow { get; set; } = true;
        }

        private readonly SceneObjectManager<BattleManager> sceneObjectManager;
        private readonly IDestructionRequest destructionRequest;

        private readonly ICC0TextureManager textureManager;
        private IShaderResourceBinding matBinding;
        private SceneObject sceneObject;
        private bool disposed;

        public BattleArena(
            SceneObjectManager<BattleManager> sceneObjectManager,
            Cube cube,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ICC0TextureManager textureManager,
            Description description)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.destructionRequest = destructionRequest;
            this.textureManager = textureManager;
            sceneObject = new SceneObject()
            {
                vertexBuffer = cube.VertexBuffer,
                skinVertexBuffer = cube.SkinVertexBuffer,
                indexBuffer = cube.IndexBuffer,
                numIndices = cube.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                position = description.Translation,
                orientation = description.Orientation,
                scale = description.Scale,
                RenderShadow = description.RenderShadow,
                GetShadows = description.GetShadow
            };

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until coroutine is finished and this is disposed.

                matBinding = await textureManager.Checkout(new CCOTextureBindingDescription(description.Texture, getShadow: description.GetShadow));
                if (disposed)
                {
                    textureManager.Return(matBinding);
                    return; //Stop loading
                }

                if (!destructionRequest.DestructionRequested)
                {
                    sceneObject.shaderResourceBinding = matBinding;
                    this.sceneObjectManager.Add(sceneObject);
                }
            });
        }

        public void Dispose()
        {
            disposed = true;
            sceneObjectManager.Remove(sceneObject);
            textureManager.TryReturn(matBinding);
        }

        internal void RequestDestruction()
        {
            destructionRequest.RequestDestruction();
        }
    }
}
