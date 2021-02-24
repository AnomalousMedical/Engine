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
    class Brick : IDisposable
    {
        private readonly SceneObjectManager sceneObjectManager;
        private readonly ICC0TextureManager textureManager;
        private IShaderResourceBinding matBinding;
        private SceneObject sceneObject;

        public Brick(
            SceneObjectManager sceneObjectManager,
            DoubleSizeCube cube,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ICC0TextureManager textureManager)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.textureManager = textureManager;
            sceneObject = new SceneObject()
            {
                vertexBuffer = cube.VertexBuffer,
                skinVertexBuffer = cube.SkinVertexBuffer,
                indexBuffer = cube.IndexBuffer,
                numIndices = cube.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                position = Vector3.Zero,
                orientation = Quaternion.Identity,
                scale = Vector3.ScaleIdentity,
                RenderShadow = true
            };

            IEnumerator<YieldAction> co()
            {
                //This will load 1 texture per brick
                yield return coroutine.Await(async () =>
                {
                    matBinding = await textureManager.Checkout(new CCOTextureBindingDescription("cc0Textures/Bricks045_1K"));
                });

                sceneObject.shaderResourceBinding = matBinding;

                this.sceneObjectManager.Add(sceneObject);
            }
            coroutine.Run(co());
        }

        public void SetLocation(Vector3 position, Quaternion orientation)
        {
            this.sceneObject.position = position;
            this.sceneObject.orientation = orientation;
        }

        public void Dispose()
        {
            sceneObjectManager.Remove(sceneObject);
            textureManager.Return(matBinding);
        }
    }
}
