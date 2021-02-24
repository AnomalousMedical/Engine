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
    class Floor : IDisposable
    {
        private readonly SceneObjectManager sceneObjectManager;
        private readonly ICC0TextureManager textureManager;
        private IShaderResourceBinding matBinding;
        private SceneObject sceneObject;

        public Floor(
            SceneObjectManager sceneObjectManager,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ICC0TextureManager textureManager)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.textureManager = textureManager;
            sceneObject = new SceneObject()
            {
                vertexBuffer = plane.VertexBuffer,
                skinVertexBuffer = plane.SkinVertexBuffer,
                indexBuffer = plane.IndexBuffer,
                numIndices = plane.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                position = new Vector3(0, -1, 0),
                orientation = Quaternion.shortestArcQuat(Vector3.Forward, Vector3.Down),
                scale = new Vector3(10, 10, 10),
                GetShadows = true,
            };

            IEnumerator<YieldAction> co()
            {
                //This will load 1 texture per brick
                yield return coroutine.Await(async () =>
                {
                    matBinding = await textureManager.Checkout("cc0Textures/Ground037_1K", true);
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
