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
    class Brick : IDisposable
    {
        public class Description : SceneObjectDesc
        {
            public String Texture { get; set; } = "cc0Textures/Bricks045_1K";

            public bool RenderShadow { get; set; } = true;

            public bool GetShadow { get; set; } = false;
        }

        private readonly SceneObjectManager sceneObjectManager;
        private readonly IBepuScene bepuScene;
        private readonly ICC0TextureManager textureManager;
        private IShaderResourceBinding matBinding;
        private SceneObject sceneObject;
        private StaticHandle staticHandle;
        private TypedIndex shapeIndex;
        private bool disposed;

        public Brick(
            SceneObjectManager sceneObjectManager,
            Cube cube,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            IBepuScene bepuScene,
            ICC0TextureManager textureManager,
            Description description)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.bepuScene = bepuScene;
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

            var shape = new Box(description.Scale.x, description.Scale.y, description.Scale.z); //Each one creates its own, try to load from resources
            shapeIndex = bepuScene.Simulation.Shapes.Add(shape);

            staticHandle = bepuScene.Simulation.Statics.Add(
                new StaticDescription(
                    new System.Numerics.Vector3(description.Translation.x, description.Translation.y, description.Translation.z),
                    new System.Numerics.Quaternion(description.Orientation.x, description.Orientation.y, description.Orientation.z, description.Orientation.w),
                    new CollidableDescription(shapeIndex, 0.1f)));

            IEnumerator<YieldAction> co()
            {
                //This will load 1 texture per brick
                yield return coroutine.Await(async () =>
                {
                    matBinding = await textureManager.Checkout(new CCOTextureBindingDescription(description.Texture, getShadow: description.GetShadow));
                    if (disposed)
                    {
                        textureManager.Return(matBinding);
                    }
                    else
                    {
                        sceneObject.shaderResourceBinding = matBinding;
                    }
                });

                if (!destructionRequest.DestructionRequested)
                {
                    this.sceneObjectManager.Add(sceneObject);
                }
            }
            coroutine.Run(co());
        }

        public void Dispose()
        {
            disposed = true;
            bepuScene.Simulation.Shapes.Remove(shapeIndex);
            bepuScene.Simulation.Statics.Remove(staticHandle);
            sceneObjectManager.Remove(sceneObject);
            textureManager.TryReturn(matBinding);
        }
    }
}
